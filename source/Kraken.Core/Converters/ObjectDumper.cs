using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    #region enum KrakenHorizontalAlign
    /// <summary>
    /// Prefix to avoid namespace collision with System.Web.UI.WebControls
    /// </summary>
    public enum KrakenHorizontalAlign
    {
        Left,
        Right
    }
    #endregion
    
    /// <summary>
    /// Dumps an IEnumerable object - useful for marshal or logs
    /// </summary>
    public class ObjectDumper<T>
    {
        #region Properties
        public KrakenHorizontalAlign KrakenHorizontalAlignDefault { get; set; }
        private readonly Func<T, ObjectDump> _dumpDataMethod;
        #endregion

        #region Constructors
        public ObjectDumper(Func<T, ObjectDump> getDumpMethod)
        {
            KrakenHorizontalAlignDefault = KrakenHorizontalAlign.Left;
            _dumpDataMethod = getDumpMethod;
        }

        public ObjectDumper()
        {
            KrakenHorizontalAlignDefault = KrakenHorizontalAlign.Left;
        }
        #endregion

        #region Instance Methods
        public string Dump(T target)
        {
            return Dump(new List<T> {target});
        }
        
        public string Dump(IEnumerable<T> target)
        {
            if (_dumpDataMethod == null)
            {
                throw KrakenException.Create("You used the wrong constructor for ObjectDumper");
            }
            var objectDumpList = new List<ObjectDump>();
            foreach (T obj in target)
            {
                objectDumpList.Add(_dumpDataMethod(obj));
            }
            return Dump(objectDumpList);
        }

        public string Dump(List<ObjectDump> objectDumpList)
        {
            List<List<string>> grid = new List<List<string>>();
            bool addedHeaders = false;

            // Collect the data
            foreach (ObjectDump dumpedObject in objectDumpList)
            {
                if (!addedHeaders)
                {
                    grid.Add(dumpedObject.Headers);
                    addedHeaders = true;
                }

                List<string> data = dumpedObject.Data;
                grid.Add(data);
            }

            // Now determine the maximum widths of each column
            List<int> maxColumnWidths = new List<int>();
            for (int rowIndex = 0; rowIndex < grid.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < grid[rowIndex].Count; columnIndex++)
                {
                    if (grid[rowIndex][columnIndex] == null)
                    {
                        grid[rowIndex][columnIndex] = "(null)";
                    }

                    int lengthThisCell = grid[rowIndex][columnIndex].Length;
                    if (maxColumnWidths.Count <= columnIndex)
                    {
                        maxColumnWidths.Add(lengthThisCell);
                    }
                    else
                    {
                        if (maxColumnWidths[columnIndex] < lengthThisCell)
                        {
                            maxColumnWidths[columnIndex] = lengthThisCell;
                        }
                    }
                }
            }

            const char delineatorChar = '-';
            const string columnSeparator = "  ";

            if (grid.Count > 0)
            {
                // Insert header/data delineators now we know the max widths
                List<string> delineators = new List<string>();
                for (int i = 0; i < maxColumnWidths.Count; i++)
                {
                    delineators.Add(string.Empty.PadRight(maxColumnWidths[i], delineatorChar));
                }
                grid.Insert(1, delineators);
            }

            // Now we can render
            StringBuilder stringBuilder = new StringBuilder();
            for (int rowIndex = 0; rowIndex < grid.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < grid[rowIndex].Count; columnIndex++)
                {
                    stringBuilder.Append(columnSeparator);
                    stringBuilder.Append(GetColumnData(grid[rowIndex][columnIndex], maxColumnWidths[columnIndex]));
                }
                stringBuilder.AppendLine(string.Empty);
            }

            return stringBuilder.ToString();
        }
        
        private string GetColumnData(string inputValue, int maxWidth)
        {
            if (KrakenHorizontalAlignDefault == KrakenHorizontalAlign.Left)
            {
                return inputValue.PadRight(maxWidth, ' ');
            }
            return inputValue.PadLeft(maxWidth, ' ');
        }
        #endregion
    }
}
