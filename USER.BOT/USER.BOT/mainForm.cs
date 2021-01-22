using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;


namespace USER.BOT
{
    public partial class mainForm : Form
    {
        string access_token;
        string user_id;
        public MembersGet mg;
        public WebClient cl;
        public string Answer;

        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.BringToFront();
            webBrowser1.Dock = DockStyle.Fill;

            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=7614304"+
                "&display=page&redirect_uri=https://oauth.vk.com/blank.html&"+
                "scope=friends+groups+wall&"+
                "response_type=token&v=5.124&state=123456");

            //https://api.vk.com/method/groups.getMembers?group_id=201385065&access_token=8bb6dceb574e3549ca5805f049422d778fb5d065f660728fc9460b051e60d9ca900e0c3adced85291b6ca&v=5.124
        }

        private string GetAnswer(string Request, string AccessToken)
        {
            string Req = Request + AccessToken + "&v=5.124";
            cl = new WebClient();
            Answer = Encoding.UTF8.GetString(cl.DownloadData(Req));
            return Answer;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Url.ToString().Contains("access_token"))
            {
                string[] param = webBrowser1.Url.ToString().Split(new[] { "#", "&" }, StringSplitOptions.RemoveEmptyEntries);
                access_token = param[1];

                string Request = "https://api.vk.com/method/account.getProfileInfo?" +
                access_token + "&v=5.124";

                WebClient cl = new WebClient();
               
                string Answer = Encoding.UTF8.GetString(cl.DownloadData(Request));

                GetProfileInfo gpi = JsonConvert.DeserializeObject<GetProfileInfo>(Answer);
                labelFamily.Text = gpi.response.last_name;
                labelName.Text = gpi.response.first_name;
                user_id = gpi.response.id.ToString();

                Request = "https://api.vk.com/method/users.get?fields=photo_100&" +
                    access_token + "&v=5.124";
                Answer = Encoding.UTF8.GetString(cl.DownloadData(Request));
                UsersGet ug = JsonConvert.DeserializeObject<UsersGet>(Answer);

                pictureBoxAvatar.ImageLocation = ug.response[0].photo_100;
                user_id = ug.response[0].id.ToString();
                webBrowser1.Hide();


                Request = "https://api.vk.com/method/groups.getMembers?group_id=201385065&";
                Answer = GetAnswer(Request, access_token);
                mg = JsonConvert.DeserializeObject<MembersGet>(Answer);

                foreach (int MemberId in mg.response.items)
                {
                    if (Convert.ToInt32(user_id) == MemberId)
                    {
                        buttonFindComments.Enabled = true;
                        buttonGDZ.Enabled = true;
                        buttonGetPopularPost.Enabled = true;
                        buttonLiking.Enabled = true;
                        buttonMassComment.Enabled = true;
                        buttonSelebrate.Enabled = true;
                        buttonTextBot.Enabled = true;
                    }
                }
                if (buttonTextBot.Enabled != true)
                {
                    label1.Visible = true;
                }
            }
        }

        private void buttonGetPopularPost_Click(object sender, EventArgs e)
        {
            FormMostPopularPost frm = new FormMostPopularPost();
            frm.access_token = this.access_token;
            frm.user_id = user_id;
            frm.Show();
        }

        private void buttonFindComments_Click(object sender, EventArgs e)
        {
            AnswerForm form = new AnswerForm();
            form.access_token = access_token;
            form.user_id = user_id;
            form.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/club201385065");
        }
    }
}
