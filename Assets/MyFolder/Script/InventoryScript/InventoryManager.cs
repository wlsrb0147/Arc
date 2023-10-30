using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager instance;
        private string _itemName;
        private bool _equipState;
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

        public void SetInfo(string itemName, bool equipstate)
        {
            _itemName = itemName;
            _equipState = equipstate;
        }

        public void ClickType(ClickType clickType)
        {
            switch (clickType)
            {
                case InventoryScript.ClickType.LeftClick:
                    break;
                case InventoryScript.ClickType.RightClick:
                    break;
                case InventoryScript.ClickType.DoubleLeftClick:
                    break;
            }
        }
    
    
    }
}
