using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
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

            print(AnswerSelectedButton.transform);

            //wordSelect = button.word.Value;
            //A faire s'abonner plus tard !!!!!!
            MovePlayerUIRpc();

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

        // ⚠️ OBLIGATOIRE : spawner l'objet sur le réseau
        playerUI.GetComponent<NetworkObject>().Spawn();

        playerUI.transform.parent = canvas.transform;
    }

    [Rpc(SendTo.Server)]
    public void MovePlayerUIRpc()
    {
        if (AnswerSelectedButton != null)
        {
            print("change position UI");
            //Test de just déplacer les positions
            playerUI.transform.position = AnswerSelectedButton.transform.position;
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
