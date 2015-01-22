from py2neo import neo4j

__author__ = 'Fabian'


from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import Selector


def check_dbPsu():
    graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
    queryRead = neo4j.CypherQuery(graph_db,  "MATCH (n:`voeding`) RETURN n LIMIT 25")
    phone = queryRead.execute()
    for record in queryRead.stream():
        print ""
    return record


def check_dbCpu():
    graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
    queryRead = neo4j.CypherQuery(graph_db,  "MATCH (n:`processor`) RETURN n LIMIT 25")
    phone = queryRead.execute()
    for record in queryRead.stream():
        print ""
    return record


def check_dbOpslag():
    graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
    queryRead = neo4j.CypherQuery(graph_db,  "MATCH (n:`opslag`) RETURN n LIMIT 25")
    phone = queryRead.execute()
    for record in queryRead.stream():
        print ""
    return record


def check_dbMB():
    graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
    queryRead = neo4j.CypherQuery(graph_db,  "MATCH (n:`moederbord`) RETURN n LIMIT 25")
    phone = queryRead.execute()
    for record in queryRead.stream():
        print ""
    return record


def check_dbRam():
    graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
    queryRead = neo4j.CypherQuery(graph_db,  "MATCH (n:`werkgeheugen`) RETURN n LIMIT 25")
    phone = queryRead.execute()
    for record in queryRead.stream():
        print ""
    return record


def check_dbCase():
    graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
    queryRead = neo4j.CypherQuery(graph_db,  "MATCH (n:`behuizing`) RETURN n LIMIT 25")
    phone = queryRead.execute()
    for record in queryRead.stream():
        print ""
    return record


def parse_price(response):
    hxs = Selector(text = response)
    titles = hxs.xpath('//ul[@id="detailview"]/li')
    price = titles.xpath('div[@id="price"]/text()').extract()
    return price


def parse_desc(response):
    hxs = Selector(text = response)
    titles = hxs.xpath('//ul[@id="detailview"]/li')
    desc = titles.xpath('div[@id="description"]/ul/li/text()').extract()
    return desc


def parse_url(response):
    hxs = Selector(text = response)
    titles = hxs.xpath('//ul[@id="detailview"]/li')
    url = titles.xpath('div[@id="title"]/a/@href').extract()
    return url


def parse_title(response):
    hxs = Selector(text = response)
    titles = hxs.xpath('//ul[@id="detailview"]/li')
    title = titles.xpath('div[@id="title"]/a/text()').extract()
    return title
    #image_urls = titles.xpath('div[@id="image"]/a/img/@src').extract()


class Database(CrawlSpider):
    name = "unittest_Case"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=004&view=6&&sort=pop&pl=800"]



    rules = (Rule(SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
                  , callback="parse_start_url", follow=True),
    )

    #---------------------------------------------database-----------------------------------------------------------------#



