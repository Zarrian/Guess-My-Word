using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Netcode;

public class PlayerScripts : MonoBehaviour
{
    public int playerID;

    public int personnal_score;
    public string playerName;
    public Image personnalPicture;

    public List<GameObject> answerSelected;

    public bool hasValidAnswer = false;
    public float timeLeft;

    private void Start()
    {

        NetworkObject netcode = GetComponent<NetworkObject>();
        if(netcode.IsLocalPlayer == false)
        {
            return;
        }


        ManagerPlayers.instance.playerLists.Value.Add(this);

        foreach (GuessButtons button in ManagerDisplayWords.instance.listButtons)
        {
            button.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ValidAnswer);
        }

    }




    public Action OnValidAnswer;
    public void ValidAnswer()
    {
        timeLeft = ManagerQuizGame.instance.currentTime.Value;
        print("Un des joueurs a validé une réponse !");

        //Dit au manager qu'un des joueurs a validé la réponse
        ManagerPlayers.instance.PlayerValidAnswer(this);

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
