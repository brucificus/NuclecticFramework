using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;

namespace PortableGameTest.Framework.Support
{
	public static class RegistrationBuilderExtensions
	{
		public static IRegistrationBuilder<T1, T2, T3> WithParameterExplicitNamingSupport<T1, T2, T3>(this IRegistrationBuilder<T1, T2, T3> self) where T2 : ReflectionActivatorData
		{
			return self.WithParameter((pi, ctx) => NamedAttribute.ValueOf(pi) != null,
				(pi, ctx) => ctx.ResolveNamed(NamedAttribute.ValueOf(pi), pi.ParameterType));
		}
	}
}
