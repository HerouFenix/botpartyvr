using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDraggable : MonoBehaviour
{
    public Rigidbody cubeRB;
    public Rigidbody pedestal;
    HeightAdjuster _hj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DraggableCube")
        {


            OVRGrabbable grabbable = cubeRB.GetComponent<OVRGrabbable>();

            if(grabbable.grabbedBy != null)
            {
                if (grabbable.grabbedBy is HandGrabber)
                {
                    ((HandGrabber)grabbable.grabbedBy)._grabbed = false;
                }
                grabbable.grabbedBy.ForceRelease(grabbable);
            }


            cubeRB.velocity = Vector3.zero;
            cubeRB.angularVelocity = Vector3.zero;
            cubeRB.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            Vector3 newPos = new Vector3(pedestal.gameObject.transform.position.x, pedestal.gameObject.transform.position.y + 0.2f, pedestal.gameObject.transform.position.z);
            cubeRB.transform.position = newPos;
            cubeRB.gameObject.transform.position = newPos;
        }
    }
}
