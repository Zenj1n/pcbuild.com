import scrapy
from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import rel, node
from py2neo import neo4j

from alternate.items import AlternateItem


class alt_gfx(CrawlSpider):
    name = "alt_gfx"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=11606&bgid=11369&tk=7&lk=9374",
        "http://www.alternate.nl/html/product/listing.html?navId=1358&tk=7&lk=9372",
        "http://www.alternate.nl/html/product/listing.html?navId=11608&bgid=10846&tk=7&lk=9365",
        "http://www.alternate.nl/html/product/listing.html?navId=14655&bgid=8985&tk=7&lk=9599",
        "http://www.alternate.nl/html/product/listing.html?navId=1360&tk=7&lk=9381",
        "http://www.alternate.nl/html/product/listing.html?navId=17232&tk=7&lk=9382",
        "http://www.alternate.nl/html/product/listing.html?navId=1362&tk=7&lk=9361"

    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        for titles in titles:
            webshop = 'alternate.nl'
            name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url = titles.select('a[@class="productLink"]/@href').extract()
            component = 'videokaart'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            gfx = desc[0]
            geheugen = desc[1]
            slots = desc[2]

            price = euro + cent

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_gfx = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:processor) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:processor)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                alt_gfx = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:processor), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_gfx = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                 query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                 "Create (c:videokaart {naam:{namedb}, gfx:{gfx}, geheugen:{geheugen}, slots:{slots}})")
                 alt_gfx = query_CreateComponentNode.execute(namedb=namedb, gfx=gfx,
                 geheugen=geheugen, slots=slots)
                 query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                 "MATCH (c:videokaart), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                 alt_gfx = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                 price=price, url=url)