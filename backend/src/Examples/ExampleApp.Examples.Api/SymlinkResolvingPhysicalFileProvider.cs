using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace ExampleApp.Examples.Api;

public sealed class SymlinkResolvingPhysicalFileProvider : PhysicalFileProvider, IFileProvider
{
    public SymlinkResolvingPhysicalFileProvider(string root)
        : base(root) { }

    public SymlinkResolvingPhysicalFileProvider(string root, ExclusionFilters filters)
        : base(root, filters) { }

    public new IFileInfo GetFileInfo(string subpath)
    {
        var result = base.GetFileInfo(subpath);

        return result.Exists && result.PhysicalPath is string path && File.ResolveLinkTarget(path, true) is FileInfo fi
            ? new PhysicalFileInfo(fi)
            : result;
    }
}
