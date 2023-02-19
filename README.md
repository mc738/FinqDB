# FinqDB

`FinqDB` is a "pocket size" graph database written in F#.

**Currently FinqDB is experimental and likely to change**

## Why?

This is an experiment in graph databases and hopefully a useful too
for projects that want to store data with flexible relationships.

My initial use case was to store relational data and files for
OSINT investigations/research.

## "Pocket size"

`FinqDB` is a single file database so it is easy to copy, transport and backup.

It is not meant to compete with databases such as `Neo4j`,
which are designed to handle large datasets.

It is designed to be quick to set up and easy to use.

## Storage Engine

`FinqDB` currently uses `SQLite` as the storage engine, 
however in the future it is possible a bespoke engine will be added.

This does mean queries are not as efficient as they could be,
with multiple executions and possible over fetches.

However for small to medium sized databases this shouldn't be too much 
of an issue.

## Queries

`FinqDB` supports a query language heavily influenced be [cyher](https://neo4j.com/docs/getting-started/current/cypher-intro/).

Features might change and diverge over time but a lot of the basic syntax is the same.

### Keywords

* `MATCH`
* `CREATE`
