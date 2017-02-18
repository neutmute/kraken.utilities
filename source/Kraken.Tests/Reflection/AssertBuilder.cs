using System;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace Kraken.Tests
{
    /// <summary>
    /// CodeGenerator is used to automatically generate assertions for an object so you don't 
    /// have to do boring monkey work
    /// </summary>
    /// <remarks>
    /// Usage: 
    /// 1)	Code up the unit test until you get to a point where you are ready to write the Assert.AreEqual bits.
    /// 2)	Insert the line CodeGen.GenerateAssertions(blahObject, "blahObject");
    /// 3)	Execute the test then grab the generated asserts from the console output and paste in and replace the spot where
    ///		the CodeGenerateAssertions method was
    ///	4)  Take a break because you just saved 20 minutes hand coding.
    /// </remarks>
    public class AssertBuilder : ObjectInspector
    {
        #region Fields
        private const string c_SuccessMessage = "Assertions were generated. See Console output tab";
        private const string c_CallingCodeFormat = "// AssertBuilder.Generate({0}, \"{0}\"{2}); // The following assertions were generated on {1}";
        private const string c_AssertArrayLengthFormat = "Assert.AreEqual({0}, {1});";
        StringBuilder m_EmittedCode;
        
        private bool _EmitGeneratedCodeToConsole;
        #endregion

        #region Properties
        /// <summary>
        /// Use ObjectXray to poke this value and change the class to work in a unit test compatible mode for the TestTheTests project
        /// (rather than as a library that generates unit test code)
        /// </summary>
        /// <remarks>Don't believe resharper that this is never used. Used in UnitTests</remarks>
        private bool TestMode
        {
            set
            {
                // reset the emitted code in case we use the same code gen object in a unit test twice
                m_EmittedCode = new StringBuilder();

                Options.AssertFailAfterGeneration = !value;
                Options.EmitRegionWrappers = !value;
                _EmitGeneratedCodeToConsole = !value;
            }
        }

        public bool LogEmittedCode { get; set; }

        public bool AppendEmittedCodeToFailMessage { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for 99.9% cases
        /// </summary>
        public AssertBuilder()
        {
            Initialise();
            _EmitGeneratedCodeToConsole = true;
            Options = new CodeGenOptions();
        }
        
        private void Initialise()
        {
            m_EmittedCode = new StringBuilder();
        }
        #endregion

        #region EmitCode Overloads
        /// <summary>
        /// Emit how the code was executed
        /// </summary>
        /// <remarks>
        /// Write it as a comment so that the code can be regenerated easily
        /// </remarks>
        private void EmitCallingCode(string targetName)
        {
            if (Options == null)
                throw new ArgumentNullException("Options");

            if (Options.EmitRegionWrappers && targetName != null)
            {
                // Add in the ignore parameters into the call signature
                string ignoreParams = String.Empty;
                foreach (string s in Options.ExcludeProperties)
                {
                    ignoreParams += ", \"" + s + "\"";
                }
                EmitCode(c_CallingCodeFormat, targetName, DateTime.Now.ToString("dd-MMM-yyyy"), ignoreParams);
            }
        }

        /// <summary>
        /// Emit the given code to the output stream
        /// </summary>
        /// <remarks>
        /// Allows for a string.format operation
        /// </remarks>
        private void EmitCode(string format, params object[] args)
        {
            EmitCode(String.Format(format, args));
        }

        /// <summary>
        /// Emit the given code to the console
        /// </summary>
        private void EmitCode(string code)
        {
            if (_EmitGeneratedCodeToConsole)
            {
                Console.WriteLine(code);
            }
            m_EmittedCode.Append(code + "\r\n");
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Given an arbitary object, will try and generate assertions for you automatically.
        /// </summary>
        /// <param name="targetObject">The actual object to generate assertions for</param>
        /// <param name="targetName">The name of the variable in your code</param>
        public void Generate(object targetObject, string targetName)
        {
            Generate(targetObject, targetName, Options);
        }

        /// <summary>
        /// Generate assertions for static properties on a class
        /// </summary>
        public void Generate(Type targetType)
        {
            Setup(null);

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            FieldInfoCollection staticMemberCollection = GetWalkablePropertiesAndFields(flags, targetType);
            IterateMemberList(WalkCallback, targetType, null, null, targetType.Name, staticMemberCollection);

            TearDown();
        }

        /// <summary>
        /// Given an arbitary object, will try and generate assertions for you automatically.
        /// </summary>
        /// <param name="targetObject">The actual object to generate assertions for</param>
        /// <param name="targetName">The name of the variable in your code</param>
        /// <param name="options">Specify extra options to control the output</param>
        /// <remarks>
        /// ignoreProperties is case sensitive. More by quick implementation than requirement.
        /// </remarks>
        public void Generate(object targetObject, string targetName, CodeGenOptions options)
        {
            Options = options;
            Setup(targetName);

            if (targetObject == null)
            {
                EmitCode("Assert." + Options.AssertSignatures.IsNull + "(" + targetName + ");");
            }
            else
            {
                GenerateAssertionsForUnknownType(targetObject, targetName, null);
            }

            TearDown();
        }

        /// <summary>
        /// Given an arbitary object, will try and generate assertions for you automatically.
        /// </summary>
        /// <param name="targetObject">The actual object to generate assertions for</param>
        /// <param name="targetName">The name of the variable in your code</param>
        /// <param name="ignoreProperties">A list of property names to be ignored.</param>
        /// <remarks>
        /// ignoreProperties is case sensitive. More by quick implementation than requirement.
        /// </remarks>
        public void Generate(object targetObject, string targetName, params string[] ignoreProperties)
        {
            Options.ExcludeProperties.AddRange(ignoreProperties);
            Generate(targetObject, targetName);
        }

        private void TearDown()
        {
            if (Options.EmitRegionWrappers)
            {
                EmitCode("#endregion");
            }

            if (Options.AssertFailAfterGeneration)
            {
                var message = c_SuccessMessage;

                if (AppendEmittedCodeToFailMessage)
                {
                    message += "\r\n" + m_EmittedCode;
                }

                TestFrameworkFacade.AssertFail(message);

                if (LogEmittedCode)
                {
                    Console.WriteLine(m_EmittedCode);
                }
            }
        }

        private void Setup(string targetName)
        {
            EmitCallingCode(targetName);

            if (Options.EmitRegionWrappers)
            {
                EmitCode("#region Generated Assertions");
            }
        }

        /// <summary>
        /// Generates assertions for a DataTable object
        /// </summary>
        /// <param name="targetObject">The actual object to generate assertions for</param>
        /// <param name="targetName">The name of the variable in your code</param>
        /// <param name="enumerateElements">Whether we want all sub properties or not</param>
        private void GenerateAssertionsForDataTable(
            DataTable targetObject,
            string targetName,
            bool enumerateElements)
        {
            EmitCode(c_AssertArrayLengthFormat, targetObject.Rows.Count, targetName + ".Rows.Count");

            if (enumerateElements)
            {
                for (int i = 0; i < targetObject.Rows.Count; i++)
                {
                    GenerateAssertionsForDataRow(targetObject.Rows[i], String.Format("{0}.Rows[{1}]", targetName, i));
                }
            }
        }

        /// <summary>
        /// Special generator for DataRows so that their properties are ignored and only hit the indexors
        /// </summary>
        private void GenerateAssertionsForDataRow(
            DataRow targetObject
            , string targetName)
        {
            for (int j = 0; j < targetObject.ItemArray.Length; j++)
            {
                string targetNameFormat = "{0}[\"{1}\"]";
                string columnName = targetObject.Table.Columns[j].ColumnName;

                if (!IsExcludedPropertyName(Options, columnName))
                {
                    GenerateAssertionsForSimpleType(
                        targetObject.ItemArray[j]
                        , String.Format(targetNameFormat, targetName, columnName));
                }
            }
        }


        /// <summary>
        /// Generate a set of unit tests for a collection
        /// </summary>
        private void GenerateAssertionsForIEnumerable(
            IEnumerable targetObject,
            string targetName)
        {
            if (targetObject is string)
            {
                GenerateAssertionsForSimpleType(targetObject, targetName);
                return;
            }

            EmitCode("System.Collections.IEnumerator enumerator = {0}.GetEnumerator();", targetName);
            EmitCode("object enumeratorPointer = enumerator.Current;", targetName);

            foreach (object o in targetObject)
            {
                GenerateAssertionsForUnknownType(o, "enumeratorPointer", null);
                EmitCode("enumeratorPointer = enumerator.MoveNext();", targetName);
            }
        }

 

        /// <summary>
        /// Generate an assertion for a simple type - ie: A value type that just is and has no 
        /// properties to enumerate
        /// </summary>
        private void GenerateAssertionsForSimpleType(object targetObject, string targetName)
        {
            string qualifierPrefix = String.Empty;
            string qualifierSuffix = String.Empty;
            string expectedValue = "null";
            bool multilineValue = false;
            string comment = null;

            if (targetObject != null)
            {
                expectedValue = targetObject.ToString();
                string typeName = targetObject.GetType().ToString();
                multilineValue = expectedValue.IndexOf("\r\n") >= 0;

                switch (typeName)
                {
                    case "System.Guid":
                        expectedValue = ((Guid)targetObject).ToString();
                        qualifierPrefix = "new Guid(\"";
                        qualifierSuffix = "\")";
                        break;

                    case "System.DateTime":
                        DateTime value = Convert.ToDateTime(targetObject);
                        if (value == DateTime.MinValue)
                        {
                            expectedValue = "DateTime.MinValue";
                            qualifierPrefix = "";
                            qualifierSuffix = "";
                        }
                        else if (value == DateTime.MaxValue)
                        {
                            expectedValue = "DateTime.MaxValue";
                            qualifierPrefix = "";
                            qualifierSuffix = "";
                        }
                        else if (value.TimeOfDay.TotalMilliseconds == 0)
                        {
                            qualifierPrefix = "Convert.ToDateTime(\"";
                            qualifierSuffix = "\")";
                            expectedValue = value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            qualifierPrefix = "Convert.ToDateTime(\"";
                            qualifierSuffix = "\")";
                            expectedValue = value.ToString("dd-MMM-yyyy HH:mm:ss.fff");
                        }
                        break;

                    case "System.Char":
                        qualifierPrefix = "'";
                        qualifierSuffix = "'";
                        break;

                    case "System.String":
                        if (multilineValue)
                        {
                            qualifierPrefix = "@\"";
                        }
                        else
                        {
                            qualifierPrefix = "\"";
                        }

                        qualifierSuffix = "\"";
                        break;

                    case "System.Single":
                        qualifierPrefix = "";
                        qualifierSuffix = "f";
                        break;

                    case "System.Double":

                        Double doubleValue = Convert.ToDouble(expectedValue);
                        if (doubleValue == Double.MinValue)
                        {
                            expectedValue = "Double.MinValue";
                            qualifierPrefix = "";
                            qualifierSuffix = "";
                        }
                        else
                        {
                            qualifierPrefix = "\"";
                            qualifierSuffix = "\"";
                            expectedValue = doubleValue.ToString();
                            targetName += ".ToString()";
                        }
                        break;

                    case "System.Decimal":
                        qualifierPrefix = "";
                        qualifierSuffix = "m";

                        Decimal decimalValue = Convert.ToDecimal(expectedValue);
                        if (decimalValue == Decimal.MinValue)
                        {
                            expectedValue = "Decimal.MinValue";
                            qualifierPrefix = "";
                            qualifierSuffix = "";
                        }
                        else if (decimalValue == Decimal.MaxValue)
                        {
                            expectedValue = "Decimal.MaxValue";
                            qualifierPrefix = "";
                            qualifierSuffix = "";
                        }
                        break;

                    case "System.Int64":
                        qualifierPrefix = "";
                        qualifierSuffix = "";

                        if (Convert.ToInt64(expectedValue) == Int64.MinValue)
                        {
                            expectedValue = "Int64.MinValue";
                        }
                        break;

                    case "System.Int32":
                        qualifierPrefix = "";
                        qualifierSuffix = "";

                        if (Convert.ToInt32(expectedValue) == Int32.MinValue)
                        {
                            expectedValue = "Int32.MinValue";
                        }
                        break;

                    case "System.Int16":
                        qualifierPrefix = "";
                        qualifierSuffix = "";

                        if (Convert.ToInt16(expectedValue) == Int16.MinValue)
                        {
                            expectedValue = "Int16.MinValue";
                        }
                        break;

                    case "System.Byte":
                        qualifierPrefix = "";
                        qualifierSuffix = "";
                        break;

                    case "System.Boolean":
                        qualifierPrefix = "";
                        qualifierSuffix = "";
                        expectedValue = expectedValue.ToLower();
                        break;

                    case "System.DBNull":
                        expectedValue = "DBNull.Value";
                        break;

                    default:
                        if (targetObject.GetType().IsEnum)
                        {
                            // Handles enumerations - system as well as custom specified
                            qualifierPrefix = typeName + ".";
                        }
                        else
                        {
                            // otherwise just evaluate the ToString()
                            qualifierPrefix = "\"";
                            qualifierSuffix = "\"";

                            // Make sure we mention it is a two string otherwise it turns in to a cast + compare which fails
                            targetName += ".ToString()";
                        }
                        break;
                }
            }

            // delimit any back slashes for the c# code
            expectedValue = expectedValue.Replace(@"\", @"\\");

            // delimit any double quotes
            if (multilineValue)
            {
                expectedValue = expectedValue.Replace("\"", @"""""");
            }
            else
            {
                expectedValue = expectedValue.Replace("\"", "\\\"");
            }

            if (!string.IsNullOrEmpty(comment))
            {
                comment = " //" + comment;
            }

            EmitCode(
                "Assert.{5}({0}{1}{2}, {3});{4}"
                , qualifierPrefix
                , expectedValue
                , qualifierSuffix
                , targetName
                , comment
                , Options.AssertSignatures.AreEqual);
        }

        private void WalkCallback(Type targetType, object targetObject, object mirrorObject, MemberInfo fieldInfo, string objectName)
        {
            bool exceptionSwallowed;

            if (fieldInfo == null)
            {
                GenerateAssertionsForUnknownType(targetObject, objectName, targetType);
            }
            else
            {
                object targetValue = InvokeMember(targetType, targetObject, fieldInfo, Options.BindingFlags, true, out exceptionSwallowed);
                string fieldName = objectName + "." + fieldInfo.Name;
                GenerateAssertionsForUnknownType(targetValue, fieldName, targetType);
            }

        }

        /// <summary>
        /// Code generate testing the properties of the target
        /// </summary>
        /// <remarks>
        /// This overload hides the option to suppress assert failures.
        /// Supression is necessary when the method recurses in for a complex property 
        /// </remarks>
        private void GenerateAssertionsForUnknownType(object targetObject, string targetName, Type parentType)
        {
            Type targetType = null;
            
            if (targetObject != null)
            {
                targetType = targetObject.GetType();
            }

            IEnumerable targetAsEnumerable = targetObject as IEnumerable;
            IList targetAsIList = targetObject as IList;
            DataTable targetAsDataTable = targetObject as DataTable;
            DataRow targetAsDataRow = targetObject as DataRow;
            bool isSimpleType = targetType == null || IsSimpleType(targetType);

            if (isSimpleType)
            {
                GenerateAssertionsForSimpleType(targetObject, targetName);
                return;
            }

            if (targetAsDataTable != null)
            {
                GenerateAssertionsForDataTable(targetAsDataTable, targetName, true);
                return;
            }

            if (targetAsDataRow != null)
            {
                GenerateAssertionsForDataRow(targetAsDataRow, targetName);
                return;
            }
             
            // Pure enumerable with no indexer
            if (targetAsEnumerable != null && targetAsIList == null)
            {
                GenerateAssertionsForIEnumerable(targetAsEnumerable, targetName);
                return;
            }

            FieldInfoCollection memberInfoList = GetWalkablePropertiesAndFields(targetType);

            // If this object didn't have any properties, it must be a simple type or just needs a ToString() test
            if (memberInfoList.Count == 0 && targetAsEnumerable == null)
            {
                GenerateAssertionsForSimpleType(targetObject, targetName);
            }

            if (memberInfoList.Count > 0)
            {
                Walk(WalkCallback, targetObject, null, targetName);
            }
        }

       

        /// <summary>
        /// Return the code that was last generated.
        /// Useful for testing this class
        /// </summary>
        public string GetEmittedCode()
        {
            string emittedCode = string.Empty;
            if (m_EmittedCode != null)
            {
                emittedCode = m_EmittedCode.ToString().Trim();
            }
            return emittedCode;
        }
        #endregion
    }
}
