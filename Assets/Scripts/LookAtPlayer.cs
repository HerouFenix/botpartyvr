using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject _playerEyes;
    private SceneSequenceManager _sceneSequenceManager;
    public float rotationSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        _sceneSequenceManager = FindObjectOfType<SceneSequenceManager>();
        _playerEyes = GameObject.Find("CenterEyeAnchor");

        if (_playerEyes == null) throw new MissingReferenceException("Player not defined");
        if (_sceneSequenceManager == null) throw new MissingReferenceException("Scene Sequence Manager not defined");

    }

    // Update is called once per frame
    void Update()
    {
        var thisEvent = _sceneSequenceManager._sequenceOfEvents[_sceneSequenceManager._currentEventIdx].sceneEvent;

        if ( thisEvent is DialogEvent && thisEvent.transform.gameObject == this.gameObject)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-1 * (_playerEyes.transform.position - transform.position));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.time * rotationSpeed);
        }
    }
}
