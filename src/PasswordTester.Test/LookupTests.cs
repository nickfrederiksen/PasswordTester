using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordTester.Test
{
	[TestClass]
	public class LookupTests
    {
		[DataTestMethod]
		[DataRow("0018A45C4D1DEF81644B54AB7F969B88D65:1",1)]
		[DataRow("00D4F6E8FA6EECAD2A3AA415EEC418D38EC:2",2)]
		[DataRow("012A7CA357541F0AC487871FEEC1891C49C:2232",2232)]
		public void TestParseHitCount(string line, int hitCountAssert)
		{
			var hitCount = PasswordLookup.ParseHitCount(line);
			Assert.AreEqual(hitCountAssert, hitCount);
		}

		[DataTestMethod]
		[DataRow("AwesomeTestPassword", "5960f5a135991dd2b1348420a1cc1a24d849fe73")]
		[DataRow("Test1234", "dddd5d7b474d2c78ebbb833789c4bfd721edf4bf")]
		[DataRow("123456", "7c4a8d09ca3762af61e59520943dc26494f8941b")]
		[DataRow("!FsX2sQSKK&*uZ8FtBEj", "542b8b29feeefe28f2026264ccf3ee554fcd839b")]
		public void TestSHA1Hash(string password, string hash)
		{
			var result = PasswordLookup.HashPassword(password);

			Assert.AreEqual(hash, result,true);
		}
    }
}
