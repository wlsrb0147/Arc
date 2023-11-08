using MyFolder.Script;
using MyFolder.Script.InventoryScript;
using UnityEngine;

public class BT_Manager : MonoBehaviour
{
    public void Tbt(int buttonNumber)
    {
        UI_Manager.instance.tempSelectedTopButton = buttonNumber;
        UI_Manager.instance.isSomethingChanged = true;
    }

    public void Lbt(int tempLeft)
    {
        var topMax = tempLeft == 10 ? 5 : 3;

        UI_Manager.instance.tempSelectedLeftButton = tempLeft;
        UI_Manager.instance.tempSelectedTopButton = 1;
        UI_Manager.instance.tobButtonMax = topMax;
        UI_Manager.instance.isSomethingChanged = true;
    }

    public void CreateItem()
    {
        InventoryCommander.CreateItemCommand();
    }

    public void DeleteItem()
    {
        InventoryCommander.DeleteItemCommand();
    }

    public void CancelFilter()
    {
        InventoryCommander.instance.CancelFilter();
    }

    public void ApplyFilter()
    {
        InventoryCommander.instance.ApplyFilter();
    }

    public void ActiveFilterTab()
    {
        InventoryCommander.instance.ActiveFilterTab();
    }

    public void CharToggle(int value)
    {
        InventoryCommander.instance.SetCharType(value);
    }

    public void EquToggle(int value)
    {
        InventoryCommander.instance.SetItemType(value);
    }

    public void AllItemCheck(bool value)
    { 
        InventoryCommander.instance.AllItemCheck(value);   
    }

    public void AllCharCheck(bool value)
    {
        InventoryCommander.instance.AllCharCheck(value);
    }
    
}