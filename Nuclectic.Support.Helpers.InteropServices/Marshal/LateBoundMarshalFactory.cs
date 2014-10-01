using System;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public class LateBoundMarshalFactory
	{
		private static readonly Lazy<Type> MarshalType = new Lazy<Type>(FindMarshalType);

		public IMarshalSizeOf CreateMarshalSizeOf()
		{
			var marshalType = MarshalType.Value;

			if (marshalType == null)
				return null;

			var sizeOfMethod = marshalType.GetMethod("SizeOf", new[] {typeof (Type)});

			if (sizeOfMethod == null)
				return null;

			return new LateBoundMarshalSizeOf((Type t) => (IntPtr)sizeOfMethod.Invoke(null, new object[]{t}));
		}

		public IMarshalOffsetOf CreateMarshalOffsetOf()
		{
			var marshalType = MarshalType.Value;

			if (marshalType == null)
				return null;

			var offsetOfMethod = marshalType.GetMethod("OffsetOf", new[] { typeof(Type), typeof(string) });

			if (offsetOfMethod == null)
				return null;

			return new LateBoundMarshalOffsetOf((Type t, string f) => (IntPtr)offsetOfMethod.Invoke(null, new object[] { t, f }));
		}

		private static Type FindMarshalType()
		{
			var result = System.Type.GetType("System.Runtime.InteropServices.Marshal", false);
			return result;
		}
	}
}