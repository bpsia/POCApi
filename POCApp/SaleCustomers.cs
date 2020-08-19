using log4net;
using Newtonsoft.Json;
using POCApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POCApp
{
    public partial class SaleCustomers : Form
    {
        public HttpClient client;
        //The URL of the WEB API Service
        private readonly string BaseUrl;
        readonly Users _applicationUser;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter

        private readonly ILog _logger;

        public SaleCustomers()
        {
            InitializeComponent();
            BaseUrl = ConfigurationSettings.AppSettings["BaseUrl"].ToString();
            BaseUrl = BaseUrl + "Customers";
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = LoadCustomersData();
            dataGridView1.DataSource = data;
            this._logger = LogManager.GetLogger("POCApi");
            this.CenterToScreen();
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
            var searchString = txtsearch.Text;
            List<Customers> customersList = new List<Customers>();
            customersList = await LoadCustomersData();
            customersList = customersList.Where(x => x.CustomerName.Contains(searchString)
                                        || x.CustomerNumber.ToString().Contains(searchString)
                                        || x.CustomerType.ToString().Contains(searchString)).ToList();
            dataGridView1.DataSource = customersList;
        }
        private async Task<List<Customers>> LoadCustomersData()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(BaseUrl);

            List<Customers> CustomersList = new List<Customers>();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                var RequestResponce = JsonConvert.DeserializeObject<List<Customers>>(responseData);

                CustomersList = RequestResponce;
            }
            return CustomersList.ToList();
        }
    }
}
