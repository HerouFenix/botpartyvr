using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    string sceneName = "";
    [SerializeField] ToggleGroup toggleGroup_Controller;
    [SerializeField] ToggleGroup toggleGroup_Hands;
    [SerializeField] ToggleGroup toggleGroup_Other;
    public Button firstButton;


    [SerializeField] private Image _image;
    [SerializeField] private float _fadeSpeed = 0.1f;
    private float _currentAlpha = 0.0f;
    private bool _startFade = false;

    private void Start()
    {
        if(firstButton != null)
        {
            firstButton.onClick.Invoke();
        }

    }

    public void LoadScenario()
    {
        if (_startFade)
            return;

        if (toggleGroup_Controller.IsActive())
        {
            switch (toggleGroup_Controller.GetFirstActiveToggle().name)
            {

                case "Buttons":
                    sceneName = "Inst_MainScene_Controller_Buttons";
                    break;

                case "Trigger":
                    sceneName = "Inst_MainScene_Controller_Trigger_Button";
                    break;

                case "Trigger_Delay":
                    sceneName = "Inst_MainScene_Controller_Trigger";
                    break;

                case "Joystick":
                    sceneName = "Inst_MainScene_Joystick";
                    break;

                case "Controller_Buzzer":
                    sceneName = "Inst_MainScene_Controller_3DButton";
                    break;

                case "Grasp":
                    sceneName = "Inst_MainScene_Controller_DragDrop";
                    break;

                case "Swipe":
                    sceneName = "Inst_MainScene_Controller_Swipe_Trigger";
                    break;

                default:
                    break;
            }
        }

        else if (toggleGroup_Hands.IsActive())
        {
            switch (toggleGroup_Hands.GetFirstActiveToggle().name)
            {

                case "Hand_Gersture":
                    sceneName = "Inst_MainScene_Hands_Gestures";
                    break;

                case "Index":
                    sceneName = "Inst_MainScene_Hands_Pointing";
                    break;

                case "OpenHand":
                    sceneName = "Inst_MainScene_Hands_Pointing_Palm";
                    break;

                case "Pinch":
                    sceneName = "Inst_MainScene_Hands_Pointing_Trigger";
                    break;

                case "Hands_Buzzer":
                    sceneName = "Inst_MainScene_Hand_3DButton";
                    break;

                case "Hands_Grasp":
                    sceneName = "Inst_MainScene_Hands_DragDrop";
                    break;

                case "Swipe":
                    sceneName = "Inst_MainScene_Hands_Swipe";
                    break;

                default:
                    break;
            }
        }

        else if (toggleGroup_Other.IsActive())
        {
            switch (toggleGroup_Other.GetFirstActiveToggle().name)
            {

                case "Gaze":
                    sceneName = "Inst_MainScene_Head_Gaze";
                    break;

                case "Head_Gestures":
                    sceneName = "Inst_MainScene_Nod_Shake";
                    break;

                case "Smack":
                    sceneName = "Inst_MainScene_Head_Smack";
                    break;

                case "Tilt":
                    sceneName = "Inst_MainScene_Head_Tilt";
                    break;

                case "Head_Controller":
                    sceneName = "Inst_MainScene_Head_GazeController";
                    break;

                case "Head_Hand":
                    sceneName = "Inst_MainScene_Head_GazeHands";
                    break;

                default:
                    break;
            }
        }

        _image.gameObject.SetActive(true);
        _image.color = new Color(0.0f, 0.0f, 0.0f, _currentAlpha);
        _startFade = true;   
    }

    private void Update()
    {
        if (_startFade)
        { // Fading
            _currentAlpha += _fadeSpeed * Time.deltaTime;
            _currentAlpha = Mathf.Clamp(_currentAlpha, 0.0f, 1.0f);
            _image.color = new Color(0.0f, 0.0f, 0.0f, _currentAlpha);

            if(_currentAlpha >= 1.0f)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                _startFade = false;
            }
        }
    }

    /*
    public void UpdateSceneName(string interactionType)
    {
        if (toggleGroup_Controller.activeSelf)
        {
            switch (interactionType)
            {

                case "Buttons":
                    //sceneName = "";
                    break;

                case "Trigger":
                    //sceneName = "";
                    break;

                case "Trigger_Delay":
                    //sceneName = "";
                    break;

                case "Joystick":
                    //sceneName = "";
                    break;

                case "Controller_Buzzer":
                    //sceneName = "";
                    break;

                case "Grasp":
                    //sceneName = "";
                    break;

                case "Swipe":
                    //sceneName = "";
                    break;

                default:
                    break;
            }
        }

        else if (toggleGroup_Hands.activeSelf)
        {
            switch (interactionType)
            {

                case "Hand_Gersture":
                    //sceneName = "";
                    break;

                case "Index":
                    //sceneName = "";
                    break;

                case "OpenHand":
                    //sceneName = "";
                    break;

                case "Pinch":
                    //sceneName = "";
                    break;

                case "Hands_Buzzer":
                    //sceneName = "";
                    break;

                default:
                    break;
            }
        }

        else if (toggleGroup_Other.activeSelf)
        {
            switch (interactionType)
            {

                case "Gaze":
                    //sceneName = "";
                    break;

                case "Head_Gestures":
                    //sceneName = "";
                    break;

                case "Smack":
                    //sceneName = "";
                    break;

                case "Tilt":
                    //sceneName = "";
                    break;

                case "Head_Controller":
                    //sceneName = "";
                    break;

                case "Head_Hand":
                    //sceneName = "";
                    break;

                case "TBD":
                    //sceneName = "";
                    break;

                case "Voice":
                    //sceneName = "";
                    break;

                default:
                    break;
            }
        }

        Debug.Log("Scene Updated: " + sceneName);
    }
    */

    public void exitButton()
    {
        Application.Quit();
    }

}
