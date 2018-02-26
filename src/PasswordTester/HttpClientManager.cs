using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace PasswordTester
{
	internal static class HttpClientManager
	{
		internal static HttpClient GetHttpClient()
		{
			var httpClient = new HttpClient();
			string version = GetAssemblyVersion();

			string userAgentString = $"PasswordTester-API-Wrapper (v{version})";
			httpClient.DefaultRequestHeaders.Add("User-Agent", userAgentString);

			return httpClient;
		}

		internal static string GetAssemblyVersion()
		{
			var assembly = Assembly.GetCallingAssembly();
			var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
			var version = fileVersion.FileVersion;
			return version;
		}
	}
}
