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
}