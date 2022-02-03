using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class FadeInEvent : SceneEvent
{

    [SerializeField] private Image _image;
    [SerializeField] private float _fadeSpeed = 0.1f;
    private float _currentAlpha = 1.0f;

    public override void StartEvent()
    {
        _image.gameObject.SetActive(true);
        _image.color = new Color(0.0f, 0.0f, 0.0f, _currentAlpha);
    }

    public override void DoEvent()
    {
        _currentAlpha -= _fadeSpeed * Time.deltaTime;
        _currentAlpha = Mathf.Clamp(_currentAlpha, 0.0f, 1.0f);
        _image.color = new Color(0.0f, 0.0f, 0.0f, _currentAlpha);

        if(_currentAlpha <= 0.0f)
        {
            _image.gameObject.SetActive(false);
        }
    }

    public override bool IsEventOver() => _currentAlpha == 0.0f;
}
