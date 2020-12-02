using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Castle");
        Debug.Log("You clicked the Start Button");
    }

    public void OnHelpButtonClick()
    {
        SceneManager.LoadScene("Help");
        Debug.Log("You clicked the Help Button");
    }

    public void OnCreditButtonClick()
    {
        SceneManager.LoadScene("Credit");
        Debug.Log("You clicked the Credit Button");
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("Main Menu");
        Debug.Log("You clicked the Back Button");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("You clicked the Quit Button");
    }
}
