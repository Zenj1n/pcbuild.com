from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j

import csv
import datetime
import time


class alt_opslag(CrawlSpider):
    name = "alt_opslag"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=980&tk=7&lk=9564",
        "http://www.alternate.nl/html/product/listing.html?navId=990&navId=988&tk=7&lk=9553",
        "http://www.alternate.nl/html/product/listing.html?navId=978&tk=7&lk=9566",
        "http://www.alternate.nl/html/product/listing.html?navId=17557&bgid=8459&tk=7&lk=9601",
        "http://www.alternate.nl/html/product/listing.html?navId=19991&tk=7&lk=12801",
        "http://www.alternate.nl/html/product/listing.html?navId=14655&bgid=8985&tk=7&lk=9599",
        "http://www.alternate.nl/html/product/listing.html?navId=1690&tk=7&lk=9590",
        "http://www.alternate.nl/html/product/listing.html?navId=11890&bgid=8985&tk=7&lk=9585"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        now = datetime.datetime.today()
        date = now.strftime('%m/%d/%Y')
        f = open("C:\\GitHub\\pcbuild.com\\Crawler\\prijsgeschiedenis.csv", "a")
        graph_db = neo4j.GraphDatabaseService("http://Horayon:Zenjin@localhost:8080/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        for titles in titles:
            webshop = 'Alternate.nl'
            name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url_raw = titles.select('a[@class="productLink"]/@href').extract()
            component = 'HDD'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro_raw = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent_raw = titles.select(
                'div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()
            type_raw = response.xpath('//*[@id="listingResult"]/div[4]/text()').extract()

            url = ''.join(url_raw).replace("[\"]\"", "")

            type = ''.join(type_raw).strip()
            capaciteit = desc[0].strip()
            snelheid = desc[1].strip()

            euro_raw = ''.join(euro_raw)
            euro = euro_raw[1:]
            cent_raw = ''.join(cent_raw)
            cent = cent_raw[:-1]
            price_raw = euro + cent
            price_raw2 = price_raw.replace("[\"]\"*", "").strip();
            price = price_raw2.replace("-", "00")

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_opslag = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                               "match (c:opslag) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_SetSpecifications = neo4j.CypherQuery(graph_db,
                                                            "MATCH (c:opslag) WHERE c.naam = {namedb} SET c.type = {type}, c.capaciteit = {capaciteit}, c.snelheid = {snelheid}")
                alt_opslag = query_SetSpecifications.execute(type=type, namedb=namedb, capaciteit=capaciteit,
                                                             snelheid=snelheid)
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                                                              "MATCH (c:opslag)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                alt_opslag = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                                  "MATCH (c:opslag), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_opslag = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                                                              "Create (c:opslag {naam:{namedb}, capaciteit:{capaciteit}, snelheid:{snelheid}, type:{type}})")
                alt_opslag = query_CreateComponentNode.execute(namedb=namedb, capaciteit=capaciteit,
                                                               snelheid=snelheid, type=type)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                                                                  "MATCH (c:opslag), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_opslag = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                                                                   price=price, url=url)
            time.sleep(10)

            csv_f = csv.reader(f)
            a = csv.writer(f, delimiter=',')
            a.writerow([str(date), name, price])