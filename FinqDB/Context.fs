namespace FinqDB

open Freql.Sqlite
open Microsoft.FSharp.Core

type FinqDBContext(ctx: SqliteContext) =
    
    
    static member TryCreate(path: string) =
        Store.V1.Utils.create path "" "" |> Result.map FinqDBContext
         
    static member Create(path: string) =
        match FinqDBContext.TryCreate path with
        | Ok r -> r
        | Error e -> failwith $"Failed to create FinqDB context. Error: `{e}`"
