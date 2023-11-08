using System;
using System.Collections;
using System.Collections.Generic;
using MyFolder.Script.InventoryScript;
using UnityEngine;

public class FilterPanel : MonoBehaviour
{
    private void OnEnable()
    {
        InventoryCommander.instance.FilterPanelEnabled();
    }
}
