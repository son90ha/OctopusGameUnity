using UnityEngine;

public class CharacterController : MonoBehaviour
{   
    public Transform layout;
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
        var newItem = Instantiate(GamePrefabMgr.inst.itemPrefab, layout);
        newItem.AddComponent<UnityEngine.UI.LayoutElement>();
        newItem.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        var color = Game.inst.GetColorByItemType(itemType);
        newItem.GetComponent<SpriteRenderer>().color = color;
    }
}
