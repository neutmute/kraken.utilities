using System;

namespace UnitTests
{
	/// <summary>
	/// Animal is a class that can be used to test the code generator
	/// </summary>
	/// <date>7-May-2007</date>
	public class Animal : UnitTests.TestClasses.IAnimal
	{
		#region Fields
		private int _LegCount;
		private int _StomachCount;
		#endregion

		#region Properties
		/// <summary>
		/// Public field
		/// </summary>
		public string Sound;

		/// <summary>
		/// Public property
		/// </summary>
		public int LegCount
		{
			get { return _LegCount; }
			set { _LegCount = value; }
		}

		/// <summary>
		/// Public property
		/// </summary>
		private int StomachCount
		{
			get { return _StomachCount; }
			set { _StomachCount = value; }
		}
		#endregion

		#region Constructors
		public Animal()
		{
			_StomachCount = 4;
		}
		#endregion
	}
}
