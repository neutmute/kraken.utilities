using System;

namespace UnitTests
{
	/// <summary>
	/// Configuration simulates a class with static properties that need testing
	/// </summary
	/// <date>6-Sep-2007</date>
	public class Configuration
	{
        #region Fields
        private static string _HiddenStaticField = "booyakasha";
        #endregion

        #region Properties
        public static string ConfigValue1
		{
			get{return "value1"; }	
		}

		public static int ConfigValue2
		{
			get{return -1; }
		}
        #endregion

        #region Constructors
        private static string ConfigValueHidden
        {
            get { return _HiddenStaticField; }
            set { _HiddenStaticField = value; }
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Test calling static methods 
        /// </summary>
	    private static void SetHiddenStaticField(string value)
        {
            _HiddenStaticField = value; 
        }
        #endregion
	}
}
