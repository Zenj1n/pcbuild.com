from py2neo import rel, node
from py2neo import neo4j

class DashboardNeo4j:
    graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@145.24.222.155:8080/db/data/")

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

    #-----------------------aantal componenten ------------------------------------#
    queryCountAlles = neo4j.CypherQuery(graph_db, "MATCH n-[r]-() RETURN count(n)")
    Alles = queryCountAlles.execute()
    for record in queryCountAlles.stream():
        Alles = record[0]

    #-----------------------aantal componenten Informatique ------------------------------------#
    queryInformatique = neo4j.CypherQuery(graph_db, "MATCH (n)-[r]-(i) where i.naam = " + " \"Informatique\" " + "RETURN count(n)")
    Informatique = queryInformatique.execute()
    for record in queryInformatique.stream():
        Informatique = record[0]
    #-----------------------aantal componenten Alternate ------------------------------------#
    queryAlternate = neo4j.CypherQuery(graph_db, "MATCH (n)-[r]-(i) where i.naam = " + " \"alternate.nl\" " + "RETURN count(n)")
    Alternate = queryAlternate.execute()
    for record in queryAlternate.stream():
        Alternate = record[0]
    #------------------------------------------------------------------------------------------------------------------#




    print "Aantal onderdelen per component"
    print "voeding" + "             " + str(voeding)
    print "opslag" + "              " + str(opslag)
    print "behuizing" + "           " + str(behuizing)
    print "werkgeheugen" + "        " + str(werkgeheugen)
    print "processor" + "           " + str(processor)
    print "moederbord" + "          " + str(moederbord)
    print "videokaart" + "          " + str(videokaart)
    print "totaal componenten" + "  " + str(voeding + opslag + behuizing + werkgeheugen + processor + moederbord + videokaart)
    print "totaal nodes" + "        " + str(Alles)

    print "Aantal Informatique producten" + " " + str(Informatique)
    print "Aantal Alternate producten" + "    " + str(Alternate)
    print "Aantal ongebruike producten (kunnen verwijderd worden)" + "  " + str(Alles - (Alternate + Informatique))

