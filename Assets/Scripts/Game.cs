using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private string pin;
    private int rounds;
    private int time;
    private string[] questions;
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

    public void setMakeRoom(int roundsRoom, int timeRoom, string[] questionsRoom)
    {
        rounds = roundsRoom;
        time = timeRoom;
        questions = questionsRoom;
        makeRoom();
    }

    private void makeRoom()
    {
        StartCoroutine(API.Instance.MakeGame(rounds, time, questions));
    }

    public override string ToString()
    {
        return pin;
    }
}
