using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableGameTest.Framework.Fonts;
using PortableGameTest.Framework.Game;
using PortableGameTest.Framework.Geometry;
using PortableGameTest.Framework.Graphics;
using PortableGameTest.Framework.UserInterface;

namespace PortableGameTest.Framework.Support
{
    public abstract class GameFrameworkCorePlatform : GameFrameworkPlatformToken
    {
        public override IEnumerable<Type> GetAutofacModuleTypes()
        {
            yield return typeof (_FontsModule);
            yield return typeof (_GameModule);
            yield return typeof (_GeometryModule);
            yield return typeof (_GraphicsModule);
            yield return typeof (_UserInterfaceModule);
        }
    }
}
