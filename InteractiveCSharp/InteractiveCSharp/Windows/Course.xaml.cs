using System.Windows;

namespace InteractiveCSharp
{
    /// <summary>
    /// Логика взаимодействия для Course.xaml
    /// </summary>
    public partial class Course : Window
    {
        public Course()
        {
            InitializeComponent();
            Themes.ItemsSource = Data.courses;
        }

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            string choosedTheme = Themes.SelectedItem.ToString();
            if (Problems.SelectedItem == null) Data.choosedCourseDirectory = Data.courseDirectory + choosedTheme + "\\Theory";
            else Data.choosedCourseDirectory = Data.courseDirectory + choosedTheme + "\\" + Problems.SelectedItem.ToString();
            Solver solver = new Solver();
            solver.Show();
            Close();
        }

        private void ThemeOpen_Click(object sender, RoutedEventArgs e)
        {
            Problems.ItemsSource = Data.GetShortProblemNames(Themes.SelectedItem.ToString());
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Data.choosedCourseDirectory == null)
            {
                MainWindow main = new MainWindow();
                main.Show();
            }
            else
            {
                Solver solver = new Solver();
                solver.Show();
            }
            Close();
        }
    }
}
