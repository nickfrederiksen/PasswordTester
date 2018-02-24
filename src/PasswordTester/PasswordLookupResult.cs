using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordTester
{
	public class PasswordLookupResult
	{
		public int HitCount { get; set; }

		public bool HasHit
		{
			get
			{
				return HitCount > 0;
			}
		}

		public static implicit operator bool(PasswordLookupResult passwordLookupResult)
		{
			return passwordLookupResult.HasHit;
		}

		public static implicit operator int(PasswordLookupResult passwordLookupResult)
		{
			return passwordLookupResult.HitCount;
		}
	}
}
