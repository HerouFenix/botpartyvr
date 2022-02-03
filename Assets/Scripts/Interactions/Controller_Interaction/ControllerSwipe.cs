using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Interactions.Controller_Interaction
{
    public class ControllerSwipe : MonoBehaviour
    {

        [SerializeField] private float _swipeDurationThreshold = 1.0f;
        [SerializeField] private float _swipeDistanceThreshold = 0.1f;

        [SerializeField] private GameObject _leftController;
        private Vector3 _previousLeftControllerPosition;
        private float _leftDistanceTravelled = 0.0f;
        
        [SerializeField] private GameObject _rightController;
        private Vector3 _previousRightControllerPosition;
        private float _rightDistanceTravelled = 0.0f;

        [SerializeField] private UnityEvent _onYes;
        [SerializeField] private UnityEvent _onNo;

        private float _currentSwipeTimer = 0.0f;

        [SerializeField] private TrailRenderer LTrail;
        [SerializeField] private TrailRenderer RTrail;

        [SerializeField] private Gradient YesColor;
        [SerializeField] private Gradient NoColor;
        [SerializeField] private Gradient defaultColor;


        void Start()
        {
            _previousLeftControllerPosition = _leftController.transform.position;
            _previousRightControllerPosition = _rightController.transform.position;

            LTrail.enabled = false;
            RTrail.enabled = false;

            LTrail.Clear();
            RTrail.Clear();
        }

        void Update()
        {

            bool swipeTimerThresholdPassed = _currentSwipeTimer <= _swipeDistanceThreshold;

            Vector3 currentRightPosition = _rightController.transform.position;
            Vector3 currentLeftPosition = _leftController.transform.position;

            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.02f) {

                //_leftDistanceTravelled = 0.0f;

                float xDisplacement = currentRightPosition.x - _previousRightControllerPosition.x;

                RTrail.enabled = true;

                if (Mathf.Sign(_rightDistanceTravelled) != Mathf.Sign(xDisplacement))
                {
                    _rightDistanceTravelled = xDisplacement;
                    _currentSwipeTimer = Time.deltaTime;

                }
                else
                {
                    _rightDistanceTravelled += xDisplacement;                    
                    _currentSwipeTimer += Time.deltaTime;
                    if (_rightDistanceTravelled >= _swipeDistanceThreshold && swipeTimerThresholdPassed)
                    {
                        _onYes.Invoke();
                        _currentSwipeTimer = 0.0f;
                        _rightDistanceTravelled = 0.0f;
                        _leftDistanceTravelled = 0.0f;

                        RTrail.colorGradient = YesColor;

                    }
                    else if (_rightDistanceTravelled <= -_swipeDistanceThreshold && swipeTimerThresholdPassed)
                    {
                        _onNo.Invoke();
                        _currentSwipeTimer = 0.0f;
                        _rightDistanceTravelled = 0.0f;
                        _leftDistanceTravelled = 0.0f;

                        RTrail.colorGradient = NoColor;

                    }
                }
            }
            else
            {
                RTrail.enabled = false;

                RTrail.Clear();

                RTrail.colorGradient = defaultColor;
            }

            if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.02f)
            {
                Debug.Log("Pressed Left");

                //_rightDistanceTravelled = 0.0f;

                float xDisplacement = currentLeftPosition.x - _previousLeftControllerPosition.x;

                LTrail.enabled = true;

                if (Mathf.Sign(_leftDistanceTravelled) != Mathf.Sign(xDisplacement))
                {
                    _leftDistanceTravelled = xDisplacement;
                    _currentSwipeTimer = Time.deltaTime;

                }
                else
                {
                    _leftDistanceTravelled += xDisplacement;
                    _currentSwipeTimer += Time.deltaTime;
                    if (_leftDistanceTravelled >= _swipeDistanceThreshold && swipeTimerThresholdPassed)
                    {
                        _onYes.Invoke();
                        _currentSwipeTimer = 0.0f;
                        _rightDistanceTravelled = 0.0f;
                        _leftDistanceTravelled = 0.0f;

                        LTrail.colorGradient = YesColor;

                    }
                    else if (_leftDistanceTravelled <= -_swipeDistanceThreshold && swipeTimerThresholdPassed)
                    {
                        _onNo.Invoke();
                        _currentSwipeTimer = 0.0f;
                        _rightDistanceTravelled = 0.0f;
                        _leftDistanceTravelled = 0.0f;

                        LTrail.colorGradient = NoColor;

                    }
                }

            }
            else
            {
                LTrail.enabled = false;

                LTrail.Clear();

                LTrail.colorGradient = defaultColor;
            }

            _previousRightControllerPosition = currentRightPosition;
            _previousLeftControllerPosition = currentLeftPosition;

            if (!swipeTimerThresholdPassed) {
                _currentSwipeTimer = 0.0f;
                _rightDistanceTravelled = 0.0f;
                _leftDistanceTravelled = 0.0f;
            }
        }



    }
}