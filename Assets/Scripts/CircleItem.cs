using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItem : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    private float angleFrom { get; set;}
    private float angleTo { get; set;}
    EItemType itemType { get; set;}

    public void Init(Vector3 position, float from, float to, EItemType type)
    {
        angleFrom = from;
        angleTo = to;
        itemType = type;
        spriteRenderer.color = Game.inst.GetColorByItemType(itemType);
        transform.position = position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
