using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Support;

namespace Nuclectic.Tests.Mocks
{
	public class GlobalExclusiveMockedGraphicsDeviceService : IMockedGraphicsDeviceService, IDisposable
	{
		private const string AlreadyDisposedMessage = "The GlobalExclusiveMockedGraphicsDeviceService instance has already been disposed.";
		private static readonly object GlobalLock = new object();

		private IDisposable _Disposable;
		private volatile bool _DisposingOrDisposed;

		private readonly Lazy<IMockedGraphicsDeviceService> _LazyGraphicsDeviceService;

		public GlobalExclusiveMockedGraphicsDeviceService(Func<IMockedGraphicsDeviceService> graphicsDeviceServiceFactory)
		{
			_LazyGraphicsDeviceService = CreateLazyGraphicsDeviceService(graphicsDeviceServiceFactory);
		}

		private Lazy<IMockedGraphicsDeviceService> CreateLazyGraphicsDeviceService(Func<IMockedGraphicsDeviceService> graphicsDeviceServiceFactory)
		{
			return new Lazy<IMockedGraphicsDeviceService>(
				() =>
				{
					GuardAgainstDisposingOrDisposed();
					System.Threading.Monitor.Enter(GlobalLock);
					var graphicsDeviceService = graphicsDeviceServiceFactory();
					_Disposable = new OnDispose(
						() =>
						{
							try
							{
								var disposableGraphicsDeviceService = graphicsDeviceService as IDisposable;
								if (disposableGraphicsDeviceService != null)
									disposableGraphicsDeviceService.Dispose();
							}
							finally
							{
								System.Threading.Monitor.Exit(GlobalLock);
							}
						});
					return graphicsDeviceService;
				});
		}

		GraphicsDevice IGraphicsDeviceService.GraphicsDevice
		{
			get
			{
				GuardAgainstDisposingOrDisposed();
				return _LazyGraphicsDeviceService.Value.GraphicsDevice;
			}
		}

		event EventHandler<EventArgs> IGraphicsDeviceService.DeviceCreated
		{
			add
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceCreated += value;
			}
			remove
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceCreated -= value;
			}
		}

		event EventHandler<EventArgs> IGraphicsDeviceService.DeviceDisposing
		{
			add
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceDisposing += value;
			}
			remove
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceDisposing -= value;
			}
		}

		event EventHandler<EventArgs> IGraphicsDeviceService.DeviceReset
		{
			add
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceReset += value;
			}
			remove
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceReset -= value;
			}
		}

		event EventHandler<EventArgs> IGraphicsDeviceService.DeviceResetting
		{
			add
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceResetting += value;
			}
			remove
			{
				GuardAgainstDisposingOrDisposed();
				_LazyGraphicsDeviceService.Value.DeviceResetting -= value;
			}
		}

		public void Dispose()
		{
			_DisposingOrDisposed = true;
			if (_Disposable != null)
				_Disposable.Dispose();
		}

		public IDisposable CreateDevice()
		{
			GuardAgainstDisposingOrDisposed();
			return _LazyGraphicsDeviceService.Value.CreateDevice();
		}

		public void DestroyDevice()
		{
			GuardAgainstDisposingOrDisposed();
			_LazyGraphicsDeviceService.Value.DestroyDevice();
		}

		public void ResetDevice()
		{
			GuardAgainstDisposingOrDisposed();
			_LazyGraphicsDeviceService.Value.ResetDevice();
		}

		private void GuardAgainstDisposingOrDisposed()
		{
			if (_DisposingOrDisposed)
				throw new ObjectDisposedException(AlreadyDisposedMessage);
		}
	}
}