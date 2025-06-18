using UnityEngine;
using Game.Managers;

namespace Game.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        private InputManager inputManager;
        private UIManager uiManager;

        private void Awake()
        {
            inputManager = GameManager.Instance.InputManager;
            uiManager    = GameManager.Instance.UIManager;
        }

        private void OnEnable()
        {
            if (inputManager != null)
                inputManager.OnPauseToggle += HandlePauseToggle;
        }

        private void OnDisable()
        {
            if (inputManager != null)
                inputManager.OnPauseToggle -= HandlePauseToggle;
        }

        private void HandlePauseToggle()
        {
            uiManager?.TogglePauseMenu();
        }
    }
}