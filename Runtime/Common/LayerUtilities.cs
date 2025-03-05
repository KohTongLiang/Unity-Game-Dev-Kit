using UnityEngine;

namespace GameCore
{
    public static class LayerUtilities
    {
        /// <summary>
        /// Determines if an object layer is included in the specified layer mask.
        /// Returns true if included, false otherwise.
        /// </summary>
        /// <param name="mask">The layer mask to check against.</param>
        /// <param name="objectLayer">The layer of the object.</param>
        /// <returns>True if the object layer is in the mask, false otherwise.</returns>
        public static bool CompareLayerToMask(LayerMask mask, int objectLayer)
        {
            return (mask & (1 << objectLayer)) != 0;
        }
    }
}