using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_FillCircle : MonoBehaviour
{
    private Image _image;
    public float fillRate = 0.1f;
    public float unfillRate = 0.1f;

    private bool _pointerOn;

    public UnityEvent onFillEvent;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (!_pointerOn)
        {
            if (_image.fillAmount > 0.0f)
                IncrementalUnfillImage();

        }
        else
        {
            if (_image.fillAmount < 1.0f)
                IncrementalFillImage();
            else
            {
                onFillEvent.Invoke();
                //_image.fillAmount = 0.0f;
            }
        }
    }

    public void PointerOn()
    {
        _pointerOn = true;
    }

    public void PointerOff()
    {
        _pointerOn = false;
    }


    private void IncrementalFillImage()
    {
        if (_image.fillAmount > 1.0f)
            _image.fillAmount = 1.0f;

        _image.fillAmount += fillRate;
    }


    private void IncrementalUnfillImage()
    {
        if (_image.fillAmount < 0.0f)
            _image.fillAmount = 0.0f;

        _image.fillAmount -= fillRate;
    }

    private void OnDisable()
    {
        _image.fillAmount = 0.0f;
    }


}
