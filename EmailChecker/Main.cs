using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;
using System.IO;
using System.Web;
using HAP = HtmlAgilityPack;
using WatiN.Core;

namespace EmailChecker
{
    public partial class Main : System.Windows.Forms.Form
    {
        private String currentSearchedMail;
        private String foundLink;
        private String sheetName;

        private IE ie;
        private bool found;

        private bool completed;

        public Main()
        {
            InitializeComponent();
            //WatiN.Core.Settings.MakeNewIeInstanceVisible = false;
            ie = new IE();
            found = false;
        }

        private void ChooseFileAction(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter += "Excel Files (*.xlsx, *.xls, *.csv)|*.xlsx;*.xls;*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = ofd.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcess_Click(object sender, EventArgs e)
        {
            ExcelUtlity util = new ExcelUtlity();
            DataTable dt = util.GetWorkSheet(txtPath.Text, 1);
            DataTable newDt = new DataTable();
            newDt.Columns.Add("LinkedIn");
            Console.WriteLine(dt.Rows.Count);
            progressBar1.Maximum = dt.Rows.Count;
            // <b>i</b> starts at 1 since we want to skip the header.
            //for (int i = 1; i < dt.Rows.Count; i++)
            for (int i = 1; i < 2; i++)
            {
                DataRow dRow = dt.Rows[i];
                DataRow newRow = newDt.NewRow();
                String firstName = dRow[0].ToString();
                String lastName = dRow[1].ToString();

                if (String.IsNullOrEmpty(dRow[2].ToString()))
                {
                    if (String.IsNullOrEmpty(firstName) && String.IsNullOrEmpty(lastName))
                    {
                        continue;
                    }
                    else
                    {
                        dRow[7] = LinkedInSearchV2(Uri.EscapeDataString(firstName), lastName);
                    }
                }
                else
                {
                    //dRow[7] = LinkedInSearchV2("Nisa", "Amoils");
                    //newRow[0] = dRow[6] = Search(dRow[2].ToString());
                    //newRow[1] = dRow[5] = found ? "Y" : "N";
                    //newRow[0] = LinkedInSearchV2(Uri.EscapeDataString(firstName), lastName);
                    newRow[0] = LinkedInSearchV2("Nisa", "Amoils");
                    //dRow[6] = Search("ajamiscosa@gmail.com");

                    //Search("ajamiscosa@gmail.com");
                }

                progressBar1.Value = i-1;

                Thread.Sleep(10000);
            }
            util.WriteDataTableToExcel(newDt, util.SheetName, @"D:\out.xlsx");
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private String LinkedInSearchV2(String firstName, String lastName)
        {
            String url = String.Format("", firstName, lastName);
            Console.WriteLine(url);
            ie.ClearCache();
            ie.ClearCookies();
            ie.GoTo(url);
            ie.WaitForComplete();
            String location = String.Empty;
            Console.WriteLine(ie.Url);
            if (ie.Url.Equals(url)) // no redirect. no single profile found.
            {
                try
                {
                    HAP.HtmlDocument document = new HAP.HtmlDocument();
                    document.LoadHtml(ie.Body.InnerHtml);
                    Console.WriteLine(ie.Body.InnerHtml);
                    foreach (HAP.HtmlNode link in document.DocumentNode.SelectNodes("//div[contains(@class, 'profile-card')]"))
                    {
                        HAP.HtmlDocument dt = new HAP.HtmlDocument();
                        dt.LoadHtml(link.InnerHtml);
                        HAP.HtmlNode node = dt.DocumentNode.SelectSingleNode("//dd");
                        location = node.InnerHtml;
                    }
                }
                catch (Exception ex)
                {
                    location = "N/A";
                }
            }
            else
            {
                try
                {
                    HAP.HtmlDocument document = new HAP.HtmlDocument();
                    document.LoadHtml(ie.Body.InnerHtml);

                    HAP.HtmlNode node = document.DocumentNode.SelectSingleNode("//span[contains(@class, 'locality')]");
                    location = node.InnerHtml;
                }
                catch (Exception ex)
                {
                    location = "N/A";
                }
            }

            Console.WriteLine("{0} {1}: {2}", firstName, lastName, location);
            return location;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private String Search(String email)
        {
            foundLink = String.Empty;
            currentSearchedMail = email;
            WebBrowser browser = new WebBrowser();
            browser.DocumentCompleted += Browser_DocumentCompleted;
            browser.Navigate(String.Format("https://www.google.com/search?q={0}", email));

            while (!completed)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }
            completed = false;
            Console.Write("\n\nDone with it!\n\n");
            return foundLink;
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = (WebBrowser)sender;
            String[] emailElements = currentSearchedMail.Split('@');

            HtmlAgilityPack.HtmlDocument document = new HAP.HtmlDocument();

            String html = browser.Document.Body.InnerHtml;
            document.LoadHtml(html);

            Console.WriteLine(html.Length > 0?"OK":"Nope");


            foreach (HAP.HtmlNode link in document.DocumentNode.SelectNodes("//div[contains(@class, 'g')]"))
            {
                if (link.InnerHtml.Contains(String.Format("<em>{0}</em>@<em>{1}</em>", emailElements[0], emailElements[1])))
                {
                    html = link.InnerHtml;
                }
            }

            try
            {
                document.LoadHtml(html);

                foreach (HAP.HtmlNode link in document.DocumentNode.SelectNodes("//cite"))
                {
                    foundLink = link.InnerText;
                }

                Console.WriteLine("Loading complete");
                Console.WriteLine("Found");

                completed = true;
                found = true;
            }
            catch (NullReferenceException ex)
            {
                foundLink = "Not found";
                Console.WriteLine("Not Found");
                completed = true;
            }
        }
    }
}
