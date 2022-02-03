using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct HandGesture
{
    public string name;
    public List<Vector3> fingerPositionData;
    public List<Vector3> fingerVecData;
    public UnityEvent onRecognized;
}

public class HandGestureRecognition : MonoBehaviour
{
    public OVRHand hand;
    public OVRSkeleton handSkeleton;
    private List<OVRBone> _fingerBones;
    public float posThreshold = 0.01f;
    public float vecThreshold = 0.01f;

    public float waitBetweenDetections = 1.5f;
    private bool _canDetect = true;
    
    public List<HandGesture> validGestures;


    public bool debugMode = false; // Allows to register gestures
    private int _currentNewGesture = 0;

    private HandGesture _previousGesture;

    public TextMesh debugText;

    public LineRenderer lineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    private HitColliderButton _lastDetectorHit_Options;

    public GameObject optionsMenu;

    public bool forMenuUsageOnly;

    private bool _leftHand;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        while (hand.PointerPose == null || handSkeleton.Bones.Count == 0)
        {
            yield return null;
        }

        _fingerBones = new List<OVRBone>(handSkeleton.Bones);
        //Debug.Log("We got " + _fingerBones.Count + " bones!");

        _previousGesture = new HandGesture();

        if (hand.gameObject.transform.parent.name == "LeftHandAnchor")
            _leftHand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hand.PointerPose == null || handSkeleton.Bones.Count == 0 || _fingerBones == null)
            return;

        /*Options Menu Stuff*/
        if (optionsMenu != null && optionsMenu.activeSelf)
        {
            if (!lineRenderer.enabled)
                lineRenderer.enabled = true;

            OptionsMenuRaycast();
            return;
        }
        if (_lastDetectorHit_Options != null)
        {
            _lastDetectorHit_Options.RaycastExit();
            _lastDetectorHit_Options = null;
        }

        if (lineRenderer.enabled)
            lineRenderer.enabled = false;
        /*//////////////////*/

        if (!_canDetect)
            return;

        if (debugMode && Input.GetKeyDown(KeyCode.S))
        { // Register new Gesture
            RegisterGesture();
        }


        HandGesture currentGesture = RecognizeGesture();
        bool hasRecognized = !currentGesture.Equals(new HandGesture());

        if (!currentGesture.Equals(_previousGesture))
        {
            if (hasRecognized)
            {
                if (forMenuUsageOnly && !(currentGesture.name == "L_Options" || currentGesture.name == "R_Options"))
                    return;

                currentGesture.onRecognized.Invoke();
            }

            _previousGesture = currentGesture;

            _canDetect = false;
            StartCoroutine(WaitUntilNextDetection());

            if (debugMode)
                ChangeDebugText(_previousGesture.name);
        }

    }

    IEnumerator WaitUntilNextDetection()
    {
        yield return new WaitForSeconds(waitBetweenDetections);
        _canDetect = true;
    }

    void RegisterGesture()
    {
        HandGesture g = new HandGesture();
        g.name = "New Gesture " + _currentNewGesture;
        _currentNewGesture++;

        // Finger Position
        List<Vector3> posData = new List<Vector3>();
        foreach (var bone in _fingerBones)
        {
            // Finger position relative to root
            posData.Add(handSkeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        g.fingerPositionData = posData;

        // Finger Vector
        List<Vector3> vecData = new List<Vector3>();
        foreach (var bone in _fingerBones)
        {
            // Finger position relative to root
            vecData.Add(handSkeleton.transform.InverseTransformDirection(bone.Transform.position));
        }
        g.fingerVecData = vecData;

        validGestures.Add(g);

        Debug.Log("Gesture Registered");
    }

    void ChangeDebugText(string name)
    {
        if (debugText != null)
            debugText.text = name;
    }

    public HandGesture RecognizeGesture()
    {
        HandGesture recognizedGesture = new HandGesture();
        float currentMinDist = Mathf.Infinity;

        foreach (var handGesture in validGestures)
        {
            float sumDistance = 0;
            bool discard = false;
            for (int i = 0; i < _fingerBones.Count; i++)
            { // Compare each of the current hand position's fingers with the corresponding fingers in this valid hand gesture

                // Compare Pos
                Vector3 currentPosData = handSkeleton.transform.InverseTransformPoint(_fingerBones[i].Transform.position);
                float posDistance = Vector3.Distance(currentPosData, handGesture.fingerPositionData[i]);

                if (posDistance > posThreshold)
                { // If any position is too different from the valid gesture, then outright discard this gesture as possible
                    discard = true;
                    break;
                }

                sumDistance += posDistance;

                // Compare Dir
                Vector3 currentVecData = handSkeleton.transform.InverseTransformDirection(_fingerBones[i].Transform.position);
                float vecDistance = Vector3.Distance(currentVecData, handGesture.fingerVecData[i]);

                if (vecDistance > vecThreshold)
                { // If any position is too different from the valid gesture, then outright discard this gesture as possible
                    discard = true;
                    break;
                }

                sumDistance += vecDistance;
            }

            if (!discard && sumDistance < currentMinDist)
            { // If the total difference between the valid gesture and the current hand position is smaller than any previous guess, then this valid gesture is the current best guess
                currentMinDist = sumDistance;
                recognizedGesture = handGesture;
            }
        }

        return recognizedGesture;
    }


    public void OpenOptions()
    {
        if (optionsMenu != null)
        {
            optionsMenu.SetActive(true);
        }
    }

    void OptionsMenuRaycast()
    {
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
                triggerPressed = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Hands);
            else
                triggerPressed = OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Hands);


            HitColliderButton hitDetector = hit.collider.gameObject.GetComponent<HitColliderButton>();
            if (hitDetector != null)
            {
                if (_lastDetectorHit_Options != hitDetector)
                {
                    if (_lastDetectorHit_Options != null)
                        _lastDetectorHit_Options.RaycastExit();

                    _lastDetectorHit_Options = hitDetector;
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
            endPosition = worldPosition + rayWorldDirection * laserMaxLength;

            if (_lastDetectorHit_Options != null)
            {
                _lastDetectorHit_Options.RaycastExit();
                _lastDetectorHit_Options = null;
            }
        }



        // Draw Pointer Line
        DrawLine(worldPosition + rayWorldDirection * .05f, endPosition);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

}
