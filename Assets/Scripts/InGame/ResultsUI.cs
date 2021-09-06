using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] GameObject resultPrefab;
    public void ShowResults(List<int> results)
    {
        if(transform.childCount != 0) 
        {
            var children = GetComponentsInChildren<Text>();
            foreach(var child in children) Destroy(child.gameObject);
        }
        var ResultList = new DropList(resultPrefab, transform, results.Count, 100, DropListOrientation.rigth);
        for (int i = 0; i < results.Count; i++)
        {
            ResultList.GetTexts()[i].text = results[i].ToString();
        }
    }
}
