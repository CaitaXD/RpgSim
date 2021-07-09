using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScript : MonoBehaviour
{
    [SerializeField] GameObject SelectinGUIPrefab;
    [SerializeField] Camera _myCam;
    [SerializeField] BoxSelector BoxSelector;
    Vector3 initMousePos;
    Vector3 DragPos;
    static int count = 0;
    public GameObject SelectionGUI() => Instantiate(SelectinGUIPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
    public static bool AnySelectedEntetieNotIdle()  =>  SelectedEnteties.Any(x => x.GetState().GetType() != typeof(IdleState));
    float timer;
    float timeFrame = 0.25f;
    Ray ray;
    public static List<EntetieScript> SelectedEnteties = new List<EntetieScript>();
    [SerializeField]Collider[] colliders;
    private void Awake()
    {
        _myCam = _myCam ? _myCam : Camera.main;
    }
    void Update()
    {   
        BoxEntetieSelection(timeFrame);
        HandleEntetiesInput();
        TrackSelectionGUI();
    }
    void BoxEntetieSelection(float timeFrame)
    {       
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if (timer > timeFrame && !Input.GetKey(KeyCode.LeftShift)) UnsellectAll();
            RaycastHit hit;
            ray = _myCam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (Input.GetMouseButtonDown(0))
            {
                initMousePos = hit.point;
                BoxSelector.min = initMousePos;
            }
            DragPos = hit.point;
            BoxSelector.max = DragPos;
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            timer = 0;
            colliders = Physics.OverlapBox(BoxSelector.center, BoxSelector.extends, Quaternion.identity);
            int firstOccurence = -1;
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider col = colliders[i];
                var enteteiScript = col.GetComponent<EntetieScript>();
                if (enteteiScript != null)
                {
                    if (firstOccurence < 0) firstOccurence = i;
                    if (firstOccurence >= i && !Input.GetKey(KeyCode.LeftShift)) UnsellectAll();        
                    Select(enteteiScript);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(BoxSelector.center, BoxSelector.size);
    }
    void TrackSelectionGUI()
    {
        foreach (EntetieScript entetieSelected in SelectedEnteties)
        {
            if (entetieSelected.SelectionGUI == null)
            {
                entetieSelected.SelectionGUI = SelectionGUI();
                AssignSelectionGUI(entetieSelected.gameObject, entetieSelected.SelectionGUI, _myCam);
            }
            else AssignSelectionGUI(entetieSelected.gameObject, entetieSelected.SelectionGUI, _myCam);
        }
    }
    void HandleEntetiesInput()
    {
        for (int i = 0; i < SelectedEnteties.Count; i++)
        {
            EntetieScript entetie = SelectedEnteties[i];
            State entetieState = entetie.GetState();
            if (AnySelectedEntetieNotIdle()) return;
            entetieState.HanldeInput();
        }
    }
    public static void SetNextEntetieToFirstIndex(EntetieScript entetie)
    {
        SelectedEnteties.Remove(entetie);
        SelectedEnteties.Add(entetie);
    }

    public static void Select(List<EntetieScript> SelectedEnteties)
    {
        for (int i = 0; i < SelectedEnteties.Count; i++)
        {
            EntetieScript entetieSelected = SelectedEnteties[i];
            SelectedEnteties.Add(entetieSelected);
        }
    }
    public void Select(EntetieScript entetieSelected)
    {
        SelectedEnteties.Add(entetieSelected);
    }
    public static void Unsellect(List<EntetieScript> SelectedEnteties)
    {
        while(SelectedEnteties.Count > 0)
        {
            Unsellect(SelectedEnteties[SelectedEnteties.Count - 1]);
        }
    }
    public static void Unsellect(EntetieScript SelectedEntetie)
    {
        SelectedEnteties.Remove(SelectedEntetie);
        OnUnselect(SelectedEntetie);
    }
    public void UnsellectAll()
    {
        Unsellect(SelectedEnteties);
    }
    static void OnUnselect(EntetieScript SelectedEntetie)
    {
        Destroy(SelectedEntetie.SelectionGUI);
    }
    void AssignSelectionGUI(GameObject selected, GameObject selectionGUI,Camera myCam)
    {
        RectTransform rectT = selectionGUI.GetComponent<RectTransform>();
        Rect rect = GUIRectWithObject(selected, myCam);

        rectT.position = new Vector2(rect.xMin, rect.yMin);
        rectT.sizeDelta = new Vector2(rect.width, rect.height);
    }
    public static Rect GUIRectWithObject(GameObject go, Camera myCam)
    {
        Vector3 cen = go.GetComponent<Renderer>().bounds.center;
        Vector3 ext = go.GetComponent<Renderer>().bounds.extents;
        Vector2[] extentPoints = new Vector2[8]
        {
            myCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
            myCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
        };
        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];
        foreach (Vector2 v in extentPoints)
        {
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }
       return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }
    public static void HandleNextMove()
    {
        if(count < SelectedEnteties.Count -1)
        {
            EntetieScript first = SelectedEnteties[0];
            first.SetState(new WalkingState(first));
            count++;
        } else count = 0;        
    }
}

[System.Serializable]
public class BoxSelector
{
    public Vector3 min, max;

    public Vector3 center
    {
        get
        {
            Vector3 center = min + (max - min) / 2;
            center.z = (max - min).magnitude / 2;
            return center;
        }
    }
    public Vector3 size
    {
        get
        {
            return new Vector3(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y), (max - min).magnitude);
        }
    }
    public Vector3 extends
    {
        get
        {
            return size / 2;
        }
    }
}