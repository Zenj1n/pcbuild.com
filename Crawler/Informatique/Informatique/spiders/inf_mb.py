from scrapy.contrib.spiders import CrawlSpider
from scrapy.selector import HtmlXPathSelector
from py2neo import neo4j


class inf_mb(CrawlSpider):
    name = "inf_mb"
    allowed_domains = ["informatique.nl"]
    start_urls = ["http://www.informatique.nl/?m=usl&g=726&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=670&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=699&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=635&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=ART&G=192&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=713&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=717&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?m=usl&g=655&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=657&view=6&&sort=pop&pl=500",
                  "http://www.informatique.nl/?M=USL&G=686&view=6&&sort=pop&pl=500",
    ]

    # rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
    # , callback="parse_start_url", follow= True),
    #)

    def parse_start_url(self, response):
        graph_db = neo4j.GraphDatabaseService("http://localhost:7474/db/data/")
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//ul[@id="detailview"]/li')
        for titles in titles:
            webshop = 'Informatique'
            name = titles.select('div[@id="title"]/a/text()').extract()
            url = titles.select('div[@id="title"]/a/@href').extract()
            component = 'moederbord'
            desc = titles.select('div[@id="description"]/ul/li/text()').extract()
            price = titles.select('div[@id="price"]/text()').extract()
            #image_urls = titles.select('div[@id="image"]/a/img/@src').extract()

            namesplit = ''.join(name).split(",")
            namedb = namesplit[0]

            print "== Adding Node to database =="

            query = neo4j.CypherQuery(graph_db,
                                      "CREATE (inf_mb {webshop:{webshop}, name:{namedb}, url:{url}, desc:{desc}, price:{price}, component:{component}})"
                                      "RETURN inf_mb")

            inf_mb = query.execute(webshop=webshop, namedb=namedb, url=url, desc=desc, price=price, component=component)