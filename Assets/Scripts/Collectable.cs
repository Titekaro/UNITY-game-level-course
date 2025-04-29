using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{

[SerializeField] private bool isRotatingObject;
[SerializeField] private GameManager gameManager;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start() {    
    }

    void Update() {   
        if(isRotatingObject) {
            gameObject.transform.Rotate(0, 90 * Time.deltaTime, 0);
        }     
    }

    void PlayCollectSound() {
        if(gameObject.GetComponent<AudioSource>() != null) {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void DestroyCollectable() {
        Destroy(gameObject);
    }

    public void CollectObject() {
        PlayCollectSound();
        //gameObject.SetActive(false); is not a good solution because it disables every "child" components (for ex. if a sound has to be played)
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.Find("Visual").gameObject.SetActive(false);
        gameManager.UpdateScore();
        Invoke("DestroyCollectable", 1f);
    }
}
