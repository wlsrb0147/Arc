using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFolder.Script
{

    [Serializable]
    public class Inventory
    {
        public Transform[] face;
        public GameObject invenPanel;
    
        public Image themePanel; 
        public Sprite[] themePanelImages;
    
        public GameObject[] lTabPanels;
        public GameObject[] tTabPanels;

        public Image[] inventoryContents;
    
        public GameObject[] faceBox;
        public Image[] faceInBox;
        public Image[] namesInBox;

        public Sprite[] faceInBoxFront;
        public Sprite[] faceInBoxSide;
    
        public Sprite[] status;
        public Sprite[] equipmentSpr;

        public Image eqipmentImg;
        public GameObject check;
        public GameObject check2;

        public Text charactersState;
        public Text notUsingName;
    
        public static int leaderNum;
        public static string selectEquipedItem;
        public static string selectFaceName;
        public static string selectInventoryItem;
        public static string tab15SelectName;
        public static bool needSort;
        public static List<Transform> notUsing = new();
    }
    [Serializable]
    public class Battle
    {
        public GameObject battlePanel;
    }
    [Serializable]
    public class Field
    {
        public GameObject fieldPanel;
    }
    public class UI_Manager : MonoBehaviour
    {
        public int selectedTopButton;
        public int tempSelectedTopButton;
        public int tempSelectedLeftButton;
        public int tobButtonMax;
        public int selectedLeftButton;
        public bool istabChanged = false;

        public Inventory inven;
        public Battle battle;
        public Field field;
    
        private bool isFieldActive = true;
        private bool isBattleActive = false;
        private bool isInvenActive = false;

        public static UI_Manager UI_instance = null;
        public float left;
    
        #region  Dictionary
        private Dictionary<string,GameObject> box = new();
        private Dictionary<string,Image> nameInBox = new();
        private Dictionary<string,Image> faceInBox = new();
        private Dictionary<string, Sprite> faceInBoxFront = new();
        private Dictionary<string, Sprite> faceInBoxSide = new();
        private Dictionary<int, GameObject> Contents = new();
        private Dictionary<int, GameObject> TTab = new();
        private Dictionary<int, GameObject> LTap = new();
        private Dictionary<string, Sprite> Status = new();
        private Dictionary<string, Sprite> Equipments = new();
        private Dictionary<int, Image> bckGround = new();
        private Dictionary<string, Color> colorSet = new();
        private Dictionary<string, string> nameChange = new();
        #endregion
    
        private int expireTab;
        private bool isInvenContentsChanged = false;
        private string currentSelect = "";
        private string currentNotUseSelect = "";
        private void Awake()
        {
            // 만약 instance가 비어있지 않고 현재 인스턴스와 다르다면 (이미 다른 인스턴스가 존재한다면)
            if (UI_instance != null && UI_instance != this)
            {
                Destroy(gameObject); // 현재 인스턴스 파괴
                return; // 이후 로직 실행하지 않음
            }

            // instance가 비어있다면 현재 인스턴스로 설정
            UI_instance = this;

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            for (int i = 0; i < Inventory.notUsing.Count; i++)
            {
                Debug.Log(Inventory.notUsing[i]);
            }
        
            field.fieldPanel.SetActive(true);
            inven.invenPanel.SetActive(false);
            battle.battlePanel.SetActive(false);
            selectedTopButton = 1;
            selectedLeftButton = 10;
            tempSelectedTopButton = 1;
            tempSelectedLeftButton = 10;
            tobButtonMax = 5;
        
            #region Dic
            LTap.Add(10, inven.lTabPanels[0]);
            LTap.Add(20, inven.lTabPanels[1]);
            LTap.Add(30, inven.lTabPanels[2]);
        
            TTab.Add(11,inven.tTabPanels[0]);
            TTab.Add(12,inven.tTabPanels[1]);
            TTab.Add(13,inven.tTabPanels[2]);
            TTab.Add(14,inven.tTabPanels[3]);
            TTab.Add(15,inven.tTabPanels[4]);
        
            TTab.Add(21,inven.tTabPanels[5]);
            TTab.Add(22,inven.tTabPanels[6]);
            TTab.Add(23,inven.tTabPanels[7]);
        
            TTab.Add(31,inven.tTabPanels[8]);
            TTab.Add(32,inven.tTabPanels[9]);
            TTab.Add(33,inven.tTabPanels[10]);
        
            box.Add("AiBox",inven.faceBox[0]);
            box.Add("CarrotBox",inven.faceBox[1]);
            box.Add("MariaBox",inven.faceBox[2]);
            box.Add("PeachBox",inven.faceBox[3]);
            box.Add("CellineBox",inven.faceBox[4]);
            box.Add("SizzBox",inven.faceBox[5]);
            box.Add("EluardBox",inven.faceBox[6]);
            box.Add("KreutzerBox",inven.faceBox[7]);
            box.Add("TenziBox",inven.faceBox[8]);

            faceInBox.Add("AiBox", inven.faceInBox[0]);
            faceInBox.Add("CarrotBox", inven.faceInBox[1]);
            faceInBox.Add("MariaBox", inven.faceInBox[2]);
            faceInBox.Add("PeachBox", inven.faceInBox[3]);
            faceInBox.Add("CellineBox",inven.faceInBox[4]);
            faceInBox.Add("SizzBox", inven.faceInBox[5]);
            faceInBox.Add("EluardBox", inven.faceInBox[6]);
            faceInBox.Add("KreutzerBox", inven.faceInBox[7]);
            faceInBox.Add("TenziBox", inven.faceInBox[8]);
        
            nameInBox.Add("AiBox", inven.namesInBox[0]);
            nameInBox.Add("CarrotBox", inven.namesInBox[1]);
            nameInBox.Add("MariaBox", inven.namesInBox[2]);
            nameInBox.Add("PeachBox", inven.namesInBox[3]);
            nameInBox.Add("CellineBox",inven.namesInBox[4]);
            nameInBox.Add("SizzBox", inven.namesInBox[5]);
            nameInBox.Add("EluardBox", inven.namesInBox[6]);
            nameInBox.Add("KreutzerBox", inven.namesInBox[7]);
            nameInBox.Add("TenziBox", inven.namesInBox[8]);
        
            colorSet.Add("AiBox",new Color(188/255f,117/255f,78/255f));
            colorSet.Add("CarrotBox",new Color(169/255f,108/255f,157/255f));
            colorSet.Add("MariaBox",new Color(189/255f,110/255f,113/255f));
            colorSet.Add("PeachBox",new Color(209/255f,152/255f,161/255f));
            colorSet.Add("CellineBox",new Color(139/255f,139/255f,161/255f));
            colorSet.Add("SizzBox",new Color(133/255f,139/255f,156/255f));
            colorSet.Add("EluardBox",new Color(255/255f,222/255f,49/255f));
            colorSet.Add("KreutzerBox",new Color(178/255f,152/255f,152/255f));
            colorSet.Add("TenziBox",new Color(67/255f,17/255f,9/255f));
        
            faceInBoxFront.Add("AiBox", inven.faceInBoxFront[0]);
            faceInBoxFront.Add("CarrotBox", inven.faceInBoxFront[1]);
            faceInBoxFront.Add("MariaBox", inven.faceInBoxFront[2]);
            faceInBoxFront.Add("PeachBox", inven.faceInBoxFront[3]);
            faceInBoxFront.Add("CellineBox",inven.faceInBoxFront[4]);
            faceInBoxFront.Add("SizzBox", inven.faceInBoxFront[5]);
            faceInBoxFront.Add("EluardBox", inven.faceInBoxFront[6]);
            faceInBoxFront.Add("KreutzerBox", inven.faceInBoxFront[7]);
            faceInBoxFront.Add("TenziBox", inven.faceInBoxFront[8]);
        
            faceInBoxSide.Add("AiBox", inven.faceInBoxSide[0]);
            faceInBoxSide.Add("CarrotBox", inven.faceInBoxSide[1]);
            faceInBoxSide.Add("MariaBox", inven.faceInBoxSide[2]);
            faceInBoxSide.Add("PeachBox", inven.faceInBoxSide[3]);
            faceInBoxSide.Add("CellineBox",inven.faceInBoxSide[4]);
            faceInBoxSide.Add("SizzBox", inven.faceInBoxSide[5]);
            faceInBoxSide.Add("EluardBox", inven.faceInBoxSide[6]);
            faceInBoxSide.Add("KreutzerBox", inven.faceInBoxSide[7]);
            faceInBoxSide.Add("TenziBox", inven.faceInBoxSide[8]);
        
            nameChange.Add("AiBox","아이");
            nameChange.Add("CarrotBox","캐럿");
            nameChange.Add("MariaBox","마리아");
            nameChange.Add("PeachBox","피치");
            nameChange.Add("CellineBox","셀린");
            nameChange.Add("SizzBox","시즈");
            nameChange.Add("EluardBox","엘류어드");
            nameChange.Add("KreutzerBox","크로이체르");
            nameChange.Add("TenziBox","텐지");
        
            Status.Add("AiBox",inven.status[0]);
            Status.Add("CarrotBox",inven.status[1]);
            Status.Add("CellineBox",inven.status[2]);
            Status.Add("EluardBox",inven.status[3]);
            Status.Add("KreutzerBox",inven.status[4]);
            Status.Add("MariaBox",inven.status[5]);
            Status.Add("PeachBox",inven.status[6]);
            Status.Add("SizzBox",inven.status[7]);
            Status.Add("TenziBox",inven.status[8]);
        
        
            Equipments.Add("AiBox",inven.equipmentSpr[0]);
            Equipments.Add("CarrotBox",inven.equipmentSpr[1]);
            Equipments.Add("CellineBox",inven.equipmentSpr[2]);
            Equipments.Add("EludardBox",inven.equipmentSpr[3]);
            Equipments.Add("KreutzerBox",inven.equipmentSpr[4]);
            Equipments.Add("MariaBox",inven.equipmentSpr[5]);
            Equipments.Add("PeachBox",inven.equipmentSpr[6]);
            Equipments.Add("SizzBox",inven.equipmentSpr[7]);
            Equipments.Add("TenziBox",inven.equipmentSpr[8]);
        
        
            #endregion
        
        }

        // LTap , TTap
    
        void Update()
        {
            Debug.Log(Inventory.selectEquipedItem);
            Debug.Log(Inventory.selectInventoryItem);
            // 배틀중에는 인벤 사용불가
            if (Input.GetKeyDown(KeyCode.I) && isBattleActive == false) 
            {
                isInvenActive = !isInvenActive;
                isFieldActive = !isFieldActive;
            }

            if (Input.GetKeyDown(KeyCode.Escape)&& isInvenActive)
            {
                isInvenActive = false;
                isFieldActive = true;
            }

            if (isInvenActive)
            {            
                Time.timeScale = 0;
                inven.invenPanel.SetActive(true);
                Inven();
            }
            else
            {
                Time.timeScale = 1;
                inven.invenPanel.SetActive(false);
            }

            if (isFieldActive)
            {
                field.fieldPanel.SetActive(true);
                Field();
            }
            else
            {
                field.fieldPanel.SetActive(false);
            }
        
        
        }

        void Field()
        {
 
        
        }

        void Inven()
        {
            if (istabChanged) // 탭 변경
            {
                if (tempSelectedTopButton > tobButtonMax)
                {
                    tempSelectedTopButton = selectedTopButton;
                    istabChanged = false;
                    return;
                }
                isInvenContentsChanged = true;
                istabChanged = false;
            }
        
            if (isInvenContentsChanged) // 내용물 변경
            {
                ChangeInven(selectedLeftButton,selectedTopButton,
                    tempSelectedLeftButton,tempSelectedTopButton);
                selectedLeftButton = tempSelectedLeftButton;
                selectedTopButton = tempSelectedTopButton;
                isInvenContentsChanged = false;
            }

            if (selectedLeftButton == 10)
            {
                ChangeFace(ref currentSelect, Inventory.selectFaceName, inven.check.transform);
            }
        
            if (selectedLeftButton + selectedTopButton == 11)
            {
                inven.inventoryContents[0].sprite = Status[Inventory.selectFaceName];
            }

            if (selectedLeftButton + selectedTopButton == 12)
            {
                inven.charactersState.text = nameChange[Inventory.selectFaceName]+"의 상태";
                inven.eqipmentImg.sprite = Equipments[Inventory.selectFaceName];
            }

            if (selectedLeftButton + selectedTopButton == 15)
            {
                ChangeFace(ref currentNotUseSelect, Inventory.tab15SelectName, inven.check2.transform);
                inven.notUsingName.text = nameChange[Inventory.tab15SelectName];
            }
        }

        void ChangeFace(ref string current,string change,Transform check)
        {
            if (current== "") // 초기값 설정
            {
                current = change;
            }
        
            if (current != change)
            {
                faceInBox[current].sprite = faceInBoxSide[current];
                nameInBox[current].color = new Color(255, 255, 255);
                current = change;
            }
            else
            {
                faceInBox[current].sprite = faceInBoxFront[current];
                check.position = box[current].transform.position - new Vector3(90, 10, 0);
                nameInBox[change].color = colorSet[change];
            }
        }
    
        void ChangeInven(int disableLeft,int disableTop,int enableLeft,int enableTop)
        {
            if (disableLeft != enableLeft)
            {
                LTap[disableLeft].SetActive(false);
                LTap[enableLeft].SetActive(true);
            }

            TTab[disableLeft+disableTop].SetActive(false);
            TTab[enableLeft+enableTop].SetActive(true);
        
        }

    }
}