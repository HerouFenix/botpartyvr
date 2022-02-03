using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the currently displayed dialog
/// Displays it on the UI
/// IMPORTANT: Assumes UI text is TextMeshPro
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class DialogEvent : SceneEvent
{
    private DialogManager _dialogManager;
    [SerializeField] private TextMeshProUGUI _question;
    [SerializeField] private GameObject _buttonA;
    [SerializeField] private GameObject _buttonB;
    [SerializeField] private GameObject _buttonABackground;
    [SerializeField] private GameObject _buttonBBackground;
    [SerializeField] private Dialog _dialog;

    private AudioSource _audioSource;
    private BinaryQuestion _currentQuestion;

    private List<string> _currentPhrases;
    private bool _displayingPhrases = false;
    private bool _questioning = false;
    private bool _answering = false;

    private AudioClip _previousClip;

    FacialExpression _face;
    private void Start()
    {
        if (_question == null) throw new MissingReferenceException("The dialog manager is missing the Question UI element");
        if (_dialog == null) throw new MissingReferenceException("A DialogEvent requires an associated Dialog to function!");
        _dialog.RestartDialog();
        _dialogManager = FindObjectOfType<DialogManager>();
        if (_dialogManager == null) throw new MissingReferenceException("There is no DialogManager in the scene!");
        _audioSource = GetComponent<AudioSource>();

        _face = GetComponentInChildren<FacialExpression>();
        if (_face == null) throw new MissingComponentException("Face was not found");
        _face.Happy();
        HideQuestionUI();
        HideAnswersUI();
    }

    public override void StartEvent() {
        _dialogManager.SetDialogAsCurrent(this);
        ShowQuestionUI();
        AskQuestion();
    }

    public override void DoEvent() {
        
        // Has the answer stopped playing?
        if (_answering && !_audioSource.isPlaying && !_displayingPhrases) {
            _answering = false;
            HideAnswersUI();
            ShowQuestionUI();
            AskQuestion();
        }


        // Has the question stopped playing
        if (_questioning && !_audioSource.isPlaying && !_displayingPhrases) {
            _questioning = false;
            _face.Happy();
            if (_currentQuestion.answerAClip != null) {
                ShowAnswersUI();
                //AnswerB(); // COMMENT THIS to make it not automatically answer
            } else {
                // Question does not have answers so advance to next by answering randomly
                AnswerB();
            }
        }
    }

    public override void FinishEvent() {
        HideQuestionUI();
        HideAnswersUI();
    }

    public override bool IsEventOver() => _dialog.IsDialogOver() && !_audioSource.isPlaying;


    public void AskQuestion() {
        _currentQuestion = _dialog.GetQuestion();
        _currentPhrases = CreatePhrases(_currentQuestion.question);
        StartCoroutine("SubtitlesRoutine");
        _audioSource.PlayOneShot(_currentQuestion.questionClip, 1.0f);
        _questioning = true;
        _face.Talking();
    }


    private uint _maxWordsPerPhrase = 10u;
    private List<string> CreatePhrases(string phrase) {
        string[] allPhrases = phrase.Split(' ');

        List<string> phrases = new List<string>();
        
        string currentPhrase = "";

        for (uint i = 0u; i < allPhrases.Length; ++i) {
            currentPhrase += allPhrases[i] + " ";

            if (i > 0u && i % _maxWordsPerPhrase == 0u) {
                phrases.Add(currentPhrase);
                currentPhrase = "";
            }
        }

        if (currentPhrase != "")
            phrases.Add(currentPhrase);

        return phrases;
    }

    private IEnumerator SubtitlesRoutine() {
        _displayingPhrases = true;
        foreach (string phrase in _currentPhrases)
        {
            _question.text = phrase;
            float lengthFactor = phrase.Length/70.0f;
            float maxDiff = Mathf.Max(lengthFactor - 1.0f, 0.0f);
            float sqrFactor = maxDiff * maxDiff;
            yield return new WaitForSeconds(4.0f * (lengthFactor + 0.1f * sqrFactor));
        }
        _displayingPhrases = false;
    }


    public void AnswerA()
    {
        if (!_dialog.IsDialogOver() && !_audioSource.isPlaying && (_currentQuestion.answerAClip == null || _currentQuestion.answerAClip != _previousClip)) {

            bool isAnswer = _currentQuestion.answerAClip != null;
            _previousClip = _currentQuestion.answerAClip;

            string answer = _dialog.Answer(true);
            _currentPhrases = CreatePhrases(answer);
            StartCoroutine("SubtitlesRoutine");
            _answering = true;
            HideButtonB();

            if (isAnswer)
            {
                if (_dialogManager.answerSelected != null)
                    _audioSource.PlayOneShot(_dialogManager.answerSelected);

                _audioSource.PlayOneShot(_currentQuestion.answerAClip, 1.0f);
                

                if (_currentQuestion.answerAIsHappy)
                    _face.Happy();
                else
                    _face.Sad();
            }
        }
    }

    public void AnswerB()
    {
        if (!_dialog.IsDialogOver() && !_audioSource.isPlaying && (_currentQuestion.answerBClip == null || _currentQuestion.answerBClip != _previousClip)) {
            bool isAnswer = _currentQuestion.answerBClip != null;
            _previousClip = _currentQuestion.answerBClip;

            string answer = _dialog.Answer(false);
            _currentPhrases = CreatePhrases(answer);
            StartCoroutine("SubtitlesRoutine");
            _answering = true;
            HideButtonA();

            if(isAnswer)
            {

                if (_dialogManager.answerSelected != null)
                    _audioSource.PlayOneShot(_dialogManager.answerSelected);

                _audioSource.PlayOneShot(_currentQuestion.answerBClip, 1.0f);

               
                if (_currentQuestion.answerBIsHappy)
                    _face.Happy();
                else
                    _face.Sad();
            }
        }
    }

    private void HideButtonA() {
        if (_buttonA != null)
        {
            _buttonA.gameObject.SetActive(false);
            _buttonABackground.SetActive(false);
        }
    }

    private void HideButtonB() {
        if (_buttonB != null)
        {
            _buttonB.gameObject.SetActive(false);
            _buttonBBackground.SetActive(false);
        }
    }


    private void ShowAnswersUI() {
        _dialogManager.onShowEvents.Invoke();

        if (_buttonA != null)
        {
            _buttonA.gameObject.SetActive(true);
            _buttonABackground.SetActive(true);
        }
        if (_buttonB != null)
        {
            _buttonB.gameObject.SetActive(true);
            _buttonBBackground.SetActive(true);
        }
    }

    private void HideAnswersUI() {
        if (_buttonA != null)
        {
            _buttonA.gameObject.SetActive(false);
            _buttonABackground.SetActive(false);
        }
        if (_buttonB != null)
        {
            _buttonB.gameObject.SetActive(false);
            _buttonBBackground.SetActive(false);
        }
    }

    private void HideQuestionUI()
    {
        _question.gameObject.SetActive(false);
    }

    private void ShowQuestionUI()
    {
        _question.gameObject.SetActive(true);
    }

}
