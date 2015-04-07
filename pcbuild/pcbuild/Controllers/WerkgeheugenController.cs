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
using pcbuild.Models.WerkgeheugenModels;
using System.Text.RegularExpressions;


namespace pcbuild.Controllers
{
    public class WerkgeheugenController : Controller
    {
        /// <summary>
        /// Deze methode maakt, voegt data en slaat de cookies op
        /// </summary>
        /// <param name="videokaart">naam van de videokaart</param>
        /// <param name="prijs">prijs van de videokaart</param>
        /// <param name="webshop">naam webshop van de videokaart</param>
        /// <returns></returns>
        public ActionResult Reload(string videokaart, string prijs, string webshop)
        {
            //Maak cookie arrays
            HttpCookie videokaart_cookie = new HttpCookie("videokaart_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");
            HttpCookie videokaartwebshop_cookie = new HttpCookie("videokaartwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie werkgeheugenprijs_cookie = new HttpCookie("werkgeheugenprijs_cookie");

            //Roep cookies aan 
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            werkgeheugenprijs_cookie = Request.Cookies["werkgeheugenprijs_cookie"];

            //Pak alle prijzen tot nu toe en tel ze bij elkaar op en als je terug kwam van vorige stap haal de eerdere prijs eruit.
            decimal prijs_videokaart = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_werkgeheugen = Convert.ToDecimal(werkgeheugenprijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS")) - prijs_werkgeheugen;
            decimal prijs_totaal = prijs_totaal_vorige + prijs_videokaart;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            werkgeheugenprijs_cookie.Value = "0,00";
            Response.Cookies.Add(werkgeheugenprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            videokaart_cookie.Value = videokaart;
            videokaartprijs_cookie.Value = prijs;
            videokaartwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(videokaart_cookie);
            Response.Cookies.Add(videokaartprijs_cookie);
            Response.Cookies.Add(videokaartwebshop_cookie);

            //roep de methode Index aan
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Roep de cookies aan voor de View, maak een connectie en haal data uit database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Roep de cookies aan voor de View
            HttpCookie videokaart_cookie = new HttpCookie("videokaart_cookie");
            HttpCookie videokaartprijs_cookie = new HttpCookie("videokaartprijs_cookie");
            HttpCookie videokaartwebshop_cookie = new HttpCookie("videokaartwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie moederbordddr_cookie = new HttpCookie("moederbordddr_cookie");  
  
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];                    
            moederbordddr_cookie = Request.Cookies["moederbordddr_cookie"];

            //Variable om de juister werkgeheugen te halen door de juiste DDR-slot van het moederbord
            string ddr = moederbordddr_cookie.Value;

            //Connectie met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data/"));
            client.Connect();

            // Query om werkgeheugen op te halen met de juiste DDR geheugen van het moederbord
            var componenten_query = client
              .Cypher
              .Match("(n:werkgeheugen)-[r:verkrijgbaar]-(p:Webshop)")
              .Where("n.ddr = {ddr_query}")
              .WithParam("ddr_query", ddr)
              .Return((n, r, p) => new ViewModelWerkgeheugen
              {
                  Werkgeheugen_all = n.As<Werkgeheugen_Model>(),
                  Verkrijgbaar_all = r.As<Verkrijgbaar_Model>(),
                  Webshop_all = p.As<Webshop_Model>(),
              })
              .Limit(100)
              .Results;
            
            //return naar de View en stuur componenten_query mee
            return View(componenten_query);          
        }
    }
}