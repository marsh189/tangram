using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<Piece> pieces;
    public AudioClip levelCompletedAudio;
    bool levelFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCompleted() && !levelFinished)
        {
            Debug.Log("Level Finished");
            pieces[0].GetComponent<AudioSource>().PlayOneShot(levelCompletedAudio);
            levelFinished = true;
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
}
