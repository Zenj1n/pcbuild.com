# -*- coding: utf-8 -*-

# Scrapy settings for Hardware project
#
# For simplicity, this file contains only the most important settings by
# default. All the other settings are documented here:
#
#     http://doc.scrapy.org/en/latest/topics/settings.html
#

BOT_NAME = 'Hardware'

SPIDER_MODULES = ['Hardware.spiders']
NEWSPIDER_MODULE = 'Hardware.spiders'
ITEM_PIPELINES = {'scrapy.contrib.pipeline.images.ImagesPipeline': 1}
#IMAGES_STORE = 'G:\Users\hoye\Documents\PCBuild\Python-Crawler\Hardware\Images'
#IMAGES_EXPIRES = 90
DEPTH_LIMIT = 10
DOWNLOAD_DELAY = 0.25
# Crawl responsibly by identifying yourself (and your website) on the user-agent
#USER_AGENT = 'Hardware (+http://www.yourdomain.com)'
