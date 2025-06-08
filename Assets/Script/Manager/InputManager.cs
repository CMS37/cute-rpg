using UnityEngine;
using System;

namespace Game.Managers
{
    [DefaultExecutionOrder(-100)]
    public class InputManager : MonoBehaviour
    {
        public event Action OnInventoryToggle;

        public Vector2 GetMovement()
        {
            return new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                OnInventoryToggle?.Invoke();
        }
    }
}
