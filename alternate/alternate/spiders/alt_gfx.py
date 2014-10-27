import scrapy

from scrapy.contrib.spiders import CrawlSpider, Rule
from scrapy.contrib.linkextractors.sgml import SgmlLinkExtractor
from scrapy.selector import HtmlXPathSelector


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

    def parse_start_url(self,response):
        hxs = HtmlXPathSelector(response)
        titles = hxs.select('//div[@class="listRow"]')
        items = []
        for titles in titles:
           item = AlternateItem()
           item['webshop'] = 'alternate.nl'
           item['name'] = titles.select('a[@class="productLink"]/span[@class="product"]/span[@class="pic"]/@title').extract()
           item['url'] = titles.select('a[@class="productLink"]/@href').extract()
           item['desc'] = titles.select('a[@class="productLink"]/span[@class="info"]/text()').extract()
           item['price'] = titles.select('div[@class= "waresSum"]/p/span[@class = "price right right10"]/text()').extract()
           item['image_urls'] = 'placeholder'
           items.append(item)
        return (items)