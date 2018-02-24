using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PasswordTester.Test
{
	[TestClass]
	public class ResultTests
	{
		[DataTestMethod]
		[DataRow(10,true)]
		[DataRow(0, false)]
		[DataRow(-1, false)]
		public void TestHitResult(int hitCount, bool hitAssert)
		{
			var lookupResult = new PasswordLookupResult()
			{
				HitCount = hitCount
			};

			Assert.AreEqual(hitAssert, lookupResult);
			Assert.AreEqual(hitCount, lookupResult);
		}
	}
}
