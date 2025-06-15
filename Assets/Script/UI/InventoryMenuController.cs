using UnityEngine;
using Game.Managers;

namespace Game.UI
{
    public class InventoryMenuController : MonoBehaviour
    {
        private InputManager inputManager;

        private void Awake()
        {
            inputManager = GameManager.Instance.InputManager;
        }

        private void OnEnable()
        {
            if (inputManager != null)
                inputManager.OnInventoryToggle += HandleInventoryToggle;
        }

        private void OnDisable()
        {
            if (inputManager != null)
                inputManager.OnInventoryToggle -= HandleInventoryToggle;
        }

        private void HandleInventoryToggle()
        {
            UIManager.Instance?.ToggleInventoryMenu();
        }
    }
}