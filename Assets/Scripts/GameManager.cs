using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Mode { Placement, Game }

    public Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        mode = Mode.Placement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
