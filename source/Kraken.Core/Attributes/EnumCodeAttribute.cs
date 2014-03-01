using System;

namespace Kraken.Core
{
	/// <summary>
	/// This Attribute can be applied to an enumeration and then used to cast to / from a default
	/// database char(1) value (for cases when a char mapping is used instead of an int)
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field)]
	public class EnumCodeAttribute : Attribute
	{
		public string Code { get; private set; }
	
		/// <summary>
		/// Used to mark what an enum casts to as its short varchar code
		/// </summary>
		public EnumCodeAttribute(string code)
		{
			Code = code;
		}
	}
}
