# -*- coding: utf-8 -*-

# Scrapy settings for alternate project
#
# For simplicity, this file contains only the most important settings by
# default. All the other settings are documented here:
#
#     http://doc.scrapy.org/en/latest/topics/settings.html
#

BOT_NAME = 'alternate'

SPIDER_MODULES = ['alternate.spiders']
NEWSPIDER_MODULE = 'alternate.spiders'
#IMAGES_STORE = 'E:\Repositories Git Hub\Python-Crawler\alternate\images'
#IMAGES_EXPIRES = 90
RANDOMIZE_DOWNLOAD_DELAY = True


# Crawl responsibly by identifying yourself (and your website) on the user-agent
#USER_AGENT = 'alternate (+http://www.yourdomain.com)'
