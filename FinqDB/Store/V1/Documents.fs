namespace FinqDB.Store.V1

[<RequireQualifiedAccess>]
module Documents =
       
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
        (stream: MemoryStream)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        (documentType: string)
        =
        ctx.ExecuteInTransaction(fun t ->
            let documentId = Utils.createId ()
            let timestamp = Utils.getTimestamp ()

            ({ Id = documentId
               Name = name
               CreatedOn = timestamp }: Parameters.NewDocument)
            |> Operations.insertDocument t

            let hash = stream.GetSHA256Hash()

            ({ Id = Utils.createId ()
               DocumentId = documentId
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
               DocumentType = documentType
               CreatedOn = timestamp }: Parameters.NewDocumentVersion)
            |> Operations.insertDocumentVersion t)

    let getLatestVersionNumber (ctx: SqliteContext) (documentId: string) =
        ctx.Bespoke(
            "SELECT version FROM document_versions WHERE document_id = @0 ORDER BY version DESC LIMIT 1;",
            [ documentId ],
            fun reader ->
                [ while reader.Read() do
                      reader.GetInt32(0) ]
        )
        |> List.tryHead

    let addVersion
        (ctx: SqliteContext)
        (documentId: string)
        (stream: MemoryStream)
        (encryptionType: EncryptionType)
        (compressionType: CompressionType)
        (documentType: string)
        =
        ctx.ExecuteInTransactionV2(fun t ->
            // ASSUMPTION - if getLatestVersionNumber returns none the document does not exist.
            match getLatestVersionNumber t documentType with
            | Some version ->

                let hash = stream.GetSHA256Hash()

                ({ Id = Utils.createId ()
                   DocumentId = documentId
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
                   DocumentType = documentType
                   CreatedOn = Utils.getTimestamp () }: Parameters.NewDocumentVersion)
                |> Operations.insertDocumentVersion t

                Ok()
            | None -> Error $"Document `{documentId}` not found.")

    let addOrUpdateMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectDocumentMetadataItemRecord t [ "WHERE document_id = @0 AND item_key = @1" ] [ id; key ] with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE document_metadata SET item_value = @0 WHERE document_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ DocumentId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewDocumentMetadataItem)
                |> Operations.insertDocumentMetadataItem t)

    let getMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectDocumentMetadataItemRecords ctx [ "WHERE document_id = @0" ] [ id ]

    let addOrUpdateVersionMetadataValue (ctx: SqliteContext) (id: string) (key: string) (value: string) =
        ctx.ExecuteInTransaction(fun t ->
            match Operations.selectDocumentVersionMetadataItemRecord t [ "WHERE document_version_id = @0 AND item_key = @1" ] [ id; key ] with
            | Some _ ->
                t.ExecuteVerbatimNonQueryAnon(
                    "UPDATE document_version_metadata SET item_value = @0 WHERE document_version_id = @1 AND item_key = @2 ",
                    [ value; id; key ]
                )
                |> ignore
            | None ->
                ({ DocumentVersionId = id
                   ItemKey = key
                   ItemValue = value }: Parameters.NewDocumentVersionMetadataItem)
                |> Operations.insertDocumentVersionMetadataItem t)

    let getVersionMetadata (ctx: SqliteContext) (id: string) =
        Operations.selectDocumentVersionMetadataItemRecords ctx [ "WHERE document_version_id = @0" ] [ id ]

