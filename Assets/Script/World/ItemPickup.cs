using UnityEngine;
using Game.Managers;

namespace Game.World
{
    [RequireComponent(typeof(Collider2D))]
    public class ItemPickup : MonoBehaviour
    {
        [Header("아이템 정보")]
        [Tooltip("획득할 아이템 ID")]
        [SerializeField] private string itemId;
        [Tooltip("획득할 수량")]
        [SerializeField] private int quantity = 1;

        private InventoryManager inventoryManager;

        private void Awake()
        {
            inventoryManager = GameManager.Instance.InventoryManager;
        }

        private void Reset()
        {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            inventoryManager.Add(itemId, quantity);

            Destroy(gameObject);
        }
    }
}
