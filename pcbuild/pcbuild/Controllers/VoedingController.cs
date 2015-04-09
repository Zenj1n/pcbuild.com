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
using pcbuild.Models.VoedingModels;


namespace pcbuild.Controllers
{
    public class VoedingController : Controller
    {
        /// <summary>
        /// Deze methode slaat maakt, voegt data toe en slaat de cookies op
        /// </summary>
        /// <param name="opslag">naam opslag</param>
        /// <param name="prijs">prijs opslag</param>
        /// <param name="webshop"naam webshop van de opslag></param>
        /// <returns></returns>
        public ActionResult Reload(string opslag, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie opslag_cookie = new HttpCookie("opslag_cookie");
            HttpCookie opslagprijs_cookie = new HttpCookie("opslagprijs_cookie");
            HttpCookie opslagwebshop_cookie = new HttpCookie("opslagwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie voedingprijs_cookie = new HttpCookie("voedingprijs_cookie");

            //Roep huide componenten prijs op 
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            voedingprijs_cookie = Request.Cookies["voedingprijs_cookie"];

            //Pak alle prijzen tot nu toe en tel ze bij elkaar op en als je terug kwam van vorige stap haal de eerdere prijs eruit.
            decimal prijs_opslag = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_voeding = Convert.ToDecimal(voedingprijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS")) - prijs_voeding;
            decimal prijs_totaal = prijs_totaal_vorige + prijs_opslag;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            voedingprijs_cookie.Value = "0,00";
            Response.Cookies.Add(voedingprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            opslag_cookie.Value = opslag;
            opslagprijs_cookie.Value = prijs;
            opslagwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(opslag_cookie);
            Response.Cookies.Add(opslagprijs_cookie);
            Response.Cookies.Add(opslagwebshop_cookie);

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Roep de cookies aan voor de View, maak een connectie en haal data uit database.
        /// </summary>
        /// <returns>return data van de database in componenten_query naar de view </returns>
        public ActionResult Index()
        {
            //Roep de cookies aan voor de View
            HttpCookie opslag_cookie = new HttpCookie("opslag_cookie");
            HttpCookie opslagprijs_cookie = new HttpCookie("opslagprijs_cookie");
            HttpCookie opslagwebshop_cookie = new HttpCookie("opslagwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            //Connectie met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            // Query om voeding op te halen
            var componenten_query = client
              .Cypher
              .Match("(n:voeding)-[r:verkrijgbaar]-(p:Webshop)")
              .Return((n, r, p) => new ViewModelVoeding
              {
                  Voeding_all = n.As<Voeding_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Results;

            //Stuur ze naar de view
            return View(componenten_query);
        }
    }
}