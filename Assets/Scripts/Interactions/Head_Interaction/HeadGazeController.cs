using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadGazeController : MonoBehaviour
{
    /* This class can be used for Controller Pointing Interactions with physical objects (i.e not Canvas elements)*/
    public Camera centerEye;


    public bool enableLine;

    [SerializeField] private Transform _reticle;

    private RaycastDetector _lastDetectorHit;
    private HitColliderButton _lastDetectorHit_Options;


    public GameObject optionsMenu;



    // Start is called before the first frame update
    void Start()
    {
        
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

            OptionsMenuRaycast();

            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
            { // Options Menu
                optionsMenu.SetActive(false);
            }

            return;
        }

        if (optionsMenu != null && (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick)))
        { // Options Menu
            optionsMenu.SetActive(true);
            return;
        }


        /*//////////////////*/

        Vector3 recticlePosition = _reticle.position;


        Transform startTransform = centerEye.transform;
        Vector3 rayDirection = centerEye.transform.forward;

        // Raycast
        RaycastHit hit;

        Physics.Raycast(startTransform.position, rayDirection, out hit, 100.0f);

        float distance;

        if (hit.collider)
        {
            bool triggerPressed = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.02f || OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.02f || OVRInput.GetDown(OVRInput.Button.One);
            distance = hit.distance;


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
        else
        {
            //distance = centerEye.farClipPlane * 0.95f;
            distance = 10f;

            if (_lastDetectorHit != null)
            {
                _lastDetectorHit.RaycastExit();
                _lastDetectorHit = null;
            }
        }


        // Draw Pointer Line
        Vector3 endPosition = startTransform.position + rayDirection * distance;
        if ((recticlePosition - endPosition).magnitude > 0.0001)
        {
            _reticle.position = centerEye.transform.position + centerEye.transform.forward * distance;
        }
        _reticle.rotation = centerEye.transform.rotation;
        _reticle.localScale = Vector3.one * distance;

    }

    void OptionsMenuRaycast()
    {
        Vector3 recticlePosition = _reticle.position;

        Transform startTransform = centerEye.transform;
        Vector3 rayDirection = centerEye.transform.forward;

        // Raycast
        RaycastHit hit;
        Physics.Raycast(startTransform.position, rayDirection, out hit, 100.0f);

        float distance;

        if (hit.collider)
        {
            bool triggerPressed = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.02f || OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.02f || OVRInput.GetDown(OVRInput.Button.One);
            distance = hit.distance;


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

        }
        else
        {
            distance = centerEye.farClipPlane * 0.95f;

            if (_lastDetectorHit_Options != null)
            {
                _lastDetectorHit_Options.RaycastExit();
                _lastDetectorHit_Options = null;
            }
        }



        // Draw Pointer Line
        Vector3 endPosition = startTransform.position + rayDirection * distance;
        if ((recticlePosition - endPosition).magnitude > 0.0001)
        {
            _reticle.position = centerEye.transform.position + centerEye.transform.forward * distance;
        }
        _reticle.rotation = centerEye.transform.rotation;
        _reticle.localScale = Vector3.one * distance;
    }







    public void EnableLine()
    {
        enableLine = true;
    }
    public void DisableLine()
    {
        enableLine = false;

        if (_lastDetectorHit != null)
        {
            _lastDetectorHit.RaycastExit();
            _lastDetectorHit = null;
        }
    }
}
