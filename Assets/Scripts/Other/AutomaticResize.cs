using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutomaticResize : MonoBehaviour
{
    [SerializeField] private RectTransform _toCopy;
    private RectTransform _rectTransform;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _padding = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.position = _rectTransform.position;

        //_rectTransform.sizeDelta = _toCopy.sizeDelta;
        Vector3 bounds = _text.bounds.size;
        Vector2 newBounds = new Vector2(bounds.x + _padding, bounds.y + _padding);
        _rectTransform.sizeDelta = newBounds;
    }
}
