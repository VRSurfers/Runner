using System;
using System.Diagnostics;

class CommonHelpers
{
	[Conditional("DEBUG")]
	public static void AssertIfTrue(bool condition)
	{
		if (!condition)
			throw new InvalidOperationException();
	}
}
