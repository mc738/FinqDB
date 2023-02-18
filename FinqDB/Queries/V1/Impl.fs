namespace FinqDB.Queries.V1

open System.Reflection.Emit
open FinqDB.Common.V1.Domain
open FinqDB.Queries.V1.Parsing
open FinqDB.Store.V1
open FinqDB.Store.V1.Persistence
open Freql.Sqlite

type ExecutionType = | Stages

type Query =
    { Keyword: Keyword
      Root: NodeQuery }

    static member Create(raw: RawQuery) =
        { Keyword = Keyword.Deserialize raw.Keyword
          Root = NodeQuery.Create raw.Node }

    member private q.ExecuteFetch(ctx: SqliteContext) =
        let rec traverse (fromNodeId: string) (curr: EdgeQuery) =
            match curr.RelationshipDirection with
            | RelationshipDirection.Forwards -> curr.ExecuteFetch(ctx, fromNodeId = fromNodeId)
            | RelationshipDirection.Backwards ->
                // TODO need to implement
                failwith "Not implemented yet"
            | RelationshipDirection.Either ->
                // TODO need to implement
                failwith "Not implemented yet"
            | RelationshipDirection.Neither -> curr.ExecuteFetch(ctx)
            |> List.choose (fun e ->
                match curr.Node.Label with
                | Some l -> Nodes.getByIdAndLabel ctx e.ToNode l
                | None -> Nodes.get ctx e.ToNode
                |> Option.map (fun n -> e, n))
            |> List.map (fun (e, n) ->

                ({ Id = e.Id
                   ToNode =
                     { Id = n.Id
                       Name = n.Name
                       Edges = curr.Node.Edge |> Option.map (traverse fromNodeId)
                       CreatedOn = n.CreatedOn
                       Metadata =
                         Nodes.getMetadata ctx n.Id
                         |> List.map (fun mdi -> mdi.ItemKey, mdi.ItemValue)
                         |> Map.ofList
                       Files = []
                       Documents = [] }
                   Metadata =
                     Edges.getMetadata ctx e.Id
                     |> List.map (fun mdi -> mdi.ItemKey, mdi.ItemValue)
                     |> Map.ofList
                   Files = []
                   Documents = [] }: NodeEdge))

        // Get root node(s)
        q.Root.ExecuteFetch(ctx)
        |> List.choose (fun n ->
            match q.Root.Edge with
            | Some e ->
                let edges = traverse n.Id e

                // Discard any that have not associated edges. These do not have relationships.
                match edges.IsEmpty with
                | true -> None
                | false ->
                    ({ Id = n.Id
                       Name = n.Name
                       Edges = Some edges
                       CreatedOn = n.CreatedOn
                       Metadata =
                         Nodes.getMetadata ctx n.Id
                         |> List.map (fun mdi -> mdi.ItemKey, mdi.ItemValue)
                         |> Map.ofList
                       Files = []
                       Documents = [] }: Node)
                    |> Some
            | None ->
                ({ Id = n.Id
                   Name = n.Name
                   Edges = None
                   CreatedOn = n.CreatedOn
                   Metadata =
                     Nodes.getMetadata ctx n.Id
                     |> List.map (fun mdi -> mdi.ItemKey, mdi.ItemValue)
                     |> Map.ofList
                   Files = []
                   Documents = [] }: Node)
                |> Some)
        |> QueryResult.Matched

    member private q.ExecuteCreate(ctx: SqliteContext) =

        QueryResult.Created

    member q.Execute(ctx: SqliteContext) =
        match q.Keyword with
        | Keyword.Match -> q.ExecuteFetch ctx
        | Keyword.Create -> q.ExecuteCreate ctx

