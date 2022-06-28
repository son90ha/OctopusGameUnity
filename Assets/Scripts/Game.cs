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
    public TextAsset ingredientDataText;
    private List<EItemType> m_listItem;
    public List<EItemType> ListItem
    {   
        get { return m_listItem; }
    }
    private List<CustomerController> m_listCustomer = new List<CustomerController>();
    private List<OctopusCustomerGenData> m_octopusCustomerGenData;
    private Dictionary<EOctopusType, OctopusData> m_octopusDataDic = new Dictionary<EOctopusType, OctopusData>();
    private EGameState m_gameState;
    public EGameState gameState
    {
        get { return m_gameState; }
    }
    public bool IsPlaying { get { return m_gameState == EGameState.PLAYING; } }
    private List<IngredientData> m_ingredientDataList = new List<IngredientData>();
    private int m_curIngredientAmout = 3;
    private PowerupTimingMgr m_powerupTimingMgr;
    public PowerupTimingMgr PowerupTimingMgr { get { return m_powerupTimingMgr; } }
    public PowerupDataScriptTable powerupAffectData;

    void Awake() {
        m_gameState = EGameState.INITILAIZING;
        Game._inst = this;
        m_powerupTimingMgr = new PowerupTimingMgr();
        LoadDataBase();

        GameEvent.CircleRotate_Pick.AddListener(OnCircleRotatePick);
        GameEvent.Character_GetAnItem.AddListener(OnCharacterGetAnItem);
        GameEvent.Character_LivesChanged.AddListener(OnCharacterLivesChanged);
        GameEvent.Character_ScoreChanged.AddListener(OnCharacterScoreChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_powerupTimingMgr.Update();
    }

    void resetListItem(int count) 
    {
        m_listItem = CONST.ListItemType.GetRange(0, count + 1); // +1 for POWER_UP 
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
            case EItemType.RED: { return Color.red; }
            default: return Color.white;
        }
    }

    private void OnCircleRotatePick(float angle)
    {
        var item = circleItemMgr.getItemFromAngle(angle);
        if (item == EItemType.POWER_UP)
        {
            var powerupInfo = circleItemMgr.PowerupItem.GetPowerupInfo();
            if (powerupInfo != EPowerupType.NONE)
            {
                GameEvent.Game_PickPowerup.Invoke(powerupInfo);
            }
        }
        else
        {
            localCharacter.onGetAnItem(item);
        }
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
                GameEvent.Game_OrderFinish.Invoke(c);

                c.OnOrderFinish();
                RemoveCustomer(c);
                return;
            }
        }
        var isReject = listStatus.TrueForAll((s => s == ECustomerStatus.REJECT));
        if (isReject)
        {
            GameEvent.Game_OrderWrong.Invoke();
        }
        else
        {
            localCharacter.ChangeState(ECharacterState.RIGHT_GRAB);
        }
    }

    public void RemoveCustomer(CustomerController customer)
    {
        var index = m_listCustomer.IndexOf(customer);
        if (index < 0)
        {
            Debug.LogError("[Game] RemoveCustomer - Customer not FOUND");
        }
        m_listCustomer.RemoveAt(index);
        if (m_listCustomer.Count == 0)
        {
            GameEvent.Game_CustomerClear.Invoke();
        }
    }

    private void LoadDataBase()
    {
        LoadOctopusCustomerGenData();
        LoadOctopusDataBase();
        LoadIngredientAmountData();
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

    private void LoadOctopusDataBase()
    {
        var lines = CSVLoader.ParseArray(octoDataBaseText.text);
        var firstRow = lines[0];
        for (int i = 1; i < firstRow.Length; i++)
        {
            var octopusName = firstRow[i];
            EOctopusType octopusNameEnum;
            if (Enum.TryParse<EOctopusType>(octopusName, out octopusNameEnum))
            {
                var newOctpusData = new OctopusData();
                newOctpusData.octopusType = octopusNameEnum;
                for (int j = 1; j < lines.Length; j++)
                {
                    var lineData = lines[j];
                    switch (lineData[0])
                    {
                        case "TentacleSpeed":
                            {
                                newOctpusData.tentacleSpeed = Utils.convertToFloat(lineData[i]);
                                break;
                            }
                        case "OctopusLives":
                            {
                                newOctpusData.octopusLives = Utils.convertToInt(lineData[i]);
                                break;
                            }
                        case "LifeLost%":
                            {
                                newOctpusData.lifeLostPercent = Utils.convertToFloat(lineData[i]);
                                break;
                            }
                        case "EasyOrder%":
                            {
                                newOctpusData.easyOrderPercent = Utils.convertToFloat(lineData[i]);
                                break;
                            }
                        case "OctopusBonusPerCustomer":
                            {
                                newOctpusData.octopusBonusPerCustomer = Utils.convertToFloat(lineData[i]);
                                break;
                            }
                        case "OctopusPatienceBonus":
                            {
                                newOctpusData.octopusPatienceBonus = Utils.convertToFloat(lineData[i]);
                                break;
                            }
                        default: break;
                    }
                }
                m_octopusDataDic.Add(octopusNameEnum, newOctpusData);
            }
        }
    }

    public OctopusCustomerGenData GetCustomerDataByScore(int score)
    {
        return m_octopusCustomerGenData.Find((e) =>
        {
            return e.PlayerScoreMin <= score && score <= e.PlayerScoreMax;
        });
    }

    public OctopusCustomerGenData GetCustomerGenDataByCurScore()
    {
        return GetCustomerDataByScore(localCharacter.Score);
    }

    public List<EItemType> GetListItemWithoutPowerUp()
    {
        var newList = new List<EItemType>(m_listItem);
        newList.RemoveAt(newList.IndexOf(EItemType.POWER_UP));
        return newList;
    }

    public OctopusData GetOctopusDataByType(EOctopusType type)
    {
        OctopusData result;
        if (m_octopusDataDic.TryGetValue(type, out result))
        {
            return result;
        }

        Debug.LogError($"[Game] GetOctopusDataByType - NOT FOUND. type = {type}");
        return new OctopusData();
    }

    private void OnCharacterLivesChanged(int lives)
    {
        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        m_gameState = EGameState.GAMEOVER;
        m_listCustomer.ForEach((c) =>
        {
            GameObject.Destroy(c.gameObject);
        });
        m_listCustomer.Clear();
        GameEvent.Game_GameOver.Invoke();
    }

    private void CreateCharacterData()
    {
        localCharacter.octopusData = GetOctopusDataByType(EOctopusType.BasicOctoChef);
    }

    public void CharacterChangeToNextSkin()
    {
        int curSkinValue = (int)localCharacter.CurOctopusType;
        int nextSkinValue = curSkinValue + 1;
        if (nextSkinValue > (int)EOctopusType.FortunateOcto)
        {
            nextSkinValue = 0;
        }

        localCharacter.octopusData = GetOctopusDataByType((EOctopusType)nextSkinValue);
    }

    public void CreateNewGame()
    {
        resetListItem(this.m_curIngredientAmout);
        circleItemMgr.createItem();
        CreateCharacterData();
        m_gameState = EGameState.PLAYING;
    }

    private void LoadIngredientAmountData()
    {
        var data = CSVLoader.ParseArray(ingredientDataText.text);
        var scoreMinLine = data[0];
        var scoreMaxLine = data[1];
        var amountLine = data[2];

        for (int i = 1; i < scoreMaxLine.Length; i++)
        {
            IngredientData newData = new IngredientData();
            newData.PlayerScoreMin = Utils.convertToInt(scoreMinLine[i]);
            newData.PlayerScoreMax = Utils.convertToInt(scoreMaxLine[i]);
            if (newData.PlayerScoreMax < 0) newData.PlayerScoreMax = int.MaxValue;
            newData.IngredientAmount = Utils.convertToInt(amountLine[i]);
            m_ingredientDataList.Add(newData);

        }
    }

    public int getIngredientAmountByScore(int score)
    {
        int result = 3;
        int i = m_ingredientDataList.FindIndex((item) =>
        {
            return item.PlayerScoreMin <= score && score <= item.PlayerScoreMax;
        });
        if (i >= 0)
        {
            result = m_ingredientDataList[i].IngredientAmount;
        }
        else
        {
            Debug.LogError("[Game] getIngredientAmountByScore - Cannot get amout with Score: " + score);
        }

        return result;
    }

    private void OnCharacterScoreChanged(int score)
    {
        int newIngredientAmout = getIngredientAmountByScore(score);
        if (newIngredientAmout != m_curIngredientAmout)
        {
            m_curIngredientAmout = newIngredientAmout;
            resetListItem(m_curIngredientAmout);
            circleItemMgr.createItem();
        }
    }
}
