using System;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;
using System.Diagnostics;

namespace tutor
{
    public partial class Default : Page
    {
        public static List<String> allSolv = new List<String>();
        bool coincidence, compilationError;
        byte checkerResult;
        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            string data = "";
            coincidence = false;
            compilationError = false;
            if (IsPostBack)
            {
                if (!String.IsNullOrWhiteSpace(Code.Text))
                {
                    data = Server.HtmlEncode(Code.Text);
                    data = data.Replace(" ", string.Empty);
                    foreach (string s in allSolv)
                        if (String.Compare(data, s) == 0)
                        {
                            coincidence = true;
                            break;
                        }
                    if (coincidence == false)
                    {

                        Label1.Text = "Данные приняты!";
                        allSolv.Add(data);
                        string[] codeText = Code.Text.Split('\n');
                        File.WriteAllLines(@"Text.cs", codeText);
                        // компиляция файла с кодом
                        var compileCSC = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe",
                                Arguments = @"Text.cs",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            }
                        };
                        compileCSC.Start();
                        string errText = compileCSC.StandardOutput.ReadToEnd();
                        if (errText.Length > 412) // шапка вывода компилятора, 412 символов
                        {
                            Label1.Text = errText.Substring(412);
                            compilationError = true;
                        }
                        compileCSC.WaitForExit();
                        if (!compilationError)
                        {
                            checkerResult = checker("333 33", "366");
                            switch (checkerResult)
                            {
                                case 0:
                                    Label1.Text = "Решение принято!";
                                    break;
                                case 1:
                                    Label1.Text = "Ошибка! Неверное решение!";
                                    break;
                                case 2:
                                    Label1.Text = "Ошибка! Превышено время ожидания результата!";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Label1.Text = "Это решение уже отправлялось на сервер!";
                    }
                }
            }
        }
        byte checker(string input, string output) // возврат 0 - удачно; 1 - неверный ответ; 2 - превышено время ожидания
        {
            var compiledFile = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"Text.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true

                }
            };
            compiledFile.Start();
            compiledFile.StandardInput.WriteLine(input);
            bool waitCheck = compiledFile.WaitForExit(1000);
            if (!waitCheck)
            {
                compiledFile.Kill();
                return 2;
            }
            if (compiledFile.StandardOutput.ReadLine() == output)
                return 0;
            return 1;
        }
    }
}
/*
using System;
    class Program
    {
        static void Main(string[] args)
        {
            string[] tokens = Console.ReadLine().Split(' ');
            Console.WriteLine(int.Parse(tokens[0]) - int.Parse(tokens[1]));
        }
    }
*/
