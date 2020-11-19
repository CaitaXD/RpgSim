using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LeftMenuUI : MonoBehaviour
{
    //ToDo: make all non-Static variables private
    public GameObject prefab;
    public Transform parent;
    public Vector2 pivot;
    [SerializeField] float spacing;
    [SerializeField] TMP_InputField inputfield;
    [SerializeField] int items;

    ResultsMenu resultsMenu;
    SearchEngine SearchEngine;
    EntetieList monsterManual;

    //ToDo: pass the static variables to a UI parent Class when said class is implemented
    static public List<DropDownMenu> ClosePopUpReferenceList = new List<DropDownMenu>();
    static public bool isPointerDraggin = false;

    private void Awake()
    {
        monsterManual = new EntetieList(@"Assets\ExternalData\5emonsters.json");

        SearchEngine = new SearchEngine(inputfield);

        resultsMenu = new ResultsMenu(prefab, parent, items, pivot, spacing);

        resultsMenu.SearchOnValueChanged(SearchEngine, monsterManual);

        resultsMenu.PrintResultsOnValuechanged();

        resultsMenu.DelegateResults();
    }
    private void Update()
    {
        SearchEngine.RollResultsIndex(resultsMenu.GetTexts());
        InputHandler();
        if (!ClosePopUpReferenceList.Contains(resultsMenu.newMenu) && resultsMenu.newMenu != null)
        {
            ClosePopUpReferenceList.Add(resultsMenu.newMenu);
            DelegateAddEntetieToGridButton(0, "Add");
        }
    }
    private void InputHandler()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Commands.ClearPopUps();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Commands.ClearPopUps();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Commands.ClearPopUps();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Commands.ClearPopUps();
        }
    }
    private void DelegateAddEntetieToGridButton(int index, string text)
    {
        var addEntetieButton = resultsMenu.newMenu.ReferenceList[index];
        addEntetieButton.GetComponentInChildren<Text>().text = text;
        resultsMenu.newMenu.AddListeners(delegate
        {
            var entetie = monsterManual.Entities.Find(x => x.GetName == addEntetieButton.parent.GetComponentInChildren<Text>().text);
            entetie.Intantiate();

        }, addEntetieButton);
    }
}

