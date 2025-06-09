# Utilities.Tron.AddressConverter

[![GitHub](https://img.shields.io/github/license/ed555009/utilities-tron-address-converter)](LICENSE)
![Build Status](https://dev.azure.com/edwang/github/_apis/build/status/utilities-tron-address-converter?branchName=main)
[![Nuget](https://img.shields.io/nuget/v/Utilities.Tron.AddressConverter)](https://www.nuget.org/packages/Utilities.Tron.AddressConverter)

![Coverage](https://sonarcloud.io/api/project_badges/measure?project=utilities-tron-address-converter&metric=coverage)
![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=utilities-tron-address-converter&metric=alert_status)
![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=utilities-tron-address-converter&metric=reliability_rating)
![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=utilities-tron-address-converter&metric=security_rating)
![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=utilities-tron-address-converter&metric=vulnerabilities)

A simple .NET library for conversion between HEX format addresses and TRON blockchain addresses, targeting .NET 8.

## Description

This library provides utility extension methods to convert hexadecimal string representations of addresses to the Base58Check encoded TRON address format. It also includes an object pool for `SHA256` instances to optimize performance and reduce memory allocations during repeated hashing operations required for address checksum calculations.

## Features

- Convert hexadecimal addresses to TRON addresses.
- Handles optional "0x" prefix in hex addresses.
- Efficiently computes checksums using a pooled `SHA256` implementation.

## Installation

You can install the package via NuGet Package Manager:

```powershell
Install-Package Utilities.Tron.AddressConverter
```

Or via the .NET CLI:

```bash
dotnet add package Utilities.Tron.AddressConverter
```

## Usage

Here's a basic example of how to convert a hexadecimal address to a TRON address:

```csharp
using Utilities.Tron.AddressConverter.Extensions;

public class Example
{
    public static void Main(string[] args)
    {
        string? hexAddress = "0xa614f803b6fd780986a42c78ec9c7f77e6ded13c"; // Example hex address (with or without "0x" prefix)
        string? tronAddress = hexAddress.ToTronAddress();

        if (tronAddress != null)
        {
            Console.WriteLine($"Hex Address: {hexAddress}");
            Console.WriteLine($"Tron Address: {tronAddress}");
            // Expected Tron Address: TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t (for the example hex above without the 0x41 prefix)
            // If the hexAddress already includes the 0x41 prefix, the output will be the same.
        }
        else
        {
            Console.WriteLine("Invalid hex address provided.");
        }
    }
}
```

### `Sha256Pool`

For applications performing many address conversions, the `Sha256Pool` class is used internally by `ToTronAddress` to manage `SHA256` crypto provider instances. This helps in reducing GC pressure by reusing `SHA256` objects. If you have other parts of your application that require `SHA256` hashing, you can also leverage this pool:

```csharp
using System.Security.Cryptography;
using Utilities.Tron.AddressConverter; // Namespace for Sha256Pool

// ...

SHA256 sha256Instance = Sha256Pool.Rent();
try
{
    // Use sha256Instance for hashing operations
    // byte[] dataToHash = ...;
    // byte[] hash = sha256Instance.ComputeHash(dataToHash);
}
finally
{
    Sha256Pool.Return(sha256Instance);
}
```
