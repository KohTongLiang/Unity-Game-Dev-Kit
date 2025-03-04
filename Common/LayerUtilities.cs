using UnityEngine;

namespace GameCore
{
    public static class LayerUtilities
    {
        /// <summary>
        /// Pass in a Layer mask and a object layer to determine if they are on the
        /// same layer. Returns True if on same layer, False otheriwse.
        /// </summary>
        /// <param name="mask">LayerMask input</param>
        /// <param name="objectLayer">GameObject layer</param>
        /// <returns></returns>
        public static bool CompareLayerToMask(LayerMask mask, int objectLayer)
        {
            return (mask & (1 << objectLayer)) != 0;
        }
    }
}