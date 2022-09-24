using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UserInterfaceManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;

    private void Start()
    {
        gameManager = GameManager.Instance;
        Player._onCollectable += OnScoreUpdate;
    }

    private void FixedUpdate()
    {
        _timeText.text = "Time: " + ((int)gameManager.GetPlayTime()).ToString();
    }

    private void OnScoreUpdate(float amount)
    {
        _scoreText.text = "Score: " + ((int)gameManager.GetScore()).ToString();
    }
}
