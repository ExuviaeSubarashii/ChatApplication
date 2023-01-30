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

            Chat.Domain.Models.User newUser = new Chat.Domain.Models.User()
            {
                Username = textBox1.Text,
                Password = textBox2.Text
            };
            var json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await HttpHelper.httpClient.PostAsJsonAsync($"/api/Users/Login", newUser);
            if (message.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Kullanici Bulunamadi");
                return;
            }
            this.Close();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream mstr = new System.IO.MemoryStream();
            Image img = new Bitmap(pictureBox1.ImageLocation);
            img.Save(mstr, img.RawFormat);
            byte[] arrImage = mstr.GetBuffer();

            Chat.Domain.Models.User newUser = new Chat.Domain.Models.User()
            {
                Username = textBox1.Text,
                Password = textBox2.Text,
                HasPassword = textBox2.Text.ConvertStringToMD5(),
                Image=arrImage,
                Server=null
            };
            var json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await HttpHelper.httpClient.PostAsync($"/api/Users/CreateUser", content);
            if (message.StatusCode==HttpStatusCode.OK)
            {
                MessageBox.Show("Kayit Basarili");
            }
            else
            {
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sourcepath = "";
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Dokuman Ekle";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    sourcepath = dlg.FileName;
                }
                pictureBox1.ImageLocation = dlg.FileName;
            }
        }
    }
}
