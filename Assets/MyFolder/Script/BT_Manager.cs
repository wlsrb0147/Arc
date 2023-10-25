using System;
using System.Collections;
using System.Collections.Generic;
using MyFolder.Script;
using MyFolder.Script.InventoryScript;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public class BT_Manager : MonoBehaviour
{
    public void Tbt(int buttonNumber)
    {
        UI_Manager.UI_instance.tempSelectedTopButton = buttonNumber;
        UI_Manager.UI_instance.istabChanged = true;
    }
    public void Lbt(int tempLeft)
    {
        int topMax;
        
        if (tempLeft == 10)   { topMax = 5; }
        else {topMax = 3; }
        
        UI_Manager.UI_instance.tempSelectedLeftButton = tempLeft;
        UI_Manager.UI_instance.tempSelectedTopButton = 1;
        UI_Manager.UI_instance.tobButtonMax = topMax;
        UI_Manager.UI_instance.istabChanged = true;
    }

    
    private InventoryManager _im;
    private InventoryManager _idb;
    
    
    public void AddList()
    {
        _idb = GameObject.FindGameObjectWithTag("Inven").GetComponent<InventoryManager>();
        ItemDatabase db = GameObject.FindGameObjectWithTag("DB").GetComponent<ItemDatabase>();

        _idb.Additem(db.Allitems[Random.Range(0,db.Allitems.Count)]);
        
    }
    
    public void RemoveList()
    {
        _idb = GameObject.FindGameObjectWithTag("Inven").GetComponent<InventoryManager>();
        ItemDatabase db = GameObject.FindGameObjectWithTag("DB").GetComponent<ItemDatabase>();

        if (_idb.inventoryItemsList.Count != 0)
        {
            _idb.DeleteItem(_idb.inventoryItemsList[Random.Range(0,_idb.inventoryItemsList.Count)]);
        }
    }
}
