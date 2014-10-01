using System;
using System.Text;

namespace Nuclectic.Support.Helpers.InteropServices
{
	public interface IMarshalSizeOf
	{
		IntPtr? SizeOf(Type t);
	}
}
