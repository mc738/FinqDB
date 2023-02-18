namespace FinqDB.Queries.V1

open System
open System.Text.RegularExpressions

module Parsing =

    type Input = char array

    let nodeRegex =
        Regex(
            "(?<name>(\w+))?(:\s{0,})?(?<label>(\w+))?(\s{0,})?(\{(?<properties>(.*?))\})?",
            RegexOptions.ExplicitCapture ||| RegexOptions.Compiled
        )

    let edgeRegex =
        Regex(
            "(?<from>\<-|-)\[(?<name>(\w+))?:(?<category>(\w+))?(\s{0,})?(\{(?<properties>(.*?))\})?\](?<to>->|-)",
            RegexOptions.ExplicitCapture ||| RegexOptions.Compiled
        )

    type NodeBlock =
        { Name: string option
          Label: string option
          PropertyString: string option
          Edge: EdgeBlock option }

        static member Create(str: string, ?edge: EdgeBlock) =
            let m = nodeRegex.Match(str)

            { Name =
                match m.Groups.[1].Success with
                | true -> Some m.Groups.[1].Value
                | _ -> None
              Label =
                match m.Groups.[2].Success with
                | true -> Some m.Groups.[2].Value
                | _ -> None
              PropertyString =
                match m.Groups.[3].Success with
                | true -> Some m.Groups.[3].Value
                | _ -> None
              Edge = edge }

    and EdgeBlock =
        { From: string option
          Name: string option
          Category: string option
          PropertyString: string option
          To: string option
          Node: NodeBlock }

        static member Create(str: string, node: NodeBlock) =
            let m = edgeRegex.Match(str)

            { From =
                match m.Groups.[1].Success with
                | true -> Some m.Groups.[1].Value
                | _ -> None
              Name =
                match m.Groups.[2].Success with
                | true -> Some m.Groups.[2].Value
                | _ -> None
              Category =
                match m.Groups.[3].Success with
                | true -> Some m.Groups.[3].Value
                | _ -> None
              PropertyString =
                match m.Groups.[4].Success with
                | true -> Some m.Groups.[4].Value
                | _ -> None
              To =
                match m.Groups.[5].Success with
                | true -> Some m.Groups.[5].Value
                | _ -> None
              Node = node }

    and RawQuery = { Keyword: string; Node: NodeBlock }

    let readUntil (c: char) (str: Input) (start: int) =
        let rec read (i) =
            match str |> Array.tryItem i with
            | Some ch when ch = c -> Some i
            | Some _ -> read (i + 1)
            | None -> None

        read (start)

    let getKeyword (input: Input) =
        match readUntil ' ' input 0 with
        | Some i -> Some(input.[0 .. i - 1] |> String, i + 1)
        | None -> None

    let createBlocks (input: Input) (start: int) =
        let rec read (i: int) =
            match readUntil ')' input i with
            | Some newI ->
                let nodeText = input.[i .. newI - 1] |> String

                match readUntil '(' input (newI + 1) with
                | Some newI2 ->

                    let edgeText = input.[newI + 1 .. newI2 - 1] |> String
                    let innerNode = read (newI2 + 1)
                    NodeBlock.Create(nodeText, EdgeBlock.Create(edgeText, innerNode))

                | None -> NodeBlock.Create(nodeText)

            | None ->
                { Name = None
                  Label = None
                  PropertyString = None
                  Edge = None }

        read (start)

    let readBlocks (input: Input) (start: int) =
        let rec read (acc: string list, i: int) =
            match readUntil ')' input i with
            | Some newI ->
                let newAcc = acc @ [ input.[i .. newI - 1] |> String ]

                let node = input.[i .. newI - 1] |> String |> NodeBlock.Create


                match readUntil '(' input (newI + 1) with
                | Some newI2 ->


                    NodeBlock.Create(input.[i .. newI - 1] |> String)

                    read (newAcc @ [ input.[newI + 1 .. newI2 - 1] |> String ], newI2 + 1)
                | None ->

                    newAcc, newI + 1
            | None -> acc, i

        read ([], start)

    let parse (str: string) =
        let input = str |> Array.ofSeq

        // Get keyword
        match getKeyword input with
        | Some (keyword, next) ->
            Ok { Keyword = keyword; Node = createBlocks input (next + 1) }
        | None -> Error "Missing keyword."
