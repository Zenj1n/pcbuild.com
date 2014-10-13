import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


from Informatique.items import  InformatiqueItem

class CasesSpider(CrawlSpider):
    name = "INF_MOTHERBOARD"
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
    
   #rules = (Rule (SgmlLinkExtractor(restrict_xpaths=('//a[contains(., "Volgende")]',))
   # , callback="parse_start_url", follow= True),
   #)
    
    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//ul[@id="detailview"]/li')
        items = []
        for titles in titles:
           item = InformatiqueItem()
           item['webshop'] = 'Informatique'
           item['name'] = titles.select('div[@id="title"]/a/text()').extract()
           item['url'] = titles.select('div[@id="title"]/a/@href').extract()
           item['desc'] = titles.select('div[@id="description"]/ul/li/text()').extract()
           item['price'] = titles.select('div[@id="price"]/text()').extract()
           item['image_urls'] = titles.select('div[@id="image"]/a/img/@src').extract()
           items.append(item)
        return (items)