using System;
using System.Diagnostics.CodeAnalysis;

namespace Layoutize.Annotations;

[AttributeUsage(AttributeTargets.Property)]
public abstract class ContextAttribute : Attribute, IEquatable<ContextAttribute>
{
	private object Key { get; }

	protected ContextAttribute(object key)
	{
		Key = key;
	}

	public bool Equals([NotNullWhen(true)] ContextAttribute? other)
	{
		return Key == other?.Key;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is ContextAttribute other && Equals(other);
	}

	public override int GetHashCode()
	{
		return Key.GetHashCode();
	}
}
