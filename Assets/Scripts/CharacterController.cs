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
    public EOctopusType octopusName;
}

public enum EOctopusType
{
    BasicOctoChef = 1,
    QuickTenty,
    InterestingOcto,
    LazyOcto,
    LuckyOcto,
    FortunateOcto,
}

public class CharacterController : MonoBehaviour
{
    public Transform itemGotLayout;
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

    void Awake()
    {
        GameEvent.Game_OrderFinish.AddListener(OnOrderFinish);
        GameEvent.Game_OrderWrong.AddListener(OnOrderWrong);
        GameEvent.Customer_TimeOut.AddListener(OnCustomerTimeOut);
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
        Score += (customer.IngredientAmount + customer.PatientPercent);
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
            m_octopusData.octopusLives -= 1;
            GameEvent.Character_LivesLost.Invoke(m_octopusData.octopusLives);
        }
        else
        {
            Debug.Log("Apply LifeLost ability");
        }
    }
}
