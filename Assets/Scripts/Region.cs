using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace BeachTest
{
    public class Region : MonoBehaviour
    {
        [SerializeField] private ItemsByTypesSO itemsByTypesSO = null;
        [SerializeField] private Item itemPrefab = null;

        [SerializeField] private int maxItemsOnRegion = 5;

        /// <summary>
        /// Pool of currently created items.
        /// </summary>
        private readonly List<Item> spawnedItems = new List<Item>();

        private int sortingLayerID = 0;

        private void Awake()
        {
            var rend = GetComponent<SpriteRenderer>();
            if (rend)
            {
                sortingLayerID = rend.sortingLayerID;
            }
        }

        private void OnMouseDown()
        {
            if (spawnedItems?.Count >= maxItemsOnRegion)
            {
                Destroy(spawnedItems[0].gameObject);
                spawnedItems.RemoveAt(0);
            }

            PlaceItem();
        }

        private void PlaceItem()
        {
            var isFound = FindAllowedItemsByType(out var itemsByType);
            if (!isFound)
            {
                Debug.LogWarning("No available item types to spawn", this);
                return;
            }

            var mouseWorldPosition = GetMouseWorldPosition();

            var spawnedItem = Instantiate(itemPrefab, mouseWorldPosition, Quaternion.identity);

            spawnedItem.SetSprite(itemsByType.GetRandomSprite());
            spawnedItem.SetSortingOrder(itemsByType.sortOrder);
            spawnedItem.SetSortingLayerID(sortingLayerID);
            spawnedItem.Type = itemsByType.type;

            spawnedItems.Add(spawnedItem);
        }

        /// <summary>
        /// Find a first type from all available types for this region that is available to spawn
        /// </summary>
        /// <param name="itemsByType"></param>
        /// <returns></returns>
        private bool FindAllowedItemsByType(out ItemsByType itemsByType)
        {
            Assert.IsNotNull(itemsByTypesSO, "itemsByTypesSO != null");
            Assert.IsNotNull(itemsByTypesSO.ItemsByTypes, "itemsByTypesSO.ItemsByTypes != null");

            var remainingTypes = itemsByTypesSO.ItemsByTypes.ToList();
            itemsByType = new ItemsByType();

            while (true)
            {
                if (remainingTypes.Count == 0)
                {
                    return false;
                }

                var randomIndex = Random.Range(0, remainingTypes.Count);
                itemsByType = remainingTypes[randomIndex];
                var found = CanSpawnOfType(itemsByType.type, itemsByType.maxNumber);

                if (found)
                {
                    return true;
                }

                remainingTypes.RemoveAt(randomIndex);
            }
        }

        /// <summary>
        /// Can one more sprite of type be spawned or there is max of them already
        /// </summary>
        /// <param name="type"></param>
        /// <param name="maxAllowed"></param>
        /// <returns></returns>
        private bool CanSpawnOfType(ItemType type, int maxAllowed)
        {
            if (type == ItemType.None || maxAllowed <= 0)
            {
                return true;
            }

            var sum = 0;
            foreach (var item in spawnedItems)
            {
                if (item.Type != type)
                {
                    continue;
                }

                sum++;
                if (sum >= maxAllowed)
                {
                    return false;
                }
            }

            return true;
        }

        private static Vector3 GetMouseWorldPosition()
        {
            var mainCamera = Camera.main;
            if (!mainCamera)
            {
                return Vector3.zero;
            }

            var worldPoint = Input.mousePosition;
            worldPoint.z = Mathf.Abs(mainCamera.transform.position.z);

            return mainCamera.ScreenToWorldPoint(worldPoint);
        }
    }
}