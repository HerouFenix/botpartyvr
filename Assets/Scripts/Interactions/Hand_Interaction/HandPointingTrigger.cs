using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static OVRHand;

public class HandPointingTrigger : MonoBehaviour
{
    /* This class can be used for Hand Pointing Interactions with physical objects (i.e not Canvas elements)*/

    public OVRHand hand;
    public OVRSkeleton handSkeleton;

    public LineRenderer lineRenderer;
    public float laserWidth = 0.05f;
    public float laserMaxLength = 3f;

    private RaycastDetector _lastDetectorHit;

    public float waitBetweenDetections = 1f;
    private bool _canDetect = true;

    public bool enableLine;

    public GameObject optionsMenu;

    private bool _leftHand;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (hand.PointerPose == null || handSkeleton.Bones.Count == 0)
        {
            yield return null;
        }

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        if (!enableLine)
            lineRenderer.enabled = false;

        if (hand.gameObject.transform.parent.name == "LeftHandAnchor")
            _leftHand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.PointerPose == null || handSkeleton.Bones.Count == 0)
            return;

        /*Options Menu Stuff*/
        if (optionsMenu != null && optionsMenu.activeSelf)
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;

            return;
        }

        if (!lineRenderer.enabled && enableLine)
            lineRenderer.enabled = true;
        /*//////////////////*/

        Transform startTransform = hand.PointerPose;
        Vector3 worldPosition = transform.TransformPoint(startTransform.position);
        Vector3 rayDirection = hand.PointerPose.forward;
        Vector3 rayWorldDirection = transform.TransformDirection(rayDirection);

        // Raycast
        RaycastHit hit;
        Ray ray = new Ray(worldPosition, rayWorldDirection);

        Physics.Raycast(ray, out hit, laserMaxLength);
        Vector3 endPosition;

        if (hit.collider)
        {
            bool triggerPressed;

            if (!_leftHand)
                triggerPressed = OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Hands);
            else
                triggerPressed = OVRInput.Get(OVRInput.Button.Three, OVRInput.Controller.Hands);

            RaycastDetector hitDetector = hit.collider.gameObject.GetComponent<RaycastDetector>();
            if (hitDetector != null)
            {
                if (_lastDetectorHit != hitDetector)
                {
                    if (_lastDetectorHit != null)
                        _lastDetectorHit.RaycastExit();

                    _lastDetectorHit = hitDetector;
                    _lastDetectorHit.RaycastEnter();
                }

                if (triggerPressed && _canDetect)
                {
                    _canDetect = false;
                    StartCoroutine(WaitUntilNextDetection());
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
            endPosition = worldPosition + rayWorldDirection * laserMaxLength;

            if (_lastDetectorHit != null)
            {
                _lastDetectorHit.RaycastExit();
                _lastDetectorHit = null;
            }
        }



        // Draw Pointer Line
        if(enableLine)
            DrawLine(worldPosition + rayWorldDirection * .05f, endPosition);
    }

    IEnumerator WaitUntilNextDetection()
    {
        yield return new WaitForSeconds(waitBetweenDetections);
        _canDetect = true;
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void EnableLine()
    {
        lineRenderer.enabled = true;
        enableLine = true;
    }

    public void DisableLine()
    {
        lineRenderer.enabled = false;
        enableLine = false;
    }
}
