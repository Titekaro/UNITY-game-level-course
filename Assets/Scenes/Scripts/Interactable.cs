using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactableObjectType;
    private Animator objectAnimator;

    void Awake() {
        objectAnimator = GameObject.Find("Visual").GetComponent<Animator>();
    }

    void Start(){
        
    }

    void Update(){
        
    }

    // appeler une fonction de l'extérieur qui affiche touché ds la console
    public void Interact(){

        switch(interactableObjectType) {
            case "toOpen":
                OpenObject();
                break;
            case "pedestal":
                Debug.Log("it's the pedestal");
                break;
            case "toBreak":
                Debug.Log("it's to break");
                break;
            
        }
    }

    void OpenObject(){
        Debug.Log("yeah! You found a chest !");
        if(gameObject.GetComponent<AudioSource>() != null) {
            gameObject.GetComponent<AudioSource>().Play();
        }
        if (objectAnimator != null) {
            objectAnimator.SetBool("isOpen", true);   
        }
    }

}
