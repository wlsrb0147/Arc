using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class ItemDatabase : MonoBehaviour
    {
        public List<Items> Allitems = new List<Items>();
 
        public static ItemDatabase instance;
        private void Awake()
        {
            if (instance != null && instance!=this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
    
}