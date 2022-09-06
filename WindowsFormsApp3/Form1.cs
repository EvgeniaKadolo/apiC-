using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        string filePath;
        string language;

        public Form1()
        {
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if(filePath != null)
            {
                var fileBytes = File.ReadAllBytes(filePath);
                var fileName = Path.GetFileName(filePath);

                var requestContent = new MultipartFormDataContent();

                requestContent.Add(new StringContent("c848ee14b288957"), "apikey");
                if (language != null)
                    requestContent.Add(new StringContent(language), "language");

                var imageContent = new ByteArrayContent(fileBytes);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                requestContent.Add(imageContent, "file", fileName);

                var client = new HttpClient();
                var response = await client.PostAsync("https://api.ocr.space/parse/image", requestContent);

                HttpContent responseContent = response.Content;

                using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                {
                    string result = await reader.ReadToEndAsync();
                    Info info = JsonConvert.DeserializeObject<Info>(result);
                    if (info.OCRExitCode == 1)
                        textBox1.Text = info.ParsedResults[0].ParsedText;
                    else
                    {
                        for (int i = 0; i < info.ErrorMessage.Length; i++)
                        {
                            textBox1.Text = info.ErrorMessage[i];
                        }
                    }
                }
            }
            else
            {
                textBox1.Text = "Изображение не выбрано";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap image;

            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image = new Bitmap(open_dialog.FileName);
                    filePath = open_dialog.FileName;
                    this.pictureBox1.Size = image.Size;
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate();
                }
                catch
                {
                    DialogResult result = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                switch(comboBox1.SelectedIndex)
                {
                    case 0:
                        language = "ara";
                        break;
                    case 1:
                        language = "bul";
                        break;
                    case 2:
                        language = "chs";
                        break;
                    case 3:
                        language = "cht";
                        break;
                    case 4:
                        language = "hrv";
                        break;
                    case 5:
                        language = "cze";
                        break;
                    case 6:
                        language = "dan";
                        break;
                    case 7:
                        language = "dut";
                        break;
                    case 8:
                        language = "eng";
                        break;
                    case 9:
                        language = "fin";
                        break;
                    case 10:
                        language = "fre";
                        break;
                    case 11:
                        language = "ger";
                        break;
                    case 12:
                        language = "gre";
                        break;
                    case 13:
                        language = "hun";
                        break;
                    case 14:
                        language = "kor";
                        break;
                    case 15:
                        language = "ita";
                        break;
                    case 16:
                        language = "jpn";
                        break;
                    case 17:
                        language = "pol";
                        break;
                    case 18:
                        language = "por";
                        break;
                    case 19:
                        language = "rus";
                        break;
                    case 20:
                        language = "slv";
                        break;
                    case 21:
                        language = "spa";
                        break;
                    case 22:
                        language = "swe";
                        break;
                    case 23:
                        language = "tur";
                        break;
                }
            }
        }
    }

    class Info
    {
        public ParsedResults[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public string[] ErrorMessage { get; set; }
    }

    class ParsedResults
    {
        public string ParsedText { get; set; }
    }
}
