using System;

namespace Nuclectic.Support
{
	public class OnDispose : IDisposable
	{
		private readonly Action _onDispose;

		public OnDispose(Action onDispose) { _onDispose = onDispose; }

		public void Dispose() { _onDispose(); }
	}
}