using Utilities.Tron.AddressConverter.Extensions;

namespace Utilities.Tron.AddressConverter.Tests;

public class StringExtensionsTests()
{
	[Theory]
	[InlineData("0xa614f803b6fd780986a42c78ec9c7f77e6ded13c", "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t")]
	[InlineData(
		"0x000000000000000000000000ce5fca5d782a217aec65e60b890cf915725031c0",
		"TUnQxDUZ7vAoBxbuqURzp4kE6qoMcounGq"
	)]
	[InlineData(
		"0x000000000000000000000000d32dba5e02cfc68d28fde3ee401d69c1d8ab28ea",
		"TVDpMmjmM61Ac4KBywzD2BGa6iQbuqSbba"
	)]
	public void ToTronAddress_ShouldSuccess(string hexAddress, string tronAddress)
	{
		// Given

		// When
		var address = hexAddress.ToTronAddress();

		// Then
		Assert.Equal(tronAddress, address);
	}

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	public void ToTronAddress_WithNullOrEmpty_ShouldReturnNull(string? hexAddress)
	{
		// Given

		// When
		var tronAddress = hexAddress?.ToTronAddress();

		// Then
		Assert.Null(tronAddress);
	}

	[Theory]
	[InlineData("some-invalid-address")]
	[InlineData("TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t")]
	public void ToTronAddress_WithInvalidAddress_ShouldThrow(string hexAddress)
	{
		// Given

		// When
		var ex = Assert.Throws<ArgumentException>(() => hexAddress.ToTronAddress());

		// Then
		Assert.NotNull(ex);
	}
}
