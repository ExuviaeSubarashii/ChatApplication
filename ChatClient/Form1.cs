using Chat.Domain.Models;
using Chat.Infrastructure;
using Chat.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text;
using Message = Chat.Domain.Models.Message;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoginForm LF = new LoginForm();
            LF.ShowDialog();
            label1.Text = AppMain.User.Username;
            using (ChatContext CP =new ChatContext())
            {
            dataGridView1.DataSource = CP.Messages.ToList();
            }
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Enter&&textBox1.Text!="")
            {
                Message newMessage = new Message()
                {
                    Message1 = textBox1.Text,
                    SenderName= AppMain.User.Username,
                    SenderTime= DateTime.UtcNow
                };
                var json = JsonConvert.SerializeObject(newMessage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage message= HttpHelper.httpClient.PostAsync($"/api/Messages/SendMessage", content).Result;
                using (ChatContext CP = new ChatContext())
                {
                    dataGridView1.DataSource = CP.Messages.ToList();
                }
            }
        }
    }
}