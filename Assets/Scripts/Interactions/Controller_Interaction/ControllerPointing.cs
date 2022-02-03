using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControllerPointing : MonoBehaviour
{
    /* This class can be used for Controller Pointing Interactions with physical objects (i.e not Canvas elements)*/
    public GameObject controller;

    public bool onlyUsedForButtons;
    public bool LeftController;

    public bool enableLine;

    public LineRenderer lineRenderer;
    public float laserWidth = 0.01f;
    public float laserMaxLength = 3f;

    private RaycastDetector _lastDetectorHit;
    private HitColliderButton _lastDetectorHit_Options;


    public GameObject optionsMenu;
    public bool ignoreButtonPresses; //Used so the left controller can also control the options menu

    public bool useHapticFeedback;
    private bool _vibrating = false;
    private bool _continuousVibrating = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    // Update is called once per frame
    void Update()
    {
        /*Options Menu Stuff*/
        if (optionsMenu != null && optionsMenu.activeSelf)
        {
            if (_lastDetectorHit != null)
            {
                _lastDetectorHit.RaycastExit();
                _lastDetectorHit = null;
            }

            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;

            OptionsMenuRaycast();

            if (!ignoreButtonPresses)
                if ((!LeftController && OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick)) || (LeftController && OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick)))
                { // Options Menu
                    optionsMenu.SetActive(false);
                }

            return;
        }

        if (!ignoreButtonPresses && optionsMenu != null && (!LeftController && OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick)) || (LeftController && OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick)))
        { // Options Menu
            optionsMenu.SetActive(true);
            return;
        }


        /*//////////////////*/

        if (!lineRenderer.enabled)
            return;

        Transform startTransform = controller.transform;
        Vector3 rayDirection = controller.transform.forward;

        // Raycast
        RaycastHit hit;
        Ray ray = new Ray(startTransform.position, rayDirection);

        Physics.Raycast(ray, out hit, laserMaxLength);
        Vector3 endPosition;

        if (hit.collider)
        {
            bool triggerPressed;
            if (LeftController)
            {
                triggerPressed = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.02f;
            }
            else
            {
                triggerPressed = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.02f;
            }

            RaycastDetector hitDetector = hit.collider.gameObject.GetComponent<RaycastDetector>();
            if (!onlyUsedForButtons)
            {
                if (hitDetector != null)
                {
                    if (_lastDetectorHit != hitDetector)
                    {
                        if (_lastDetectorHit != null)
                            _lastDetectorHit.RaycastExit();

                        _lastDetectorHit = hitDetector;

                        if (useHapticFeedback && !_vibrating && !triggerPressed)
                            HapticFeedback(.8f, .25f);

                    }

                    
                    if (triggerPressed)
                    {
                        if (useHapticFeedback && !_continuousVibrating)
                        {
                            ContinuousHapticFeedback(.6f, .8f);
                        }

                        _lastDetectorHit.RaycastEnter();

                    }
                    else
                    {
                        if (useHapticFeedback && _continuousVibrating)
                            StopContinuousHapticFeedback();


                        if (_lastDetectorHit != null)
                        {
                            _lastDetectorHit.RaycastExit();
                        }
                    }
                   
                }
                else
                {
                    if (useHapticFeedback && _continuousVibrating)
                        StopContinuousHapticFeedback();

                    if (_lastDetectorHit != null)
                    {
                        _lastDetectorHit.RaycastExit();
                        _lastDetectorHit = null;
                    }
                }
            }
            else
            {
                if (hitDetector != null)
                {
                    if (_lastDetectorHit != hitDetector)
                    {
                        if (_lastDetectorHit != null)
                            _lastDetectorHit.RaycastExit();

                        _lastDetectorHit = hitDetector;
                        if (useHapticFeedback && !_vibrating)
                            HapticFeedback(.8f, .25f);

                        _lastDetectorHit.RaycastEnter();
                    }

                    if (triggerPressed)
                        _lastDetectorHit.RaycastClick();

                }
                else
                {
                    if (_lastDetectorHit != null)
                    {
                        _lastDetectorHit.RaycastExit();
                        _lastDetectorHit = null;
                    }
                }
            }


            endPosition = hit.point;
        }
        else
        {
            endPosition = startTransform.position + rayDirection * laserMaxLength;

            if (_lastDetectorHit != null)
            {
                _lastDetectorHit.RaycastExit();
                _lastDetectorHit = null;
            }
        }


        // Draw Pointer Line
        if (enableLine)
        {
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
            DrawLine(startTransform.position + rayDirection * .05f, endPosition);
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }

    }

    void OptionsMenuRaycast()
    {
        Transform startTransform = controller.transform;
        Vector3 rayDirection = controller.transform.forward;

        // Raycast
        RaycastHit hit;
        Ray ray = new Ray(startTransform.position, rayDirection);

        Physics.Raycast(ray, out hit, laserMaxLength);
        Vector3 endPosition;

        if (hit.collider)
        {
            bool triggerPressed;
            if (LeftController)
            {
                triggerPressed = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.02f;
            }
            else
            {
                triggerPressed = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.02f;
            }

            HitColliderButton hitDetector = hit.collider.gameObject.GetComponent<HitColliderButton>();
            if (hitDetector != null)
            {
                if (_lastDetectorHit_Options != hitDetector)
                {
                    if (_lastDetectorHit_Options != null)
                        _lastDetectorHit_Options.RaycastExit();

                    _lastDetectorHit_Options = hitDetector;

                    if (useHapticFeedback && !_vibrating)
                        HapticFeedback(.8f, .25f);

                    _lastDetectorHit_Options.RaycastEnter();
                }

                if (triggerPressed)
                    _lastDetectorHit_Options.RaycastClick();

            }
            else
            {
                if (_lastDetectorHit_Options != null)
                {
                    _lastDetectorHit_Options.RaycastExit();
                    _lastDetectorHit_Options = null;
                }
            }

            endPosition = hit.point;
        }
        else
        {
            endPosition = startTransform.position + rayDirection * laserMaxLength;

            if (_lastDetectorHit_Options != null)
            {
                _lastDetectorHit_Options.RaycastExit();
                _lastDetectorHit_Options = null;
            }
        }



        // Draw Pointer Line
        DrawLine(startTransform.position + rayDirection * .05f, endPosition);
    }



    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void HapticFeedback(float frequency, float amplitude, float seconds = 0.05f)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, LeftController ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        _vibrating = true;

        StartCoroutine(EndHapticFeedback(seconds));
    }

    IEnumerator EndHapticFeedback(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OVRInput.SetControllerVibration(0, 0, LeftController ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        _vibrating = false;
    }

    void ContinuousHapticFeedback(float frequency, float amplitude)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, LeftController ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        _continuousVibrating = true;
    }

    void StopContinuousHapticFeedback()
    {
        OVRInput.SetControllerVibration(0, 0, LeftController ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        _continuousVibrating = false;
    }

    public void EnableLine()
    {
        Debug.Log("Hi");
        enableLine = true;
        lineRenderer.enabled = true;
    }
    public void DisableLine()
    {
        enableLine = false;
        lineRenderer.enabled = false;

        if (_lastDetectorHit != null)
        {
            if (useHapticFeedback && _continuousVibrating)
                StopContinuousHapticFeedback();

            _lastDetectorHit.RaycastExit();
            _lastDetectorHit = null;
        }
    }

}
