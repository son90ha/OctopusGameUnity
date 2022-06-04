using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UnityEngine.UI.Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameEvent.Character_ScoreChanged.AddListener(_onCharacterScoreChanged);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void _onCharacterScoreChanged(int score)
    {
        scoreText.text = score.ToString();
    }
}
