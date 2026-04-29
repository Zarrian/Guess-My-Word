using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManagerQuizGame : NetworkBehaviour
{
    [Header("CSV Part")]
    public ReadCSV readCSV;
    [HideInInspector] public int nbWord = 220;

    [Header("Langague List")]
    public List<int> listLangages;
    [HideInInspector] public List<int> langageToGuessList;

    [Header("Word To Guess")]
    public string WordDisplay;
    public string WordToGuess;

    [HideInInspector] public int wordLine;
    [HideInInspector] public int originalLangage;
    [HideInInspector] public int langueToGuess;

    [Header("Timer")]
    public NetworkVariable<bool> TimerOnGoing;

    public NetworkVariable<float> maxTimePerRound;
    public NetworkVariable<float> currentTime;

    public static ManagerQuizGame instance;

    private void FixedUpdate()
    {
        if(TimerOnGoing.Value == true)
        {
            currentTime.Value += Time.fixedDeltaTime;
            if(currentTime.Value >= maxTimePerRound.Value)
            {
                EndRound();
            }
        }
    }

    private void Awake()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public Action OnStartGame;
    public void StartGame()
    {
        print("Start Game !");
        OnStartGame?.Invoke();
    }

    public Action OnEndGame;
    public void EndGame()
    {
        print("End Game !");
        OnEndGame?.Invoke();
    }


    public Action OnNewRound;
    /// <summary>
    /// Randomise a Word to guess and the langage to guess
    /// </summary>
    public void NewRound()
    {
        TimerOnGoing.Value = true;
        print("NewRound !");

        //Random a word and a langue
        wordLine = UnityEngine.Random.Range(1, nbWord);
        originalLangage = listLangages[UnityEngine.Random.Range(0, listLangages.Count)];

        //Display the word
        WordDisplay = readCSV.ReadWord(wordLine, originalLangage);

        //Copy the list
        langageToGuessList.Clear();
        langageToGuessList = new List<int>();
        foreach (int langage in listLangages)
        {
            if(langage != originalLangage)
                langageToGuessList.Add(langage);
        }


        //Then choose a langue to guess in
        langueToGuess = langageToGuessList[UnityEngine.Random.Range(0, langageToGuessList.Count)];
        WordToGuess = readCSV.ReadWord(wordLine, langueToGuess);

        //Call Action
        OnNewRound?.Invoke();
    }


    public Action OnEndRound;
    public void EndRound()
    {
        TimerOnGoing.Value = false;
        currentTime.Value = 0;
        OnEndRound?.Invoke();
    }


    //Maybe i willl move this function elsewhere ?
    public Action OnShowResult;
    public void ShowResult()
    {
        print("ShowResult");

        OnShowResult?.Invoke();
    }

}
