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

    public Image loadingScreen;

    bool isLoadingEnabled;
    float elapsedTransitionTime, transitionTime;

    // Start is called before the first frame update
    void Awake()
    {
        transitionTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoadingEnabled)
        {
            loadingScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTransitionTime / transitionTime));
            elapsedTransitionTime += Time.deltaTime;
        }
        if (elapsedTransitionTime > transitionTime) isLoadingEnabled = false;
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
        loadingScreen.gameObject.SetActive(true);
        elapsedTransitionTime = 0f;
        isLoadingEnabled = true;
        
        StartCoroutine(loadScene());
        
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(puzzleName.Split('.')[0]);
    }
}
