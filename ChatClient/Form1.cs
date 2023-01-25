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
        ChatContext _CP = new ChatContext();
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoginForm LF = new LoginForm();
            LF.ShowDialog();
            label1.Text = AppMain.User.Username;
            timer1.Start();
            GetAll();
        }

        private void GetAll()
        {
            dataGridView1.DataSource = _CP.Messages.ToList();
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.Text != "")
            {
                Message newMessage = new Message()
                {
                    Message1 = textBox1.Text,
                    SenderName = AppMain.User.Username,
                    SenderTime = DateTime.UtcNow
                };
                var json = JsonConvert.SerializeObject(newMessage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await HttpHelper.httpClient.PostAsync($"/api/Messages/SendMessage", content).Result.Content.ReadAsStringAsync();
                dataGridView1.DataSource = _CP.Messages.ToList();
                textBox1.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _CP.Messages.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}