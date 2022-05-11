using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace Layoutize.Tests;

public abstract class LayoutTests
{
    public static IEnumerable<object[]> InvalidLayoutAttributes => new[]
    {
        new object[]
        {
            new Hashtable(),
            typeof(KeyNotFoundException),
        },
        new object[]
        {
            new Dictionary<object, object>(),
            typeof(KeyNotFoundException),
        },
        new object[]
        {
            ImmutableDictionary<object, object>.Empty,
            typeof(KeyNotFoundException),
        }
    };

    public static IEnumerable<object[]> ValidLayoutAttributes => new[]
    {
        new object[]
        {
            new Hashtable() { { "Name", nameof(ValidLayoutAttributes) } },
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidLayoutAttributes)) }),
        },
        new object[]
        {
            new Dictionary<object, object>() { { "Name", nameof(ValidLayoutAttributes) } },
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidLayoutAttributes)) }),
        },
        new object[]
        {
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidLayoutAttributes)) }),
            ImmutableDictionary.CreateRange(new[] { KeyValuePair.Create<object, object>("Name", nameof(ValidLayoutAttributes)) }),
        }
    };

    [Theory]
    [MemberData(nameof(InvalidLayoutAttributes))]
    public void GetAttributes_ConstructWithInvalidEnumerable_ThrowsException(IDictionary attributes, Type expectedExceptionType)
    {
        Assert.Throws(expectedExceptionType, () => CreateLayout(attributes));
    }

    [Theory]
    [MemberData(nameof(ValidLayoutAttributes))]
    public void GetAttributes_ConstructWithValidEnumerable_ReturnsImmutableDictionary(IDictionary attributes, IImmutableDictionary<object, object> expected)
    {
        Layout layout = CreateLayout(attributes);
        if (attributes is not IImmutableDictionary<object, object>)
        {
            attributes.Clear();
            attributes.Add(new object(), new object());
        }
        else
        {
            Assert.True(layout.Attributes == attributes);
        }
        layout.Attributes.Clear();
        layout.Attributes.Add(new object(), new object());
        Assert.Equal(expected, layout.Attributes);
    }

    private protected abstract Layout CreateLayout(IDictionary attributes);
}
