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
    [SerializeField] string[] texts;

    DropDownMenu resultsMenu;
    SearchEngine SearchEngine;
    EntetieList monsterManual;

    //ToDo: pass the static variables to a UI parent Class when said class is implemented
    static public List<DropDownMenu> ClosePopUpReferenceList = new List<DropDownMenu>();
    static public bool isPointerDraggin = false;

    private void Awake()
    {
        monsterManual = Commands.LoadMonsterManual();
        SearchEngine = new SearchEngine(inputfield);
        resultsMenu = new DropDownMenu(prefab, parent, texts, pivot, spacing);

        OnInputValue(delegate { SearchEngine.Search(monsterManual.GetNames);});
        OnInputValue(delegate { PrintResults(resultsMenu, SearchEngine.GetResults());});

        ShowOptions(monsterManual, resultsMenu);       
    }
    private void Update()
    {
        SearchEngine.RollResultsIndex(resultsMenu.GetTexts());
        InputHandler();
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
    private void ShowOptions(EntetieList entetieList, DropDownMenu menu)
    {
        texts = new string[3];
        texts[0] = "Add";
        texts[1] = "Edit";
        texts[2] = "Remove";
        for (int i = 0; i < menu.ReferenceList.Count; i++)
        {
            int x = i;
            var reference = menu.ReferenceList[x];
            var referenceText = reference.GetComponentInChildren<Text>();
            menu.AddListeners(delegate {
                var expandedMenu = menu.Expand(reference, texts);
                var addEntetieButton = expandedMenu.ReferenceList[0].GetComponentInChildren<Button>();
                var entetie = entetieList.Entities.Find(thing => thing.GetName == referenceText.text);

                addEntetieButton.onClick.RemoveAllListeners();
                addEntetieButton.onClick.AddListener(delegate { entetie.Intantiate(); });
            }, reference);
        }
    }
}
