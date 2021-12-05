using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockfish : MonoBehaviour
{

    System.Diagnostics.Process stockfish;

    string nextMove;

    bool isMoveReady;

    [SerializeField]
    Chessboard chessboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveReady)
        {
            chessboard.playNextMove(nextMove);
            isMoveReady = false;
        }
    }

    public void StartStockfish(string os)
    {
        String path = $"{Application.streamingAssetsPath}/Stockfish/";
        if (os == "windows")
        {
            path += "Windows/stockfish_14.1_win_x64_avx2.exe";
        }
        else
        {
            path += "Linux/stockfish_14.1_linux_x64_bmi2";
        }
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = path;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler((sender, e) =>
        {
            if (e.Data.Contains("bestmove"))
            {
                nextMove = e.Data.Split(' ')[1];
                isMoveReady = true;
            }
        });
        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        p.Start();
        p.BeginOutputReadLine();
        stockfish = p;
    }

 
    public void GetBestMove(string[] moves)
    {
        nextMove = null;

        string setupString = "position startpos moves " + String.Join(" ", moves);
        stockfish.StandardInput.WriteLine(setupString);
        // Process for 5 seconds
        string processString = "go movetime 500";

        // Process 20 deep
        // string processString = "go depth 20";

        stockfish.StandardInput.WriteLine(processString);
    }

    public void GetBestMove(string fen)
    {
        nextMove = null;
        string setupString = "position fen " + fen;

        stockfish.StandardInput.WriteLine(setupString);
        // Process for 5 seconds
        string processString = "go movetime 250";

        // Process 20 deep
        // string processString = "go depth 20";

        stockfish.StandardInput.WriteLine(processString);
        /*if (nextMove[nextMove.Length-1] == 'q')
        {
            return nextMove.Replace("q", string.Empty);
        }*/
    }

}
