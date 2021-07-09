using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    Vector3 initPos;
    int range;
    string actionType, requestedValue;
    public AttackState(EntetieScript entetieScript) : base(entetieScript)
    {
    }
    private int GetAttackValue(string attackType, string desiredValue)
    {
        string field = entetieScript.Entetie.fields["Actions"];
        List<char> numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        int number = 0;
        string str = "";
        string action = field.Substring(field.IndexOf(attackType));
        action = action.Substring(action.IndexOf(desiredValue), action.IndexOf(".") - action.IndexOf(desiredValue));
        for (int i = 0; i < action.Length; i++)
        {
            if (numbers.Contains(action[i]))
            {
                str = str + action[i];
            }
        }
        number = int.Parse(str);
        return number / 5;
    }
    public override IEnumerator Start()
    {
        initPos = entetieScript.transform.position;
        range = GetAttackValue("WeaponAttack", "reach");
        entetieScript.HighLightGridCircle(initPos, range, Color.red);
        yield break;
    }
    public override void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                var dis = Vector3.Distance(initPos, hit.transform.position) / entetieScript.Testing.spacing;
                if (dis <= range + entetieScript.Testing.grid.roundingErrorOffset && hit.transform.name == "Tile")
                {
                    //Place();
                    return;
                }
                entetieScript.HighLightGridCircle(initPos, range, Color.white);
                entetieScript.SetState(new IdleState(entetieScript));
            }
        }
    }
   
}
