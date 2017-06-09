using System.Collections.Generic;

namespace InteractiveCSharp
{
    static class Data
    {
        static public string courseDirectory;
        static public string choosedCourseDirectory;
        static public List<string> courses = new List<string>();
        static public List<string> problems = new List<string>();
        static public string firstName;
        static public string lastName;
        static public string mode;
        static public List<string> test = new List<string>();
        static public int numOfTests = 0;
        static public string pathToProgram;
        static public string pathToDotNet;

        public static List<string> GetShortProblemNames(string course)
        {
            List<string> newProblems = new List<string>();
            foreach (var problem in problems)
                if (problem.Contains(course)) newProblems.Add(problem.Substring(course.Length + 1));
            return newProblems;
        }

        public static string GetCurrentTheme()
        {
            foreach (var course in courses) if (choosedCourseDirectory.Contains(course)) return course;
            return null;
        }
        public static string GetCurrentProblem()
        {
            foreach (var problem in problems) if (choosedCourseDirectory.Contains(problem)) return problem;
            return null;
        }
        public static string GetFirstProblem(string course)
        {
            foreach (var problem in problems) if (problem.Contains(course)) return problem.Substring(course.Length + 1);
            return null;
        }
    }
}
