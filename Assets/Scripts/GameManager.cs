using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private Image loadingImage;
    [SerializeField] private TextMeshProUGUI scoreTextField;
    [SerializeField] private int life = 3;
    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    void Update()
    {
        /*
        if (life < 0)
        {
            scoreTextField.text = "GAME OVER";
        }
        else
        {
            scoreTextField.text = "Score: " + score.ToString();
            scoreTextField.text += "Life: " + life.ToString();
        }
        */
        
    }

    public void UpdateScore()
    {
        score++;
    }

    public void UpdateLife()
    {
        life--;
    }

    public void LoadNextLevel(int levelIndex)
    {
        SceneManager.LoadSceneAsync("LoadingScene");
        StartCoroutine(LoadSceneAsync(levelIndex));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        new WaitForSeconds(5);
        Debug.Log("Level " + sceneIndex + " loaded");
    }
}