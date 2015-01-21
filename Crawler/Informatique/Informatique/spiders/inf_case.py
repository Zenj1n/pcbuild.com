import csv
import datetime
import time
from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import Selector
from py2neo import neo4j

class inf_case(CrawlSpider):
    name = "inf_case"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800"]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
                  , callback="parse_start_url", follow=True),
    )

    def parse_start_url(self, response):
        now = datetime.datetime.today()
        date = now.strftime('%m/%d/%Y')
        #f = open("C:\\GitHub\\pcbuild.com\\Crawler\\prijsgeschiedenis.csv",
         #        "a")
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = Selector(response)
        titles = hxs.xpath('//ul[@id="detailview"]/li')
        for titles in titles:
            webshop = 'Informatique'
            name_raw = titles.xpath('div[@id="title"]/a/text()').extract()
            url_raw = titles.xpath('div[@id="title"]/a/@href').extract()
            component = 'behuizing'
            desc = titles.xpath('div[@id="description"]/ul/li/text()').extract()
            price_raw = titles.xpath('div[@id="price"]/text()').extract()
            #image_urls = titles.xpath('div[@id="image"]/a/img/@src').extract()

            url = ''.join(url_raw).replace("[\"]\"","")
            name = ''.join(name_raw).replace("\"[u'", "")
            price = ''.join(price_raw)[1:].replace("[\"]\"*", "").strip();


            try:
                vormfactor = desc[0].replace("\"[u'", "").strip();
            except:
                vormfactor = "onbekend"
            try:
                interfaces = desc[1].replace("\"[u'", "").strip()
            except:
                interfaces = "onbekend"
            try:
                vormvoeding = desc[2].replace("\"[u'", "").strip()
            except:
                vormvoeding = "onbekend"

            kernen = "onbekend"



            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_case = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:behuizing) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                inf_case = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                MATCH = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                "Create (c:behuizing {naam:{namedb}, interfaces:{interfaces}, vormfactor:{vormfactor}, vormvoeding:{vormvoeding}})")
                MATCH = query_CreateComponentNode.execute(namedb=namedb, interfaces=interfaces,
                vormfactor=vormfactor, vormvoeding=vormvoeding)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                MATCH = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                price=price, url=url)
            time.sleep(10)

           # csv_f = csv.reader(f)
           # a = csv.writer(f, delimiter=',')
           # a.writerow([str(date), name, price])