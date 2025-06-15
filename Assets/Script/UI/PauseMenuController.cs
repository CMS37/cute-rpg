using UnityEngine;
using Game.Managers;

namespace Game.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        private InputManager inputManager;

        private void Awake()
        {
            inputManager = GameManager.Instance.InputManager;
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
            Debug.Log("Call PauseToggle");
            UIManager.Instance?.TogglePauseMenu();
        }
    }
}