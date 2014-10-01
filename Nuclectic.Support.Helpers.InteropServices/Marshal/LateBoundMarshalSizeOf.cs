using System;

namespace Nuclectic.Support.Helpers.InteropServices
{
	public class LateBoundMarshalSizeOf
		: IMarshalSizeOf
	{
		private readonly Func<Type, IntPtr> _sizeOf;

		public LateBoundMarshalSizeOf(Func<Type, IntPtr> sizeOf)
		{
			if (sizeOf == null) throw new ArgumentNullException("sizeOf");

			_sizeOf = sizeOf;
		}

		public IntPtr? SizeOf(Type t)
		{
			return _sizeOf(t);
		}
	}
}