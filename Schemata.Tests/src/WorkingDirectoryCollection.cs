using Xunit;

namespace Schemata.Tests;

[CollectionDefinition("Working directory")]
public sealed class WorkingDirectoryCollection : ICollectionFixture<WorkingDirectoryFixture>
{
}
