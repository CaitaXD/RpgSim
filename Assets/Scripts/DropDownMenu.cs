using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropDownMenu
{
    protected float _spacing = 75;
    protected Vector2 _pivot = new Vector2(0.5f, 0.5f);
    protected Vector3 _offset;
    protected Vector3 _pos;
    protected GameObject _preafab;
    protected Transform _parent;
    protected int _items;
    public static Transform lastReference;
    public DropDownMenu newMenu;
    public List<Transform> ReferenceList { get; private set; } = new List<Transform>();
    protected DropDownMenu() 
    {
    }
    public DropDownMenu(GameObject preafab, Transform parent, int items)
    {
        Instantiate(preafab, parent, items, _pivot, _spacing);
    }
    public DropDownMenu(GameObject preafab, Transform parent, int items, Vector2 pivot)
    {
        Instantiate(preafab, parent, items, pivot, _spacing);
    }
    public DropDownMenu(GameObject preafab, Transform parent, int items, Vector2 pivot, float spacing)
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
            ReferenceList.Add(gam.transform);
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
        foreach (var transform in ReferenceList)
        {
            transform.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }
    }
    public void ChangeDropParent(Transform parent)
    {
        _pos = parent.position;
        foreach (var transform in ReferenceList)
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
                _pos -= _offset * ReferenceList.Count;
                transform.position = _pos;
                _pos += _offset;
            } else _pos += _offset;
        }
    }
    public void EnableDisable()
    {
        foreach (var transform in ReferenceList)
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
        foreach (var transform in ReferenceList)
        {
            if (transform.gameObject.activeInHierarchy)
                transform.gameObject.SetActive(false);
            else break;
        }
    }
    public void Destroy()
    {
        foreach (var transform in ReferenceList)
        {
            ReferenceList = new List<Transform>();
            GameObject.Destroy(transform.gameObject);
        }
    }
    public List<Text> GetTexts()
    {
        List<Text> texts = new List<Text>();
        foreach (var var in ReferenceList)
        {
            texts.Add(var.GetComponentInChildren<Text>());
        }
        return texts;
    }
    public DropDownMenu Expand(Transform parent, int Items)
    {
        var isThisLastReference = lastReference == null;
        var sameReferenceCalled = parent == lastReference;
        lastReference = parent;

        if (isThisLastReference && !sameReferenceCalled)
        {
          return newMenu = new DropDownMenu(_preafab, parent, Items, new Vector2((_pivot.x - 1) * (-1), _pivot.y), _spacing);
        }
        else if (!isThisLastReference && sameReferenceCalled)
        {
            newMenu.EnableDisable();
            return newMenu;
        }
        else if (!isThisLastReference && !sameReferenceCalled)
        {
            newMenu.ChangeDropParent(parent);
            return newMenu;
        }
        return newMenu;
    }
    public void SetPosition(Vector3 pos)
    {
        foreach (var transform in ReferenceList)
        {
            if (transform.gameObject.activeInHierarchy == false)
            {
                transform.gameObject.SetActive(true);
            }
            transform.localScale = Vector3.one;
            transform.position = pos;
            if (Camera.main.WorldToScreenPoint(transform.position).y < Screen.height)
            {
                pos -= _offset * ReferenceList.Count;
                transform.position = pos;
                pos += _offset;
            }
            else pos += _offset;
        }
    }
}
