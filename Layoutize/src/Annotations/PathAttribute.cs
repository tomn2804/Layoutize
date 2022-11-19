using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Layoutize.Annotations;

internal sealed class PathAttribute : LayoutAttribute
{
	protected override void Validate([NotNull] object? value)
	{
		if (value is not string path)
		{
			throw new ValidationException($"'{nameof(PathAttribute)}' value is not of type string.");
		}
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new ValidationException(
				$"'{nameof(PathAttribute)}' value is either null, empty, or consists of only white-space characters."
			);
		}
		if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
		{
			throw new ValidationException($"'{nameof(PathAttribute)}' value contains invalid characters.");
		}
		if (!Path.IsPathFullyQualified(path))
		{
			throw new ValidationException($"'{nameof(PathAttribute)}' value is not an absolute path.");
		}
	}
}
