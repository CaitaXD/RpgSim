using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindSearchToMenuType : MonoBehaviour
{
    private Button[] Buttons;
    private List<Vector3> rectOrginPos = new List<Vector3>();
    private bool interacted = false;
    [SerializeField] LeftMenuUI leftUI;
    [SerializeField] Transform SearchMenusTransfrom;
    [SerializeField] Transform InputFieldTransform;
    private void Start()
    {
        Buttons = SearchMenusTransfrom.GetComponentsInChildren<Button>();
        foreach(var button in Buttons )
        {
            rectOrginPos.Add(button.GetComponent<RectTransform>().position);
            button.onClick.AddListener(delegate{ SelectSearchType(button.gameObject); });
        }
    }
     private void SelectSearchType(GameObject currentButtonObject)
    {
        if(interacted == false)
        {
            foreach (var button in Buttons) button.gameObject.SetActive(!button.IsActive());
            currentButtonObject.SetActive(!currentButtonObject.activeInHierarchy);

            var currentButtonRectTransform = currentButtonObject.GetComponent<RectTransform>();
            var topPosition = Buttons[0].GetComponent<RectTransform>().position;

            if (currentButtonObject.activeInHierarchy) currentButtonRectTransform.position = topPosition;
            interacted = true;
   
            InputFieldTransform.SetParent(currentButtonObject.transform);
            InputFieldTransform.gameObject.SetActive(true);
            InputFieldTransform.position = new Vector3(topPosition.x,topPosition.y -45,topPosition.z);
            string path = Application.streamingAssetsPath + @"\" + InputFieldTransform.parent.name + ".json";
            leftUI.DrawMonsterManual(path);
        }
        else
        {
            foreach (var button in Buttons)
            {
                button.gameObject.SetActive(true);
                button.GetComponent<RectTransform>().position = rectOrginPos[button.transform.GetSiblingIndex()];
            }
            InputFieldTransform.gameObject.SetActive(false);
            interacted = false;
        }
     }
}
