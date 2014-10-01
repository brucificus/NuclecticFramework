using System;

namespace Nuclectic.Support.Helpers.InteropServices
{
	public interface IMarshalOffsetOf
	{
		IntPtr? OffsetOf(Type t, string fieldName);
	}
}