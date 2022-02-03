using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static OVRHand;

public class HandPointing : MonoBehaviour
{
    /* This class can be used for Hand Pointing Interactions with physical objects (i.e not Canvas elements)*/

    public OVRHand hand;
    public OVRSkeleton handSkeleton;
    private OVRBone _indexFinger;
    private OVRBone _previousIndexFingerBone;

    public bool useIndexPosition = true;

    public LineRenderer lineRenderer;
    public float laserWidth = 0.05f;
    public float laserMaxLength = 3f;

    private RaycastDetector _lastDetectorHit;

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

        foreach (var bone in handSkeleton.Bones)
        {
            Debug.Log(bone.Id);
            if (bone.Id == OVRSkeleton.BoneId.Hand_Index2)
            {
                _previousIndexFingerBone = bone;
            }

            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                _indexFinger = bone;
            }
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
        if (hand.PointerPose == null || handSkeleton.Bones.Count == 0 || _indexFinger == null)
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
        if (!lineRenderer.enabled)
            return;

        Transform startTransform = useIndexPosition ? _indexFinger.Transform : hand.PointerPose;
        Vector3 worldPosition = useIndexPosition ? startTransform.position : transform.TransformPoint(startTransform.position);
        Vector3 rayDirection = useIndexPosition ? (_indexFinger.Transform.position - _previousIndexFingerBone.Transform.position).normalized : hand.PointerPose.forward;
        Vector3 rayWorldDirection = useIndexPosition ? rayDirection : transform.TransformDirection(rayDirection);


        // Raycast
        RaycastHit hit;
        Ray ray = new Ray(worldPosition, rayWorldDirection);

        Physics.Raycast(ray, out hit, laserMaxLength);
        Vector3 endPosition;

        if (hit.collider)
        {
            RaycastDetector hitDetector = hit.collider.gameObject.GetComponent<RaycastDetector>();
            if (hitDetector != null)
            {
                if(_lastDetectorHit != hitDetector)
                {
                    if(_lastDetectorHit != null)
                        _lastDetectorHit.RaycastExit();

                    _lastDetectorHit = hitDetector;
                    _lastDetectorHit.RaycastEnter();
                }

            }
            else
            {
                if(_lastDetectorHit != null)
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
        if (enableLine)
        {
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;
                DrawLine(worldPosition + rayWorldDirection * .05f, endPosition);
            }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }

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

        if (_lastDetectorHit != null)
        {
            _lastDetectorHit.RaycastExit();
            _lastDetectorHit = null;
        }
    }
}
