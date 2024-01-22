using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickfixing : MonoBehaviour
{
    public static quickfixing Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        DontDestroyOnLoad(Instance);
    }
}
