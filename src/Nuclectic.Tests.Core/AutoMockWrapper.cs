using System;
using System.Diagnostics;
using Autofac;
using Moq;

namespace Nuclectic.Tests
{
	[DebuggerStepThrough, DebuggerNonUserCode]
	public class AutoMockWrapper : IDisposable
	{
		private readonly AutoMock.AutoMock _AutoMock;
		private bool? _VerifyAll;

		public static AutoMockWrapper CreateLoose()
		{
			return new AutoMockWrapper(AutoMock.AutoMock.GetLoose());
		}

		public static AutoMockWrapper CreateStrict()
		{
			return new AutoMockWrapper(AutoMock.AutoMock.GetStrict());
		}

		private AutoMockWrapper(AutoMock.AutoMock autoMock)
		{
			_AutoMock = autoMock;
		}

		public T Create<T>()
		{
			return _AutoMock.Create<T>();
		}

		public T1 Create<T1, T2>(T2 explicitParameter)
		{
			return _AutoMock.Create<T1>(new TypedParameter(typeof(T2), explicitParameter));
		}

		public Mock<T> Mock<T>()
			where T : class
		{
			return _AutoMock.Mock<T>();
		}

		public bool VerifyAll { get { return _VerifyAll.Value; } set { _VerifyAll = value; } }

		public void Dispose()
		{
			_AutoMock.VerifyAll = _VerifyAll.GetValueOrDefault(true);
			_AutoMock.Dispose();
		}
	}
}