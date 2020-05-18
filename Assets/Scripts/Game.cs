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

    public string getPin()
    {
        return pin;
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

    public override string ToString()
    {
        return pin;
    }
}
