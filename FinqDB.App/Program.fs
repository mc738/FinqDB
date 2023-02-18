open System.IO
open System.Text.RegularExpressions
open FinqDB.App
open FinqDB.Queries.V1
open FinqDB.Store.V1
open Freql.Sqlite

// Basic query -
// CREATE (p:LABEL {id: node_1})-[:CATEGORY]->(t:LABEL {id: node_2})

// Example:
// MATCH (p:person {id: node_1})-[:likes]->(t:job {role: ""})

module Test =

    let onError (result: Result<unit, string>) =
        match result with
        | Ok _ -> ()
        | Error e -> failwith $"Operation failed. Error: `{e}`"

    let setup (root: string) =
        let path = Path.Combine(root, "finq_test.db")

        printInfo "Creating test database."
        
        match FinqDB.Store.V1.Utils.create path "finq_test" "Finq test database" with
        | Ok ctx ->
            // Add nodes
            Nodes.add ctx "node_1" "Bryan Cranston" |> onError
            Nodes.add ctx "node_2" "Breaking Bad" |> onError
            Nodes.add ctx "node_3" "Godzilla" |> onError
            Nodes.addNodeLabel ctx "node_1" "person" |> onError
            Nodes.addNodeLabel ctx "node_2" "tv_show" |> onError
            Nodes.addNodeLabel ctx "node_3" "movie" |> onError
            
            Nodes.add ctx "node_4" "Benedict Cumberbatch" |> onError
            Nodes.add ctx "node_5" "Sherlock" |> onError
            Nodes.add ctx "node_6" "Dr. Strange" |> onError
            Nodes.addNodeLabel ctx "node_4" "person" |> onError
            Nodes.addNodeLabel ctx "node_5" "tv_show" |> onError
            Nodes.addNodeLabel ctx "node_6" "movie" |> onError
            
            Nodes.add ctx "node_7" "Scott Derrickson" |> onError
            Nodes.addNodeLabel ctx "node_7" "person" |> onError
            
            
            Edges.add ctx "edge_1" "edge_1" "node_1" "node_2" false |> onError
            Edges.add ctx "edge_2" "edge_2" "node_1" "node_3" false |> onError
            Edges.addEdgeWeight ctx "edge_1" "acted_in" 1m |> onError
            Edges.addEdgeWeight ctx "edge_2" "acted_in" 1m |> onError

            Edges.add ctx "edge_3" "edge_3" "node_4" "node_5" false |> onError
            Edges.add ctx "edge_4" "edge_4" "node_4" "node_6" false |> onError
            Edges.addEdgeWeight ctx "edge_3" "acted_in" 1m |> onError
            Edges.addEdgeWeight ctx "edge_4" "acted_in" 1m |> onError

            Edges.add ctx "edge_5" "edge_5" "node_7" "node_6" false |> onError
            Edges.addEdgeWeight ctx "edge_5" "directed" 1m |> onError
            
            
            printSuccess "Test database created."
        

        | Error e -> failwith $"Failed to create database. Error: `{e}`"


        ()

    let run (root: string) =

        
        printInfo "Running test queries."
        
        use ctx = SqliteContext.Open(Path.Combine(root, "finq_test.db"))
        
        let q1 = "MATCH (p:person)-[r:acted_in]->(m:movie)"
        let q2 = "MATCH (p:person)-[r:acted_in]->(t:tv_show)"
        let q3 = "MATCH (p:person)-[r:directed]->(m:movie)"
        let q4 = "MATCH (p:person)-[r:acted_in]->(m)"
        
        let createQuery (str: string) =
            Parsing.parse str
            |> Result.map (Query.Create)
            |> function Ok q -> q | Error e -> failwith $"Failed to create query. Error: `{e}`"
        
        let runQuery (q: Query) = q.Execute ctx
        
        let r1 = createQuery q1 |> runQuery
        let r2 = createQuery q2 |> runQuery
        let r3 = createQuery q3 |> runQuery
        let r4 = createQuery q4 |> runQuery
        
        ()


//Test.setup "C:\\ProjectData\\FinqDB"
Test.run "C:\\ProjectData\\FinqDB"


let r =
    Parsing.parse "MATCH (p:person {id: node_1})-[:likes]->(t:job {role: \"test\"})"


let r1 =
    Regex(
        "(?<name>(\w+))?(:\s{0,})?(?<label>(\w+))?(\s{0,})?(\{(?<properties>(.*?))\})?",
        RegexOptions.ExplicitCapture ||| RegexOptions.Compiled
    )

let m1 = Parsing.NodeBlock.Create("p:person {id: node_1}")
let m2 = Parsing.NodeBlock.Create("p {id: node_1}")
let m3 = Parsing.NodeBlock.Create(":person {id: node_1}")
let m4 = Parsing.NodeBlock.Create("p:person")
let m5 = Parsing.NodeBlock.Create("")

//let m6 = Parsing.EdgeBlock.Create("-[name:LABEL { test: \"Name\" }]->")

// TODO handle commands (not just repl).
Repl.run ()
