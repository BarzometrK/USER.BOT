using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

namespace USER.BOT
{
    public partial class AnswerForm : Form
    {
        public string access_token;
        public string user_id;
        public string Request;
        public WebClient cl;
        public string Answer;
        public WallGet wg;
        public string postID;
        public string commentID;
        public int target = 0;
        public int postCount;
        public int progress;
        public int stage;

        public AnswerForm()
        {
            InitializeComponent();
        }

        private void AnswerForm_Load(object sender, EventArgs e)
        {
            
        }

        private string GetAnswer(string Request, string AccessToken)
        {
            string Req = Request + AccessToken + "&v=5.124";
            cl = new WebClient();
            Answer = Encoding.UTF8.GetString(cl.DownloadData(Req));
            return Answer;
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox1.Enabled = false;

            if (stage == 0)
            {
                OutputPosts();
            }

            if (stage == 2)
            {
                WorkWithComments();
            }
        }

        private void OutputPosts()
        {
            stage = 1;
            Request = "https://api.vk.com/method/wall.get?owner_id="+ user_id +"&";
            Answer = GetAnswer(Request, access_token);
            wg = JsonConvert.DeserializeObject<WallGet>(Answer);

            labelOutput.Text = "Пожалуйста подождите...";
            buttonInput.Enabled = false;
            listView1.Enabled = false;
            listView2.Enabled = false;
            Select();

            foreach (WallGet.Item itemWG in wg.response.items)
            {
                if (itemWG.comments.count > 0)
                {
                    progress++;
                }
            }

            progressBar1.Maximum = progress;
            progressBar1.Value = progressBar1.Maximum;
            label4.Text = "Осталось: " + progressBar1.Value.ToString();

            foreach (WallGet.Item itemWG in wg.response.items)
            {
                if (itemWG.comments.count > 0)
                {
                    string[] LVitem = new string[4];
                    LVitem[0] = itemWG.id.ToString();
                    LVitem[1] = itemWG.comments.count.ToString();
                    LVitem[2] = itemWG.text;
                    if (itemWG.attachments != null)
                    {
                        foreach (WallGet.Attachment attachmentsWG in itemWG.attachments)
                        {
                            if (attachmentsWG.type != "photo")
                            {
                                LVitem[3] = null;
                            }
                            else
                            {
                                try
                                {
                                    if (attachmentsWG.photo.sizes.Count > 3)
                                    {
                                        LVitem[3] = itemWG.attachments[0].photo.sizes[3].url;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LVitem[3] = itemWG.attachments[0].photo.sizes[0].url;
                                }
                            }
                        }
                    }

                    ListViewItem lvi = new ListViewItem(LVitem);
                    listView1.Items.Add(lvi);
                    progressBar1.Value--;
                    label4.Text = "Осталось: " + progressBar1.Value.ToString();
                }
                Application.DoEvents();
                Thread.Sleep(500);
            }

            labelOutput.Text = "Найдено "+ progress + " записи с комментариями " + "\r\n" +
                "Выберите запись, комментарии к которой хотите увидеть, " + "\r\n" +
                "нажав на соответствующее ID, и нажмите на кнопку " + "\r\n" + "для вывода";
            buttonInput.Text = "Вывод";
            listView1.Enabled = true;
            listView2.Enabled = true;
        }

        private void WorkWithComments()
        {
            if (target == 1)
            {
                OutputComments();
            }
            else
            {
                AnswerToComment();
            }
        }

        private void OutputComments()
        {
            progressBar1.Maximum = progress;
            progressBar1.Value = progressBar1.Maximum;
            label4.Text = "Осталось: " + progressBar1.Value.ToString();

            if (labelOutput.Text != "")
            {
                WallGet.Item itemWG = new WallGet.Item();
                itemWG.id = Convert.ToInt16(postID);
                Request = "https://api.vk.com/method/wall.getComments?owner_id=" + user_id + "&post_id=" + itemWG.id + 
                    "&count=100&";
                Answer = GetAnswer(Request, access_token);
                CommentsGet csg = JsonConvert.DeserializeObject<CommentsGet>(Answer);
                listView2.Items.Clear();

                labelOutput.Text = "Пожалуйста подождите...";
                buttonInput.Enabled = false;
                listView1.Enabled = false;
                listView2.Enabled = false;
                Select();

                foreach (CommentsGet.Item itemCG in csg.response.items)
                {
                    string[] LVitem2 = new string[5];

                    Request = "https://api.vk.com/method/users.get?user_ids=" + itemCG.from_id + "&";
                    Answer = GetAnswer(Request, access_token);
                    UsersGet ug = JsonConvert.DeserializeObject<UsersGet>(Answer);

                    Application.DoEvents();

                    LVitem2[0] = itemCG.id.ToString();
                    if (ug.response.Count != 0)
                    {
                        LVitem2[2] = ug.response[0].first_name;
                        LVitem2[3] = ug.response[0].last_name;
                        LVitem2[4] = itemCG.text;
                    }
                    else
                    {
                        LVitem2[2] = "  -";
                        LVitem2[3] = "  -";
                        LVitem2[4] = "  -";
                    }

                    ListViewItem lvi2 = new ListViewItem(LVitem2);
                    listView2.Items.Add(lvi2);

                    string id = itemCG.id.ToString();
                    Request = "https://api.vk.com/method/wall.getComments?owner_id=" + user_id + "&post_id=" + itemWG.id + 
                        "&comment_id=" + id + "&count=100&";
                    Answer = GetAnswer(Request, access_token);
                    CommentsGet2 csg2 = JsonConvert.DeserializeObject<CommentsGet2>(Answer);

                    Thread.Sleep(600);

                    progressBar1.Value--;
                    label4.Text = "Осталось: " + progressBar1.Value.ToString();

                    foreach (CommentsGet2.Item itemCsG2 in csg2.response.items)
                    {
                        Request = "https://api.vk.com/method/users.get?user_ids=" + itemCsG2.from_id + "&";
                        Answer = GetAnswer(Request, access_token);
                        ug = JsonConvert.DeserializeObject<UsersGet>(Answer);

                        Application.DoEvents();

                        LVitem2[0] = itemCsG2.id.ToString();
                        LVitem2[1] = id.ToString();
                        LVitem2[2] = ug.response[0].first_name;
                        LVitem2[3] = ug.response[0].last_name;
                        LVitem2[4] = itemCsG2.text;

                        lvi2 = new ListViewItem(LVitem2);
                        listView2.Items.Add(lvi2);

                        Thread.Sleep(600);

                        progressBar1.Value--;
                        label4.Text = "Осталось: " + progressBar1.Value.ToString();
                    }
                }
            }
            labelOutput.Text = "Выберите комментарий, ответ на который хотите отправить " + "\r\n" +
                "Для этого нажмите на ID соответствующего комментария " + "\r\n" +
                "Вы можете выбрать несколько, удерживая Ctrl или Shift, или " + "\r\n" + 
                "выбрать все с помощью кнопки " + "\r\n" +
                "Либо выберите другую запись";
            buttonInput.Enabled = true;
            buttonSelect.Enabled = true;
            //buttonDelete.Enabled = true;
            listView1.Enabled  = true;
            listView2.Enabled = true;
        }

        private void AnswerToComment()
        {
            if (textBoxInput.Text != "")
            {
                labelOutput.Text = "Пожалуйста подождите...";

                progressBar1.Maximum = listView2.SelectedItems.Count;
                progressBar1.Value= listView2.SelectedItems.Count;
                label4.Text = "Осталось: " + progressBar1.Value.ToString();

                foreach (ListViewItem lwi in listView2.SelectedItems)
                {
                    foreach (ListViewItem lvi in listView1.Items)
                    {
                        if (lvi.SubItems[0].Text == postID)
                        {
                            lvi.SubItems[1].Text = Convert.ToString(Convert.ToInt32(lvi.SubItems[1].Text) + 1);
                        }
                    }

                    commentID = lwi.Text;
                    Request = "https://api.vk.com/method/wall.createComment?owner_id=" + user_id + "&post_id=" + postID +
                        "&message=" + textBoxInput.Text + "&reply_to_comment=" + commentID + "&";
                    Answer = GetAnswer(Request, access_token);

                    progressBar1.Value--;
                    label4.Text = "Осталось: " + progressBar1.Value.ToString();
                    Application.DoEvents();
                    Thread.Sleep(600);
                }

                labelOutput.Text = "Комментарий(ии) отправлен(ы) " + "\r\n" + "Вы можете выбрать другую запись или комментарий";
                textBoxInput.Text = "";
                progress = 0;
            }
            else
            {
                labelOutput.Text = "Введите ответ на комментарий(ии)";
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                labelOutput.Text = "Выбрана запись с ID: " + listView1.SelectedItems[0].Text + "\r\n" +
                    "Нажмите кнопку для вывода комментариев";
                postID = listView1.SelectedItems[0].Text;

                progress = Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text);
                
                target = 1;

                if (listView1.SelectedItems[0].SubItems[3].Text != "")
                {
                    pictureBox1.ImageLocation = listView1.SelectedItems[0].SubItems[3].Text;
                }
                else
                {
                    pictureBox1.Image = null;
                }

                buttonInput.Text = "Вывод";
                textBoxInput.Enabled = false;
                stage = 2;
                buttonInput.Enabled = true;
                buttonSelect.Enabled = false;
                //buttonDelete.Enabled = false;
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                labelOutput.Text = "Выбран(ы) комментарий(ии) с ID: ";
                foreach (ListViewItem lvi in listView2.SelectedItems)
                {
                    labelOutput.Text = labelOutput.Text + lvi.Text + " ";
                }
                labelOutput.Text = labelOutput.Text + " " + "\r\n" +
                    "Введите текст ответа и нажмите кнопку для отправки, либо нажмите Удалить для удаления комментариев";

                target = 2;
                textBoxInput.Enabled = true;

                buttonSelect.Text = "Убрать выделение";
            }
            else
            {
                buttonSelect.Text = "Выбрать все комментарии";
            }
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            buttonInput.Text = "Отправить";
        }

