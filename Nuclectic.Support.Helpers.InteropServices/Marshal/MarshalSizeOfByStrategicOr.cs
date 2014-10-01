using System;
using System.Collections.Generic;

namespace Nuclectic.Support.Helpers.InteropServices
{
	public class MarshalSizeOfByStrategicOr
		: IMarshalSizeOf
	{
		private readonly IEnumerable<IMarshalSizeOf> _providers;

		public MarshalSizeOfByStrategicOr(IEnumerable<IMarshalSizeOf> providers)
		{
			_providers = providers;
		}

		public IntPtr? SizeOf(Type t)
		{
			foreach (var provider in _providers)
			{
				var result = provider.SizeOf(t);
				if (result.HasValue)
					return result.Value;
			}
			return null;
		}
	}
}