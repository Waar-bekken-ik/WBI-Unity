using System;
using System.Threading.Tasks;
using PusherClient;
using System.Collections.Generic;
using UnityEngine;

public class PusherManager : MonoBehaviour
{
    public static PusherManager instance = null;
    private Pusher _pusher;
    private Channel _channel;
    private const string APP_KEY = "c6fd201f50ddc27a1163";
    private const string APP_CLUSTER = "eu";
    private List<string> playerNames = new List<string>();
    private List<string> playerAnswered = new List<string>();

    async Task Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Console.WriteLine("Starting");
    }

    public async void StartPusher()
    {
        await InitialisePusher();
    }

    private async Task InitialisePusher()
    {
        //Environment.SetEnvironmentVariable("PREFER_DNS_IN_ADVANCE", "true");

        if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER"))
        {
            _pusher = new Pusher(APP_KEY, new PusherOptions()
            {
                Cluster = APP_CLUSTER,
                Encrypted = true
            });

            _pusher.Error += OnPusherOnError;
            _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            _pusher.Connected += PusherOnConnected;

            _channel = await _pusher.SubscribeAsync(GameMaster.Instance.game.getPin());
            _channel.Subscribed += OnChannelOnSubscribed;
            await _pusher.ConnectAsync();
        }
        else
        {
            Debug.LogError("APP_KEY and APP_CLUSTER must be correctly set.");
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Connected");
        _channel.Bind("my-event", (dynamic data) =>
        {
            Debug.Log("data= " +  data.data);
        });

        _channel.Bind("player-joining", (dynamic data) =>
        {
            Debug.Log("joined= " + data.data);
            playerNames.Add(data.data.ToString());
            //GameMaster.Instance.playerSubscribed("j= ");
        });

        _channel.Bind("send-answer", (dynamic data) =>
        {
            Debug.Log("answered= " + data.data);
            playerAnswered.Add(data.data.ToString());
        });

        
    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.Log("Connection state changed");
    }

    public List<string> getPlayerNames()
    {
        return playerNames;
    }

    public List<string> getPlayerAnswered()
    {
        return playerAnswered;
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.Log("Errored");
    }

    private void OnChannelOnSubscribed(object s)
    {
        Debug.Log("Subscribed");
    }

    async Task OnApplicationQuit()
    {
        if (_pusher != null)
        {
            await _pusher.DisconnectAsync();
        }
    }
}