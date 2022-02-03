using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaycastDetector : MonoBehaviour
{
    public bool isCircle;
    public bool isToggle;
    public Toggle toggle;

    public Button button;

    public UI_FillCircle circle;

    GameObject eventSystem;
    GameObject menu;

    private void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
    }

    public void RaycastEnter()
    {
        if (isToggle)
        {
            if (toggle.IsInteractable())
                toggle.Select();
        }
        else if(isCircle)
        {
            circle.PointerOn();
            
        }
        else
        {
            if (button.IsInteractable())
                button.Select();
        }
    }

    public void RaycastClick()
    {
        if (isToggle)
        {
            if (toggle.IsInteractable())
                toggle.isOn = true;
        }
        else if(!isCircle)
        {
            if (button.IsInteractable())
                button.onClick.Invoke();
        }
    }

    public void RaycastExit()
    {
        if(isCircle)
            circle.PointerOff();
        else
            eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void LeaveScene()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
