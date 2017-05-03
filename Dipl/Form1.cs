using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Dipl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public delegate void InvokeDelegate();
        System.Diagnostics.Process CompiledFile = new System.Diagnostics.Process();
        StreamWriter myStreamWriter;
        StreamReader myStreamReader;
        char outputText;

        private void button1_Click(object sender, EventArgs e)
        {
            //outputText = '';
            string errText = "";
            richTextBox3.Text = "";
            bool error = false;
            // передача содержимого RichTextBox в файл
            string[] CodeText = richTextBox1.Text.Split('\n');
            File.WriteAllLines(@"Text.cs", CodeText);
            // компиляция файла с кодом
            System.Diagnostics.Process CompileCSC = new System.Diagnostics.Process();
            CompileCSC.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
            CompileCSC.StartInfo.Arguments = @"C:\Users\joke4\OneDrive\Dipl\Dipl\bin\Debug\Text.cs";
            CompileCSC.StartInfo.UseShellExecute = false;
            CompileCSC.StartInfo.RedirectStandardOutput = true;
            CompileCSC.StartInfo.CreateNoWindow = true;
            CompileCSC.Start();
            errText = CompileCSC.StandardOutput.ReadToEnd();
            if (errText.Length > 412) // шапка вывода компилятора, 412 символов
            {
                richTextBox2.Text = errText.Substring(412);
                error = true;
            }
            CompileCSC.WaitForExit();
            // проверка на отсутствие ошибок и запуск скомпилированного приложения
            if (!error)
            {
                richTextBox2.Text = "";
                CompiledFile.Start();
                checkBox1.Checked = true;
                myStreamWriter = CompiledFile.StandardInput;
                myStreamReader = CompiledFile.StandardOutput;
                new Thread(ThreadRefreshRichTextBox).Start();
            }
        }

        public void ThreadRefreshRichTextBox()
        {
            while (!CompiledFile.StandardOutput.EndOfStream)
            {
                outputText = (char)myStreamReader.Read();
                if ((outputText != '\r') && (outputText != 65535)) richTextBox3.Invoke(new InvokeDelegate(GetText));

            }
            richTextBox3.Invoke(new InvokeDelegate(ChekBoxFalse));

        }

        public void GetText()
        {
            richTextBox3.Text += outputText;
        }

        public void ChekBoxFalse()
        {
            checkBox1.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox3.Text += textBox1.Text + '\n';
            myStreamWriter.Write(textBox1.Text);
            myStreamWriter.Write('\n');
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            CompiledFile.StartInfo.FileName = "Text.exe";
            CompiledFile.StartInfo.UseShellExecute = false;
            CompiledFile.StartInfo.RedirectStandardInput = true;
            CompiledFile.StartInfo.RedirectStandardOutput = true;
            CompiledFile.StartInfo.CreateNoWindow = true;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) CompiledFile.Kill();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox3.Text += textBox1.Text + '\n';
            myStreamWriter.Write(textBox1.Text);
            myStreamWriter.Write('\n');
        }
    }
}