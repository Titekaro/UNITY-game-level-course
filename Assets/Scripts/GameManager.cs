using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextField;
    [SerializeField] private int life = 3;
    private int score = 0;

    void Awake() {
    }

    void Start(){
    }

    void Update(){
        if(life < 0) {
            scoreTextField.text = "GAME OVER";
        } else {
            scoreTextField.text = "Score: " + score.ToString();
            scoreTextField.text += "Life: " + life.ToString();
        }
    }

    public void UpdateScore() {
        score++;
    }

    public void UpdateLife() {
        life--;
    }
}
