using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform itemGotLayout;
    private List<EItemType> m_curListItemGot = new List<EItemType>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameEvent.Game_OrderFinish.AddListener(OnOrderFinish);
        GameEvent.Game_OrderWrong.AddListener(OnOrderWrong);
        GameEvent.Customer_TimeOut.AddListener(OnCustomerTimeOut);
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
    private void OnOrderFinish()
    {
        ResetItemGot();
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
