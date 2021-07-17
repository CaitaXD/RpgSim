using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    Transform currentTargetTransform => entetieScript.currentTarget;
    EntetieScript currentTargetEntetieScript;
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
                    if(currentTargetTransform != null) currentTargetEntetieScript = currentTargetTransform.GetComponent<EntetieScript>();
                    if(currentTargetEntetieScript != null) entetieScript.AlterFieldValue(currentTargetEntetieScript, "HitPoints", -1);
                }
            },
            { 
                KeyCode.T,  delegate
                {
                    entetieScript.currentTarget = entetieScript.Target(entetieScript, Camera.main); 
                    entetieScript.DrawArrow(currentTargetTransform);
                    if(Input.GetKey(KeyCode.LeftControl)) SelectionScript.SetNextEntetieToFirstIndex(entetieScript);
                }
            },
            { 
                KeyCode.Escape,  delegate
                {
                    entetieScript.currentTarget = null;
                    currentTargetEntetieScript = null;
                    entetieScript.lineRenderer.enabled = false;
                }
            },
            { 
                KeyCode.R,  delegate
                {
                    
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
