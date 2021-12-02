using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField]
    TMP_InputField username;

    [SerializeField]
    GameObject loginScreen, menuScreen, puzzlesScreen;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Login()
    {
        string path = Application.persistentDataPath + username.text + ".json";
        if (File.Exists(path))
        {
            Debug.Log("Existing username, loading the main menu...");
            string userData = File.ReadAllText(path);
        }
        else
        {
            Debug.Log("New username");
            File.Create(path);
        }

        loginScreen.SetActive(false);
        menuScreen.SetActive(true);

    }

    public void Puzzles()
    {
        menuScreen.SetActive(false);
        puzzlesScreen.SetActive(true);
    }




}
