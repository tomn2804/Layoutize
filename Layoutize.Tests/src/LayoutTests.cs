using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Management.Automation;
using Xunit;

namespace Layoutize.Tests;

[Collection(nameof(WorkingDirectoryCollection))]
public abstract class LayoutTests : IDisposable
{
    private protected LayoutTests(WorkingDirectoryFixture fixture)
    {
        Shell = PowerShell.Create();
        WorkingDirectory = fixture.GetNewWorkingDirectory();
    }

    public static IEnumerable<object[]> InvalidAttributes => new[]
    {
        new object[]
        {
            new Hashtable() { { "Name", nameof(ValidAttributes) } },
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
        },
        new object[]
        {
            new Dictionary<object, object>() { { "Name", nameof(ValidAttributes) } },
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
        },
        new object[]
        {
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
        }
    };

    public static IEnumerable<object[]> ValidAttributes => new[]
        {
        new object[]
        {
            new Hashtable() { { "Name", nameof(ValidAttributes) } },
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
        },
        new object[]
        {
            new Dictionary<object, object>() { { "Name", nameof(ValidAttributes) } },
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
        },
        new object[]
        {
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidAttributes)) }),
        }
    };

    private protected PowerShell Shell { get; }

    private protected DirectoryInfo WorkingDirectory { get; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Shell.Dispose();
    }

    [Theory]
    [MemberData(nameof(ValidAttributes))]
    public void GetAttributes_InvalidEnumerableToImmutable_ThrowsException(IEnumerable attributes, IImmutableDictionary<object, object> expected)
    {
        Layout layout = CreateLayout(attributes);
        Assert.Equal(expected, layout.Attributes);
    }

    [Theory]
    [MemberData(nameof(ValidAttributes))]
    public void GetAttributes_ValidEnumerableToImmutable_ReturnsImmutableDictionary(IEnumerable attributes, IImmutableDictionary<object, object> expected)
    {
        Layout layout = CreateLayout(attributes);
        Assert.Equal(expected, layout.Attributes);
    }

    private protected abstract Layout CreateLayout(IEnumerable attributes);
}
