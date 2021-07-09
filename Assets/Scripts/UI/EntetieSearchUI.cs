using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EntetieSearchUI : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;
    [SerializeField] Vector2 pivot;
    [SerializeField] float spacing;
    [SerializeField] TMP_InputField inputfield;
    [SerializeField] int items;

    private ResultsDropList resultsMenu;
    private SearchEngine SearchEngine;


    private Button[] SearhTypes;
    private List<Vector3> rectOrginPos = new List<Vector3>();
    private bool interacted = false;
    private EntetieList CurrentList;
    private string SearchType;


    static public List<DropList> DropListTracker = new List<DropList>();
    static public bool isPointerDraggin = false;

    private void OnEnable()
    {
        ImportScript.OnImport += DrawCurrentList;
    }
    private void OnDisable()
    {
        ImportScript.OnImport -= DrawCurrentList;
    }
    private void Awake()
    {
        SearchEngine = new SearchEngine(inputfield);
        resultsMenu = new ResultsDropList(prefab, parent, items, pivot, spacing);
        resultsMenu.DelegateResults();    
    }
    private void Start()
    {
        SearhTypes = transform.GetComponentsInChildren<Button>();
        foreach (var searchType in SearhTypes)
        {
            rectOrginPos.Add(searchType.GetComponent<RectTransform>().position);
            searchType.onClick.AddListener(delegate { SelectSearchType(searchType.gameObject); });
        }
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
            Commands.ClearDropDownMenus();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Commands.ClearDropDownMenus();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Commands.ClearDropDownMenus();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Commands.ClearDropDownMenus();
        }
    }
    private void AddEntetieButton(int buttonIndex, string buttonText)
    {
        var addEntetieButton = resultsMenu.newList._Items[buttonIndex];
        addEntetieButton.GetComponentInChildren<Text>().text = buttonText;
        resultsMenu.newList.AddListeners(delegate
        {
            var entetie = CurrentList.Entities.Find(x => x.GetName == addEntetieButton.parent.GetComponentInChildren<Text>().text);
            entetie.Intantiate();

        }, addEntetieButton);
    }
    public void DrawCurrentList()
    {
        string path = Application.streamingAssetsPath + @"\" + SearchType + ".json";
        if (File.Exists(path))
        {
            if (!ListOfEntetieLists.PathListPair.ContainsKey(path))
            {
                CurrentList = new EntetieList(path);
                ListOfEntetieLists.PathListPair.Add(path, CurrentList);
            }
            resultsMenu.SearchOnValueChanged(SearchEngine, CurrentList);
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
    private void SelectSearchType(GameObject currentButtonObject)
    {
        var inputFieldfRef = inputfield.transform;
        if (interacted == false)
        {
            foreach (var searchType in SearhTypes) searchType.gameObject.SetActive(!searchType.IsActive());
            currentButtonObject.SetActive(!currentButtonObject.activeInHierarchy);
            var currentButtonRectTransform = currentButtonObject.GetComponent<RectTransform>();
            var topPosition = SearhTypes[0].GetComponent<RectTransform>().position;

            if (currentButtonObject.activeInHierarchy) currentButtonRectTransform.position = topPosition;
            interacted = true;
           
            inputFieldfRef.SetParent(currentButtonObject.transform);
            inputFieldfRef.gameObject.SetActive(true);
            inputFieldfRef.position = new Vector3(topPosition.x, topPosition.y - 45, topPosition.z);

            SearchType = currentButtonObject.name;
            var path = Application.streamingAssetsPath + @"\" + SearchType + ".json";
            if (File.Exists(path))
            {
                DrawCurrentList();
            }      
        }
        else
        {
            foreach (var searchType in SearhTypes)
            {
                searchType.gameObject.SetActive(true);
                searchType.GetComponent<RectTransform>().position = rectOrginPos[searchType.transform.GetSiblingIndex()];
            }
            inputFieldfRef.gameObject.SetActive(false);
            interacted = false;
        }
    }
}


