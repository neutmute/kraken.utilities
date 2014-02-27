using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using Kraken.Framework.TestMonkey;
using Kraken.Framework.TestMonkey.Tests;
using NLog;
using NLog.Targets;
using NUnit.Framework;
using UnitTests.TestClasses;

namespace UnitTests
{
	/// <summary>
	/// AssertBuilderFixture
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <date>7-May-2007</date>
	[TestFixture]
	public class AssertBuilderFixture : Fixture
	{
        #region Properties
        /// <summary>
        /// Override the default generator with our own default options
        /// </summary>
        public AssertBuilder CodeGen
        {
            get 
            {
                AssertBuilder codeGen = new AssertBuilder();
                codeGen.Options = GetDefaultOptions();
                return codeGen; 
            }
        }
        #endregion

        #region Instance Methods
        public CodeGenOptions GetDefaultOptions()
        {
            CodeGenOptions options = new CodeGenOptions();
            options.EmitRegionWrappers = false;
            options.AssertFailAfterGeneration = false;
            return options;
        }

		/// <summary>
		/// Generates code assertions to assert the code generator is working
		/// </summary>
        private void GetAssertBuilderOutput(AssertBuilder codeGen, object target, params string[] ignoreProperties)
		{
			ObjectXRay xray = ObjectXRay.NewType(typeof(AssertBuilder), codeGen);
            xray.SetProperty("TestMode", true);
			codeGen.Generate(target, "target", ignoreProperties);

			Console.WriteLine("Assert.AreEqual(@\"" + EncodeForString(codeGen.GetEmittedCode()) + "\", codeGen.GetEmittedCode());");
		}

        /// <summary>
        /// Generates code assertions to assert the code generator is working
        /// </summary>
        private void GetAssertBuilderOutput(AssertBuilder codeGen, object target, CodeGenOptions options)
        {
            ObjectXRay xray = ObjectXRay.NewType(typeof(AssertBuilder), codeGen);
            xray.SetProperty("TestMode", true);
            codeGen.Generate(target, "target", options);

            Console.WriteLine("Assert.AreEqual(@\"" + EncodeForString(codeGen.GetEmittedCode()) + "\", codeGen.GetEmittedCode());");
        }

		private void GetAssertBuilderOutput(AssertBuilder codeGen, Type target)
		{
			ObjectXRay xray = ObjectXRay.NewType(typeof(AssertBuilder), codeGen);
			xray.SetProperty("TestMode", true);
			codeGen.Generate(target);

			Console.WriteLine("Assert.AreEqual(@\"" + EncodeForString(codeGen.GetEmittedCode()) + "\", codeGen.GetEmittedCode());");
		}

		/// <summary>
		/// Replace quotes with delimited quotes compatible with C#
		/// </summary>
		private string EncodeForString(string input)
		{
			return input.Replace("\"",@"""""");
        }
        #endregion

        #region Test Methods
        [Test]
        public void MemoryTargetTest()
        {
            MemoryTarget target = Log.GetMemoryTarget();

            Log.Info("Here is some log data");

            Assert.AreEqual(1, target.Logs.Count);
            Assert.AreEqual("Here is some log data", target.Logs[0]);

        }

        [Test]
		public void DateTimeDoesntEnumerate()
		{
			DateTime testDate = DateTime.Parse("2001-02-03 04:05:06");
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, testDate);
            Assert.AreEqual(@"Assert.AreEqual(Convert.ToDateTime(""03-Feb-2001 04:05:06.000""), target);", codeGen.GetEmittedCode());

		}

