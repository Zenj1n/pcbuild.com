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
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        print "== Initializing =="

        for titles in titles:
            webshop = 'alternate.nl'
            name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url = titles.select('a[@class="productLink"]/@href').extract()
            component = 'behuizing'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            interfaces = desc[0];
            vormfactor = desc[1];
            vormvoeding = desc[2];

            price = euro + cent



            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            #query_test = 'MATCH (c:behuizing), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:'+price+' ]-w'

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_case = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:processor) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_SetSpecifications = neo4j.CypherQuery(graph_db,
                "MATCH (c:behuizing) WHERE c.naam = {namedb} SET c.interfaces = {interfaces}, c.vormfactor = {vormfactor}, c.vormvoeding = {vormvoeding}")
                alt_case = query_DeleteRelationships.execute(namedb=namedb, interfaces=interfaces, vormfactor=vormfactor, vormvoeding=vormvoeding)
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






       

