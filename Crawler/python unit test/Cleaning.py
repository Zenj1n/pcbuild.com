__author__ = 'Fabian'

class Cleaning():

    def clean_name(self, naam):
        clean_naam0 = naam.replace("[","")
        clean_naam1 = clean_naam0.replace("]","")
        clean_naam2 = clean_naam1.replace("/","")
        return clean_naam2

    def clean_price(self, price):
        clean_price0 = price.replace("[","")
        clean_price1 = clean_price0.replace("]","")
        clean_price2 = clean_price1.replace("/","")
        clean_price3 = clean_price2.replace("-","00")
        clean_price4 = clean_price3.strip()
        return clean_price4

