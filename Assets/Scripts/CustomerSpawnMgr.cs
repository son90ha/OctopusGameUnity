using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct CustomerData
{
    public CustomerData(int par1, float par2, float par3) {
        orderCount = par1;
        timePerOrder = par2;
        customerSpawnTime = par3;
    }
    public int orderCount;
    public float timePerOrder;
    public float customerSpawnTime; 
}
public class CustomerSpawnMgr : MonoBehaviour
{
    public Transform customerLayout;
    private CustomerData m_customerData = new CustomerData(1, 10, 20);
    private float m_spawnTiming = 0;
    // Start is called before the first frame update
    void Start()
    {
        CreateCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnTiming += Time.deltaTime;
        Debug.Log(m_spawnTiming + "|" + m_customerData.customerSpawnTime);
        if (m_spawnTiming >= m_customerData.customerSpawnTime) {
            CreateCustomer();
            m_spawnTiming = 0;
        }
    }

    private void CreateCustomer() {
        var newCustom = Instantiate(GamePrefabMgr.inst.customerPrefab, customerLayout);
    }
}
