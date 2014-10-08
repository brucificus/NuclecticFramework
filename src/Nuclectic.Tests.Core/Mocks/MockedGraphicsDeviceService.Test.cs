//#region CPL License
///*
//Nuclex Framework
//Copyright (C) 2002-2009 Nuclex Development Labs

//This library is free software; you can redistribute it and/or
//modify it under the terms of the IBM Common Public License as
//published by the IBM Corporation; either version 1.0 of the
//License, or (at your option) any later version.

//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//IBM Common Public License for more details.

//You should have received a copy of the IBM Common Public
//License along with this library
//*/
//#endregion

//using Moq;
//#if UNITTEST
//using System;
//using Microsoft.Xna.Framework.Graphics;
//using NUnit.Framework;

//namespace Nuclectic.Tests.Mocks {

//  /// <summary>Unit tests for the graphics device mock test helper</summary>
//  [TestFixture]
//  public class MockedGraphicsDeviceServiceTest {

//#region interface IGraphicsDeviceServiceSubscriber

//	/// <summary>Subscriber for the event of the graphics device service</summary>
//	public interface IGraphicsDeviceServiceSubscriber {
//	  /// <summary>Called when a graphics device has been created</summary>
//	  /// <param name="sender">
//	  ///   Graphics device service that created a graphics device
//	  /// </param>
//	  /// <param name="arguments">Not used</param>
//	  void DeviceCreated(object sender, EventArgs arguments);
//	  /// <summary>Called when a graphics device is about to be destroyed</summary>
//	  /// <param name="sender">
//	  ///   Graphics device service that is about to destroy its graphics device
//	  /// </param>
//	  /// <param name="arguments">Not used</param>
//	  void DeviceDisposing(object sender, EventArgs arguments);
//	  /// <summary>Called when the graphics device is about to reset itself</summary>
//	  /// <param name="sender">
//	  ///   Graphics device service whose graphics device is about to reset itself
//	  /// </param>
//	  /// <param name="arguments">Not used</param>
//	  void DeviceResetting(object sender, EventArgs arguments);
//	  /// <summary>Called when the graphics device has completed a reset</summary>
//	  /// <param name="sender">
//	  ///   Graphics device service whose graphics device has completed a reset
//	  /// </param>
//	  /// <param name="arguments">Not used</param>
//	  void DeviceReset(object sender, EventArgs arguments);
//	}

//#endregion // interface IGraphicsDeviceSubscriber

//	/// <summary>Initialization routine executed before each test is run</summary>
//	[SetUp]
//	public void Setup() {
//	  this.mockery = new MockFactory();
//	}

//	/// <summary>Tests whether the mock's service provider is set up correctly</summary>
//	[Test]
//	public void TestServiceProvider() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService();

//	  IServiceProvider serviceProvider = mock.ServiceProvider;
//	  Assert.IsNotNull(serviceProvider);

//	  IGraphicsDeviceService service = (IGraphicsDeviceService)serviceProvider.GetService(
//		typeof(IGraphicsDeviceService)
//	  );
//	  Assert.AreSame(mock, service);
//	}

//	/// <summary>Tests whether a graphics device can be created</summary>
//	[Test]
//	public void TestGraphicsDeviceCreation() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService();

//	  using(IDisposable keeper = mock.CreateDevice()) {
//		Assert.IsNotNull(mock.GraphicsDevice);
//	  }
//	}

//	/// <summary>
//	///   Verifies that the graphics device is destroyed when the keeper returned
//	///   by the CreateDevice() method gets disposed explicitely.
//	/// </summary>
//	[Test]
//	public void TestAutomaticGraphicsDeviceDestruction() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService();

//	  try {
//		using(IDisposable keeper = mock.CreateDevice()) {
//		  Assert.IsNotNull(mock.GraphicsDevice);
//		  throw new ArithmeticException("Test exception");
//		}
//	  }
//	  catch(ArithmeticException) {
//		// Munch
//	  }

//	  Assert.IsNull(mock.GraphicsDevice);
//	}

//	/// <summary>
//	///   Verifies that the mocked graphics device service fires its events
//	/// </summary>
//	[Test]
//	public void TestGraphicsDeviceServiceEvents() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService();
//	  Mock<IGraphicsDeviceServiceSubscriber> mockedSubscriber = mockSubscriber(mock);

//	  mockedSubscriber.Expects.One.Method(
//		m => m.DeviceCreated(null, null)
//	  ).WithAnyArguments();

