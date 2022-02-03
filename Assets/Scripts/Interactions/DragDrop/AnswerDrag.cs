using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnswerDrag : MonoBehaviour
{
    public Rigidbody cubeRB;
    [SerializeField] private UnityEvent _onCollide;

    HeightAdjuster _hj;

    private Vector3 _startPos;

    IEnumerator Start()
    {

        _hj = cubeRB.gameObject.GetComponent<HeightAdjuster>();
        if (_hj != null)
        {
            while (!_hj.hasBeenAdjusted)
            {
                yield return null;
            }
        }

        _startPos = cubeRB.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DraggableCube")
        {
            //cubeRB.position = _startPos;
            //cubeRB.gameObject.transform.position = _startPos;

            _onCollide.Invoke();

            OVRGrabbable grabbable = cubeRB.GetComponent<OVRGrabbable>();

            if (grabbable.grabbedBy != null)
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

            cubeRB.position = _startPos;
            cubeRB.gameObject.transform.position = _startPos;

            

        }
    }
}
