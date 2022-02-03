
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadGestureRecognizer : MonoBehaviour
{
    [SerializeField] private Camera _cameraToMeasure;

    [SerializeField]
    [Tooltip("The minimum difference in quaternion readings to trigger a gesture check.")]
    private float _deltaThreshold = 0.015f;

    [SerializeField]
    [Tooltip("The minimum amount of time that each gesture motion must be from the previous to be read as a gesture.")]
    private float _timeThreshold = 1.0f;

    [SerializeField]
    [Tooltip("The minimum amout of passing gesture checks that must occur to create a valid gesture.")]
    private int _gestureAmountThreshold = 3;


    [SerializeField] private UnityEvent _onNod;
    [SerializeField] private UnityEvent _onShake;

    private Gesture _yes = new Gesture(), _no = new Gesture();

    private class Gesture
    {
        float _lastVelocity = 0.0f;
        bool _negDirection = false;
        int _headDirectionChanges = 0;
        float _timeSinceLast = 0.0f;
        float _lastMeasurement = 0.0f;

        /// <summary>
        /// Checks to see if this gestur has been completed.
        /// </summary>
        /// <param name="newMeasurement">The new measurement data to compare against the previous measurement data.</param>
        /// <param name="deltaThreshold">The threshold for difference in measurement. Any difference less than this is considered noise.</param>
        /// <param name="timeThreshold">The minimum amount of time that each gesture motion must be from the previous to be read as a gesture.</param>
        /// <param name="gestureAmountThreshold">The minimum amout of passing gesture checks that must occur to create a valid gesture.</param>
        /// <returns></returns>
        public bool CheckGesture(float newMeasurement, float deltaThreshold, float timeThreshold, int gestureAmountThreshold)
        {
            bool completedGesture = false;

            float newVelocity = (newMeasurement - _lastMeasurement) / Time.fixedDeltaTime;
            if (Mathf.Abs(newMeasurement - _lastMeasurement) > deltaThreshold)
            {
                if ((_lastVelocity < 0 && newVelocity < 0 && !_negDirection) || (_lastVelocity > 0 && newVelocity > 0 && _negDirection))
                {
                    if (_timeSinceLast < timeThreshold)
                    {
                        _headDirectionChanges++;
                        if (_headDirectionChanges >= gestureAmountThreshold)
                        {
                            _headDirectionChanges = 0;
                            completedGesture = true;
                        }
                    }
                    else
                    {
                        _headDirectionChanges = 0;
                        completedGesture = false;

                    }
                    _timeSinceLast = 0.0f;
                    _negDirection = (_lastVelocity < 0 && newVelocity < 0);
                }
            }

            _lastMeasurement = newMeasurement;
            _lastVelocity = newVelocity;
            _timeSinceLast += Time.deltaTime;

            return completedGesture;
        }

    }

    void FixedUpdate()
    {
        // Using fixed update to reduce the number of verifications per second

        if (_yes.CheckGesture(_cameraToMeasure.transform.rotation.x, _deltaThreshold, _timeThreshold, _gestureAmountThreshold))
            _onNod.Invoke();

        if (_no.CheckGesture(_cameraToMeasure.transform.rotation.y, _deltaThreshold, _timeThreshold, _gestureAmountThreshold))
            _onShake.Invoke();
    }
}

