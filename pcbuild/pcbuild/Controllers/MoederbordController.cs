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
        // GET: Moederbord
        public ActionResult Reload(string processor, string socket, string prijs, string webshop)
        {
            socket = Regex.Replace(socket, " ", "+", RegexOptions.IgnoreCase); //Voor Sockets die een "+" hebben en URL's kunnen dat niet meegeven

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

            decimal prijs_processor = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));                          //Pak prijs van de vorige stap
            decimal prijs_moederbord = Convert.ToDecimal(moederbordprijs_cookie.Value, new CultureInfo("is-IS")); //Pak de prijs van de component van deze stap
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value,                            //Pak de vorige totale prijs van vorige stap en verminder dat met 
            new CultureInfo("is-IS")) - prijs_moederbord;                                                         //De prijs van dit component(om te voorkomen dat bij terugstap de prijs opnieuw optelt)
            decimal prijs_totaal = prijs_totaal_vorige + prijs_processor;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            moederbordprijs_cookie.Value = "0,00";                                                                 //Zet de huidige component prijs weer op 0.00 om de stap opnieuw te beginnen
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

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            //Roep Cookies aan voor de View
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie processorwebshop_cookie = new HttpCookie("processorwebshop_cookie");
            HttpCookie processorsocket_cookie = new HttpCookie("processorsocket_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            processorsocket_cookie = Request.Cookies["processorsocket_cookie"];

            string socket = processorsocket_cookie.Value;

            //Where Query voor compatible
            string socket_search = "(?i).*" + socket + ".*";
            Debug.WriteLine(socket);

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            //Query om moederborden op te halen met de juiste socket van het processor
            var componenten_query = client
           .Cypher
           .Match("(n:moederbord)-[r:verkrijgbaar]-(p:Webshop)")
           .Where("n.socket = {socket_c}")
           .WithParam("socket_c", socket)
           .Return((n, r, p) => new ViewModelMoederbord
           {
               Moederbord_all = n.As<Moederbord_Model>(),
               Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
               Webshop_all = p.As<Webshop_Model>(),
           })
          .Results;


            return View(componenten_query);
        }

    }
}