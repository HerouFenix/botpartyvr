using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    public string sceneName;

    [SerializeField] private Image _image;
    [SerializeField] private float _fadeSpeed = 0.1f;
    private float _currentAlpha = 0.0f;
    private bool _startFade = false;

    public void AdvanceScene()
    {
        if (_startFade)
            return;

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

            if (_currentAlpha >= 1.0f)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                _startFade = false;
            }
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
