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
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread buttonOne = new Thread(RunChrome);
            buttonOne.IsBackground = true;

            buttonOne.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread buttonTwo = new Thread(ClearListView);
            buttonTwo.IsBackground = true;
            buttonTwo.Start();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            TextBox serverDBname = Application.OpenForms["Form1"].Controls["serverDBname"] as TextBox;
            TextBox serverDBport = Application.OpenForms["Form1"].Controls["serverDBport"] as TextBox;
            TextBox userDBname = Application.OpenForms["Form1"].Controls["userDBname"] as TextBox;
            TextBox userDBpassword = Application.OpenForms["Form1"].Controls["userDBpassword"] as TextBox;
            TextBox DBname = Application.OpenForms["Form1"].Controls["DBname"] as TextBox;
            string serverName = serverDBname.Text.Trim();
            string serverPort = serverDBport.Text.Trim();
            string userName = userDBname.Text.Trim();
            string password = userDBpassword.Text.Trim();
            string dbName = DBname.Text.Trim();
            if (serverName != "" && serverPort != "" && userName != "" && password != "")
            {
                Thread buttonThree = new Thread(SaveToDb);
                buttonThree.IsBackground = true;
                buttonThree.Start();     
            }
            else
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin của cơ sở dữ liệu MySQL trước khi cào");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TextBox serverDBname = Application.OpenForms["Form1"].Controls["serverDBname"] as TextBox;
            TextBox serverDBport = Application.OpenForms["Form1"].Controls["serverDBport"] as TextBox;
            TextBox userDBname = Application.OpenForms["Form1"].Controls["userDBname"] as TextBox;
            TextBox userDBpassword = Application.OpenForms["Form1"].Controls["userDBpassword"] as TextBox;
            TextBox DBname = Application.OpenForms["Form1"].Controls["DBname"] as TextBox;
            string serverName = serverDBname.Text.Trim();
            string serverPort = serverDBport.Text.Trim();
            string userName = userDBname.Text.Trim();
            string password = userDBpassword.Text.Trim();
            string dbName = DBname.Text.Trim();
            if (serverName != "" && serverPort != "" && userName != "" && password != "")
            {
                Thread buttonFour = new Thread(LoadFromDb);
                buttonFour.IsBackground = true;
                buttonFour.Start();
            }
            else
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin của cơ sở dữ liệu MySQL trước khi cào");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            resultToTextFile();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Thread button6 = new Thread(StopGetData);
            button6.IsBackground = true;
            button6.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Thread buttonSeven = new Thread(RunChromeAllPages);
            buttonSeven.IsBackground = true;

            buttonSeven.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Thread buttonEight = new Thread(deleteAllDataInDbForm);
            buttonEight.IsBackground = true;
            buttonEight.Start();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            TextBox serverDBname = Application.OpenForms["Form1"].Controls["serverDBname"] as TextBox;
            TextBox serverDBport = Application.OpenForms["Form1"].Controls["serverDBport"] as TextBox;
            TextBox userDBname = Application.OpenForms["Form1"].Controls["userDBname"] as TextBox;
            TextBox userDBpassword = Application.OpenForms["Form1"].Controls["userDBpassword"] as TextBox;
            TextBox DBname = Application.OpenForms["Form1"].Controls["DBname"] as TextBox;
            string serverName = serverDBname.Text.Trim();
            string serverPort = serverDBport.Text.Trim();
            string userName = userDBname.Text.Trim();
            string password = userDBpassword.Text.Trim();
            string dbName = DBname.Text.Trim();
            if(serverName != "" && serverPort != "" && userName != "" && password != "")
            {
                label2.Text = "Dữ liệu cào của từng danh mục được lưu tự động vào database";
                foreach (var process in Process.GetProcessesByName("chrome"))
                {
                    process.Kill();
                }
                button1.Enabled = false;
                button9.Enabled = false;
                button7.Enabled = false;
                button6.Enabled = true;

                //dùng new task() để tạo thread mới run ở bg, tách biệt với UI của winform
                Task bgTask = new Task(() =>
                {
                    //option giới hạn lượng thread chạy
                    ParallelOptions parallelOptions = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 3
                    };
                    // dùng parallel.invoke để thêm 1 nhóm các thread mới
                    Parallel.Invoke(
                           parallelOptions,
                           // task 1 trang
                           () => { crawlAllDoanhnghiep(); },
                           () => { crawlAllTintuc(); },
                           () => { crawlAllWiki(); },
                           () => { crawlAllPhongthuy(); },
                           () => { crawlAllNoingoaithat(); },
                           // task nhiều trang
                           () => { crawlAllNhadatban(); },
                           () => { crawlAllNhadatchothue(); },
                           () => { crawlAllDuan(); },
                           () => { crawlAllNhamoigioi(); }
                       );
                    //hết cái trên mới chạy tới cái dưới
                    Parallel.Invoke(
                            () => {
                                button1.Enabled = true;
                                button9.Enabled = true;
                                button7.Enabled = true;
                                button6.Enabled = false;
                                label2.Text = "Lưu ý: Tool sẽ đóng tiến trình Chrome hiện tại khi bắt đầu cào";
                            }
                        );
                });
                bgTask.Start();

            }
            else
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin của cơ sở dữ liệu MySQL trước khi cào");
            }

        }


    }
}