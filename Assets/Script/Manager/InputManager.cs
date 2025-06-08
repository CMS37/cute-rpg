using UnityEngine;
using System;

namespace Managers
{
    [DefaultExecutionOrder(-100)]
    public class InputManager : MonoBehaviour
    {
        public event Action onInventoryToggle;

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
                onInventoryToggle?.Invoke();
        }
    }
}
