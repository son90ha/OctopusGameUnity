using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour
{   

    private static Game _inst;
    public static Game inst
    { 
        get { return Game._inst; }
    }

    public CharacterController localCharacter;
    public CircleItemMgr circleItemMgr;
    public TextAsset octoDataBaseText;
    public TextAsset octoCustomerGenText;
    private List<EItemType> m_listItem;
    public List<EItemType> ListItem
    {   
        get { return m_listItem; }
    }
    private List<CustomerController> m_listCustomer = new List<CustomerController>();
    private List<OctopusCustomerGenData> m_octopusCustomerGenData;

    void Awake() {
        Game._inst = this;
        resetListItem(6);
        LoadDataBase();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvent.CircleRotate_Stop.AddListener(OnCircleRotateStop);
        GameEvent.Character_GetAnItem.AddListener(OnCharacterGetAnItem);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void resetListItem(int count) 
    {
        m_listItem = CONST.ListItemType.GetRange(0, count);
    }

    public Color GetColorByItemType(EItemType itemType)
    {
        switch (itemType)
        {
            case EItemType.BLACK: { return Color.black; }
            case EItemType.BLUE: { return Color.blue; }
            case EItemType.CYAN: { return Color.cyan; }
            case EItemType.GRAY: { return Color.gray; }
            case EItemType.GREEN: { return Color.green; }
            case EItemType.MAGENTA: { return Color.magenta; }
            case EItemType.YELLOW: { return Color.yellow; }
            case EItemType.ORANGE: { return new Color(1f, 0.65f, 0f); }
            default: return Color.white;
        }
    }

    private void OnCircleRotateStop(float angle)
    {
        var item = circleItemMgr.getItemFromAngle(angle);
        localCharacter.onGetAnItem(item);
    }

    public void AddNewCustomer(CustomerController customer)
    {
        m_listCustomer.Add(customer);
    }

    private void OnCharacterGetAnItem(List<EItemType> listItem)
    {
        List<ECustomerStatus> listStatus = new List<ECustomerStatus>();
        foreach (var c in m_listCustomer)
        {
            var status = c.CheckItem(listItem);
            listStatus.Add(status);
            if (status == ECustomerStatus.FINISH)
            {
                c.OnOrderFinish();
                RemoveCustomer(c);
                GameEvent.Game_OrderFinish.Invoke();
                return;
            }
        }
        var isReject = listStatus.TrueForAll((s => s == ECustomerStatus.REJECT));
        if (isReject)
        {
            GameEvent.Game_OrderWrong.Invoke();
        }
    }

    private void RemoveCustomer(CustomerController customer)
    {
        var index = m_listCustomer.IndexOf(customer);
        if (index < 0)
        {
            Debug.LogError("[Game] RemoveCustomer - Customer not FOUND");
        }
        m_listCustomer.RemoveAt(index);
        if (m_listCustomer.Count == 0)
        {
            // customer clear event here
        }
    }

    private void LoadDataBase()
    {
        LoadOctopusCustomerGenData();
    }

    private void LoadOctopusCustomerGenData()
    {
        var octCusLines = CSVLoader.ParseArray(octoCustomerGenText.text);
        int dataCount = octCusLines[0].Length - 1;
        m_octopusCustomerGenData = new List<OctopusCustomerGenData>(new OctopusCustomerGenData[dataCount]);

        for (int i = 0; i < octCusLines.Length; i++)
        {
            var lineData = octCusLines[i];
            for (int j = 1; j < lineData.Length; j++)
            {
                if (m_octopusCustomerGenData[j - 1] == null)
                {
                    m_octopusCustomerGenData[j - 1] = new OctopusCustomerGenData();
                }
                var data = m_octopusCustomerGenData[j - 1];
                switch (lineData[0])
                {
                    case "PlayerScoreMin":
                        {
                            data.PlayerScoreMin = Utils.convertToInt(lineData[j]);
                            break;
                        }
                    case "PlayerScoreMax":
                        {
                            data.PlayerScoreMax = Utils.convertToInt(lineData[j]);
                            if (data.PlayerScoreMax < 0)
                            {
                                data.PlayerScoreMax = int.MaxValue;
                            }
                            break;
                        }
                    case "SpawnIntervalMin":
                        {
                            data.SpawnIntervalMin = Utils.convertToFloat(lineData[j]);
                            break;
                        }
                    case "SpawnIntervalMax":
                        {
                            data.SpawnIntervalMax = Utils.convertToFloat(lineData[j]);
                            break;
                        }
                    case "IngredientAmountMin":
                        {
                            data.IngredientAmountMin = Utils.convertToInt(lineData[j]);
                            break;
                        }
                    case "IngredientAmountMax":
                        {
                            data.IngredientAmountMax = Utils.convertToInt(lineData[j]);
                            break;
                        }
                    case "PatienceMin":
                        {
                            data.PatienceMin = Utils.convertToFloat(lineData[j]);
                            break;
                        }
                    case "PatienceMax":
                        {
                            data.PatienceMax = Utils.convertToFloat(lineData[j]);
                            break;
                        }
                    default: break;
                }
            }
        }
        // m_octopusCustomerGenData.ForEach((e) =>
        // {
        //     Utils.CheckAllValueOfClass(e);
        // });
    }

    public OctopusCustomerGenData GetCustomerDataByScore(int score)
    {
        return m_octopusCustomerGenData.Find((e) =>
        {
            return e.PlayerScoreMin <= score && score <= e.PlayerScoreMax;
        });
    }
}
