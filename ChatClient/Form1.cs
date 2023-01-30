using Chat.Domain.Dtos;
using Chat.Domain.Models;
using Chat.Infrastructure;
using Chat.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Net.Http.Json;
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
            if (AppMain.User != null)
            {

            }
            else
            {
                Application.Exit();
            }
            if (AppMain.User == null)
            {
                return;
            }
            label1.Text = AppMain.User.Username;
            timer1.Start();
            GetAll();
            var imageQuery = (_CP.Users.Where(x => x.Username == AppMain.User.Username).Select(y => y.Image).FirstOrDefault());
            if (imageQuery != null)
            {
                MemoryStream ms = new MemoryStream(imageQuery);
                Image returnImage = Image.FromStream(ms);
                pictureBox1.Image = returnImage;
            }
            else if (imageQuery == null)
            {
                pictureBox1.Image = null;
            }
            this.dataGridView1.Columns["Id"].Visible = false;

            if (textBox2.Text == "")
            {
                button4.Enabled = false;
            }
            GetServerNames();
        }
        string[] sw;
        Button button = new Button();

        public void GetServerNames()
        {
            var result = HttpHelper.httpClient.GetAsync($"/api/Users/GetAll/" + AppMain.User.Username).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            List<User> model = JsonConvert.DeserializeObject<List<User>>(json);

            foreach (var item in model)
            {
                sw = item.Server.Split(',');
                for (int i = 0; i < sw.Count(); i++)
                {
                    Button button2 = new Button();
                    flowLayoutPanel1.Controls.Add(button2);
                    button2.Font=new Font(button2.Font.Name,10,button2.Font.Style);
                    button2.Text = sw[i];
                    button.Text = button2.Text;
                    button2.Click += HandleClick; ;
                }
            }
        }

        private void HandleClick(object? sender, EventArgs e)
        {
            var btn=sender as Button;
            label2.Text = btn.Text.Trim();
            var query = _CP.Messages.Where(x => x.Server == label2.Text).ToList();
            dataGridView1.DataSource = query.ToList();
            this.dataGridView1.Columns["Id"].Visible = false;
        }

        private void GetAll()
        {
            var query = _CP.Messages.Where(x => x.Server == label2.Text).ToList();
            dataGridView1.DataSource = query.ToList();
            this.dataGridView1.Columns["Id"].Visible = false;
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.Text != "")
            {
                Message newMessage = new Message()
                {
                    Message1 = textBox1.Text,
                    SenderName = AppMain.User.Username,
                    SenderTime = DateTime.UtcNow,
                    Server = label2.Text
                };
                var json = JsonConvert.SerializeObject(newMessage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await HttpHelper.httpClient.PostAsync($"/api/Messages/SendMessage", content).Result.Content.ReadAsStringAsync();
                var query = _CP.Messages.Where(x => x.Server == label2.Text).ToList();
                dataGridView1.DataSource = query.ToList()/*_CP.Messages.ToList()*/;
                textBox1.Text = "";
                this.dataGridView1.Columns["Id"].Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var query = _CP.Messages.Where(x => x.Server == label2.Text).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            User newUser = new User()
            {
                Username = AppMain.User.Username,
                Server = textBox2.Text
            };
            var json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await HttpHelper.httpClient.PutAsync($"/api/Users/AddServer", content);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                button4.Enabled = false;
            }
        }
    }
}