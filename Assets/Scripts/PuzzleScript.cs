﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScript : MonoBehaviour
{
    bool f2Lever, f3Lever, f1puzzle = false;
    GameObject Statue1, Statue2, Statue3;
    public Sprite active;

    // Start is called before the first frame update
    void Start()
    {
        //Statues now get picked up as.. well the stautes. the Dummy statues shall be ignored as they do absolutly nothing.
        Statue1 = GameObject.Find("Statue 1");
        Statue2 = GameObject.Find("Statue 2");
        Statue3 = GameObject.Find("Statue 3");
    }


    //will be called from the player script once E is pressed on a real statue.
    void Activation()
    {
        Debug.Log("Let's active some statues!");
        //Changes the statue's sprite to the more active image to show players it's been activated.
        Statue1.GetComponent<SpriteRenderer>().sprite = active;
    }

    void CheckStatue()
    {
        Debug.Log("Are they activated?");
        if (Statue1.GetComponent<SpriteRenderer>().sprite == active && Statue2.GetComponent<SpriteRenderer>().sprite == active && Statue3.GetComponent<SpriteRenderer>().sprite == active)
        {
            f1puzzle = true;
            CheckPuzzle();
        }
    }

    //Will be called whenever a puzzle is completed to see if they all are done in order to open up the boss room.
    void CheckPuzzle()
    {
        Debug.Log("Puzzles?");
        if (f1puzzle == true && f2Lever == true && f3Lever == true)
        {

        }
    }
}
