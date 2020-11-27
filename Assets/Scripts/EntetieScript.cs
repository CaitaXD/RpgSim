using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EntetieScript : MonoBehaviour
{
    public Entetie Entetie;
    private int items;
    private float timer;
    private DropDownMenu infoMenu;
    private UnityAction State;
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
                        var uiNav = GameObject.Find("Canvas").GetComponentInChildren<LeftMenuUI>();
                        var offSettedPivot = new Vector2((uiNav.pivot.x - 1) * (-1), uiNav.pivot.y);

                        infoMenu = new DropDownMenu(uiNav.prefab, uiNav.parent,3, offSettedPivot, 75);
                        infoMenu.SetPosition(pos);

                        DelegateDestroyerButtom(2, "Remove");

                        LeftMenuUI.TempMenusRef.Add(infoMenu);
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
    private void DelegateDestroyerButtom(int index, string text)
    {
        var buttonReference = infoMenu.ReferenceList[index];
        buttonReference.GetComponentInChildren<Text>().text = text;
        infoMenu.AddListeners(delegate
        {
            Destroy(gameObject);
            infoMenu.Destroy();
        }, buttonReference);
    }
}