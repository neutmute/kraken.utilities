# Kraken.Tests
Generate `Assert` statements against a complex object in your chosen .NET test framework

## XUnit Configuration
In your base test constructor, add the following code to configure for XUnit
	
First add some properties

    public ObjectComparer ObjectComparer { get; set; }

    public CodeGenerator CodeGen
    {
        get; private set;
    }

Then add this to your constructor to inform the framework how to execute assertions

    CodeGen.Options.AssertSignatures.AreEqual = "Equal";
    CodeGen.Options.AssertSignatures.IsNull = "Null";
    CodeGen.AppendEmittedCodeToFailMessage = true;

	TestFrameworkFacade.AssertEqual = (o1, o2, m) =>
	{
	    if (o1 is string && o2 is string)
	    {
	        // get original messages through
	        var o1AsString = o1 as string;
	        var o2AsString = o2 as string;
	        if (!o1AsString.Equals(o2AsString))
	        {
	            Assert.False(true, m);
	        }
	    }
	    else
	    {
	        Assert.Equal(o1, o2);
	    }
	};
	TestFrameworkFacade.AssertNotEqual = (o1, o2, m) => { Assert.NotEqual(o1, o2); };
	TestFrameworkFacade.AssertFail = (mf, args) => {
	    Log.InfoFormat(mf, args);
	    if (args.Length == 0)
	    {
	        Assert.False(true, mf);
	    }
	    else
	    {
	        Assert.False(true, string.Format(mf, args));
	    }
	};