using UnityEngine;
using UnityEngine.UI;

public class AddEntry : MonoBehaviour
{
    [SerializeField] int fieldsAmount;
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject newField;
    private GameObject minusField;
    [SerializeField] Transform AddEntryWindow;
    [SerializeField] Transform NewEntryWindow;
    [SerializeField] Vector2 pivot;
    private DropList Fields;
    private DropList Data;
    private bool onOff = false;
    private void Awake()
    {
        NewEntryWindow.gameObject.SetActive(onOff);
        AddSheetWindowListener();
    }
    void IncrementField()
    {
        if(fieldsAmount < 25)
            fieldsAmount++;
        Fields.Resize(fieldsAmount);
        Data.Resize(fieldsAmount);
    }
    void DecrementField()
    {
        if (fieldsAmount > 0)
            fieldsAmount--;
        Fields.Resize(fieldsAmount);
        Data.Resize(fieldsAmount);
    }
    public void DrawFields()
    {
        if (Fields == null)
        {
            fieldsAmount = 1;
            Fields = new DropList(prefab, NewEntryWindow, fieldsAmount, pivot, 30);
            Data = new DropList(prefab, NewEntryWindow, fieldsAmount, new Vector2(pivot.x - 1, pivot.y), 30);
            newField = GameObject.Instantiate(newField,NewEntryWindow);
            var rectP = newField.GetComponent<RectTransform>();
            rectP.anchoredPosition = Vector3.zero;
            rectP.pivot = new Vector2(-4, -6.33f);
            minusField = GameObject.Instantiate(newField, NewEntryWindow);
            var rectM = minusField.GetComponent<RectTransform>();
            rectM.anchoredPosition = Vector3.zero;
            rectM.pivot = new Vector2(-2, -6.33f);
            minusField.GetComponentInChildren<Text>().text = "-";
            Fields._Items[0].GetComponent<InputField>().text = "name";
            var plusButton = newField.GetComponent<Button>();
            plusButton.onClick.AddListener(IncrementField);
            var minusButton = minusField.GetComponent<Button>();
            minusButton.onClick.AddListener(DecrementField);
        }
    }

    private void AddSheetWindowListener()
    {
        AddEntryWindow.GetComponent<Button>().onClick.AddListener(delegate {
            onOff = !onOff;
            NewEntryWindow.gameObject.SetActive(onOff);
        });
    }
}
