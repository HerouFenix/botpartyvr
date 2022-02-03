using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class HandGrabber : OVRGrabber
{
    private OVRHand m_hand;
    private float pinchThreshold = 0.7f;
    public string handName; //for debug
    [HideInInspector] public bool _grabbed;

    protected override void Start()
    {
        base.Start();
        m_hand = GetComponent<OVRHand>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        CheckIndexPinch();
    }

    void CheckIndexPinch()
    {
        float pinchStrength = m_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

        if (!m_grabbedObj && pinchStrength > pinchThreshold && m_grabCandidates.Count > 0 && !_grabbed)
        {
            Debug.Log("Grabbed" + handName);
            GrabBegin();
            _grabbed = true;
        }
        else if ((m_grabbedObj && !(pinchStrength > pinchThreshold)) || (!m_grabbedObj && _grabbed))
        {
            Debug.Log("Dropped " + handName);
            GrabEnd();
            _grabbed = false;
        }

        //if(m_grabbedObj != null)
        //    Debug.Log(((HandGrabber)grabbedObject.grabbedBy).handName);
    }
}