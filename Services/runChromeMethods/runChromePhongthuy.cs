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

        void runChromePhongthuy()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("user-data-dir=C:/Users/manh/AppData/Local/Google/Chrome/User Data");
            chromeOptions.AddArgument("--profile-directory=Default");
            // đóng toàn bộ tiến trình chrome trước khi mở ứng dụng
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                process.Kill();
            }

            //ẩn terminal
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeDriver chromeDriver = new ChromeDriver(service, chromeOptions);
            chromeDriver.Manage().Window.Maximize();

            listView1.Columns.Add("Url bài viết", 400);
            listView1.Columns.Add("Tiêu đề", 700);
            if (label2.Text == "Kết quả")
            {
                return;
            }
            string url = $"{new batdongsanURL().phongthuy}";
            chromeDriver.Navigate().GoToUrl(url);

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

                listView1.Items.Add(item);
            }
            chromeDriver.Quit();
        }

    }
}