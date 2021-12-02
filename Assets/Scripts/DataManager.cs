using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    
    class UserData
    {
        public string username;
        public int[] grades;
    }

    string username;

    int[] grades;

    public static DataManager Instance;

    [SerializeField]
    TMP_InputField usernameText;

    [SerializeField]
    GameObject loginScreen, menuScreen, puzzlesScreen;

    [SerializeField]
    GameObject puzzlePrefab;

    [SerializeField]
    GameObject listPuzzles;

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
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Login()
    {
        username = usernameText.text;
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

        loginScreen.SetActive(false);
        menuScreen.SetActive(true);

    }

    int getNumberOfPuzzles()
    {
        return EditorBuildSettings.scenes.Where(scene => scene.enabled && scene.path.ToLower().Contains("puzzle")).Select(scene => scene.path).Count();

    }

    public void Puzzles()
    {
        menuScreen.SetActive(false);
        puzzlesScreen.SetActive(true);
        string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled && scene.path.ToLower().Contains("puzzle")).Select(scene => scene.path).ToArray();
        for (int i = 0; i < scenes.Length; i++)
        {
            Puzzle puzzle = Instantiate(puzzlePrefab).GetComponent<Puzzle>();
            puzzle.puzzleName = scenes[i].Split('/').Last();
            puzzle.populatePuzzle(i+1, grades[i]);
            puzzle.transform.SetParent(listPuzzles.transform);
        }
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
