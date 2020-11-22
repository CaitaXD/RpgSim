using UnityEngine;
using UnityEngine.UI;

public class ResultsMenu : DropDownMenu
{
    private SearchEngine searchEngine;
    public ResultsMenu(GameObject _preafab, Transform _parent, int _Items)
    {
        Instantiate(_preafab, _parent, _Items, pivot, spacing);
    }
    public ResultsMenu(GameObject _preafab, Transform _parent, int _Items, Vector2 _pivot)
    {
        Instantiate(_preafab, _parent, _Items, _pivot, spacing);
    }
    public ResultsMenu(GameObject _preafab, Transform _parent, int _Items, Vector2 _pivot, float _spacing)
    {
        Instantiate(_preafab, _parent, _Items, _pivot, _spacing);
    }
    public void SearchOnValueChanged(SearchEngine searchEngine , EntetieList entetieList)
    {
        this.searchEngine = searchEngine;
        searchEngine.inputField.onValueChanged.AddListener(
        delegate 
        {
            searchEngine.Search(entetieList.GetNames);
        });
    }
    public void PrintResultsOnValuechanged()
    {
        if(searchEngine.GetResults() == null)
            Debug.LogError("Invalid Search");
        else
        {
            searchEngine.inputField.onValueChanged.AddListener(
            delegate
            {
                foreach (var Text in GetTexts())
                    Text.text = "";
                for (int i = 0; i < items && i < searchEngine.GetResults().Count; i++)
                    GetTexts()[i].text = searchEngine.GetResults()[i];
            });
        }
    }
    public void DelegateResults()
    {
        for (int i = 0; i < ReferenceList.Count; i++)
        {
            int x = i;
            var reference = ReferenceList[x];
            AddListeners(delegate {
                if(reference.GetComponentInChildren<Text>().text != "")
                newMenu = Expand(reference, 3);
            }, reference);
        }
    }
}
