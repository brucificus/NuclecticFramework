#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2009 Nuclex Development Labs

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
using System.Diagnostics;

using Microsoft.Xna.Framework.Graphics;

using Nuclex.Graphics.Batching;
using Nuclex.Support;

namespace Nuclex.Graphics.SpecialEffects.Particles.HighLevel {

  partial class ParticleSystemManager {

    #region class PrimitiveBatchHolder

    /// <summary>Holds a primitive batch used to render particles</summary>
    private abstract class PrimitiveBatchHolder {

      /// <summary>Initializes a new primitive batch holder</summary>
      /// <param name="manager">Particle system manager the holder belongs to</param>
      public PrimitiveBatchHolder(ParticleSystemManager manager) {
        this.Manager = manager;
        this.ReferenceCount = 1;
      }

      /// <summary>Begins drawing with the contained primitive batch</summary>
      public abstract void Begin();
      /// <summary>Ends drawing with the contained primitve batch</summary>
      public abstract void End();

      /// <summary>Releases or destroys the referenced primitive batch</summary>
      public abstract void Release();

      /// <summary>Particle system manager this holder belongs to</summary>
      protected ParticleSystemManager Manager;
      /// <summary>Number of active references to the primitive batch holder</summary>
      public int ReferenceCount;

    }

    #endregion // class PrimitiveBatchHolder

    #region class PrimitiveBatchHolder<>

    /// <summary>Holds a type-safe primitive batch used to render particles</summary>
    private class PrimitiveBatchHolder<VertexType> : PrimitiveBatchHolder
      where VertexType : struct, IVertexType {

      /// <summary>Initializes a new primitive batch holder</summary>
      /// <param name="manager">
      ///   Particle system manager the primitive batch belongs to
      /// </param>
      /// <param name="primitiveBatch">Primitive batch being held</param>
      public PrimitiveBatchHolder(
        ParticleSystemManager manager,
        PrimitiveBatch<VertexType> primitiveBatch
      ) :
        base(manager) {
        this.PrimitiveBatch = primitiveBatch;
      }

      /// <summary>Begins drawing with the contained primitive batch</summary>
      public override void Begin() {
        this.PrimitiveBatch.Begin(QueueingStrategy.Deferred);
      }

      /// <summary>Ends drawing with the contained primitive batch</summary>
      public override void End() {
        this.PrimitiveBatch.End();
      }

      /// <summary>Releases or destroys the referenced primitive batch</summary>
      public override void Release() {
        if(this.ReferenceCount > 1) {
          --this.ReferenceCount;
        } else {
          this.PrimitiveBatch.Dispose();
          base.Manager.primitiveBatches.Remove(typeof(VertexType));
        }
      }

      /// <summary>Primitive batch for the holder's vertex type</summary>
      public PrimitiveBatch<VertexType> PrimitiveBatch;

    }

    #endregion // class PrimitiveBatchHolder<>

    /// <summary>Delegate for creating a new primitive batch holder</summary>
    internal delegate void InduceErrorDelegate();

    /// <summary>
    ///   Retrieves or creates the primitive batch for the specified vertex type
    /// </summary>
    /// <typeparam name="VertexType">
    ///   Vertex type a primitive batch will be returned for
    /// </typeparam>
    /// <returns>A primitive batch that renders the specified vertex type</returns>
    private PrimitiveBatchHolder<VertexType> getOrCreatePrimitiveBatch<VertexType>()
      where VertexType : struct, IVertexType {
      PrimitiveBatchHolder holder;

      // Find out if we a primitive batch and vertex declaration already exist
      if(this.primitiveBatches.TryGetValue(typeof(VertexType), out holder)) {

        // Yes, add another reference to this primitive batch and vertex declaration
        ++holder.ReferenceCount;
        return (holder as PrimitiveBatchHolder<VertexType>);

      } else { // No, let's create one

        // Create the primitive batch for rendering this vertex structure
        PrimitiveBatchHolder<VertexType> newHolder;

#if UNITTEST
        if(this.InducePrimitiveBatchErrorDelegate != null) {
          this.InducePrimitiveBatchErrorDelegate();
        }
#endif
        newHolder = new PrimitiveBatchHolder<VertexType>(
          this, new PrimitiveBatch<VertexType>(GraphicsDevice)
        );
        this.primitiveBatches.Add(typeof(VertexType), newHolder);
        this.holderArraysDirty = true;

        return newHolder;
      }
    }

#if UNITTEST
    /// <summary>
    ///   Can be used to induce a construction error in the primitive batch holder
    /// </summary>
    internal InduceErrorDelegate InducePrimitiveBatchErrorDelegate;
#endif
    /// <summary>Primitive batches for the vertex types of all particle systems</summary>
    private Dictionary<Type, PrimitiveBatchHolder> primitiveBatches;

  }

} // namespace Nuclex.Graphics.SpecialEffects.Particles.HighLevel
