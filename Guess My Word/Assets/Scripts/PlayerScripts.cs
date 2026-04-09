using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScripts : NetworkBehaviour
{
    public int playerID;
    public int personnal_score;

    public bool hasValidAnswer = false;


    private void Start()
    {
        //Check owner
        if (IsOwner == true)
        {
            NetworkObject netcode = GetComponent<NetworkObject>();
            foreach (GuessButtons button in ManagerDisplayWords.instance.listButtons)
            {
                button.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ValidAnswer);
            }
        }
    }


    public override void OnNetworkSpawn()
    {
        ManagerPlayers.instance.playersMap[OwnerClientId] = this;

        if (IsOwner)
        {
            // On vérifie que le manager est bien spawné avant d'appeler
            if (ManagerPlayers.instance.IsSpawned)
                ManagerPlayers.instance.AddPlayerRpc(OwnerClientId);
            else
                StartCoroutine(WaitAndRegister());
        }
    }

    private IEnumerator WaitAndRegister()
    {
        // On attend que le manager soit spawné
        yield return new WaitUntil(() => ManagerPlayers.instance.IsSpawned);
        ManagerPlayers.instance.AddPlayerRpc(OwnerClientId);
    }


    public Action OnValidAnswer;
    public void ValidAnswer()
    {
        print("Un des joueurs a validé une réponse !");

        OnValidAnswer?.Invoke();
    }


    public Action OnGoodAnswer;
    public void GoodAnswer()
    {
        print("Good answer");
        OnGoodAnswer?.Invoke();
    }


    public Action OnWrongAnswer;
    public void WrongAnswer()
    {
        print("wrong answer");
        OnWrongAnswer?.Invoke();
    }


}
