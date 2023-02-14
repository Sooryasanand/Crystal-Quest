using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUpdate : MonoBehaviour
{
    
    public Text objective;

    public void updateObjective(string objectiveInput)
    {
        
        objective.text = objectiveInput;
    }
}
