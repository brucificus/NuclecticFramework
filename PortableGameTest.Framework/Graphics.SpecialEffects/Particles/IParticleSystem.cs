using System;

namespace Nuclex.Graphics.SpecialEffects.Particles
{
    public interface IParticleSystem<ParticleType> where ParticleType : struct
    {
        /// <summary>Adds a new particle to the particle system</summary>
        /// <param name="particle">Particle that will be added to the system</param>
        /// <remarks>
        ///   If the particle system is full, the added particle will be silently discarded.
        /// </remarks>
        void AddParticle(ParticleType particle);

        /// <summary>Removes a particle from the particle system</summary>
        /// <param name="index">Index of the particle that will be removed</param>
        void RemoveParticle(int index);

        /// <summary>Particles being simulated by the particle system</summary>
        ArraySegment<ParticleType> Particles { get; }

        /// <summary>Number of particles the particle system can manage</summary>
        int Capacity { get; }

        /// <summary>Affectors that are influencing the particles in this system</summary>
        AffectorCollection<ParticleType> Affectors { get; }

        /// <summary>Runs the specified number of updates on the particle system</summary>
        /// <param name="updates">Number of updates that will be run</param>
        void Update(int updates);

        /// <summary>Begins a multi-threaded update of the particles</summary>
        /// <param name="updates">
        ///   Number of updates to perform. A single update will take full advantage
        ///   of multiple threads as well.
        /// </param>
        /// <param name="threads">Number of threads to perform the updates in</param>
        /// <param name="callback">
        ///   Callback that will be invoked when the update has finished
        /// </param>
        /// <param name="state">
        ///   User defined parameter that will be passed to the callback
        /// </param>
        /// <returns>An asynchronous result handle for the background operation</returns>
        IAsyncResult BeginUpdate(
            int updates, int threads, AsyncCallback callback, object state
            );

        /// <summary>Ends a multi-threaded particle system update</summary>
        /// <param name="asyncResult">
        ///   Asynchronous result handle returned by the BeginUpdate() method
        /// </param>
        void EndUpdate(IAsyncResult asyncResult);

        /// <summary>Prunes dead particles from the system</summary>
        /// <param name="pruneDelegate">
        ///   Delegate deciding which particles will be pruned
        /// </param>
        void Prune(ParticleSystem<ParticleType>.PrunePredicate pruneDelegate);

        /// <summary>Begins a threaded prune of the particles</summary>
        /// <param name="pruneDelegate">
        ///   Method that evaluates whether a particle should be pruned from the system
        /// </param>
        /// <param name="callback">
        ///   Callback that will be invoked when the update has finished
        /// </param>
        /// <param name="state">
        ///   User defined parameter that will be passed to the callback
        /// </param>
        /// <returns>An asynchronous result handle for the background operation</returns>
        IAsyncResult BeginPrune(
            ParticleSystem<ParticleType>.PrunePredicate pruneDelegate, AsyncCallback callback, object state
            );

        /// <summary>Ends a multi-threaded prune of the particle system</summary>
        /// <param name="asyncResult">
        ///   Asynchronous result handle returned by the BeginPrune() method
        /// </param>
        void EndPrune(IAsyncResult asyncResult);
    }
}