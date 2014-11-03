from ConfigParser import SafeConfigParser
from py2neo import neo4j, cypher
import py2neo

class DatabaseConnectionNeo4j:
   
    url = None
    username = None
    password = None
    isRead = False
    isConnect = False
    db_path = "db_path"

#Get Data from db_config.conf
    parser = SafeConfigParser()
    try:
        parser.read('alternate/db_config.conf')   #Defines Path
    except:
        print "Cannot open file \"db_config.conf\". Probably wrong path."
    url = parser.get('DATABASE', 'url')
    username = parser.get('DATABASE', 'username')
    password = parser.get('DATABASE', 'password')
    isRead = True

    def __init__(self):
        pass

    def openDb(self):
        graph_db = neo4j.GraphDatabaseService(self.url)
        self.isConnect = True
        return graph_db

    def closeDb(self):
        pass

    def getSomeData(self, graph_db):
        print graph_db.get_node(0)

    def getCountNodes(self, graph_db):
        print graph_db.get_node_count()

    def createNodeFromDict(self, graph_db, dict):
        isCreated = False
        graph_db.create(dict)
        isCreated = True
        return isCreated

    def getNode(self, graph_db, label): #Gets node from database. label = the Label of the Hardware, always include this in self
        node = 'alt_case'
        tempNode = 'alt_case'
        result = neo4j.CypherQuery(graph_db, "MATCH (n) WHERE n.Label = \""+ 'alt_case'+"\" RETURN n").execute()
        for r in result:
            tempNode = r[0]
        uri = neo4j.ServiceRoot('http://localhost:7474/db/data/index/node' + 'alt_case')
        node = py2neo.neo4j.Node(uri)
        return node