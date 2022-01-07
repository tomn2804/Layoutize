using Xunit;

namespace Templatize.Tests;

[CollectionDefinition("Working directory")]
public sealed class WorkingDirectoryCollection : ICollectionFixture<WorkingDirectoryFixture>
{
}
