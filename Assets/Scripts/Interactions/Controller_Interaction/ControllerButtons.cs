using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtons : MonoBehaviour
{
    /* This class can be used for Controller button Interactions */
    public GameObject controller;

    public bool LeftController;

    public LineRenderer lineRenderer;
    public float laserWidth = 0.01f;
    public float laserMaxLength = 3f;

    private HitColliderButton _lastDetectorHit;

    public bool debugMode;
    public TextMesh debugText;

    public bool ignoreButtonPresses; //Used so the left controller can also control the options menu

    public GameObject optionsMenu;

    public DialogManager dialogManager;

    public bool useHapticFeedback;
    private bool _vibrating = false;

    public bool useOnlyForMenu = false;

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
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;

            OptionsMenuRaycast();

            if (!ignoreButtonPresses)
                if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch))
                { // Options Menu
                    optionsMenu.SetActive(false);
                }

            return;
        }
        if (_lastDetectorHit != null)
        {
            _lastDetectorHit.RaycastExit();
            _lastDetectorHit = null;
        }

        if (lineRenderer.enabled)
            lineRenderer.enabled = false;
        /*//////////////////*/

        if (ignoreButtonPresses)
            return;

        if (useOnlyForMenu)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch))
            { // Options Menu
                if (debugMode)
                    ChangeDebugText("Options");

                optionsMenu.SetActive(true);
            }

            return;
        }

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Touch))
        { //Yes
            bool left = OVRInput.GetDown(OVRInput.Button.One) ? false : true;

            if (debugMode)
                ChangeDebugText("Yes");

            /* INTERACTION FOR WHEN CONTROLLER YES */
            dialogManager.AnswerA();

            if (useHapticFeedback && !_vibrating)
                HapticFeedback(.8f, 1f, left);

        }
        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.Touch))
        { //No
            bool left = OVRInput.GetDown(OVRInput.Button.Two) ? false : true;

            if (debugMode)
                ChangeDebugText("No");

            /* INTERACTION FOR WHEN CONTROLLER NO */
            dialogManager.AnswerB();

            if (useHapticFeedback && !_vibrating)
                HapticFeedback(.8f, 1f, left);

        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch))
        { // Options Menu
            if (debugMode)
                ChangeDebugText("Options");

            optionsMenu.SetActive(true);
        }
        
    }

    void OptionsMenuRaycast() {
        Transform startTransform = controller.transform;
        Vector3 rayDirection = controller.transform.forward;

        // Raycast
        RaycastHit hit;
        Ray ray = new Ray(startTransform.position, rayDirection);

        Physics.Raycast(ray, out hit, laserMaxLength, 7);
        Vector3 endPosition;

        if (hit.collider)
        {
            bool triggerPressed;
            if (LeftController)
            {
                triggerPressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.Three);
            }
            else
            {
                triggerPressed = OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.One);
            }

            HitColliderButton hitDetector = hit.collider.gameObject.GetComponent<HitColliderButton>(); 
            if (hitDetector != null)
            {
                if (_lastDetectorHit != hitDetector)
                {
                    if (_lastDetectorHit != null)
                        _lastDetectorHit.RaycastExit();

                    _lastDetectorHit = hitDetector;

                    if (useHapticFeedback && !_vibrating)
                        HapticFeedback(.8f, .25f, LeftController);

                    _lastDetectorHit.RaycastEnter();
                }

                if(triggerPressed)
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
        DrawLine(startTransform.position + rayDirection * .05f, endPosition);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void HapticFeedback(float frequency, float amplitude, bool left = false, float seconds = 0.05f)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        _vibrating = true;

        StartCoroutine(EndHapticFeedback(seconds, left));
    }

    IEnumerator EndHapticFeedback(float seconds, bool left = false)
    {
        yield return new WaitForSeconds(seconds);
        OVRInput.SetControllerVibration(0, 0, left ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
        _vibrating = false;
    }

    void ChangeDebugText(string name)
    {
        if (debugText != null)
            debugText.text = name;
    }
}
