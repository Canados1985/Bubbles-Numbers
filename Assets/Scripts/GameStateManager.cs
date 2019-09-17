using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameStateManager : MonoBehaviour {

    public static GameStateManager cl_GameStateManager;


    ////// OBJECTS OF EACH GAME STATE ///////

    public GameObject go_MenuStateOnCanvas;
    public GameObject go_GameStateOnCanvas;
    public GameObject go_GameOverStateOnCanvas;

    /////////////////////////////////////////

    GameManager gmRef;
    Canvas canvasRef;

    public bool b_IsGameIsPaused = false;
    public bool b_IsGameMode = false;
    public bool b_MenuMode = false;
    public bool b_GameOverMode = false;

    public bool b_startButtonPressed = false;
    

    void Awake()
    {
    DontDestroyOnLoad(this.gameObject);
    }

    void Start() {

        gmRef = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvasRef = gmRef.mainCanvas;
        cl_GameStateManager = this; 
        go_GameStateOnCanvas.SetActive(false); // Here we make not active Game Mode on Canvas
        go_MenuStateOnCanvas.SetActive(true); // Here we male active Menu Mode on Canvas
        b_IsGameMode = false;
        b_MenuMode = true;
        GameMenu();
    }

    // Game Manu State
    public void GameMenu()
    {

        go_GameStateOnCanvas.SetActive(false); // Here we make not active Game Mode on Canvas
        go_MenuStateOnCanvas.SetActive(true); // Here we make not active Menu Mode on Canvas
        go_GameOverStateOnCanvas.SetActive(false); // Here we make  active Game Over menu on Canvas 
        b_MenuMode = true;
        b_IsGameMode = false;
        b_GameOverMode = false;
        //FindObjectOfType<AudioManager>().Play("menuTheme");
    }

    // Game State
    public void StartGame()
    {
        b_startButtonPressed = true;
    }
    // this folows right after game starts thru the CANVAS Script
    public void InstStartGame()
    {
        go_GameStateOnCanvas.SetActive(true); // Here we make not active Game Mode on Canvas
        go_MenuStateOnCanvas.SetActive(false); // Here we make active Menu Mode on Canvas
        go_GameOverStateOnCanvas.SetActive(false); // Here we make not active Game Over menu on Canvas 
        b_MenuMode = false;
        b_GameOverMode = false;
        b_IsGameMode = true;
        gmRef.GameStateInstantiation();
        
    }

    // Game Pause State
    public void PauseGame()
    {

    }
    // Game Unpause State
    public void UnpausedGame()
    {

    }
    
    // Game Over State
    internal void GameOver()
    {
        
        go_GameStateOnCanvas.SetActive(false); // Here we make not active Game Mode on Canvas
        go_MenuStateOnCanvas.SetActive(false); // Here we make not active Menu Mode on Canvas
        go_GameOverStateOnCanvas.SetActive(true); // Here we make  active Game Over menu on Canvas 
        b_MenuMode = false;
        b_IsGameMode = false;
        b_GameOverMode = true;
    }
    // Game Win State
    internal void GameWin()
    {


    }
    // Game Exit
    public void GameExit()
    {
        Debug.Log("Here we close aplication");
        Application.Quit();
    }


    public void ReloadScene()
    { SceneManager.LoadScene(0); }





    void Update () {

        

    }
}
