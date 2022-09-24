using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private ControlActions _controlActions;

    [SerializeField] private GameInformation _gameInformation;

    [SerializeField] private MenuManager _menuManager;

    private void Awake()
    {
        Init();
        StartNewGame();
        Player._onCollectable += OnScoreUpdate;
        Player._onGameOver += OnGameOver;

        //Activate Controls for pauseButton
        _controlActions = new ControlActions();
        _controlActions.Player.Pause.Enable();
        _controlActions.Player.Pause.started += OnPauseGame;
    }

    /// <summary>
    /// Restart the game scene and trigger a menu animation
    /// </summary>
    public void RestartGame()
    {
        //Activate blackoutmenu and animation
        _menuManager.ActivateMenu(3);
        Time.timeScale = 1;

        //Delay tween till halfway blackoutmenu and restart game
        float angle = 0;
        DOTween.To(() => angle, x => angle = x, 360, .5f)
            .OnComplete(() => {
                SceneManager.UnloadSceneAsync(1);
                StartNewGame();
            });
    }

    /// Get's called by key and gamepad input
    private void OnPauseGame(InputAction.CallbackContext obj)
    {
        if (_menuManager.IsActive(0))
        {
            _menuManager.DeactivateAll();
            Time.timeScale = 1;
        }
        else
        {
            _menuManager.ActivateMenu(0);
            Time.timeScale = 0;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _menuManager.InitLoopbacks();
    }

    private void FixedUpdate()
    {
        if(_gameInformation._timerIsRunning)
            _gameInformation._playTime += Time.deltaTime;
    }

    private void OnScoreUpdate(float amount)
    {
        _gameInformation._score += amount;
    }

    private void OnGameOver(bool finished)
    {
        _gameInformation._timerIsRunning = false;

        //Player reached finish
        if(finished) _menuManager.ActivateMenu(1);

        //Player died
        if (!finished)
        {
            //Delay tween for menu activation
            float angle = 0;
            DOTween.To(() => angle, x => angle = x, 360, 1.5f)
                .OnComplete(() => {
                    _menuManager.ActivateMenu(2);
                });
        }

        SaveGame();
    }

    private void SaveGame()
    {
        //SAVE JSON HERE
        SaveFile file = new SaveFile
        {
            gameName = _gameInformation._gameName,
            timePlayed = (int)_gameInformation._playTime,
            score = (int)_gameInformation._score,
            timeJsonMade = JsonSystem.GetUnixTime()
        };

        JsonSystem.Save(file);
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        _gameInformation = new GameInformation("Block pushers");
    }

    private void OnDestroy()
    {
        Player._onCollectable -= OnScoreUpdate;
        Player._onGameOver -= OnGameOver;
        _controlActions.Player.Pause.started -= OnPauseGame;
    }

    public float GetPlayTime()
    {
        return _gameInformation._playTime;
    }

    public float GetScore()
    {
        return _gameInformation._score;
    }

    //Used by ui attached button
    public void OnUnPauseButton()
    {
        _menuManager.DeactivateAll();
        Time.timeScale = 1;
    }

    private class GameInformation
    {
        public GameInformation(string gameName)
        {
            _gameName = gameName;
        }

        public string _gameName;
        public float _playTime = 0;
        public float _score = 0;
        public bool _timerIsRunning = true;
    }
}

