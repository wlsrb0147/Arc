using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class InventoryCommander : MonoBehaviour
    {
        public static InventoryCommander instance;
        public string ItemName { get; set; }
        private ClickType _clickType;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        

        public void ClickType(ClickType clickType)
        {
            switch (clickType)
            {
                case InventoryScript.ClickType.LeftClick:
                    break;
                case InventoryScript.ClickType.RightClick:
                case InventoryScript.ClickType.DoubleLeftClick:
                    ItemManager.instance.ChangeItem(ItemName);
                    break;
            }
        }

        public void CreateItemCommand()
        {
            ItemManager.instance.CreateItem();
        }

        public void DeleteItemCommand()
        {
            ItemManager.instance.DeleteItem();
        }
    }
}
