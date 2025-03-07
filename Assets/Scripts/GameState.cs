﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PHL.Common.Utility;

public class GameState : MonoBehaviour
{
    [SerializeField] private BlockPairSpawner _playerOneSpanwer;
    [SerializeField] private BlockPairSpawner _playerTwoSpanwer;
    [SerializeField]private PlayerBlockInput _playerOneInput;
    [SerializeField]private PlayerBlockInput _playerTwoInput;
    [SerializeField]private AnimationHandler _playerOneAnim;
    [SerializeField]private AnimationHandler _playerTwoAnim;

    private int countdownTime = 3;
    [SerializeField] private Text _countdownDisplayText;
    [SerializeField] private Text _pacWins;
    [SerializeField] private Text _ocWins;
    [SerializeField]private CanvasGroup _gameOverInstructions;
    public SecureEvent gameStartEvent = new SecureEvent();
    public SecureEvent pinFallEvent = new SecureEvent();

    private void Start()
    {
        StartCoroutine(StartCountdownRoutine());

        //these are kinda confusing cause i swapped the characters
        _playerOneSpanwer.gameEndEvent.AddListener(PlayerOneWins);
        _playerTwoSpanwer.gameEndEvent.AddListener(PlayerTwoWins);

    }

    private IEnumerator StartCountdownRoutine()
    {
        //do 3,2,1 ding ding ding instead
        while (countdownTime > 0)
        {
            _countdownDisplayText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
            countdownTime--;
        }
        _countdownDisplayText.text = "FIGHT!";
        yield return new WaitForSeconds(1);
        _playerOneInput.gameObject.SetActive(true);
        _playerTwoInput.gameObject.SetActive(true);
        _playerOneSpanwer.SpawnBlockPair();
        _playerTwoSpanwer.SpawnBlockPair();
        _countdownDisplayText.gameObject.SetActive(false);
        gameStartEvent.Invoke();

    }

    private void PlayerOneWins()
    {
        StartCoroutine(WinnerFade(_pacWins));
        _playerOneSpanwer.StopAllCoroutines();
        _playerTwoSpanwer.StopAllCoroutines();
        _playerOneSpanwer.activeBlockPair.spawnNextEvent.RemoveAllListeners();
        _playerTwoSpanwer.activeBlockPair.spawnNextEvent.RemoveAllListeners();
        pinFallEvent.Invoke();
    }

    private void PlayerTwoWins()
    {
        StartCoroutine(WinnerFade(_ocWins));
        _playerOneSpanwer.StopAllCoroutines();
        _playerTwoSpanwer.StopAllCoroutines();
        _playerOneSpanwer.activeBlockPair.spawnNextEvent.RemoveAllListeners();
        _playerTwoSpanwer.activeBlockPair.spawnNextEvent.RemoveAllListeners();
        pinFallEvent.Invoke();
    }

    IEnumerator WinnerFade(Text winnerText)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            winnerText.color = new Color(winnerText.color.r, winnerText.color.g, winnerText.color.b, i);
            _gameOverInstructions.alpha = i;
            yield return null;
        }
    }
}

