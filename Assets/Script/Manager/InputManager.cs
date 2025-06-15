using UnityEngine;
using System;

namespace Game.Managers
{
    public class InputManager : MonoBehaviour
    {
        public event Action OnInventoryToggle;
        public event Action OnPauseToggle;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                OnInventoryToggle?.Invoke();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Call InputManager Escape");
                OnPauseToggle?.Invoke();
            }
        }

        public Vector2 GetMovement()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            return new Vector2(x, y).normalized;
        }

        public bool GetAttackInput() => Input.GetKeyDown(KeyCode.Space);
    }
}