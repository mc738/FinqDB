namespace FinqDB.Store.V1

module Categories =
    
    open Freql.Sqlite
    open FinqDB.Store.V1.Persistence

    let add (ctx: SqliteContext) (category: string) =
        ctx.ExecuteInTransactionV2(fun t ->
            match Operations.selectCategoryRecord t [ "WHERE name = @0" ] [ category ] with
            | Some _ -> Error $"Category `{category}` already exists."
            | None -> ({ Name = category }: Parameters.NewCategory) |> Operations.insertCategory t |> Ok)

    let getAll (ctx: SqliteContext) = Operations.selectCategoryRecords ctx [] []
    

