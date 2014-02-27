using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;

namespace Kraken.Framework.TestMonkey
{
    #region Delegates
    
    /// <summary>
    /// A delegate for a callback method to be called when the <see cref="ObjectInspector"/> is walking the tree.
    /// </summary>
    public delegate void ReflectionWalkerCallback(
                                Type targetType
                                , object targetObject
                                , object mirrorObject
                                , MemberInfo fieldInfo
                                , string objectName);
    #endregion

    /// <summary>
    /// Base class from which other helpers inherit. This is the engine of the object recursion but its interfaces are more 
    /// complicated than need be, so the subclasses hide the complexity and only expose what the consumer is interested in
    /// </summary> 
    public class ObjectInspector
    {
        #region Fields
        private int _currentTraversalDepth;
        private bool _mirrorInUse;
        private static Logger Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets an instance of <see cref="CodeGenOptions"/> that will be used to govern the <see cref="ObjectInspector"/>'s
        /// behaviour.
        /// </summary>
        public CodeGenOptions Options { get; set; }
        #endregion

        #region Constructors
        internal ObjectInspector()
        {
            Options = new CodeGenOptions();
        }
        #endregion

        #region Instance Methods
        internal void Walk(ReflectionWalkerCallback callback, object targetObject, object mirrorObject, string objectName)
        {
            _currentTraversalDepth = 0;
            _mirrorInUse = mirrorObject != null;
            InnerWalk(callback, targetObject, mirrorObject, objectName);
        }

        private void WalkCollection(
            ReflectionWalkerCallback callback
            ,Type targetType
            ,object targetObject
            ,object mirrorObject
            ,string objectName)
        {
            if (targetObject is string)
            {
                callback(targetType, targetObject, mirrorObject, null, objectName);
                return;
            }

            IEnumerable targetAsEnumerable = targetObject as IEnumerable;
            IList targetAsList = targetObject as IList;

            FieldInfoCollection fields = GetWalkablePropertiesAndFields(targetType);

            MemberInfo countField = fields.Find(f => f.Name == "Count");
            MemberInfo lengthField = fields.Find(f => f.Name == "Length");

            if (countField != null)
            {
                callback(targetType, targetObject, mirrorObject, countField, objectName);
            }

            if (lengthField != null)
            {
                callback(targetType, targetObject, mirrorObject, lengthField, objectName);
            }

            if (targetAsList != null)
            {
                for (int i = 0; i < targetAsList.Count; i++)
                {
                    object innerTarget = targetAsList[i];

                    if (innerTarget == null)
                    {
                        Log.Warn("CodeGenWalker skipping list item {0} since the inner target is null", i);
                        continue;
                    }

                    bool isSimpleType = IsSimpleType(innerTarget.GetType());
                    object innerMirrorTarget = null;

                    if (mirrorObject != null)
                    {
                        innerMirrorTarget = ((IList) mirrorObject)[i];
                    }

                    string objectNameWithIndexer = string.Format("{0}[{1}]", objectName, i);

                    if (IsOfType(innerTarget, Options.UpcastTypes))
                    {
                        objectNameWithIndexer = GetUpCastTargetName(innerTarget, objectNameWithIndexer);
                    }

                    if (isSimpleType)
                    {
                        // THIS WILL ALWAYS BREAK MOFO
                        callback(targetType, innerTarget, innerMirrorTarget, null, objectNameWithIndexer);
                    }
                    else
                    {
                        InnerWalk(callback, innerTarget, innerMirrorTarget, objectNameWithIndexer);
                    }
                }
            }
            else if (targetAsEnumerable != null)
            {
                throw TestMonkeyException.Create("Object Inspector doesn't work with pure IEnumerables: " + objectName);
            }
        }

        private void InnerWalk(ReflectionWalkerCallback callback, object targetObject, object mirrorObject, string objectName)
        {
            _currentTraversalDepth++;

            if (targetObject != null && _currentTraversalDepth <= Options.MaximumTraversalDepth)
            {
                Type targetType = targetObject.GetType();

                IEnumerable targetAsEnumerable = targetObject as IEnumerable;
                bool isEnumerable = targetAsEnumerable != null;

                FieldInfoCollection memberInfoCollection = GetWalkablePropertiesAndFields(targetType);

                if (memberInfoCollection.Count == 0 && !isEnumerable)
                {
                    callback(targetType, targetObject, mirrorObject, null, objectName);
                }
                
                if (isEnumerable)
                {
                    WalkCollection(callback, targetType, targetObject, mirrorObject, objectName);

                    // Check to see whether we should continue in this method and enumerate all properties
                    if (!Options.EnumerateAllCollectionProperties)
                    {
                        return;
                    }
                }

                IterateMemberList(callback, targetType, targetObject, mirrorObject, objectName, memberInfoCollection);
            }

            _currentTraversalDepth--;
        }

