// This software is part of the Autofac IoC container
// Copyright (c) 2007 - 2008 Autofac Contributors
// http://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Moq;

namespace Nuclectic.Tests.AutoMock
{
	/// <summary> Resolves unknown interfaces and Mocks using the <see cref="MockRepository"/> from the scope. </summary>
	[DebuggerStepThrough, DebuggerNonUserCode]
	internal class MoqRegistrationHandler : IRegistrationSource
	{
		private readonly MethodInfo _createMethod;

		/// <summary>
		/// </summary>
		[SecurityCritical]
		public MoqRegistrationHandler()
		{
			var factoryType = typeof (MockRepository);
			_createMethod = factoryType.GetMethod("Create", new Type[] {});
		}

		/// <summary>
		/// Retrieve a registration for an unregistered service, to be used
		/// by the container.
		/// </summary>
		/// <param name="service">The service that was requested.</param>
		/// <param name="registrationAccessor"></param>
		/// <returns>
		/// Registrations for the service.
		/// </returns>
		[SecuritySafeCritical]
		public IEnumerable<IComponentRegistration> RegistrationsFor
			(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
		{
			if (service == null)
				throw new ArgumentNullException("service");

			var typedService = service as TypedService;
			if (typedService == null)
				yield break;

			bool isCompatibleMockableType = IsCompatibleMockableType(typedService.ServiceType);
			bool isCompatibleMockType = IsCompatibleMockType(typedService.ServiceType);

			if (isCompatibleMockType || isCompatibleMockableType)
			{
				Type mockType;
				Type serviceType;

				if (isCompatibleMockType)
				{
					mockType = typedService.ServiceType;
					serviceType = typedService.ServiceType.GetGenericArguments().Single();
				}
				else
				{
					mockType = typeof (Mock<>).MakeGenericType(typedService.ServiceType);
					serviceType = typedService.ServiceType;
				}

				yield return RegistrationBuilder
					.ForDelegate(mockType, (c, p) => CreateMock(c, serviceType))
					.As(mockType)
					.InstancePerLifetimeScope()
					.CreateRegistration();

				yield return RegistrationBuilder
					.ForDelegate((c, p) => ((Mock)c.Resolve(mockType)).Object)
					.As(serviceType)
					.InstancePerLifetimeScope()
					.CreateRegistration();
			}
		}

		private static bool IsCompatibleMockType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Mock<>) && IsCompatibleMockableType(type.GetGenericArguments().Single());
		}

		private static bool IsCompatibleMockableType(Type type)
		{
			return !((type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>))
					 || type.IsArray
					 || typeof (IStartable).IsAssignableFrom(type))
				   && (type.IsInterface || type.IsAbstract)
				   && !typeof (Mock).IsAssignableFrom(type)
				;
		}

		public bool IsAdapterForIndividualComponents { get { return false; } }

		/// <summary>
		/// Creates a mock object.
		/// </summary>
		/// <param name="context">The component context.</param>
		/// <param name="typedService">The typed service.</param>
		/// <returns></returns>
		[SecuritySafeCritical]
		private Mock CreateMock(IComponentContext context, Type serviceType)
		{
			var specificCreateMethod =
				_createMethod.MakeGenericMethod(new[] {serviceType});
			var mock = (Mock)specificCreateMethod.Invoke(context.Resolve<MockRepository>(), null);
			return mock;
		}
	}
}