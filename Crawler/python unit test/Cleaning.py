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

    def clean_socket(self, socket):
        clean_socket0 = socket.replace("[","")
        clean_socket1 = clean_socket0.replace("]","")
        clean_socket2 = clean_socket1.replace("(Intel)","")
        clean_socket3 = clean_socket2.replace("(AMD)","")
        clean_socket4 = clean_socket3.replace("Socket","")
        clean_socket5 = clean_socket4.strip()
        return clean_socket5






