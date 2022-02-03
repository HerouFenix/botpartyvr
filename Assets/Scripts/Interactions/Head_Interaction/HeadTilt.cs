using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadTilt : MonoBehaviour
{
    public GameObject _anchor;
    [SerializeField] private float _minAngle = 1.0f;
    [SerializeField] private float _maxAngle = 5.0f;
    [SerializeField] private float _minTime = 1.0f; // Min time between each key point of the movement (Start, Edge, Start) to validate it
    private float _startTime = 0.0f;
    private float _edgeTime = 0.0f;
    private float _returnTime = 0.0f;
    private bool _edgeReached = false;
    private bool _tiltedRight = false;
    private bool _tiltedLeft = false;
    public UnityEvent leftAnswer;
    public UnityEvent rightAnswer;
    public float currentRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //_anchor = GameObject.Find("CenterEyeAnchor");
        if (_anchor == null) throw new MissingReferenceException("Couldn't find user");
        if (leftAnswer == null || rightAnswer == null) throw new MissingReferenceException("Both answer events need to be specified ");
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = _anchor.transform.rotation.eulerAngles.z;
        if(currentRotation > 180)
        {
            currentRotation = currentRotation - 360;
        }

        if(Math.Abs(currentRotation) < _minAngle && !_edgeReached)
        {
            _startTime = 0.0f;
            _edgeTime = 0.0f;
            _returnTime = 0.0f;
            
            return;
        }

        // Beggining of movement
        if(Math.Abs(currentRotation) > _minAngle && _startTime == 0.0f)
        {
            _startTime = Time.time;
        }
        
        // Edge trigger detection
        if(Math.Abs(currentRotation) > _maxAngle && _startTime != 0.0f && !_edgeReached)
        {
            _edgeTime = Time.time;
            _edgeReached = true;

            if(currentRotation > 0.0f)
            {
                /*_tiltedRight = true;
                _tiltedLeft = false;*/
                if (_edgeTime - _startTime < _minTime)
                {
                    leftAnswer.Invoke();
                }

            }
            else
            {
                /*_tiltedLeft = true;
                _tiltedRight = false;*/
                if(_edgeTime - _startTime < _minTime)
                {
                    rightAnswer.Invoke();
                }


            }
        }
        
        // End of movement
        if (Math.Abs(currentRotation) < _minAngle && _edgeReached)
        {
            _edgeReached = false;
        }

        
    }
}
