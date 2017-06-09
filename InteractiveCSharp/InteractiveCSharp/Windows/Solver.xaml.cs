using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
namespace InteractiveCSharp
{
    public partial class Solver : Window
    {
        bool compilationError, end = false;
        byte checkerResult;
        string bufferOfOutput;
        string choosingProblemOrTheory = "";
        string pathToCodeFile = Data.pathToProgram + "Text.cs";

        public Solver()
        {
            InitializeComponent();
            if (Data.choosedCourseDirectory.Contains(Data.GetCurrentTheme() + "\\Theory"))
                choosingProblemOrTheory = "";
            else
                choosingProblemOrTheory = "\\Problem";
            LoadTextOfProblem();
            Next.Visibility = Visibility.Hidden;
            if (Data.numOfTests != 0) Course.Visibility = Visibility.Hidden;
            else Course.Visibility = Visibility.Visible;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Results.Document.Blocks.Clear();
            string errText = "";
            string data = "";
            compilationError = false;
            if (!string.IsNullOrWhiteSpace(new TextRange(Code.Document.ContentStart, Code.Document.ContentEnd).Text))
            {
                data = new TextRange(Code.Document.ContentStart, Code.Document.ContentEnd).Text;
                if (choosingProblemOrTheory != "")
                {
                    if (CheckForCoinsidence(data))
                    {
                        string[] CodeText = data.Split('\n');
                        File.WriteAllLines(@"Text.cs", CodeText);
                        Process CompileCSC = CreateCompileProcess();
                        CompileCSC.Start();
                        errText = CompileCSC.StandardOutput.ReadToEnd();
                        if (errText.Length > 412) // шапка вывода компилятора, 412 символов
                        {
                            bufferOfOutput += errText.Substring(412) + "\n";
                            compilationError = true;
                        }
                        CompileCSC.WaitForExit();

                        if (!compilationError)
                        {
                            checkerResult = Checkers.Check(Data.choosedCourseDirectory);
                            switch (checkerResult)
                            {
                                case 0:
                                    bufferOfOutput += "Решение принято!";
                                    break;
                                case 1:
                                    bufferOfOutput += "Ошибка! Неверное решение!";
                                    break;
                                case 2:
                                    bufferOfOutput += "Ошибка! Превышено время ожидания результата!";
                                    break;
                            }
                            if (checkerResult == 0)
                            {
                                Next.Visibility = Visibility.Visible;
                                Send.Visibility = Visibility.Hidden;
                                Directory.CreateDirectory("usersdata");
                                string wasTest = "";
                                if (Data.numOfTests != 0) wasTest = "Test: ";
                                File.AppendAllText("usersdata/" + Data.firstName + " " + Data.lastName, wasTest + "In theme " + Data.GetCurrentTheme() + " was solved " + Data.GetCurrentProblem().Substring(Data.GetCurrentTheme().Length + 1) + " date: " + DateTime.UtcNow + "\n");
                                SendSolveAndResults(data);
                            }
                        }
                    }
                }
                else
                {
                    if (CheckTest(data))
                    {
                        Next.Visibility = Visibility.Visible;
                        Send.Visibility = Visibility.Hidden;
                    }
                }
                Results.Document.Blocks.Add(new Paragraph(new Run(bufferOfOutput)));
                bufferOfOutput = "";
            }
        }

