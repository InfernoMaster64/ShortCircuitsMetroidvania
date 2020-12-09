using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScript : MonoBehaviour
{
    private static PuzzleScript instance;

    bool f2Lever, f3Lever = false;
    public bool f1puzzle = false;
    public GameObject Statue1, Statue2, Statue3;
    public Sprite active;
    public GameObject barrier;


    private void Awake()
    {
        if (instance == null) //prevents duplicate stat objects
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }

    //will be called from the player script once E is pressed on a real statue.
    public void Activation()
    {
        Debug.Log("Let's active some statues!");
        //Changes the statue's sprite to the more active image to show players it's been activated.
        Statue1.GetComponent<SpriteRenderer>().sprite = active;
        CheckStatue();
    }

    public void ActivationTwo()
    {
        Debug.Log("Let's active some statues!");
        //Changes the statue's sprite to the more active image to show players it's been activated.
        Statue2.GetComponent<SpriteRenderer>().sprite = active;
        CheckStatue();
    }

    public void ActivationThree()
    {
        Debug.Log("Let's active some statues!");
        //Changes the statue's sprite to the more active image to show players it's been activated.
        Statue3.GetComponent<SpriteRenderer>().sprite = active;
        CheckStatue();
    }

    public void CheckStatue()
    {
        barrier = GameObject.FindGameObjectWithTag("Barrier");
        Debug.Log("Are they activated?");
        if (Statue1.GetComponent<SpriteRenderer>().sprite == active && Statue2.GetComponent<SpriteRenderer>().sprite == active && Statue3.GetComponent<SpriteRenderer>().sprite == active)
        {
            f1puzzle = true;
            barrier.gameObject.SetActive(false);
        }
    }

    //Will be called whenever a puzzle is completed to see if they all are done in order to open up the boss room.
    //void CheckPuzzle()
    //{
    //    Debug.Log("Puzzles?");
    //    if (f1puzzle == true && f2Lever == true && f3Lever == true)
    //    {
    //        //Insert the barrir for the Boss Room being disabled here.
    //    }
    //}
}
