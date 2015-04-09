using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Diagnostics;

namespace PCBuild.nl_Installer
{
    public partial class Download_rstudio : Form
    {
        // global references
        WebClient webClient;
        Stopwatch sw = new Stopwatch();

        public Download_rstudio()
        {
            InitializeComponent();
        }

        public void DownloadFile(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            labelSpeed.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar1.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            labelPerc.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            labelDownloaded.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                MessageBox.Show("Download is gestopt.");
            }
            else
            {
                MessageBox.Show("Download voltooid!");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlAddress"></param>
        /// <param name="location"></param>
        public void DownloadScript(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(scriptCompleted);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(scriptProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void scriptProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            //labelSpeed.Text = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            //progressBar1.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            labelPerc2.Text = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            //labelDownloaded.Text = string.Format("{0} MB's / {1} MB's",
            //    (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
            //    (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void scriptCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            //sw.Reset();

            //if (e.Cancelled == true)
            //{
            //    MessageBox.Show("Download is gestopt.");
            //}
            //else
            //{
            //    MessageBox.Show("Download voltooid!");
            //}
        }

        //download bestand
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            DownloadFile("http://cran.r-project.org/bin/windows/base/R-3.1.3-win.exe", @"C:\PCBuild.nl\R.exe");
            //DownloadScript("https://dl-web.dropbox.com/get/public/script2.bat?_subject_uid=126566317&w=AABOdVClX2s2ggwSkG8DibVpz8Cd6WnbV4RE_p-6OO8e5w&dl=1", @"C:\PCBuild.nl\script2.bat");
        }

        // sluit het venster
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                webClient.CancelAsync();
            }
            catch
            {
                // nothing
            }
            this.Close();
        }
    }
}
