# Kraken.Tests
Uses reflection to generate `Assert` statements against a complex object in your chosen .NET test framework.

Useful for certain scenarios where generic `Assert.NotNull` isn't really cutting the mustard.

## XUnit Configuration
See the [sample project](https://github.com/neutmute/kraken.utilities/tree/master/samples/Sample.Tests.Xunit)

## NUnit Configuration
See [KrakenFixture.cs](https://github.com/neutmute/kraken.utilities/blob/master/source/Kraken.Tests.NUnit/KrakenFixture.cs)

## Usage
### AssertBuilder
1. `Arrange` and `Act` your test

		[Fact]
        public void AssertBuilderDemo()
        {
            // Arrange, Act
            var car = new Car();

            // Assert
            AssertBuilder.Generate(car, "car");

            // Look in the error message and you will see the asserts
        }
Add the call to the `AssertBuilder`, passing in the object you want to Assert against and the name of the variable

2. Execute your test. The test will fail but in the output you will find `Assert`
3. Paste in the asserts against the properties of your object - found in the failure message of your test

		// AssertBuilder.Generate(car, "car"); // The following assertions were generated on 19-Feb-2017
		#region Generated Assertions
		Assert.Equal(false, car.Spoiler);
		Assert.Equal(4, car.WheelCount);
		Assert.Equal("Wacky Wheels", car.Name);
		#endregion

4. You now have a speedily written test
	* Be sure to comment out the AssertBuilder line - it will always fail a test. 
	* Recompile and execute your test
	* Marvel at the time saved from all those magically written Assert's

#### Tips
* Use judiciously. Running `AssertBuilder` over a 50 item list sure makes for a lot of `Assert` calls but isn't proving a whole lot.
* Use the `Options` property to exclude dynamic data such as `CreatedDate` properties.
* Eyeball the results of your generated asserts to ensure they make sense. Applying blindly just `Asserts` that your code generated _something_ not that it is necessarily correct.

### ObjectComparer
ObjectComparer will traverse two different objects of the same type and assert that their properties are equal. Lists/arrays are supported and must be equal in length and have the same content:

     	[Test]
        public void AreEqualArray()
        {
            NetworkAddress address1 = new NetworkAddress(new byte[] { 1, 2 });
            NetworkAddress address2 = new NetworkAddress(new byte[] { 1, 2 });

            ObjectComparer.AssertEqual(address1, address2);
        }  

Use `ObjectComparer.Options` to specify properties that should be excluded from assertions.