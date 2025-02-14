using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// For items that will be spawned at a later time
    /// </summary>
    public class ItemBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool SpawnOnStart = true;
        [SerializeField] private ItemSo itemBlueprint;

        private void Start()
        {
            if (SpawnOnStart) SpawnItem();
        }

        public void SpawnItem()
        {
            ItemFactory.Instance.CreateItem(itemBlueprint, transform.position);
        }
    }
}