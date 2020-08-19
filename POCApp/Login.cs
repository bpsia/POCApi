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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace POCApp
{
    public partial class Login : Form
    {
        HttpClient client;
        //The URL of the WEB API Service
        private readonly string BaseUrl;
        //The HttpClient Class, this will be used for performing 
        //HTTP Operations, GET, POST, PUT, DELETE
        //Set the base address and the Header Formatter
        private readonly ILog _logger;

        public Login()
        {

            InitializeComponent();
            BaseUrl = ConfigurationSettings.AppSettings["BaseUrl"].ToString();
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // this.WindowState = FormWindowState.Maximized;
            this._logger = LogManager.GetLogger("POCApi");
            this.CenterToScreen();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var userName = txtUserName.Text;
            var passWord = txtPassword.Text;
            Users appUser = new Users();
            HttpResponseMessage responseMessage = await client.GetAsync(BaseUrl + "/" + "AuthenticateUser" + "/" + userName + "/" + passWord);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;

                appUser = JsonConvert.DeserializeObject<Users>(responseData);
            }
            this.Hide();
            WelCome form = new WelCome(appUser);
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
