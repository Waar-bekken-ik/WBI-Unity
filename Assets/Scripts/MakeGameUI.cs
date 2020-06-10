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
    public GameObject panel0;
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

    public Text peopleJoinedCounterText;

    public GameObject[] helpTexts;

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

        GameMaster.Instance.game.setMakeRoom(GameMaster.Instance.game.getQuestionsCount(), int.Parse(time.text));
    }

    public void NextQuestion()
    {
        GameMaster.Instance.game.nextQuestion();
        currentQuestion.text = GameMaster.Instance.game.getCurrentQuestion();

        dublicateNamesAnswered.Clear();
        PusherManager.instance.ResetPlayerAnswered();
        peopleAnswered.text = "";
        peopleCount.text =  "0" + "/" + dublicateNames.Count.ToString();
    }

    public void ShowHelpText(int wich)
    {
        helpTexts[wich].SetActive(true);
    }

    public void HideHelpText(int wich)
    {
        helpTexts[wich].SetActive(false);
    }

    public void ShowChoosePanel()
    {
        makePanel.SetActive(true);
        panel0.SetActive(false);
    }

    public void CheckAnswer(bool last)
    {
        GameMaster.Instance.game.checkAnswer(last);
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
            timerText.text = timer.ToString("0");
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

                peopleJoinedCounterText.text = dublicateNames.Count.ToString() + ((dublicateNames.Count == 1) ? " student speelt mee" : " studenten spelen mee");
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
                Debug.Log(score + " added");
                break;
            }
        }

        if(dublicateHighscores.Count == dublicateNames.Count)
        {
            Debug.Log("IN DUB NAMES");
            highscoreFilling = false;
            highScoreText.text = calculateHighScore();

            questionPanel.SetActive(false);

            PelvisSectionShower.Instance.ResetCamera();

            StartCoroutine(showHighScoreUI());
        }
    }

    private string calculateHighScore()
    {
        string[] topNames = new string[dublicateHighscores.Count];
        int[] topScores = new int[dublicateHighscores.Count];
        List<int> endScores = new List<int>(){};
        int arrayCounter = 0;
        //Debug.Log("VOOR DE LOGICA IN HIGHSCOEW");

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
            //Debug.Log("IN STRING UIT ELKAAR HALEN");
        }

        //score sorteren
        endScores.Sort();
        //Debug.Log("LIST GESORTEERD");

        string highscoreBuilder = "";

        if(dublicateHighscores.Count >= 1)
        {
            int place1 = System.Array.IndexOf(topScores, endScores[dublicateHighscores.Count -1]);
            string[] winner = new string[]{topNames[place1], endScores[dublicateHighscores.Count -1].ToString()};
            highscoreBuilder += winner[0] + " - " + winner[1] + "\n";

            if(dublicateHighscores.Count >= 2)
            {
                int place2 = System.Array.IndexOf(topScores, endScores[dublicateHighscores.Count -2]);
                string[] second = new string[]{topNames[place2], endScores[dublicateHighscores.Count -2].ToString()};
                highscoreBuilder += second[0] + " - " + second[1] + "\n";

                if(dublicateHighscores.Count >= 3)
                {
                    int place3 = System.Array.IndexOf(topScores, endScores[dublicateHighscores.Count -3]);
                    string[] third = new string[]{topNames[place3], endScores[dublicateHighscores.Count -3].ToString()};
                    highscoreBuilder += third[0] + " - " + third[1];
                }
            }
        }
        return highscoreBuilder;
    }

    private IEnumerator NextQuestionRoutine()
    {
        if(roundCounter > 1)
        {
            CheckAnswer(false);
            yield return new WaitForSeconds(5f);
        }
        
        NextQuestion();

        yield return new WaitForSeconds(3.5f);
        
        timer = GameMaster.Instance.game.getTime();
        startTimer = true;

        yield return new WaitForSeconds(GameMaster.Instance.game.getTime());

        startTimer = false;
        
        if(roundCounter > GameMaster.Instance.game.getRounds())
        {
            CheckAnswer(true);
            highscoreFilling = true;
            Debug.Log("HIGHSCORES");

            //roundCounter > GameMaster.Instance.game.getRounds() ? true : false
        }

        nextQuestionBool = true;
    }

    private IEnumerator fillQuestions()
    {
        yield return new WaitForSeconds(2f);
        
        float startPosLeft = 337;
        float startPosRight = 337;
        bool setLeft = true;

        foreach(string question in GameMaster.Instance.availableQuestions)
        {
            var toggle = Instantiate(myToggle, new Vector3(0,0,0), Quaternion.identity);
            toggle.transform.SetParent(scrollViewContent.transform);

            if(setLeft)
            {
                toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-460,startPosLeft);
                startPosLeft -= 137f;
                setLeft = false;
            }
            else
            {
                toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(460,startPosRight);
                startPosRight -= 137f;
                setLeft = true;
            }

            toggle.GetComponent<TogglePart>().SetName(question);
        }
    }

    private IEnumerator showHighScoreUI()
    {
        yield return new WaitForSeconds(2.5f);
        highscorePanel.SetActive(true);
    }
}
