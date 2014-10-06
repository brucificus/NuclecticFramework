using System.IO;
using System.Threading.Tasks;

namespace Nuclectic.Support.IO
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