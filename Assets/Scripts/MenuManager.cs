using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    TMP_InputField usernameText;

    [SerializeField]
    GameObject loginScreen, menuScreen, puzzlesScreen;

    [SerializeField]
    Image loadingScreen;

    [SerializeField]
    GameObject puzzlePrefab;

    [SerializeField]
    GameObject listPuzzles;

    List<Puzzle> listPuzzleObject;

    bool isEnterPressed;
    float enterPressedElapsedTime;

    private void Awake()
    {
        usernameText.onEndEdit.AddListener((_) => StartCoroutine(checkEnter()));
        isEnterPressed = false;
        listPuzzleObject = new List<Puzzle>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && puzzlesScreen.activeInHierarchy)
        {
            NavigateToMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return) && !isEnterPressed)
        {
            isEnterPressed = true;
            enterPressedElapsedTime = 0f;
        }

        if (isEnterPressed)
        {
            enterPressedElapsedTime += Time.deltaTime;
        }

        if (enterPressedElapsedTime > 0.5f) isEnterPressed = false;
    }

    IEnumerator checkEnter()
    {
        yield return new WaitForSeconds(0.2f);
        if (isEnterPressed)
        {
            Login();
        }
    }

    void NavigateToMenu()
    {
        puzzlesScreen.SetActive(false);
        loginScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void Login()
    {
        DataManager.Instance.LoginUser(usernameText.text);

        NavigateToMenu();

    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit(); // original code to quit Unity player
#endif
    }


    public void Puzzles()
    {
        foreach(Puzzle puzzle in listPuzzleObject)
        {
            Destroy(puzzle.gameObject);
        }
        listPuzzleObject = new List<Puzzle>();
        menuScreen.SetActive(false);
        puzzlesScreen.SetActive(true);
        string[] scenes = DataManager.Instance.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            Puzzle puzzle = Instantiate(puzzlePrefab).GetComponent<Puzzle>();
            puzzle.puzzleName = scenes[i].Split('/').Last();
            puzzle.populatePuzzle(i + 1, DataManager.Instance.grades[i]);
            puzzle.transform.SetParent(listPuzzles.transform);
            puzzle.loadingScreen = loadingScreen;
            listPuzzleObject.Add(puzzle);
        }
    }
}
