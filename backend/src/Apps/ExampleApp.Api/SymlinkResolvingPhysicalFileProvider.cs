using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace ExampleApp.Api;

public sealed class SymlinkResolvingPhysicalFileProvider : IFileProvider
{
    private readonly PhysicalFileProvider innerProvider;

    public SymlinkResolvingPhysicalFileProvider(PhysicalFileProvider innerProvider)
    {
        this.innerProvider = innerProvider;
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        var result = innerProvider.GetFileInfo(subpath);

        if (result.Exists && result.PhysicalPath is not null)
        {
            var fsi = File.ResolveLinkTarget(result.PhysicalPath, true);

            if (fsi is FileInfo fi)
            {
                return new PhysicalFileInfo(fi);
            }
        }

        return result;
    }

    public IDirectoryContents GetDirectoryContents(string subpath) => innerProvider.GetDirectoryContents(subpath);

    public IChangeToken Watch(string filter) => innerProvider.Watch(filter);
}
