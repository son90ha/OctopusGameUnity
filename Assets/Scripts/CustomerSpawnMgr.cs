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
        int ingredientAmountMax = Mathf.Min(1, CurCustomerGenData.IngredientAmountMax + 1 - easyOrderDecrease);

        //  Apply easy order ability
        if (easyOrderDecrease > 0)
        {
            Debug.Log("Apply easy order ability");
        }
        //

        int ingradientAmount = Random.Range(CurCustomerGenData.IngredientAmountMin, ingredientAmountMax);
        float patienceTime = Random.Range(CurCustomerGenData.PatienceMin, CurCustomerGenData.PatienceMax + 1);
        customerController.init(ingradientAmount, patienceTime + Game.inst.localCharacter.PatienceBonus);
        m_spawnInterval = Random.Range(CurCustomerGenData.SpawnIntervalMin, CurCustomerGenData.SpawnIntervalMax + 1);
    }

    private void OnCustomerClear()
    {
        CreateCustomer();
    }
}
