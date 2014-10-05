using System;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public interface IMarshalSizeOf
	{
		IntPtr? SizeOf(Type t);
	}
}
