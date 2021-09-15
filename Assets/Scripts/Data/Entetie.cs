using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum EntetieListName
{
    MonsterManual = 0
}
public class Entetie : ScriptableObject
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
        spriteRenderer.sprite = Resources.Load<Sprite>("SkeliBoi");
        var entetieInstance = gam.AddComponent<EntetieScript>();
        entetieInstance.Entetie = this;
        spriteRenderer.sortingOrder = 1;
        var col = gam.AddComponent<BoxCollider>();
        col.isTrigger = true;
        return gam;
    }
}
public class EntetieList
{
    public List<Entetie> Entities = new List<Entetie>();
    public string _path;
    public List<Dictionary<string, string>> listFields = new List<Dictionary<string, string>>();
    public EntetieListName _someName;

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
        var newFields = JArrayDeserializer.JsonToStringStringDictionaryList(path);
        for (int i = 0; i < newFields.Count; i++)
        {
            Entities.Add(new Entetie(newFields[i]));
            listFields.Add(newFields[i]);
        }
    }
    public EntetieList(EntetieListName someName, string path)
    {
       listFields = JArrayDeserializer.JsonToStringStringDictionaryList(path);
        for (int i = 0; i < listFields.Count; i++)
        {
            Entities.Add(new Entetie(listFields[i]));
        }
        _someName = someName;
        _path = path;
    }
    public EntetieList(string path)
    {
        listFields = JArrayDeserializer.JsonToStringStringDictionaryList(path);
        for (int i = 0; i < listFields.Count; i++)
        {
            Entities.Add(new Entetie(listFields[i]));
        }
        _path = path;
    }
}
public class ListOfEntetieLists
{
    public static readonly Dictionary<string, EntetieList> PathListPair = new Dictionary<string, EntetieList>();
    public static readonly Dictionary<EntetieListName, string> NamePathPair = new Dictionary<EntetieListName, string>();
    public void AddList(EntetieListName someName , string path)
    {
        NamePathPair.Add(someName, path);
        PathListPair.Add(path, new EntetieList(someName, path));
    }
    public void AddList(string path)
    {
        PathListPair.Add(path, new EntetieList(path));
    }
    public ListOfEntetieLists()
    { }
    public ListOfEntetieLists(List<string> paths)
    {
        foreach (var path in paths)
        {
            AddList(path);
        }
    }
}



