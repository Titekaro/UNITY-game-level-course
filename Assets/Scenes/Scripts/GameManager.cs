using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreTextField;

    void Start(){
    }

    void Update(){
    }

    public void UpdateScore() {
        score++;
        scoreTextField.text = "Score: " + score.ToString();
    }
}
