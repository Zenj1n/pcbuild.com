namespace Dashboard
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series13 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series14 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series15 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series16 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Aantal_onderdelen_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Avg_Prijzen_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.Totale_Prijzen_Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBox6 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Aantal_onderdelen_chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Avg_Prijzen_chart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Totale_Prijzen_Chart)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Location = new System.Drawing.Point(466, 14);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(155, 39);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Dashboard";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Aantal_onderdelen_chart
            // 
            chartArea1.Name = "ChartArea1";
            this.Aantal_onderdelen_chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.Aantal_onderdelen_chart.Legends.Add(legend1);
            this.Aantal_onderdelen_chart.Location = new System.Drawing.Point(86, 121);
            this.Aantal_onderdelen_chart.Name = "Aantal_onderdelen_chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "CPU";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "GPU";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "PSU";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "RAM";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Case";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Opslag";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Moederbord";
            this.Aantal_onderdelen_chart.Series.Add(series1);
            this.Aantal_onderdelen_chart.Series.Add(series2);
            this.Aantal_onderdelen_chart.Series.Add(series3);
            this.Aantal_onderdelen_chart.Series.Add(series4);
            this.Aantal_onderdelen_chart.Series.Add(series5);
            this.Aantal_onderdelen_chart.Series.Add(series6);
            this.Aantal_onderdelen_chart.Series.Add(series7);
            this.Aantal_onderdelen_chart.Size = new System.Drawing.Size(408, 189);
            this.Aantal_onderdelen_chart.TabIndex = 1;
            this.Aantal_onderdelen_chart.Text = "chart1";
            this.Aantal_onderdelen_chart.Click += new System.EventHandler(this.chart1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(212, 89);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(136, 26);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "Aantal onderdelen";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(212, 316);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(136, 26);
            this.textBox3.TabIndex = 3;
            // 
            // Avg_Prijzen_chart
            // 
            chartArea2.Name = "ChartArea1";
            this.Avg_Prijzen_chart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.Avg_Prijzen_chart.Legends.Add(legend2);
            this.Avg_Prijzen_chart.Location = new System.Drawing.Point(620, 121);
            this.Avg_Prijzen_chart.Name = "Avg_Prijzen_chart";
            this.Avg_Prijzen_chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series8.ChartArea = "ChartArea1";
            series8.Legend = "Legend1";
            series8.Name = "CPU";
            series8.YValuesPerPoint = 2;
            series9.ChartArea = "ChartArea1";
            series9.Legend = "Legend1";
            series9.Name = "GPU";
            series10.ChartArea = "ChartArea1";
            series10.Legend = "Legend1";
            series10.Name = "PSU";
            series11.ChartArea = "ChartArea1";
            series11.Legend = "Legend1";
            series11.Name = "RAM";
            series12.ChartArea = "ChartArea1";
            series12.Legend = "Legend1";
            series12.Name = "Case";
            series13.ChartArea = "ChartArea1";
            series13.Legend = "Legend1";
            series13.Name = "Opslag";
            series14.ChartArea = "ChartArea1";
            series14.Legend = "Legend1";
            series14.Name = "Moederbord";
            this.Avg_Prijzen_chart.Series.Add(series8);
            this.Avg_Prijzen_chart.Series.Add(series9);
            this.Avg_Prijzen_chart.Series.Add(series10);
            this.Avg_Prijzen_chart.Series.Add(series11);
            this.Avg_Prijzen_chart.Series.Add(series12);
            this.Avg_Prijzen_chart.Series.Add(series13);
            this.Avg_Prijzen_chart.Series.Add(series14);
            this.Avg_Prijzen_chart.Size = new System.Drawing.Size(369, 189);
            this.Avg_Prijzen_chart.TabIndex = 4;
            this.Avg_Prijzen_chart.Text = "chart1";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(689, 89);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(260, 26);
            this.textBox4.TabIndex = 5;
            this.textBox4.Text = "Gemiddelde prijzen per component";
            // 
            // Totale_Prijzen_Chart
            // 
            chartArea3.Name = "ChartArea1";
            this.Totale_Prijzen_Chart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.Totale_Prijzen_Chart.Legends.Add(legend3);
            this.Totale_Prijzen_Chart.Location = new System.Drawing.Point(366, 427);
            this.Totale_Prijzen_Chart.Name = "Totale_Prijzen_Chart";
            this.Totale_Prijzen_Chart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            series15.ChartArea = "ChartArea1";
            series15.Legend = "Legend1";
            series15.Name = "CPU";
            series15.YValuesPerPoint = 2;
            series16.ChartArea = "ChartArea1";
            series16.Legend = "Legend1";
            series16.Name = "GPU";
            series17.ChartArea = "ChartArea1";
            series17.Legend = "Legend1";
            series17.Name = "PSU";
            series18.ChartArea = "ChartArea1";
            series18.Legend = "Legend1";
            series18.Name = "RAM";
            series19.ChartArea = "ChartArea1";
            series19.Legend = "Legend1";
            series19.Name = "Case";
            series20.ChartArea = "ChartArea1";
            series20.Legend = "Legend1";
            series20.Name = "Opslag";
            series21.ChartArea = "ChartArea1";
            series21.Legend = "Legend1";
            series21.Name = "Moederbord";
            this.Totale_Prijzen_Chart.Series.Add(series15);
            this.Totale_Prijzen_Chart.Series.Add(series16);
            this.Totale_Prijzen_Chart.Series.Add(series17);
            this.Totale_Prijzen_Chart.Series.Add(series18);
            this.Totale_Prijzen_Chart.Series.Add(series19);
            this.Totale_Prijzen_Chart.Series.Add(series20);
            this.Totale_Prijzen_Chart.Series.Add(series21);
            this.Totale_Prijzen_Chart.Size = new System.Drawing.Size(405, 208);
            this.Totale_Prijzen_Chart.TabIndex = 7;
            this.Totale_Prijzen_Chart.Text = "chart1";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(478, 395);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(202, 26);
            this.textBox6.TabIndex = 8;
            this.textBox6.Text = "Totale prijs per component";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 647);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.Totale_Prijzen_Chart);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.Avg_Prijzen_chart);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.Aantal_onderdelen_chart);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Aantal_onderdelen_chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Avg_Prijzen_chart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Totale_Prijzen_Chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart Aantal_onderdelen_chart;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataVisualization.Charting.Chart Avg_Prijzen_chart;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.DataVisualization.Charting.Chart Totale_Prijzen_Chart;
        private System.Windows.Forms.TextBox textBox6;
    }
}

