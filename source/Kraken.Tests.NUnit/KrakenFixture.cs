using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using Autofac;
using Kraken.Tests;
using Common.Logging;
using NUnit.Framework;

namespace Kraken.Tests.NUnit
{
    [TestFixture]
    public abstract class KrakenFixture
    {
        private static readonly ILog _Log = LogManager.GetLogger<KrakenFixture>();

        private bool _isTestTempDirectoryCleaned;
        private TransactionScope _transactionScope;
        private static string _tempTestDirectory;

        /// <summary>
        /// Whether to automatically start and dispose a TransactionScope in setup and teardown
        /// </summary>
        public bool EnableTransactionScope { get; set; }
        
        public AssertBuilder AssertBuilder { get; protected set; }

        public ObjectComparer ObjectComparer { get; protected set; }

        public static IContainer ContainerContext { get; set; }

        protected IContainer Container { get; private set; }

        
        /// <summary>
        /// Instance of container builder which can be used by each individual test to obtain a container instance or auto-mocked container instance.
        /// </summary>
        protected ContainerBuilder ContainerBuilder { get; private set; }
        
        private bool _isContainerBuilt;
      
        protected static ILog Log
        {
            get { return _Log; }
        }

        protected string TestTempDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_tempTestDirectory))
                {
                    throw TestMonkeyException.Create("Please call SetTestTempDirectory to set unique test folder for your test project in order to allow parallel CI builds");
                }
                if (!_isTestTempDirectoryCleaned)
                {
                    
                    _isTestTempDirectoryCleaned = true;
                    if (Directory.Exists(_tempTestDirectory))
                    {
                        var dirInfo = new DirectoryInfo(_tempTestDirectory);
                        
                        foreach(var fsi in dirInfo.GetFileSystemInfos())
                        {
                            DeleteFileSystemInfo(fsi);
                        }
                        Directory.Delete(_tempTestDirectory, true);
                    }
                    Directory.CreateDirectory(_tempTestDirectory);
                }
                return _tempTestDirectory;
            }
        }

        private static void DeleteFileSystemInfo(FileSystemInfo fsi)
        {
            fsi.Attributes = FileAttributes.Normal;
            var di = fsi as DirectoryInfo;

            if (di != null)
            {
                foreach (var dirInfo in di.GetFileSystemInfos())
                {
                    DeleteFileSystemInfo(dirInfo);
                }
            }

            fsi.Delete();
        }
        
        #region Constructor

        protected KrakenFixture()
        {
            EnableTransactionScope = true;
        }
        #endregion
        
        public void WithinLifetimeScope(Action<ILifetimeScope> actionMethod)
        {
            BuildContainer();

            using (ILifetimeScope lifetime = Container.BeginLifetimeScope())
            {
                actionMethod(lifetime);
            }
        }

        #region Setup
        [SetUp]
        public virtual void Setup()
        {
            _isTestTempDirectoryCleaned = false;
            AssertBuilder = new AssertBuilder();
            ObjectComparer = new ObjectComparer();


            ContainerBuilder = new ContainerBuilder();
            _isContainerBuilt = false;
            RegisterAutofacModules();

            TestFrameworkFacade.AssertEqual = (o1, o2, s) => {Assert.AreEqual(o1, o2, s);};

            if (EnableTransactionScope)
            {
                _transactionScope = new TransactionScope();
            }
        }

        protected abstract void RegisterAutofacModules();

        protected void BuildContainer()
        {
            if (!_isContainerBuilt)
            {
                Container = ContainerBuilder.Build();
                _isContainerBuilt = true;
            }
        }

        [TearDown]
        public virtual void Teardown()
        {
            if (_transactionScope != null)
            {
                _transactionScope.Dispose();
            }
        }

        #endregion
        
        #region Static

        public static void SetTestTempDirectory(string directory)
        {
            _tempTestDirectory = directory;
        }

        #endregion

    }
}
