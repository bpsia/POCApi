using log4net;
using log4net.Util.TypeConverters;
using POCApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POCApp
{
    public partial class WelCome : Form
    {
        public HttpClient client;
        //The URL of the WEB API Service
        public readonly string BaseUrl;
        readonly Users _applicationUser;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        private readonly ILog _logger;

        public WelCome()
        {
            InitializeComponent();
            BaseUrl = ConfigurationSettings.AppSettings["BaseUrl"].ToString();
            BaseUrl = BaseUrl + "Customers";
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this._logger = LogManager.GetLogger("POCApi");
            this.CenterToScreen();
        }
        public WelCome(Users users)
        {
            InitializeComponent();
            this._applicationUser = users;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            //To where your opendialog box get starting location. My initial directory location is desktop.
            openFileDialog1.InitialDirectory = "C://Desktop";
            //Your opendialog box title name.
            openFileDialog1.Title = "Select file to be upload.";
            //which type file format you want to upload in database. just add them.
            openFileDialog1.Filter = "Select Valid Document(*.txt; *.doc; *.xlsx; *.csv;)|*.txt; *.docx; *.xlsx; *.csv";
            //FilterIndex property represents the index of the filter currently selected in the file dialog box.
            openFileDialog1.FilterIndex = 1;
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {
                        string path = System.IO.Path.GetFullPath(openFileDialog1.FileName);
                        txtFilePath.Text = path;
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload document.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login form = new Login();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            _logger.Info("Loading file..");
            string StringUrl = ConfigurationSettings.AppSettings["BaseUrl"].ToString();
            StringUrl = StringUrl + "Customers";
            client = new HttpClient();
            client.BaseAddress = new Uri(StringUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var filePath = txtFilePath.Text;
            var minSales = txtMinSalesAmount.Text;

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please select a file/Add address of file");
            }
            if (string.IsNullOrEmpty(minSales))
            {
                MessageBox.Show("Please enter Min sales amount");
            }
            List<Customers> cutomersList = new List<Customers>();
            Customers customers = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = null;

                while (null != (line = reader.ReadLine()))
                {
                   
                    if (!line.StartsWith("#"))
                    {
                        string[] values = line.Split(',');
                        var datestring = values[4].Replace("[", "")
                                                 .Replace("(", "")
                                                 .Replace(")", "")
                                                 .Replace("]", "").Trim();
                        if (IsValidDate(datestring))
                        {
                            if ((Convert.ToDouble(values[3]) > Convert.ToDouble(minSales)) && (DateTime.Parse(datestring) < DateTime.Now))

                            {
                                customers = new Customers()
                                {
                                    CustomerName = values[2].ToString(),
                                    CustomerNumber = values[0],
                                    CustomerType = (values[1] == "1") ? "Private person" : "Company",
                                    Timestamp = datestring,
                                    TotalAmountOfSales = Convert.ToDouble(values[3])
                                };
                                cutomersList.Add(customers);
                            }
                        }

                    }
                }
            }

            if (cutomersList.Count > 0)
            {
                foreach (var item in cutomersList.ToList())
                {
                    HttpResponseMessage responseMessage = await client.PostAsJsonAsync(StringUrl, item);
                }

                this.Hide();
                SaleCustomers form = new SaleCustomers();
                form.Closed += (s, args) => this.Close();
                form.Show();
            }
        }

        private bool IsValidDate(string datetring)
        {
            string[] dd = datetring.Split('-');
            if (dd[0].Length == 4 && dd[1].Length == 2 && dd[2].Length == 2)
                return true;
            else
                return false;
        }


    }
}
