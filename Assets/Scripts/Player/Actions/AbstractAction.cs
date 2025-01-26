using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAction : MonoBehaviour
{
    [Header("Player Actions")]
    [SerializeField] protected PlayerActions _playerActions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void ActionMethod();
}