		[Test]
		public void DateTimeEmitsTimeOfDayOnlyWhenNecessary()
		{
			DateTime testDate = DateTime.Parse("2001-02-03");
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, testDate);
            Assert.AreEqual(@"Assert.AreEqual(Convert.ToDateTime(""03-Feb-2001""), target);", codeGen.GetEmittedCode());
		}

        [Test]
        public void Boolean ()
        {
            Light light = new Light { IsOn = true, DimmerPosition = 100 };
            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, light);

            Assert.AreEqual(@"Assert.AreEqual(true, target.IsOn);
Assert.AreEqual(100, target.DimmerPosition);", codeGen.GetEmittedCode());
        }

	    [Test]
		public void FieldsEnumerated()
		{
			Animal cat = new Animal();
			cat.LegCount = 3;
			cat.Sound = "meow";

			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, cat);

			Assert.AreEqual(@"Assert.AreEqual(3, target.LegCount);
Assert.AreEqual(""meow"", target.Sound);", codeGen.GetEmittedCode());
		}

		[Test]
		public void Enum()
		{
			Classification classification = Classification.Mineral;
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, classification);

			Assert.AreEqual(@"Assert.AreEqual(UnitTests.Classification.Mineral, target);", codeGen.GetEmittedCode());
		}

		[Test]
		public void Double()
		{
			Double myNumber = 3.14159654;

			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, myNumber);

            Assert.AreEqual("Assert.AreEqual(\"3.14159654\", target.ToString());", codeGen.GetEmittedCode());
		}

        [Test]
        public void DoubleHighPrecision()
        {
            Double myNumber = 414.53936348408712d;

            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, myNumber);

            Assert.AreEqual("Assert.AreEqual(\"414.539363484087\", target.ToString());", codeGen.GetEmittedCode());
        }

		[Test]
		public void Decimal()
		{
			Decimal myNumber = 3.14159654m;

			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, myNumber);

			Assert.AreEqual(@"Assert.AreEqual(3.14159654m, target);", codeGen.GetEmittedCode());
		}

		[Test]
		public void SinglelineStringsEscaped()
		{
			string input = "this tests that the single line string escaping \"Here is a quoted message\" ";
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, input);
			
			Assert.AreEqual(@"Assert.AreEqual(""this tests that the single line string escaping \""Here is a quoted message\"" "", target);", codeGen.GetEmittedCode());
		}

		[Test]
		public void MultilineStringsEscaped()
		{
			string input = "this tests that the \r\n correct escape sequence is used over multiple lines. \"Here is a quoted message\" ";
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, input);

			Assert.AreEqual(@"Assert.AreEqual(@""this tests that the 
 correct escape sequence is used over multiple lines. """"Here is a quoted message"""" "", target);", codeGen.GetEmittedCode());

		}

		[Test]
		public void StaticClass()
		{
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, typeof(Configuration));

			Assert.AreEqual(@"Assert.AreEqual(""value1"", Configuration.ConfigValue1);
Assert.AreEqual(-1, Configuration.ConfigValue2);", codeGen.GetEmittedCode());
		}

		[Test]
		public void NoPropertiesToEnumerate()
		{
			AbsoluteBearing bearing = new AbsoluteBearing(12.3456789m);
			AssertBuilder codeGen = new AssertBuilder();
			GetAssertBuilderOutput(codeGen, bearing);

			Assert.AreEqual(@"Assert.AreEqual(""12.34567890"", target.ToString());", codeGen.GetEmittedCode());
		}

        //[Ignore("Functionality regression")]
        [Test]
        public void Indexers()
        {
            Indexer indexer = new Indexer();
            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, indexer);

            Assert.AreEqual(@"System.Collections.IEnumerator enumerator = target.GetEnumerator();
object enumeratorPointer = enumerator.Current;
Assert.AreEqual(4, enumeratorPointer);
enumeratorPointer = enumerator.MoveNext();
Assert.AreEqual(5, enumeratorPointer);
enumeratorPointer = enumerator.MoveNext();
Assert.AreEqual(6, enumeratorPointer);
enumeratorPointer = enumerator.MoveNext();", codeGen.GetEmittedCode());
        }

        /// <summary>
        /// Older versions emitted Assert.AreEqual("12.34000000", target.ToString()); which caused cast failures
        /// </summary>
        [Test]
        public void ToStringOverride()
        {
            AbsoluteBearing bearing = new AbsoluteBearing(12.34m);
            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, bearing);

            Assert.AreEqual(@"Assert.AreEqual(""12.34000000"", target.ToString());", codeGen.GetEmittedCode());
        }

	    [Test]
        public void Float()
        {
            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, 24.5f);
            Assert.AreEqual(@"Assert.AreEqual(24.5f, target);", codeGen.GetEmittedCode());

            codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, Single.MinValue);
            Assert.AreEqual(@"Assert.AreEqual(-3.402823E+38f, target);", codeGen.GetEmittedCode());
        }

        [Test]
        public void ExcludeProperties()
        {
            AnimalCollection animals = new Zoo().Animals;



            AssertBuilder codeGen = new AssertBuilder();
            CodeGenOptions options = GetDefaultOptions();
            options.ExcludeProperties.Add("sound");         // test case insensitivity

            GetAssertBuilderOutput(codeGen, animals, options);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Count);
Assert.AreEqual(4, target[0].LegCount);
Assert.AreEqual(2, target[1].LegCount);
Assert.AreEqual(3, target[2].LegCount);", codeGen.GetEmittedCode());
        }

	    [Test]
        public void CastBeforeAssertActualType()
        {
            Zoo zoo = new Zoo();
            AnimalCollection animals = zoo.Animals;

            CodeGenOptions options = GetDefaultOptions();
            options.EnumerateAllCollectionProperties = true;
            options.UpcastTypes.Add(typeof(Animal));

            AssertBuilder codeGen = new AssertBuilder();
            
            GetAssertBuilderOutput(codeGen, animals, options);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Count);
