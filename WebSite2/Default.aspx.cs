using System;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace Diplom
{
    public partial class Default : Page
    {

        public static List<String> allSolv = new List<String>();
        bool coincidence, compilationError;
        byte checkerResult;
        string problem= @"G:\OneDrive\Dipl\data\Chapter1\a+b problem";

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            Response.Redirect("StartPage.html");
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
                            checkerResult = Checkers.Check(problem);
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
        
    }
}
/*
using System;
    class Program
    {
        static void Main(string[] args)
        {
            string[] tokens = Console.ReadLine().Split(' ');
            Console.WriteLine(int.Parse(tokens[0]) + int.Parse(tokens[1]));
        }
    }
*/
