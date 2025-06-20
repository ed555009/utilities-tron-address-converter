using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;

namespace Utilities.Tron.AddressConverter;

/// <summary>
/// Provides a pool of <see cref="SHA256"/> objects to reduce allocations.
/// </summary>
public static class Sha256Pool
{
	private static readonly DefaultObjectPool<SHA256> _pool = new(new Sha256PooledObjectPolicy());

	/// <summary>
	/// Rents a <see cref="SHA256"/> instance from the pool.
	/// </summary>
	/// <returns>A <see cref="SHA256"/> instance.</returns>
	public static SHA256 Rent() => _pool.Get();

	/// <summary>
	/// Release a <see cref="SHA256"/> instance to the pool.
	/// </summary>
	/// <param name="sha256">The <see cref="SHA256"/> instance to return.</param>
	public static void Release(SHA256 sha256) => _pool.Return(sha256);

	private sealed class Sha256PooledObjectPolicy : PooledObjectPolicy<SHA256>
	{
		/// <inheritdoc/>
		public override SHA256 Create() => SHA256.Create();

		/// <inheritdoc/>
		public override bool Return(SHA256 obj) =>
			// HashAlgorithm has no reset, so always return true
			true;
	}
}
