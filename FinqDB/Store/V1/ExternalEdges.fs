namespace FinqDB.Store.V1

[<RequireQualifiedAccess>]
module ExternalEdges =

    open System.IO
    open Freql.Core.Common.Types
    open Freql.Sqlite
    open FinqDB.Common
    open FinqDB.Store.V1.Persistence

    let add (ctx: SqliteContext) (id: string) (name: string) (nodeId: string) (bidirectional: bool) =
        ctx.ExecuteInTransaction(fun t ->
            ({ Id = id
               Name = name
               NodeId = nodeId
               Bidirectional = bidirectional
               CreatedOn = Utils.getTimestamp () }: Parameters.NewExternalEdge)
            |> Operations.insertExternalEdge t)

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectExternalEdgeRecord ctx [ "WHERE id = @0" ] [ id ]

    let getByNodeId (ctx: SqliteContext) (fromNodeId: string) =
        Operations.selectExternalEdgeRecords ctx [ "WHERE node_id = @0;" ] [ fromNodeId ]

    let addOrUpdateMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match
                Operations.selectExternalEdgeMetadataItemRecord
                    t
                    [ "WHERE external_edge_id = @0 AND item_key = @1" ]
                    [ id; key ]
            with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE external_edge_metadata SET item_value = @0 WHERE external_edge_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ ExternalEdgeId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewExternalEdgeMetadataItem)
                |> Operations.insertExternalEdgeMetadataItem t)

    let getMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectExternalEdgeMetadataItemRecords ctx [ "WHERE external_edge_id = @0" ] [ id ]

    let getLatestPropertyVersion (ctx: SqliteContext) (id: string) =
        ctx.Bespoke(
            "SELECT version FROM external_edge_properties WHERE external_edge_id = @0 ORDER BY version DESC LIMIT 1;",
            [ id ],
            fun reader ->
                [ while reader.Read() do
                      reader.GetInt32(0) ]
        )
        |> List.tryHead
        |> Option.defaultValue 0

    let getProperties (ctx: SqliteContext) (id: string) =
        Operations.selectExternalEdgePropertiesRecord
            ctx
            [ "WHERE external_edge_id = @0 ORDER BY version DESC LIMIT 1;" ]
            [ id ]

    let addProperties (ctx: SqliteContext) (id: string) (raw: MemoryStream) =
        ctx.ExecuteInTransaction(fun t ->
            let hash = raw.GetSHA256Hash()

            ({ ExternalEdgeId = id
               Version = (getLatestPropertyVersion t id) + 1
               JsonBlob = BlobField.FromBytes raw
               Hash = hash
               CreatedOn = Utils.getTimestamp () }: Parameters.NewExternalEdgeProperties)
            |> Operations.insertExternalEdgeProperties t)

    let addJsonProperties<'T> (ctx: SqliteContext) (id: string) (properties: 'T) =
        Utils.trySerializeJson properties
        |> Result.bind (fun json ->
            use ms = new MemoryStream(json.ToUtf8Bytes())
            addProperties ctx id ms)
