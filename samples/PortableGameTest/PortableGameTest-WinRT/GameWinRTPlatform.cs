using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableGameTest.Core;
using PortableGameTest.Framework;

namespace PortableGameTest_WinRT
{
    public class GameWinRTPlatform
        : GamePlatformToken<GameFrameworkWinRTPlatform>
    {
        public override IEnumerable<Type> GetAutofacModuleTypes()
        {
            foreach (var moduleType in base.GetAutofacModuleTypes())
                yield return moduleType;
        }

        public override string Name
        {
            get { return "WinRT"; }
        }
    }
}