        private void Course_Click(object sender, RoutedEventArgs e)
        {
            Course course = new Course();
            course.Show();
            Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

            if (Data.numOfTests == 0)
            {
                if (!end)
                {
                    GetNextProblemOrNewTheory();
                    if (choosingProblemOrTheory != "")
                    {
                        Next.Visibility = Visibility.Hidden;
                        Send.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                GetNextTest();
                Next.Visibility = Visibility.Hidden;
                Send.Visibility = Visibility.Visible;
            }
            LoadTextOfProblem();
            Results.Document.Blocks.Clear();
            Exit();
        }

        void GetNextTest()
        {
            Random random = new Random();
            Data.choosedCourseDirectory = Data.courseDirectory + Data.test.ElementAt(random.Next(0, Data.test.Count));
            Data.test.Remove(Data.choosedCourseDirectory.Substring(Data.courseDirectory.Length));
            Data.numOfTests--;
            if (Data.numOfTests == 0)
            {
                end = true;
                MessageBox.Show("Поздравляем с завершением тестирования!");
            }
        }

        void GetNextProblemOrNewTheory()
        {
            if (!Data.choosedCourseDirectory.Contains(Data.GetCurrentTheme() + "\\Theory"))
            {
                List<string> currentShortProblems = new List<string>();
                currentShortProblems = Data.GetShortProblemNames(Data.GetCurrentTheme());
                string gk = Data.GetCurrentProblem().Substring(Data.GetCurrentTheme().Length + 1);
                int currentProblemNumber = currentShortProblems.IndexOf(gk);
                if (currentShortProblems.Count - 1 > currentProblemNumber)
                {
                    Data.choosedCourseDirectory = Data.courseDirectory + Data.GetCurrentTheme() + "\\" + currentShortProblems.ElementAt(currentProblemNumber + 1);
                    choosingProblemOrTheory = "\\Problem";
                }
                else
                {
                    int currentCourseNumber = Data.courses.IndexOf(Data.GetCurrentTheme());
                    if (Data.courses.Count - 1 > currentCourseNumber)
                    {
                        choosingProblemOrTheory = "";
                        Data.choosedCourseDirectory = Data.courseDirectory + Data.courses.ElementAt(currentCourseNumber + 1) + "\\Theory";
                    }
                    else
                    {
                        MessageBox.Show("Поздравляем с окончанием курса!");
                        end = true;
                    }
                }
            }
            else
            {
                choosingProblemOrTheory = "\\Problem";
                Data.choosedCourseDirectory = Data.courseDirectory + Data.GetCurrentTheme() + "\\" + Data.GetFirstProblem(Data.GetCurrentTheme());
            }
        }

        void Exit()
        {
            if (end)
            {
                MainWindow main = new MainWindow();
                main.Show();
                Close();
            }
        }

        bool CheckTest(string data)
        {
            string[] tests = File.ReadAllText(Data.choosedCourseDirectory.Replace("Theory", "Tests")).Split('\n');
            string[] answers = data.Split('\n');
            for (int i = 0; i < tests.Length; i++)
                if (tests[i].Replace("\r", String.Empty).Replace("\n", String.Empty) != answers[i].Replace("\r", String.Empty).Replace("\n", String.Empty))
                {
                    bufferOfOutput += "Ошибка! Неверное решение " + (i + 1) + "ого вопроса!";
                    return false;
                }
            bufferOfOutput += "Решение принято! Можно приступить к практической части.";
            return true;
        }

        bool CheckForCoinsidence(string data)
        {
            data = data.Replace(" ", string.Empty);
            foreach (string s in GetAllSolves())
                if (string.Compare(data, s) == 0)
                {
                    bufferOfOutput += "Это решение уже отправлялось на сервер!";
                    return false;
                }
            bufferOfOutput += "Данные приняты!\n";
            return true;
        }

        void SendSolveAndResults(string data)
        {
            File.AppendAllText("test/" + Data.firstName + " " + Data.lastName, File.ReadAllText("usersdata/" + Data.firstName + " " + Data.lastName));
            File.AppendAllText("test/" + Data.firstName + " " + Data.lastName + "Code", data);
        }

        List<string> GetAllSolves()
        {
            return new List<string>();
        }

        Process CreateCompileProcess()
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Data.pathToDotNet,
                    Arguments = pathToCodeFile,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
        }
        void LoadTextOfProblem()
        {
            Task.Document.Blocks.Clear();
            Task.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(Data.choosedCourseDirectory + choosingProblemOrTheory, System.Text.Encoding.GetEncoding(1251)))));
        }
    }
}
