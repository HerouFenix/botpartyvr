using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class ChangeSceneEvent : SceneEvent {

    [SerializeField] private string _targetSceme = "MainMenu";

    public override void StartEvent() {
        SceneManager.LoadScene(_targetSceme, LoadSceneMode.Single);
    }

    public override void DoEvent() {}

    public override bool IsEventOver() => false;
}