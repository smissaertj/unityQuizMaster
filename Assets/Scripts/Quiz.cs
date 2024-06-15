using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")] [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions; // = new List<QuestionSO>();
    public QuestionSO currentQuestion;

    [Header("Answers")] [SerializeField] GameObject[] answerButtons;
    private int _correctAnswerIndex;
    private bool _hasAnsweredEarly = true;

    [Header("Button Colors")] [SerializeField]
    Sprite defaultAnswerSprite;

    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")] [SerializeField] private Image timerImage;
    public Timer timer;

    [Header("Scoring")] [SerializeField] private TextMeshProUGUI scoreText;
    public ScoreKeeper scoreKeeper;

    [Header("ProgressBar")] [SerializeField]
    private Slider progressBar;

    public bool isComplete;


    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            _hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!_hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayCorrectAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int buttonIndex)
    {
        _hasAnsweredEarly = true;
        DisplayCorrectAnswer(buttonIndex);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayCorrectAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text =
                "Sorry, the correct answer is:\n" + currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswerIndex());
            buttonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
        }

        // Always highlight the correct answer
        buttonImage.sprite = correctAnswerSprite;
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    private void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}