using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeachTest
{
    [Serializable]
    public struct ItemsByType
    {
        public ItemType type;
        public Sprite[] items;
        
        /// <summary>
        /// Max number of this type of item allowed to exist simultaneously.
        /// </summary>
        public int maxNumber;

        public Sprite GetRandomSprite()
        {
            if (items == null || items.Length == 0)
            {
                return null;
            }

            return items[Random.Range(0, items.Length)];
        }
    }
}