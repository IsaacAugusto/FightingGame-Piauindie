using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSingleton : MonoBehaviour
{
    public static BoxSingleton Instance = null;

    public int Player1Score, Player2Score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
