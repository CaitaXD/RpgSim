﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Commands
{
    public static EntetieList LoadMonsterManual()
    {
        Tuple<List<List<string>>,List<List<string>>> data = CustomJsonDeserializer.DeserializeFromJson(@"Assets\ExternalData\5emonsters.json");
        EntetieList monsterManual = new EntetieList(data.Item1,data.Item2);
        return monsterManual;
    }
    //==========================================================================================================
    public static Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    //==========================================================================================================
    public static void ClearPopUps()
    {
        foreach (var popUp in LeftMenuUI.ClosePopUpReferenceList)
        {
            if(popUp != null)popUp.Disable();
        }
    }
    //===========================================================================================================
}
