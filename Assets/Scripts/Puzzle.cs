using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Puzzle : MonoBehaviour
{

    public string puzzleName;

    [SerializeField]
    TextMeshProUGUI puzzleNumber;

    [SerializeField]
    List<Image> queens;

    // Start is called before the first frame update
    void Awake()
    {
        //colorQueens(nbQueens);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void populatePuzzle(int nb, int grade)
    {
        puzzleNumber.text = nb.ToString();
        Sprite goldQueen = Resources.Load<Sprite>("Sprites/queen2_gold");
        for (int i = 0; i < grade; i++)
        {
            queens[i].sprite = goldQueen;
        }
    }

    public void loadPuzzle()
    {
        SceneManager.LoadScene(puzzleName.Split('.')[0]);
    }
}
