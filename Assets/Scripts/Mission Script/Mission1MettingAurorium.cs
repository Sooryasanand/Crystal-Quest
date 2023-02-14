using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission1MettingAurorium : MonoBehaviour
{
    public Animator ObjectiveAnimation;

    public void Prerequistes()
    {
        
        ObjectiveAnimation.Play("ObjectiveBoxEntry");
        GameObject.Find("ObjectiveBox").GetComponent<ObjectiveUpdate>().updateObjective("Talk to the King of Aurorium");
    }
}
