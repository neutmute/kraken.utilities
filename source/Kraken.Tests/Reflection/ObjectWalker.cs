using System;
using System.Reflection;

namespace Kraken.Tests
{
    /// <summary>
    /// Walks an object tree and can callback whenever it encounters a named property
    /// </summary>
    public class ObjectWalker<T> : ObjectInspector
    {
        #region Fields
        private Action<T> _assertWalkerCallback;
        
        private string _propertyName;
        #endregion

        #region Instance Methods
        
        /// <summary>
        /// Walks the property tree of <paramref name="targetObject"/>.
        /// </summary>
        public void GetValue(object targetObject, string propertyName, Action<T> callback)
        {
            _assertWalkerCallback = callback;
            _propertyName = propertyName;
            Walk(WalkCallback, targetObject, null, string.Empty);
        }

        private void WalkCallback(Type targetType, object targetObject, object mirrorObject, MemberInfo fieldInfo, string objectName)
        {
            if (fieldInfo == null)
            {
                return;
            }

            if (fieldInfo.Name == _propertyName)
            {
                bool exceptionSwallowedTarget;
                object targetPropertyValue = InvokeMember(
                                                            targetType
                                                            ,targetObject
                                                            ,fieldInfo
                                                            ,Options.BindingFlags
                                                            ,false
                                                            ,out exceptionSwallowedTarget);

                T castValue = (T) targetPropertyValue;

                try
                {
                    if (Options.LogToConsole)
                    {
                        string message = string.Format("ObjectWalker callback: {0}.{1}", objectName, fieldInfo.Name);
                        Console.WriteLine(message);
                    }
                    _assertWalkerCallback(castValue);
                }
                catch (Exception e)
                {
                    throw TestMonkeyException.Create("AssertWalker callback failed on " + objectName + "." + fieldInfo.Name, e);
                }
            }
        }
        #endregion
    }
}
