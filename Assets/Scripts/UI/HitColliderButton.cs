using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HitColliderButton : MonoBehaviour
{
    public bool isToggle;
    public Toggle toggle;

    public Button button;
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
        else
        {
            if (button.IsInteractable())
                button.onClick.Invoke();
        }
    }

    public void RaycastExit()
    {
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void LeaveScene()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
