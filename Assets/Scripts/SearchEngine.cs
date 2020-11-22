using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class SearchEngine
{
    public TMP_InputField inputField { get;private set; }
    List<string> results = new List<string>();
    int increment = 0;

    public SearchEngine(TMP_InputField inputField)
    {
        this.inputField = inputField;
    }
    public List<string> Search(List<string> Data)
    {  
        results = new List<string>();
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i].ToLower().Contains(inputField.text.ToLower()))
            {
                results.Add(Data[i]);
            }
        }
        return results;
    }
    public void RollResults(List<Text> _Texts)
    {
        if (results.Count > _Texts.Count)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
              increment++;
              for (int i = 0; i < _Texts.Count; i++)
              {
                if (i + increment < results.Count) 
                {
                  if (results.Last() != _Texts.Last().text)
                  {
                  _Texts[i].text = results[i + increment];            
                  }
                }
                  else increment--;
              }
            } 
            if (Input.GetKey(KeyCode.UpArrow))
            {
            increment--;
            for (int i = 0; i < _Texts.Count; i++)
              {
                if (i + increment >= 0) 
                {
                   if (results[i] != _Texts[i].text)
                   {
                    _Texts[i].text = results[i + increment];
                   }
                }
                else increment = 0;
              }  
            }
        }
    }

    public List<string> GetResults()
    {
        return results;
    }
}
