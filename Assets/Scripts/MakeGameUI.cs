using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeGameUI : MonoBehaviour
{
    public InputField rounds;
    public InputField time;
    public Dropdown question;

    public Text roomCode;
    public Text people;

    public GameObject makePanel;
    public GameObject startPanel;
    public GameObject questionPanel;
    public GameObject highscorePanel;

    private List<string> dublicateNames = new List<string>();
    private List<string> dublicateNamesAnswered = new List<string>();
    private List<string> dublicateHighscores = new List<string>();


    private bool joining = true;
    private bool playing = false;
    private bool highscoreFilling = false;

    public GameObject scrollViewContent;
    public GameObject myToggle;
    public Text currentQuestion;
    public Text peopleAnswered;
    public Text peopleCount;

    public Text timerText;
    public Text highScoreText;

    //game running vars
    private int roundCounter = 1;
    private bool nextQuestionBool = false;
    private float timer;
    private bool startTimer = false;

    void Start() 
    {
        //als api request er is de data daarmee aanvullen
        StartCoroutine(fillQuestions());
    }

    public void MakeGame()
    {
        makePanel.SetActive(false);
        startPanel.SetActive(true);

        GameMaster.Instance.game.setMakeRoom(int.Parse(rounds.text), int.Parse(time.text));
    }

    public void NextQuestion(bool last)
    {
        GameMaster.Instance.game.nextQuestion(last);
        currentQuestion.text = GameMaster.Instance.game.getCurrentQuestion();

        dublicateNamesAnswered.Clear();
        PusherManager.instance.ResetPlayerAnswered();
        peopleAnswered.text = "";
        peopleCount.text =  "0" + "/" + dublicateNames.Count.ToString();
    }

    public void CheckAnswer()
    {
        GameMaster.Instance.game.checkAnswer();
    }

    public void addPlayerName(string name)
    {
        people.text = name + "\n";
    }

    public void setPinCode()
    {
        roomCode.text = GameMaster.Instance.game.getPin();
    }

    public void StartGame()
    {
        GameMaster.Instance.game.startGame();
        joining = false;
        makePanel.SetActive(false);
        startPanel.SetActive(false);

        questionPanel.SetActive(true);
        //NextQuestion();
        StartCoroutine(waitTillStart());

        //InCoroutine straks:
        playing = true;
    }

    void Update()
    {
        if(joining)
        {
            fillPlayers();
        }
        
        if(playing)
        {
            joining = false;

            if((roundCounter <= GameMaster.Instance.game.getRounds()) && nextQuestionBool)
            {
                nextQuestionBool = false;
                StartCoroutine(NextQuestionRoutine());
                roundCounter++;
            }

            timerRunning();
            fillPlayerAnswers();
        }
        if(highscoreFilling)
        {
            playing = false;
            processHighscore();
        }
    }

    private void timerRunning()
    {
        if(startTimer)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString();
        }
    }

    //Nog niet DRY, moest 1 gemaakt worden, nu dit voor testing
    private void fillPlayers()
    {
        //pusher werkt kut, sorry voor deze lelijke slechte code;
        foreach (string player in PusherManager.instance.getPlayerNames())
        {
            if (!dublicateNames.Contains(player))
            {
                dublicateNames.Add(player);
                people.text += player + "\n";
                break;
            }
        }
    }

    private void fillPlayerAnswers()
    {
        //pusher werkt kut, sorry voor deze lelijke slechte code;
        foreach (string player in PusherManager.instance.getPlayerAnswered())
        {
            if (!dublicateNamesAnswered.Contains(player))
            {
                dublicateNamesAnswered.Add(player);
                peopleAnswered.text += player + "\n";
                peopleCount.text =  PusherManager.instance.getPlayerAnswered().Count.ToString() + "/" + dublicateNames.Count.ToString();
                break;
            }
        }
    }

    private IEnumerator waitTillStart()
    {
        yield return new WaitForSeconds(1f);
        //NextQuestion();
        nextQuestionBool = true;
    }

    private void processHighscore()
    { 
        //pusher werkt kut, sorry voor deze lelijke slechte code;
        foreach (string score in PusherManager.instance.getHighscores())
        {
            if (!dublicateHighscores.Contains(score))
            {
                dublicateHighscores.Add(score);
                break;
            }
        }

        if(dublicateHighscores.Count == dublicateNames.Count)
        {
            highscoreFilling = false;
            highScoreText.text = calculateHighScore();
            
            highscorePanel.SetActive(true);
            questionPanel.SetActive(false);
        }
    }

    private string calculateHighScore()
    {
        string[] topNames = new string[dublicateNames.Count];
        int[] topScores = new int[dublicateNames.Count];
        List<int> endScores = new List<int>(){};
        int arrayCounter = 0;

        foreach(string score in dublicateHighscores)
        {
            //string uit elkaar halen
            string[] data = score.Split(',');
            int scoreNumber = int.Parse(data[1]);

            //waardes apart zetten
            topNames[arrayCounter] = data[0];
            topScores[arrayCounter] = scoreNumber;
            endScores.Add(scoreNumber);

            arrayCounter++;
        }

        //score sorteren
        endScores.Sort();

        int place1 = System.Array.IndexOf(topScores, endScores[dublicateHighscores.Count -1]);
        int place2 = System.Array.IndexOf(topScores, endScores[dublicateHighscores.Count -2]);
        int place3 = System.Array.IndexOf(topScores, endScores[dublicateHighscores.Count -3]);

        string[] winner = new string[]{topNames[place1], endScores[dublicateHighscores.Count -1].ToString()};
        string[] second = new string[]{topNames[place2], endScores[dublicateHighscores.Count -2].ToString()};
        string[] third = new string[]{topNames[place3], endScores[dublicateHighscores.Count -3].ToString()};

        string highscoreBuilder = winner[0] + " - " + winner[1] + "\n" + 
                                second[0] + " - " + second[1] + "\n" +
                                third[0] + " - " + third[1];
        
        return highscoreBuilder;
    }

    private IEnumerator NextQuestionRoutine()
    {
        if(roundCounter > 1)
        {
            CheckAnswer();
            yield return new WaitForSeconds(2f);
        }
        
        NextQuestion(roundCounter == GameMaster.Instance.game.getRounds() ? true : false);
        
        timer = GameMaster.Instance.game.getTime();
        startTimer = true;

        yield return new WaitForSeconds(GameMaster.Instance.game.getTime());

        startTimer = false;
        
        if(roundCounter > GameMaster.Instance.game.getRounds())
        {
            highscoreFilling = true;
            Debug.Log("HIGHSCORES");
        }

        nextQuestionBool = true;
    }

    private IEnumerator fillQuestions()
    {
        yield return new WaitForSeconds(2f);
        
        float startPos = 135;

        foreach(string question in GameMaster.Instance.availableQuestions)
        {
            var toggle = Instantiate(myToggle, new Vector3(0,0,0), Quaternion.identity);
            toggle.transform.SetParent(scrollViewContent.transform);
            toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-6,startPos);
            toggle.GetComponent<ToggleQ>().SetName(question);
            startPos -= 20f;
        }
    }
}
