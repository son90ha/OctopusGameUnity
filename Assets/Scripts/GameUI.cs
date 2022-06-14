using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text liveText;

    void Awake()
    {
        GameEvent.Character_ScoreChanged.AddListener(OnCharacterScoreChanged);
        GameEvent.Character_LivesLost.AddListener(OnCharacterLivesLost);
        GameEvent.Character_DataChanged.AddListener(OnCharacterDataChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCharacterScoreChanged(int score)
    {
        setScore(score);
    }

    private void OnCharacterLivesLost(int lives)
    {
        setLive(lives);
    }

    private void OnCharacterDataChanged(OctopusData data)
    {
        setLive(data.octopusLives);
    }

    private void setScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void setLive(int lives)
    {
        liveText.text = "Live: " + lives.ToString();
    }
}
