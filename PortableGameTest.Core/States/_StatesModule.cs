﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core.States
{
    public class _StatesModule
        : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
	        builder.RegisterType<SplashState>().AsSelf().WithParameterExplicitNamingSupport();
        }
    }
}
