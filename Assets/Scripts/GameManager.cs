
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public Helper helperRef; // Helper script
    public GameStateManager stateManagerRef;

    public int screenWidth;
    public int screenHeight;
    public float playableWidth;
    public float playableHeight;


    public bool playerLostBubbleGM = false; // bool for help takes value from PlayerRef;
    public float TESTPLAYERSPROGRESS; 
     
    public Canvas mainCanvas;
    public ProgressBar progressBar; // Progress bar on UI
    public Text playerTurnTextUI; // player's turn text on UI
    public Text rebornPlayerBubbleTextUI; // reborn player's bubble text on UI
    public RuntimeAnimatorController[] bubblesController = new RuntimeAnimatorController[8]; 
    
    // ***** OBJECTS POOL ***** //
    public GameObject go_Pool_Container; // Container which holds objects with Text Mesh component
    public GameObject go_TopPlatforms_Container; // This container has all platforms on top
    public GameObject go_BottonPlatforms_Container; // This container has all platforms in the bottom
    public GameObject go_RightPlatforms_Container; // This container has all platforms from the right
    public GameObject go_LeftPlatforms_Container; // This container has all platforms from the left

    public GameObject go_Effects_Container; // This is container has all effects
    public GameObject touch_Effect; // touch effect game object
    public GameObject score_Prefab; // Prefab for list below player scores 
    public GameObject impact_Prefab;



    public List<GameObject> topPlatformsList = new List<GameObject>();
    public List<GameObject> bottomPlatformsList = new List<GameObject>();
    public List<GameObject> leftPlatformsList = new List<GameObject>();
    public List<GameObject> rightPlatformsList = new List<GameObject>();
    public List<GameObject> score = new List<GameObject>(); // Here we have a list with score MeshText
    public List<GameObject> loot = new List<GameObject>(); // Here we have a list with loot gamaobjects
    public List<GameObject> effects = new List<GameObject>(); // Here we have a list with effects gamaobjects

    //public GameObject[] explosion = new GameObject[5]; 
    //public GameObject[] explosion_poison = new GameObject[5]; 
   // public List<GameObject> impacts = new List<GameObject>(); // Here we have a list with impacts gamaobjects

    ///////////////////////////////////////////////////////////////////

    public List<GameObject> bubbles = new List<GameObject>();

    public List<GameObject> bubblesEnd = new List<GameObject>();

    public GameObject explosionPrefab;
    public GameObject explosion_poisonPrefab;
    public GameObject bubblePrefab; // example of bubble GameObject

    public GameObject bubbleEndPrefab; // example of bubbleEnd GameObject

    public GameObject playerPrefab; // exaplme of player GameObject

    public GameObject platformEdgePrefab;
    
    public Camera mainCamera;

    public Player playerRef;
    public int highScore;
    public int highBubbleSize;
    public int highKills;

    public bool touchEvent = false;
    public bool gameIsStarts = false;

    public bool GameIsOver = false;
    public bool deleteGame = false;
    public bool notSpawnNewBubbles;
    public bool playerTurn;

    public bool mute = false;
    public bool faqActive;

    public bool creditsActive;

    /////  RANDOM's INT's and BOOL's CONTROL OVER PANELS  ///////
    public int chanceToHavePotralOnTurn;
    public int chanceToHavePortalTopBottomPanel;
    public int chanceToHavePortalLeftRightPanel;

    public int chanceToHaveZero;

    public bool zeroEdgeOnTop;
    public bool zeroEdgeOnRight;
    public bool zeroEdgeOnBottom;
    public bool zeroEdgeOnLeft;


    ///////////////////////////////////////////////////////////

    void Awake()
    {
      DontDestroyOnLoad(this.gameObject);
    }


    //////// INSTANTIATIONS START NEW GAME  ////////
    public void GameStateInstantiation()
    {

        //Time.timeScale = 1;
        gameManager = this; // Inst of GameManager
        Instantiate(playerPrefab); // Inst main player
        stateManagerRef = GameObject.Find("GameStateManager").GetComponent<GameStateManager>();
        helperRef = gameObject.GetComponent<Helper>();
        mainCamera = Camera.main;
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        mainCanvas.record = PlayerPrefs.GetInt("HighScore"); 
        go_Effects_Container = GameObject.Find("LootContainer"); // assign REF for LootContainer
        //Finding all game objects here
        go_Pool_Container = GameObject.Find("PoolContainer");
        go_TopPlatforms_Container = GameObject.Find("TopPlatformContainer");
        go_BottonPlatforms_Container = GameObject.Find("BottomPlatformContainer");
        go_LeftPlatforms_Container = GameObject.Find("LeftPlatformContainer");
        go_RightPlatforms_Container = GameObject.Find("RightPlatformContainer");

        chanceToHavePortalTopBottomPanel = Random.Range(0, 8);
        chanceToHavePortalLeftRightPanel = Random.Range(0, 4);  
        chanceToHavePotralOnTurn = Random.Range(0, 11);

        helperRef.SetScreenEdges();

        touchEvent = false;
        gameIsStarts = false;
        playerLostBubbleGM = false;   
        deleteGame = false;
        GameIsOver = false;
        faqActive = false;
        creditsActive = false;


        // finding and applying animation controllers  
        for (int i = 0; i< bubblesController.Length; i++)
        {
            if (i == 0) { bubblesController[i] = (RuntimeAnimatorController)Resources.Load("blue_controller"); }
            if (i == 1) {  bubblesController[i] = (RuntimeAnimatorController)Resources.Load("green_controller"); }
            if (i == 2) { bubblesController[i] = (RuntimeAnimatorController)Resources.Load("pink_controller"); }
            if (i == 3) {  bubblesController[i] = (RuntimeAnimatorController)Resources.Load("purple_controller"); }
            if (i == 4) {  bubblesController[i] = (RuntimeAnimatorController)Resources.Load("red_controller"); }
            if (i == 5) { bubblesController[i] = (RuntimeAnimatorController)Resources.Load("white_controller"); }
            if (i == 6) {  bubblesController[i] = (RuntimeAnimatorController)Resources.Load("yellow_controller"); }
            if (i == 7) {  bubblesController[i] = (RuntimeAnimatorController)Resources.Load("player_controller"); }
        }
        
        // Instantiation of the explosion
        for (int i = 0; i <5; i++ )
        {
            GameObject tempExplosion = Instantiate(explosionPrefab);
            tempExplosion.transform.parent = go_Effects_Container.transform;
            effects.Add(tempExplosion);
        }
        // Instatntiation of the explosion with Poison
        for (int i = 0; i <5; i++ )
        {
            GameObject tempExplosionPoison = Instantiate(explosion_poisonPrefab);
            tempExplosionPoison.transform.parent = go_Effects_Container.transform;
            effects.Add(tempExplosionPoison);
        }

        //Here we can change quantaty of the bubbles for spawn ---->
        for (int i = 0; i <10; i++ )
        {
            GameObject tempBubble = Instantiate(bubblePrefab);
            bubbles.Add(tempBubble);
        }
        //SET SPECIFIC NUMBER FOR EACH BUBBLE IN THE LIST
        for (int i = 0; i < bubbles.Count; i++)
        {
            Bubble tempBubble = bubbles[i].GetComponent<Bubble>();
            tempBubble.NUMBER_in_the_list = i;
        }

        //Here we can change quantaty of the impact_RED for spawn ---->
        for (int i = 0; i <25; i++ )
        {
            GameObject tempBubble = Instantiate(impact_Prefab);
            tempBubble.name = "impact_RED";
            //effects.Add(tempBubble);

        }

        for (int i = 0; i <25; i++ )
        {
            GameObject tempBubble = Instantiate(impact_Prefab);
            tempBubble.name = "impact_GREEN";
            //impacts.Add(tempBubble);

        }

        //Creating list of the objects which hold BUBBLE_ENDS OBJECTS ---->
        for (int i = 0; i <100; i++ )
        {
            GameObject tempBubble = Instantiate(bubbleEndPrefab);
            tempBubble.SetActive(false);
            tempBubble.name = "bubbleEnd_" + i;
            tempBubble.transform.parent = go_Pool_Container.transform;
            tempBubble.transform.position = go_Pool_Container.transform.position;
            bubblesEnd.Add(tempBubble);
        }


        //Creating list of the objects which hold SCORES OBJECTS ---->
        for (int i = 0; i <100; i ++)
        {
            GameObject tempObj = Instantiate(score_Prefab);
            tempObj.SetActive(false);
            score.Add(tempObj);
            score[i].transform.parent = go_Pool_Container.transform;
            score[i].transform.position = go_Pool_Container.transform.position;
            score[i].name = "score_NoName";         

        }


        /////////////////////////////// HERE WE ADD EDGE PLATFORMS IN THE GAME ///////////////////////////
        #region START FOR All TABLE EGES IS HERE
            
        
        //Calling TOP platroms and ading them into top list
        for (int i = 0; i < 10; i ++)
        {
            GameObject tempObj = Instantiate(platformEdgePrefab);
            GameObject tempChild = tempObj.transform.GetChild(0).gameObject;
            //Debug.Log(tempChild.name);
            topPlatformsList.Add(tempObj);
            topPlatformsList[i].transform.parent = go_TopPlatforms_Container.transform;
            
            EdgeBoard tempAwaiking = tempObj.GetComponent<EdgeBoard>();

            if(i==chanceToHavePortalTopBottomPanel && chanceToHavePotralOnTurn > 5)
            {
              tempAwaiking.panelIsPortal = true;
            }

            if(i==0)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i + 1 * 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            if(i==1)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 4, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            if(i==2)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i; 
              tempAwaiking.numberInList = i;     
            }
            if(i==3)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==4)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==5)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==6)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==7)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==8)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==9)
            {
              topPlatformsList[i].transform.position = new Vector3(go_TopPlatforms_Container.transform.position.x + i * 2 + 2, go_TopPlatforms_Container.transform.position.y, go_TopPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 1 + (float)i;
              tempAwaiking.timerAwakingInMemory = 1 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            topPlatformsList[i].name = "TopPlatform";         
            tempObj.transform.rotation = new Quaternion(0,0,0,0);
        }

        //Calling RIGHT platforms and ading them into bottom list
        for (int i = 0; i < 6; i ++)
        {
            GameObject tempObj = Instantiate(platformEdgePrefab);
            GameObject tempChild = tempObj.transform.GetChild(0).gameObject;
            rightPlatformsList.Add(tempObj);
            rightPlatformsList[i].transform.parent = go_RightPlatforms_Container.transform;
            
            EdgeBoard tempAwaiking = tempObj.GetComponent<EdgeBoard>();

            if(i==chanceToHavePortalLeftRightPanel && chanceToHavePotralOnTurn > 5)
            {
               tempAwaiking.panelIsPortal = true;
            }

            if(i==0)
            {
              rightPlatformsList[i].transform.position = new Vector3(go_RightPlatforms_Container.transform.position.x - 0.75f, go_RightPlatforms_Container.transform.position.y + i + 0.5f * 2, go_RightPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 16 + (float)i;  
              tempAwaiking.timerAwakingInMemory = 16 + (float)i;
              tempAwaiking.numberInList = i;   
            }
            if(i==1)
            {
              rightPlatformsList[i].transform.position = new Vector3(go_RightPlatforms_Container.transform.position.x - 0.75f, go_RightPlatforms_Container.transform.position.y + i + 1 * 2, go_RightPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 14 + (float)i;
              tempAwaiking.timerAwakingInMemory = 14 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==2)
            {
              rightPlatformsList[i].transform.position = new Vector3(go_RightPlatforms_Container.transform.position.x - 0.75f, go_RightPlatforms_Container.transform.position.y + i + 3, go_RightPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 12 + (float)i;
              tempAwaiking.timerAwakingInMemory = 12 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==3)
            {
              rightPlatformsList[i].transform.position = new Vector3(go_RightPlatforms_Container.transform.position.x - 0.75f, go_RightPlatforms_Container.transform.position.y + i + 4, go_RightPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 10 + (float)i;
              tempAwaiking.timerAwakingInMemory = 10 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==4)
            {
              rightPlatformsList[i].transform.position = new Vector3(go_RightPlatforms_Container.transform.position.x - 0.75f, go_RightPlatforms_Container.transform.position.y + i + 5, go_RightPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 8 + (float)i;
              tempAwaiking.timerAwakingInMemory = 8 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==5)
            {
              rightPlatformsList[i].transform.position = new Vector3(go_RightPlatforms_Container.transform.position.x - 0.75f, go_RightPlatforms_Container.transform.position.y + i + 6, go_RightPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 6 + (float)i;
              tempAwaiking.timerAwakingInMemory = 6 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            rightPlatformsList[i].name = "RightPlatform";
            tempObj.transform.rotation = new Quaternion(0,0,-90,90);
            tempChild.transform.rotation = new Quaternion(0,0,0,180);

        }

        //Calling BOTTOM platforms and ading them into bottom list
        for (int i = 0; i < 10; i ++)
        {
            GameObject tempObj = Instantiate(platformEdgePrefab);
            GameObject tempChild = tempObj.transform.GetChild(0).gameObject;
            bottomPlatformsList.Add(tempObj);
            bottomPlatformsList[i].transform.parent = go_BottonPlatforms_Container.transform;
            
            EdgeBoard tempAwaiking = tempObj.GetComponent<EdgeBoard>();

            if(i==chanceToHavePortalTopBottomPanel && chanceToHavePotralOnTurn > 5)
            {
              tempAwaiking.panelIsPortal = true;
            }

            if(i==0)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i + 1 * 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 26 + (float)i;
              tempAwaiking.timerAwakingInMemory = 26 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==1)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 4, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 24 + (float)i;
              tempAwaiking.timerAwakingInMemory = 24 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==2)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 22 + (float)i;
              tempAwaiking.timerAwakingInMemory = 22 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==3)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 20 + (float)i;
              tempAwaiking.timerAwakingInMemory = 20 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==4)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 18 + (float)i;
              tempAwaiking.timerAwakingInMemory = 18 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==5)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 16 + (float)i;
              tempAwaiking.timerAwakingInMemory = 16 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==6)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 14 + (float)i;
              tempAwaiking.timerAwakingInMemory = 14 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==7)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 12 + (float)i;
              tempAwaiking.timerAwakingInMemory = 12 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==8)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 10 + (float)i;
              tempAwaiking.timerAwakingInMemory = 10 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            if(i==9)
            {
              bottomPlatformsList[i].transform.position = new Vector3(go_BottonPlatforms_Container.transform.position.x + i * 2 + 2, go_BottonPlatforms_Container.transform.position.y, go_BottonPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 8 + (float)i;
              tempAwaiking.timerAwakingInMemory = 8 + (float)i;
              tempAwaiking.numberInList = i;      
            }
            bottomPlatformsList[i].name = "BottomPlatform";
            tempObj.transform.rotation = new Quaternion(0,0,180,0);
            tempChild.transform.rotation = new Quaternion(0,0,0,180);
            
        }

        //Calling LEFT platforms and ading them into bottom list
        for (int i = 0; i < 6; i ++)
        {
            GameObject tempObj = Instantiate(platformEdgePrefab);
            GameObject tempChild = tempObj.transform.GetChild(0).gameObject;
            leftPlatformsList.Add(tempObj);
            leftPlatformsList[i].transform.parent = go_LeftPlatforms_Container.transform;
            
            EdgeBoard tempAwaiking = tempObj.GetComponent<EdgeBoard>();

            if(i==chanceToHavePortalLeftRightPanel && chanceToHavePotralOnTurn > 5)
            {
              tempAwaiking.panelIsPortal = true;
            }

            if(i==0)
            {
              leftPlatformsList[i].transform.position = new Vector3(go_LeftPlatforms_Container.transform.position.x + 0.75f, go_LeftPlatforms_Container.transform.position.y + i + 0.5f * 2, go_LeftPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 28 + (float)i; 
              tempAwaiking.timerAwakingInMemory = 28 + (float)i;
              tempAwaiking.numberInList = i;     
            }
            if(i==1)
            {
              leftPlatformsList[i].transform.position = new Vector3(go_LeftPlatforms_Container.transform.position.x + 0.75f, go_LeftPlatforms_Container.transform.position.y + i + 1 * 2, go_LeftPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 28 + (float)i;
              tempAwaiking.timerAwakingInMemory = 28 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            if(i==2)
            {
              leftPlatformsList[i].transform.position = new Vector3(go_LeftPlatforms_Container.transform.position.x + 0.75f, go_LeftPlatforms_Container.transform.position.y + i + 3, go_LeftPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 28 + (float)i;
              tempAwaiking.timerAwakingInMemory = 28 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            if(i==3)
            {
              leftPlatformsList[i].transform.position = new Vector3(go_LeftPlatforms_Container.transform.position.x + 0.75f, go_LeftPlatforms_Container.transform.position.y + i + 4, go_LeftPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 28 + (float)i;
              tempAwaiking.timerAwakingInMemory = 28 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            if(i==4)
            {
              leftPlatformsList[i].transform.position = new Vector3(go_LeftPlatforms_Container.transform.position.x + 0.75f, go_LeftPlatforms_Container.transform.position.y + i + 5, go_LeftPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 28 + (float)i;
              tempAwaiking.timerAwakingInMemory = 28 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            if(i==5)
            {
              leftPlatformsList[i].transform.position = new Vector3(go_LeftPlatforms_Container.transform.position.x + 0.75f, go_LeftPlatforms_Container.transform.position.y + i + 6, go_LeftPlatforms_Container.transform.position.z);
              tempAwaiking.timerAwaking = 28 + (float)i;
              tempAwaiking.timerAwakingInMemory = 28 + (float)i;
              tempAwaiking.numberInList = i;       
            }
            leftPlatformsList[i].name = "LeftPlatform";
            tempObj.transform.rotation = new Quaternion(0,0,-180,-180);
            tempChild.transform.rotation = new Quaternion(0,0,0,180);

        }




       
        #endregion
        //////////////////////////////////////////////////////////////////////////////////////////////////

        
        ApplySetUpForFirstBubbles(); // Here we assign timer awaking for enemies bubbles ------>     
        
    }
    ///////////////////////////////////////////////////////////



    /////// SET UP TIME FOR FIRST SET OF BUBBLES APPEAR //////
    void ApplySetUpForFirstBubbles()
    {
        for(int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].GetComponent<Bubble>().timerAwaking += i;
        }    
        playerTurn = false;
        Invoke("LetPlayerMakeTurn", 4.5f); // Here we give the player first turn
      
    }
    /////////////////////////////////////////////////////////

    private void LetPlayerMakeTurn()
    {
        if(!playerTurn && playerRef.rb_player.velocity.magnitude == 0  && !touchEvent && !gameIsStarts)
        {
            playerTurn = true;
            deleteGame = false;
            playerTurnTextUI.gameObject.SetActive(true); // Here we activate player's turn text UI 
        }
    }

  //////// BUTTONS LEFT CORNER ON CANVAS ////////
  public void MuteOnOff()
  {
      if(!mute)
      {
        mute = true;
      }
      else
      {
        mute = false;
      }
      
  }

  public void FaqPanelOnOff()
  {
    if(!faqActive)
    {
      faqActive = true;
    }
    else
    {
      faqActive = false;
    }
  }

  public void CreditsOnOff()
  {
    if(!creditsActive)
    {
      creditsActive = true;
    }
    else
    {
      creditsActive = false;
    }
  }

////////////////////////////////////////////////


public void ResetPlayerRecord()
{
  PlayerPrefs.SetInt("HighScore", 0);
  PlayerPrefs.SetInt("HighBubble", 0);
  PlayerPrefs.SetInt("HighKills", 0);
}

/////////// DELETE GAME AFTER ITS OVER ///////////////
    void DeleteGame()
    {
            
            gameIsStarts = false;
            int effectsNUM = effects.Count;
        
            for (int i = 0; i < effects.Count; i++)
            {
                if(effects[i] != null)
                {
                  effects[i].SetActive(true);
                  Destroy(effects[i].gameObject);
                }
            }
                  
            for (int i = 0; i <loot.Count; i++)
            {   
              if(loot[i] != null)
              {
                loot[i].SetActive(true);
                Destroy(loot[i].gameObject);
              }

            }

            for (int i = 0; i <score.Count; i++)
            {
              if(score[i] != null)
              {
                score[i].SetActive(true);
                Destroy(score[i].gameObject);
              }
            }
            for (int i = 0; i <bubblesEnd.Count; i++)
            {   
              if(bubblesEnd[i] != null)
              {
                bubblesEnd[i].SetActive(true);
                Destroy(bubblesEnd[i].gameObject);
              }
            }
              

            effects.Clear();
            score.Clear();
            loot.Clear();
            bubblesEnd.Clear();
            topPlatformsList.Clear();
            bottomPlatformsList.Clear();
            leftPlatformsList.Clear();
            rightPlatformsList.Clear();
            progressBar.f_fill = 0;
            deleteGame = false;
    }
////////////////////////////////////////////////////// 



	  void Update () {


        if(playerRef != null)
        {
            if(playerTurn || playerRef.gameObject.activeSelf == false)
            {
                helperRef.TouchScreenEvent();
                helperRef.TrackingPlayerRecords(); 
            }

            screenWidth = mainCamera.pixelWidth;
            screenHeight = mainCamera.pixelHeight;

            if(progressBar.f_fill >= 1 && !deleteGame)
            {
                playerRef.playerBubbleLife = 0;
            }  

            //   <<<<<<<<<<<< GAME OVER MODE ACTIVATION >>>>>>>>>>>>
            //               If player lost all bubbles...
            //      
            if(playerRef.playerBubbleLife <= 0 && !deleteGame)
            {
                
                DestroyTableEdges();
                //here we can try add some animation for bubbles after player lost game
                for (float i = 0; i < bubbles.Count; i++)
                {
                    Bubble tempBubble = bubbles[(int)i].GetComponent<Bubble>();
                    tempBubble.rb_bubble.velocity = Vector3.zero;
                    
                    tempBubble.Invoke("DestroyBubble", 0.0005f + i / 10);
                }

                deleteGame = true;
                Destroy(playerRef.gameObject);
                
            }
        }

        if(bubbles.Count <=0 && deleteGame)
        {
          playerTurnTextUI.gameObject.SetActive(false);
          
          
          DeleteGame();
          GameIsOver = true;
        }


    }


    ///// SET TIMER FOR BOARD EDGES ON DESTROY //////
    void DestroyTableEdges()
{
        //Calling TOP platroms and ading them into top list
        for (int i = 0; i < topPlatformsList.Count; i ++)
        {
            EdgeBoard tempDestroy = topPlatformsList[i].GetComponent<EdgeBoard>();
            if(i==0)
            {               
                tempDestroy.timerDestroy = 1 + i;
            }
            if(i==1)
            {
                tempDestroy.timerDestroy = 1 + i;       
            }
            if(i==2)
            {
                tempDestroy.timerDestroy = 1 + i;
            }
            if(i==3)
            {
                tempDestroy.timerDestroy = 1 + i;   
            }
            if(i==4)
            {
                tempDestroy.timerDestroy = 1 + i;
            }
            if(i==5)
            {
                tempDestroy.timerDestroy = 1 + i;    
            }
            if(i==6)
            {
                tempDestroy.timerDestroy = 1 + i;      
            }
            if(i==7)
            {
                tempDestroy.timerDestroy = 1 + i;        
            }
            if(i==8)
            {
                tempDestroy.timerDestroy = 1 + i;        
            }
            if(i==9)
            {
                tempDestroy.timerDestroy = 1 + i;      
            }

        }

        //Calling RIGHT platforms and ading them into bottom list
        for (int i = 0; i < rightPlatformsList.Count; i ++)
        {
            
            EdgeBoard tempDestroy = rightPlatformsList[i].GetComponent<EdgeBoard>();

            if(i==0)
            {
                tempDestroy.timerDestroy = 16 + i;    
            }
            if(i==1)
            {
                tempDestroy.timerDestroy = 14 + i;       
            }
            if(i==2)
            {
                tempDestroy.timerDestroy = 12 + i;     
            }
            if(i==3)
            {
                tempDestroy.timerDestroy = 10 + i;      
            }
            if(i==4)
            {
                tempDestroy.timerDestroy = 8 + i;       
            }
            if(i==5)
            {
                tempDestroy.timerDestroy = 6 + i;      
            }
        }

        //Calling BOTTOM platforms and ading them into bottom list
        for (int i = 0; i < bottomPlatformsList.Count; i ++)
        {
            
            EdgeBoard tempDestroy = bottomPlatformsList[i].GetComponent<EdgeBoard>();

            if(i==0)
            {
                tempDestroy.timerDestroy = 26 + i;      
            }
            if(i==1)
            {
                tempDestroy.timerDestroy = 24 + i;       
            }
            if(i==2)
            {
                tempDestroy.timerDestroy = 22 + i;      
            }
            if(i==3)
            {
                tempDestroy.timerDestroy = 20 + i;      
            }
            if(i==4)
            {
                tempDestroy.timerDestroy = 18 + i;        
            }
            if(i==5)
            {
                tempDestroy.timerDestroy = 16 + i;       
            }
            if(i==6)
            {
                tempDestroy.timerDestroy = 14 + i;        
            }
            if(i==7)
            {
                tempDestroy.timerDestroy =  12 + i;         
            }
            if(i==8)
            {
                tempDestroy.timerDestroy = 10 + i;       
            }
            if(i==9)
            {
                tempDestroy.timerDestroy = 8 + i;        
            }
            
        }

        //Calling LEFT platforms and ading them into bottom list
        for (int i = 0; i < leftPlatformsList.Count; i ++)
        {

            
            EdgeBoard tempDestroy = leftPlatformsList[i].GetComponent<EdgeBoard>();

            if(i==0)
            {
                tempDestroy.timerDestroy = 28 + i;     
            }
            if(i==1)
            {
                tempDestroy.timerDestroy = 28 + i;         
            }
            if(i==2)
            {
                tempDestroy.timerDestroy = 28 + i;      
            }
            if(i==3)
            {
                tempDestroy.timerDestroy = 28 + i;      
            }
            if(i==4)
            {
                tempDestroy.timerDestroy = 28 + i;       
            }
            if(i==5)
            {
                tempDestroy.timerDestroy = 28 + i;         
            }

        }



}



}

