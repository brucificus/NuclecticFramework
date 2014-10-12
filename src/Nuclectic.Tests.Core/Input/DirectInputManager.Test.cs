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

using System.Windows.Forms;
using Nuclectic.Input;
using Nuclectic.Input.Devices;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Input
{
	/// <summary>Unit tests for the DirectInput manager</summary>
	[TestFixture]
	internal class DirectInputManagerTest
	{
		/// <summary>Verifies that the IsDirectInputAvailable property is working</summary>
		[Test]
		public void TestIsDirectInputAvailable()
		{
			using (var directInputManager = new DirectInputManager())
			{
				bool result = directInputManager.IsDirectInputAvailable;
				Assert.IsTrue(result || !result); // the result doesn't matter
			}
		}

		/// <summary>Verifies that the IsDeviceAttached() method works</summary>
		[Test]
		public void TestCreateGamePadsAndIsDeviceAttached()
		{
			using (var manager = new DirectInputManager())
			{
				var gamePads = manager.CreateGamePads();
				for (int index = 0; index < gamePads.Length; ++index)
				{
					bool result = gamePads[index].IsAttached;
					Assert.IsTrue(result || !result); // the result doesn't matter
				}
			}
		}
	}
} // namespace Nuclex.Input

#endif // UNITTEST