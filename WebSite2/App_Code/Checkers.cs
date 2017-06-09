using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Сводное описание для Class1
/// </summary>
public static class Checkers
{
        static byte Checker(string input, string output)
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
        static public byte Check(string problemPath)
        {
            List<string> files = new List<string>();
            files = Directory.EnumerateFiles(problemPath + "/input/").ToList();
            byte checkerResult = new byte();
            foreach (var file in files)
            {
                checkerResult = Checker(File.ReadAllLines(file).ElementAt(0), File.ReadAllLines(Directory.EnumerateFiles(problemPath + "/output/").ToList().ElementAt(files.IndexOf(file))).ElementAt(0));
                if (checkerResult != 0) return checkerResult;
            }
            return checkerResult;
        }
}