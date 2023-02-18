namespace FinqDB.Store.V1

open System.Text.Json

[<RequireQualifiedAccess>]
module Utils =

    open System
    open System.IO
    open Freql.Sqlite
    open FinqDB.Common.V1.Domain
    open FinqDB.Store.V1.Persistence

    let createId _ = Guid.NewGuid().ToString("n")

    let getTimestamp _ = DateTime.UtcNow

    let initialize (ctx: SqliteContext) =
        [ Records.FinqInfoItem.CreateTableSql()
          Records.MetadataItem.CreateTableSql()
          Records.FileType.CreateTableSql()
          Records.EncryptionType.CreateTableSql()
          Records.CompressionType.CreateTableSql()
          Records.Label.CreateTableSql()
          Records.Category.CreateTableSql()
          Records.Node.CreateTableSql()
          Records.NodeProperties.CreateTableSql()
          Records.NodeLabel.CreateTableSql()
          Records.Edge.CreateTableSql()
          Records.EdgeProperties.CreateTableSql()
          Records.ExternalEdge.CreateTableSql()
          Records.ExternalEdgeProperties.CreateTableSql()
          Records.File.CreateTableSql()
          Records.FileVersion.CreateTableSql()
          Records.Document.CreateTableSql()
          Records.DocumentVersion.CreateTableSql()
          Records.NodeFile.CreateTableSql()
          Records.NodeDocument.CreateTableSql()
          Records.NodeMetadataItem.CreateTableSql()
          Records.EdgeFile.CreateTableSql()
          Records.EdgeDocument.CreateTableSql()
          Records.EdgeMetadataItem.CreateTableSql()
          Records.EdgeWeight.CreateTableSql()
          Records.ExternalEdgeFile.CreateTableSql()
          Records.ExternalEdgeDocument.CreateTableSql()
          Records.ExternalEdgeMetadataItem.CreateTableSql()
          Records.FileMetadataItem.CreateTableSql()
          Records.FileVersionMetadataItem.CreateTableSql()
          Records.DocumentMetadataItem.CreateTableSql()
          Records.DocumentVersionMetadataItem.CreateTableSql() ]
        |> List.iter (ctx.ExecuteSqlNonQuery >> ignore)

    let seed (ctx: SqliteContext) (name: string) (description: string) =
        ({ Id = createId ()
           Name = name
           Description = description
           IsReadOnly = false
           Version = 1
           CreatedOn = getTimestamp () }: Parameters.NewFinqInfoItem)
        |> Operations.insertFinqInfoItem ctx

        ({ Name = "general" }: Parameters.NewCategory) |> Operations.insertCategory ctx

        FileType.All()
        |> List.map (fun ft ->
            ({ Name = ft.Serialize()
               Extension = ft.GetExtension()
               ContentType = ft.GetContentType() }: Parameters.NewFileType))
        |> List.iter (Operations.insertFileType ctx)

        EncryptionType.All()
        |> List.map (fun et -> ({ Name = et.Serialize() }: Parameters.NewEncryptionType))
        |> List.iter (Operations.insertEncryptionType ctx)

        CompressionType.All()
        |> List.map (fun ct -> ({ Name = ct.Serialize() }: Parameters.NewCompressionType))
        |> List.iter (Operations.insertCompressionType ctx)

    let create (path: string) (name: string) (description: string) =
        match File.Exists path with
        | true -> SqliteContext.Open path |> Ok
        | false ->
            let ctx = SqliteContext.Create path

            ctx.ExecuteInTransaction(fun t ->
                initialize t
                seed t name description)
            |> Result.map (fun _ -> ctx)

    let trySerializeJson<'T> (obj: 'T) =
        try
            JsonSerializer.Serialize obj |> Ok
        with exn ->
            Error $"Failed to deserialize json. Error: {exn.Message}"
