using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public OVRGrabber left;
    public OVRGrabber right;
    private Outline _outline;

    private void Start()
    {
        _outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((left.m_grabCandidates.Count > 0 || right.m_grabCandidates.Count > 0) && !_outline.enabled)
        {
            _outline.enabled = true;
        }
        else if(_outline.enabled && (left.m_grabCandidates.Count <= 0 && right.m_grabCandidates.Count <= 0))
        {
            _outline.enabled = false;
        }
       
    }
}
