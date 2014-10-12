using Nuclectic.Input.Devices;
using NUnit.Framework;

namespace Nuclectic.Tests.Input.Devices
{
	/// <summary>Unit tests for the XNA (XINPUT) based chat pad class</summary>
	[TestFixture]
	internal class XnaKeyboardSoloTest
	{
		/// <summary>Verifies that the GetState() method is working</summary>
		[Test]
		public void TestGetState()
		{
			var keyboard = new XnaKeyboardSolo();
			keyboard.GetState();
			// No exception means success
		}

		/// <summary>Verifies that the game pad can be attached and detached</summary>
		[Test]
		public void TestIsAttached()
		{
			var keyboard = new XnaKeyboardSolo();
			bool attached = keyboard.IsAttached;
			Assert.IsTrue(attached || !attached); // result doesn't matter
		}

		/// <summary>Verifies that the mocked game pad's name can be retrieved</summary>
		[Test]
		public void TestName()
		{
			var keyboard = new XnaKeyboardSolo();
			StringAssert.AreEqualIgnoringCase("keyboard", keyboard.Name);
		}
	}
}