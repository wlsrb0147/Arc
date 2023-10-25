using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyFolder.Script.InventoryScript
{
    public class ItemDatabase : MonoBehaviour
    {
        public List<Items> Allitems = new List<Items>();

        /*public List<Items> FindItemByName(string name)
        {
            return Allitems.Where(item => item.ItemName == name).ToList();
        }*/

        public Items FindItemByName(string name)
        {
            return Allitems.FirstOrDefault(item => item.ItemName == name);
        }
    }
}