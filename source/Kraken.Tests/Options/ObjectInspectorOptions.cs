using System.Collections.Generic;
using System.Reflection;

namespace Kraken.Framework.TestMonkey
{
    /// <summary>
    /// A container for options that govern the behaviour of an <see cref="ObjectInspector"/>.
    /// </summary>
    public class ObjectInspectorOptions
    {
        #region Fields
        /// <summary>
        /// Use this in conjunction with EnumerateAllProperties = false
        /// </summary>
        private List<string> _IncludeProperties;
        private List<string> _ExcludeProperties;
        private List<string> _ExcludeTypes;
        #endregion

        #region Properties
        /// <summary>
        /// Log what asserts were made to the console. Useful for double checking the tool is 
        /// doing what you thought it should be doing.
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not all properties should be enumerated. 
        /// The default is <code>true</code>.
        /// </summary>
        public bool EnumerateAllProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not all collection properties should 
        /// be enumerated. The default is <code>true</code>.
        /// </summary>
        public bool EnumerateAllCollectionProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the emitted code will be wrapped in a 
        /// <code>#region</code>. The default is <code>true</code>.
        /// </summary>
        public bool EmitRegionWrappers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ObjectInspector"/> should 
        /// call Assert.Fail after completion. The default is <code>true</code>.
        /// </summary>
        public bool AssertFailAfterGeneration { get; set; }

        /// <summary>
        /// Use this to limit the depth of the code generation.
        /// Useful with a deep object tree and you only want to skim the top
        /// </summary>
        public int MaximumTraversalDepth { get; set; }

        /// <summary>
        /// Gets a <see cref="List{String}"/> of property names that should be excluded from 
        /// the output when <see cref="EnumerateAllProperties"/> is <code>true</code>.
        /// </summary>
        public List<string> ExcludeProperties
        {
            get { return _ExcludeProperties; }
        }

        /// <summary>
        /// Gets a <see cref="List{String}"/> of property names that should be included in
        /// the output when <see cref="EnumerateAllProperties"/> is <code>false</code>.
        /// </summary>
        public List<string> IncludeProperties
        {
            get { return _IncludeProperties; }
        }

        /// <summary>
        /// Skip over trying to code gen these types
        /// </summary>
        /// <remarks>
        /// By design is a string so we don't haev to have references to all the future types we may want to exclude
        /// </remarks>
        public List<string> ExcludeTypes
        {
            get { return _ExcludeTypes; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not <see cref="System.Reflection.BindingFlags.Static"/> should be
        /// included in the <see cref="BindingFlags"/> property. The default is <code>false</code>.
        /// </summary>
        public bool BindStatic { get; set; }

        /// <summary>
        /// Gets a <see cref="BindingFlags"/> bitmask that is equal to <see cref="System.Reflection.BindingFlags.Public"/>,
        /// <see cref="System.Reflection.BindingFlags.Instance"/>, <see cref="System.Reflection.BindingFlags.GetField"/>,
        /// <see cref="System.Reflection.BindingFlags.GetProperty"/> and (if <see cref="BindStatic"/> is <code>true</code>)
        /// <see cref="System.Reflection.BindingFlags.Static"/>.
        /// </summary>
        public BindingFlags BindingFlags
        {
            get
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty;

                if (BindStatic)
                {
                    flags |= BindingFlags.Static;
                }

                return flags;
            }
        }
        #endregion

        #region Constructors
        
        /// <summary>
        /// Creates a new instance of <see cref="ObjectInspectorOptions"/>.
        /// </summary>
        public ObjectInspectorOptions()
        {
            EmitRegionWrappers = true;
            AssertFailAfterGeneration = true;
            EnumerateAllProperties = true;
            MaximumTraversalDepth = 999;

            _IncludeProperties = new List<string>();
            _ExcludeProperties = new List<string>();
            _ExcludeTypes = new List<string>();

            // Preset some WPF types that cause problems if we code gen against them
            _ExcludeTypes.Add("System.Windows.Threading.Dispatcher");
            _ExcludeTypes.Add("System.Windows.Media.Imaging.BitmapSource");
            _ExcludeTypes.Add("System.Windows.Media.Imaging.BitmapImage");
            _ExcludeTypes.Add("System.Windows.Media.Imaging.InteropBitmapSource");
            _ExcludeTypes.Add("System.Windows.Input.RoutedUICommand");
        }
        #endregion
    }
}
