using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToEvent : SceneEvent {

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _slowRadius = 1.5f;
    [SerializeField]
    private float _radiusToReachTarget = 0.1f;

    [SerializeField]
    private Transform _endTarget;

    [SerializeField]
    private Transform[] _path;
    private uint _currentTargetIdx = 0;

    private bool _eventOver = false;

    private void Start() {
        if (_path.Length == 0) throw new MissingReferenceException("MoveToEvent has an empty path!");
    }

    public override void DoEvent() {

        if (ReachedEndOfPath()) {
            if (_endTarget != null) {
                Vector3 toEnd = _endTarget.position - transform.position;
                toEnd.y = 0.0f;
                transform.LookAt(transform.position - toEnd.normalized);
            }
            _eventOver = true;
        } else {
            Vector3 currentTarget = _path[_currentTargetIdx].position;

            if (MoveToTarget(currentTarget)) {
                ++_currentTargetIdx;
            }    
        }    
    }

    private bool MoveToTarget(Vector3 target) {

        Vector3 movementDir = (target - transform.position);

        float movementSpeed = _movementSpeed;

        float distanceToTarget = movementDir.magnitude;

        if (distanceToTarget < _radiusToReachTarget)
        {
            return true;
        }

        if (_currentTargetIdx == _path.Length - 1 && distanceToTarget < _slowRadius)
        {
            movementSpeed = _movementSpeed * (distanceToTarget / _slowRadius);
        }

        movementDir.y = 0.0f;
        Vector3 movement = movementDir.normalized * movementSpeed * Time.deltaTime;
        // Forward is inverted in CRTs so we flip the forward here to fix the issue
        transform.LookAt(transform.position - movement);
        transform.position += movement;

        return false;
    }

    private bool ReachedEndOfPath() => _currentTargetIdx == _path.Length;

    public override bool IsEventOver() => _eventOver;



}