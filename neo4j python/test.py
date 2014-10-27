from py2neo import neo4j
from py2neo import node, rel

graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
query = neo4j.CypherQuery(graph_db, "CREATE (a {name:{name_a}})-[ab:KNOWS]->(b {name:{name_b}})"
                              "RETURN a, b, ab")
a, b, ab = query.execute(name_a="Alice", name_b="Bob").data[0]