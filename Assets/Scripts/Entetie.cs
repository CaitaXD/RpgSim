using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
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
    [Serializable]
public class EntetieList
{
    public readonly List<Entetie> Entities = new List<Entetie>();
    public readonly List<List<string>> fieldNames = new List<List<string>>(); 
    private readonly List<List<string>> fieldsData = new List<List<string>>();

    public List<string> GetNames => fieldsData[0];
    public EntetieList(List<List<string>> fieldNames, List<List<string>> fieldValues)
    {
        this.fieldsData.Add(new List<string>());
        this.fieldNames.Add(new List<string>());
        for (int y = 0; y < fieldValues[y].Count; y++)
        {
            for (int x = 0; x < fieldValues.Count; x++)
            {
                Entities.Add(new Entetie(fieldNames[x], fieldValues[x]));
                if (this.fieldsData.Count <= x)
                {
                    this.fieldsData.Add(new List<string>());
                    this.fieldNames.Add(new List<string>());
                }
                if (fieldValues[x].Count > y)
                {
                    this.fieldsData[y].Add(fieldValues[x][y]);
                }
                if(fieldNames[x].Count > y)
                {
                    this.fieldNames[y].Add(fieldNames[x][y]);
                }
            }
        }
    }
}



