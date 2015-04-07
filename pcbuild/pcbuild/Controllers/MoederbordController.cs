using Neo4jClient;
using pcbuild.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections;
using Neo4jClient.Cypher;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using pcbuild.Models.MoederbordModels;
using System.Globalization;
using pcbuild.Models.ProcessorModels;
using System.Text.RegularExpressions;

namespace pcbuild.Controllers
{

    public class MoederbordController : Controller
    {
       /// <summary>
        ///Deze methode maakt, voegt data en slaat de cookies op
       /// </summary>
       /// <param name="processor"> Processor naam van de vorige stap </param>
        /// <param name="socket">Processor socket naam van de vorige stap</param>
        /// <param name="prijs">Processor prijs van de vorige stap</param>
        /// <param name="webshop">Webshop naam van de processor van de vorige stap</param>
       /// <returns></returns>
        public ActionResult Reload(string processor, string socket, string prijs, string webshop)
        {
            //Verander de socket strings voor URL gebruik, want de URL neemt geen spaties mee.
            socket = Regex.Replace(socket, " ", "+", RegexOptions.IgnoreCase);
            prijs = Regex.Replace(prijs, "€", "", RegexOptions.IgnoreCase);

            //Roep Cookies aan
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie processorwebshop_cookie = new HttpCookie("processorwebshop_cookie");
            HttpCookie processorsocket_cookie = new HttpCookie("processorsocket_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie moederbordprijs_cookie = new HttpCookie("moederbordprijs_cookie");

            //Stop de cookies data in een variable
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            moederbordprijs_cookie = Request.Cookies["moederbordprijs_cookie"];

            //Pak alle prijzen tot nu toe en tel ze bij elkaar op en als je terug kwam van vorige stap haal de eerdere prijs eruit.
            decimal prijs_processor = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));                          
            decimal prijs_moederbord = Convert.ToDecimal(moederbordprijs_cookie.Value, new CultureInfo("is-IS")); 
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value,                             
            new CultureInfo("is-IS")) - prijs_moederbord;                                                         
            decimal prijs_totaal = prijs_totaal_vorige + prijs_processor;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            moederbordprijs_cookie.Value = "0,00";                              
            Response.Cookies.Add(moederbordprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            processor_cookie.Value = processor;
            processorprijs_cookie.Value = prijs;
            processorwebshop_cookie.Value = webshop;
            processorsocket_cookie.Value = socket;

            //save the cookies!!!
            Response.Cookies.Add(processor_cookie);
            Response.Cookies.Add(processorprijs_cookie);
            Response.Cookies.Add(processorwebshop_cookie);
            Response.Cookies.Add(processorsocket_cookie);            
            Response.Cookies.Add(totale_prijs_cookie);

            //Roep de Index methode aan
            return RedirectToAction("Index");
        }

        /// <summary>
        ///  Deze methode maakt connectie met de NEO4J database, haalt data uit de NEO4J database 
        ///  en roept de cookies aan die wij gaan gebruiken
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Roep Cookies aan voor de View
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie processorwebshop_cookie = new HttpCookie("processorwebshop_cookie");
            HttpCookie processorsocket_cookie = new HttpCookie("processorsocket_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            processorsocket_cookie = Request.Cookies["processorsocket_cookie"];

            //Variables van de socket van de processor voor de WHERE query 
            string socket = processorsocket_cookie.Value;
            string ddr_mb = ".*.*";
                  
            //Connectie met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            //Query om moederborden op te halen met de juiste socket van het processor
            var componenten_query = client
           .Cypher
           .Match("(n:moederbord)-[r:verkrijgbaar]-(p:Webshop)")
           .Where("n.socket = {socket_c}")
           .AndWhere("n.ddr =~ {ddr_mb}")
           .WithParam("socket_c", socket)
           .WithParam("ddr_mb", ddr_mb)
           .Return((n, r, p) => new ViewModelMoederbord
           {
               Moederbord_all = n.As<Moederbord_Model>(),
               Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
               Webshop_all = p.As<Webshop_Model>(),
           })
          .Results;

            //return naar de View en neem componenten_query mee
            return View(componenten_query);
        }

    }
}