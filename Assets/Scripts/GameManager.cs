using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<Piece> pieces;
    public AudioClip levelCompletedAudio;
    public CanvasGroup finishedScreen;
    public float fadeSpeed = 5f;

    public bool levelFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        finishedScreen.alpha = 0;
        finishedScreen.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCompleted())
        {
            if (!levelFinished)
            {
                Debug.Log("Level Finished");
                pieces[0].GetComponent<AudioSource>().PlayOneShot(levelCompletedAudio);
                levelFinished = true;
            }

            if(finishedScreen.alpha < 1)
            {
                finishedScreen.alpha += Time.deltaTime * fadeSpeed;
            }
            else
            {
                finishedScreen.interactable = true;
            }
        }
    }

    bool CheckCompleted()
    {
        foreach (Piece piece in pieces)
        {
            if (!piece.correct) //At least one piece is not in correct location
            {
                return false;
            }
        }
        return true; //all pieces are placed
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
