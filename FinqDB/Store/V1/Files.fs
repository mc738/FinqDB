namespace FinqDB.Store.V1

[<RequireQualifiedAccess>]
module Files =
        
    open System.IO
    open Freql.Core.Common.Types
    open Freql.Core
    open Freql.Sqlite
    open FinqDB.Common
    open FinqDB.Common.V1.Domain
    open FinqDB.Store.V1.Persistence

    let add
        (ctx: SqliteContext)
        (name: string)
        (fileType: FileType)
        (stream: MemoryStream)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        =
        ctx.ExecuteInTransaction(fun t ->
            let fileId = Utils.createId ()
            let timestamp = Utils.getTimestamp ()

            ({ Id = fileId
               Name = name
               FileType = fileType.Serialize()
               CreatedOn = timestamp }: Parameters.NewFile)
            |> Operations.insertFile t

            let hash = stream.GetSHA256Hash()

            ({ Id = Utils.createId ()
               FileId = fileId
               Version = 1
               RawBlob =
                 // Compress and then encrypt file
                 // NOTE - It is probably best not to compress AND encrypt the stream
                 // https://crypto.stackexchange.com/questions/33737/is-it-better-to-encrypt-before-compression-or-vice-versa
                 stream
                 |> (V1.Compression.compressMemoryStream compressionType)
                 |> (V1.Encryption.encryptMemoryStream encryptionType)
                 |> BlobField.FromBytes
               Hash = hash
               EncryptionType = encryptionType.Serialize()
               CompressionType = compressionType.Serialize()
               CreatedOn = timestamp }: Parameters.NewFileVersion)
            |> Operations.insertFileVersion t)

    let getLatestVersionNumber (ctx: SqliteContext) (fileId: string) =
        ctx.Bespoke(
            "SELECT version FROM file_versions WHERE file_id = @0 ORDER BY version DESC LIMIT 1;",
            [ fileId ],
            fun reader ->
                [ while reader.Read() do
                      reader.GetInt32(0) ]
        )
        |> List.tryHead

    let addVersion
        (ctx: SqliteContext)
        (fileId: string)
        (stream: MemoryStream)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        =
        ctx.ExecuteInTransactionV2(fun t ->
            // ASSUMPTION - if getLatestVersionNumber returns none the file does not exist.
            match getLatestVersionNumber t fileId with
            | Some version ->

                let hash = stream.GetSHA256Hash()

                ({ Id = Utils.createId ()
                   FileId = fileId
                   Version = version + 1
                   RawBlob =
                     // Compress and then encrypt file
                     // NOTE - It is probably best not to compress AND encrypt the stream
                     // https://crypto.stackexchange.com/questions/33737/is-it-better-to-encrypt-before-compression-or-vice-versa
                     stream
                     |> (V1.Compression.compressMemoryStream compressionType)
                     |> (V1.Encryption.encryptMemoryStream encryptionType)
                     |> BlobField.FromBytes
                   Hash = hash
                   EncryptionType = encryptionType.Serialize()
                   CompressionType = compressionType.Serialize()
                   CreatedOn = Utils.getTimestamp () }: Parameters.NewFileVersion)
                |> Operations.insertFileVersion t

                Ok()
            | None -> Error $"File `{fileId}` not found.")

    let addOrUpdateMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectFileMetadataItemRecord t [ "WHERE file_id = @0 AND item_key = @1" ] [ id; key ] with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE file_metadata SET item_value = @0 WHERE edge_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ ExternalEdgeId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewExternalEdgeMetadataItem)
                |> Operations.insertExternalEdgeMetadataItem t)

    let getMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectFileMetadataItemRecords ctx [ "WHERE file_id = @0" ] [ id ]
    
    let addOrUpdateVersionMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectFileVersionMetadataItemRecord t [ "WHERE file_version_id = @0 AND item_key = @1" ] [ id; key ] with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE file_version_metadata SET item_value = @0 WHERE file_version_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ FileVersionId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewFileVersionMetadataItem)
                |> Operations.insertFileVersionMetadataItem t)

    let getVersionMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectFileVersionMetadataItemRecords ctx [ "WHERE file_version_id = @0" ] [ id ]