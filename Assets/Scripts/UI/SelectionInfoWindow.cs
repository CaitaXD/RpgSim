using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionInfoWindow : MonoBehaviour
{
    [SerializeField] GameObject WindowPrefab;
    [SerializeField] List<GameObject> Windows;
    Image _image;
    Text _name;
    Text _hp;

    public void Awake()
    {
        UpdateSelectedEnteties();
    }
    public void Start()
    {
        Events.current.onSelectionChange += UpdateSelectedEnteties;
        Events.current.onSelectionClearing += ClearInfoWindows;
        Events.current.onUpdateEntetie += UpdateDysplayedValue;
    }
    public void Update()
    {
        //UpdateSelectedEnteties();
    }
    void UpdateSelectedEnteties()
    {
        for (int i = 0; i < SelectionScript.SelectedEnteties.Count; i++)
        {
            EntetieScript entetie = SelectionScript.SelectedEnteties[i];
            GameObject window = Windows[i];
            _image = window.GetComponentInChildren<Image>();
            _image.sprite = entetie._sprite;
            _name = window.GetComponentsInChildren<Text>()[0];
            _name.text = entetie.name;
            _hp = window.GetComponentsInChildren<Text>()[1];
            _hp.text = entetie.Health.ToString();
            Windows.Add(window);
        }
    }

    void UpdateDysplayedValue()
    {
        for (int i = 0; i < SelectionScript.SelectedEnteties.Count; i++)
        {
            EntetieScript entetie = SelectionScript.SelectedEnteties[i];
            _name.text = _name ? entetie.name : entetie.name;
            _hp.text = _hp ? entetie.Health.ToString() : _hp.text;
        }
    }

    void ClearInfoWindows()
    {
        _image.sprite = WindowPrefab.GetComponentInChildren<Image>().sprite;
        _name.text = WindowPrefab.GetComponentInChildren<Text>().text;
        _hp.text = WindowPrefab.GetComponentsInChildren<Text>()[1].text;
    }
}
