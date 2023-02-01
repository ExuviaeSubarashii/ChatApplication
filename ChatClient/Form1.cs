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
        string[] sww;
        Button button = new Button();
        Button button3 = new Button();
        public async void GetChannelNames()
        {
            flowLayoutPanel2.Controls.Clear();
            var result = await HttpHelper.httpClient.GetAsync($"/api/Users/GetAllChannels/" + label2.Text).Result.Content.ReadAsStringAsync();
            var json = result;
            List<Servers> model = JsonConvert.DeserializeObject<List<Servers>>(json);
            foreach (var item in model)
            {
                if (item.Channels!=null)
                {
                    sww = item.Channels.Split(',');
                    for (int i = 0; i < sww.Count(); i++)
                    {
                        Button button2 = new Button();
                        flowLayoutPanel2.Controls.Add(button2);
                        button2.Font = new Font(button2.Font.Name, 10, button2.Font.Style);
                        button2.Text = sww[i];
                        button3.Text = button2.Text;
                        button2.Click += Channel_Click;
                    }
                }
            }
        }

        private void Channel_Click(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            label3.Text = btn.Text.Trim();
            var query = _CP.Servers.Where(x => x.ServerName==label2.Text&&x.Channels==label3.Text).ToList();
            dataGridView1.DataSource = query.ToList();
        }

        public async void GetServerNames()
        {
            flowLayoutPanel1.Controls.Clear();
            var result = await HttpHelper.httpClient.GetAsync($"/api/Users/GetAll/" + AppMain.User.Username).Result.Content.ReadAsStringAsync();
            var json = result;
            List<User> model = JsonConvert.DeserializeObject<List<User>>(json);

            foreach (var item in model)
            {
                if (item.Server != null)
                {
                    sw = item.Server.Split(',');
                    for (int i = 0; i < sw.Count(); i++)
                    {
                        Button button2 = new Button();
                        flowLayoutPanel1.Controls.Add(button2);
                        button2.Font = new Font(button2.Font.Name, 10, button2.Font.Style);
                        button2.Text = sw[i];
                        button.Text = button2.Text;
                        button2.Click += HandleClick;
                        button2.MouseUp += DeleteServers_MouseUp;
                    }
                }
                else
                {
                    return;
                }
            }
            
        }

        private async void DeleteServers_MouseUp(object? sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            User serverUser = new User()
            {
                Username = AppMain.User.Username,
                Server = btn.Text
            };
            if (e.Button == MouseButtons.Right)
            {
                DialogResult dialogresult = MessageBox.Show("Do you want to leave from this server?", btn.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogresult == DialogResult.Yes)
                {
                    await HttpHelper.httpClient.PutAsJsonAsync($"/api/Users/DeleteServer/", serverUser).Result.Content.ReadAsStringAsync();
                    GetServerNames();
                }
            }
            else if (e.Button==MouseButtons.Middle)
            {
                textBox2.Text=btn.Text;
            }
        }

        private void HandleClick(object? sender, EventArgs e)
        {
            var btn = sender as Button;
            label2.Text = btn.Text.Trim();
            var query = _CP.Messages.Where(x => x.Server == label2.Text && x.Channel == label3.Text).ToList();
            dataGridView1.DataSource = query.ToList();
            this.dataGridView1.Columns["Id"].Visible = false;
            GetChannelNames();
        }

        private void GetAll()
        {
            var query = _CP.Messages.Where(x => x.Server == label2.Text && x.Channel == label3.Text).ToList();
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
                    Server = label2.Text,
                    Channel=label3.Text
                };
                var json = JsonConvert.SerializeObject(newMessage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await HttpHelper.httpClient.PostAsync($"/api/Messages/SendMessage", content).Result.Content.ReadAsStringAsync();
                var query = _CP.Messages.Where(x => x.Server == label2.Text && x.Channel == label3.Text).ToList();
                dataGridView1.DataSource = query.ToList();
                textBox1.Clear();
                this.dataGridView1.Columns["Id"].Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var query = _CP.Messages.Where(x => x.Server == label2.Text && x.Channel == label3.Text).ToList();
            dataGridView1.DataSource = query.ToList();
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
            Servers newServer = new Servers()
            {
                ServerName=textBox2.Text,
                Channels=textBox3.Text,
                UserNames=AppMain.User.Username,
            };
            if (newServer.Channels==null)
            {
                MessageBox.Show("Bir kanal ismi ekleyiniz");
                return;
            }
            var newUserjson = JsonConvert.SerializeObject(newUser);
            var newUsercontent = new StringContent(newUserjson, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await HttpHelper.httpClient.PutAsync($"/api/Users/AddServer", newUsercontent);

            var newtoServerjson = JsonConvert.SerializeObject(newServer);
            var newtoServerrcontent = new StringContent(newtoServerjson, Encoding.UTF8, "application/json");
            HttpResponseMessage message3 = await HttpHelper.httpClient.PostAsync($"/api/Users/AddNewServer", newtoServerrcontent);

            var newServerjson = JsonConvert.SerializeObject(newServer);
            var newServercontent = new StringContent(newServerjson, Encoding.UTF8, "application/json");
            HttpResponseMessage message2 = await HttpHelper.httpClient.PutAsync($"/api/Users/AddChannel", newServercontent);
            flowLayoutPanel1.Controls.Clear();
            GetServerNames();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }
        }
    }
}