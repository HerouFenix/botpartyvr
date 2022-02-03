using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadzone = .025f;
    
    
    public UnityEvent onPressed, onReleased;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public ControllerHaptics leftHaptics;
    public ControllerHaptics rightHaptics;

    public bool useHaptics;


    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(_startPos.x, transform.localPosition.y > _startPos.y ? _startPos.y : transform.localPosition.y, _startPos.z);

        if(!_isPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }

        if (_isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadzone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();

        // Do Haptic Feedback

        Debug.Log("Pressed");
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!useHaptics)
            return;

        if (collision.gameObject.transform.parent == null || collision.gameObject.transform.parent.parent == null)
            return;

        var grandparentName = collision.gameObject.transform.parent.parent.name;
        if(grandparentName == "RightHandAnchor")
        {
            rightHaptics.ContinuousHapticFeedback(.02f, .05f);
        }else if(grandparentName == "LeftHandAnchor")
        {
            leftHaptics.ContinuousHapticFeedback(.02f, .05f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!useHaptics)
            return;

        if (collision.gameObject.transform.parent == null || collision.gameObject.transform.parent.parent == null)
            return;

        var grandparentName = collision.gameObject.transform.parent.parent.name;
        if (grandparentName == "RightHandAnchor")
        {
            rightHaptics.StopContinuousHapticFeedback();
        }
        else if (grandparentName == "LeftHandAnchor")
        {
            leftHaptics.StopContinuousHapticFeedback();
        }
    }

}
