using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using System.Windows.Forms;
using System.Globalization;

namespace Dashboard_HoyeLam
{
    public partial class Form1 : Form
    {
        public class componenten_model
        {
            public string naam { get; set; }
        }
                
        public Form1()
        {
            InitializeComponent();
        }

        private void componenten_button_Click(object sender, EventArgs e)
        {
           
            var client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            //Alle componenten 
            var totaal_componenten_query = client
              .Cypher
              .Match("(n)-[r:verkrijgbaar]-(p:Webshop)")
              .Return(n => new
              {
                  totaal_componenten = (int)n.Count()
              }).Results.ToArray();
            int totaal_comp = totaal_componenten_query[0].totaal_componenten;

            //Alle processoren
            var totaal_processoren_query = client
             .Cypher
             .Match("(n:processor)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_processor = (int)n.Count()
             }).Results.ToArray();
            int totaal_processoren = totaal_processoren_query[0].totaal_processor;

            //Alle Moederborden
            var totaal_moederborden_query = client
             .Cypher
             .Match("(n:moederbord)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_moederbord = (int)n.Count()
             }).Results.ToArray();
            int totaal_moederborden = totaal_moederborden_query[0].totaal_moederbord;

            //Alle Videokaarten
            var totaal_videokaarten_query = client
             .Cypher
             .Match("(n:videokaart)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_videokaart = (int)n.Count()
             }).Results.ToArray();
            int totaal_videokaarten = totaal_videokaarten_query[0].totaal_videokaart;

            //Alle Werkgeheugen
            var totaal_werkgeheugen_query = client
             .Cypher
             .Match("(n:werkgeheugen)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_ram = (int)n.Count()
             }).Results.ToArray();
            int totaal_werkgeheugen = totaal_werkgeheugen_query[0].totaal_ram;

            //Alle Behuizingen
            var totaal_behuizingen_query = client
             .Cypher
             .Match("(n:behuizing)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_behuizing = (int)n.Count()
             }).Results.ToArray();
            int totaal_behuizingen = totaal_behuizingen_query[0].totaal_behuizing;

            //Alle Opslag
            var totaal_opslag_query = client
             .Cypher
             .Match("(n:opslag)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_opslag = (int)n.Count()
             }).Results.ToArray();
            int totaal_opslagen = totaal_opslag_query[0].totaal_opslag;

            //Alle voeding
            var totaal_voedingen_query = client
             .Cypher
             .Match("(n:voeding)-[r:verkrijgbaar]-(p:Webshop)")
             .Return(n => new
             {
                 totaal_voeding = (int)n.Count()
             }).Results.ToArray();
            int totaal_voedingen = totaal_voedingen_query[0].totaal_voeding;

            //Unused componenten in onze database
            int niet_gebruikte_componenten = totaal_comp - totaal_behuizingen - totaal_moederborden
                - totaal_opslagen - totaal_processoren - totaal_videokaarten - totaal_voedingen -
                totaal_werkgeheugen;

            //
            label1.Text = "Totaal componeten in database    : " + totaal_comp.ToString();
            label2.Text = "Totaal moederborden              : " + totaal_moederborden.ToString();
            label3.Text = "Totaal processoren               : " + totaal_processoren.ToString();
            label4.Text = "Totaal videokaarten              : " + totaal_videokaarten.ToString();
            label5.Text = "Totaal werkgeheugen              : " + totaal_werkgeheugen.ToString();
            label6.Text = "Totaal behuizingen               : " + totaal_behuizingen.ToString();
            label7.Text = "Totaal opslag                    : " + totaal_opslagen.ToString();
            label8.Text = "Totaal voedingen                 : " + totaal_voedingen.ToString();
            label9.Text = "Totaal niet gebruikte componenten: " + niet_gebruikte_componenten.ToString();
            
        }

    }
}
