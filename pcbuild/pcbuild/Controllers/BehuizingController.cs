﻿using Neo4jClient;
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
    public class BehuizingController : Controller
    {
        /// <summary>
        /// Deze methode maakt, voegt data en slaat de cookies op
        /// </summary>
        /// <param name="werkgeheugen">naam van het werkgeheugen</param>
        /// <param name="prijs">prijs van het werkgeheugen</param>
        /// <param name="webshop">naam webshop van het werkgeheugen</param>
        /// <returns></returns>
        public ActionResult Reload(string werkgeheugen, string prijs, string webshop)
        {
            //Roep cookie arrays
            HttpCookie werkgeheugen_cookie = new HttpCookie("werkgeheugen_cookie");
            HttpCookie werkgeheugenprijs_cookie = new HttpCookie("werkgeheugenprijs_cookie");
            HttpCookie werkgeheugenwebshop_cookie = new HttpCookie("werkgeheugenwebshop_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");
            HttpCookie behuizingprijs_cookie = new HttpCookie("behuizingprijs_cookie");

            //Roep prijs cookies aan
            totale_prijs_cookie = Request.Cookies["totale_prijs_cookie"];
            behuizingprijs_cookie = Request.Cookies["behuizingprijs_cookie"];

            //Pak alle prijzen tot nu toe en tel ze bij elkaar op en als je terug kwam van vorige stap haal de eerdere prijs eruit.
            decimal prijs_werkgeheugen = Convert.ToDecimal(prijs, new CultureInfo("is-IS"));
            decimal prijs_behuizing = Convert.ToDecimal(behuizingprijs_cookie.Value, new CultureInfo("is-IS"));
            decimal prijs_totaal_vorige = Convert.ToDecimal(totale_prijs_cookie.Value, new CultureInfo("is-IS")) - prijs_behuizing;
            decimal prijs_totaal = prijs_totaal_vorige + prijs_werkgeheugen;
            string prijs_totaal_string = prijs_totaal.ToString();
            totale_prijs_cookie.Value = prijs_totaal_string;
            behuizingprijs_cookie.Value = "0,00";
            Response.Cookies.Add(behuizingprijs_cookie);
            Response.Cookies.Add(totale_prijs_cookie);

            //voeg data toe aan cookies
            werkgeheugen_cookie.Value = werkgeheugen;
            werkgeheugenprijs_cookie.Value = prijs;
            werkgeheugenwebshop_cookie.Value = webshop;

            //save the cookies!!!
            Response.Cookies.Add(werkgeheugen_cookie);
            Response.Cookies.Add(werkgeheugenprijs_cookie);
            Response.Cookies.Add(werkgeheugenwebshop_cookie);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Roep de cookies aan voor de View, maak een connectie en haal data uit database.
        /// </summary>
        /// <returns>return data van de database in componenten_query naar de view</returns>
        public ActionResult Index()
        {
            //Roep de cookies aan voor de View
            HttpCookie werkgeheugen_cookie = new HttpCookie("werkgeheugen_cookie");
            HttpCookie werkgeheugenprijs_cookie = new HttpCookie("werkgeheugenprijs_cookie");
            HttpCookie werkgeheugenwebshop_cookie = new HttpCookie("werkgeheugenwebshop_cookie");
            HttpCookie moederbordvormfactor_cookie = new HttpCookie("moederbordvormfactor_cookie");
            HttpCookie totale_prijs_cookie = new HttpCookie("totale_prijs_cookie");

            //Haal de cookies op om te matchen
            moederbordvormfactor_cookie = Request.Cookies["moederbordvormfactor_cookie"];

            //Maak er een query van om te zoeken in onze database
            string vormfactor = moederbordvormfactor_cookie.Value;  // moederbord vormfactor voor matchen
            string vormfactor_search = "(?i)" + vormfactor[0] + vormfactor[1] + vormfactor[2] + ".*";

            //Connectie met database
            var client = new GraphClient(new Uri("http://Horayon:Zenjin@localhost:8080/db/data"));
            client.Connect();

            // Query om behuizingen op te halen met de juiste vormfactor die wij kregen van moederbord
            var componenten_query = client
              .Cypher
              .Match("(n:behuizing)-[r:verkrijgbaar]-(p:Webshop)")
              .Where("n.vormfactor =~ {vormfactor_c}")
              .WithParam("vormfactor_c", vormfactor_search)
              .Return((n, r, p) => new ViewModelBehuizing
              {
                  Behuizing_test = n.As<Behuizing_Model>(),
                  Verkrijgbaar_test = r.As<Verkrijgbaar_Model>(),
                  Webshop_test = p.As<Webshop_Model>(),
              })
              .Results;

                return View(componenten_query);
            }

        }
}
