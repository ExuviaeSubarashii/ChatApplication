using Chat.Domain.Models;
using Chat.Infrastructure;
using Chat.Service;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            AppMain.User = new Chat.Domain.Models.User();
            AppMain.User.Username = textBox1.Text;

            Chat.Domain.Models.User newUser= new Chat.Domain.Models.User()
            {
                Username=textBox1.Text,
                Password=textBox2.Text,
                HasPassword=textBox2.Text.ConvertStringToMD5()
            };
            var json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message=  await HttpHelper.httpClient.PostAsync($"/api/Users/Login", content);
            if (message.StatusCode!=HttpStatusCode.OK)
            {
                MessageBox.Show("Kullanici Bulunamadi");
                return;
            }
            this.Close();
        }
    }
}
