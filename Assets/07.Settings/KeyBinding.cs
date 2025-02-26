using UnityEngine;

public enum KeyAction { UP, DOWN, LEFT, RIGHT }

namespace Preference
{
    public class KeyBinding: MonoBehaviour
    {
        private static readonly string controlTypeKey = "controlTypeKey";

        public void UpdateKeyType(int index)
        {
            PlayerPrefs.SetInt(controlTypeKey, index);
        }

        public Vector2 GetMovementDirection()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            return new Vector2(horizontal, vertical).normalized;
        }
    }
}