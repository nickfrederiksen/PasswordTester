using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordTester
{
    public static class Client
	{
		public static readonly HttpClient httpClient = new HttpClient();

		private static async Task<(bool hasHit, int hitCount)> TestPassword(string password)
		{
			var hash = HashPassword(password);
			var subStr = hash.Substring(0, 5);

			var url = $"https://api.pwnedpasswords.com/range/{subStr}";
			var response = await httpClient.GetAsync(url).ConfigureAwait(false);

			return await ParseResponse(response, hash, subStr).ConfigureAwait(false);

		}

		private static async Task<(bool hasHit, int hitCount)> ParseResponse(HttpResponseMessage response, string hash, string subStr)
		{
			var hitCount = -1;
			var hasHit = false;

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
				var firstHit = lines.FirstOrDefault(line => $"{subStr}{line}".StartsWith(hash));
				hasHit = firstHit != null;
				if (hasHit == true)
				{
					hitCount = ParseHitCount(firstHit);
				}
			}


			return (hasHit, hitCount);
		}

		private static int ParseHitCount(string hitLine)
		{
			var hitCount = -1;
			var lineParts = hitLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			var hitCountPart = lineParts.ElementAtOrDefault(1);
			if (hitCountPart != null && int.TryParse(hitCountPart, out var parsedHitCount))
			{
				hitCount = parsedHitCount;
			}

			return hitCount;
		}

		private static string HashPassword(string password)
		{
			using (SHA1Managed sha1 = new SHA1Managed())
			{
				var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
				var sb = new StringBuilder(hash.Length * 2);

				foreach (byte b in hash)
				{
					sb.Append(b.ToString("X2"));
				}

				return sb.ToString();
			}

		}
	}
}
