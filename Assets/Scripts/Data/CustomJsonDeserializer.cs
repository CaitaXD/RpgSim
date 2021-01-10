using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CustomJsonDeserializer
{
    public static List<Dictionary<string,string>> DeserializeFromJson (string path)
    {
        List<string> Results = new List<string>();
        var txtString = File.ReadAllText(path);
        JArray jArray = JArray.Parse(txtString);
        List<string> recipient;
        List<Dictionary<string, string>> fields = new List<Dictionary<string, string>>();

        for (int i1 = 0; i1 < jArray.Count; i1++)
        {
            JToken token = jArray[i1];
            string s = token.ToString();
            if (s.Contains("\""))
            { 
                Results = s.Split(new string[] { "\"," }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                fields.Add(new Dictionary<string, string>());
                for (int i = 0; i < Results.Count; i++)
                {
                    string var = Results[i];
                    recipient = var.Split(new string[] { "\":" }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                    fields[i1].Add(RemoveSpecialCharacters(recipient[0]).Trim(), RemoveSpecialCharacters(recipient[1]));
                }
            }
        }
        return fields;
    }
    public static string RemoveSpecialCharacters(this string str)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '(' || c == ')')
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}


