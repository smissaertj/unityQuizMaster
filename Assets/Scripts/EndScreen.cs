using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winScoreTxt;
    private ScoreKeeper _scoreKeeper;
    void Awake()
    {
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void ShowFinalScore()
    {
        winScoreTxt.text = "Congratulations!\nYou scored " + _scoreKeeper.CalculateScore() + "%";
    }
}
