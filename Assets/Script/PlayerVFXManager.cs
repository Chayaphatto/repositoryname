using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
    
{
    public static PlayerVFXManager instance;
    public ParticleSystem footStep;
    // Start is called before the first frame update

    private void Awake()
    {
         instance = this;
    }

    public void FootStep()
    {
        footStep.Play(); 
    }

}
