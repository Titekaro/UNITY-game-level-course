using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactableObjectType;
    private Animator chestAnimator;

    void Awake() {
        chestAnimator = GameObject.Find("Visual").GetComponent<Animator>();
    }

    void Start(){
        
    }

    void Update(){
        
    }

    // appeler une fonction de l'extérieur qui affiche touché ds la console
    public void Interact(){

        switch(interactableObjectType) {
            case "chest":
                OpenChest();
                break;
            case "pedestal":
                Debug.Log("it's the pedestal");
                break;
            
        }
    }

    void OpenChest(){
        Debug.Log("yeah! You found a chest !");
        chestAnimator.SetBool("isOpen", true);
    }
}
