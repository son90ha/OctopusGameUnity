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
    public Transform orderLayout;
    public CustomerProgressBar progressBar;
    
    private float m_curTime = 1;
    private float m_totalTime = 1;
    private List<EItemType> m_listOrderItem = new List<EItemType>();
    private float m_patientPercent;
    public int PatientPercent
    {
        get { return Mathf.RoundToInt(m_patientPercent * 100); }
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
            m_curTime = Mathf.Max(0, m_curTime - Time.deltaTime);
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
    }

    void CreateOrderItem(int count) 
    {
        m_listOrderItem =  Utils.Shuffle<EItemType>(new List<EItemType>(Game.inst.ListItem)).GetRange(0, count);
        foreach (var item in m_listOrderItem)
        {
            var newItem = Instantiate(GamePrefabMgr.inst.itemPrefab, orderLayout);
            newItem.gameObject.AddComponent<UnityEngine.UI.LayoutElement>();
            newItem.GetComponent<SpriteRenderer>().color = Game.inst.GetColorByItemType(item);
        }
    }

    public void init(int orderCount, float timePerOrder) 
    {
        m_totalTime = m_curTime = timePerOrder;
        CreateOrderItem(orderCount);
    }

    public ECustomerStatus CheckItem(List<EItemType> listItemGot)
    {
        var tempListOrderItem = new List<EItemType>(m_listOrderItem);
        foreach (var item in listItemGot)
        {
            var index = tempListOrderItem.IndexOf(item);
            if (index < 0)
            {
                return ECustomerStatus.REJECT;
            }
            else
            {
                tempListOrderItem.RemoveAt(index);
            }
        }

        if (tempListOrderItem.Count == 0)
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
}
