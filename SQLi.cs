using System;
using System.Collections.Generic;

class SQLInterpreter
{
    static void Main(String[] args)
    {
        if (args.Length < 1)
        {
            return;
        }
        Database D = new Database(args[0]);
        string input;
        Console.Write("?> ");
        while(!(input = Console.ReadLine()).Equals(":q"))
        {
            try
            {
                Console.WriteLine(D.ProcessQuery(input));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            Console.Write("?> ");
        }
        Console.WriteLine("Done");
        D.Write();
    }
}
