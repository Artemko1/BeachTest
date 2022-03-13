using UnityEngine;

namespace BeachTest
{
    public class Item : MonoBehaviour
    {
        public ItemType Type { get; set; }

        private SpriteRenderer spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            if (spriteRenderer)
            {
                spriteRenderer.sprite = sprite;
            }
        }

        public void SetSortingOrder(int order)
        {
            if (spriteRenderer)
            {
                spriteRenderer.sortingOrder = order;
            }
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}