using System;

namespace ClimaDrumond.Helpers
{
	using System;
	using System.Globalization;

	public class CultureChangeEventArgs : EventArgs
	{
		public CultureInfo NewCulture
		{
			get;
		}

		public CultureInfo OldCulture
		{
			get;
		}

		public CultureChangeEventArgs(CultureInfo oldCulture, CultureInfo newCulture)
		{ }
	}
}