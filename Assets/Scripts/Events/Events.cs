using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events current;

    public void Awake()
    {
        current = this;
    }

    public event Action onSelectionChange;
    public event Action onSelectionClearing;
    public event Action onUpdateEntetie;
    public void SelectionChange() => onSelectionChange?.Invoke();
    public void ClearSelection() => onSelectionClearing?.Invoke();
    public void UpdateEntetie() => onUpdateEntetie?.Invoke();
}
