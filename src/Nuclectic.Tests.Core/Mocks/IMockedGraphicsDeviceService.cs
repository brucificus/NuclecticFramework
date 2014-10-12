using System;
using Microsoft.Xna.Framework.Graphics;

namespace Nuclectic.Tests.Mocks
{
	public interface IMockedGraphicsDeviceService : IGraphicsDeviceService, IDisposable
	{
		/// <summary>Creates a new graphics device</summary>
		/// <returns>
		///     An object implementing IDisposable that will destroy the graphics device
		///     again as soon as its Dispose() method is called.
		/// </returns>
		/// <remarks>
		///     <para>
		///         Make sure to call DestroyGraphicsDevice() either manually,
		///         or by disposing the returned object. A typical usage of this method is
		///         shown in the following code.
		///     </para>
		///     <example>
		///         <code>
		///       using(IDisposable keeper = CreateDevice()) {
		///         GraphicsDevice.DoSomethingThatCouldFail();
		///       }
		///     </code>
		///     </example>
		/// </remarks>
		IDisposable CreateDevice();

		/// <summary>Destroys the created graphics device again</summary>
		void DestroyDevice();

		/// <summary>Performs a graphics device reset</summary>
		void ResetDevice();
	}
}