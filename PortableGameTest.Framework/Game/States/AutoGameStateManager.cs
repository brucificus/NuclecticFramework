using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.OwnedInstances;
using Microsoft.Xna.Framework;
using Nuclex.Game.States;

namespace PortableGameTest.Framework.Game.States
{
	public class AutoGameStateManager
		: IAutoGameStateService, IDisposable
	{
		private readonly IComponentContext _StateResolver;
		private readonly IManualGameStateService _ManualGameStateService;
		private readonly Dictionary<IGameState, Owned<IGameState>> _GameStateOwners = new Dictionary<IGameState, Owned<IGameState>>(); 

		public AutoGameStateManager(Autofac.IComponentContext stateResolver, IManualGameStateService manualGameStateService)
		{
			_StateResolver = stateResolver;
			_ManualGameStateService = manualGameStateService;
		}

		public void Push<TGameState>(GameStateModality modality) where TGameState : IGameState
		{
			var ownedGameState = _StateResolver.Resolve<Owned<TGameState>>();
			_GameStateOwners.Add(ownedGameState.Value, new Owned<IGameState>(ownedGameState.Value, ownedGameState));
			_ManualGameStateService.Push(ownedGameState.Value, modality);
		}

		public void Push<TGameState, TParameter1>(GameStateModality modality, TParameter1 parameter1) where TGameState : IGameState
		{
			var ownedGameState = _StateResolver.Resolve<Owned<TGameState>>(new TypedParameter(typeof(TParameter1), parameter1));
			_GameStateOwners.Add(ownedGameState.Value, new Owned<IGameState>(ownedGameState.Value, ownedGameState));
			_ManualGameStateService.Push(ownedGameState.Value, modality);
		}

		public void Push<TGameState, TParameter1, TParameter2>(GameStateModality modality, TParameter1 parameter1, TParameter2 parameter2) where TGameState : IGameState
		{
			var ownedGameState = _StateResolver.Resolve<Owned<TGameState>>(
				new TypedParameter(typeof(TParameter1), parameter1),
				new TypedParameter(typeof(TParameter2), parameter2)
				);
			_GameStateOwners.Add(ownedGameState.Value, new Owned<IGameState>(ownedGameState.Value, ownedGameState));
			_ManualGameStateService.Push(ownedGameState.Value, modality);
		}

		public void Push<TGameState, TParameter1, TParameter2, TParameter3>(GameStateModality modality, TParameter1 parameter1, TParameter2 parameter2, TParameter3 parameter3) where TGameState : IGameState
		{
			var ownedGameState = _StateResolver.Resolve<Owned<TGameState>>(
				new TypedParameter(typeof(TParameter1), parameter1),
				new TypedParameter(typeof(TParameter2), parameter2),
				new TypedParameter(typeof(TParameter3), parameter3)
				);
			_GameStateOwners.Add(ownedGameState.Value, new Owned<IGameState>(ownedGameState.Value, ownedGameState));
			_ManualGameStateService.Push(ownedGameState.Value, modality);
		}

		public void Pop()
		{
			var gameState = _ManualGameStateService.Pop();
			Owned<IGameState> owner;
			if(_GameStateOwners.TryGetValue(gameState, out owner))
			{
				_GameStateOwners.Remove(gameState);
				owner.Dispose();
			}
		}

		public void Update(GameTime gameTime)
		{
			_ManualGameStateService.Update(gameTime);
		}

		public bool Enabled { get { return _ManualGameStateService.Enabled; } }
		public int UpdateOrder { get { return _ManualGameStateService.UpdateOrder; } }
		public event EventHandler<EventArgs> EnabledChanged
		{
			add { _ManualGameStateService.EnabledChanged += value; }
			remove { _ManualGameStateService.EnabledChanged -= value; }
		}
		public event EventHandler<EventArgs> UpdateOrderChanged
		{
			add { _ManualGameStateService.UpdateOrderChanged += value; }
			remove { _ManualGameStateService.UpdateOrderChanged -= value; }
		}

		public void Draw(GameTime gameTime)
		{
			_ManualGameStateService.Draw(gameTime);
		}

		public int DrawOrder { get { return _ManualGameStateService.DrawOrder; } }
		public bool Visible { get { return _ManualGameStateService.Visible; } }
		public event EventHandler<EventArgs> DrawOrderChanged
		{
			add { _ManualGameStateService.DrawOrderChanged += value; }
			remove { _ManualGameStateService.DrawOrderChanged -= value; }
		}
		public event EventHandler<EventArgs> VisibleChanged 
		{ 
			add { _ManualGameStateService.VisibleChanged += value; } 
			remove { _ManualGameStateService.VisibleChanged -= value; }
		}
		public IGameState ActiveState { get { return _ManualGameStateService.ActiveState; } }
		public void Pause()
		{
			_ManualGameStateService.Pause();
		}

		public void Resume()
		{
			_ManualGameStateService.Resume();
		}

		public void Dispose()
		{
			foreach (var gameState in _GameStateOwners.Keys)
				_GameStateOwners[gameState].Dispose();
			_GameStateOwners.Clear();
		}
	}
}
