using UnityEngine;
using UnityEngine.UI;

namespace MyFolder.Script
{
    public class SetChangedStat0 : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = gameObject.GetComponent<Text>();
        }

        private void OnEnable()
        {
            _text.text = "";
        }
    }
}
