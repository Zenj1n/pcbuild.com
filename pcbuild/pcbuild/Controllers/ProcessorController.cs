using Neo4jClient;
using pcbuild.Models;
using pcbuild.Models.ProcessorModels;
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

namespace pcbuild.Controllers
{
    public class ProcessorController : Controller
    {
        static ViewModelProcessor test = new ViewModelProcessor();

        // GET: Processor
        public ActionResult Index()
        {
            //Maak cookie arrays
            HttpCookie processor_cookie = new HttpCookie("processor_cookie");
            HttpCookie moederbord_cookie = new HttpCookie("moederbord_cookie");
            HttpCookie videokaart_cookie = new HttpCookie("videokaart_cookie");
            HttpCookie werkgeheugen_cookie = new HttpCookie("werkgeheugen_cookie");
            HttpCookie behuizing_cookie = new HttpCookie("behuizing_cookie");
            HttpCookie opslag_cookie = new HttpCookie("opslag_cookie");
            HttpCookie voeding_cookie = new HttpCookie("voeding_cookie");

            HttpCookie processorprijs_cookie = new HttpCookie("processorprijs_cookie");
            HttpCookie moederbordprijs_cookie = new HttpCookie("moederbordprijs_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");
            HttpCookie werkgeheugenprijs_cookie = new HttpCookie("werkgeheugenprijs_cookie");
            HttpCookie behuizingprijs_cookie = new HttpCookie("behuizingprijs_cookie");
            HttpCookie opslagprijs_cookie = new HttpCookie("opslagprijs_cookie");
            HttpCookie voedingprijs_cookie = new HttpCookie("voedingprijs_cookie");

            //voeg data toe aan cookies
            processor_cookie.Value = "Geen processor toegevoegd.";
            moederbord_cookie.Value = "Geen moederbord toegevoegd.";
            videokaart_cookie.Value = "Geen videokaart toegevoegd.";
            werkgeheugen_cookie.Value = "Geen werkgeheugen toegevoegd.";
            behuizing_cookie.Value = "Geen behuizing toegevoegd.";
            opslag_cookie.Value = "Geen opslag toegevoegd.";
            voeding_cookie.Value = "Geen voeding toegevoegd.";

            processorprijs_cookie.Value = "€ 0,-";
            moederbordprijs_cookie.Value = "€ 0,-";
            videokaartprijs_cookie.Value = "€ 0,-"; ;
            werkgeheugenprijs_cookie.Value = "€ 0,-";
            behuizingprijs_cookie.Value = "€ 0,-";
            opslagprijs_cookie.Value = "€ 0,-";
            voedingprijs_cookie.Value = "€ 0,-";

            //save the cookies!!!
            Response.Cookies.Add(processor_cookie);
            Response.Cookies.Add(moederbord_cookie);
            Response.Cookies.Add(videokaart_cookie);
            Response.Cookies.Add(werkgeheugen_cookie);
            Response.Cookies.Add(behuizing_cookie);
            Response.Cookies.Add(opslag_cookie);
            Response.Cookies.Add(voeding_cookie);

            Response.Cookies.Add(processorprijs_cookie);
            Response.Cookies.Add(moederbordprijs_cookie);
            Response.Cookies.Add(videokaartprijs_cookie);
            Response.Cookies.Add(werkgeheugenprijs_cookie);
            Response.Cookies.Add(behuizingprijs_cookie);
            Response.Cookies.Add(opslagprijs_cookie);
            Response.Cookies.Add(voedingprijs_cookie);

            //Connectie met database
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            // Query om alle processoren op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:processor)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelProcessor
              {
                  Proccesor_m = n.As<Processor_Model>(),
                  Verkrijgbaar_m = r.As<Verkrijgbaar_Model>(),
                  Webshop_m = p.As<Webshop_Model>(),
              })
              .Results;

            return View(componenten_query);
        }

        public ActionResult Voeg_Toe_Processor(String naam_lijst)
        {
            return null;
        }

    }
}