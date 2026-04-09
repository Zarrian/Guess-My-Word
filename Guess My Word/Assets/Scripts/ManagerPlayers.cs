using System.Collections.Generic;
using System.Security.Claims;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ManagerPlayers : NetworkBehaviour
{
    public static ManagerPlayers instance;

    private void Start()
    {
        ManagerQuizGame.instance.OnNewRound += NextRound;
        ManagerQuizGame.instance.OnEndRound += EndRound;
    }


    public NetworkVariable<int> numberPlayers = new NetworkVariable<int>(0);

    public Dictionary<ulong, PlayerScripts> playersMap = new();

    [Rpc(SendTo.Server)]
    public void AddPlayerRpc(ulong clientId)
    {
        numberPlayers.Value++;

        // On retrouve le script via le dictionnaire
        if (playersMap.TryGetValue(clientId, out PlayerScripts player))
        {
            player.playerID = numberPlayers.Value;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void PlayerValidAnswer(PlayerScripts PlayerValid)
    {
            //ManagerQuizGame.instance.EndRound();
    }


    public void NextRound()
    {
  
    }

    public void EndRound()
    {

    }


}