Assert.AreEqual(4, ((UnitTests.Animal)target[0]).LegCount);
Assert.AreEqual(null, ((UnitTests.Animal)target[0]).Sound);
Assert.AreEqual(2, ((UnitTests.Animal)target[1]).LegCount);
Assert.AreEqual(null, ((UnitTests.Animal)target[1]).Sound);
Assert.AreEqual(3, ((UnitTests.Animal)target[2]).LegCount);
Assert.AreEqual(null, ((UnitTests.Animal)target[2]).Sound);
Assert.AreEqual(9, target.TotalLegs);
Assert.AreEqual(4, target.Capacity);
Assert.AreEqual(3, target.Count);", codeGen.GetEmittedCode());

        }

        [Test]
        public void CastBeforeAssertSubType()
        {
            Zoo zoo = new Zoo();
            AnimalCollection animals = zoo.Animals;

            CodeGenOptions options = GetDefaultOptions();
            options.EnumerateAllCollectionProperties = true;
            options.UpcastTypes.Add(typeof(IAnimal));

            AssertBuilder codeGen = new AssertBuilder();

            GetAssertBuilderOutput(codeGen, animals, options);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Count);
Assert.AreEqual(4, ((UnitTests.Animal)target[0]).LegCount);
Assert.AreEqual(null, ((UnitTests.Animal)target[0]).Sound);
Assert.AreEqual(2, ((UnitTests.Animal)target[1]).LegCount);
Assert.AreEqual(null, ((UnitTests.Animal)target[1]).Sound);
Assert.AreEqual(3, ((UnitTests.Animal)target[2]).LegCount);
Assert.AreEqual(null, ((UnitTests.Animal)target[2]).Sound);
Assert.AreEqual(9, target.TotalLegs);
Assert.AreEqual(4, target.Capacity);
Assert.AreEqual(3, target.Count);", codeGen.GetEmittedCode());

        }

	    [Test]
        public void EnumerateAllCollectionProperties()
        {
            Zoo zoo = new Zoo();
            AnimalCollection animals = zoo.Animals;
            CodeGenOptions options = GetDefaultOptions();
            options.EnumerateAllCollectionProperties = true;

            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, animals, options);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Count);
Assert.AreEqual(4, target[0].LegCount);
Assert.AreEqual(null, target[0].Sound);
Assert.AreEqual(2, target[1].LegCount);
Assert.AreEqual(null, target[1].Sound);
Assert.AreEqual(3, target[2].LegCount);
Assert.AreEqual(null, target[2].Sound);
Assert.AreEqual(9, target.TotalLegs);
Assert.AreEqual(4, target.Capacity);
Assert.AreEqual(3, target.Count);", codeGen.GetEmittedCode());
        }
        
        [Test]
        public void EnumerateAllCollectionPropertiesInsideAnObject()
        {
            Zoo zoo = new Zoo();

            CodeGenOptions options = GetDefaultOptions();
            options.EnumerateAllCollectionProperties = true;

            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, zoo, options);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Animals.Count);
Assert.AreEqual(4, target.Animals[0].LegCount);
Assert.AreEqual(null, target.Animals[0].Sound);
Assert.AreEqual(2, target.Animals[1].LegCount);
Assert.AreEqual(null, target.Animals[1].Sound);
Assert.AreEqual(3, target.Animals[2].LegCount);
Assert.AreEqual(null, target.Animals[2].Sound);
Assert.AreEqual(9, target.Animals.TotalLegs);
Assert.AreEqual(4, target.Animals.Capacity);
Assert.AreEqual(3, target.Animals.Count);", codeGen.GetEmittedCode());
        }

        [Test]
        public void ExplicitInclude()
        {
            Zoo zoo = new Zoo();
            AnimalCollection animals = zoo.Animals;

            AssertBuilder codeGen = new AssertBuilder();
            codeGen.Options.IncludeProperties.Add("LegCount");
            codeGen.Options.EnumerateAllProperties = false;
            GetAssertBuilderOutput(codeGen, animals);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Count);
