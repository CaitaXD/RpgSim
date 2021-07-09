using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingState : State
{
    public float timer;
    public DraggingState(EntetieScript entetieScript) : base(entetieScript)
    {
    }
    public override void Update()
    {
        Dragging();
    }
    public override IEnumerator Start()
    {
        EntetieSearchUI.isPointerDraggin = true;
        return base.Start();
    }
    public void HandleLeftMouseButton(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) && EntetieSearchUI.isPointerDraggin)
        {
            if (hit.transform.name == "Tile")
            {
                Place();
                entetieScript.SetState(new IdleState(entetieScript));
                return;
            }
        }
        if (Input.GetMouseButtonUp(0) && timer > 0.25f)
        {
            if (hit.transform.name == "Tile")
            {
                Place();
                return;
            }
        }
    }
    private void Dragging()
    {
        timer += Time.deltaTime;
        entetieScript.gameObject.layer = 2;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            entetieScript.MoveEntetie(hit.transform.position);
            HandleLeftMouseButton(hit);
        }
    }
    protected override void Place()
    {
        entetieScript.gameObject.layer = 0;
        timer = 0;
        entetieScript.SetState(new IdleState(entetieScript));
        base.Place();
    }
}
