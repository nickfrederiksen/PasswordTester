using System;

namespace PasswordTester.Console
{
    class Program
    {
        static void Main(string[] args)
        {	
			while (true)
			{
				System.Console.Write("Enter password to test: ");
				var password = System.Console.ReadLine();
				var resultTask = PasswordLookup.Lookup(password);
				resultTask.ConfigureAwait(false);
				resultTask.Wait();
				var result = resultTask.Result;

				if (result)
				{
					System.Console.WriteLine($"Password is public and was found {result.HitCount} time(s)");
				}
				else
				{
					System.Console.WriteLine("Password was not found in the wild.");
				}

				System.Console.WriteLine();
			}
        }
    }
}
