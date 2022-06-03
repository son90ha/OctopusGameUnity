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
    private List<EItemType> m_listItem;
    public List<EItemType> ListItem
    {   
        get { return m_listItem; }
    }
    private List<CustomerController> m_listCustomer = new List<CustomerController>();

    void Awake() {
        Game._inst = this;
        resetListItem(6);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvent.CircleRotate_Stop.AddListener(OnCircleRotateStop);
        GameEvent.Character_GetAnItem.AddListener(OnCharacterGetAnItem);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void resetListItem(int count) 
    {
        m_listItem = CONST.ListItemType.GetRange(0, count);
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
            default: return Color.white;
        }
    }

    private void OnCircleRotateStop(float angle)
    {
        var item = circleItemMgr.getItemFromAngle(angle);
        localCharacter.onGetAnItem(item);
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
                c.OnOrderFinish();
                RemoveCustomer(c);
                GameEvent.Game_OrderFinish.Invoke();
                return;
            }
        }
        var isReject = listStatus.TrueForAll((s => s == ECustomerStatus.REJECT));
        listStatus.ForEach(i =>
        {
            Debug.Log(i.ToString());
        });
        Debug.Log("isReject " + isReject);
        if (isReject)
        {
            GameEvent.Game_OrderWrong.Invoke();
        }
    }

    private void RemoveCustomer(CustomerController customer)
    {
        var index = m_listCustomer.IndexOf(customer);
        if (index < 0)
        {
            Debug.LogError("[Game] RemoveCustomer - Customer not FOUND");
        }
        m_listCustomer.RemoveAt(index);
        if (m_listCustomer.Count == 0)
        {
            // customer clear event here
        }
    }
}
