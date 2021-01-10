using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Entetie
{
    public Dictionary<string, string> fields; 
    public string GetName => fields["name"];
    public Entetie(List<string> fieldNames, List<string> fieldValues)
    {
        for (int i = 0; i < fieldNames.Count; i++)
        {
            fields.Add(fieldNames[i],fieldValues[i]);
        }
    }
    public Entetie(string fieldName, string fieldValue)
    {
        fields.Add(fieldName,fieldValue);
    }

    public Entetie(Dictionary<string,string> keyValuePairs)
    {
        fields = keyValuePairs;
    }
    public GameObject Intantiate()
    {
        string[] Items = new string[1];
        Items[0] = GetName;
        GameObject gam = new GameObject(GetName, typeof(SpriteRenderer));
        SpriteRenderer spriteRenderer = gam.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("placeholderSkull");
        var entetieInstance = gam.AddComponent<EntetieScript>();
        entetieInstance.Entetie = this;
        spriteRenderer.sortingOrder = 1;
        var col = gam.AddComponent<BoxCollider>();
        col.isTrigger = true;
        gam.transform.localScale /= 5;
        return gam;
    }
}
public class EntetieList
{
    public List<Entetie> Entities = new List<Entetie>();
    public List<Dictionary<string, string>> listFields = new List<Dictionary<string, string>>();

    public List<string> GetNames()
    {
        var lis = new List<string>();
        foreach (var l in listFields)
        {
            lis.Add(l["name"]);
        }
        return lis;
    }
    public void Import(string path)
    {
        var newFields = CustomJsonDeserializer.DeserializeFromJson(path);
        for (int i = 0; i < newFields.Count; i++)
        {
            Entities.Add(new Entetie(newFields[i]));
            listFields.Add(newFields[i]);
        }

    }
    public EntetieList(string path)
    {
       listFields = CustomJsonDeserializer.DeserializeFromJson(path);
        for (int i = 0; i < listFields.Count; i++)
        {
            Entities.Add(new Entetie(listFields[i]));
        }
    }
}



