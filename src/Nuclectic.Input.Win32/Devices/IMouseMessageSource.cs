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

namespace Nuclectic.Input.Devices
{
	/// <summary>
	///   Sends out notifications for intercepted window messages related to the mouse
	/// </summary>
	public interface IMouseMessageSource
	{
		/// <summary>Triggered when a mouse button has been pressed</summary>
		event MouseButtonEventDelegate MouseButtonPressed;

		/// <summary>Triggered when a mouse button has been released</summary>
		event MouseButtonEventDelegate MouseButtonReleased;

		/// <summary>Triggered when the mouse has been moved</summary>
		event MouseMoveEventDelegate MouseMoved;

		/// <summary>Triggered when the mouse wheel has been rotated</summary>
		event MouseWheelEventDelegate MouseWheelRotated;
	}
} // namespace Nuclex.Input.Devices