using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableGameTest.Framework.Support
{
    public abstract class GameFrameworkPlatformToken
    {
        public virtual IEnumerable<Type> GetAutofacModuleTypes()
        {
            yield break;
        }

        public abstract string Name { get; }
    }
}
