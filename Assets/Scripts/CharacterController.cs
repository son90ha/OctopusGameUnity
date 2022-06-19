using System.Collections.Generic;
using UnityEngine;

public struct OctopusData
{
    public float tentacleSpeed;
    public int octopusLives;
    public float lifeLostPercent;
    public float easyOrderPercent;
    public float octopusBonusPerCustomer;
    public float octopusPatienceBonus;
    public EOctopusType octopusType;
}

public enum EOctopusType
{
    BasicOctoChef = 0,
    QuickTenty,
    InterestingOcto,
    LazyOcto,
    LuckyOcto,
    FortunateOcto,
}

public class CharacterController : MonoBehaviour, IOnPickPowerup
{
    public Transform itemGotLayout;
    public SpriteRenderer octopusSpriteRender;
    private List<EItemType> m_curListItemGot = new List<EItemType>();

    private int m_score = 0;
    private OctopusData m_octopusData;
    public OctopusData octopusData
    {
        get
        {
            return m_octopusData;
        }
        set
        {
            m_octopusData = value;
            Lives = m_octopusData.octopusLives;
            SetSkinColor(m_octopusData.octopusType);
            GameEvent.Character_DataChanged.Invoke(m_octopusData);
        }
    }
    public int Score
    {
        get { return m_score; }
        set
        {
            m_score = value;
            GameEvent.Character_ScoreChanged.Invoke(m_score);
        }
    }

    public float EasyOrderPercent
    {
        get { return m_octopusData.easyOrderPercent; }
    }
    public float PatienceBonus
    {
        get { return m_octopusData.octopusPatienceBonus; }
    }
    public EOctopusType CurOctopusType
    {
        get { return m_octopusData.octopusType; }
    }

    private int m_lives = 0;
    public int Lives
    {
        get { return m_lives; }
        set
        {
            if (m_lives != value)
            {
                m_lives = value;
                GameEvent.Character_LivesChanged.Invoke(value);
            }
        }
    }

    void Awake()
    {
        GameEvent.Game_OrderFinish.AddListener(OnOrderFinish);
        GameEvent.Game_OrderWrong.AddListener(OnOrderWrong);
        GameEvent.Customer_TimeOut.AddListener(OnCustomerTimeOut);
        GameEvent.Game_PickPowerup.AddListener(OnPickPowerUp);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void onGetAnItem(EItemType itemType)
    {
        var newItem = Instantiate(GamePrefabMgr.inst.itemPrefab, itemGotLayout);
        newItem.AddComponent<UnityEngine.UI.LayoutElement>();
        newItem.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        var color = Game.inst.GetColorByItemType(itemType);
        newItem.GetComponent<SpriteRenderer>().color = color;
        m_curListItemGot.Add(itemType);

        GameEvent.Character_GetAnItem.Invoke(m_curListItemGot);
    }

    private void OnCustomerTimeOut()
    {
        LivesLost();
    }
    private void OnOrderWrong()
    {
        ResetItemGot();
    }
    private void OnOrderFinish(CustomerController customer)
    {
        ResetItemGot();
        Score += Mathf.RoundToInt((customer.IngredientAmount + customer.PatientPercent) * m_octopusData.octopusBonusPerCustomer);
    }

    private void ResetItemGot()
    {
        foreach (Transform child in itemGotLayout)
        {
            GameObject.Destroy(child.gameObject);
        }
        m_curListItemGot.Clear();
    }

    private void LivesLost()
    {
        if (Random.Range(0, 100) > m_octopusData.lifeLostPercent)
        {
            Lives -= 1;
        }
        else
        {
            Debug.Log("Apply LifeLost ability");
        }
    }

    private void SetSkinColor(EOctopusType type)
    {
        Color color = Color.white;
        switch (type)
        {
            case EOctopusType.BasicOctoChef: { color = Color.magenta; break; }
            case EOctopusType.QuickTenty: { color = Color.blue; break; }
            case EOctopusType.InterestingOcto: { color = Color.cyan; break; }
            case EOctopusType.LazyOcto: { color = Color.yellow; break; }
            case EOctopusType.LuckyOcto: { color = Color.green; break; }
            case EOctopusType.FortunateOcto: { color = Color.red; break; }
            default: break;
        }

        octopusSpriteRender.color = color;
    }

    public void OnPickPowerUp(EPowerupType powerupType)
    {
        switch (powerupType)
        {
            case EPowerupType.EXTRA_HP:
                {
                    Lives += 1;
                    break;
                }
            default: return;
        }
    }
}