        internal void IterateMemberList(ReflectionWalkerCallback callback, Type targetType, object targetObject, object mirrorObject, string objectName, FieldInfoCollection memberInfoCollection)
        {
            foreach (MemberInfo memberInfo in memberInfoCollection)
            {
                if (IsExcludedPropertyName(Options, memberInfo.Name))
                {
                    continue;
                }

                if (IsExcludedType(Options, memberInfo))
                {
                    continue;
                }

                bool targetPropertySwallowedException;
                object targetPropertyValue = InvokeMember(targetType, targetObject, memberInfo, Options.BindingFlags, true, out targetPropertySwallowedException);
                object mirrorPropertyValue = null;

                if (_mirrorInUse)
                {
                    bool mirrorPropertySwallowedExpection;
                    mirrorPropertyValue = InvokeMember(targetType, mirrorObject, memberInfo, Options.BindingFlags, true, out mirrorPropertySwallowedExpection);
                }

                if (targetPropertyValue != null && !IsSimpleType(targetPropertyValue.GetType()))
                {
                    InnerWalk(callback, targetPropertyValue, mirrorPropertyValue, objectName + "." + memberInfo.Name);
                }
                else
                {
                    callback(targetType, targetObject, mirrorObject, memberInfo, objectName);
                }
            }
        }
        #endregion

        #region Static Methods
        
        /// <summary>
        /// Returns a <see cref="String"/> in the format "((<paramref name="targetObject"/>.GetType())<paramref name="targetName"/>)".
        /// </summary>
        protected static string GetUpCastTargetName(object targetObject, string targetName)
        {
            targetName = "((" + targetObject.GetType() + ")" + targetName + ")";
            return targetName;
        }

        /// <summary>
        /// Invokes the member indicates by <paramref name="memberInfo"/> on the supplied <paramref name="targetObject"/>.
        /// </summary>
        protected static object InvokeMember(
            Type targetType
            ,object targetObject
            ,MemberInfo memberInfo 
            ,BindingFlags bindingFlags
            ,bool throwException
            ,out bool exceptionSwallowed)
        {
            object targetPropertyValue = null;

            exceptionSwallowed = false;

            try
            {
                targetPropertyValue = targetType.InvokeMember(memberInfo.Name, bindingFlags, null, targetObject, null);
            }
            catch (Exception e)
            {
                string message = string.Format(
                    "\r\n\r\n*** ObjectInsepector failed to invoke {1}.{0} ***\r\n\r\nCheck your code isn't throwing the exception.\r\n\r\n"
                    , memberInfo == null ? "<NULL>": memberInfo.Name
                    , targetType.Name);

                if (throwException)
                {
                    Exception ex = new Exception(message, e.InnerException);
                    throw ex;
                }

                exceptionSwallowed = true;
            }

            return targetPropertyValue;
        }

        /// <summary>
        /// Gets a <see cref="FieldInfoCollection"/> containing all <see cref="BindingFlags.Public"/>,
        /// <see cref="BindingFlags.Instance"/> properties and fields on the supplied <paramref name="targetType"/>.
        /// </summary>
        protected static FieldInfoCollection GetWalkablePropertiesAndFields(Type targetType)
        {
            return GetWalkablePropertiesAndFields(BindingFlags.Public | BindingFlags.Instance, targetType);
        }

        /// <summary>
        /// Gets a <see cref="FieldInfoCollection"/> containing all properties and fields on the supplied 
        /// <paramref name="targetType"/> that conform to the supplied <paramref name="bindingFlags"/>.
        /// </summary>
        protected static FieldInfoCollection GetWalkablePropertiesAndFields(BindingFlags bindingFlags, Type targetType)
        {
            // Find all properties and generate assertions for them
            FieldInfo[] fieldList = targetType.GetFields(bindingFlags);

            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();
            propertyInfoList.AddRange(targetType.GetProperties(bindingFlags));

            // Indexers were causing problems for the code gen so filter them out
            propertyInfoList.RemoveAll(propertyInfo => propertyInfo.GetIndexParameters().Length > 0);

            FieldInfoCollection memberInfoList = new FieldInfoCollection();
            memberInfoList.AddRange(propertyInfoList.ToArray());
            memberInfoList.AddRange(fieldList);

            return memberInfoList;
        }

