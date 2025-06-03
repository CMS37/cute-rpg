using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private Vector2 moveInput;

        private void Update()
        {
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");
            Vector2 raw = new Vector2(hor, ver);

            if (raw.sqrMagnitude > 1f)
                raw.Normalize();

            moveInput = raw;
        }

        public Vector2 GetMovement()
        {
            return moveInput;
        }
    }
}
