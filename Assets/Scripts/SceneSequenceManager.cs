using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any event in the scene
/// </summary>
public abstract class SceneEvent : MonoBehaviour {

    public virtual void StartEvent() {}

    public virtual void DoEvent() {}

    public virtual void FinishEvent() {}

    public abstract bool IsEventOver();
}

[Serializable]
public struct Event {
    public string name;
    public SceneEvent sceneEvent;
}


/// <summary>
/// Knows which events happen in the scene and in what sequence
/// </summary>
public class SceneSequenceManager : MonoBehaviour
{ 
    [SerializeField]
    private float _startDelay = 4.0f;

    private bool _playScene = false;

    [SerializeField]
    private bool _startScene = false;


    [SerializeField]
    private string _currentScene = "No Scene";

    [SerializeField]
    public Event[] _sequenceOfEvents; 
    public uint _currentEventIdx = 0u;
    
    public void StartScene() {
        _playScene = true;
        _sequenceOfEvents[_currentEventIdx].sceneEvent.StartEvent();
    }
    public bool IsSceneOver() => _currentEventIdx == _sequenceOfEvents.Length;

    private void Update() {

        if (_startScene) {
            Invoke("StartScene", _startDelay);
            _startScene = false;
        }
    
        if (_playScene && !IsSceneOver()) {

            Event currentEvent = _sequenceOfEvents[_currentEventIdx];
            _currentScene = currentEvent.name;

            if (!currentEvent.sceneEvent.IsEventOver()) {
                currentEvent.sceneEvent.DoEvent();
            } else {
                currentEvent.sceneEvent.FinishEvent();
                ++_currentEventIdx;

                if (!IsSceneOver()) {
                    _sequenceOfEvents[_currentEventIdx].sceneEvent.StartEvent();
                }
            }
        } else {
            _currentScene = "No Scene";
        } 
    }
}