        /// <summary>
        /// Simple in that it can be asserted directly against
        /// </summary>
        protected static bool IsSimpleType(Type targetType)
        {
            if (targetType == null || targetType == typeof(string) || targetType == typeof(DateTime) || targetType.IsEnum)
            {
                return true;
            }

            bool isSystemNameSpace = targetType.Namespace.StartsWith("System");
            bool isEnumerable = IsImplementationOf(targetType, typeof(IEnumerable));
            bool isDataTable = targetType.Equals(typeof(DataTable));
            bool isDataRow = targetType.Equals(typeof(DataRow));
            bool isSystemNetMail = targetType.Namespace.StartsWith("System.Net.Mail");
            return isSystemNameSpace && !isEnumerable && !isDataTable && !isDataRow && !isSystemNetMail;
        }

        /// <summary>
        /// Returns a <see cref="Boolean"/> indicating whether or not <paramref name="targetType"/> implements
        /// the <paramref name="interfaceType"/> interface.
        /// </summary>
        protected static bool IsImplementationOf(Type targetType, Type interfaceType)
        {
            return targetType.GetInterface(interfaceType.FullName) != null;
        }

        /// <summary>
        /// Returns a <see cref="Boolean"/> indicating whether or not the <paramref name="target"/> object is 
        /// or is assignable from one of the types in the supplied <paramref name="list"/>.
        /// </summary>
        protected static bool IsOfType(object target, List<Type> list)
        {
            if (target == null || list == null)
            {
                return false;
            }

            bool isOfType = false;
            Type targetType = target.GetType();
            foreach (Type listedType in list)
            {
                // inherits from
                isOfType |= targetType.IsSubclassOf(listedType);

                // implements interface
                isOfType |= listedType.IsAssignableFrom(targetType);

                // is actually
                isOfType |= targetType == listedType;
            }
            return isOfType;
        }

        /// <summary>
        /// Check against the property name to see whether it is on any exclude lists
        /// </summary>
        protected static bool IsExcludedPropertyName(ObjectInspectorOptions options, string targetName)
        {
            string propertyName = targetName;

            // Trap cases where it is like target[0].Legcount (and we are including "legcount"
            int lastPeriodPosition = targetName.LastIndexOf(".");
            if (lastPeriodPosition > 0)
            {
                propertyName = targetName.Substring(lastPeriodPosition + 1);
            }

            // Handle the dataRow case - trim out the row text and get the actual column name 
            if (propertyName.StartsWith("Rows"))
            {
                const string columnNamePattern = @"""(\w+)""";
                Regex regex = new Regex(columnNamePattern);
                Match match = regex.Match(propertyName);

                if (match != null && match.Captures.Count > 0)
                {
                    propertyName = match.Captures[0].Value;
                    propertyName = propertyName.Replace("\"", string.Empty);
                }
            }

            // Do we have any exclusions
            string exclusionTest = options.ExcludeProperties.Find(exclude => string.Compare(targetName, exclude, true) == 0);
            if (!String.IsNullOrEmpty(exclusionTest))
            {
                return true;
            }

            // Do we only honour explicit includes
            if (!options.EnumerateAllProperties && !options.IncludeProperties.Contains(propertyName))
            {
                return true;
            }

            // otherwise ok
            return false;
        }

        /// <summary>
        /// Returns a <see cref="Boolean"/> indicating whether or not the supplied <paramref name="options"/>
        /// contains the <see cref="Type"/> of <paramref name="memberInfo"/> in its 
        /// <see cref="ObjectInspectorOptions.ExcludeTypes"/> list.
        /// </summary>
        protected static bool IsExcludedType(ObjectInspectorOptions options, MemberInfo memberInfo)
        {
            Type propertyType = memberInfo.ReflectedType;
            string targetTypeLiteral = propertyType.ToString();

            return options.ExcludeTypes.Contains(targetTypeLiteral);
        }
        #endregion
    }

}
