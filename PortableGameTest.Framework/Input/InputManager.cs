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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nuclex.Input.Devices;
using PortableGameTest.Framework.Input;
using XnaMouse = Microsoft.Xna.Framework.Input.Mouse;
using XnaEventHandler = System.EventHandler<System.EventArgs>;

namespace Nuclex.Input {

  /// <summary>Manages and polls input devices</summary>
  public class InputManager :
    IInputService, IGameComponent, IUpdateable, IDisposable {

    /// <summary>Fired when the UpdateOrder property changes its  value</summary>
    public event XnaEventHandler UpdateOrderChanged;

    /// <summary>Fired when the Enabled property changes its value</summary>
    public event XnaEventHandler EnabledChanged { add { } remove { } }

    /// <summary>Initializs a new input manager</summary>
    /// <param name="services">Game service container the manager registers to</param>
    public InputManager(IDirectInputManager directInputManager, GameServiceContainer services = null)
    {
#if !NO_WININPUT
      this.windowMessageFilter = new WindowMessageFilter(windowHandle);
#endif

      if (directInputManager.IsDirectInputAvailable) {
        this.directInputManager = directInputManager;
      }

      setupGamePads();
      setupMouse();
      setupKeyboards();
      setupTouchPanels();

      if (services != null) {
        this.gameServices = services;
        this.gameServices.AddService(typeof(IInputService), this);
      }
    }

    /// <summary>Immediately releases all resources owned by the instance</summary>
    public void Dispose() {
      if (this.gameServices != null) {
        object inputService = this.gameServices.GetService(typeof(IInputService));
        if (ReferenceEquals(inputService, this)) {
          this.gameServices.RemoveService(typeof(IInputService));
        }

        this.gameServices = null;
      }

      if (this.touchPanels != null) {
        CollectionHelper.DisposeItems(this.touchPanels);
        this.touchPanels = null;
      }
      if (this.keyboards != null) {
        CollectionHelper.DisposeItems(this.keyboards);
        this.keyboards = null;
      }
      if (this.mice != null) {
        CollectionHelper.DisposeItems(this.mice);
        this.mice = null;
      }
      if (this.gamePads != null) {
        CollectionHelper.DisposeItems(this.gamePads);
        this.gamePads = null;
      }

      if (this.directInputManager != null) {
        //this.directInputManager.Dispose();
        this.directInputManager = null;
      }

#if !NO_WININPUT
      if (this.windowMessageFilter != null) {
        this.windowMessageFilter.Dispose();
        this.windowMessageFilter = null;
      }
#endif
    }

    /// <summary>All keyboards known to the system</summary>
    public ReadOnlyCollection<IKeyboard> Keyboards {
      get { return this.keyboards; }
    }

    /// <summary>All mice known to the system</summary>
    public ReadOnlyCollection<IMouse> Mice {
      get { return this.mice; }
    }

    /// <summary>All game pads known to the system</summary>
    public ReadOnlyCollection<IGamePad> GamePads {
      get { return this.gamePads; }
    }

    /// <summary>All touch panels known to the system</summary>
    public ReadOnlyCollection<ITouchPanel> TouchPanels {
      get { return this.touchPanels; }
    }

    /// <summary>Returns the primary mouse input device</summary>
    /// <returns>The primary mouse</returns>
    public IMouse GetMouse() {
      return CollectionHelper.GetIfExists(this.mice, 0);
    }

    /// <summary>Returns the keyboard on a PC</summary>
    /// <returns>The keyboard</returns>
    public IKeyboard GetKeyboard() {
      return CollectionHelper.GetIfExists(this.keyboards, 4);
    }

    /// <summary>Returns the chat pad for the specified player</summary>
    /// <param name="playerIndex">Player whose chat pad will be returned</param>
    /// <returns>The chat pad of the specified player</returns>
    public IKeyboard GetKeyboard(PlayerIndex playerIndex) {
      return this.keyboards[(int)playerIndex];
    }

    /// <summary>Returns the game pad for the specified player</summary>
    /// <param name="playerIndex">Player whose game pad will be returned</param>
    /// <returns>The game pad of the specified player</returns>
    /// <remarks>
    ///   This will only return the XINPUT devices (aka XBox 360 controllers)
    ///   attached. Any standard game pads attached to a PC can only be
    ///   returned through the ExtendedPlayerIndex overload where they will
    ///   take the places of game pads for player 5 and upwards.
    /// </remarks>
    public IGamePad GetGamePad(PlayerIndex playerIndex) {
      return this.gamePads[(int)playerIndex];
    }

    /// <summary>Returns the game pad for the specified player</summary>
    /// <param name="playerIndex">Player whose game pad will be returned</param>
    /// <returns>The game pad of the specified player</returns>
    public IGamePad GetGamePad(ExtendedPlayerIndex playerIndex) {
      return this.gamePads[(int)playerIndex];
    }

    /// <summary>Returns the touch panel on the system</summary>
    /// <returns>The system's touch panel</returns>
    public ITouchPanel GetTouchPanel() {
      return this.touchPanels[0];
    }

    /// <summary>Updates the state of all input devices</summary>
    /// <remarks>
    ///   <para>
    ///     If this method is called with no snapshots in the queue, it will
    ///     query the state of all input devices immediately, raising events
    ///     for any changed states. This way, you can ignore the entire
    ///     snapshot system if you just want basic input device access.
    ///   </para>
    ///   <para>
    ///     If this method is called while one or more snapshots are waiting in
    ///     the queue, this method takes the next snapshot from the queue and makes
    ///     it the current state of all active devices.
    ///   </para>
    /// </remarks>
    public void Update() {
      if (this.snapshotCount > 0) {
        --this.snapshotCount;
      }

      for (int index = 0; index < this.gamePads.Count; ++index) {
        this.gamePads[index].Update();
      }
      for (int index = 0; index < this.mice.Count; ++index) {
        this.mice[index].Update();
      }
      for (int index = 0; index < this.keyboards.Count; ++index) {
        this.keyboards[index].Update();
      }
    }

    /// <summary>Takes a snapshot of the current state of all input devices</summary>
    /// <remarks>
    ///   This snapshot will be queued until the user calls the Update() method,
    ///   where the next polled snapshot will be taken from the queue and provided
    ///   to the user.
    /// </remarks>
    public void TakeSnapshot() {
      for (int index = 0; index < this.gamePads.Count; ++index) {
        this.gamePads[index].TakeSnapshot();
      }
      for (int index = 0; index < this.mice.Count; ++index) {
        this.mice[index].TakeSnapshot();
      }
      for (int index = 0; index < this.keyboards.Count; ++index) {
        this.keyboards[index].TakeSnapshot();
      }

      ++this.snapshotCount;
    }

    /// <summary>Number of snapshots currently in the queue</summary>
    public int SnapshotCount {
      get { return this.snapshotCount; }
    }

    /// <summary>
    ///   Indicates when the game component should be updated relative to other game
    ///   components. Lower values are updated first.
    /// </summary>
    public int UpdateOrder {
      get { return this.updateOrder; }
      set {
        if (value != this.updateOrder) {
          this.updateOrder = value;
          OnUpdateOrderChanged();
        }
      }
    }

    /// <summary>Fires the UpdateOrderChanged event</summary>
    protected void OnUpdateOrderChanged() {
      if (UpdateOrderChanged != null) {
        UpdateOrderChanged(this, EventArgs.Empty);
      }
    }

    /// <summary>Only exists to implement IGameComponent</summary>
    void IGameComponent.Initialize() { }

    /// <summary>Updates the state of all input devices</summary>
    /// <param name="gameTime">Not used</param>
    void IUpdateable.Update(GameTime gameTime) {
      Update();
    }

    /// <summary>Whether the component is currently enabled</summary>
    bool IUpdateable.Enabled {
      get { return true; }
    }

    /// <summary>Sets up the collection of available game pads</summary>
    private void setupGamePads() {
      var gamePads = new List<IGamePad>();

#if !NO_XNAPAD
      // Add default XNA game pads
      for (PlayerIndex player = PlayerIndex.One; player <= PlayerIndex.Four; ++player) {
        gamePads.Add(new XnaGamePad(player));
      }
#endif

      // Add DirectInput-based game pads
      if (this.directInputManager != null) {
        gamePads.AddRange(this.directInputManager.CreateGamePads());
      }

      // Add place holders for all unattached game pads
      while (gamePads.Count < 8) {
        gamePads.Add(new NoGamePad());
      }

      this.gamePads = new ReadOnlyCollection<IGamePad>(gamePads);
    }

    /// <summary>Sets up the collection of available mice</summary>
    private void setupMouse() {
      var mice = new List<IMouse>();
#if NO_WININPUT
      // Add a dummy mouse
      mice.Add(new NoMouse());
#else
      // Add main PC mouse
      mice.Add(new WindowMessageMouse(this.windowMessageFilter));
#endif

      this.mice = new ReadOnlyCollection<IMouse>(mice);
    }

    /// <summary>Sets up the collection of available keyboards</summary>
    private void setupKeyboards() {
      var keyboards = new List<IKeyboard>();

      // Add XNA chat pads
      for (PlayerIndex player = PlayerIndex.One; player <= PlayerIndex.Four; ++player) {
        keyboards.Add(new XnaKeyboard(player, this.gamePads[(int)player]));
      }
#if NO_WININPUT
      // Add a dummy keyboard
      keyboards.Add(new NoKeyboard());
#else
      // Add main PC keyboard
      keyboards.Add(new WindowMessageKeyboard(this.windowMessageFilter));
#endif
      this.keyboards = new ReadOnlyCollection<IKeyboard>(keyboards);
    }

    /// <summary>Sets up the collection of available touch panels</summary>
    private void setupTouchPanels() {
      var touchPanels = new List<ITouchPanel>();

#if WINDOWS_PHONE
      // Add the Windows Phone 7 touch panel
      touchPanels.Add(new XnaTouchPanel());
#else
      // Add dummy touch panel
      touchPanels.Add(new NoTouchPanel());
#endif

      this.touchPanels = new ReadOnlyCollection<ITouchPanel>(touchPanels);
    }

    /// <summary>Manages DirectInput, if DirectInput is installed</summary>
    private IDirectInputManager directInputManager;

#if WINDOWS || WINRT || LINUX
    /// <summary>Intercepts input-related messages to XNA's main window</summary>
    //private WindowMessageFilter windowMessageFilter;
#endif
    /// <summary>Collection of all game pads known to the system</summary>
    private ReadOnlyCollection<IGamePad> gamePads;
    /// <summary>Collection of all mice known to the system</summary>
    private ReadOnlyCollection<IMouse> mice;
    /// <summary>Collection of all keyboards known to the system</summary>
    private ReadOnlyCollection<IKeyboard> keyboards;
    /// <summary>Collection of all touch panels known to the system</summary>
    private ReadOnlyCollection<ITouchPanel> touchPanels;

    /// <summary>Number of state snap shots currently queued</summary>
    private int snapshotCount;

    /// <summary>
    ///   Controls the order in which this game component is updated relative
    ///   to other game components.
    /// </summary>
    private int updateOrder = int.MinValue;
    /// <summary>Game service container, saved to unregister on dispose</summary>
    private GameServiceContainer gameServices;

  }

} // namespace Nuclex.Input

