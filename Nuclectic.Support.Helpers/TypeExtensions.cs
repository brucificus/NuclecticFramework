using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nuclectic.Support.Helpers
{
	public static class TypeExtensions
	{
		public static MethodInfo GetMethod(this Type self, string name, params Type[] parameterTypes)
		{
			var methodsWithName = self.GetTypeInfo().DeclaredMethods.Where(mi => mi.Name == name).ToArray();
			if (!methodsWithName.Any())
				return null;

			var methodsWithParameterCount = methodsWithName.Where(mi => mi.GetParameters().Length == parameterTypes.Length);
			if (!methodsWithName.Any())
				return null;

			var result = methodsWithParameterCount.SingleOrDefault(mi => mi.GetParameters().Select(p=>p.ParameterType).SequenceEqual(parameterTypes));

			return result;
		}

		public static FieldInfo GetField(this Type self, string name)
		{
			return self.GetTypeInfo().DeclaredFields.SingleOrDefault(fi => fi.Name == name);
		}

		public static IEnumerable<FieldInfo> GetFields(this Type self)
		{
			return self.GetTypeInfo().DeclaredFields;
		}
	}
}
