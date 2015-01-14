import csv
import datetime

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class alt_case(CrawlSpider):
    name = "alt_case"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=2436&bgid=8148&tk=7&lk=9309"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        now = datetime.datetime.now()
        f = open("E:\\Repositories Git Hub\\pcbuild.com\\Crawler\\alternate\\components\\case\\prijsgeschiedenis.csv", "a")
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        print "== Initializing =="

        for titles in titles:
            webshop = 'alternate.nl'
            name_raw = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url_raw = titles.select('a[@class="productLink"]/@href').extract()
            component = 'behuizing'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro_raw = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent_raw = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            url = ''.join(url_raw).replace("[\"]\"", "")
            name = ''.join(name_raw).replace("\"[u'","")

            interfaces = desc[0].strip();
            vormfactor = desc[1].strip();
            vormvoeding = desc[2].strip();

            euro_raw = ''.join(euro_raw)
            euro = euro_raw[1:]
            cent_raw = ''.join(cent_raw)
            cent = cent_raw[:-1]
            price_raw = euro + cent
            price = price_raw.replace("[\"]\"*", "").strip();

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_case = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:behuizing) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_SetSpecifications = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing) WHERE c.naam = {namedb} SET c.interfaces = {interfaces}, c.vormfactor = {vormfactor}, c.vormvoeding = {vormvoeding}")
                alt_case = query_SetSpecifications.execute(namedb=namedb, interfaces=interfaces, vormfactor=vormfactor, vormvoeding=vormvoeding)
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                alt_case = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_case = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                "Create (c:behuizing {naam:{namedb}, interfaces:{interfaces}, vormfactor:{vormfactor}, vormvoeding:{vormvoeding}})")
                alt_case = query_CreateComponentNode.execute(namedb=namedb, interfaces=interfaces,
                vormfactor=vormfactor, vormvoeding=vormvoeding)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_case = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                price=price, url=url)

            csv_f = csv.reader(f)
            a = csv.writer(f, delimiter=',')
            a.writerow([str(now), name, price])






       

