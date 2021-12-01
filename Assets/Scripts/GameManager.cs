using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    Button startButton;

    [SerializeField]
    GameObject winScreen;

    [SerializeField]
    GameObject loseScreen;

    [SerializeField]
    TextMeshProUGUI score;

    [SerializeField]
    List<Image> queens;

    [SerializeField]
    Chessboard chessBoard;

    [SerializeField]
    int lowGrade, middleGrade, highGrade;

    public float groundLevel;

    public enum Mode { Placement, Game, Won, Lost }

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
        Debug.Log(SystemInfo.operatingSystem);
    }

    void Awake()
    {
        groundLevel = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        //startButton.enabled = mode == Mode.Placement;
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
            if (chessBoard.nbMoves < lowGrade) queens[0].sprite = goldQueen;
            if (chessBoard.nbMoves < middleGrade) queens[1].sprite = goldQueen;
            if (chessBoard.nbMoves < highGrade) queens[2].sprite = goldQueen;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
