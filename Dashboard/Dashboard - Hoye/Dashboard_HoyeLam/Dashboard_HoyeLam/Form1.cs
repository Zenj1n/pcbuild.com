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
            this.chart2.Visible = false;

            label1.Text = ":";
            label2.Text = ":";
            label3.Text = ":";
            label4.Text = ":";
            label5.Text = ":";
            label6.Text = ":";
            label7.Text = ":";
            label8.Text = ":";
            label9.Text = ":";
            try
            {
                var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data/"));
                client.Connect();

                //Alle componenten 
                var totaal_componenten_query = client
                  .Cypher
                  .Match("(n)")
                  .Return(n => new
                  {
                      totaal_componenten = (int)n.Count()
                  }).Results.ToArray();
                int totaal_comp = totaal_componenten_query[0].totaal_componenten;

                //Alle processoren
                var totaal_processoren_query = client
                 .Cypher
                 .Match("(n:processor)")
                 .Return(n => new
                 {
                     totaal_processor = (int)n.Count()
                 }).Results.ToArray();
                int totaal_processoren = totaal_processoren_query[0].totaal_processor;

                //Alle Moederborden
                var totaal_moederborden_query = client
                 .Cypher
                 .Match("(n:moederbord)")
                 .Return(n => new
                 {
                     totaal_moederbord = (int)n.Count()
                 }).Results.ToArray();
                int totaal_moederborden = totaal_moederborden_query[0].totaal_moederbord;

                //Alle Videokaarten
                var totaal_videokaarten_query = client
                 .Cypher
                 .Match("(n:videokaart)")
                 .Return(n => new
                 {
                     totaal_videokaart = (int)n.Count()
                 }).Results.ToArray();
                int totaal_videokaarten = totaal_videokaarten_query[0].totaal_videokaart;

                //Alle Werkgeheugen
                var totaal_werkgeheugen_query = client
                 .Cypher
                 .Match("(n:werkgeheugen)")
                 .Return(n => new
                 {
                     totaal_ram = (int)n.Count()
                 }).Results.ToArray();
                int totaal_werkgeheugen = totaal_werkgeheugen_query[0].totaal_ram;

                //Alle Behuizingen
                var totaal_behuizingen_query = client
                 .Cypher
                 .Match("(n:behuizing)")
                 .Return(n => new
                 {
                     totaal_behuizing = (int)n.Count()
                 }).Results.ToArray();
                int totaal_behuizingen = totaal_behuizingen_query[0].totaal_behuizing;

                //Alle Opslag
                var totaal_opslag_query = client
                 .Cypher
                 .Match("(n:opslag)")
                 .Return(n => new
                 {
                     totaal_opslag = (int)n.Count()
                 }).Results.ToArray();
                int totaal_opslagen = totaal_opslag_query[0].totaal_opslag;

                //Alle voeding
                var totaal_voedingen_query = client
                 .Cypher
                 .Match("(n:voeding)")
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

                this.chart1.Visible = true;

                this.chart1.Series["componenten"].Points.AddXY("Totaal", totaal_comp);
                this.chart1.Series["componenten"].Points.AddXY("Moederbord", totaal_moederborden);
                this.chart1.Series["componenten"].Points.AddXY("Processor", totaal_processoren);
                this.chart1.Series["componenten"].Points.AddXY("videokaarten", totaal_videokaarten);
                this.chart1.Series["componenten"].Points.AddXY("werkgeheugen", totaal_werkgeheugen);
                this.chart1.Series["componenten"].Points.AddXY("behuizing", totaal_behuizingen);
                this.chart1.Series["componenten"].Points.AddXY("voeding", totaal_voedingen);
                this.chart1.Series["componenten"].Points.AddXY("Ongebruikt", niet_gebruikte_componenten);

                this.chart3.Visible = true;
                this.chart3.Series["componenten"].Points.AddXY("Moederbord", totaal_moederborden);
                this.chart3.Series["componenten"].Points.AddXY("Processor", totaal_processoren);
                this.chart3.Series["componenten"].Points.AddXY("Videokaarten", totaal_videokaarten);
                this.chart3.Series["componenten"].Points.AddXY("werkgeheugen", totaal_werkgeheugen);
                this.chart3.Series["componenten"].Points.AddXY("behuizing", totaal_behuizingen);
                this.chart3.Series["componenten"].Points.AddXY("voeding", totaal_voedingen);
                this.chart3.Series["componenten"].Points.AddXY("Ongebruikt", niet_gebruikte_componenten);
            }
            catch (AggregateException server_error)
            {
                this.label1.Text = server_error.ToString();
                this.label2.Text = "SERVER IS OFFLINE, SORRY VOOR HET ONGEMAK";
            }
        }

        public class inf_prijzen
        {
            public string prijs { get; set; }
        }

        public class alt_prijzen
        {
            public string prijs { get; set; }
        }
        private void prijs_webshop_button_Click(object sender, EventArgs e)
        {
            //Connectie met database

            try
            {
                var client = new GraphClient(new Uri("http://Horayon:Zenjin@145.24.222.155:8080/db/data/"));
                client.Connect();

                this.chart1.Visible = false;
                this.chart3.Visible = false;

                label1.Text = ":";
                label2.Text = ":";
                label3.Text = ":";
                label4.Text = ":";
                label5.Text = ":";
                label6.Text = ":";
                label7.Text = ":";
                label8.Text = ":";
                label9.Text = ":";

                //Haal alle prijzen van informatique op
                string informatique = "Informatique";
                string informatique2 = "Informatique.nl";
                var componenten_query = client
                .Cypher
                .Match("(n)-[r:verkrijgbaar]-(p:Webshop)")
                .Where("p.naam = {inf}")
                .OrWhere("p.naam = {inf2}")
                .WithParam("inf2", informatique2)
                .WithParam("inf", informatique)
                .Return(r => r.As<inf_prijzen>())
                .Results.ToList();

                //Bereken de gemiddelde prijs
                string prijs_avg_inf = "00,00";
                decimal prijs_informatique = Convert.ToDecimal(prijs_avg_inf, new CultureInfo("is-IS"));

                foreach (var item in componenten_query)
                {
                    decimal prijs_inf = Convert.ToDecimal(item.prijs, new CultureInfo("is-IS"));
                    prijs_informatique = prijs_inf + prijs_informatique;
                }
                prijs_informatique = prijs_informatique / componenten_query.Count();
                Math.Round(prijs_informatique, 2);
                this.label1.Text = "Gemiddelde prijs van Informatique: " + prijs_informatique.ToString();
                this.label2.Text = "Aantal gemeten componenten van Informatique: " + componenten_query.Count();

                //Haal alle prijzen van alternate op bereken gemiddelde prijs
                string alternate = "alternate.nl";
                var componenten_query2 = client
                .Cypher
                .Match("(n)-[r:verkrijgbaar]-(p:Webshop)")
                .Where("p.naam = {alt}")
                .WithParam("alt", alternate)
                .Return(r => r.As<alt_prijzen>())
                .Results.ToList();

                string prijs_avg_alt = "00,00";
                decimal prijs_alternate = Convert.ToDecimal(prijs_avg_alt, new CultureInfo("is-IS"));

                foreach (var item in componenten_query2)
                {
                    try
                    {
                        decimal prijs_alt = Convert.ToDecimal(item.prijs, new CultureInfo("is-IS"));
                        prijs_alternate = prijs_alt + prijs_alternate;
                    }
                    catch (FormatException eze)
                    {
                        // do nothing
                    }
                }
                prijs_alternate = prijs_alternate / componenten_query.Count();
                Math.Round(prijs_alternate, 2);
                this.label3.Text = "Gemiddelde prijs van Alternate: " + prijs_alternate.ToString();
                this.label4.Text = "Aantal gemeten componenten van Alternate: " + componenten_query2.Count();


                //Bereken gemiddle prijs van alle webshops
                decimal prijs_alle_webshops = prijs_alternate + prijs_informatique;
                prijs_alle_webshops = prijs_alle_webshops / 2;
                Math.Round(prijs_alle_webshops, 2);
                this.label5.Text = "Gemddle prijs van alle Webshops" + prijs_alle_webshops.ToString();

                this.chart2.Visible = true;

                this.chart2.Series["Alternate"].Points.AddXY(System.DateTime.Now, prijs_alternate);
                this.chart2.Series["Informatique"].Points.AddXY(System.DateTime.Now, prijs_informatique);
                this.chart2.Series["Alle Webshops"].Points.AddXY(System.DateTime.Now, prijs_alle_webshops);
            }
            catch (AggregateException server_error)
            {
                this.label1.Text = server_error.ToString();
                this.label2.Text = "SERVER IS OFFLINE, SORRY VOOR HET ONGEMAK";
            }
        }
    }
}
