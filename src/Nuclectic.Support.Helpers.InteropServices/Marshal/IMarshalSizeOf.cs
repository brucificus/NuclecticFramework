using System;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public interface IMarshalSizeOf
	{
		int? SizeOf(Type t);
	}
}
