﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace EmailChecker.v2
{
    public partial class Main : Form
    {
        private String _foundUrl;
        private ChromeDriver _driver;

        public Main()
        {
            Environment.SetEnvironmentVariable("webdriver.chrome.driver",
                String.Format(@"{0}\chromedriver.exe", System.Environment.CurrentDirectory));

            InitializeComponent();

            MessageBox.Show("Please make sure the LinkedIn account that you will be using is an actual LinkedIn account. This tool will not be able to process records without a working LinkedIn account.","Warning!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LoadEvent(object sender, EventArgs e)
        {
        }

        private void LoginToLinkedIn(String username, String password)
        {
            _driver.Url = "https://www.linkedin.com/";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            IWebElement emailField = FindObjectBy("login-email", TargetType.ID);
            IWebElement passwordField = FindObjectBy("login-password", TargetType.ID);
            IWebElement loginButton = FindObjectBy("login-submit", TargetType.ID);

            Actions actions = new Actions(_driver);
            actions
                .MoveToElement(emailField)
                .SendKeys(emailField, username)
                .MoveToElement(passwordField)
                .SendKeys(passwordField, password)
                .MoveToElement(loginButton)
                .Click(loginButton)
                .Perform();
        }

        private bool IsEmailPublic(String emailAddress)
        {
            _driver.Url = "https://www.proxysite.com/";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            IWebElement searchField = FindObjectBy("/html/body/div[1]/main/div[1]/div/div[3]/form/div[2]/input", TargetType.XPATH);
            IWebElement searchButton = FindObjectBy("/html/body/div[1]/main/div[1]/div/div[3]/form/div[2]/button", TargetType.XPATH);
            IWebElement proxyServer = FindObjectBy("/html/body/div[1]/main/div[1]/div/div[3]/form/div[1]/select", TargetType.XPATH);

            searchField.Clear();

            Actions actions = new Actions(_driver);
            actions
                .Click(proxyServer)
                .SendKeys(proxyServer, "US")
                .SendKeys("\n")
                .SendKeys(searchField, "google.com")
                .Click(searchButton)
                .Perform();

            IWebElement googleSearchField = _driver.FindElementByXPath("/html/body/div[4]/div[4]/form/div[2]/div/div[1]/div/div[1]/input");
            IWebElement googleSearchButton = _driver.FindElementByXPath("/html/body/div[4]/div[4]/form/div[2]/div/div[3]/center/input[1]");

            actions = new Actions(_driver);
            actions
                .SendKeys(googleSearchField, emailAddress)
                .Click(googleSearchButton)
                .Perform();

            try
            {
                // Clear last found url.
                _foundUrl = String.Empty;

                // Find .srg from current page.
                IWebElement container = _driver.FindElementByClassName("srg");
                foreach (IWebElement element in container.FindElements(By.CssSelector("div.g")))
                {
                    IWebElement headerEle = element.FindElement(By.ClassName("r"));
                    String headerLinkText = headerEle.Text;
                    
                    String url = element.FindElement(By.TagName("cite")).Text;

                    IWebElement contentEle = element.FindElement(By.ClassName("r"));
                    String content = element.FindElement(By.ClassName("st")).Text;

                    String[] emailElements = emailAddress.Split('@');
                
                    if (content.Contains(emailAddress))
                    {
                        // Store matched url and return true.
                        _foundUrl = url;
                        return true;
                    }
                }
                // If it reached outside the loop, well, it didn't found any matches.
                return false;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        private String SearchLinkedInURL(String firstName, String lastName, object companyName)
        {
            _driver.Url = "https://www.proxysite.com/";
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            IWebElement searchField = FindObjectBy("/html/body/div[1]/main/div[1]/div/div[3]/form/div[2]/input", TargetType.XPATH);
            IWebElement searchButton = FindObjectBy("/html/body/div[1]/main/div[1]/div/div[3]/form/div[2]/button", TargetType.XPATH);
            IWebElement proxyServer = FindObjectBy("/html/body/div[1]/main/div[1]/div/div[3]/form/div[1]/select", TargetType.XPATH);

            Actions actions = new Actions(_driver);
            actions
                .Click(proxyServer)
                .SendKeys(proxyServer, "US")
                .SendKeys("\n")
                .SendKeys(searchField, "google.com")
                .Click(searchButton)
                .Perform();

            IWebElement googleSearchField = FindObjectBy("/html/body/div[4]/div[4]/form/div[2]/div/div[1]/div/div[1]/input", TargetType.XPATH);
            IWebElement googleSearchButton = FindObjectBy("/html/body/div[4]/div[4]/form/div[2]/div/div[3]/center/input[1]", TargetType.XPATH);
            
            //stringArray.Any(stringToCheck.Contains)
            String companyString = companyName.ToString();
            String[] companyArray = new String[] { };
            companyArray = companyString.Split(' ');


            String searchCriteria = String.Format("site:linkedin.com/in {{ {0} {1} and {2} }}", firstName, lastName, companyString);
            actions = new Actions(_driver);
            actions
                .SendKeys(googleSearchField, searchCriteria)
                .Click(googleSearchButton)
                .Perform();

            IWebElement container = _driver.FindElementByClassName("srg");
            foreach (IWebElement element in container.FindElements(By.ClassName("g")))
            {
                IWebElement headerEle = element.FindElement(By.ClassName("r"));
                String headerLinkText = headerEle.Text;

                IWebElement contentEle = element.FindElement(By.CssSelector("div.s"));
                String url = contentEle.FindElement(By.TagName("cite")).Text;
                String content = contentEle.FindElement(By.ClassName("st")).Text;

                if ((content.Contains(companyString) || companyArray.Any(content.Contains)) && headerLinkText.Contains(lastName) && headerLinkText.Contains(firstName))
                {
                    return url;
                }
            }

            throw new NotFoundException(String.Format("No suitable LinkedIn address found for {0} {1} of {2}.", firstName, lastName, companyString));
        }


        private String GetLinkedInLocation(String url)
        {
            _driver.Url = url;
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            IWebElement locationLabel = FindObjectBy("pv-top-card-section__location", TargetType.CLASS);

            return locationLabel.Text;
        }
        private IWebElement FindObjectBy(String objectLocators, TargetType targetType) {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            
            switch(targetType) {
                case TargetType.ID:
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.Id(objectLocators)));
                case TargetType.CSS:
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(objectLocators)));
                case TargetType.XPATH:
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(objectLocators)));
                case TargetType.NAME:
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.Name(objectLocators)));
                case TargetType.CLASS:
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(objectLocators)));
                default:
                    throw new Exception("WTF?");
            }
        }

        private enum TargetType
        {
            ID,
            CSS,
            XPATH,
            NAME,
            CLASS,
            UNDEFINED
        }

        private void OpenExcelFile(object sender, EventArgs e)
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
        private void StartProcess(object sender, EventArgs e)
        {

            String username = txtUsername.Text;
            String password = txtPassword.Text;

            String path = txtPath.Text;

            if(username.IsNullOrEmptyOrWhiteSpace() || password.IsNullOrEmptyOrWhiteSpace())
            {
                MessageBox.Show("Valid LinkedIn account is required", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (path.IsNullOrEmptyOrWhiteSpace())
            {
                MessageBox.Show("Input Excel file is required!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ChromeOptions options = new ChromeOptions();
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            //options.AddArgument("headless");

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(service, options);
            
            // LinkedIn Location Verification.
            // Step 1. Login to LinkedIn.
            LoginToLinkedIn(username, password);

            // Step 2. Load Input Excel.
            ExcelUtlity util = new ExcelUtlity();
            DataTable sourceDataTable = util.GetWorkSheet(path, 1);

            // Prepare output datatable.
            DataTable outputDataTable = new DataTable();
            outputDataTable.Columns.Add("IsPublic");
            outputDataTable.Columns.Add("ReferenceLink");
            outputDataTable.Columns.Add("LinkedInLocation");
            
            // Step 3. Iterate through input.
            // We'll start at index 1 to skip the header row at index 0.
            for (int i = 1; i < sourceDataTable.Rows.Count; i++)
            {
                DataRow entry = sourceDataTable.Rows[i];
                String firstName    = entry[0].ToString();
                String lastName     = entry[1].ToString();
                String emailAddress = entry[2].ToString();
                String companyName  = entry[3].ToString();

                Console.Write("{0} {1} | {2}: ", firstName, lastName, emailAddress);
                // Step 4. Check Google if email is publicly available.
                // If email is not public, skip.
                // While you're at it, well, fill the output data set as you go through each.
                DataRow newRow = outputDataTable.NewRow();
                if(emailAddress.IsNullOrEmptyOrWhiteSpace())
                {
                    newRow[0] = "N";
                    newRow[1] = "N/A";
                }
                else
                {
                    if (IsEmailPublic(emailAddress))
                    {
                        newRow[0] = "Y";
                        newRow[1] = _foundUrl;

                        try
                        {
                            String linkedInURL = SearchLinkedInURL(firstName, lastName, companyName);
                            String location = GetLinkedInLocation(linkedInURL);

                            newRow[2] = location;
                        }
                        catch (NotFoundException notfound)
                        {
                            newRow[2] = "N/A";
                        }

                        //String linkedInURL = SearchLinkedInURL(firstName, lastName, companyName);
                        //String location = GetLinkedInLocation(linkedInURL);

                        //newRow[2] = location;
                    }
                    else
                    {
                        newRow[0] = "N";
                        newRow[1] = "N/A";
                    }
                }
                Console.Write("\t{0}\t{1}\t{2}\n", newRow[0], newRow[1], newRow[2]);
                outputDataTable.Rows.Add(newRow);
            }

            util.WriteDataTableToExcel(outputDataTable, "Sheet 1", @"D:\out.xlsx");
        }
    }
}