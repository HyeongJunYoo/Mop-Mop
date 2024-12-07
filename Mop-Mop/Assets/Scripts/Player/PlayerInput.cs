using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }

        private void OnMove(InputValue value)
        {
            MoveInput = value.Get<Vector2>();
        }
    }
}
