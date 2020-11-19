using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Entetie
{
    private readonly List<string> FieldNames = new List<string>();
    private readonly List<string> FieldValues = new List<string>();
    private string EntetieName => FieldValues[0];
    public string GetName => EntetieName; 

    public Entetie(List<string> fieldNames, List<string> fieldValues)
    {
        FieldNames = fieldNames; FieldValues = fieldValues;
    }
    public Entetie(string fieldName, string fieldValue)
    {
        FieldNames.Add(fieldName); FieldValues.Add(fieldValue);
    }
    public GameObject Intantiate()
    {
        string[] Items = new string[1];
        Items[0] = EntetieName;
        GameObject gam = new GameObject(EntetieName, typeof(SpriteRenderer));
        SpriteRenderer spriteRenderer = gam.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("placeholderSkull");
        var pointerHandler = gam.AddComponent<EntetieScript>();
        pointerHandler.Entetie = this;
        spriteRenderer.sortingOrder = 1;
        var col = gam.AddComponent<BoxCollider>();
        col.isTrigger = true;
        gam.transform.localScale /= 5;
        return gam;
    }
}
public class EntetieList
{
    public readonly List<Entetie> Entities = new List<Entetie>();
    public readonly List<List<string>> fieldNames = new List<List<string>>(); 
    private readonly List<List<string>> fieldsData = new List<List<string>>();

    public List<string> GetNames => fieldsData[0];
    public EntetieList(string path)
    {
        Tuple<List<List<string>>, List<List<string>>> fields = CustomJsonDeserializer.DeserializeFromJson(path);
        fieldsData.Add(new List<string>());
        fieldNames.Add(new List<string>());

        for (int y = 0; y < fields.Item2[y].Count; y++)
        {
            for (int x = 0; x < fields.Item2.Count; x++)
            {
                Entities.Add(new Entetie(fields.Item1[x], fields.Item2[x]));
                if (fieldsData.Count <= x)
                {
                    fieldsData.Add(new List<string>());
                    fieldNames.Add(new List<string>());
                }
                if (fields.Item2[x].Count > y)
                {
                    fieldsData[y].Add(fields.Item2[x][y]);
                }
                if(fields.Item1[x].Count > y)
                {
                    fieldNames[y].Add(fields.Item1[x][y]);
                }
            }
        }
    }
}



