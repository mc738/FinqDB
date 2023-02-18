namespace FinqDB.App

open System
open Freql.Core
open Freql.Sqlite
open FinqDB.Store.V1

module Repl =

    let unwrap<'T> (result: Result<'T, string>) =
        match result with
        | Ok r -> r
        | Error e -> failwith e

    let toLower (str: string) = str.ToLower()

    let getInput _ =
        Console.Write "> "
        Console.ReadLine()

    let getBool _ =
        match getInput () |> toLower with
        | "y"
        | "yes"
        | "true"
        | "1"
        | "ok" -> true
        | _ -> false

    let ifEmpty (defaultValue: unit -> string) (value: string) =
        match String.IsNullOrWhiteSpace value with
        | true -> defaultValue ()
        | false -> value

    let tryGetDecimal _ =
        let rec attempt () =
            match getInput () |> Decimal.TryParse with
            | true, r -> r
            | false, _ ->
                printError "Input could not be parsed as number. Try again."
                attempt ()

        attempt ()


    type ReplCommand =
        | AddNode
        | AddNodeLabel
        | GetNodesByLabel
        | AddEdge
        | AddEdgeWeight
        | Help
        | Clear
        | Exit
        | Unknown of string

        static member Parse(str: string) =
            match str.ToLower() with
            // Example add-node
            | "add-node" -> ReplCommand.AddNode
            | "add-node-label" -> ReplCommand.AddNodeLabel
            | "get-nodes-by-label" -> ReplCommand.GetNodesByLabel
            | "add-connection"
            | "add-edge" -> ReplCommand.AddEdge
            | "add-edge-weight" -> ReplCommand.AddEdgeWeight
            | "help"
            | "h" -> ReplCommand.Help
            | "clear"
            | "cls" -> ReplCommand.Clear
            | "x"
            | "exit"
            | "quit" -> ReplCommand.Exit
            | _ -> ReplCommand.Unknown str

    let run _ =
        printInfo "Welcome to FinqDB."

        printInfo "Connect to existing database? (y/n)"

        use ctx =
            match getBool () with
            | true ->
                printInfo "Enter existing database path:"
                let path = getInput ()
                SqliteContext.Open(path)
            | false ->
                printInfo "Enter new database path:"
                let path = getInput ()
                printInfo "Enter new database name:"
                let name = getInput ()
                printInfo "Enter new database description:"
                let description = getInput ()
                Utils.create path name description |> unwrap

        let rec loop () =

            let cont =
                match getInput () |> ReplCommand.Parse with
                | AddNode ->
                    printInfo "Enter new node id (leave blank for auto generated):"
                    let id = getInput () |> ifEmpty Utils.createId
                    printInfo "Enter new node name:"
                    let name = getInput ()

                    match Nodes.add ctx id name with
                    | Ok _ -> printSuccess $"Node added! Id: `{id}`"
                    | Error e -> printError $"Failed to add node. Error: `{e}`"

                    true
                | AddNodeLabel ->
                    printInfo "Enter node id:"
                    let nodeId = getInput ()
                    printInfo "Enter label:"
                    let label = getInput ()

                    match Nodes.addNodeLabel ctx nodeId label with
                    | Ok _ -> printSuccess "Node label added!"
                    | Error e -> printError $"Failed to add node label. Error: `{e}`"

                    true
                | GetNodesByLabel ->
                    printInfo "Enter label:"
                    let label = getInput ()

                    Nodes.getByLabel ctx label
                    |> List.iteri (fun i node ->
                        printInfo $"# Node {i + 1}"
                        printInfo $"* Id: {node.Id}"
                        printInfo $"* Name: {node.Name}"
                        printInfo $"* Created on: {node.CreatedOn}")

                    true
                | AddEdge ->
                    printInfo "Enter new edge id (leave blank for auto generated):"
                    let id = getInput () |> ifEmpty Utils.createId
                    printInfo "Enter new edge name:"
                    let name = getInput ()
                    printInfo "Enter from node id:"
                    let fromNodeId = getInput ()
                    printInfo "Enter to node id:"
                    let toNodeId = getInput ()
                    printInfo "Is edge bidirectional? (y/n)"
                    let bidirectional = getBool ()

                    match Edges.add ctx id name fromNodeId toNodeId bidirectional with
                    | Ok _ -> printSuccess $"Edge added! Id: `{id}`"
                    | Error e -> printError $"Failed to add edge. Error: `{e}`"

                    true

                | AddEdgeWeight ->
                    printInfo "Enter edge id:"
                    let edgeId = getInput ()
                    printInfo "Enter category:"
                    let category = getInput ()
                    printInfo "Enter weight:"
                    let weight = tryGetDecimal ()

                    match Edges.addEdgeWeight ctx edgeId category weight with
                    | Ok _ -> printSuccess "Edge weight added!"
                    | Error e -> printError $"Failed to add edge weight. Error: `{e}`"

                    true
                | Help ->

                    true
                | Clear ->
                    Console.Clear()
                    true
                | Exit ->

                    false
                | Unknown cmd ->
                    printError $"Unknown command: `{cmd}`"
                    true

            match cont with
            | true -> loop ()
            | false -> printInfo "Exiting."

        loop ()
