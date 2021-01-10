using UnityEngine;
using UnityEngine.UI;

public class AddEntry : MonoBehaviour
{
    [SerializeField] InputField inputField;
    private int fieldsAmount;
    [SerializeField] GameObject prefab;
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
        OnInputFieldValueChange();
    }
    private void OnInputFieldValueChange()
    {
        void DrawFields()
        {
            if (Fields == null)
            {
                Fields = new DropList(prefab, NewEntryWindow, fieldsAmount, pivot, 20);
                Data = new DropList(prefab, NewEntryWindow, fieldsAmount, new Vector2(pivot.x - 1,pivot.y), 20);
            }
            else
            {
                Fields.Resize(fieldsAmount);
                Data.Resize(fieldsAmount);
            }
        }
        inputField.onValueChanged.AddListener(delegate {
            var value = inputField.text;
            if (value != null)
            {
                if (Fields != null)
                {
                    //Fields.Destroy();
                    if (value != "")
                    {
                        fieldsAmount = int.Parse(value);
                        if (fieldsAmount > 45)
                        {
                            fieldsAmount = 45;
                            inputField.text = fieldsAmount.ToString();
                        }
                        else if (fieldsAmount < 0)
                        {
                            fieldsAmount = 0;
                            inputField.text = fieldsAmount.ToString();
                        }
                        else
                        {
                            DrawFields();
                        }
                    }
                }
                else if (Fields == null)
                {
                    fieldsAmount = int.Parse(value);
                    DrawFields();
                }
            }
        });
    }
    private void AddSheetWindowListener()
    {
        AddEntryWindow.GetComponent<Button>().onClick.AddListener(delegate {
            onOff = !onOff;
            NewEntryWindow.gameObject.SetActive(onOff);
        });
    }
}
