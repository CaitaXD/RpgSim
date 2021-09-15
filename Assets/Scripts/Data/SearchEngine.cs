using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SearchEngine
{
    public TMP_InputField inputField { get;private set; }
    List<string> _results = new List<string>();
    int _increment = 0;

    public SearchEngine(TMP_InputField inputField)
    {
        this.inputField = inputField;
    }
    public List<string> Search(List<string> Data)
    {  
        _results = new List<string>();
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i].ToLower().Contains(inputField.text.ToLower()))
            {
                _results.Add(Data[i]);
            }
        }
        return _results;
    }
    public void RollResultsList(List<Text> Texts)
    {
        if (_results.Count > Texts.Count)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
              _increment++;
              for (int i = 0; i < Texts.Count; i++)
              {
                if (i + _increment < _results.Count) 
                {
                  if (_results.Last() != Texts.Last().text)
                  {
                  Texts[i].text = _results[i + _increment];            
                  }
                }
                  else _increment--;
              }
            } 
            if (Input.GetKey(KeyCode.UpArrow))
            {
            _increment--;
            for (int i = 0; i < Texts.Count; i++)
              {
                if (i + _increment >= 0) 
                {
                   if (_results[i] != Texts[i].text)
                   {
                    Texts[i].text = _results[i + _increment];
                   }
                }
                else _increment = 0;
              }  
            }
        }
    }

    public List<string> GetResults() => _results;
}
