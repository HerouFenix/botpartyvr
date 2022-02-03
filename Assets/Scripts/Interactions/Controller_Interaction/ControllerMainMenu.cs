using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMainMenu : MonoBehaviour
{
    /* This class can be used for Controller Pointing Interactions with physical objects (i.e not Canvas elements)*/
    public GameObject controller;

    public bool LeftController;

    public LineRenderer lineRenderer;
    public float laserWidth = 0.01f;
    public float laserMaxLength = 3f;

    private HitColliderButton _lastDetectorHit;


    public bool useHapticFeedback;
    private bool _vibrating = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    // Update is called once per frame
    void Update()
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
                triggerPressed = (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.Three));
            }
            else
            {
                triggerPressed = (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || OVRInput.GetDown(OVRInput.Button.One));
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
                        HapticFeedback(.8f,.25f);
                        

                    _lastDetectorHit.RaycastEnter();
                }

                if (triggerPressed)
                {   
                    _lastDetectorHit.RaycastClick();
                }

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

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
