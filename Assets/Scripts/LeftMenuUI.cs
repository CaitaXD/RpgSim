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
    static public List<DropDownMenu> TempMenusRef = new List<DropDownMenu>();
    static public bool isPointerDraggin = false;

    private void Awake()
    {
        monsterManual = new EntetieList(@"Assets\ExternalData\5emonsters.json");
        SearchEngine = new SearchEngine(inputfield);
        resultsMenu = new ResultsMenu(prefab, parent, items, pivot, spacing);
        ResultsMenuRotine(resultsMenu);
        
    }
    private void Update()
    {
        SearchEngine.RollResults(resultsMenu.GetTexts());
        InputHandler();
        var menuNotCalled = !TempMenusRef.Contains(resultsMenu.newMenu);
        var menuNotNull = resultsMenu.newMenu != null;
        if (menuNotCalled && menuNotNull)
        {
            TempMenusRef.Add(resultsMenu.newMenu);
            AddEntetieButton(0, "Add");
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
    private void AddEntetieButton(int index, string text)
    {
        var addEntetieButton = resultsMenu.newMenu.ReferenceList[index];
        addEntetieButton.GetComponentInChildren<Text>().text = text;
        resultsMenu.newMenu.AddListeners(delegate
        {
            var entetie = monsterManual.Entities.Find(x => x.GetName == addEntetieButton.parent.GetComponentInChildren<Text>().text);
            entetie.Intantiate();

        }, addEntetieButton);
    }
    private void ResultsMenuRotine(ResultsMenu menu)
    {
        menu.SearchOnValueChanged(SearchEngine, monsterManual);
        menu.PrintResultsOnValuechanged();
        menu.DelegateResults();
    }
}


