using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class GuessButtons : MonoBehaviour
{

    public ReadCSV myCSV;

    public bool isGoodAnswer = false;
    public SyncedText sync;


    public void SetGoodAnswer()
    {
        isGoodAnswer = true;
        sync.SetText(ManagerQuizGame.instance.WordToGuess.ToString());
    }

    public void SetBadAnswer()
    {
        isGoodAnswer = false;

        string wrongWord = myCSV.ReadWord(Random.Range(0, ManagerQuizGame.instance.nbWord) , ManagerQuizGame.instance.langueToGuess);
        sync.SetText(wrongWord);
    }
}
