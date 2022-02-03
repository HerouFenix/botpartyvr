using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactions.Controller_Interaction
{
    public class ControllerJoystick : MonoBehaviour
    {

        [SerializeField] private UnityEvent _onYes;
        [SerializeField] private UnityEvent _onNo;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 leftJoystickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            Vector2 rightJoystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            if (leftJoystickInput.x > 0.9f) 
            {
                _onYes.Invoke();
            }
            else if (leftJoystickInput.x < -0.9f)
            {
                _onNo.Invoke();
            }
            else if (rightJoystickInput.x > 0.9f)
            {
                _onYes.Invoke();
            }
            else if (rightJoystickInput.x < -0.9f)
            {
                _onNo.Invoke();
            }
        }
    }
}