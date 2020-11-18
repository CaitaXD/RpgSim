using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

/*public class NavigationScriptDeleted : MonoBehaviour
{
    public static NavigationScriptDeleted Instance;
    SearchEngine searchMonsterManual = new SearchEngine();
    [SerializeField] TMP_InputField inputField;
    public Transform menuParent;
    public GameObject prefab;
    public List<Text> buttonText;
    public static EntetieList monsterManual { get; private set; }
    public static Transform PreviusUIinteraction;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<NavigationScriptDeleted>();
        }
        else Destroy(gameObject);
        monsterManual = Commands.LoadMonsterManual();
       // var menu = CascadeMenu.Create(prefab,menuParent,new string[13],Vector2.one,75);
      /*  foreach (var transform in menu)
        {
           // transform.gameObject.GetComponent<Button>().onClick.AddListener(delegate { ExpandEntetieMenu(transform); });
        }
        foreach (Transform child in menuParent)
        {
            buttonText.Add(child.GetComponentInChildren<Text>());
        }
        Search();
    }
    void Update()
    {
       // searchMonsterManual.RollResultsIndex(buttonText);
    }
    public void Search()
    {
        List<string> newList;
        newList = searchMonsterManual.search(inputField,monsterManual.fieldsData, 0);
        for (int i = 0; i < buttonText.Count; i++)
        {
            buttonText[i].text = "";
        }
        for (int i = 0; i < newList.Count && i < buttonText.Count; i++)
        {
            buttonText[i].text = newList[i];
        }
    }
   /* void ExpandEntetieMenu(Transform transform)
    {
        var items = new string[3];
        items[0] = "Add";
        items[1] = "Edit";
        items[2] = "Remove";
        if (monsterManual.fieldsData[0].Contains(transform.GetComponentInChildren<Text>().text))
        {
            if (tempReference == null)
            {
                tempReference = InstantiateMenu.Create(prefab, transform, items, new Vector2(0, 1), 75);
                foreach (var item in tempReference)
                {
                    switch (item.GetSiblingIndex())
                    {
                        case 1:
                            item.GetComponentInChildren<Text>().text = items[0];
                            item.GetComponent<Button>().onClick.RemoveAllListeners();
                            item.GetComponent<Button>().onClick.AddListener(delegate { AddEntetie(item, monsterManual); });
                            break;
                        case 2:
                            item.GetComponentInChildren<Text>().text = items[1];
                            item.GetComponent<Button>().onClick.RemoveAllListeners();
                            item.GetComponent<Button>().onClick.AddListener(delegate { EditEntetie(item, monsterManual); });
                            break;
                        case 3:
                            item.GetComponentInChildren<Text>().text = items[2];
                            item.GetComponent<Button>().onClick.RemoveAllListeners();
                            //item.GetComponent<Button>().onClick.AddListener(delegate { AddEntetie(item, monsterManual); });
                            break;
                    }
                }
            }
            else
            {
                if (tempReference[0].parent != transform && tempReference.Count == items.Length)
                {
                    Shufler.Shufle(tempReference, transform);
                    foreach (var item in tempReference)
                    {
                        switch (item.GetSiblingIndex())
                        {
                            case 1:
                                item.GetComponentInChildren<Text>().text = items[0];
                                item.GetComponent<Button>().onClick.RemoveAllListeners();
                                item.GetComponent<Button>().onClick.AddListener(delegate { AddEntetie(item, monsterManual); });
                                break;
                            case 2:
                                item.GetComponentInChildren<Text>().text = items[1];
                                item.GetComponent<Button>().onClick.RemoveAllListeners();
                                item.GetComponent<Button>().onClick.AddListener(delegate { EditEntetie(item, monsterManual); });
                                break;
                            case 3:
                                item.GetComponentInChildren<Text>().text = items[2];
                                item.GetComponent<Button>().onClick.RemoveAllListeners();
                                //item.GetComponent<Button>().onClick.AddListener(delegate { AddEntetie(item, monsterManual); });
                                break;
                        }
                    }
                }
                else
                {
                    if (tempReference[0].parent == transform)
                        Shufler.ShowHide(tempReference);
                    if (tempReference.Count != items.Length)
                    {
                        foreach (var item in tempReference)
                        {
                            Destroy(item.gameObject);
                        }
                        tempReference = InstantiateMenu.Create(prefab, transform, items, new Vector2(0, 1), 75);
                        foreach (var item in tempReference)
                        {
                            switch (item.GetSiblingIndex())
                            {
                                case 1:
                                    item.GetComponentInChildren<Text>().text = items[0];
                                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                                    item.GetComponent<Button>().onClick.AddListener(delegate { AddEntetie(item, monsterManual); });
                                    break;
                                case 2:
                                    item.GetComponentInChildren<Text>().text = items[1];
                                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                                    item.GetComponent<Button>().onClick.AddListener(delegate { EditEntetie(item, monsterManual); });
                                    break;
                                case 3:
                                    item.GetComponentInChildren<Text>().text = items[2];
                                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                                    //item.GetComponent<Button>().onClick.AddListener(delegate { AddEntetie(item, monsterManual); });
                                    break;
                            }
                        }
                        }
                }
            }
        }
    }
    void AddEntetie(Transform transform, EntetieList entetieList)
    {

        Entetie entetie;
        string entetieName = transform.parent.GetComponentInChildren<Text>().text;
        foreach (var _entetie in entetieList.Entities)
        {
            foreach (var nameField in _entetie.FieldValues)
            {
                if(nameField.Contains(entetieName))
                {
                    entetie = _entetie;
                    break;
                }
            }
        }
        GameObject gam = new GameObject(entetieName, typeof(SpriteRenderer));
        SpriteRenderer spriteRenderer = gam.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("skull-icon-png-3");
        gam.AddComponent<DragHandler>().draggin = true;
        spriteRenderer.sortingOrder = 1;
        var col = gam.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        gam.transform.localScale /= 5;
        //InstantiateMenu.ShowHide(tempReference);
    }
  /*  void EditEntetie(Transform transform, EntetieList entetieList)
    {
            Entetie entetie = null;
        List<string> fields = new List<string>();
        string entetieName = transform.parent.GetComponentInChildren<Text>().text;
            foreach (var _entetie in entetieList.Entities)
            {
                foreach (var nameField in _entetie.FieldValues)
                {
                    if (nameField.Contains(entetieName))
                    {
                        entetie = _entetie;
                        break;
                    }
                }
            }
        if (tempReference == null)
        {
            tempReference = InstantiateMenu.Create(prefab, transform, entetie.FieldNames.ToArray(), new Vector2(-1, 1), 30);
            foreach (var field in entetie.FieldNames)
            {
                fields.Add(field);
            }
            foreach (var field in tempReference)
            {
                field.gameObject.GetComponentInChildren<Text>().text = fields[field.GetSiblingIndex() - 1];
            }
        }
        else if (tempReference[0].parent != transform && tempReference.Count == entetie.FieldNames.Count)
        {
            Shufler.Shufle(tempReference,transform);
        }
        else 
        {
            if (tempReference[0].parent == transform)
                Shufler.ShowHide(tempReference);
            if(tempReference.Count != entetie.FieldNames.Count)
            {
                InstantiateMenu.Create(prefab, transform, entetie.FieldNames.ToArray(), new Vector2(-1, 1), 30);
                foreach (var field in entetie.FieldNames)
                {
                    fields.Add(field);
                }
                foreach (var field in tempReference)
                {
                    field.gameObject.GetComponentInChildren<Text>().text = fields[field.GetSiblingIndex() - 1];
                }

            }
        }
    }  
}
*/