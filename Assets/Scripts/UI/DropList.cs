using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropList
{
    protected float _spacing = 75;
    protected Vector2 _pivot = new Vector2(0.5f, 0.5f);
    protected Vector3 _offset;
    protected Vector3 _pos;
    protected GameObject _preafab;
    protected Transform _parent;
    protected int _items;
    public static Transform lastReference;
    public DropList newList;
    public List<Transform> _Items { get; private set; } = new List<Transform>();
    protected DropList() 
    {
    }
    public DropList(GameObject preafab, Transform parent, int items)
    {
        Instantiate(preafab, parent, items, _pivot, _spacing);
    }
    public DropList(GameObject preafab, Transform parent, int items, Vector2 pivot)
    {
        Instantiate(preafab, parent, items, pivot, _spacing);
    }
    public DropList(GameObject preafab, Transform parent, int items, Vector2 pivot, float spacing)
    {
        Instantiate(preafab, parent, items, pivot, spacing);
    }
    protected void Instantiate(GameObject preafab, Transform parent, int items, Vector2 pivot, float spacing)
    {
        _preafab = preafab;
        _parent = parent;
        _spacing = spacing;
        _pivot = pivot;
        _items = items;
        Vector3 pos = parent.position;
        _offset = new Vector3(0, -spacing, 0);
        for (int i = 0; i < items; i++)
        {
            _offset = new Vector3(0, -spacing, 0);
            var gam = GameObject.Instantiate(_preafab);
            gam.transform.SetParent(parent);
            gam.transform.position = pos;
            _Items.Add(gam.transform);
            gam.GetComponent<RectTransform>().pivot = pivot;
            _offset.y /= gam.transform.localScale.y;
            gam.transform.localScale = new Vector3(1, 1, 1);
            if (Camera.main.WorldToScreenPoint(gam.transform.position).y < Screen.height)
            {
                pos -= _offset * items;
                gam.transform.position = pos;
                pos += _offset;
            }
            else pos += _offset;
        }
    }
    public void AddListeners(UnityAction action, Transform transform)
    {
        var button = transform.GetComponentInChildren<Button>();
        if (button != null)
            button.onClick.AddListener(action);
        else
            Debug.LogError("Menu Dosent Conatin Buttons");
    }
    public void RemoveAllListeners()
    {
        foreach (var transform in _Items)
        {
            transform.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }
    }
    public void ChangeDropParent(Transform parent)
    {
        _pos = parent.position;
        foreach (var transform in _Items)
        {
            if (transform.gameObject.activeInHierarchy == false)
            {
                transform.gameObject.SetActive(true);
            }
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            transform.position = _pos;
            if (Camera.main.WorldToScreenPoint(transform.position).y < Screen.height)
            {
                _pos -= _offset * _Items.Count;
                transform.position = _pos;
                _pos += _offset;
            } else _pos += _offset;
        }
    }
    public void EnableDisable()
    {
        foreach (var transform in _Items)
        {
            switch (transform.gameObject.activeInHierarchy)
            {
                case (true):
                    transform.gameObject.SetActive(false);
                    break;
                case (false):
                    transform.gameObject.SetActive(true);
                    break;
            }
        }
    }
    public void Disable()
    {
        foreach (var transform in _Items)
        {
            if (transform.gameObject.activeInHierarchy)
                transform.gameObject.SetActive(false);
            else break;
        }
    }
    public void Destroy()
    {
        foreach (var transform in _Items)
        {
            _Items = new List<Transform>();
            GameObject.Destroy(transform.gameObject);
        }
    }
    public List<Text> GetTexts()
    {
        List<Text> texts = new List<Text>();
        foreach (var var in _Items)
        {
            texts.Add(var.GetComponentInChildren<Text>());
        }
        return texts;
    }
    public bool IsActive()
    {
        return  _Items[0].gameObject.activeInHierarchy;
    }
    public DropList Expand(Transform parent, int items)
    {
        var isThisLastReference = lastReference == null;
        var sameReferenceCalled = parent == lastReference;
        lastReference = parent;

        if (isThisLastReference && !sameReferenceCalled)
        {
          return newList = new DropList(_preafab, parent, items, new Vector2((_pivot.x - 1) * (-1), _pivot.y), _spacing);
        }
        else if (!isThisLastReference && sameReferenceCalled)
        {
            newList.EnableDisable();
            return newList;
        }
        else if (!isThisLastReference && !sameReferenceCalled)
        {
            newList.ChangeDropParent(parent);
            return newList;
        }
        return newList;
    }
    public void ExpandAll(int items)
    {
        for (int i = 0; i < _Items.Count; i++)
            for (int k = 0; k < items; k++)
            {
                var gam = GameObject.Instantiate(_preafab, _Items[i]);
                var tran = gam.transform;
                var pos = gam.transform.localPosition;
                var rect = _Items[i].GetComponent<RectTransform>();
                tran.localPosition = new Vector3(pos.x + rect.sizeDelta.x ,pos.y - rect.sizeDelta.y,pos.z);
            }

    }
    public void Resize(int items)
    {
        if (items > _Items.Count)
        {          
            items -= _Items.Count;
            Instantiate(_preafab, _parent, items, _pivot, _spacing);
            SetPosition(_parent.position);
        }
        else if(items < _Items.Count)
        {
            for (int i = _Items.Count - 1; i > items -1; i--)
            {
                GameObject.Destroy(_Items[i].gameObject);
                _Items.Remove(_Items[i]);
            }
        }
                
    }
    public void SetPosition(Vector3 pos)
    {
        foreach (var transform in _Items)
        {
            if (transform.gameObject.activeInHierarchy == false)
            {
                transform.gameObject.SetActive(true);
            }
            transform.localScale = Vector3.one;
            transform.position = pos;
            if (Camera.main.WorldToScreenPoint(transform.position).y < Screen.height)
            {
                pos -= _offset * _Items.Count;
                transform.position = pos;
                pos += _offset;
            }
            else pos += _offset;
        }
    }
}
