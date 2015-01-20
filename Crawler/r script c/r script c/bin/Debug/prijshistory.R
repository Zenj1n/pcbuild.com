library(ggplot2)
args<-commandArgs(TRUE)
options(echo=TRUE)
Prijshistory = read.csv('E:/Repositories Git Hub/pcbuild.com/Crawler/alternate/components/case/prijsgeschiedenis.csv', header = F)
names(Prijshistory) <- c("datum","naam","prijs")

input = args[1]
var2 = gsub("_", " ", input)
var = gsub("/", " ", var2)


image = ggplot(data=Prijshistory[Prijshistory$naam == var2, ], aes(x=datum, y=prijs, group=1)) + geom_line()
ggsave(file=paste("E:/Repositories Git Hub/pcbuild.com/Crawler/afbeeldingen/",var,".jpeg", sep=""), plot=image, width=10, height=8)


