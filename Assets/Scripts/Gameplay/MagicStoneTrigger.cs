using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStoneTrigger : MonoBehaviour
{

    [SerializeField] private GameObject target;

    private void OnTriggerEnter()
    {
        Dissolve(target);
    }

    private void Dissolve(GameObject target)
    {
        Material material = target.transform.GetComponent<SkinnedMeshRenderer>().material;
        
        material.SetFloat("CutoffHeight", 0.5f);
        target.SetActive(false);
    }
}
