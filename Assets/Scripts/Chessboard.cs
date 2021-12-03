using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Chessboard : MonoBehaviour
{
    List<string> letters = new List<string>(){ "A", "B", "C", "D", "E", "F", "G", "H" };

    Square[,] board;

    [SerializeField]
    Stockfish stockfish;

    List<(Square, Square)> listMoves;

    public string fen;

    public string turn;

    public bool isCheckmate;

    public bool isDraw;

    Dictionary<string, int> fenCounts;

    public int nbMoves;

    [SerializeField]
    GameObject whiteQueenPrefab, blackQueenPrefab;

    [SerializeField]
    CinemachineVirtualCamera camera;

    Quaternion cameraInitRotation;

    bool rotateCamera;
    Vector3 newRotation = new Vector3(50f, 0f, 0f);
    float rotationElapsedTime, rotationTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraInitRotation = camera.transform.rotation;
        rotateCamera = false;
        rotationTime = 2f;
        board = new Square[8, 8];
        for (int i =0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[j, i] = GameObject.Find($"{letters[j]}{i+1}").GetComponent<Square>();
                board[j, i].squarePosition = new Vector3(j, 0f, i);
                board[j, i].gridPosition = (j, i);
            }
        }
        listMoves = new List<(Square, Square)>();
        turn = "b";
        //StartCoroutine(MovePieceTest());
        isCheckmate = false;
        isDraw = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateCamera)
        {
            
            camera.transform.eulerAngles = Vector3.Lerp(cameraInitRotation.eulerAngles, newRotation, Mathf.SmoothStep(0, 1, rotationElapsedTime / rotationTime));
            rotationElapsedTime += Time.deltaTime;
        }
        if (rotationElapsedTime > rotationTime)
        {
            rotationElapsedTime = 0f;
            rotateCamera = false;
        }
    }

    bool willCastle(Square from, Square to)
    {
        bool isKing = from.squarePiece as King != null;
        bool isLongMove = Mathf.Abs(to.gridPosition.Item1 - from.gridPosition.Item1) > 1;
        bool hasAlreadyMoved = isKing && (from.squarePiece as King).hasMoved;
        return isKing && isLongMove && !hasAlreadyMoved;
    }

    void makeCastle(Square from, Square to)
    {
        to.assignPiece(from.squarePiece);
        from.squarePiece = null;
        if (to.gridPosition.Item1 > from.gridPosition.Item1) // Right castle (B/W)
        {
            board[to.gridPosition.Item1 - 1, to.gridPosition.Item2].assignPiece(board[7, to.gridPosition.Item2].squarePiece);
            board[7, to.gridPosition.Item2].squarePiece = null;
        }
        else // Left castle (B/W)
        {
            board[to.gridPosition.Item1 + 1, to.gridPosition.Item2].assignPiece(board[0, to.gridPosition.Item2].squarePiece);
            board[0, to.gridPosition.Item2].squarePiece = null;
        }
    }

    bool checkPromotion(Square from, Square to)
    {
        if (from.squarePiece as Pawn != null && (to.gridPosition.Item2 == 0 || to.gridPosition.Item2 == 7)) {
            return true;
        }
        return false;
    }

    void makePromotion(Square from, Square to)
    {

        from.removePiece();
        GameObject prefab = turn == "b" ? blackQueenPrefab : whiteQueenPrefab;
        Vector3 pos = new Vector3(to.transform.position.x, 0.1f, to.transform.position.z);
        GameObject queen = Instantiate(prefab, pos, prefab.transform.rotation);
        to.assignPiece(queen.GetComponent<Queen>());
    }

    public void MovePiece(Square from, Square to)
    {
        if (from.squarePiece != null && to.squarePiece == null)
        {
            if (willCastle(from, to))
            {
                Debug.Log("Castling the king");
                makeCastle(from, to);
            }
            else
            {
                Debug.Log($"Moving the piece {from.squarePiece.pieceName} to an empty square");
                if(checkPromotion(from, to))
                {
                    Debug.Log("ON PROMOUVOIT");
                    makePromotion(from, to);
                }
                else
                {
                    to.assignPiece(from.squarePiece);
                    from.squarePiece = null;
                }
            }
        }
        else if (from.squarePiece != null && to.squarePiece != null)
        {
            Debug.Log($"Moving the piece {from.squarePiece.pieceName} to take {to.squarePiece.pieceName}");
            to.removePiece();
            if (checkPromotion(from, to))
            {
                Debug.Log("ON PROMOUVOIT");
                makePromotion(from, to);
            }
            else
            {
                to.assignPiece(from.squarePiece);
                from.squarePiece = null;
            }
        }
        else
        {
            Debug.Log($"Cannot move the piece {from.squarePiece} to {to.squarePosition}");
        }
        turn = turn == "w" ? "b" : "w";
        updateFen();
        nbMoves++;
    }

    public void StartGame()
    {
        if (GameManager.Instance.mode == GameManager.Mode.Placement)
        {
            rotateCamera = true;
            rotationElapsedTime = 0f;
            fenCounts = new Dictionary<string, int>();
            nbMoves = 0;
            stockfish.StartStockfish();
            updateFen();
            StartCoroutine(MovePieceTest());
        }
    }

    IEnumerator MovePieceTest()
    {
        yield return new WaitForSeconds(0.4f);
        
        if (GameManager.Instance.mode == GameManager.Mode.Placement) GameManager.Instance.mode = GameManager.Mode.Game;
        //string move = stockfish.GetBestMove(listMoves.ConvertAll<string>(elem => $"{elem.Item1.name.ToLower()}{elem.Item2.name.ToLower()}").ToArray());
        Debug.Log("Getting the best move from Stockfish");
        string move = stockfish.GetBestMove(fen);
        Debug.Log(move);
        if (move != "(none)" && !isDraw)
        {
            (int, int) fromIdx = (letters.IndexOf(move[0].ToString().ToUpper()), (int.Parse(move[1].ToString()) - 1));
            (int, int) toIdx = (letters.IndexOf(move[2].ToString().ToUpper()), (int.Parse(move[3].ToString()) - 1));

            listMoves.Add((board[fromIdx.Item1, fromIdx.Item2], board[toIdx.Item1, toIdx.Item2]));
            MovePiece(board[fromIdx.Item1, fromIdx.Item2], board[toIdx.Item1, toIdx.Item2]);
            StartCoroutine(MovePieceTest());
        }
        else // Checkmate
        {
            if (isDraw)
            {
                GameManager.Instance.EndGame(false);
            }
            else
            {
                GameManager.Instance.EndGame(turn == "b");
            }
        }
    }

    void updateFen()
    {
        fen = "";
        for (int j = 7; j >= 0; j--)
        {
            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                if (board[i, j].squarePiece != null)
                {
                    if (counter > 0)
                    {
                        fen += counter;
                        counter = 0;
                    }
                    fen += board[i, j].squarePiece.color == ChessPiece.PieceColor.Black
                    ? board[i, j].squarePiece.pieceName.ToLower()
                    : board[i, j].squarePiece.pieceName.ToUpper();
                }
                else
                {
                    counter++;
                }
                
            }
            if (counter > 0)
            {
                fen += counter;
            }
            fen += "/";
        }
        fen += " " + turn;
        fen += " - - 0 1";

        if (fenCounts.ContainsKey(fen))
        {
            fenCounts[fen]++;
            check3MoveRep();
        }
        else
        {
            fenCounts.Add(fen, 1);
        }
        Debug.Log(fen);
    }

    void check3MoveRep()
    {
        if (fenCounts.ContainsValue(3)) isDraw = true;
    }

}
