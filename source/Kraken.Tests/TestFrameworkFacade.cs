using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Tests
{
    public delegate void AssertEqualSignature(object object1, object object2, string message);

    public delegate void AssertFailSignature(string format, params object[] args);

    public static class TestFrameworkFacade
    {
        private static AssertFailSignature _assertFailCallback;
        private static AssertEqualSignature _assertCallback;

        /// <summary>
        /// Decouple from Test implementation
        /// </summary>
        public static AssertEqualSignature AssertEqual
        {
            get
            {
                if (_assertCallback == null)
                {
                    throw TestMonkeyException.Create("AssertCallBack property must be set");
                }
                return _assertCallback;
            }
            set { _assertCallback = value; }
        }


        /// <summary>
        /// Decouple from Test implementation
        /// </summary>
        public static AssertFailSignature AssertFail
        {
            get
            {
                if (_assertFailCallback == null)
                {
                    throw TestMonkeyException.Create("AssertFail property must be set");
                }
                return _assertFailCallback;
            }
            set { _assertFailCallback = value; }
        }
    }
}
