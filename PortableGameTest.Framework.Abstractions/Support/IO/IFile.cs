using System.IO;
using System.Threading.Tasks;

namespace PortableGameTest.Framework.Support.IO
{
	public interface IFile : IFileSystemElement
	{
		long Length { get; }

		Stream Open(FileAccess fileAccess);

		Task<Stream> OpenAsync(FileAccess fileAccess);

		void Delete();

		Task DeleteAsync();
	}
}