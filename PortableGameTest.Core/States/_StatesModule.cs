using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Nuclex.Game.States;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core.States
{
    public class _StatesModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			var stateTypesInThisAssembly = 
				typeof(_StatesModule)
				.GetTypeInfo()
				.Assembly
				.DefinedTypes
				.Where(t => t.IsClass && !t.IsAbstract && t.ImplementedInterfaces.Contains(typeof(IGameState)))
				.ToArray();

	        foreach (var stateType in stateTypesInThisAssembly)
	        {
		        builder.RegisterType(stateType.AsType()).AsSelf().WithParameterExplicitNamingSupport();
	        }
        }
    }
}
