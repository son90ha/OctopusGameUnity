using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECustomerStatus
{
    FINISH,
    A_PART,
    REJECT,
}

public class CustomerController : MonoBehaviour
{
    struct OrderItem
    {
        public OrderItem(EItemType t, GameObject g)
        {
            itemType = t;
            gameObj = g;
        }
        public EItemType itemType;
        public GameObject gameObj;
    }
    public Transform orderLayout;
    public CustomerProgressBar progressBar;
    
    private float m_curTime = 1;
    private float m_totalTime = 1;
    private List<OrderItem> m_listOrderItem = new List<OrderItem>();
    private float m_patientPercent;
    private bool m_slowTimeActive = false;
    public int PatientPercent
    {
        get { return Mathf.RoundToInt(m_patientPercent * 100); }
    }

    void Awake()
    {
        GameEvent.Powerup_ActiveChanged.AddListener(OnPowerupActiveChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        Game.inst.AddNewCustomer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_curTime > 0) {
            float subTime = m_slowTimeActive ? Time.deltaTime * Game.inst.powerupAffectData.slowTimeValue : Time.deltaTime;
            m_curTime = Mathf.Max(0, m_curTime - subTime);
            m_patientPercent = m_curTime / m_totalTime;
            progressBar.setProgress(m_patientPercent);
            if (m_curTime == 0)
            {
                TimeOut();
            }
        }
    }

    void TimeOut()
    {
        Game.inst.RemoveCustomer(this);
        Destroy(gameObject);

        GameEvent.Customer_TimeOut.Invoke();
    }

    void CreateOrderItem(int count) 
    {
        var listItemType = Utils.Shuffle<EItemType>(Game.inst.GetListItemWithoutPowerUp()).GetRange(0, count);
        foreach (var item in listItemType)
        {
            var newItem = Instantiate(GamePrefabMgr.inst.itemPrefab, orderLayout);
            newItem.AddComponent<UnityEngine.UI.LayoutElement>();
            newItem.GetComponent<SpriteRenderer>().color = Game.inst.GetColorByItemType(item);
            m_listOrderItem.Add(new OrderItem(item, newItem));
        }
    }

    public void init(int orderCount, float timePerOrder) 
    {
        m_totalTime = m_curTime = timePerOrder;
        CreateOrderItem(orderCount);
    }

    public ECustomerStatus CheckItem(List<EItemType> listItemGot)
    {
        int correctCount = 0;
        foreach (var item in listItemGot)
        {
            int index = m_listOrderItem.FindIndex(e => item == e.itemType);
            if (index < 0)
            {
                unmarkAllItem();
                return ECustomerStatus.REJECT;
            }
            else
            {
                markItem(m_listOrderItem[index]);
                correctCount++;
            }
        }

        if (correctCount == m_listOrderItem.Count)
        {
            return ECustomerStatus.FINISH;
        }

        return ECustomerStatus.A_PART;
    }

    public void OnOrderFinish()
    {
        GameObject.Destroy(gameObject);
    }

    public int IngredientAmount
    {
        get { return m_listOrderItem.Count; }
    }

    private void OnPowerupActiveChanged(EPowerupType powerupType, bool active)
    {
        if (powerupType == EPowerupType.SLOW_TIME)
        {
            m_slowTimeActive = active;
            if (active)
            {
                // Apply slow Powerup
            }
        }
    }

    private void markItem(OrderItem item)
    {
        var sr = item.gameObj.GetComponent<SpriteRenderer>();
        var newColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
        sr.color = newColor;
    }

    private void unmarkItem(OrderItem item)
    {
        var sr = item.gameObj.GetComponent<SpriteRenderer>();
        var newColor = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        sr.color = newColor;
    }

    public void unmarkAllItem()
    {
        m_listOrderItem.ForEach(e =>
        {
            unmarkItem(e);
        });
    }
}
