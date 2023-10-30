using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class InventoryCommander : MonoBehaviour
    {
        public static InventoryCommander instance;
        private string _itemName;
        private bool _equipped;
        private ClickType _clickType;
        public ItemEvent SelectedItem { get; set; }
        
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

        public void SetItemInfo(string itemName, bool equipped)
        {
            _itemName = itemName;
            _equipped = equipped;
        }

        public void ClickType(ClickType clickType)
        {
            switch (clickType)
            {
                case InventoryScript.ClickType.LeftClick:
                    break;
                case InventoryScript.ClickType.RightClick:
                    ItemManager.instance.ChangeItem(_itemName,_equipped);
                    ItemManager.instance.RefreshItem();
                    break;
                case InventoryScript.ClickType.DoubleLeftClick:
                    break;
            }
        }

        public void CreateItemCommand()
        {
            ItemManager.instance.CreateItem();
            ItemManager.instance.RefreshItem();
        }

        public void DeleteItemCommand()
        {
            ItemManager.instance.DeleteItem();
            ItemManager.instance.RefreshItem();
        }
    }
}
