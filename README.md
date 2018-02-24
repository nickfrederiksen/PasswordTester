# PasswordTester
.NET wrapper for Troy Hunts Password API: https://haveibeenpwned.com/API/v2#SearchingPwnedPasswordsByRange


Read more about the password leaks and service on Troys blog: https://www.troyhunt.com/ive-just-launched-pwned-passwords-version-2/

## Usage

``` csharp
var result = PasswordTester.PasswordLookup.Lookup("SomePassword");

var hasHit = result.HasHit;
var hitCount = result.HitCount;

if(result == true)
{
    //  has hit count > 0
}

if(result >= 100)
{
    // has hit count >= 100
}

```