﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EntetieScript : EntetieStateMachine
{
    public Transform currentTarget;
    public Entetie Entetie;
    public Sprite _sprite;
    public Dictionary<string,string> fields = new Dictionary<string, string>();
    private DropList infoMenu;
    public GameObject prefab;
    public Transform parent;
    public Vector2 pivot;
    public Testing Testing;
    public bool SelectedCurrently;
    public GameObject SelectionGUI;
    public LineRenderer lineRenderer;
    bool isAlive = true;
    public bool IsAlive() { if(Health <= 0) return isAlive = false; else return isAlive = true;}
    public int Health;
    public int optionsAmount = 3;

    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sprite = _spriteRenderer.sprite;
        Testing = GameObject.Find("Scripts").GetComponent<Testing>();
        parent = parent ? parent : GameObject.Find("Canvas").GetComponent<Transform>();
        prefab = prefab ? prefab : Resources.Load("Prefabs/Ui/UiText") as GameObject;
        lineRenderer = lineRenderer ? lineRenderer : Instantiate((Resources.Load("Prefabs/Ui/DeafultTargetLineDrawer") as GameObject).GetComponent<LineRenderer>(),transform);
        SetState(new DraggingState(this));
    }
    public int GetAtrtibuteValue(string attribute)
    {
        List<char> numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-'};
        int number = 0;
        string str = "";
        string[] speeds = attribute.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
        speeds = attribute.Split(new string[] { "(" }, System.StringSplitOptions.RemoveEmptyEntries);
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
    public int GetAtrtibuteValue(EntetieScript target,string attribute)
    {
        string speedField = target.fields[attribute];
        List<char> numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-' };
        int number = 0;
        string str = "";
        string[] speeds = speedField.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
        speeds = speedField.Split(new string[] { "(" }, System.StringSplitOptions.RemoveEmptyEntries);
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
    private void Update()
    {
        Health = GetAtrtibuteValue(fields["HitPoints"]);
        State.Update();
    }
    public void ShowInteractions()
    {
        var pos = Input.mousePosition;
        if (infoMenu == null)
        {
            InstantiateInfoMenu(pos);
            DelegateButtons();
            EntetieSearchUI.DropListTracker.Add(infoMenu);
        }
        if (infoMenu.IsActive()) infoMenu.EnableDisable();
        else infoMenu.SetPosition(pos);

        //=== Internal Methods ===

        void InstantiateInfoMenu(Vector3 position)
        {
            var offSettedPivot = new Vector2((pivot.x - 1) * (-1), pivot.y);
            infoMenu = new DropList(prefab, parent, optionsAmount, offSettedPivot, 20);
            infoMenu.SetPosition(position);
        }
        void DelegateButtons()
        {
            InstantiateButton(0, "Move", delegate
            {
                SetState(new WalkingState(this));
            });
            InstantiateButton(1, "Roll", delegate
            {
                //TODO: Roll Dice with entetie specific modifiers
            });
            InstantiateButton(2, "Remove", delegate
            {
                Destroy(gameObject); infoMenu.Destroy();
            });
        }

        //=== Internal Methods ===
    }
    public void MoveEntetie(Vector3 position)
    {
        transform.position = position;
    }
    private Vector3 WorldPositionToGrid(Vector3 WorldPos, float MultiplicativeConstant, Vector3 AdditiveCosntant)
    {
        return new Vector3(
             (WorldPos.x * MultiplicativeConstant + AdditiveCosntant.x),
             (WorldPos.y * MultiplicativeConstant + AdditiveCosntant.y),
             WorldPos.z);
    }
    public void HighLightGridCircle(Vector3 center, int radius, Color color)
    {
        var offset = Testing.grid.roundingErrorOffset;
        Vector3 grid = WorldPositionToGrid(center, 1/Testing.spacing, - Testing.grid.AnglePosOffset);;

        for (int i = (int)grid.x, x = (int)grid.x; i <= radius + grid.x + 1; i++, x--)
            for (int j = (int)grid.y, y = (int)grid.y; j <= radius + grid.y + 1; j++, y--)
            {
                HighLightFirstQaudrant(center, radius, color, offset, i, j);
                HighLightSecondQuadrant(center, radius, color, offset, x, j);
                HighLightThridQuadrant(center, radius, color, offset, x, y);
                HighLightFourthQuadrant(center, radius, color, offset, i, y);
            }
    }
    private void HighLightFirstQaudrant(Vector3 initPos, int range, Color color, float offset, int i, int j)
    {
        if (i < Testing.grid.width && j < Testing.grid.height && Vector3.Distance(initPos, Testing.grid.GridArray[i, j].transform.position) / Testing.spacing <= range + offset)
            Testing.grid.GridArray[i, j].GetComponentInChildren<SpriteRenderer>().color = color;
    }
    private void HighLightSecondQuadrant(Vector3 initPos, int range, Color color, float offset, int x, int j)
    {
        if (x >= 0 && j < Testing.grid.height && Vector3.Distance(initPos, Testing.grid.GridArray[x, j].transform.position) / Testing.spacing <= range + offset)
            Testing.grid.GridArray[x, j].GetComponentInChildren<SpriteRenderer>().color = color;
    }
    private void HighLightFourthQuadrant(Vector3 initPos, int range, Color color, float offset, int i, int y)
    {
        if (i < Testing.grid.width && y >= 0 && Vector3.Distance(initPos, Testing.grid.GridArray[i, y].transform.position) / Testing.spacing <= range + offset)
            Testing.grid.GridArray[i, y].GetComponentInChildren<SpriteRenderer>().color = color;
    }
    private void HighLightThridQuadrant(Vector3 initPos, int range, Color color, float offset, int x, int y)
    {
        if (x >= 0 && y >= 0 && Vector3.Distance(initPos, Testing.grid.GridArray[x, y].transform.position) / Testing.spacing <= range + offset)
            Testing.grid.GridArray[x, y].GetComponentInChildren<SpriteRenderer>().color = color;
    }
    private void InstantiateButton(int index, string text, Action action)
    {
        var buttonReference = infoMenu._Items[index];
        buttonReference.GetComponentInChildren<Text>().text = text;
        infoMenu.AddListeners(action.Invoke,buttonReference);
    }

    public Action Result (bool result,Action OnSucess ,Action OnFaliure)
    {
        if(result) return OnSucess;
        else return OnFaliure;
    }
    public bool Contest(EntetieScript target, string Field1, string Field2)
    {
        int f1, f2;
        f1 = GetAtrtibuteValue(Field1);
        f2 = GetAtrtibuteValue(target, Field2);
        if (f1 > f2) return true;
        else  return false;
    }
    public void AlterTargetFieldValue(EntetieScript target,string field,int value)
    {
        int fieldValue = GetAtrtibuteValue(target, field);
        fieldValue = fieldValue + value;
        target.fields[field] = fieldValue.ToString();
        Events.current.UpdateEntetie();
    }
    public Transform Target(EntetieScript entetie, Camera myCam)
    {
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform;
        }
        return null;
    }
    public LineRenderer DrawArrow(Transform target)
    {
        if(target == null) return null;
        lineRenderer.enabled = true;
        Vector3 origin = transform.position;
        Vector3 destination = target.position;

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, destination);
        return lineRenderer;
    }
    public LineRenderer DrawLineToCurrentTarget()
    {
        if(currentTarget == null) return null;
        lineRenderer.enabled = true;
        Vector3 origin = transform.position;
        Vector3 destination = currentTarget.position;

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, destination);
        return lineRenderer;
    }
}