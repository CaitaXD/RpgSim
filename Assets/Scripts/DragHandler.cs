using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler
{
    [SerializeField] RectTransform _rectTransfrom;
    [SerializeField] Canvas _canvas;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _canvas = _canvas ? _canvas : GameObject.Find("Canvas").GetComponent<Canvas>();
        _rectTransfrom = _rectTransfrom ? _rectTransfrom : _transform.parent.GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransfrom.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
}
