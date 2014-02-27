using System;
using System.Text;


namespace UnitTests
{
	/// <summary>
	/// Used for testing the case where no properties are exposed by a class
	/// in which case the ToString() is evaluated.
	/// Previously the enumeration code executed and so generated non compiling code
	/// </summary>
	public class AbsoluteBearing
	{
		#region Fields
		Decimal _decimal;
		#endregion

		#region Constructors
		public AbsoluteBearing(Decimal value)
		{
			_decimal = Math.Abs(value);
		}
		#endregion

		#region Instance Methods
		public override string ToString()
		{
			return _decimal.ToString("N8");
		}
            
        public string FormatAsString(string format)
        {
            return (format != null) ? _decimal.ToString(format) : this.ToString();
        }

	    #endregion
	}
}

 
