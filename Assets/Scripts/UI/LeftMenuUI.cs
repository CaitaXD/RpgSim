using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LeftMenuUI : MonoBehaviour
{
    //ToDo: all fields should be private
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;
    [SerializeField] Vector2 pivot;
    [SerializeField] float spacing;
    [SerializeField] TMP_InputField inputfield;
    [SerializeField] int items;
    [SerializeField] int EntryItems;

    private ResultsDropList resultsMenu;
    private SearchEngine SearchEngine;
    public EntetieList monsterManual;

    //ToDo: there should not be static fields
    static public List<DropList> DropListTracker = new List<DropList>();
    static public bool isPointerDraggin = false;

    private void Awake()
    {
        SearchEngine = new SearchEngine(inputfield);
        resultsMenu = new ResultsDropList(prefab, parent, items, pivot, spacing);
        resultsMenu.DelegateResults();
    }
    private void Update()
    {
        SearchEngine.RollResultsList(resultsMenu.GetTexts());
        InputHandler();
        RoutineOfCurrentResults();
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
        var addEntetieButton = resultsMenu.newList._Items[index];
        addEntetieButton.GetComponentInChildren<Text>().text = text;
        resultsMenu.newList.AddListeners(delegate
        {
            var entetie = monsterManual.Entities.Find(x => x.GetName == addEntetieButton.parent.GetComponentInChildren<Text>().text);
            entetie.Intantiate();

        }, addEntetieButton);
    }
    public void DrawMonsterManual(string path)
    {
        if (File.Exists(path))
        {
            monsterManual = new EntetieList(path);
            resultsMenu.SearchOnValueChanged(SearchEngine, monsterManual);
            resultsMenu.PrintResultsOnValuechanged();
        }
    }
    public void DrawNothing()
    {
        resultsMenu.ClearSearch(SearchEngine);
    }
    private void RoutineOfCurrentResults()
    {
        var menuNotCalled = !DropListTracker.Contains(resultsMenu.newList);
        var menuNotNull = resultsMenu.newList != null;
        if (menuNotCalled && menuNotNull)
        {
            DropListTracker.Add(resultsMenu.newList);
            AddEntetieButton(0, "Add");
        }
    }  
}


