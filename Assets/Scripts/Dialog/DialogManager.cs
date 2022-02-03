using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DialogManager : MonoBehaviour
{
    private DialogEvent _currentDialogEvent;

    public UnityEvent onShowEvents;
    public UnityEvent onHideEvents;

    public AudioClip answerSelected;

    private bool _answered = false;

    public void SetDialogAsCurrent(DialogEvent dialogEvent)
    {
        _currentDialogEvent = dialogEvent;
    }

    public void AnswerA()
    {
        if (_currentDialogEvent != null && !_currentDialogEvent.IsEventOver()) {
            _currentDialogEvent.AnswerA();
            onHideEvents.Invoke();
        }

    }

    public void AnswerB()
    {
        if (_currentDialogEvent != null && !_currentDialogEvent.IsEventOver()) {
            _currentDialogEvent.AnswerB();
            onHideEvents.Invoke();
            _answered = true;
        }
    }

    private void Update()
    {
        if (_currentDialogEvent != null && _currentDialogEvent.IsEventOver())
        {
            _currentDialogEvent = null;
        }
    }
}