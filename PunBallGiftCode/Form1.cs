using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PunBallGiftCode.Models;
using System.Text.Json;

namespace PunBallGiftCode
{
    public partial class Form1 : Form
    {
        public string TempCaptcha { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private async void OnFormLoad(object sender, EventArgs e)
        {
            await GetVaildCodeImage();
        }

        private async Task GetVaildCodeImage()
        {
            using (var hc = new HttpClient())
            {
                var httpResponseMessage = await hc.PostAsync("https://mail-punball2.habby.com/api/v1/captcha/generate", new StringContent(string.Empty));
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    var responesData = await JsonSerializer.DeserializeAsync<ResponesData>(stream);
                    pictureBox1.ImageLocation = $"https://mail-punball2.habby.com/api/v1/captcha/image/{responesData.data.captchaId}";
                    TempCaptcha = responesData.data.captchaId;
                    textBox3.ResetText();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private async void button2_On_Click(object sender, EventArgs e)
        {
            await GetVaildCodeImage();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //https://mail-punball2.habby.com/api/v1/giftcode/claim

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please input ID!");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please input GiftCode!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please input VaildCode!");
                return;
            }

            var postData = new PostData()
            {
                userId = textBox1.Text,
                giftCode = textBox2.Text,
                captcha = textBox3.Text,
                captchaId = TempCaptcha,
            };
            var jsonString = JsonSerializer.Serialize(postData);
            var stringContent = new StringContent(jsonString);
            using (var hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var httpResponseMessage = await hc.PostAsync("https://mail-punball2.habby.com/api/v1/giftcode/claim", stringContent);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    var responesData = await JsonSerializer.DeserializeAsync<ResponesData>(stream);
                    if (string.IsNullOrWhiteSpace(responesData.message) == false)
                    {
                        MessageBox.Show(responesData.message);
                    }
                    else
                    {
                        MessageBox.Show("Redeem Success!");
                    }
                    await GetVaildCodeImage();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
