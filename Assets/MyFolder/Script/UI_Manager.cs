using System;
using System.Collections.Generic;
using MyFolder.Script.InventoryScript;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MyFolder.Script
{
    [Serializable]
    public class Inventory
    {
        public static string selectFaceName;
        public static string tab15SelectName;
        public static bool needSort;
        public static List<Transform> notUsing = new();
        public Transform[] face;
        public GameObject invenPanel;

        public Image themePanel;
        public Sprite[] themePanelImages;

        public GameObject[] lTabPanels;
        public GameObject[] tTabPanels;

        public Image[] inventoryContents;

        public Text[] tab11Text;

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
        public static UI_Manager instance;
        public int selectedTopButton;
        public int tempSelectedTopButton;
        public int tempSelectedLeftButton;
        public int tobButtonMax;
        public int selectedLeftButton;
        [FormerlySerializedAs("istabChanged")] public bool isSomethingChanged;

        public Inventory inven;
        public Battle battle;
        public Field field;
        public float left;
        private readonly bool isBattleActive = false;
        private string currentNotUseSelect = "";
        private string currentSelect = "";

        private int expireTab;

        private bool isFieldActive = true;
        private bool isInvenActive;
        private bool isInvenContentsChanged;

        private void Awake()
        {
            // 만약 instance가 비어있지 않고 현재 인스턴스와 다르다면 (이미 다른 인스턴스가 존재한다면)
            if (instance != null && instance != this)
            {
                Destroy(gameObject); // 현재 인스턴스 파괴
                return; // 이후 로직 실행하지 않음
            }

            // instance가 비어있다면 현재 인스턴스로 설정
            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
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

            TTab.Add(11, inven.tTabPanels[0]);
            TTab.Add(12, inven.tTabPanels[1]);
            TTab.Add(13, inven.tTabPanels[2]);
            TTab.Add(14, inven.tTabPanels[3]);
            TTab.Add(15, inven.tTabPanels[4]);

            TTab.Add(21, inven.tTabPanels[5]);
            TTab.Add(22, inven.tTabPanels[6]);
            TTab.Add(23, inven.tTabPanels[7]);

            TTab.Add(31, inven.tTabPanels[8]);
            TTab.Add(32, inven.tTabPanels[9]);
            TTab.Add(33, inven.tTabPanels[10]);

            box.Add("AiBox", inven.faceBox[0]);
            box.Add("CarrotBox", inven.faceBox[1]);
            box.Add("MariaBox", inven.faceBox[2]);
            box.Add("PeachBox", inven.faceBox[3]);
            box.Add("CellineBox", inven.faceBox[4]);
            box.Add("SizzBox", inven.faceBox[5]);
            box.Add("EluardBox", inven.faceBox[6]);
            box.Add("KreutzerBox", inven.faceBox[7]);
            box.Add("TenziBox", inven.faceBox[8]);

            faceInBox.Add("AiBox", inven.faceInBox[0]);
            faceInBox.Add("CarrotBox", inven.faceInBox[1]);
            faceInBox.Add("MariaBox", inven.faceInBox[2]);
            faceInBox.Add("PeachBox", inven.faceInBox[3]);
            faceInBox.Add("CellineBox", inven.faceInBox[4]);
            faceInBox.Add("SizzBox", inven.faceInBox[5]);
            faceInBox.Add("EluardBox", inven.faceInBox[6]);
            faceInBox.Add("KreutzerBox", inven.faceInBox[7]);
            faceInBox.Add("TenziBox", inven.faceInBox[8]);

            nameInBox.Add("AiBox", inven.namesInBox[0]);
            nameInBox.Add("CarrotBox", inven.namesInBox[1]);
            nameInBox.Add("MariaBox", inven.namesInBox[2]);
            nameInBox.Add("PeachBox", inven.namesInBox[3]);
            nameInBox.Add("CellineBox", inven.namesInBox[4]);
            nameInBox.Add("SizzBox", inven.namesInBox[5]);
            nameInBox.Add("EluardBox", inven.namesInBox[6]);
            nameInBox.Add("KreutzerBox", inven.namesInBox[7]);
            nameInBox.Add("TenziBox", inven.namesInBox[8]);

            colorSet.Add("AiBox", new Color(188 / 255f, 117 / 255f, 78 / 255f));
            colorSet.Add("CarrotBox", new Color(169 / 255f, 108 / 255f, 157 / 255f));
            colorSet.Add("MariaBox", new Color(189 / 255f, 110 / 255f, 113 / 255f));
            colorSet.Add("PeachBox", new Color(209 / 255f, 152 / 255f, 161 / 255f));
            colorSet.Add("CellineBox", new Color(139 / 255f, 139 / 255f, 161 / 255f));
            colorSet.Add("SizzBox", new Color(133 / 255f, 139 / 255f, 156 / 255f));
            colorSet.Add("EluardBox", new Color(255 / 255f, 222 / 255f, 49 / 255f));
            colorSet.Add("KreutzerBox", new Color(178 / 255f, 152 / 255f, 152 / 255f));
            colorSet.Add("TenziBox", new Color(67 / 255f, 17 / 255f, 9 / 255f));

            faceInBoxFront.Add("AiBox", inven.faceInBoxFront[0]);
            faceInBoxFront.Add("CarrotBox", inven.faceInBoxFront[1]);
            faceInBoxFront.Add("MariaBox", inven.faceInBoxFront[2]);
            faceInBoxFront.Add("PeachBox", inven.faceInBoxFront[3]);
            faceInBoxFront.Add("CellineBox", inven.faceInBoxFront[4]);
            faceInBoxFront.Add("SizzBox", inven.faceInBoxFront[5]);
            faceInBoxFront.Add("EluardBox", inven.faceInBoxFront[6]);
            faceInBoxFront.Add("KreutzerBox", inven.faceInBoxFront[7]);
            faceInBoxFront.Add("TenziBox", inven.faceInBoxFront[8]);

            faceInBoxSide.Add("AiBox", inven.faceInBoxSide[0]);
            faceInBoxSide.Add("CarrotBox", inven.faceInBoxSide[1]);
            faceInBoxSide.Add("MariaBox", inven.faceInBoxSide[2]);
            faceInBoxSide.Add("PeachBox", inven.faceInBoxSide[3]);
            faceInBoxSide.Add("CellineBox", inven.faceInBoxSide[4]);
            faceInBoxSide.Add("SizzBox", inven.faceInBoxSide[5]);
            faceInBoxSide.Add("EluardBox", inven.faceInBoxSide[6]);
            faceInBoxSide.Add("KreutzerBox", inven.faceInBoxSide[7]);
            faceInBoxSide.Add("TenziBox", inven.faceInBoxSide[8]);

            nameChange.Add("AiBox", "아이");
            nameChange.Add("CarrotBox", "캐럿");
            nameChange.Add("MariaBox", "마리아");
            nameChange.Add("PeachBox", "피치");
            nameChange.Add("CellineBox", "셀린");
            nameChange.Add("SizzBox", "시즈");
            nameChange.Add("EluardBox", "엘류어드");
            nameChange.Add("KreutzerBox", "크로이체르");
            nameChange.Add("TenziBox", "텐지");

            Status.Add("AiBox", inven.status[0]);
            Status.Add("CarrotBox", inven.status[1]);
            Status.Add("CellineBox", inven.status[2]);
            Status.Add("EluardBox", inven.status[3]);
            Status.Add("KreutzerBox", inven.status[4]);
            Status.Add("MariaBox", inven.status[5]);
            Status.Add("PeachBox", inven.status[6]);
            Status.Add("SizzBox", inven.status[7]);
            Status.Add("TenziBox", inven.status[8]);


            Equipments.Add("AiBox", inven.equipmentSpr[0]);
            Equipments.Add("CarrotBox", inven.equipmentSpr[1]);
            Equipments.Add("CellineBox", inven.equipmentSpr[2]);
            Equipments.Add("EluardBox", inven.equipmentSpr[3]);
            Equipments.Add("KreutzerBox", inven.equipmentSpr[4]);
            Equipments.Add("MariaBox", inven.equipmentSpr[5]);
            Equipments.Add("PeachBox", inven.equipmentSpr[6]);
            Equipments.Add("SizzBox", inven.equipmentSpr[7]);
            Equipments.Add("TenziBox", inven.equipmentSpr[8]);

            stats.Add("AiBox", CharacterManager.instance.info[0]);
            stats.Add("CarrotBox", CharacterManager.instance.info[1]);
            stats.Add("CellineBox", CharacterManager.instance.info[2]);
            stats.Add("EluardBox", CharacterManager.instance.info[3]);
            stats.Add("KreutzerBox", CharacterManager.instance.info[4]);
            stats.Add("MariaBox", CharacterManager.instance.info[5]);
            stats.Add("PeachBox", CharacterManager.instance.info[6]);
            stats.Add("SizzBox", CharacterManager.instance.info[7]);
            stats.Add("TenziBox", CharacterManager.instance.info[8]);

            name2type.Add("AiBox", CharacterType.Ai);
            name2type.Add("CarrotBox", CharacterType.Carrot);
            name2type.Add("CellineBox", CharacterType.Celline);
            name2type.Add("EluardBox", CharacterType.Eluard);
            name2type.Add("KreutzerBox", CharacterType.Kreutzer);
            name2type.Add("MariaBox", CharacterType.Maria);
            name2type.Add("PeachBox", CharacterType.Peach);
            name2type.Add("SizzBox", CharacterType.Sizz);
            name2type.Add("TenziBox", CharacterType.Tenzi);

            #endregion
        }

        // LTap , TTap

        private void Update()
        {
            // 배틀중에는 인벤 사용불가
            if (Input.GetKeyDown(KeyCode.I) && isBattleActive == false)
            {
                isInvenActive = !isInvenActive;
                isFieldActive = !isFieldActive;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && isInvenActive)
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

        private void Field()
        {
        }

        private void Inven()
        {
            if (isSomethingChanged) // 탭 변경
            {
                if (tempSelectedTopButton > tobButtonMax)
                {
                    tempSelectedTopButton = selectedTopButton;
                    isSomethingChanged = false;
                    return;
                }

                isInvenContentsChanged = true;
                isSomethingChanged = false;
            }

            // 
            if (isInvenContentsChanged) // 탭이 변경됐을 때 실행
            {
                ChangeInven(selectedLeftButton, selectedTopButton,
                    tempSelectedLeftButton, tempSelectedTopButton);
                selectedLeftButton = tempSelectedLeftButton;
                selectedTopButton = tempSelectedTopButton;
                isInvenContentsChanged = false;
// 여기까지였던 대괄호를 맨아래까지 묶음
                if (selectedLeftButton == 10)
                    ChangeFace(ref currentSelect, Inventory.selectFaceName, inven.check.transform);

                if (selectedLeftButton + selectedTopButton == 11)
                {
                    inven.inventoryContents[0].sprite = Status[Inventory.selectFaceName];
                    ChangeTab11Stat();
                }

                if (selectedLeftButton + selectedTopButton == 12)
                    inven.eqipmentImg.sprite = Equipments[Inventory.selectFaceName];

                if (selectedLeftButton + selectedTopButton == 13)
                    inven.charactersState.text = nameChange[Inventory.selectFaceName] + "의 상태";

                if (selectedLeftButton + selectedTopButton == 15)
                {
                    ChangeFace(ref currentNotUseSelect, Inventory.tab15SelectName, inven.check2.transform);
                    inven.notUsingName.text = nameChange[Inventory.tab15SelectName];
                }
            }
        }

        private void ChangeFace(ref string current, string change, Transform check)
        {
            if (current == "") // 초기값 설정
                current = change;

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

        private void ChangeInven(int disableLeft, int disableTop, int enableLeft, int enableTop)
        {
            if (disableLeft != enableLeft)
            {
                LTap[disableLeft].SetActive(false);
                LTap[enableLeft].SetActive(true);
            }

            TTab[disableLeft + disableTop].SetActive(false);
            TTab[enableLeft + enableTop].SetActive(true);
        }

        // 레벨, 경험치/맥스경험치, 아머/맥스아머, hp/maxhp, 힘민능스피드럭 , 공방마법스피드크리
        private void ChangeTab11Stat()
        {
            inven.tab11Text[0].text = stats[Inventory.selectFaceName].Level.ToString();
            inven.tab11Text[1].text = stats[Inventory.selectFaceName].exp.Value + "/" +
                                      stats[Inventory.selectFaceName].exp.MaxValue;
            inven.tab11Text[2].text = stats[Inventory.selectFaceName].Bar.ToString();
            inven.tab11Text[3].text = stats[Inventory.selectFaceName].hp.Value + "/" +
                                      stats[Inventory.selectFaceName].hp.MaxValue;
            inven.tab11Text[4].text = stats[Inventory.selectFaceName].charBase.Str.ToString();
            inven.tab11Text[5].text = stats[Inventory.selectFaceName].charBase.Vit.ToString();
            inven.tab11Text[6].text = stats[Inventory.selectFaceName].charBase.Int.ToString();
            inven.tab11Text[7].text = stats[Inventory.selectFaceName].charBase.Agi.ToString();
            inven.tab11Text[8].text = stats[Inventory.selectFaceName].charBase.Luk.ToString();
            inven.tab11Text[9].text = stats[Inventory.selectFaceName].Atk.ToString();
            inven.tab11Text[10].text = stats[Inventory.selectFaceName].Def.ToString();
            inven.tab11Text[11].text = stats[Inventory.selectFaceName].Mag.ToString();
            inven.tab11Text[12].text = stats[Inventory.selectFaceName].Spd.ToString();
            inven.tab11Text[13].text = stats[Inventory.selectFaceName].Cri.ToString();
        }

        public CharacterType GetType(string characterName)
        {
            return name2type[characterName];
        }

        #region Dictionary

        private readonly Dictionary<string, GameObject> box = new();
        private readonly Dictionary<string, Image> nameInBox = new();
        private readonly Dictionary<string, Image> faceInBox = new();
        private readonly Dictionary<string, Sprite> faceInBoxFront = new();
        private readonly Dictionary<string, Sprite> faceInBoxSide = new();
        private Dictionary<int, GameObject> Contents = new();
        private readonly Dictionary<int, GameObject> TTab = new();
        private readonly Dictionary<int, GameObject> LTap = new();
        private readonly Dictionary<string, Sprite> Status = new();
        private readonly Dictionary<string, Sprite> Equipments = new();
        private Dictionary<int, Image> bckGround = new();
        private readonly Dictionary<string, Color> colorSet = new();
        private readonly Dictionary<string, string> nameChange = new();
        private readonly Dictionary<string, CharacterStat> stats = new();
        private readonly Dictionary<string, CharacterType> name2type = new();

        #endregion
    }
}