        private void listView1_Enter(object sender, EventArgs e)
        {
            listView1_SelectedIndexChanged(sender, e);
        }

        private void listView2_Enter(object sender, EventArgs e)
        {
            listView2_SelectedIndexChanged(sender, e);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (buttonSelect.Text == "Выбрать все комментарии")
            {
                foreach (ListViewItem lvi in listView2.Items)
                {
                    lvi.Selected = true;
                    buttonSelect.Text = "Убрать выделение";
                }
            }
            else if (buttonSelect.Text == "Убрать выделение")
            {
                foreach (ListViewItem lvi in listView2.Items)
                {
                    lvi.Selected = false;
                    buttonSelect.Text = "Выбрать все комментарии";
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //labelOutput.Text = "Пожалуйста подождите...";

            //progressBar1.Maximum = listView2.SelectedItems.Count;
            //progressBar1.Value = listView2.SelectedItems.Count;
            //label4.Text = "Осталось: " + progressBar1.Value.ToString();

            //foreach (ListViewItem lvi in listView2.SelectedItems)
            //{
            //    if (lvi.SubItems[2].Text != "  -")
            //    {
            //        if (lvi.SubItems[1].Text != "")
            //        {
            //            foreach (ListViewItem lvi1 in listView1.Items)
            //            {
            //                if (lvi1.SubItems[0].Text == postID)
            //                {
            //                    lvi1.SubItems[1].Text = Convert.ToString(Convert.ToInt32(lvi1.SubItems[1].Text) - 1);
            //                }
            //            }
            //        }
            //        commentID = lvi.Text;
            //        Request = "https://api.vk.com/method/wall.deleteComment?owner_id=" + user_id + "&comment_id=" + commentID + "&";
            //        Answer = GetAnswer(Request, access_token);

            //        progressBar1.Value--;
            //        label4.Text = "Осталось: " + progressBar1.Value.ToString();
            //        Application.DoEvents();
            //        Thread.Sleep(600);
            //    }
            //    else
            //    {
            //        progressBar1.Value--;
            //        label4.Text = "Осталось: 0";
            //    }
            //}

            //labelOutput.Text = "Комментарий(ии) удален(ы)";
            //textBoxInput.Text = "";
            //progress = 0;
        }

        private void labelOutput_Click(object sender, EventArgs e)
        {
            MessageBox.Show(labelOutput.Text);
        }

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                user_id = "-" + textBox1.Text;
                textBox1.Enabled = false;
                textBox1.Text = "";
                button1.Enabled = false;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
