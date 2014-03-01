using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UnitTests.TestClasses
{
    /// <summary>
    /// Make a data table with some data for testing against
    /// </summary>
    public class ColourDataTable : DataTable
    {
        #region Fields
        private const string ColumnId = "Id";
        private const string ColumnName = "Name";
        private const string ColumnRgb = "Rgb";
        private const string ColumnDateCreated = "DateCreated";
        #endregion

        #region Constructors
        public ColourDataTable()
        {
            Columns.Add(ColumnId, typeof(int));
            Columns.Add(ColumnName, typeof(string));
            Columns.Add(ColumnRgb, typeof(string));
            Columns.Add(ColumnDateCreated, typeof(DateTime));

            AcceptChanges();

            DateTime anchorDate = DateTime.Parse("2010-03-29 16:56");
            AddNewRow(1, "Red", "FF0000", anchorDate);
            AddNewRow(2, "Green", "00FF00", anchorDate.AddDays(-1));
            AddNewRow(3, "Blue", "0000FF", anchorDate.AddMilliseconds(500));
        }
        #endregion

        #region Instance Methods
        private void AddNewRow(int id, string name, string rgb, DateTime dateCreated)
        {
            
            DataRow row = NewRow();

            row["id"] = id;
            row["Name"] = name;
            row["rgb"] = rgb;
            row["DateCreated"] = dateCreated;

            Rows.Add(row);
        }
        #endregion
    }
}
