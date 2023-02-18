namespace FinqDB.Store.V1


[<RequireQualifiedAccess>]
module Edges =

    open System.IO
    open Freql.Core.Common.Types
    open Freql.Sqlite
    open FinqDB.Common
    open FinqDB.Store.V1.Persistence

    let add
        (ctx: SqliteContext)
        (id: string)
        (name: string)
        (fromNodeId: string)
        (toNodeId: string)
        (bidirectional: bool)
        =
        ctx.ExecuteInTransaction(fun t ->
            ({ Id = id
               Name = name
               FromNode = fromNodeId
               ToNode = toNodeId
               Bidirectional = bidirectional
               CreatedOn = Utils.getTimestamp () }: Parameters.NewEdge)
            |> Operations.insertEdge t)

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectEdgeRecord ctx [ "WHERE id = @0" ] [ id ]

    let getByFromNode (ctx: SqliteContext) (fromNodeId: string) =
        Operations.selectEdgeRecords ctx [ "WHERE from_node = @0;" ] [ fromNodeId ]

    let getByToNode (ctx: SqliteContext) (toNodeId: string) =
        Operations.selectEdgeRecords ctx [ "WHERE to_node = @0;" ] [ toNodeId ]

    let getConnectingNodes (ctx: SqliteContext) (fromNodeId: string) (toNodeId: string) =
        Operations.selectEdgeRecords ctx [ "WHERE from_node = @0 AND to_node = @1" ] [ fromNodeId; toNodeId ]

    let addOrUpdateMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectEdgeMetadataItemRecord t [ "WHERE edge_id = @0 AND item_key = @1" ] [ id; key ] with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE edge_metadata SET item_value = @0 WHERE edge_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ EdgeId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewEdgeMetadataItem)
                |> Operations.insertEdgeMetadataItem t)

    let getMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectEdgeMetadataItemRecords ctx [ "WHERE edge_id = @0" ] [ id ]

    let getLatestPropertyVersion (ctx: SqliteContext) (id: string) =
        ctx.Bespoke(
            "SELECT version FROM edge_properties WHERE edge_id = @0 ORDER BY version DESC LIMIT 1;",
            [ id ],
            fun reader ->
                [ while reader.Read() do
                      reader.GetInt32(0) ]
        )
        |> List.tryHead
        |> Option.defaultValue 0

    let getProperties (ctx: SqliteContext) (id: string) =
        Operations.selectEdgePropertiesRecord ctx [ "WHERE edge_id = @0 ORDER BY version DESC LIMIT 1;" ] [ id ]

    let addProperties (ctx: SqliteContext) (id: string) (raw: MemoryStream) =
        ctx.ExecuteInTransaction(fun t ->
            let hash = raw.GetSHA256Hash()

            ({ EdgeId = id
               Version = (getLatestPropertyVersion t id) + 1
               JsonBlob = BlobField.FromBytes raw
               Hash = hash
               CreatedOn = Utils.getTimestamp () }: Parameters.NewEdgeProperties)
            |> Operations.insertEdgeProperties t)
    
    let addJsonProperties<'T> (ctx: SqliteContext) (id: string) (properties: 'T) =
        Utils.trySerializeJson properties
        |> Result.bind (fun json ->
            use ms = new MemoryStream(json.ToUtf8Bytes())
            addProperties ctx id ms)
    
    let addEdgeWeight (ctx: SqliteContext) (id: string) (category: string) (weight: decimal) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectCategoryRecord t [ "WHERE name = @0;" ] [ category ] with
            | Some _ -> ()
            | None -> ({ Name = category }: Parameters.NewCategory) |> Operations.insertCategory t

            ({ EdgeId = id
               Category = category
               Weight = weight }: Parameters.NewEdgeWeight)
            |> Operations.insertEdgeWeight t)

    let getByCategory (ctx: SqliteContext) (category: string) = 
        Operations.selectEdgeRecords
            ctx
            [ "JOIN edge_weights ew ON edges.id = ew.edge_id"
              "WHERE ew.category = @0;" ]
            [ category ]
            
    let getByCategoryAndFromNode (ctx: SqliteContext) (category: string) (fromNode: string) =
        Operations.selectEdgeRecords
            ctx
            [ "JOIN edge_weights ew ON edges.id = ew.edge_id"
              "WHERE ew.category = @0 AND edges.from_node = @1;" ]
            [ category; fromNode; ]
            
    let getByCategoryAndToNode (ctx: SqliteContext) (category: string) (toNode: string) =
        Operations.selectEdgeRecords
            ctx
            [ "JOIN edge_weights ew ON edges.id = ew.edge_id"
              "WHERE ew.category = @0 AND edges.to_node = @1;" ]
            [ category; toNode; ]