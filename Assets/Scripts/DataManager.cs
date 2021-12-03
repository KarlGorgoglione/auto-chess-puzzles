using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    
    class UserData
    {
        public string username;
        public int[] grades;
    }

    string username;

    public int[] grades;

    public static DataManager Instance { get; private set; }

    public string[] scenes;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        scenes = getAllScenesPath().Where(path => path.ToLower().Contains("puzzle")).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string[] getAllScenesPath()
    {
        // Get build scenes
        var sceneNumber = SceneManager.sceneCountInBuildSettings;
        string[] arrayOfNames;
        arrayOfNames = new string[sceneNumber];
        for (int i = 0; i < sceneNumber; i++)
        {
            arrayOfNames[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        return arrayOfNames;
    }

    public void LoginUser(string user)
    {
        username = user;
        string path = GetActivePath();
        if (File.Exists(path))
        {
            Debug.Log("Existing username, loading the main menu...");
            UserData data = LoadUserData();
            username = data.username;
            grades = data.grades;
        }
        else
        {
            Debug.Log("New username");
            grades = new int[getNumberOfPuzzles()];
            grades.Select(_ => 0);

            UserData data = new UserData();
            data.username = username;
            data.grades = grades;
            SaveUserData(data);
        }

    }

    int getNumberOfPuzzles()
    {
        return getAllScenesPath().Where(scene => scene.ToLower().Contains("puzzle")).Count();
    }

    public string GetActivePath()
    {
        return Application.persistentDataPath + "/" + username + ".json";
    }

    UserData LoadUserData()
    {
        string userData = File.ReadAllText(GetActivePath());
        UserData data = JsonConvert.DeserializeObject<UserData>(userData);
        Debug.Log(data.grades);
        Debug.Log(data.username);
        return data;
    }

    void SaveUserData(UserData data)
    {
        File.WriteAllText(GetActivePath(), JsonConvert.SerializeObject(data));
    }


    public void SavePuzzle(int puzzleIdx, int grade)
    {
        Debug.Log(puzzleIdx);
        Debug.Log(grade);
        UserData data = LoadUserData();

        data.grades[puzzleIdx] = grade;
        SaveUserData(data);
    }

}
