using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InteractiveCSharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Data.pathToProgram = @"G:\OneDrive\Dipl\InteractiveCSharp\InteractiveCSharp\bin\Debug\";
            Data.pathToProgram = "";
            Data.pathToDotNet = @"C:\Windows\Microsoft.NET\Framework" + @"\v4.0.30319\csc.exe";
            //Data.courseDirectory = @"G:\OneDrive\Dipl\data\";
            Data.courseDirectory= @"data\";
            Data.courses = LoadCourses();
            Data.problems = LoadProblems();
        }

        List<string> LoadCourses()
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.EnumerateDirectories(Data.courseDirectory).ToList())
            {
                files.Add(file.Substring(Data.courseDirectory.Length));
            }
            return files;
        }

        List<string> LoadProblems()
        {
            List<string> files = new List<string>();
            foreach (var course in Data.courses)
                foreach (var problem in Directory.EnumerateDirectories(Data.courseDirectory + course + @"\").ToList())
                    files.Add(problem.Substring(Data.courseDirectory.Length));
            return files;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Data.firstName = FirstName.Text;
            Data.lastName = SecondName.Text;
            if (Data.mode == "Аттестация - 1")
            {
                GetRandomTest(0, Data.courses.Count / 2);
            }
            if (Data.mode == "Аттестация - 2")
            {
                GetRandomTest(Data.courses.Count / 2, Data.courses.Count);
            }
            if (Data.mode == "Экзамен")
            {
                GetRandomTest(0, Data.courses.Count);
            }
            if (Data.mode == "Обучение")
            {
                Course course = new Course();
                course.Show();
            }
            else
            {
                Solver solver = new Solver();
                solver.Show();
            }
            Close();
        }

        void GetRandomTest(int begin, int end)
        {
            Random random = new Random();
            for (int i = begin; i < end; i++)
            {
                foreach (var problem in Data.problems) if (problem.Contains(Data.courses.ElementAt(i))) Data.test.Add(problem);
            }
            Data.numOfTests = int.Parse(NumberOfTests.Text);
            Data.choosedCourseDirectory = Data.courseDirectory + Data.test.ElementAt(random.Next(0, Data.test.Count));
            Data.test.Remove(Data.choosedCourseDirectory.Substring(Data.courseDirectory.Length));
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radiobutton = (RadioButton)sender;
            Data.mode = radiobutton.Content.ToString();
            if (Data.mode != "Обучение")
            {
                TestLabel.Visibility = Visibility.Visible;
                NumberOfTests.Visibility = Visibility.Visible;
            }
            else
            {
                TestLabel.Visibility = Visibility.Hidden;
                NumberOfTests.Visibility = Visibility.Hidden;
            }
        }
    }
}
