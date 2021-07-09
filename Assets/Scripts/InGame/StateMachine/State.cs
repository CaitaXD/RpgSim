using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected EntetieScript entetieScript;
    protected Dictionary<KeyCode, Action> KeyBinds = new Dictionary<KeyCode, Action>()
    {
        {KeyCode.Escape, null}
    };
    public void HanldeInput() 
    {
        foreach (var KeyValuePair in KeyBinds)
        {
            if (Input.GetKeyDown(KeyValuePair.Key))
            {
                if (KeyValuePair.Value == null) break;
                KeyValuePair.Value();
            }
        }
    }
    public State(EntetieScript entetieScript)
    {
        this.entetieScript = entetieScript;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }
    public virtual void Update()
    {

    }
    protected virtual void Place()
    {
    }
    protected virtual void CancelMovement(Vector3 initPos)
    {
        entetieScript.transform.position = initPos;
    }
}
