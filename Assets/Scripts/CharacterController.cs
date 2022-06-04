using System.Collections.Generic;
using UnityEngine;

public struct OctopusData
{
    float tentacleSpeed;
    uint octopusLives;
    float lifeLostPercent;
    float easyOrderPercent;
    float octopusBonusPerCustomer;
    float octopusPatienceBonus;
    EOctopusName octopusName;
}

public enum EOctopusName
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
    public int Score
    {
        get { return m_score; }
        set
        {
            m_score = value;
            GameEvent.Character_ScoreChanged.Invoke(m_score);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameEvent.Game_OrderFinish.AddListener(OnOrderFinish);
        GameEvent.Game_OrderWrong.AddListener(OnOrderWrong);
        GameEvent.Customer_TimeOut.AddListener(OnCustomerTimeOut);
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
}
