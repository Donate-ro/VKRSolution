using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Dipl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static List<string> allSolv = new List<string>();
        bool coincidence, compilationError;
        byte checkerResult;
        string problem;

        private void button1_Click(object sender, EventArgs e)
        {
            string errText = "";
            string data = "";
            coincidence = false;
            compilationError = false;
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                data = richTextBox1.Text;
                data = data.Replace(" ", string.Empty);
                foreach (string s in allSolv)
                    if (string.Compare(data, s) == 0)
                    {
                        coincidence = true;
                        break;
                    }
                //if (CheckForConsidence())
                {
                    richTextBox2.Text = "Данные приняты!\n";
                    allSolv.Add(data);
                    string[] CodeText = richTextBox1.Text.Split('\n');
                    File.WriteAllLines(@"Text.cs", CodeText);
                    // компиляция файла с кодом
                    Process CompileCSC = CreateCompileProcess();
                    CompileCSC.Start();
                    errText = CompileCSC.StandardOutput.ReadToEnd();
                    if (errText.Length > 412) // шапка вывода компилятора, 412 символов
                    {
                        richTextBox2.Text = errText.Substring(412);
                        compilationError = true;
                    }
                    CompileCSC.WaitForExit();
                    
                    if (!compilationError)
                    {
                        checkerResult = Checkers.Check(problem);
                        switch (checkerResult)
                        {
                            case 0:
                                richTextBox2.Text += "Решение принято!";
                                break;
                            case 1:
                                richTextBox2.Text += "Ошибка! Неверное решение!";
                                break;
                            case 2:
                                richTextBox2.Text += "Ошибка! Превышено время ожидания результата!";
                                break;
                        }
                        if(checkerResult==0) button2.Enabled = true;
                    }
                }
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Интерактивный учебник по языку программирования C#";
            label1.Text = "Проверка решения";
            label2.Text = "Текст программы";
            button1.Text = "Отправить";
            button2.Text = "Далее";
            button2.Enabled = false;
            problem = @"C:\Users\joke4\OneDrive\Dipl\data\Chapter1\a+b problem";
            richTextBox3.Text = File.ReadAllText(problem+"/Problem.txt");
        }

        bool CheckForConsidence()
        {
            if (!coincidence) return true;
            else
            {
                richTextBox2.Text = "Это решение уже отправлялось на сервер!";
                return false;
            }
        }

        Process CreateCompileProcess()
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe",
                    Arguments = @"C:\Users\joke4\OneDrive\Dipl\Dipl\bin\Debug\Text.cs",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
        }
    }
}