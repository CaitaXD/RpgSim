using UnityEngine;
using UnityEngine.UI;

public class ResultsDropList : DropList
{
    private SearchEngine searchEngine;
    public ResultsDropList(GameObject preafab, Transform parent, int items)
    {
        Instantiate(preafab, parent, items, _pivot, _spacing, _orientation);
    }
    public ResultsDropList(GameObject preafab, Transform parent, int items, Vector2 pivot)
    {
        Instantiate(preafab, parent, items, pivot, _spacing, _orientation);
    }
    public ResultsDropList(GameObject preafab, Transform parent, int items, Vector2 pivot, float spacing)
    {
        Instantiate(preafab, parent, items, pivot, spacing, _orientation);
    }
    public void SearchOnValueChanged(SearchEngine searchEngine , EntetieList entetieList)
    {
        this.searchEngine = searchEngine;
        searchEngine.inputField.onValueChanged.AddListener(
        delegate 
        {
            searchEngine.Search(entetieList.GetNames());
        });
    }
    public void ClearSearch(SearchEngine searchEngine)
    {
        searchEngine.inputField.onValueChanged.RemoveAllListeners();
        foreach (var Text in GetTexts())
            Text.text = "";
        searchEngine.inputField.text = "";
    }
    public void PrintResultsOnValuechanged()
    {
        if(searchEngine.GetResults() == null)
            Debug.Log("Invalid Search");
        else
        {
            searchEngine.inputField.onValueChanged.AddListener(
            delegate
            {
                foreach (var Text in GetTexts())
                    Text.text = "";
                for (int i = 0; i < _items && i < searchEngine.GetResults().Count; i++)
                    GetTexts()[i].text = searchEngine.GetResults()[i];
            });
        }
    }
    public void DelegateResults()
    {
        for (int i = 0; i < _Items.Count; i++)
        {
            int x = i;
            var reference = _Items[x];
            AddListeners(delegate {
                if(reference.GetComponentInChildren<Text>().text != "")
                newList = Expand(reference, 3);
            }, reference);
        }
    }
}
