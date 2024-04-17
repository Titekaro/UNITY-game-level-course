using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactableObjectType;

    void Start(){
        
    }

    void Update(){
        
    }

    // appeler une fonction de l'extérieur qui affiche touché ds la console
    public void Interact(){

        switch(interactableObjectType) {
            case "chest":
                Debug.Log("yeah! You found a chest !");
                break;
            case "pedestal":
                Debug.Log("it's the pedestal");
                break;
            
        }
    }
}
