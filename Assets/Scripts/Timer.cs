using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeToCompleteQuestion = 30f;
    [SerializeField] private float timeToShowCorrectAnswer = 10f;
    
    float _timerValue;
    
    public float fillFraction;
    public bool isAnsweringQuestion;
    public bool loadNextQuestion;
    
    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        _timerValue = 0;
    }
    
    void UpdateTimer()
    {
        _timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (_timerValue > 0)
            {
                fillFraction = _timerValue / timeToCompleteQuestion;
            }
            else
            {
                isAnsweringQuestion = false;
                _timerValue = timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (_timerValue > 0)
            {
                fillFraction = _timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                _timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
        // Debug.Log(isAnsweringQuestion + ": " + _timerValue + " = " + fillFraction);
    }
}
