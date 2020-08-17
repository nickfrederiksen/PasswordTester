using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("PasswordTester.Test")]
namespace PasswordTester
{
	/// <summary>
	/// Helper class to look up passwords in a public directory.
	/// </summary>
	public static class PasswordLookup
	{
		private const string ServiceURL = "https://api.pwnedpasswords.com/range/";
		private static readonly HttpClient httpClient = HttpClientManager.GetHttpClient();

		/// <summary>
		/// Looks up the password in a public directory.
		/// </summary>
		/// <param name="password">The password to look up.</param>
		/// <returns>The lookup result</returns>
		public static async Task<PasswordLookupResult> LookupAsync(string password)
		{
			var hash = HashPassword(password);
			var subStr = hash.Substring(0, 5);

			var url = $"{ServiceURL}{subStr}";
			using (var response = await httpClient.GetAsync(url).ConfigureAwait(false))
			{
				response.EnsureSuccessStatusCode();

				return await ParseResponseAsync(response, hash, subStr).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Looks up the password in a public directory.
		/// </summary>
		/// <param name="password">The password to look up.</param>
		/// <returns>The lookup result</returns>
		[Obsolete("Use LookupAsync instead.")]
		public static Task<PasswordLookupResult> Lookup(string password)
		{
			return LookupAsync(password);
		}

		internal static async Task<PasswordLookupResult> ParseResponseAsync(HttpResponseMessage response, string hash, string subStr)
		{
			var hitCount = 0;
			var hasHit = false;

			if (response.IsSuccessStatusCode)
			{
				IEnumerable<string> lines = await ParseLinesAsync(response).ConfigureAwait(false);
				var firstHit = lines.FirstOrDefault(line => $"{subStr}{line}".StartsWith(hash));
				hasHit = firstHit != null;
				if (hasHit == true)
				{
					hitCount = ParseHitCount(firstHit);
				}
			}

			return new PasswordLookupResult()
			{
				HitCount = hitCount
			};
		}

		internal static async Task<IEnumerable<string>> ParseLinesAsync(HttpResponseMessage response)
		{
			var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Where(l => !l.EndsWith(":0"));
			return lines;
		}

		internal static int ParseHitCount(string hitLine)
		{
			var hitCount = 0;
			var lineParts = hitLine.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			var hitCountPart = lineParts.ElementAtOrDefault(1);
			if (hitCountPart != null && int.TryParse(hitCountPart, out var parsedHitCount))
			{
				hitCount = parsedHitCount;
			}

			return hitCount;
		}

		internal static string HashPassword(string password)
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
