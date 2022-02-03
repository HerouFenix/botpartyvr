using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Variant of the DialogManager
/// Does not assume there are buttons to click on
/// </summary>
public class DialogManagerNoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _question;
    [SerializeField] private GameObject _buttonA;
    [SerializeField] private GameObject _buttonB;

    [SerializeField] private Dialog _currentDialog;

    private bool _waitingForAnswer = false;

    public bool startWithDisplayOn = true;

    public UnityEvent onShowEvents;
    public UnityEvent onHideEvents;

    public void DisplayDialog(Dialog dialog)
    {
        _currentDialog = dialog;
        ShowUI();
        UpdateDisplayedQuestion();
    }

    private void Start()
    {
        if (_question == null) throw new MissingReferenceException("The dialog manager is missing the Question UI element");

        if (_currentDialog != null)
        {
            _currentDialog = FindObjectOfType<Dialog>();
            UpdateDisplayedQuestion();

            if (startWithDisplayOn)
                ShowUI();
            else
                HideUI();
        } else
        {
            HideUI();
        }
    }

    public void AnswerA()
    {
        if (_waitingForAnswer)
        {
            _currentDialog.Answer(true);
            UpdateDisplayedQuestion();
        }
    }

    public void AnswerB()
    {
        if (_waitingForAnswer)
        {
            _currentDialog.Answer(false);
            UpdateDisplayedQuestion();
        }
    }

    private void UpdateDisplayedQuestion()
    {
        if (_currentDialog.IsDialogOver())
        {
            HideUI();
            return;
        } 

        BinaryQuestion currentQuestion = _currentDialog.GetQuestion();

        _question.text = currentQuestion.question;
    }

    private void HideUI()
    {
        _question.gameObject.SetActive(false);
        if(_buttonA != null)
            _buttonA.gameObject.SetActive(false);
        if (_buttonB != null)
            _buttonB.gameObject.SetActive(false);

        onHideEvents.Invoke();
        

        _waitingForAnswer = false;
    }

    private void ShowUI()
    {
        _question.gameObject.SetActive(true);
        if (_buttonA != null)
            _buttonA.gameObject.SetActive(true);
        if (_buttonB != null)
            _buttonB.gameObject.SetActive(true);


        onShowEvents.Invoke();
        

        _waitingForAnswer = true;
    }
}
