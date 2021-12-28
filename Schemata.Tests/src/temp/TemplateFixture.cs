using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Schemata.Tests;

public sealed class TemplateFixture : IDisposable
{
    private PowerShell Instance { get; } = PowerShell.Create();

    public Template DynamicDirectoryTemplate { get; }

    public Template DynamicFileTemplate { get; }

    public TemplateFixture()
    {
        DynamicDirectoryTemplate = (Template)Instance.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class DynamicDirectoryTemplate : Template[DirectoryModel] {{
                DynamicDirectoryTemplate([IEnumerable]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{ return [DirectoryTemplate]$this.Details }}
            }}

            [DynamicDirectoryTemplate]@{{ [Template+RequiredDetails]::Name = 'DynamicDirectoryTemplate' }}
        ").Invoke().Last().BaseObject;

        DynamicFileTemplate = (Template)Instance.AddScript($@"
            using module Schemata
            using namespace Schemata
            using namespace System.Collections

            class DynamicFileTemplate : Template[FileModel] {{
                DynamicFileTemplate([IEnumerable]$details) : base($details) {{}}

                [Blueprint]ToBlueprint() {{ return [FileTemplate]$this.Details }}
            }}

            [DynamicFileTemplate]@{{ [Template+RequiredDetails]::Name = 'DynamicFileTemplate' }}
        ").Invoke().Last().BaseObject;
    }

    public void Dispose()
    {
        Instance.Dispose();
    }
}
