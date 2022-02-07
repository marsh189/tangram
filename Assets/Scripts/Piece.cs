using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    [Header("Movement and Rotation")]
    Vector3 offset;
    float zCoord;
    bool selected = false;
    public float yOffset = 0.1f;
    public float positionError = 0.1f;
    public float rotationAmount = 45f;

    [Header("Completion Checks")]
    public bool correct = false;
    public List<Transform> finalPositions;
    public GameManager gameManager;

    [Header("Audio")]
    AudioSource source;
    public AudioClip selectedAudio;
    public AudioClip rotateAudio;
    public AudioClip correctAudio;

    [Header("Instructions")]
    public GameObject moveInstructions;
    public GameObject rotateInstructions;
    


    void Start()
    {
        source = GetComponent<AudioSource>();
        moveInstructions.SetActive(true);
        rotateInstructions.SetActive(false);
        gameManager.instructionsFollowed = 0;
    }
    void Update()
    {
        if (selected)
        {

            if (Input.GetKeyDown(KeyCode.D))
            {
                RotateRight();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                RotateLeft();
            }
        }
        else if (!correct) 
        {
            //once piece is dropped, check if piece is in one of the correct positions
            foreach (Transform finalPosition in finalPositions)
            {
                
                if(Vector3.Distance(transform.position, finalPosition.position) <= positionError) //check to see how far away the piece is to final position
                {
                    if(transform.localRotation.y - finalPosition.localRotation.y < 1) //make sure rotation is correct
                    {
                        source.PlayOneShot(correctAudio);
                        correct = true;
                        Vector3 finalPos = finalPosition.position;
                        finalPos.y = 0;
                        transform.position = finalPos;

                    }
                }
            }
        }

    }

    void OnMouseDown() //mouse has clicked on this piece
    {
        if (!gameManager.levelFinished)
        {
            source.PlayOneShot(selectedAudio);
            zCoord = Camera.main.WorldToScreenPoint(transform.position).z; //get piece's z coordinate in screenpoint
            offset = transform.position - GetMouseWorldPos(); //set offset between piece and mouse position
            selected = true;
            correct = false;

            if(gameManager.instructionsFollowed < 1)
            {
                gameManager.instructionsFollowed++;
                moveInstructions.SetActive(false);
                rotateInstructions.SetActive(true);
            }
        }
    }

    void OnMouseDrag() //mouse button is down and mouse is moving
    {
        if (!gameManager.levelFinished)
        {
            //get mouse's world position and set the piece position to newPos
            Vector3 newPos = GetMouseWorldPos() + offset;
            newPos.y = yOffset;
            transform.position = newPos;
        }
    }

    void OnMouseUp() //mouse button let go
    {
        if (!gameManager.levelFinished)
        {
            //drop piece
            Vector3 newPos = transform.position;
            newPos.y = 0;
            transform.position = newPos;
            selected = false;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint); //find mouse's position in the world point
    }


    public void RotateRight()
    {
        source.PlayOneShot(rotateAudio);
        Vector3 newRotation = transform.eulerAngles;
        newRotation.y += rotationAmount;
        transform.eulerAngles = newRotation;

        if (gameManager.instructionsFollowed < 2)
        {
            gameManager.instructionsFollowed++;
            moveInstructions.SetActive(false);
            rotateInstructions.SetActive(false);
        }
    }

    public void RotateLeft()
    {
        source.PlayOneShot(rotateAudio);
        Vector3 newRotation = transform.eulerAngles;
        newRotation.y -= rotationAmount;
        transform.eulerAngles = newRotation;

        if (gameManager.instructionsFollowed < 2)
        {
            gameManager.instructionsFollowed++;
            moveInstructions.SetActive(false);
            rotateInstructions.SetActive(false);
        }
    }

}
