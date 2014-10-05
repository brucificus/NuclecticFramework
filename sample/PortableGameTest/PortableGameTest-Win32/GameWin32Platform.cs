using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableGameTest.Core;
using PortableGameTest.Framework;

namespace PortableGameTest_Win32
{
    public class GameWin32Platform
        : GamePlatformToken<GameFrameworkWin32Platform>
    {
        public override IEnumerable<Type> GetAutofacModuleTypes()
        {
            foreach (var moduleType in base.GetAutofacModuleTypes())
                yield return moduleType;
        }

        public override string Name
        {
            get { return "Win32"; }
        }
    }
}
