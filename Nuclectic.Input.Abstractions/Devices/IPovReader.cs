namespace Nuclectic.Input.Devices
{
    /// <summary>Reads the state of a point-of-view controller</summary>
    public interface IPovReader {

        /// <summary>Retrieves the current direction of the PoV controller</summary>
        /// <param name="povs">PoV states the direction will be read from</param>
        /// <returns>The direction of the PoV controller</returns>
        int GetDirection(int[] povs);

        /// <summary>
        ///   Reports whether the state of the point-of-view controller has changed
        /// </summary>
        /// <param name="previous">Previous states of the PoV controllers</param>
        /// <param name="current">Current states of the PoV controllers</param>
        /// <returns>True if the state of the PoV controller has changed</returns>
        bool HasChanged(int[] previous, int[] current);

    }
}