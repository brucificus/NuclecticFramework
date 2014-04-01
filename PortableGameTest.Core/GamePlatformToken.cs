using System;
using System.Collections.Generic;
using PortableGameTest.Core.States;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core
{
    public abstract class GamePlatformToken
    {
        public virtual IEnumerable<Type> GetAutofacModuleTypes()
        {
            yield return typeof (_StatesModule);
        }

        public abstract string Name { get; }
    }

    public abstract class GamePlatformToken<TGameFrameworkPlatform>
        : GamePlatformToken
        where  TGameFrameworkPlatform : GameFrameworkPlatformToken, new()
    {
        public override IEnumerable<Type> GetAutofacModuleTypes()
        {
            var frameworkPlatform = new TGameFrameworkPlatform();
            foreach (var moduleType in base.GetAutofacModuleTypes())
                yield return moduleType;
            foreach (var moduleType in frameworkPlatform.GetAutofacModuleTypes())
                yield return moduleType;
        }

    }
}
