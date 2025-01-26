using Core.Systems;
using UnityEngine;

namespace Core.Gameplay
{
    public class PlayerInputSource : IInputSource
    {
        public float GetAcceleration()
        {
            return Input.GetAxis("Vertical");
        }

        public bool GetBrake()
        {
            return Input.GetKey(KeyCode.Space);
        }

        public float GetSteering()
        {
            return Input.GetAxis("Horizontal");
        }

        public void UpdateInput()
        {
            //because using an old Input, dont needed
        }
    }
}