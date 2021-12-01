using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    TextMeshProUGUI score;

    [SerializeField]
    List<Image> queens;

    [SerializeField]
    Chessboard chessBoard;

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
        if (!isWon) mode = Mode.Lost;
        else
        {
            mode = Mode.Won;
            winScreen.SetActive(true);
            score.text = $"You checkmated in {chessBoard.nbMoves} moves";
            Sprite goldQueen = Resources.Load<Sprite>("Sprites/queen2_gold");
            queens.ForEach(queen => queen.sprite = goldQueen);
        }
    }
}
