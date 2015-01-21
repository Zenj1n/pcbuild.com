__author__ = 'Fabian'

class Cleaning():

    def clean_name(self, naam):
        clean_naam0 = naam.replace("[","")
        clean_naam1 = clean_naam0.replace("]","")
        clean_naam2 = clean_naam1.replace("/","")
        return clean_naam2


