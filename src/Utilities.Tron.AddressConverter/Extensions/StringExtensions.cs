using System.Security.Cryptography;
using SimpleBase;

namespace Utilities.Tron.AddressConverter.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Converts a hexadecimal string to a Tron address.
	/// </summary>
	/// <param name="hexAddress">The hexadecimal address string.</param>
	/// <returns>The Tron address string.</returns>
	/// <exception cref="ArgumentException">Thrown if the hex address is in an invalid format.</exception>
	public static string? ToTronAddress(this string? hexAddress)
	{
		if (string.IsNullOrEmpty(hexAddress))
			return null;

		ReadOnlySpan<char> hexSpan = hexAddress.AsSpan();
		if (hexSpan.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			hexSpan = hexSpan[2..];

		if (hexSpan.Length > 40)
			hexSpan = hexSpan[^40..];

		Span<byte> fullAddress = stackalloc byte[21];
		fullAddress[0] = 0x41;
		if (!TryFromHexString(hexSpan, fullAddress.Slice(1)))
			throw new ArgumentException("Invalid hex address format.");

		Span<byte> checksum = stackalloc byte[4];
		SHA256 sha256 = Sha256Pool.Rent();
		try
		{
			Span<byte> hash = stackalloc byte[32];
			sha256.TryComputeHash(fullAddress, hash, out _);
			sha256.TryComputeHash(hash, hash, out _);
			hash[..4].CopyTo(checksum);
		}
		finally
		{
			Sha256Pool.Release(sha256);
		}

		Span<byte> addressWithChecksum = stackalloc byte[25];
		fullAddress.CopyTo(addressWithChecksum);
		checksum.CopyTo(addressWithChecksum[21..]);

		return Base58.Bitcoin.Encode(addressWithChecksum);
	}

	/// <summary>
	/// Tries to convert a hexadecimal string to a byte span.
	/// </summary>
	/// <param name="hex">The hexadecimal string.</param>
	/// <param name="bytes">The span to write the bytes to.</param>
	/// <returns><c>true</c> if the conversion was successful; otherwise, <c>false</c>.</returns>
	private static bool TryFromHexString(ReadOnlySpan<char> hex, Span<byte> bytes)
	{
		if (hex.Length % 2 != 0 || bytes.Length < hex.Length / 2)
			return false;

		for (int i = 0; i < hex.Length; i += 2)
			if (!byte.TryParse(hex.Slice(i, 2), System.Globalization.NumberStyles.HexNumber, null, out bytes[i / 2]))
				return false;

		return true;
	}
}