Assert.AreEqual(4, target[0].LegCount);
Assert.AreEqual(2, target[1].LegCount);
Assert.AreEqual(3, target[2].LegCount);", codeGen.GetEmittedCode());
        }

	    [Test]
        public void MaximumTraversalDepth()
        {
            ParentChain greatGrandfather = ParentChain.GetGrandFatherSample();

            CodeGenOptions options = GetDefaultOptions();
	        options.MaximumTraversalDepth = 999;

            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, greatGrandfather, options);

            Assert.AreEqual(@"Assert.AreEqual(null, target.Child.Child.Child.Child);
Assert.AreEqual(""son"", target.Child.Child.Child.Name);
Assert.AreEqual(""father"", target.Child.Child.Name);
Assert.AreEqual(""grandfather"", target.Child.Name);
Assert.AreEqual(""greatGrandfather"", target.Name);", codeGen.GetEmittedCode());

            options.MaximumTraversalDepth = 2;
            GetAssertBuilderOutput(codeGen, greatGrandfather, options);

            Assert.AreEqual(@"Assert.AreEqual(""grandfather"", target.Child.Name);
Assert.AreEqual(""greatGrandfather"", target.Name);", codeGen.GetEmittedCode());
        }

        [Test]
        public void DataRow()
        {
            ColourDataTable dataTable = new ColourDataTable();
            DataRow dataRow = dataTable.Rows[0];

            AssertBuilder codeGen = new AssertBuilder();
            codeGen.Options.ExcludeProperties.Add("DateCreated");
            GetAssertBuilderOutput(codeGen, dataRow);

            Assert.AreEqual(@"Assert.AreEqual(1, target[""Id""]);
Assert.AreEqual(""Red"", target[""Name""]);
Assert.AreEqual(""FF0000"", target[""Rgb""]);", codeGen.GetEmittedCode());
        }

	    [Test]
        public void DataTable()
        {
            ColourDataTable dataTable = new ColourDataTable();

            AssertBuilder codeGen = new AssertBuilder(); 
            GetAssertBuilderOutput(codeGen, dataTable);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Rows.Count);
Assert.AreEqual(1, target.Rows[0][""Id""]);
Assert.AreEqual(""Red"", target.Rows[0][""Name""]);
Assert.AreEqual(""FF0000"", target.Rows[0][""Rgb""]);
Assert.AreEqual(Convert.ToDateTime(""29-Mar-2010 16:56:00.000""), target.Rows[0][""DateCreated""]);
Assert.AreEqual(2, target.Rows[1][""Id""]);
Assert.AreEqual(""Green"", target.Rows[1][""Name""]);
Assert.AreEqual(""00FF00"", target.Rows[1][""Rgb""]);
Assert.AreEqual(Convert.ToDateTime(""28-Mar-2010 16:56:00.000""), target.Rows[1][""DateCreated""]);
Assert.AreEqual(3, target.Rows[2][""Id""]);
Assert.AreEqual(""Blue"", target.Rows[2][""Name""]);
Assert.AreEqual(""0000FF"", target.Rows[2][""Rgb""]);
Assert.AreEqual(Convert.ToDateTime(""29-Mar-2010 16:56:00.500""), target.Rows[2][""DateCreated""]);", codeGen.GetEmittedCode());


        }

        [Test]
        public void DataTableHonoursInclusiveProperties()
        {
            ColourDataTable dataTable = new ColourDataTable();

            AssertBuilder codeGen = new AssertBuilder();
            codeGen.Options.EnumerateAllProperties = false;
            codeGen.Options.IncludeProperties.Add("Name");
            codeGen.Options.ExcludeProperties.Add("DateCreated");

            GetAssertBuilderOutput(codeGen, dataTable);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Rows.Count);
Assert.AreEqual(""Red"", target.Rows[0][""Name""]);
Assert.AreEqual(""Green"", target.Rows[1][""Name""]);
Assert.AreEqual(""Blue"", target.Rows[2][""Name""]);", codeGen.GetEmittedCode());

        }

        [Test]
        public void ListString()
        {
            List<string> strings = new List<string> {"Dog", "Cat", "Horse", "Bird"};

            AssertBuilder codeGen = new AssertBuilder();
            GetAssertBuilderOutput(codeGen, strings);

            Assert.AreEqual(@"Assert.AreEqual(4, target.Count);
Assert.AreEqual(""Dog"", target[0]);
Assert.AreEqual(""Cat"", target[1]);
Assert.AreEqual(""Horse"", target[2]);
Assert.AreEqual(""Bird"", target[3]);", codeGen.GetEmittedCode());
        }

	    [Test]
        public void SyntacticalSugarListString()
        {
            Zoo zoo = new Zoo();

            AssertBuilder codeGen = new AssertBuilder();
            codeGen.Options.EnumerateAllProperties = false;
            codeGen.Options.IncludeProperties.Add("Animals", "Name", "Sound", "Count");

            GetAssertBuilderOutput(codeGen, zoo);

            Assert.AreEqual(@"Assert.AreEqual(3, target.Animals.Count);
Assert.AreEqual(null, target.Animals[0].Sound);
Assert.AreEqual(null, target.Animals[1].Sound);
Assert.AreEqual(null, target.Animals[2].Sound);", codeGen.GetEmittedCode());
        }
        #endregion
	}
}
