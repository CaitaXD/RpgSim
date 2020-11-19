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

    DropDownMenu resultsMenu;
    DropDownMenu expandedMenu;
    SearchEngine SearchEngine;
    EntetieList monsterManual;

    //ToDo: pass the static variables to a UI parent Class when said class is implemented
    static public List<DropDownMenu> ClosePopUpReferenceList = new List<DropDownMenu>();
    static public bool isPointerDraggin = false;

    private void Awake()
    {
        monsterManual = LoadMonsterManual(@"Assets\ExternalData\5emonsters.json");
        SearchEngine = new SearchEngine(inputfield);
        resultsMenu = new DropDownMenu(prefab, parent, items, pivot, spacing);

        OnInputValue(delegate { SearchEngine.Search(monsterManual.GetNames);});
        OnInputValue(delegate { PrintResults(resultsMenu, SearchEngine.GetResults());});

        DelegateExpandButtons();
    }
    private void Update()
    {
        SearchEngine.RollResultsIndex(resultsMenu.GetTexts());
        InputHandler();
        if (!ClosePopUpReferenceList.Contains(expandedMenu) && expandedMenu != null)
        {
            ClosePopUpReferenceList.Add(expandedMenu);
            DelegateAddEntetieToGridButton(0, "Add");
        }
    }
    private void PrintResults(DropDownMenu menu, List<string> SearchResults)
    {
        var DysplayTexts = menu.GetTexts();
        foreach (var Text in DysplayTexts)
        {
            Text.text = "";
        }
        for (int i = 0; i < DysplayTexts.Count && i < SearchResults.Count; i++)
        {
            DysplayTexts[i].text = SearchResults[i];
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
    private void OnInputValue(UnityAction unityaction)
    {
        inputfield.onValueChanged.AddListener(delegate { unityaction(); });
    }
    private void DelegateExpandButtons()
    {
        for (int i = 0; i < resultsMenu.ReferenceList.Count; i++)
        {
            int x = i;
            var reference = resultsMenu.ReferenceList[x];
            var referenceText = reference.GetComponentInChildren<Text>();
            resultsMenu.AddListeners(delegate {expandedMenu = resultsMenu.Expand(reference, 3);
            }, reference);
        }
    }
    private void DelegateAddEntetieToGridButton(int index, string text)
    {
        var addEntetieButton = expandedMenu.ReferenceList[index];

        addEntetieButton.GetComponentInChildren<Text>().text = text;
        expandedMenu.AddListeners(delegate
        {
            var entetie = monsterManual.Entities.Find(x => x.GetName == addEntetieButton.parent.GetComponentInChildren<Text>().text);
            entetie.Intantiate();

        }, addEntetieButton);
    }
    private EntetieList LoadMonsterManual(string path)
    {
        Tuple<List<List<string>>, List<List<string>>> data = CustomJsonDeserializer.DeserializeFromJson(path);
        EntetieList monsterManual = new EntetieList(data.Item1, data.Item2);
        return monsterManual;
    }
}