and NodeQuery =
    { Name: string option
      Label: string option
      Edge: EdgeQuery option
      Properties: Map<string, obj> }

    static member Create(block: NodeBlock) =
        { Name = block.Name
          Label = block.Label
          Edge = block.Edge |> Option.map EdgeQuery.Create
          Properties = Map.empty }

    member nq.ExecuteFetch(ctx: SqliteContext) : Records.Node list =
        match nq.Label, nq.Properties.IsEmpty with
        | Some label, false ->
            // Get by label and properties.
            Nodes.getByLabel ctx label
        | Some label, true ->
            // Get by label only
            Nodes.getByLabel ctx label
        | None, false ->
            // TODO implement property queries.
            []
        | None, true ->
            // Get all
            []

and EdgeQuery =
    { Name: string option
      Category: string option
      Node: NodeQuery
      RelationshipDirection: RelationshipDirection
      Properties: Map<string, obj> }

    static member Create(block: EdgeBlock) =
        { Name = block.Name
          Category = block.Category
          Node = NodeQuery.Create block.Node
          RelationshipDirection =
            match block.From, block.To with
            | Some f, Some t ->
                match f, t with
                | "<-", "->" -> RelationshipDirection.Either
                | "<-", _ -> RelationshipDirection.Backwards
                | _, "->" -> RelationshipDirection.Forwards
                | _, _ -> RelationshipDirection.Neither
            | Some f, None ->
                match f with
                | "<-" -> RelationshipDirection.Backwards
                | _ -> RelationshipDirection.Neither
            | None, Some t ->
                match t with
                | "->" -> RelationshipDirection.Forwards
                | _ -> RelationshipDirection.Neither
            | None, None -> RelationshipDirection.Neither

          Properties = Map.empty }

    member eq.ExecuteFetch(ctx: SqliteContext, ?fromNodeId: string, ?toNodeId: string) : Records.Edge list =
        // NOTE - a bit of a large match to cover all possibilities.
        // NOTE - handling needed for bidirectional edges(?)
        match eq.RelationshipDirection, eq.Category, eq.Properties.IsEmpty with
        | RelationshipDirection.Forwards, Some category, true ->
            fromNodeId
            |> Option.map (Edges.getByCategoryAndFromNode ctx category)
            |> Option.defaultValue []
        | RelationshipDirection.Forwards, Some category, false ->
            // TODO implement property query
            fromNodeId
            |> Option.map (Edges.getByCategoryAndFromNode ctx category)
            |> Option.defaultValue []
        | RelationshipDirection.Forwards, None, true ->
            fromNodeId |> Option.map (Edges.getByFromNode ctx) |> Option.defaultValue []
        | RelationshipDirection.Forwards, None, false ->
            // TODO implement property query
            fromNodeId |> Option.map (Edges.getByFromNode ctx) |> Option.defaultValue []
        | RelationshipDirection.Backwards, Some category, true ->
            toNodeId
            |> Option.map (Edges.getByCategoryAndToNode ctx category)
            |> Option.defaultValue []
        | RelationshipDirection.Backwards, Some category, false ->
            // TODO implement property query
            toNodeId
            |> Option.map (Edges.getByCategoryAndToNode ctx category)
            |> Option.defaultValue []
        | RelationshipDirection.Backwards, None, true ->
            toNodeId |> Option.map (Edges.getByToNode ctx) |> Option.defaultValue []
        | RelationshipDirection.Backwards, None, false ->
            // TODO implement property query
            toNodeId |> Option.map (Edges.getByToNode ctx) |> Option.defaultValue []
        | RelationshipDirection.Either, Some category, true ->
            // TODO either query
            []
        | RelationshipDirection.Either, Some category, false ->
            // TODO either query
            []
        | RelationshipDirection.Either, None, true ->
            // TODO either query
            []
        | RelationshipDirection.Either, None, false ->
            // TODO either query
            []
        | RelationshipDirection.Neither, _, _ -> []

and RelationshipDirection =
    | Forwards
    | Backwards
    | Either
    | Neither

and Keyword =
    | Create
    | Match

    static member Deserialize(str: string) =
        match str.ToLower() with
        | "create" -> Keyword.Create
        | "match" -> Keyword.Match
        | _ -> failwith $"Unknown keyword: `{str}`"

and QueryResult =
    | Created
    | Matched of Node list
