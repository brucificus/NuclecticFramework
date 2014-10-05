using System;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public interface IMarshalOffsetOf
	{
		IntPtr? OffsetOf(Type t, string fieldName);
	}
}