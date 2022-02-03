using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineGrabbable : OVRGrabbable
{
    private Outline _outline;

    protected override void Start()
    {
        base.Start();
        _outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(isGrabbed);
        if ((isGrabbed) && !_outline.enabled)
        {
            _outline.enabled = true;
        }else if (!isGrabbed && _outline.enabled)
        {
            _outline.enabled = false;
        }
    }
}