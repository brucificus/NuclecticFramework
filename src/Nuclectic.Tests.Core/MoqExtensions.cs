using System.Reflection;
using Moq.Language;
using Moq.Language.Flow;

namespace Nuclectic.Tests
{
	public static class MoqExtensions
	{
		public delegate void OutAction<TOut>(out TOut outVal);

		public delegate void OutAction<in T1, TOut>(T1 arg1, out TOut outVal);

		public static IReturnsThrows<TMock, TReturn> OutCallback<TMock, TReturn, TOut>(this ICallback<TMock, TReturn> mock, OutAction<TOut> action)
			where TMock : class
		{
			return OutCallbackInternal(mock, action);
		}

		public static IReturnsThrows<TMock, TReturn> OutCallback<TMock, TReturn, T1, TOut>(this ICallback<TMock, TReturn> mock, OutAction<T1, TOut> action)
			where TMock : class
		{
			return OutCallbackInternal(mock, action);
		}

		private static IReturnsThrows<TMock, TReturn> OutCallbackInternal<TMock, TReturn>(ICallback<TMock, TReturn> mock, object action)
			where TMock : class
		{
			mock.GetType()
				.Assembly.GetType("Moq.MethodCall")
				.InvokeMember(
							  "SetCallbackWithArguments", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, mock,
							  new[] {action});
			return mock as IReturnsThrows<TMock, TReturn>;
		}

		public static ICallbackResult OutCallback<TOut>(this ICallback mock, OutAction<TOut> action) { return OutCallbackInternal(mock, action); }

		public static ICallbackResult OutCallback<T1, TOut>(this ICallback mock, OutAction<T1, TOut> action) { return OutCallbackInternal(mock, action); }

		private static ICallbackResult OutCallbackInternal(ICallback mock, object action)
		{
			mock.GetType().Assembly.GetType("Moq.MethodCall")
				.InvokeMember("SetCallbackWithArguments", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, mock, new[] {action});
			return (ICallbackResult)mock;
		}
	}
}