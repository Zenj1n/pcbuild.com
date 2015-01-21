import csv
import datetime
import time


from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class inf_ram(CrawlSpider):
    name = "inf_ram"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=522&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=725&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=194&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=ART&G=077&view=6&&sort=pop&pl=500"
    ]

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
            component = 'werkgeheugen'
            desc = titles.select('div[@id="description"]/ul/li/text()').extract()
            price_raw = titles.select('div[@id="price"]/text()').extract()
            ddr_raw = response.xpath('//*[@id="hdr"]/h1/text()').extract()
            #image_urls = titles.select('div[@id="image"]/a/img/@src').extract()

            url = ''.join(url_raw).replace("[\"]\"","")
            name = ''.join(name_raw).replace("\"[u'", "")
            price = ''.join(price_raw)[1:].replace("[\"]\"","")
            ddr = ''.join(ddr_raw).replace("modules","").replace("[\"]\"","").strip().lower()

            try:
                capaciteit = desc[0].replace("\"[u'", "").strip()
            except:
                capaciteit = "onbekend"
            try:
                modules = desc[2].replace("\"[u'", "").strip()
            except:
                modules = "onbekend"

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            inf_ram = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:werkgeheugen) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:werkgeheugen)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                inf_ram = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:werkgeheugen), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                inf_ram = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                "Create (c:werkgeheugen {naam:{namedb}, capaciteit:{capaciteit}, ddr:{ddr}, modules:{modules}})")
                inf_ram = query_CreateComponentNode.execute(namedb=namedb, capaciteit=capaciteit,
                ddr=ddr, modules=modules)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:werkgeheugen), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                inf_ram = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                price=price, url=url)
            time.sleep(10)

            csv_f = csv.reader(f)
            a = csv.writer(f, delimiter=',')
            a.writerow([str(date), name, price])