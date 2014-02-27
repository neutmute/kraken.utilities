using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    /// <summary>
    /// A class should supply a method that returns this object if it wants to use the ObjectDumper
    /// </summary>
    public class ObjectDump
    {
        #region Properties
        public List<string> Headers { get; set; }

        public List<string> Data { get; set; }
        #endregion

        #region Constructors
        public ObjectDump()
        {
            Headers = new List<string>();
            Data = new List<string>();
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Simple dump of an object, designed to make a ToString easy to implement
        /// </summary>
        /// <remarks>
        /// Not useful for the real intention of ObjectDumper which is a clean tabular representation
        /// </remarks>
        public override string ToString()
        {
            List<string> nameValuePairs = new List<string>();
            for (int i = 0; i < Data.Count; i++)
            {
                nameValuePairs.Add(string.Format("{0}={1}", Headers[i], Data[i]));
            }
            return nameValuePairs.ToCsv(", ");
        }
        #endregion

        #region static Methods
        /// <summary>
        /// for compound classes (eg: Atex Billing AtexBillChartFieldMapping.cs)
        /// </summary>
        public static ObjectDump Merge(params ObjectDump[] dumpsToMerge)
        {
            ObjectDump mergedDump = new ObjectDump();

            foreach (ObjectDump mergee in dumpsToMerge)
            {
                mergedDump.Headers.AddRange(mergee.Headers);
                mergedDump.Data.AddRange(mergee.Data);
            }

            return mergedDump;
        }
        #endregion
    }
}
