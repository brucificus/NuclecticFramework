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
using Nuclectic.Input;
using Nuclectic.Input.Devices;
#if UNITTEST
using System;
using NUnit.Framework;

namespace Nuclectic.Tests.Input
{

	/// <summary>Unit tests for the input manager</summary>
	[TestFixture]
	public class InputManagerTest
	{

		#region interface IUpdateableSubscriber

		/// <summary>Subscriber to an updateable object</summary>
		public interface IUpdateableSubscriber
		{

			/// <summary>Called when the updateable's update order has been changed</summary>
			/// <param name="sender">Updateable who's update order changed</param>
			/// <param name="arguments">Not used</param>
			void UpdateOrderChanged(object sender, EventArgs arguments);

			/// <summary>Called when the updateable is enabled or disabled</summary>
			/// <param name="sender">Updateable that has been enabled or disabled</param>
			/// <param name="arguments">Not used</param>
			void EnabledChanged(object sender, EventArgs arguments);

		}

		#endregion // interface IUpdateableSubscriber

		/// <summary>Ensures that the default constructor is working</summary>
		[Test]
		public void TestDefaultConstructor()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager); // nonsense, avoids compiler warning
			}
		}

		/// <summary>Ensures that the service container constructor is working</summary>
		[Test]
		public void TestServiceConstructor()
		{
			var services = new GameServiceContainer();

			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(services.GetService(typeof(IInputService)));
			}

			Assert.IsNull(services.GetService(typeof(IInputService)));
		}

		/// <summary>Verifies that the keyboards collection isn't empty</summary>
		[Test]
		public void TestKeyboardsCollection()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.Greater(manager.Keyboards.Count, 0);
			}
		}

		/// <summary>Verifies that the mice collection isn't empty</summary>
		[Test]
		public void TestMiceCollection()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.Greater(manager.Mice.Count, 0);
			}
		}

		/// <summary>Verifies that the game pads collection isn't empty</summary>
		[Test]
		public void TestGamePadsCollection()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.Greater(manager.GamePads.Count, 0);
			}
		}

		/// <summary>Verifies that the touch panels collection isn't empty</summary>
		[Test]
		public void TestTouchPanelsCollection()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.Greater(manager.TouchPanels.Count, 0);
			}
		}

		/// <summary>Verifies that a mouse can be retrieved</summary>
		[Test]
		public void TestGetMouse()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager.GetMouse());
			}
		}

		/// <summary>Verifies that the main keyboard can be retrieved</summary>
		[Test]
		public void TestGetKeyboard()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager.GetKeyboard());
			}
		}

		/// <summary>Verifies that a chat pad can be retrieved</summary>
		[Test]
		public void TestGetChatPad()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager.GetKeyboard(PlayerIndex.One));
			}
		}

		/// <summary>Verifies that an XINPUT game pad can be retrieved</summary>
		[Test]
		public void TestGetXinputGamePad()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager.GetGamePad(PlayerIndex.One));
			}
		}

		/// <summary>Verifies that a DirectInput game pad can be retrieved</summary>
		[Test]
		public void TestGetDirectInputGamePad()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager.GetGamePad(ExtendedPlayerIndex.Five));
			}
		}

		/// <summary>Verifies that a touch panel can be retrieved</summary>
		[Test]
		public void TestGetTouchPanel()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsNotNull(manager.GetTouchPanel());
			}
		}

		/// <summary>Verifies that the snapshot system is working</summary>
		[Test]
		public void TestSnapshots()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.AreEqual(0, manager.SnapshotCount);
				manager.TakeSnapshot();
				Assert.AreEqual(1, manager.SnapshotCount);
				manager.Update();
				Assert.AreEqual(0, manager.SnapshotCount);
			}
		}

		/// <summary>Verifies that the UpdateOrder property behaves correctly</summary>
		[Test]
		public void TestChangeUpdateOrder()
		{
			using (var manager = CreateDummyInputManager())
			{
				Mock<IUpdateableSubscriber> updateable = new Mock<IUpdateableSubscriber>();
				manager.EnabledChanged += updateable.Object.EnabledChanged;
				manager.UpdateOrderChanged += updateable.Object.UpdateOrderChanged;

				updateable.Setup(x => x.UpdateOrderChanged(It.IsAny<object>(), It.IsAny<EventArgs>())).Verifiable();
				manager.UpdateOrder = 123;
				Assert.AreEqual(123, manager.UpdateOrder);

				manager.UpdateOrderChanged -= updateable.Object.UpdateOrderChanged;
				manager.EnabledChanged -= updateable.Object.EnabledChanged;

				updateable.VerifyAll();
			}
		}

		/// <summary>
		///   Verifies that input manager implements the IGameComponent interface
		/// </summary>
		[Test]
		public void TestInitializeGameComponent()
		{
			using (var manager = CreateDummyInputManager())
			{
				((IGameComponent)manager).Initialize();
			}
		}

		/// <summary>
		///   Verifies that input manager provides an enabled property
		/// </summary>
		[Test]
		public void TestEnabledProperty()
		{
			using (var manager = CreateDummyInputManager())
			{
				Assert.IsTrue(((IUpdateable)manager).Enabled);
			}
		}

		/// <summary>
		///   Verifies that the input manager can be updated via the IUpdateable interface
		/// </summary>
		[Test]
		public void TestUpdateViaIUpdateable()
		{
			using(var manager = CreateDummyInputManager())
			{
				((IUpdateable)manager).Update(new GameTime());
			}
		}

		private static InputManager CreateDummyInputManager()
		{
			return new InputManager(new NoDirectInputManager(), new NoKeyboardSoloFactory(), new NoMouseFactory());
		}
	}

} // namespace Nuclex.Input

#endif // UNITTEST
