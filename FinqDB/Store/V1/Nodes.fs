namespace FinqDB.Store.V1

open System.IO

[<RequireQualifiedAccess>]
module Nodes =

    open System.IO
    open Freql.Core.Common.Types
    open Freql.Sqlite
    open FinqDB.Common
    open FinqDB.Store.V1.Persistence

    let add (ctx: SqliteContext) (id: string) (name: string) =
        ctx.ExecuteInTransaction(fun t ->
            ({ Id = id
               Name = name
               CreatedOn = Utils.getTimestamp () }: Parameters.NewNode)
            |> Operations.insertNode t)

    let get (ctx: SqliteContext) (id: string) =
        Operations.selectNodeRecord ctx [ "WHERE id = @0;" ] [ id ]

    let addOrUpdateMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectNodeMetadataItemRecord t [ "WHERE node_id = @0 AND item_key = @1" ] [ id; key ] with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE node_metadata SET item_value = @0 WHERE node_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ NodeId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewNodeMetadataItem)
                |> Operations.insertNodeMetadataItem t)

    let getMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectNodeMetadataItemRecords ctx [ "WHERE node_id = @0" ] [ id ]

    let getLatestPropertyVersion (ctx: SqliteContext) (id: string) =
        ctx.Bespoke(
            "SELECT version FROM node_properties WHERE node_id = @0 ORDER BY version DESC LIMIT 1;",
            [ id ],
            fun reader ->
                [ while reader.Read() do
                      reader.GetInt32(0) ]
        )
        |> List.tryHead
        |> Option.defaultValue 0

    let getProperties (ctx: SqliteContext) (id: string) =
        Operations.selectNodePropertiesRecord ctx [ "WHERE node_id = @0 ORDER BY version DESC LIMIT 1;" ] [ id ]

    let addProperties (ctx: SqliteContext) (id: string) (raw: MemoryStream) =
        ctx.ExecuteInTransaction(fun t ->
            let hash = raw.GetSHA256Hash()

            ({ NodeId = id
               Version = (getLatestPropertyVersion t id) + 1
               JsonBlob = BlobField.FromBytes raw
               Hash = hash
               CreatedOn = Utils.getTimestamp () }: Parameters.NewNodeProperties)
            |> Operations.insertNodeProperties t)

    let addJsonProperties<'T> (ctx: SqliteContext) (id: string) (properties: 'T) =
        Utils.trySerializeJson properties
        |> Result.bind (fun json ->
            use ms = new MemoryStream(json.ToUtf8Bytes())
            addProperties ctx id ms)

    let addNodeLabel (ctx: SqliteContext) (id: string) (label: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectLabelRecord t [ "WHERE name = @0;" ] [ label ] with
            | Some _ -> ()
            | None -> ({ Name = label }: Parameters.NewLabel) |> Operations.insertLabel t

            ({ NodeId = id; Label = label }: Parameters.NewNodeLabel)
            |> Operations.insertNodeLabel t)

    let getByLabel (ctx: SqliteContext) (label: string) =
        Operations.selectNodeRecords
            ctx
            [ "JOIN node_labels nl ON nodes.id = nl.node_id"; "WHERE nl.label = @0;" ]
            [ label ]

    let getByIdAndLabel (ctx: SqliteContext) (id: string) (label: string) =
        Operations.selectNodeRecord
            ctx
            [ "JOIN node_labels nl ON nodes.id = nl.node_id"; "WHERE nodes.id = @0 AND nl.label = @1;" ]
            [ id; label ]