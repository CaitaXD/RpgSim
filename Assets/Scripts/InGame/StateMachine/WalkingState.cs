using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkingState : State
{
    float lerpingFloat = -1;  
    Vector3 initPos;
    Vector3 destination = Vector3.positiveInfinity;
    int walkingSpeed;
    string attribute = "Speed";
    public WalkingState(EntetieScript entetieScript) : base(entetieScript)
    {
    }
    private int GetAtrtibuteValue(string attribute)
    {
        string speedField = entetieScript.Entetie.fields[attribute];
        List<char> numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        int number = 0;
        string str = "";
        string[] speeds = speedField.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < speeds[0].Length; i++)
        {
            if (numbers.Contains(speeds[0][i]))
            {
                str = str + speeds[0][i];
            }
        }
        number = int.Parse(str);
        return number;
    }
    public override IEnumerator Start()
    {
        EntetieSearchUI.isPointerDraggin = true;
        walkingSpeed = GetAtrtibuteValue(attribute) /5;
        initPos = entetieScript.transform.position;
        entetieScript.HighLightGridCircle(initPos, walkingSpeed, Color.green);
        yield break;
    }
    public override void Update()
    {
        SetDestination(walkingSpeed, initPos);
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        if (lerpingFloat > 0)
        {
            lerpingFloat += Time.deltaTime * 1.25f;
            entetieScript.transform.position = Vector3.Lerp(initPos, destination, lerpingFloat);
            if (Vector3.Distance(entetieScript.transform.position, destination) == 0)
            {
                Place();
                return;
            }
        }
    }

    private void SetDestination(int WalkingSpeed, Vector3 initPos)
    {
        Commands.ClearDropDownMenus();
        entetieScript.gameObject.layer = 2;
        EntetieSearchUI.isPointerDraggin = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        List<GameObject> AvaliableSquares = new List<GameObject>();
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButton(0))
            {  
                destination = hit.transform.position;               
                entetieScript.HighLightGridCircle(initPos, WalkingSpeed, Color.white);
                var dis = Vector3.Distance(initPos, hit.transform.position) / entetieScript.Testing.spacing;
                if (dis <= WalkingSpeed + entetieScript.Testing.grid.roundingErrorOffset && hit.transform.name == "Tile")
                {
                    lerpingFloat = 0.125f;
                    SelectionScript.SetNextEntetieToFirstIndex(entetieScript);
                }
                else CancelMovement(initPos);
            }
        }
    }
    protected override void CancelMovement(Vector3 initPos)
    {
        entetieScript.gameObject.layer = 0;
        entetieScript.transform.position = initPos;
        entetieScript.SetState(new IdleState(entetieScript));
    }
    protected override void Place()
    {
        entetieScript.gameObject.layer = 0;
        entetieScript.SetState(new IdleState(entetieScript));
    }
}
