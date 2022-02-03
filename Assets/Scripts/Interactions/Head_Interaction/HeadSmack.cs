using Assets.Scripts.Other;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactions.Head_Interaction
{
    public class HeadSmack : MonoBehaviour
    {
        [SerializeField] private GameObject _centerEye;
        private Vector3 _previousPosition;
        private Vector3 _currentVelocity;
        [SerializeField] private GameObject _yesTarget;
        [SerializeField] private GameObject _noTarget;

        [SerializeField] private UnityEvent _onYes;
        [SerializeField] private UnityEvent _onNo;

        [SerializeField] private float _springStrength = 1.0f;

        private bool _hitYes = false;
        private float _xYes = 0.0f;
        private Vector3 _startPositionYes;
        private Vector3 _targetPositionYes;
        private bool _hitNo = false;
        private float _xNo = 0.0f;
        private Vector3 _startPositionNo;
        private Vector3 _targetPositionNo;

        private void Start()
        {
            _previousPosition = _centerEye.transform.position;
        }

        void Update()
        {
            _currentVelocity = (_centerEye.transform.position - _previousPosition) / Time.deltaTime;
            _previousPosition = _centerEye.transform.position;

            if (_hitYes)
            {
                _xYes += Mathf.Clamp(_springStrength * Time.deltaTime, 0.0f, 1.0f);
                _yesTarget.transform.position = Vector3.Lerp(_targetPositionYes, _startPositionYes, SpringFunction(_xYes));
                if (_xYes >= 1.0f)
                {
                    _hitYes = false;
                }
            }

            if (_hitNo)
            {
                _xNo += Mathf.Clamp(_springStrength * Time.deltaTime, 0.0f, 1.0f);
                _noTarget.transform.position = Vector3.Lerp(_targetPositionNo, _startPositionNo, SpringFunction(_xNo));
                if (_xNo >= 1.0f)
                {
                    _hitNo = false;
                }
            }

            // Place head smack targets at the height of the head
            Vector3 yesPosition = _yesTarget.transform.position;
            Vector3 noPosition = _noTarget.transform.position;
            yesPosition.y = _centerEye.transform.position.y;
            noPosition.y = _centerEye.transform.position.y;

            _yesTarget.transform.position = yesPosition;
            _noTarget.transform.position = noPosition;
        }

        private void FixedUpdate()
        { 
            if (!_hitYes && Physics.CheckSphere(_yesTarget.transform.position, 0.2f))
            {
                _onYes.Invoke();
                _xYes = 0.0f;
                _startPositionYes = _yesTarget.transform.position;
                _targetPositionYes = _startPositionYes + 
                    _currentVelocity.normalized * Mathf.Min(8.0f, _currentVelocity.magnitude);
                _hitYes = true;
            }

            if (!_hitNo && Physics.CheckSphere(_noTarget.transform.position, 0.2f))
            {
                _onNo.Invoke();
                _xNo = 0.0f;
                _startPositionNo = _noTarget.transform.position;
                _targetPositionNo = _startPositionNo +
                    _currentVelocity.normalized * Mathf.Min(8.0f, _currentVelocity.magnitude);
                _hitNo = true;
            }
        }

        private float SpringFunction(float x)
        {
            x = Mathf.Clamp(x, 0.0f, 1.0f);
            return Mathf.Pow((x - 0.5f), 2.0f) * 4.0f;
        }
    }
}