using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIteractions : MonoBehaviour
{
    [SerializeField] Transform gridTransform;
    [SerializeField] Testing testing;
    public Dictionary<string, string> SavedInteractions = new Dictionary<string, string>
    {
        { "Name", "Command" },
    };
    public Vector3 Target(float range)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return (hit.transform.position- new Vector3(gridTransform.position.x,gridTransform.position.y,0))/testing.spacing;
        }
        return Vector3.zero;
    }
    public bool Contest(float contestant, float defender)
    {
        return true;
    }
    public Action OnSuccess(Action action)
    {
        return action;
    }
    public Action OnFaliure(Action action)
    {
        return action;
    }
    public int Roll(int amount, int dice)
    {
        return 0;
    }
    public void Damage(float duration ,float damgaeValue, string damagedField, Entetie entetie)
    {
    }
}
