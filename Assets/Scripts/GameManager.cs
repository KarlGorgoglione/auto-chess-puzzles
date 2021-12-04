using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    GameObject winScreen;

    [SerializeField]
    GameObject loseScreen;

    [SerializeField]
    GameObject tutorialScreen;

    [SerializeField]
    GameObject loadingScreen;

    [SerializeField]
    TextMeshProUGUI score;

    [SerializeField]
    Button nextLevelButton;

    [SerializeField]
    List<Image> queens;

    [SerializeField]
    Chessboard chessBoard;

    [SerializeField]
    int lowGrade, middleGrade, highGrade;

    public float groundLevel;

    public enum Mode { Placement, Game, Won, Lost }

    public Mode mode;

    float transitionElapsedTime, transitionTime;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
           
        Instance = this;
        mode = Mode.Placement;

        loadingScreen.SetActive(true);
        transitionElapsedTime = 0f;
        transitionTime = 0.7f;

        Debug.Log(SystemInfo.operatingSystem);
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingScreen.activeInHierarchy)
        {
            loadingScreen.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, transitionElapsedTime / transitionTime));
            transitionElapsedTime += Time.deltaTime;
            if (transitionElapsedTime > transitionTime) loadingScreen.SetActive(false);
        }
        
    }

    public void hideTutorial()
    {
        tutorialScreen.SetActive(false);
    }

    public void EndGame(bool isWon)
    {
        Debug.Log(isWon);
        if (!isWon)
        {
            mode = Mode.Lost;
            loseScreen.SetActive(true);
        }
        else
        {
            mode = Mode.Won;
            winScreen.SetActive(true);
            score.text = $"You checkmated in {chessBoard.nbMoves} moves";
            Sprite goldQueen = Resources.Load<Sprite>("Sprites/queen2_gold");
            int grade = 0;
            if (chessBoard.nbMoves < lowGrade)
            {
                queens[0].sprite = goldQueen;
                grade++;
            }
            if (chessBoard.nbMoves < middleGrade)
            {
                queens[1].sprite = goldQueen;
                grade++;
            }
            if (chessBoard.nbMoves < highGrade)
            {
                queens[2].sprite = goldQueen;
                grade++;
            }
            DataManager.Instance.SavePuzzle(SceneManager.GetActiveScene().buildIndex - 1, grade);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
