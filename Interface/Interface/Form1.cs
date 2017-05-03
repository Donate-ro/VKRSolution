using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Interface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random random = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://acm.timus.ru/submit.aspx?space=1");
                request.Method = "POST";
                request.Timeout = 100000;
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.GetEncoding(1251).GetBytes("Action=submit&SpaceID=1&JudgeID=" + textBox1.Text + "&Language=27&ProblemNum=1000&+Source=" + richTextBox1.Text + "\"");  // 226001CH
                request.ContentLength = sentData.Length;
                Stream sendStream = request.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                WebResponse result = request.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                StreamReader streamReader = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
                string streamReasult = streamReader.ReadToEnd();
                webBrowser1.DocumentText = streamReasult;
                streamReader.Close();
            }
            catch { webBrowser1.DocumentText = "Ошибка соединения с сервером"; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int randomValue = random.Next(1000, 2110);
                while ((randomValue == 1897) || (randomValue == 1898) || (randomValue == 1899)) randomValue = random.Next(1000, 2110);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://acm.timus.ru/problem.aspx?space=1&num=" + randomValue + "&locale=ru");
                label3.Text = "Номер задачи - " + randomValue;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string streamResult = streamReader.ReadToEnd();
                streamReader.Close();
                int startIMG = 0;
                while (startIMG != -1)
                {
                    startIMG = streamResult.IndexOf(@"IMG SRC=""/");
                    streamResult = streamResult.Insert(startIMG + 9, "http://acm.timus.ru");
                }

                int indexOfSource = streamResult.IndexOf("<DIV CLASS=\"problem_source\">");
                int indexOfPar = streamResult.IndexOf("<DIV CLASS=\"problem_par\">");
                streamResult = streamResult.Substring(indexOfPar, indexOfSource - indexOfPar);
                webBrowser1.DocumentText = streamResult;
            }
            catch { webBrowser1.DocumentText = "Ошибка соединения с сервером"; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.Navigate("http://acm.timus.ru/status.aspx");
            }
            catch { webBrowser1.DocumentText = "Ошибка соединения с сервером"; }
        }
    }
}