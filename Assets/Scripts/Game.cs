using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private string pin;
    private int rounds;
    private int time;
    private List<string> questions = new List<string>();
    private int players;

    private int questionCounter = 0;
    private string currentQuestion;

    public string getPin()
    {
        return pin;
    }
    public int getTime()
    {
        return time;
    }
    public int getRounds()
    {
        return rounds;
    }
    public void setPin(string pinRoom)
    {
        if(pin == null)
        {
            pin = pinRoom;
        }
    }
    public void setPlayers()
    {
        players++; 
    }
    public int getPlayers()
    {
        return players;
    }

    public string getCurrentQuestion()
    {
        return currentQuestion;
    }

    public void setMakeRoom(int roundsRoom, int timeRoom)
    {
        rounds = roundsRoom;
        time = timeRoom;
        makeRoom();
    }

    public void addQuestion(string name)
    {
        questions.Add(name);
    }

    public void removeQuestion(string name)
    {
        questions.Remove(name);
    }

    private void makeRoom()
    {
        StartCoroutine(API.Instance.MakeGame(rounds, time, questions));
    }

    public void startGame()
    {
        StartCoroutine(API.Instance.StartGame(pin));
    }

    public void nextQuestion()
    {
        if(questionCounter <= rounds)
        {
            StartCoroutine(API.Instance.NextQuestion(pin, questions[questionCounter]));
            currentQuestion = questions[questionCounter];
            questionCounter++;
        }
    }

    public void checkAnswer(bool last)
    {
        StartCoroutine(API.Instance.correctAnswer(pin, questions[questionCounter-1], last));
    }

    public override string ToString()
    {
        return pin;
    }
}
