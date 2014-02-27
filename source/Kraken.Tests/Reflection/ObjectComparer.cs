using System;
using System.Reflection;

namespace Kraken.Framework.TestMonkey
{

    /// <summary>
    /// Compare two objects and recurse down through their object tree checking everything is the same
    /// </summary>
    public class ObjectComparer : ObjectInspector
    {

        #region Instance Methods
        
        /// <summary>
        /// Asserts that the <paramref name="target"/> and <paramref name="mirrorObject"/> are equal based on the values of their public fields and properties.
        /// </summary>
        public void AssertEqual(object target, object mirrorObject)
        {
            Walk(AssertAreEqual, target, mirrorObject, string.Empty);
        }

        /// <summary>
        /// Asserts that the <paramref name="target"/> and <paramref name="mirrorObject"/> are not equal based on the values of their public fields and properties.
        /// </summary>
        public void AssertNotEqual(object target, object mirrorObject)
        {
            Walk(AssertNotEqual, target, mirrorObject, string.Empty);
        }

        private void InvokeAndAssert(Type targetType, object targetObject, object mirrorObject, MemberInfo fieldInfo, string objectName)
        {
            bool exceptionSwallowedTarget;
            bool exceptionSwallowedMirror;

            object targetProperty = targetObject;
            object mirrorProperty = mirrorObject;
            string fieldName = fieldInfo == null ? string.Empty : fieldInfo.Name;

            if (fieldInfo != null)
            {
                targetProperty = InvokeMember(targetType, targetObject, fieldInfo, Options.BindingFlags, true, out exceptionSwallowedTarget);
                mirrorProperty = InvokeMember(targetType, mirrorObject, fieldInfo, Options.BindingFlags, true, out exceptionSwallowedMirror);
            }

            if (Options.LogToConsole)
            {
                string consoleMessage = string.Format(
                    "ObjectComparer.Assert: {0}.{1}, values={2}|{3}"
                    , objectName
                    , fieldName
                    , targetProperty ?? "<null>"
                    , mirrorProperty ?? "<null>");

                Console.WriteLine(consoleMessage);
            }

            string message = string.Format(
                "ObjectComparer assertion failed: value1={3}, value2={4} on object={0}, type={1}, field={2}"
                , objectName
                , targetObject.GetType()
                , fieldName
                , targetProperty
                , mirrorProperty);


            TestFrameworkFacade.AssertEqual(targetProperty, mirrorProperty, message);
        }

        private void AssertNotEqual(Type targetType, object targetObject, object mirrorObject, MemberInfo fieldInfo, string objectName)
        {
            InvokeAndAssert(targetType, targetObject, mirrorObject, fieldInfo, objectName);
        }

        private void AssertAreEqual(Type targetType, object targetObject, object mirrorObject, MemberInfo fieldInfo, string objectName)
        {
            InvokeAndAssert(targetType, targetObject, mirrorObject, fieldInfo, objectName);
        }
        #endregion
    }
}
