#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/

#endregion

using Microsoft.Xna.Framework;
using Moq;
using Nuclectic.Game.Component;
#if UNITTEST
using System;
using NUnit.Framework;

namespace Nuclectic.Tests.Game
{

	/// <summary>Unit test for the drawable component class</summary>
	[TestFixture]
	public class DrawableComponentTest
	{

		#region interface IDrawableComponentSubscriber

		/// <summary>Interface for a subscriber to the DrawableComponent's events</summary>
		public interface IDrawableComponentSubscriber
		{

			/// <summary>
			///   Called when the component's drawing order has changed
			/// </summary>
			/// <param name="sender">Component whose drawing order property has changed</param>
			/// <param name="arguments">Not used</param>
			void DrawOrderChanged(object sender, EventArgs arguments);

			/// <summary>
			///   Called when the Component's visible property has changed
			/// </summary>
			/// <param name="sender">Component whose visible property has changed</param>
			/// <param name="arguments">Not used</param>
			void VisibleChanged(object sender, EventArgs arguments);

		}

		#endregion // interface IDrawableComponentSubscriber

		/// <summary>
		///   Verifies that the constructor of the drawable component is working
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			DrawableComponent testComponent = new DrawableComponent();
		}

		/// <summary>Tests whether the Initialize() method is working</summary>
		[Test]
		public void TestInitialize()
		{
			var testComponent = new DrawableComponent();
			testComponent.Initialize();
		}

		/// <summary>
		///   Tests whether the drawable component can draw itself
		/// </summary>
		[Test]
		public void TestDraw()
		{
			var testComponent = new DrawableComponent();
			testComponent.Initialize();
			testComponent.Draw(new GameTime());
		}

		/// <summary>Verifies that the DrawOrder property is working correctly</summary>
		[Test]
		public void TestDrawOrder()
		{
			var testComponent = new DrawableComponent();
			testComponent.DrawOrder = 4321;
			Assert.AreEqual(4321, testComponent.DrawOrder);
		}

		/// <summary>
		///   Verifies that the DrawOrder change event is triggered when the drawing order
		///   of the component is changed
		/// </summary>
		[Test]
		public void TestDrawOrderChangedEvent()
		{
			var testComponent = new DrawableComponent();
			Mock<IDrawableComponentSubscriber> subscriber = mockSubscriber(testComponent);

			subscriber.Setup(s => s.DrawOrderChanged(It.IsAny<object>(), It.IsAny<EventArgs>())).Verifiable();
			testComponent.DrawOrder = 4321;

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that the Visible property is working correctly</summary>
		[Test]
		public void TestVisible()
		{
			var testComponent = new DrawableComponent();
			testComponent.Visible = false;
			Assert.IsFalse(testComponent.Visible);
			testComponent.Visible = true;
			Assert.IsTrue(testComponent.Visible);
		}

		/// <summary>
		///   Verifies that the visible change event is triggered when the visibility
		///   of the component is changed
		/// </summary>
		[Test]
		public void TestVisibleChangedEvent()
		{
			var testComponent = new DrawableComponent();
			testComponent.Visible = false;

			Mock<IDrawableComponentSubscriber> subscriber = mockSubscriber(testComponent);
			subscriber.Setup(s=>s.VisibleChanged(It.IsAny<object>(), It.IsAny<EventArgs>())).Verifiable();
			testComponent.Visible = true;
			
			subscriber.VerifyAll();
		}

		/// <summary>Mocks a subscriber for the events of a tracker</summary>
		/// <param name="drawableComponent">Component to mock an event subscriber for</param>
		/// <returns>The mocked event subscriber</returns>
		private Mock<IDrawableComponentSubscriber> mockSubscriber(
		  DrawableComponent drawableComponent
		)
		{
			Mock<IDrawableComponentSubscriber> mockedSubscriber =
			  new Mock<IDrawableComponentSubscriber>();

			drawableComponent.DrawOrderChanged += new EventHandler<EventArgs>(
			  mockedSubscriber.Object.DrawOrderChanged
			);
			drawableComponent.VisibleChanged += new EventHandler<EventArgs>(
			  mockedSubscriber.Object.VisibleChanged
			);

			return mockedSubscriber;
		}
	}

} // namespace Nuclex.Game

#endif // UNITTEST
