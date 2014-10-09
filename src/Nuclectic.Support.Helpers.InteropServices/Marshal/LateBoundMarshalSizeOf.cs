using System;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public class LateBoundMarshalSizeOf
		: IMarshalSizeOf
	{
		private readonly Func<Type, int> _sizeOf;

		public LateBoundMarshalSizeOf(Func<Type, int> sizeOf)
		{
			if (sizeOf == null) throw new ArgumentNullException("sizeOf");

			_sizeOf = sizeOf;
		}

		public int? SizeOf(Type t) { return _sizeOf(t); }
	}
}