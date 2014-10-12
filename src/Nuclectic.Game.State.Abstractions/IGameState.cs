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

namespace Nuclectic.Game.State
{
	/// <summary>State the game can be in</summary>
	/// <remarks>
	///     <para>
	///         To obtain Draw() and Update() notifications, implement the IDrawable
	///         and IUpdateable interfaces.
	///     </para>
	///     <para>
	///         This class follows the usual game state concept: Instead of using
	///         hard to maintain if/else trees for deciding whether to render the main menu,
	///         game scene or credits scroller, each of the game's phases is put in its own
	///         game state class. This improves modularity and helps keeping parts of code
	///         separated that have nothing to with each other. The game state manager
	///         allows multiple states to be active at the same time and manages these
	///         active states in a stack, which is useful for realizing ingame menus
	///         and nested scenes.
	///     </para>
	///     <para>
	///         Game states can be either active or inactive and will be notified when this
	///         state changes by the OnEntered() and OnLeaving() methods. Any game state
	///         starts out as being inactive. Game states should only load and keep their
	///         resources during their active period and free them again when they
	///         become inactive.
	///     </para>
	///     <para>
	///         In addition to being active and inactive, game states also have a pause mode.
	///         This mode can only be entered by active game states and is used to put
	///         the game state in the back when another game state is pushed on top of it
	///         in the stack. Thus, if multiple game states are active, only the topmost
	///         state can be in unpaused mode.
	///     </para>
	/// </remarks>
	public interface IGameState
	{
		/// <summary>Called when the game state has been entered</summary>
		void Enter();

		/// <summary>Called when the game state is being left again</summary>
		void Leave();

		/// <summary>Called when the game state is being paused</summary>
		void Pause();

		/// <summary>Called when the game state is being resumed from pause mode</summary>
		void Resume();
	}
} // namespace Nuclex.Game.States