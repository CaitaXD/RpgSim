using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropDownMenu
{
    private float spacing = 75;
    private Vector2 pivot = new Vector2(0.5f, 0.5f);
    private Vector3 offset;
    private Vector3 pos;
    private GameObject preafab;
    private Transform parent;
    private int Items;
    public static Transform lastReference;
    private DropDownMenu newMenu;

    public List<Transform> ReferenceList { get; private set; } = new List<Transform>();
    //Constructors
    public DropDownMenu(GameObject _preafab, Transform _parent, int _Items)
    {
        Instantiate(_preafab, _parent, _Items, pivot, spacing);
    }
    public DropDownMenu(GameObject _preafab, Transform _parent, int _Items, Vector2 _pivot)
    {
        Instantiate(_preafab, _parent, _Items, _pivot, spacing);
    }
    public DropDownMenu(GameObject _preafab, Transform _parent, int _Items, Vector2 _pivot, float _spacing)
    {
        Instantiate(_preafab, _parent, _Items, _pivot, _spacing);
    }
    private void Instantiate(GameObject _preafab, Transform _parent, int _Items, Vector2 _pivot, float _spacing)
    {
        preafab = _preafab;
        parent = _parent;
        Items = _Items;
        spacing = _spacing;
        pivot = _pivot;
        Vector3 pos = _parent.position;
        offset = new Vector3(0, -_spacing, 0);
        for (int i = 0; i < _Items; i++)
        {
            offset = new Vector3(0, -_spacing, 0);
            var gam = GameObject.Instantiate(_preafab);
            gam.transform.SetParent(_parent);
            gam.transform.position = pos;
            ReferenceList.Add(gam.transform);
            gam.GetComponent<RectTransform>().pivot = _pivot;
            offset.y /= gam.transform.localScale.y;
            gam.transform.localScale = new Vector3(1, 1, 1);
            if (Camera.main.WorldToScreenPoint(gam.transform.position).y < Screen.height)
            {
                pos -= offset * _Items;
                gam.transform.position = pos;
                pos += offset;
            }
            else pos += offset;
        }
    }
    //==============================================================================

    //Delegation
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

    //=============================================================================


    //Trasformations 
    private void ChangeDropParent(Transform parent)
    {
        pos = parent.position;
        foreach (var transform in ReferenceList)
        {
            if (transform.gameObject.activeInHierarchy == false)
            {
                transform.gameObject.SetActive(true);
            }
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            transform.position = pos;
            if (Camera.main.WorldToScreenPoint(transform.position).y < Screen.height)
            {
                pos -= offset * ReferenceList.Count;
                transform.position = pos;
                pos += offset;
            } else pos += offset;
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
          return newMenu = new DropDownMenu(preafab, parent, Items, new Vector2((pivot.x - 1) * (-1), pivot.y), spacing);
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
                pos -= offset * ReferenceList.Count;
                transform.position = pos;
                pos += offset;
            }
            else pos += offset;
        }
    }

    //==============================================================================
}
