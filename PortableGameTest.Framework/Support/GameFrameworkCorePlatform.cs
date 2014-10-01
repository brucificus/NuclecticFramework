using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableGameTest.Framework.Fonts;
using PortableGameTest.Framework.Game;

namespace PortableGameTest.Framework.Support
{
    public abstract class GameFrameworkCorePlatform : GameFrameworkPlatformToken
    {
        public override IEnumerable<Type> GetAutofacModuleTypes()
        {
            yield return typeof (_FontsModule);
            yield return typeof (_GameModule);
            //yield return typeof (_GeometryModule);
            yield return typeof (_GraphicsModule);
            yield return typeof (_InputModule);
            yield return typeof (_UserInterfaceModule);
            yield return typeof (_FrameworkModule);
	        yield return typeof (_WellKnownContentModule);
        }
    }
}
