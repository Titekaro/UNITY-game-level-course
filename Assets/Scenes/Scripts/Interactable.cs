using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactableObjectType;
    [SerializeField] private Animator objectAnimator;

    void Awake() {
        objectAnimator = transform.Find("Visual").GetComponent<Animator>();
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

    public void OpenObject(){
        Debug.Log("yeah! You found a chest !");
        Debug.Log(transform);
        if(transform.GetComponent<AudioSource>() != null) {
            transform.GetComponent<AudioSource>().Play();
        }
        if (objectAnimator != null) {
            objectAnimator.SetBool("isOpen", true);   
        }
    }

}
