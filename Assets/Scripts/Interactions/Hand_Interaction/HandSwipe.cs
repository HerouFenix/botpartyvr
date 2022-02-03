using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class HandSwipe : MonoBehaviour
{

    [SerializeField] private float _swipeDurationThreshold = 1.0f;
    [SerializeField] private float _swipeDistanceThreshold = 0.1f;

    [SerializeField] private OVRHand _leftHand;
    private Vector3 _previousLeftHandPosition;
    private float _leftDistanceTravelled = 0.0f;

    [SerializeField] private OVRHand _rightHand;
    private Vector3 _previousRightHandPosition;
    private float _rightDistanceTravelled = 0.0f;

    [SerializeField] private UnityEvent _onYes;
    [SerializeField] private UnityEvent _onNo;

    private float _currentRightSwipeTimer = 0.0f;
    private float _currentLeftSwipeTimer = 0.0f;


    [SerializeField] private TrailRenderer LTrail;
    [SerializeField] private TrailRenderer RTrail;

    [SerializeField] private Gradient yesColor;
    [SerializeField] private Gradient noColor;
    [SerializeField] private Gradient defaultColor;
    [SerializeField] private Color yesHandColor;
    [SerializeField] private Color noHandColor;
    [SerializeField] private Color defaultHandColor;

    [SerializeField] private bool debugMode;


    bool swipingWithRight = false;
    bool swipingWithLeft = false;

    [SerializeField] private float minDisplacement = 0.02f;

    OVRSkeleton rightSkeleton;
    OVRSkeleton leftSkeleton;

    bool swipeCooldown = false;
    float cooldownTime = 1.0f;
    float cooldownStartTime = 0.0f;

    [SerializeField] TextMeshProUGUI debugPosition;

    [SerializeField] TextMeshProUGUI debugText;

    void Start()
    {
        _previousLeftHandPosition = _leftHand.transform.position;
        _previousRightHandPosition = _rightHand.transform.position;

        rightSkeleton = _rightHand.GetComponent<OVRSkeleton>();
        leftSkeleton = _leftHand.GetComponent<OVRSkeleton>();

        if(debugMode)
        {
            debugPosition.transform.gameObject.SetActive(true);
            debugText.transform.gameObject.SetActive(true);

        }
    }

    void Update()
    {

        bool swipeTimerThresholdPassed = _currentRightSwipeTimer <= _swipeDurationThreshold || _currentLeftSwipeTimer <= _swipeDurationThreshold;

        Vector3 currentRightPosition = rightSkeleton.Bones[20].Transform.position;
        Vector3 currentLeftPosition = leftSkeleton.Bones[20].Transform.position;

        float xRightDisplacement = currentRightPosition.x - _previousRightHandPosition.x;

        if(LTrail != null)
            LTrail.transform.position = currentLeftPosition;
        if(RTrail != null)
            RTrail.transform.position = currentRightPosition;

        if(debugMode)
            debugPosition.text = "R:" + currentRightPosition + "\nL" + currentLeftPosition;

        if (swipeCooldown)
        {
            if(Time.time - cooldownStartTime >= cooldownTime)
            {
                swipeCooldown = false;

                _previousRightHandPosition = currentRightPosition;
                _previousLeftHandPosition = currentLeftPosition;

            }
            else return;
        }

        if (Math.Abs(xRightDisplacement) > minDisplacement || swipingWithRight)
        {
            if (Mathf.Sign(_rightDistanceTravelled) != Mathf.Sign(xRightDisplacement))
            {
                _rightDistanceTravelled = xRightDisplacement;
                _currentRightSwipeTimer = Time.deltaTime;
                swipingWithRight = false;
            }
            else
            {
                swipingWithRight = true;
                _rightDistanceTravelled += xRightDisplacement;
                _currentRightSwipeTimer += Time.deltaTime;
                if (_rightDistanceTravelled >= _swipeDistanceThreshold && swipeTimerThresholdPassed)
                {
                    _onYes.Invoke();
                    swipeCooldown = true;
                    cooldownStartTime = Time.time;
                    _currentLeftSwipeTimer = 0.0f;
                    _currentRightSwipeTimer = 0.0f;
                    _rightDistanceTravelled = 0.0f;
                    _leftDistanceTravelled = 0.0f;

                    if(RTrail != null)
                        RTrail.colorGradient = yesColor;

                    if (debugMode)
                        debugText.text = "Swiped right w R";

                }
                else if (_rightDistanceTravelled <= -_swipeDistanceThreshold && swipeTimerThresholdPassed)
                {
                    _onNo.Invoke();
                    swipeCooldown = true;
                    cooldownStartTime = Time.time;
                    _currentLeftSwipeTimer = 0.0f;
                    _currentRightSwipeTimer = 0.0f;
                    _rightDistanceTravelled = 0.0f;
                    _leftDistanceTravelled = 0.0f;

                    if(RTrail != null)
                       RTrail.colorGradient = noColor;

                    if (debugMode)
                        debugText.text = "Swiped left w R";
                }
            }
        }
        else
        { 
            if (RTrail != null)
                RTrail.colorGradient = defaultColor;
        }
           
        float xLeftDisplacement = currentLeftPosition.x - _previousLeftHandPosition.x;

        if (Math.Abs(xLeftDisplacement) > minDisplacement || swipingWithLeft)
        {
            if (Mathf.Sign(_leftDistanceTravelled) != Mathf.Sign(xLeftDisplacement))
            {
                _leftDistanceTravelled = xLeftDisplacement;
                _currentLeftSwipeTimer = Time.deltaTime;
                swipingWithLeft = false;
            }
            else
            {
                swipingWithLeft = true;
                _leftDistanceTravelled += xLeftDisplacement;
                _currentLeftSwipeTimer += Time.deltaTime;
                if (_leftDistanceTravelled >= _swipeDistanceThreshold && swipeTimerThresholdPassed)
                {
                    _onYes.Invoke();
                    swipeCooldown = true;
                    cooldownStartTime = Time.time;

                    _currentLeftSwipeTimer = 0.0f;
                    _currentRightSwipeTimer = 0.0f;
                    _rightDistanceTravelled = 0.0f;
                    _leftDistanceTravelled = 0.0f;

                    if (LTrail != null)
                        LTrail.colorGradient = yesColor;

                    if(debugMode)
                        debugText.text = "Swiped right w L";

                }
                else if (_leftDistanceTravelled <= -_swipeDistanceThreshold && swipeTimerThresholdPassed)
                {
                    _onNo.Invoke();
                    swipeCooldown = true;
                    cooldownStartTime = Time.time;

                    _currentLeftSwipeTimer = 0.0f;
                    _currentRightSwipeTimer = 0.0f;
                    _rightDistanceTravelled = 0.0f;
                    _leftDistanceTravelled = 0.0f;

                    if (LTrail != null)
                        LTrail.colorGradient = noColor;

                    if(debugMode)
                        debugText.text = "Swiped left w L";

                }
            }
        }
        else
        {
            if(LTrail != null) 
                LTrail.colorGradient = defaultColor;
        }

        _previousRightHandPosition = currentRightPosition;
        _previousLeftHandPosition = currentLeftPosition;

        if (!swipeTimerThresholdPassed)
        {
            _currentLeftSwipeTimer = 0.0f;
            _currentRightSwipeTimer = 0.0f;
            _rightDistanceTravelled = 0.0f;
            _leftDistanceTravelled = 0.0f;
            swipingWithRight = false;
            swipingWithLeft = false;
        }
    }

}
