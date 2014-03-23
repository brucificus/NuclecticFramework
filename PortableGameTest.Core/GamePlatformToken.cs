using System;
using System.Collections.Generic;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core
{
    public abstract class GamePlatformToken
    {
        public virtual IEnumerable<Type> GetAutofacModuleTypes()
        {
            yield break;
        }

        public abstract string Name { get; }
    }

    public abstract class GamePlatformToken<TGameFrameworkPlatform>
        : GamePlatformToken
        where  TGameFrameworkPlatform : GameFrameworkPlatformToken, new()
    {
        public virtual IEnumerable<Type> GetAutofacModuleTypes()
        {
            var frameworkPlatform = new TGameFrameworkPlatform();
            foreach (var moduleType in frameworkPlatform.GetAutofacModuleTypes())
                yield return moduleType;
        }

    }
}
