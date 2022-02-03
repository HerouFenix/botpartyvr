using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BinaryQuestion
{
    [TextArea(2, 10)] public string question;

    public AudioClip questionClip;

    [TextArea(2, 10)] public string answerA;

    public AudioClip answerAClip;

    public bool answerAIsHappy;

    [TextArea(2, 10)] public string answerB;

    public AudioClip answerBClip;

    public bool answerBIsHappy;
}

/// <summary>
/// Represents a single dialog or conversation with binary questions
/// Broadcasts a GameEvent once the dialog finished
/// </summary>
[CreateAssetMenu]
public class Dialog : ScriptableObject
{
    private class DialogOverException : Exception
    {
        public DialogOverException(string message) : base(message) {}
    }

    [SerializeField] private GameEvent _onDialogFinishEvent;
    [SerializeField] private BinaryQuestion[] _questions;
    private uint _currQuestionIdx = 0;

    public bool IsDialogOver() => _currQuestionIdx == _questions.Length;
    
    public void RestartDialog()
    {
        _currQuestionIdx = 0;
    }

    public BinaryQuestion GetQuestion()
    {
        if (IsDialogOver())
        {
            throw new DialogOverException("Dialog is over therefore you cannot get the current question!");
        }

        return _questions[_currQuestionIdx];
    }

    public string Answer(bool answer)
    {
        if (IsDialogOver())
        {
            throw new DialogOverException("Dialog is over therefore you cannot answer a question!");
        }

        BinaryQuestion question = _questions[_currQuestionIdx];

        AdvanceQuestion();

        return answer ? question.answerA : question.answerB;
    }

    public void MoveToPreviousQuestion()
    {
        if (_currQuestionIdx > 0)
            --_currQuestionIdx;
    }

    private void AdvanceQuestion()
    {
        ++_currQuestionIdx;

        if (IsDialogOver())
        {
            FinishDialog();
        }
    }

    private void FinishDialog()
    {
        if (_onDialogFinishEvent != null) {
            _onDialogFinishEvent.Raise();
        }
    }
}
