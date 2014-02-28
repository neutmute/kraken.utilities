using System;
using System.Reflection;

namespace Kraken.Tests.Tests
{
	/// <summary>
	/// Take a peek inside private/protected/internal classes for testing...
	/// </summary>
	public class ObjectXRay
    {
        #region constants

        /// <summary>
        /// The format of the exception message to throw when a method could not be located
        /// in a particular type.
        /// </summary>
        /// <remarks>
        /// parameter 0 = the method name passed in.
        /// parameter 1 = the type that was searched.
        /// </remarks>
        private const string MethodNotFoundMessageFormat = "There is no method '{0}' for type '{1}'.";

        #endregion

        #region Fields (private)
        private object _instance = null;
		private Type _t;
		#endregion

		#region Constructor : private (empty)
		private ObjectXRay (){}
		#endregion

		#region Factory methods NewType(type) +1 overload
		
        /// <summary>
        /// Returns an <see cref="ObjectXRay"/> instance for the specified <paramref name="type"/>.
        /// </summary>
        public static ObjectXRay NewType (System.Type type)
		{
			ObjectXRay newInstance = new ObjectXRay();
			newInstance._t = type;
			return newInstance;
		}

        /// <summary>
        /// Returns an <see cref="ObjectXRay"/> instance for the specified <paramref name="type"/> and <paramref name="instance"/>.
        /// </summary>
        public static ObjectXRay NewType(System.Type type, object instance)
		{
			ObjectXRay newInstance = NewType (type);
			newInstance._instance = instance;
			return newInstance;
		}

        /// <summary>
        /// Returns an <see cref="ObjectXRay"/> instance for the specified <paramref name="typename"/> (in <paramref name="assembly"/>)
        /// and <paramref name="instance"/>.
        /// </summary>
        public static ObjectXRay NewType(string assembly, string typename, object instance)
		{
			ObjectXRay newInstance = NewType (assembly, typename);
			newInstance._instance = instance;
			return newInstance;
		}

        /// <summary>
        /// Returns an <see cref="ObjectXRay"/> instance for the specified <paramref name="typename"/> (in <paramref name="assembly"/>).
        /// </summary>
        public static ObjectXRay NewType(string assembly, string typename)
		{
			Assembly a = Assembly.Load(assembly);
			if (a == null)
			{
				throw new ArgumentException("Assembly '" + assembly + "' could not be loaded.");
			}
			ObjectXRay newInstance = new ObjectXRay();
			Type type = a.GetType(assembly +"."+ typename);
			if (type == null)
			{
				throw new ArgumentException("Type '" + typename + "' could not be loaded from assembly '"+assembly+"'.");
			}
			newInstance._t = type;
			return newInstance;
		}
		#endregion

		#region Instance Methods
		/// <summary>
		/// Set an instance of a reflected type before accessing it's properties/methods
		/// </summary>
		/// <param name="instance">an instance of the object 'type' that this XRay represents</param>
		public void SetInstance (object instance)
		{
			_instance = instance;
		}

		/// <summary>
		/// Get the current 'internal' instance, or an empty instance if it hasn't been set yet
		/// </summary>
		/// <returns>instance of the initialized type</returns>
		public object GetInstance()
		{
			return GetInstance(null);
		}
	
		/// <summary>
		/// Get the current 'internal' instance, or an empty instance if it hasn't been set yet
		/// </summary>
		/// <returns>instance of the initialized type</returns>
		public object GetInstance(object[] constructorArgs)
		{
			if (null == _instance)
			{
				BindingFlags eFlags =  BindingFlags.DeclaredOnly | 
					BindingFlags.NonPublic | BindingFlags.Instance | 
					BindingFlags.CreateInstance;
				_instance = _t.InvokeMember(null, eFlags, null, null, constructorArgs);
			} 
			return _instance;
		}

		/// <summary>
        /// Invokes the spcified static <paramref name="methodName"/> on the current type with the specified
        /// <paramref name="args"/> and returns the result.
		/// </summary>
        public object RunStaticMethod(string methodName, params object [] args) 
		{
			BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return RunMethod(methodName, args, eFlags);
		}

