using Microsoft.Xna.Framework;

namespace Nuclex.Geometry.Volumes
{
    /// <summary>A point of contact between two volumes</summary>
    public struct ContactPoint {

        /// <summary>The absolute location where the contact occurs</summary>
        public Vector3 Location;
        /// <summary>The time at which the contact occurs</summary>
        public double Time;

    }
}