using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScripts : NetworkBehaviour
{

    public GameObject playerUI;

    public int playerID;
    public int personnal_score;

    public string wordSelect;
    public NetworkVariable<NetworkObjectReference> answerSelected;

    // Propriété helper pour accéder au GuessButtons facilement
    public GuessButtons AnswerSelectedButton
    {
        get
        {
            if (answerSelected.Value.TryGet(out NetworkObject netObj))
                return netObj.GetComponent<GuessButtons>();
            return null;
        }
    }

    public NetworkVariable<bool> hasValidAnswer;


    private void Start()
    {
        //Check owner
        if (IsOwner == true)
        {
            NetworkObject netcode = GetComponent<NetworkObject>();
            foreach (GuessButtons button in ManagerDisplayWords.instance.listButtons)
            {
                NetworkObject netObj = button.GetComponent<NetworkObject>();

                button.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(
                    () => ValidAnswerRpc(netObj)
                );
            }

            InstantiatePlayerUIRpc();
        }

        ManagerPlayers.instance.OnNextRound += RoundStart;
        ManagerPlayers.instance.OnEndRound += RoundEnd;
        
    }

    public Action OnValidAnswer;
    [Rpc(SendTo.Server)]
    public void ValidAnswerRpc(NetworkObjectReference buttonRef)
    {
        if (buttonRef.TryGet(out NetworkObject netObj))
        {
            GuessButtons button = netObj.GetComponent<GuessButtons>();

            hasValidAnswer.Value = true;
            answerSelected.Value = new NetworkObjectReference(button.GetComponent<NetworkObject>());

            //wordSelect = button.word.Value;
            //A faire s'abonner plus tard !!!!!!
            playerUI.GetComponent<Player_UI>().MovePlayerUIRpc(AnswerSelectedButton);

            OnValidAnswer?.Invoke();
        }
    }

    [Rpc(SendTo.Server)]
    public void InstantiatePlayerUIRpc()
    {
        // Vérifie que le Canvas existe
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("Canvas introuvable !");
            return;
        }

        GameObject newPlayerUI = Instantiate(playerUI, canvas.transform);
        playerUI = newPlayerUI;

        playerUI.GetComponent<Player_UI>().player = this;

        // ⚠️ OBLIGATOIRE : spawner l'objet sur le réseau
        playerUI.GetComponent<NetworkObject>().Spawn();
        playerUI.transform.parent = canvas.transform;
  
    }

    //test move in Player UI

    //[Rpc(SendTo.Server)]
    //public void MovePlayerUIRpc()
    //{
    //    if (AnswerSelectedButton != null)
    //    {
    //        //Test de just déplacer les positions
    //        playerUI.transform.position = AnswerSelectedButton.transform.position;
    //    }
    //}


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


    public Action OnGoodAnswer;
    public void GoodAnswer()
    {
        OnGoodAnswer?.Invoke();
    }


    public Action OnWrongAnswer;
    public void WrongAnswer()
    {
        OnWrongAnswer?.Invoke();
    }

    public void CheckAnswer()
    {
        if (IsOwner == false)
            return;

        if (answerSelected.Value.NetworkObjectId == ManagerDisplayWords.instance.goodAnswer.Value.NetworkObjectId)
        {
            GoodAnswer();
        }
        else
            WrongAnswer();
    }

    public Action OnRoundStart;
    public void RoundStart()
    {
        OnRoundStart?.Invoke();
    }

    public Action OnRoundEnd;
    public void RoundEnd()
    {
        OnRoundEnd?.Invoke();
    }


}