        /// <summary>
        /// Executes a protected or private static method on a class.
        /// </summary>
        /// <param name="methodName">
        /// The <see cref="String"/> name of the method to execute.
        /// </param>
        /// <param name="parameters">
        /// The parameters to pass to the method.
        /// </param>
        /// <returns>The return value of the method, if any.</returns>
        /// <exception cref="ArgumentException">
        /// The method <paramref name="methodName"/> could not be found in the type.
        /// </exception>
        public object RunStaticMethodWithNoCheck(string methodName, object[] parameters)
        {
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return this.RunMethodWithNoCheck(methodName, null, parameters, bindingFlags, null);
        }
        /// <summary>
        /// Executes a protected or private static method on a class.
        /// </summary>
        /// <param name="methodName">
        /// The <see cref="String"/> name of the method to execute.
        /// </param>
        /// <param name="parameters">
        /// The parameters to pass to the method.
        /// </param>
        /// <param name="parameterTypes">
        /// The types for the parameters.
        /// </param>
        /// <returns>The return value of the method, if any.</returns>
        /// <exception cref="ArgumentException">
        /// The method <paramref name="methodName"/> could not be found in the type.
        /// </exception>
        public object RunStaticMethodWithNoCheck(string methodName, object[] parameters, Type[] parameterTypes)
        {
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return this.RunMethodWithNoCheck(methodName, null, parameters, bindingFlags, parameterTypes);
        }

        /// <summary>
        /// Executes a protected or private instance method on a class.
        /// </summary>
        /// <param name="methodName">
        /// The <see cref="String"/> name of the method to execute.
        /// </param>
        /// <param name="instance">
        /// The <see cref="Object"/> instance upon which the execution should occur.</param>
        /// <param name="parameters">
        /// The parameters to pass to the method.
        /// </param>
        /// <returns>The return value of the method, if any.</returns>
        /// <exception cref="ArgumentException">
        /// The method <paramref name="methodName"/> could not be found in the type.
        /// </exception>
        public object RunInstanceMethodWithNoCheck(string methodName, object instance, object[] parameters)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return this.RunMethodWithNoCheck(methodName, instance, parameters, bindingFlags, null);
        }

