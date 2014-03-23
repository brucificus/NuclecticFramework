using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableGameTest.Framework.Input;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Framework
{
    public class GameFrameworkWin32Platform : GameFrameworkCorePlatform
    {
        public override IEnumerable<Type> GetAutofacModuleTypes()
        {
            foreach (var moduleType in base.GetAutofacModuleTypes())
                yield return moduleType;

            yield return typeof(_InputModuleForWin32);
        }

        public override string Name
        {
            get { return "Win32"; }
        }
    }
}
