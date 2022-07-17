﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Text;
using web_scraping_csharp.Controllers;
using web_scraping_csharp.Services;

namespace web_scraping_csharp
{
    public partial class Form1 : Form
    {

        void crawlAllTintuc()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            // chromeOptions.AddArgument("user-data-dir=C:/Users/manh/AppData/Local/Google/Chrome/User Data");
            // chromeOptions.AddArgument("--profile-directory=Default");
             chromeOptions.AddArgument("--incognito");
            chromeOptions.BinaryLocation = "Chromium\\102.0.5005.0\\chrome.exe";

            //ẩn terminal
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeDriver chromeDriver = new ChromeDriver(service, chromeOptions);
            chromeDriver.Manage().Window.Maximize();

            if (label2.Text == "Kết quả")
            {
                return;
            }
            string url = $"{new batdongsanURL().tintuc}";
            chromeDriver.Navigate().GoToUrl(url);
            List<ListViewItem> insertItems = new();

            IWebElement tintucList = chromeDriver.FindElement(By.ClassName("re__nlcc-main-left"));
            List<IWebElement> productItem = tintucList.FindElements(By.ClassName("re__link-se")).ToList();
            foreach (var product in productItem)
            {
                if (label2.Text == "Kết quả")
                {
                    break;
                }
                ListViewItem item = new ListViewItem();
                if (product.GetAttribute("href") != null)
                {
                    item.Text = product.GetAttribute("href");
                    item.SubItems.Add(product.GetAttribute("innerText").Trim());
                }
                else { item.Text = ""; item.SubItems.Add("Trống"); }

                insertItems.Add(item);

            }
                if (label2.Text == "Kết quả")
                {
                    chromeDriver.Quit();
                    return;
                }else{
                    new tintucController().queryInsertAll(insertItems);
                }
            chromeDriver.Quit();
        }

    }
}