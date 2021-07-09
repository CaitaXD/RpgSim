using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public Transform currentTarget;
    public bool wasRayCasted;
    public IdleState(EntetieScript entetieScript) : base(entetieScript)
    {

    }
    public override IEnumerator Start()
    {
        KeyBinds = new Dictionary<KeyCode, Action>()
        {
            { 
                KeyCode.M, delegate
                {
                    entetieScript.SetState(new WalkingState(entetieScript));
                } 
            },
            { 
                KeyCode.D,  delegate
                {
                    EntetieManager.AlterFieldValue(entetieScript, "HitPoints", -1);
                }
            },
            { 
                KeyCode.T,  delegate
                {
                    currentTarget = EntetieManager.Target(entetieScript, Camera.main);
                }
            },
            { 
                KeyCode.Escape,  delegate
                {
                    currentTarget = null;
                }
            }
        };
        EntetieSearchUI.isPointerDraggin = false;
        return base.Start();
    }
    public override void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == entetieScript.gameObject)
        {
            HandleInput();
        }
    }
    public void HandleInput()
    {
        if (Input.GetMouseButtonUp(0) && EntetieSearchUI.isPointerDraggin == false)
        {
            wasRayCasted = false;
        }
        if (Input.GetMouseButtonUp(1) && EntetieSearchUI.isPointerDraggin == false)
        {
            entetieScript.ShowInteractions();
        }
    }
}
