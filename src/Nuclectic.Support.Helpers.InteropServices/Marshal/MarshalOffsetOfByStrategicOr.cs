using System;
using System.Collections.Generic;
using System.Linq;

namespace Nuclectic.Support.Helpers.InteropServices.Marshal
{
	public class MarshalOffsetOfByStrategicOr
		: IMarshalOffsetOf
	{
		private readonly IEnumerable<IMarshalOffsetOf> _providers;

		public MarshalOffsetOfByStrategicOr(params IMarshalOffsetOf[] providers)
			: this(providers.AsEnumerable()) { }

		public MarshalOffsetOfByStrategicOr(IEnumerable<IMarshalOffsetOf> providers)
		{
			if (providers == null) throw new ArgumentNullException("providers");
			_providers = providers;
		}

		public IntPtr? OffsetOf(Type t, string fieldName)
		{
			foreach (var provider in _providers)
			{
				var result = provider.OffsetOf(t, fieldName);
				if (result.HasValue)
					return result;
			}
			return null;
		}
	}
}