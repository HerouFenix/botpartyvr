using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHaptics : MonoBehaviour
{
    public bool leftController;

    public void ContinuousHapticFeedback(float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, leftController ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
    }

    public void StopContinuousHapticFeedback()
    {
        OVRInput.SetControllerVibration(0, 0, leftController ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
    }
}