//	  using(IDisposable keeper = mock.CreateDevice()) {
//		this.mockery.VerifyAllExpectationsHaveBeenMet();

//		mockedSubscriber.Expects.One.Method(
//		  m => m.DeviceResetting(null, null)
//		).WithAnyArguments();
//		mockedSubscriber.Expects.One.Method(
//		  m => m.DeviceReset(null, null)
//		).WithAnyArguments();

//		mock.ResetDevice();
//		this.mockery.VerifyAllExpectationsHaveBeenMet();

//		mockedSubscriber.Expects.One.Method(
//		  m => m.DeviceDisposing(null, null)
//		).WithAnyArguments();
//	  }
//	  this.mockery.VerifyAllExpectationsHaveBeenMet();
//	}

//	/// <summary>
//	///   Tests whether the graphics device can be destroyed manually even
//	///   though it the RAII helper is used without causing an exception
//	/// </summary>
//	[Test]
//	public void TestRedundantDestroyInvocation() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService();

//	  using(IDisposable keeper = mock.CreateDevice()) {
//		mock.DestroyDevice();
//	  } // should not cause an exception
//	}

//	/// <summary>
//	///   Verifies that the mocked graphics device service cleans up the graphics
//	///   device and all of its resources again when an exception occurs during
//	///   its creation
//	/// </summary>
//	[Test]
//	public void TestExceptionDuringDeviceCreation() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService();

//	  Mock<IGraphicsDeviceServiceSubscriber> mockedSubscriber = mockSubscriber(mock);

//	  mockedSubscriber.Expects.One.Method(
//		m => m.DeviceCreated(null, null)
//	  ).WithAnyArguments();

//	  mock.DeviceCreated += (EventHandler<EventArgs>)delegate(object sender, EventArgs arguments) {
//		Assert.IsNotNull(mock.GraphicsDevice);
//		throw new ArithmeticException("Test exception");
//	  };
//	  try {
//		mock.CreateDevice();
//	  }
//	  catch(ArithmeticException) {
//		// Munch
//	  }

//	  Assert.IsNull(mock.GraphicsDevice);
//	}

//	/// <summary>
//	///   Verifies that the mocked graphics device service can cope with
//	///   a NotSupportedException when the reference rasterizer is selected
//	/// </summary>
//	[Test]
//	public void TestNotSupportedExceptionForReferenceRasterizer() {
//	  MockedGraphicsDeviceService mock = new MockedGraphicsDeviceService(
//		DeviceType.Reference
//	  );
//	  mock.DeviceCreated += delegate(object sender, EventArgs arguments) {
//		throw new InvalidOperationException("Simulated error for unit testing");
//	  };

//	  Console.Error.WriteLine(
//		"The next line should contain an error message indicating that the reference " +
//		"rasterizer could not be created"
//	  );
//	  Assert.Throws<InvalidOperationException>(
//		delegate() {
//		  mock.CreateDevice();
//		  mock.DestroyDevice();
//		}
//	  );
//	}

//	/// <summary>
//	///   Mocks a subscriber for the events of the mocked graphics device service
//	/// </summary>
//	/// <returns>The mocked event subscriber</returns>
//	private Mock<IGraphicsDeviceServiceSubscriber> mockSubscriber(
//	  IGraphicsDeviceService graphicsDeviceService
//	) {
//	  Mock<IGraphicsDeviceServiceSubscriber> mockedSubscriber =
//		this.mockery.CreateMock<IGraphicsDeviceServiceSubscriber>();

//	  graphicsDeviceService.DeviceCreated += new EventHandler<EventArgs>(
//		mockedSubscriber.MockObject.DeviceCreated
//	  );
//	  graphicsDeviceService.DeviceResetting += new EventHandler<EventArgs>(
//		mockedSubscriber.MockObject.DeviceResetting
//	  );
//	  graphicsDeviceService.DeviceReset += new EventHandler<EventArgs>(
//		mockedSubscriber.MockObject.DeviceReset
//	  );
//	  graphicsDeviceService.DeviceDisposing += new EventHandler<EventArgs>(
//		mockedSubscriber.MockObject.DeviceDisposing
//	  );

//	  return mockedSubscriber;
//	}

//	/// <summary>Mock object factory</summary>
//	private MockFactory mockery;

//  }

//} // namespace Nuclex.Testing.Xna

//#endif // UNITTEST
