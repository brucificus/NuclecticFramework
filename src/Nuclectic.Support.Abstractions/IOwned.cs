using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuclectic.Support
{
	/// <summary>
	///     Represents an object whose disposal logic is not tied directly to the object itself.
	/// </summary>
	/// <typeparam name="T">The type of value exposed.</typeparam>
	/// <remarks>An abstract version of Autofac's http://autofac.org/apidoc/html/4EE77638.htm</remarks>
	public interface IOwned<out T>
		: IDisposable
	{
		/// <summary>
		///     The owned value.
		/// </summary>
		T Value { get; }
	}
}