using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CustomJsonDeserializer
{
    public static Tuple<List<List<string>>,List<List<string>>> DeserializeFromJson (string path)
    {
        List<List<string>> FieldName = new List<List<string>>(); List<List<string>> FieldData = new List<List<string>>();
        List<List<string>> Results = new List<List<string>>();
        var txtData = Resources.Load("5emonsters") as TextAsset;
        var textString = txtData.ToString();
        var jObject = JObject.Parse(textString);
        string jString = jObject.ToString();
        string[] recipient;
        List<string> fieldName = new List<string>();
        List<string> fieldData = new List<string>();
        recipient = jString.Split(new char[] { '{', '}' });
        //Breaks the List into Lists
        foreach (string s in recipient)
        {
            if (s.Contains("[")) { }
            else
            if (s.Contains("\"")) { Results.Add(s.Split(new string[] { "\"," }, System.StringSplitOptions.RemoveEmptyEntries).ToList()); }
        }
        for (int i = 0; i < Results.Count; i++)
        {
            for (int c = 0; c < Results[i].Count; c++)
            {
                recipient = Results[i][c].Split(new string[] { "\": ", "\"," }, System.StringSplitOptions.RemoveEmptyEntries);
                for (int d = 0; d < recipient.Length; d++)
                {
                        if (d % 2 != 0)
                        {
                            var newRecipient = recipient[d].Split(new string[] { "\"" }, System.StringSplitOptions.RemoveEmptyEntries);
                            if (recipient.Equals("\"")) { }
                            else
                            {
                                for (int h = 0; h < newRecipient.Length; h++)
                                    fieldData.Add(newRecipient[h]);
                            }
                        }
                        else
                        {
                            var newRecipient = recipient[d].Split(new string[] { "\"" }, System.StringSplitOptions.RemoveEmptyEntries);
                            newRecipient = newRecipient.Where(_string => !string.IsNullOrWhiteSpace(_string)).ToArray();
                        if (!recipient.Equals("\"")) 
                            {
                                for (int h = 0; h < newRecipient.Length; h++)
                                {
                                fieldName.Add(newRecipient[h]);
                                }
                            }
                        }
                }
            } 
            FieldName.Add(fieldName.ToList());
            FieldData.Add(fieldData.ToList());
            fieldName = new List<string>();
            fieldData = new List<string>();
        }
        return Tuple.Create(FieldName, FieldData);
    }
}


