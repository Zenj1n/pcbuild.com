import csv
import datetime
import time

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class inf_gfx(CrawlSpider):
    name = "inf_gfx"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=166&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=500",
                  "http://www.informatique.nl/?m=usl&g=585&view=6&&sort=pop&pl=500"]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
                  , callback="parse_start_url", follow=True),
    )

    def parse_start_url(self, response):
        now = datetime.datetime.today()
        date = now.strftime('%m/%d/%Y')
        f = open("C:\\GitHub\\pcbuild.com\\Crawler\\prijsgeschiedenis.csv",
                 "a")
        graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@localhost:8080/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//ul[@id="detailview"]/li')
        for titles in titles:
            webshop = 'Informatique.nl'
            name_raw = titles.select('div[@id="title"]/a/text()').extract()
            url_raw = titles.select('div[@id="title"]/a/@href').extract()
            component = 'videokaart'
            desc = titles.select('div[@id="description"]/ul/li/text()').extract()
            price_raw = titles.select('div[@id="price"]/text()').extract()
            # image_urls = titles.select('div[@id="image"]/a/img/@src').extract()

            #filter de data, maak eerst strings van---------------------------------------------------------------------

            url = ''.join(url_raw).replace("[\"]\"", "")
            name = ''.join(name_raw).replace("\"[u'", "")
            price = ''.join(price_raw)[1:].replace("[\"]\"", "")

            try:
                gfx = desc[0].replace("\"[u'", "").strip()
            except:
                gfx = "onbekend"
            try:
                geheugen = desc[1].replace("\"[u'", "").strip()
            except:
                geheugen = "onbeklend"
            slots = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_gfx = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                               "match (c:videokaart) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            #check of de componenten matchen/al bestaan, update het anders make een nieuwe node aan

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                              "MATCH (c:videokaart)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                inf_gfx = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                                  "MATCH (c:videokaart), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                inf_gfx = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                              "Create (c:videokaart {naam:{namedb}, gfx:{gfx}, geheugen:{geheugen}, slots:{slots}})")
                inf_gfx = query_CreateComponentNode.execute(namedb=namedb, gfx=gfx,
                                                            geheugen=geheugen, slots=slots)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                                  "MATCH (c:videokaart), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                inf_gfx = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                                price=price, url=url)
            time.sleep(10)

            #zet de data in een csv
            csv_f = csv.reader(f)
            a = csv.writer(f, delimiter=',')
            a.writerow([str(date), name, price])