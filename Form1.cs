using System;
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
using SeleniumExtras.WaitHelpers;

namespace SeleniumApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
   
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {



            //nolasa datus no formas

            //string productName = txtProductName.Text.Trim();
            //string minPrice = txtMinPrice.Text.Trim();
            //string maxPrice = txtMaxPrice.Text.Trim();





            //noklusējuma parametri atbilstoši uzdevumam
            string productName = "notebook";
            string minPrice = "500";
            string maxPrice = "700";

            // modalais logs ar paziņojumu par noklusējuma datu izmantošanu
            MessageBox.Show($"Šobrīd tiek izmantoti noklusējuma dati: {productName}/{minPrice}/{maxPrice}");


            //pārbauda vai ir ievadīta prece, ja nav, tad ir kļūdas paziņojums
            //ja netiek izmantoti noklusējuma parametri un ir izmantota lietotāja ievade
            if (string.IsNullOrEmpty(productName))
            {
              
                MessageBox.Show("Lūdzu ievadiet preces nosaukumu!");
                return;
            }

            try
            {
                // inicializē driveri
                var driver = new ChromeDriver();
                driver.Manage().Window.Maximize();

                // atvēr Chrome pārlūku un  aliexpress.com
                driver.Navigate().GoToUrl("https://www.aliexpress.com/");

                // Preces meklēšana
                //Atrod meklēšanas lauku
                var searchBox = driver.FindElement(By.Id("search-words"));
                // Ievada preces nosaukumu meklešānas laukā
                searchBox.SendKeys(productName);
                //Spied submit pogu
                searchBox.SendKeys(OpenQA.Selenium.Keys.Enter);

             

                // atlasa preces robežās no min cenas līdz max cenai
                if (!string.IsNullOrEmpty(minPrice) || !string.IsNullOrEmpty(maxPrice))
                {
           

                    // ievada min cenu
                    if (!string.IsNullOrEmpty(minPrice))
                    {
                        var minPriceInput = driver.FindElement(By.XPath("//input[@placeholder='Min.']"));
                        minPriceInput.SendKeys(minPrice);
                    }

                    // ievada max cenu
                    if (!string.IsNullOrEmpty(maxPrice))
                    {
                        var maxPriceInput = driver.FindElement(By.XPath("//input[@placeholder='Max.']"));
                        maxPriceInput.SendKeys(maxPrice);
                    }

                    // Nospiež pogu "OK", kas ir blakus Cenas filram aliexpress.com
                    //tā kā kaut kāda koda daļa neļāva nospiest OK, tad tika izmantots JS pogas piespiedu nospiešanai

                    var okButton = driver.FindElement(By.XPath("//span[text()='OK']"));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].click();", okButton);

                   
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Notikusi kļūda: {ex.Message}");
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        

            try
            {
                
                var processes = System.Diagnostics.Process.GetProcessesByName("ChromeDriver");

                
                foreach (var process in processes)
                {
                    process.Kill(); 
      
                }
            }
            catch
            {
                Console.WriteLine("Kļūda, nevarēja aizvērt ChromeDriver procesu.");
            }

        }
    }
}


