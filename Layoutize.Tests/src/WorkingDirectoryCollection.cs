using Xunit;

namespace Layoutize.Tests;

[CollectionDefinition(nameof(WorkingDirectoryCollection))]
public sealed class WorkingDirectoryCollection : ICollectionFixture<WorkingDirectoryFixture>
{
}
