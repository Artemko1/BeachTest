using UnityEngine;

namespace BeachTest
{
    [CreateAssetMenu(fileName = "ItemsByTypes", menuName = "ItemsByTypes", order = 0)]
    public class ItemsByTypesSO : ScriptableObject
    {
        [SerializeField] private ItemsByType[] itemsByTypes = null;

        public ItemsByType[] ItemsByTypes => itemsByTypes;
    }
}