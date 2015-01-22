from py2neo import rel, node
from py2neo import neo4j

class neo4jcasus:
    graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@localhost:8080/db/data/")

    #-----------------------aantal componenten voeding------------------------------------#
    queryCountVoeding = neo4j.CypherQuery(graph_db, "MATCH (n:`voeding`) RETURN count(n)")
    voeding = queryCountVoeding.execute()
    for record in queryCountVoeding.stream():
        voeding = record[0]

    #-----------------------aantal componenten opslag------------------------------------#
    queryCountOpslag = neo4j.CypherQuery(graph_db, "MATCH (n:`opslag`) RETURN count(n)")
    opslag = queryCountOpslag.execute()
    for record in queryCountOpslag.stream():
        opslag = record[0]

    #-----------------------aantal componenten behuzing------------------------------------#
    queryCountBehuzing = neo4j.CypherQuery(graph_db, "MATCH (n:`behuizing`) RETURN count(n)")
    behuzing = queryCountBehuzing.execute()
    for record in queryCountBehuzing.stream():
        behuizing = record[0]

    #-----------------------aantal componenten werkgeheugen------------------------------------#
    queryCountWerkgeheugen = neo4j.CypherQuery(graph_db, "MATCH (n:`werkgeheugen`) RETURN count(n)")
    werkgeheugen = queryCountWerkgeheugen.execute()
    for record in queryCountWerkgeheugen.stream():
        werkgeheugen = record[0]

    #-----------------------aantal componenten processor------------------------------------#
    queryCountProcessor = neo4j.CypherQuery(graph_db, "MATCH (n:`processor`) RETURN count(n)")
    processor = queryCountProcessor.execute()
    for record in queryCountProcessor.stream():
        processor = record[0]

    #-----------------------aantal componenten moederbord------------------------------------#
    queryCountMoederbord = neo4j.CypherQuery(graph_db, "MATCH (n:`moederbord`) RETURN count(n)")
    moederbord = queryCountMoederbord.execute()
    for record in queryCountMoederbord.stream():
        moederbord = record[0]

    #-----------------------aantal componenten videokaart------------------------------------#
    queryCountVideokaart = neo4j.CypherQuery(graph_db, "MATCH (n:`videokaart`) RETURN count(n)")
    videokaart = queryCountVideokaart.execute()
    for record in queryCountVideokaart.stream():
        videokaart = record[0]
    #------------------------------------------------------------------------------------------------------------------#

    queryPriceVoeding = neo4j.CypherQuery(graph_db, "MATCH (n:`voeding`)-[r]-() RETURN r.prijs LIMIT 25")
    voedingprice = queryPriceVoeding.execute()
    for record in queryPriceVoeding.stream():
        voedingprice = record[0]

    print "Aantal onderdelen per component"
    print "voeding" + "      " + str(voeding)
    print "opslag" + "       " + str(opslag)
    print "behuizing" + "    " + str(behuizing)
    print "werkgeheugen" + " " + str(werkgeheugen)
    print "processor" + "    " + str(processor)
    print "moederbord" + "   " + str(moederbord)
    print "videokaart" + "   " + str(videokaart)
    print "totaal" + "       " + str( voeding + opslag + behuizing + werkgeheugen + processor + moederbord + videokaart)

    print str(voedingprice)

