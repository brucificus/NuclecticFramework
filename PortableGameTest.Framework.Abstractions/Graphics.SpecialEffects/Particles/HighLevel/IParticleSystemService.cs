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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.Graphics.Batching;

namespace Nuclex.Graphics.SpecialEffects.Particles.HighLevel {

  /// <summary>Provides services for rendering and updating particle systems</summary>
  public interface IParticleSystemService {

    /// <summary>Adds a particle system to be processed by the manager</summary>
    /// <typeparam name="ParticleType">
    ///   Type of particles being stored in the particle system
    /// </typeparam>
    /// <typeparam name="VertexType">
    ///   Type of vertices that will be generated from the particles
    /// </typeparam>
    /// <param name="particleSystem">
    ///   Particle system that will be added to the manager
    /// </param>
    /// <param name="pruneDelegate">Method used to detect dead particles</param>
    /// <param name="renderer">
    ///   Particle renderer that will turn the particles into vertices and send
    ///   them to a primitive batch for rendering
    /// </param>
    void AddParticleSystem<ParticleType, VertexType>(
      ParticleSystem<ParticleType> particleSystem,
      ParticleSystem<ParticleType>.PrunePredicate pruneDelegate,
      ParticleRenderer<ParticleType, VertexType> renderer
    )
      where ParticleType : struct
      where VertexType : struct, IVertexType;

    /// <summary>Removes a particle system from the manager</summary>
    /// <typeparam name="ParticleType">
    ///   Type of particles being stored in the particle system
    /// </typeparam>
    /// <param name="particleSystem">
    ///   Particle system that will be removed from the manager
    /// </param>
    void RemoveParticleSystem<ParticleType>(
      ParticleSystem<ParticleType> particleSystem
    ) where ParticleType : struct, IVertexType;

  }

} // namespace Nuclex.Graphics.SpecialEffects.Particles.HighLevel
