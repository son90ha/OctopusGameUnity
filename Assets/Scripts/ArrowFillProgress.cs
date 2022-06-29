using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFillProgress : MonoBehaviour
{

    public RectTransform mask;
    private float m_curTime = 0;
    private float m_totalTime = 1;

    public void setTime(float time)
    {
        m_curTime = 0;
        m_totalTime = time;
    }

    public void Update()
    {
        if (m_curTime < m_totalTime)
        {
            m_curTime += Time.deltaTime;
            float ratio = Mathf.Min(1f, m_curTime / m_totalTime);
            float x = Mathf.Lerp(0, 2, ratio);
            mask.localPosition = new Vector3(x, 0, 0);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

}
