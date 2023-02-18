namespace FinqDB.Store.V1


[<RequireQualifiedAccess>]
module Labels =

    open Freql.Sqlite
    open FinqDB.Store.V1.Persistence

    let add (ctx: SqliteContext) (label: string) =
        ctx.ExecuteInTransactionV2(fun t ->
            match Operations.selectLabelRecord t [ "WHERE name = @0" ] [ label ] with
            | Some _ -> Error $"Label `{label}` already exists."
            | None -> ({ Name = label }: Parameters.NewLabel) |> Operations.insertLabel t |> Ok)

    let getAll (ctx: SqliteContext) = Operations.selectLabelRecords ctx [] []
