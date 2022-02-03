using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class ControllerGrabber : OVRGrabber
{
    public Outline grabbableOutline;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(m_grabCandidates.Count > 0 && !grabbableOutline.enabled)
        {
            grabbableOutline.enabled = true;
        }
        else if(m_grabCandidates.Count <= 0 && grabbableOutline.enabled)
        {
            grabbableOutline.enabled = false;
        }
    }
}
