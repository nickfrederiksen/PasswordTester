using System.Threading.Tasks;

namespace PasswordTester.Console
{
	class Program
    {
        static async Task Main(string[] args)
        {	
			while (true)
			{
				System.Console.Write("Enter password to test: ");
				var password = System.Console.ReadLine();
				var result = await PasswordLookup.LookupAsync(password);

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
