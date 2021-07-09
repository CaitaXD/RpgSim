using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntetieManager
{
    public static Action Result (bool result,Action OnSucess ,Action OnFaliure)
    {
        if(result) return OnSucess;
        else return OnFaliure;
    }
    public static bool Contest(EntetieScript entetie1, EntetieScript entetie2, string Field1, string Field2)
    {
        int f1, f2;
        f1 = GetAtrtibuteValue(entetie1, Field1);
        f2 = GetAtrtibuteValue(entetie2, Field2);
        if (f1 > f2) return true;
        else  return false;
    }
    public static bool Contest(EntetieScript entetie, string Field1, string Field2)
    {
        int f1, f2;
        f1 = GetAtrtibuteValue(entetie, Field1);
        f2 = GetAtrtibuteValue(Field2);
        if (f1 > f2) return true;
        else  return false;
    }
    public static void AlterFieldValue(EntetieScript entetieScript,string field,int value)
    {
        int fieldValue = GetAtrtibuteValue(entetieScript, field);
        fieldValue = fieldValue + value;
        entetieScript.fields[field] = fieldValue.ToString();
    }
    public static int GetAtrtibuteValue(EntetieScript entetieScript,string attribute)
    {
        string speedField = entetieScript.fields[attribute];
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
     static int GetAtrtibuteValue(string field)
    {
        List<char> numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        int number = 0;
        string str = "";
        string[] speeds = field.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
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
    public static Transform Target(EntetieScript entetie, Camera myCam)
    {
        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform;
        }
        return null;
    }
}
