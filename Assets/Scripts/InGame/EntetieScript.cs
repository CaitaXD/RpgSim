using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EntetieScript : MonoBehaviour
{
    public Entetie Entetie;
    private int items;
    private float timer;
    private DropList infoMenu;
    private UnityAction State;
    public GameObject prefab;
    public Transform parent;
    public Vector2 pivot;

    private void Awake()
    {
        parent = parent ? parent : GameObject.Find("Canvas").GetComponent<Transform>();
        prefab = prefab ? prefab : Resources.Load("Prefabs/UiText") as GameObject;
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
                        var offSettedPivot = new Vector2((pivot.x - 1) * (-1), pivot.y);

                        infoMenu = new DropList(prefab, parent,3, offSettedPivot, 20);
                        infoMenu.SetPosition(pos);

                        DelegateDestroyerButtom(2, "Remove");

                        LeftMenuUI.DropListTracker.Add(infoMenu);
                    }
                    else if (infoMenu != null && !infoMenu._Items[0].gameObject.activeInHierarchy)
                    {
                        infoMenu.SetPosition(pos);
                    }
                    else if (infoMenu != null && infoMenu._Items[0].gameObject.activeInHierarchy)
                    {
                        infoMenu.EnableDisable();
                    }
                }            
            }
        }        
    }
    private void DelegateDestroyerButtom(int index, string text)
    {
        var buttonReference = infoMenu._Items[index];
        buttonReference.GetComponentInChildren<Text>().text = text;
        infoMenu.AddListeners(delegate
        {
            Destroy(gameObject);
            infoMenu.Destroy();
        }, buttonReference);
    }
}