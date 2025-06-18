using UnityEngine;
using System;

namespace Game.Managers
{
    public class InputManager : MonoBehaviour
    {
        public event Action OnInventoryToggle;
        public event Action OnPauseToggle;

        [Header("Key Bindings")]
        [SerializeField] private KeyCode inventoryKey = KeyCode.I;
        [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
        [SerializeField] private KeyCode attackKey = KeyCode.Space;

        private bool isInputLocked = false;


        private void Update()
        {
            if (isInputLocked) return;

            if (Input.GetKeyDown(inventoryKey))
                OnInventoryToggle?.Invoke();

            if (Input.GetKeyDown(pauseKey))
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

        public bool GetAttackInput() => Input.GetKeyDown(attackKey);

        public void LockInput()  => isInputLocked = true;
        public void UnlockInput() => isInputLocked = false;
        public bool IsInputLocked() => isInputLocked;
    }
}