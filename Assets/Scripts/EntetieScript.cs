using UnityEngine;
using UnityEngine.Events;

public class EntetieScript : MonoBehaviour
{
    float timer;
    public Entetie Entetie;
    private string[] ShowInfo = new string[4];
    private DropDownMenu infoMenu;
    UnityAction State;
    private void Awake()
    {
        State = MovingState;
    }
    private void Update()
    {
        State();
    }
    private void MovingState()
    {
        gameObject.layer = 2;
        LeftMenuUI.isPointerDraggin = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            transform.position = hit.transform.position;
            if (Input.GetMouseButtonDown(0) && LeftMenuUI.isPointerDraggin)
            {
                if (hit.transform.name == "Tile")
                {
                    State = LockedState;
                    LeftMenuUI.isPointerDraggin = false;
                }
                gameObject.layer = 0;
            }
            if(Input.GetMouseButtonUp(0) && timer > 0.25f)
            {
                if (hit.transform.name == "Tile")
                {
                    State = LockedState;
                    LeftMenuUI.isPointerDraggin = false;
                    timer = 0;
                    gameObject.layer = 0;
                }
            }
        }
    }
    private void OnMouseDrag()
    {
        timer += Time.deltaTime;
    }
    private void LockedState()
    {
        if (Input.GetMouseButtonDown(0) && !LeftMenuUI.isPointerDraggin)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform)
                {
                    State = MovingState;
                    LeftMenuUI.isPointerDraggin = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform == transform)
                {
                    var pos = Input.mousePosition;
                    if (infoMenu == null)
                    {
                        ShowInfo[0] = Entetie.GetName;
                        ShowInfo[1] = "Hit Points: Null/Null";
                        ShowInfo[2] = "State: Null";
                        ShowInfo[3] = "Remove";

                        var uiNav = GameObject.Find("Canvas").GetComponent<LeftMenuUI>();
                        var offSettedPivot = new Vector2((uiNav.pivot.x - 1) * (-1), uiNav.pivot.y);

                        infoMenu = new DropDownMenu(uiNav.prefab, uiNav.parent, ShowInfo, offSettedPivot, 75);
                        infoMenu.SetPosition(pos);

                        LeftMenuUI.ClosePopUpReferenceList.Add(infoMenu);

                        AddListeners(infoMenu);
                    }
                    else if (infoMenu != null && !infoMenu.ReferenceList[0].gameObject.activeInHierarchy)
                    {
                        infoMenu.SetPosition(pos);
                    }
                    else if (infoMenu != null && infoMenu.ReferenceList[0].gameObject.activeInHierarchy)
                    {
                        infoMenu.EnableDisable();
                    }
                }            
            }
        }        
    }
    private void AddListeners(DropDownMenu menu)
    {
        foreach (var _Text in menu.GetTexts())
        {
            switch (_Text.text)
            {
                case "Remove":
                    menu.AddListeners(
                        delegate {
                            menu.Destroy();
                            GameObject.Destroy(gameObject);
                        },_Text.transform);
                    break;
            }
        }
    }
}