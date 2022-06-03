using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{   
    public Transform orderLayout;
    public CustomerProgressBar progressBar;
    
    private float m_curTime = 1;
    private float m_totalTime = 1;
    private List<EItemType> m_listOrderItem = new List<EItemType>();

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
            progressBar.setProgress(m_curTime / m_totalTime);
            if (m_curTime == 0)
            {
                OnTimeOut();
            }
        }
    }

    void OnTimeOut() {
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
}
