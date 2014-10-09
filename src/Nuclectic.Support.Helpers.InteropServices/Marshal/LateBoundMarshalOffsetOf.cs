using System;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public class LateBoundMarshalOffsetOf
		: IMarshalOffsetOf
	{
		private readonly Func<Type, string, IntPtr> _offsetOf;

		public LateBoundMarshalOffsetOf(Func<Type, string, IntPtr> offsetOf)
		{
			if (offsetOf == null) throw new ArgumentNullException("offsetOf");

			_offsetOf = offsetOf;
		}

		public IntPtr? OffsetOf(Type t, string fieldName) { return _offsetOf(t, fieldName); }
	}
}