        /// <summary>
        /// Executes a protected or private instance method on a class.
        /// </summary>
        /// <param name="methodName">
        /// The <see cref="String"/> name of the method to execute.
        /// </param>
        /// <param name="instance">
        /// The <see cref="Object"/> instance upon which the execution should occur.</param>
        /// <param name="parameters">
        /// The parameters to pass to the method.
        /// </param>
        /// <param name="parameterTypes">
        /// The types for the parameters.
        /// </param>
        /// <returns>The return value of the method, if any.</returns>
        /// <exception cref="ArgumentException">
        /// The method <paramref name="methodName"/> could not be found in the type.
        /// </exception>
        public object RunInstanceMethodWithNoCheck(string methodName, object instance, object[] parameters, Type [] parameterTypes)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return this.RunMethodWithNoCheck(methodName, instance, parameters, bindingFlags, parameterTypes);
        }



		/// <summary>
		/// Use reflection to execute a private instance method
		/// </summary>
		/// <remarks>
		/// EXAMPLE:
		/// <code>
		/// // create the object with 'private' method
		/// EmailProcessor emailProcObject = new EmailProcessor();
		/// // call the 'private' method with object array of parameters
		/// object o = RunInstanceMethod (
		///		  typeof(Mercury.RulesEngine.EmailProcessor)
		///		, "ProcessEmail"
		///		, emailProcObject
		///		, new object[] {emailIn});
		///	// cast the result to the expected 'private' method return type
		/// EmailState result = (EmailState)o;
		/// ;</code>
		/// </remarks>
		public object RunInstanceMethod(string method, params object [] parameters) 
		{
			BindingFlags eFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			return RunMethod (method, parameters, eFlags);
		}

		/// <summary>
		/// Used by above to public Run*Method() methods
		/// </summary>
		private object RunMethod(
			string method
			,object [] parameters
			,BindingFlags eFlags) 
		{
			MethodInfo m;
			try 
			{
				Type[] types = new Type[parameters.Length];
				for (int i = 0; i < parameters.Length; i++)
				{
					types[i] = parameters[i].GetType();
				}

				m = _t.GetMethod(method, eFlags, null, types, null);
				if (m == null)
				{
					throw new ArgumentException("There is no method '" + 
						method + "' for type '" + _t.ToString() + "'.");
				}
                            
				object objRet = m.Invoke(_instance, parameters);
				return objRet;
			}
			catch (System.Reflection.TargetInvocationException tie)
			{	// from codeproject link above (comments)
				if (tie.InnerException != null)
				{	// throw the inner-exception (peel off the TargetInvocation bit)
					throw tie.InnerException;
				}
				else 
				{	// unless it's empty
					throw tie;
				}
			} 
			catch 
			{	// rethrow all others
				throw;
			}
		}

        /// <summary>
        /// Executes a protected or private method on a class.
        /// </summary>
        /// <param name="methodName">
        /// The <see cref="String"/> name of the method to execute.
        /// </param>
        /// <param name="instance">
        /// The <see cref="Object"/> instance upon which the execution should occur.
        /// </param>
        /// <param name="parameters">
        /// The parameters to pass to the method.
        /// </param>
        /// <param name="parameterTypes">
        /// The types for the parameters.
        /// </param>
        /// <param name="bindingFlags">
        /// The <see cref="BindingFlags"/> to use to locate the method.
        /// </param>
        /// <returns>The return value of the method, if any.</returns>
        /// <exception cref="ArgumentException">
        /// The method <paramref name="methodName"/> could not be found in the type.
        /// </exception>
        /// <remarks>
        /// This method does not do a type check for the parameters, allowing to pass null
        /// parameters.
        /// </remarks>
        private object RunMethodWithNoCheck(string methodName, object instance, object[] parameters, BindingFlags bindingFlags, Type [] parameterTypes)
        {
            //this._t.GetMethod()
            
            MethodInfo method = (parameterTypes == null) ? this._t.GetMethod(methodName, bindingFlags) : this._t.GetMethod(methodName,bindingFlags, null, parameterTypes, null);

            if (method == null)
                throw new ArgumentException(String.Format(MethodNotFoundMessageFormat, methodName, this._t.Name));
            try
            {
                return method.Invoke(instance, parameters);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                throw;
            }
        }




		#endregion
                
		#region GetNestedTypeProperty (nested Type Property/Field inspector)
		/// <summary>
		/// Access classes that are declared inside another class
        /// 
        /// eg: OKCommand inside a DataModel
		/// </summary>
		/// <param name="nestedtypename">internal type name</param>
		/// <param name="propertyOrField">property or field to query</param>
		/// <returns>object of the property/fields type</returns>
		public object GetNestedTypeProperty (string nestedtypename, string propertyOrField)
		{
			BindingFlags eFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static ;
			Type nestedType = _t.GetNestedType(nestedtypename, eFlags);
			eFlags = BindingFlags.GetProperty | BindingFlags.GetField ;
			return nestedType.InvokeMember(propertyOrField, eFlags, null, _instance, new object[] {});
		}

        /// <summary>
        /// Returns the value the <paramref name="propertyOrField"/> from the current instance 
        /// converted from <paramref name="nestedtypename"/> to a <see cref="DateTime"/>.
        /// </summary>
        public DateTime GetNestedTypePropertyAsDateTime(string nestedtypename, string propertyOrField)
		{
			return Convert.ToDateTime(GetNestedTypeProperty (nestedtypename, propertyOrField));
		}
        
        /// <summary>
        /// Returns the value the <paramref name="propertyOrField"/> from the current instance 
        /// converted from <paramref name="nestedtypename"/> to a <see cref="String"/>.
        /// </summary>
        public String GetNestedTypePropertyAsString(string nestedtypename, string propertyOrField)
		{
			return Convert.ToString(GetNestedTypeProperty (nestedtypename, propertyOrField));
		}
		
        /// <summary>
        /// Returns the value the <paramref name="propertyOrField"/> from the current instance 
        /// converted from <paramref name="nestedtypename"/> to an <see cref="Int32"/>.
        /// </summary>
        public int GetNestedTypePropertyAsInt32 (string nestedtypename, string propertyOrField)
		{
			return Convert.ToInt32(GetNestedTypeProperty (nestedtypename, propertyOrField));
		}
		#endregion
        
		#region SetFields
		/// <summary>
		/// Poke a private field of the object
		/// </summary>
		public void SetField(string fieldName, object value)
		{
			FieldInfo field = _instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			field.SetValue(_instance, value);
		}

        /// <summary>
        /// Poke a private property of the object
        /// </summary>
        public void SetProperty(string propertyName, object value)
        {
            PropertyInfo property = _t.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(_instance, value, null);
        }

		/// <summary>
		/// Set a static field
		/// </summary>
		public void SetFieldStatic(string fieldName, object value)
		{
			FieldInfo field = _t.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
			field.SetValue(_t.GetType(), value);
		}

		/// <summary>
		/// Get the value of a field
		/// </summary>
		public object GetField(string fieldName)
		{
			FieldInfo field = _t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			return field.GetValue(_instance);
		}

		/// <summary>
		/// Get the value of a hidden property
		/// </summary>
		public object GetProperty(string propertyName)
		{
			PropertyInfo property = _t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			return property.GetValue(_instance, new object[0]);
		}
		#endregion
	}
}
