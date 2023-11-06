using UnityEngine;
using UnityEngine.UI;

namespace MyFolder.Script.InventoryScript
{
    public class SprTransParent : MonoBehaviour
    {
        private Sprite _spr;
        private Image _img;

        private void Awake()
        {
            _img = GetComponent<Image>();
            _spr = _img.sprite;
        }

        void Update()
        {
            _spr = _img.sprite;
            _img.color = _spr == null ? Color.clear : Color.white;
        }
    }
}