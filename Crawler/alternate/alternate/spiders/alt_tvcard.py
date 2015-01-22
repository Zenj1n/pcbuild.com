from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class alt_tvcard(CrawlSpider):
    name = "alt_tvcard"
    allowed_domains = ["alternate.nl"]
    start_urls = [
        "http://www.alternate.nl/html/product/listing.html?navId=960&tk=7&lk=9525",
        "http://www.alternate.nl/html/product/listing.html?navId=964&tk=7&lk=9522",
        "http://www.alternate.nl/html/product/listing.html?navId=962&tk=7&lk=9524",
        "http://www.alternate.nl/html/product/listing.html?navId=966&tk=7&lk=9523"
    ]

    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[@class="next"]')), callback='parse_start_url', follow=True),)

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        for titles in titles:
            webshop = 'alternate.nl'
            name = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
            url_raw = titles.select('a[@class="productLink"]/@href').extract()
            component = 'tvkaart'
            desc = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
            euro = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
            cent = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/sup/text()').extract()

            url = ''.join(url_raw).replace("[\"]\"", "")

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query_CreateWebshopNode = neo4j.CypherQuery(graph_db,
                                                        "MERGE (w:Webshop { naam: {webshop} })")
            alt_tvcard = query_CreateWebshopNode.execute(webshop=webshop)

            query_CheckOnExistingComponent = neo4j.CypherQuery(graph_db,
                                                      "match (c:processor) where c.naam = {namedb} with COUNT(c) as Count_C RETURN Count_C")
            matchCount = query_CheckOnExistingComponent.execute(namedb=namedb)
            for record in query_CheckOnExistingComponent.stream(namedb=namedb):
                matchCountNumber = record[0]

            if matchCountNumber != 0:
                query_DeleteRelationships = neo4j.CypherQuery(graph_db,
                "MATCH (c:processor)-[r]-(w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} DELETE r")
                alt_tvcard = query_DeleteRelationships.execute(namedb=namedb, webshop=webshop)
                query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                "MATCH (c:processor), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                alt_tvcard = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop, price=price, url=url)
            else:
                 query_CreateComponentNode = neo4j.CypherQuery(graph_db,
                 "Create (c:processor {naam:{namedb}, kloksnelheid:{kloksnelheid}, socket:{socket}, kernen:{kernen}})")
                 alt_tvcard = query_CreateComponentNode.execute(namedb=namedb, kloksnelheid=kloksnelheid,
                 socket=socket, kernen=kernen)
                 query_CreatePriceRelationship = neo4j.CypherQuery(graph_db,
                 "MATCH (c:processor), (w:Webshop)  WHERE c.naam = {namedb} AND w.naam = {webshop} CREATE UNIQUE  c-[:verkrijgbaar{prijs:{price}, url:{url}}]-w")
                 alt_tvcard = query_CreatePriceRelationship.execute(namedb=namedb, webshop=webshop,
                 price=price, url=url)

            csv_f = csv.reader(f)
            a = csv.writer(f, delimiter=',')
            a.writerow([str(now), name, price])