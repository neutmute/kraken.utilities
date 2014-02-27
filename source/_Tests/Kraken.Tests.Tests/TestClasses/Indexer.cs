using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.TestClasses
{
    #region IndexerColumns enum
    public enum IndexerColumns
    {
        Column1,
        Column2,
        Column3
    }
    #endregion

    /// <summary>
    /// Simulates a billfile from Polecat that CodeGen choked on
    /// </summary>
    public class Indexer : IEnumerable<int>
    {
        #region Fields
        private List<int> _InternalList;
        #endregion

        #region Properties
        public int this[IndexerColumns key]
        {
            get { return (int) key; }
        }
        #endregion

        #region Constructors
        public Indexer()
        {
            _InternalList = new List<int>();
            _InternalList.Add(4);
            _InternalList.Add(5);
            _InternalList.Add(6);
        }
        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
