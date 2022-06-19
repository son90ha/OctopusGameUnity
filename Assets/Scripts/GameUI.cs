using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text liveText;
    public UnityEngine.UI.Text skinBtnText;
    public GameObject buttonStart;
    public UnityEngine.UI.Text btnStartText;
    void Awake()
    {
        GameEvent.Character_ScoreChanged.AddListener(OnCharacterScoreChanged);
        GameEvent.Character_LivesChanged.AddListener(OnCharacterLivesChanged);
        GameEvent.Character_DataChanged.AddListener(OnCharacterDataChanged);
        GameEvent.Game_GameOver.AddListener(OnGameOver);
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

    private void OnCharacterLivesChanged(int lives)
    {
        setLive(lives);
    }

    private void OnCharacterDataChanged(OctopusData data)
    {
        skinBtnText.text = data.octopusType.ToString();
    }

    private void setScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void setLive(int lives)
    {
        liveText.text = "Live: " + lives.ToString();
    }

    public void OnChangedSkinClick()
    {
        Game.inst.CharacterChangeToNextSkin();
    }

    public void OnStartClick()
    {
        Game.inst.CreateNewGame();
        buttonStart.SetActive(false);
    }

    private void OnGameOver()
    {
        buttonStart.SetActive(true);
        btnStartText.text = "Play Again";
    }
}
