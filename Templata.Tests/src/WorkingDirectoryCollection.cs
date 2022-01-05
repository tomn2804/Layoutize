using Xunit;

namespace Templata.Tests;

[CollectionDefinition("Working directory")]
public sealed class WorkingDirectoryCollection : ICollectionFixture<WorkingDirectoryFixture>
{
}
