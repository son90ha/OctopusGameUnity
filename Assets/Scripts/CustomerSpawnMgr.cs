using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnMgr : MonoBehaviour
{
    public Transform customerLayout;
    private float m_spawnTiming = 0;
    public OctopusCustomerGenData CurCustomerGenData
    {
        get; set;
    }
    private float m_spawnInterval;

    void Awake()
    {
        GameEvent.Character_ScoreChanged.AddListener(OnCharacterScoreChanged);
        GameEvent.Game_CustomerClear.AddListener(OnCustomerClear);
    }

    // Start is called before the first frame update
    void Start()
    {

        // Init current data
        CurCustomerGenData = Game.inst.GetCustomerDataByScore(0);
        m_spawnInterval = 0;
    }

    private void OnCharacterScoreChanged(int score)
    {
        CurCustomerGenData = Game.inst.GetCustomerDataByScore(score);
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.inst.IsPlaying)
        {
            m_spawnTiming += Time.deltaTime;
            if (m_spawnTiming >= m_spawnInterval)
            {
                CreateCustomer();
                m_spawnTiming = 0;
            }
        }
    }

    private void CreateCustomer() {
        var newCustom = Instantiate(GamePrefabMgr.inst.customerPrefab, customerLayout);
        var customerController = newCustom.GetComponent<CustomerController>();
        int easyOrderDecrease = (Random.Range(0, 100) > Game.inst.localCharacter.EasyOrderPercent) ? 0 : 1;
        int simplifyOrderDecrease = Game.inst.PowerupTimingMgr.IsPowerupActive(EPowerupType.SIMPLIFY_ORDER) ? 1 : 0;
        int ingredientAmountMax = Mathf.Min(1, CurCustomerGenData.IngredientAmountMax + 1 - easyOrderDecrease - simplifyOrderDecrease);

        //  Apply easy order ability
        if (easyOrderDecrease > 0)
        {
            Debug.Log("Apply easy order ability");
        }
        //

        if (simplifyOrderDecrease > 1)
        {
            Debug.Log("Apply simplify order powerup");
        }
        int ingradientAmount = Random.Range(CurCustomerGenData.IngredientAmountMin, ingredientAmountMax);
        customerController.init(ingradientAmount, GetCurPatienceTime());
        m_spawnInterval = Random.Range(CurCustomerGenData.SpawnIntervalMin, CurCustomerGenData.SpawnIntervalMax + 1);
    }

    private void OnCustomerClear()
    {
        CreateCustomer();
    }

    /// <summary>
    /// Get Patient time with localCharacter bonus and powerup
    /// <summary>
    private float GetCurPatienceTime()
    {
        float patienceBase = Random.Range(CurCustomerGenData.PatienceMin, CurCustomerGenData.PatienceMax + 1);
        float bonusTime = Game.inst.localCharacter.PatienceBonus;
        if (Game.inst.PowerupTimingMgr.IsPowerupActive(EPowerupType.EXTRA_PATIENCE))
        {
            bonusTime = Game.inst.powerupAffectData.extraPatienceTime;
            Debug.Log("Has Extra patience time");
        }
        return patienceBase + bonusTime;
    }
}
