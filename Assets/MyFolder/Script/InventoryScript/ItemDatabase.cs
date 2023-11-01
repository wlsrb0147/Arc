using System.Collections.Generic;
using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class ItemDatabase : MonoBehaviour
    {
        public static ItemDatabase instance;
        public List<Items> Allitems = new();

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
            else
                instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}