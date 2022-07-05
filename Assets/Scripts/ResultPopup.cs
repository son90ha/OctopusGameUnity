using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPopup : MonoBehaviour
{
    public static string PATH = "ResultPopup";

    [SerializeField]
    private UnityEngine.UI.Text scoreText;

    public void Init(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void Close()
    {
        GameObject.Destroy(gameObject);
    }
}
