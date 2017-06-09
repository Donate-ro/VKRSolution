using System;

public class Sum
{
    private static void Main()
    {
        string[] tokens = Console.ReadLine().Split(' ');
        Console.WriteLine(int.Parse(tokens[0]) + int.Parse(tokens[1]));
    }
}
