using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;
using UnityEditor.Rendering;
using TMPro;
// using System.Numerics;
using UnityEngine.Playables;
using UnityEngine.Video;
using UnityEngine.Formats.Alembic.Importer;
using Unity.Hierarchy;
using extOSC;
using Unity.VisualScripting;

public class AddBuilding : MonoBehaviour
{
    [Header("OSC Settings")]
    public string Address = "/OurCity";
    public OSCReceiver Receiver;


    public List<GameObject> buildingPrefabs;
    public List<GameObject> roofPrefabs;

    Vector3 cameraPos;
    Quaternion cameraRotation;

    public int indexOfCurrentPrompt = 0;

    public GameObject hyperrailObjects;

    public Camera mainCamera;

    public GameObject highlighter;

    public bool shouldCameraMoveUp;


    public PlayableDirector hyperrailDirector;

    public GameObject cameraAxis;

    public Camera topViewCam;

    public GameObject cameraInAxis;

    private Dialogue dialogueScript;

    public GameObject tutorialAlembic;

    public PlayableDirector directorForAlembic;

    public GameObject cinematicPlayer;

    private Vector3 targetCameraAxisPosition;


    public GameObject newHighlighter;

    public GameObject tutorialPiece1;
    public GameObject tutorialPiece2;
    public GameObject tutorialPiece3;
    public GameObject tutorialPiece4;
    public GameObject tutorialPiece5;
    public GameObject tutorialPiece6;
    public GameObject tutorialPiece7;
    public GameObject tutorialPiece8;
    public GameObject tutorialPiece9;
    public GameObject tutorialFacade1;
    public GameObject tutorialFacade2;
    public GameObject tutorialFacade3;
    public GameObject tutorialFacade4;
    public GameObject tutorialFacade5;
    public GameObject tutorialFacade6;
    public GameObject tutorialFacade7;
    public GameObject tutorialFacade8;
    public GameObject tutorialFacade9;

    public GameObject tutorialCanvas;

    public GameObject movableCamera;

    private VideoPlayer videoPlayer;

    public bool isTutorialStarted = false;
    public bool isRealGameStarted = false;

    public TMP_Text tutorialText;
    public TMP_Text tutorialText2;
    public TMP_Text tutorialText3;

    // stores all the places that a building can be spawned at
    private Dictionary<int, Vector3> possibleTilePositions = new Dictionary<int, Vector3>
    {

        // row 1
        { 0, new Vector3(23.13f, -7.1f, 39.7f) },
        { 1, new Vector3(30.95f, -7.1f, 39.7f) },
        { 2, new Vector3(38.81f, -7.1f, 39.7f) },
        { 3, new Vector3(46.6f, -7.1f, 39.7f) },
        { 4, new Vector3(54.43f, -7.1f, 39.7f) },

        // row 2
        { 5, new Vector3(19.16f, -7.1f, 32.91f) },
        { 6, new Vector3(27.02f, -7.1f, 32.91f) },
        { 7, new Vector3(34.88f, -7.1f, 32.91f) },
        { 8, new Vector3(42.74f, -7.1f, 32.91f) },
        { 9, new Vector3(50.6f, -7.1f, 32.91f) },
        { 10, new Vector3(58.46f, -7.1f, 32.91f) },

        // row 3
        { 11, new Vector3(15.36f, -7.1f, 26.11f) },
        { 12, new Vector3(23.13f, -7.1f, 26.11f) },
        { 13, new Vector3(30.95f, -7.1f, 26.11f) },
        { 14, new Vector3(38.81f, -7.1f, 26.11f) },
        { 15, new Vector3(46.6f, -7.1f, 26.11f) },
        { 16, new Vector3(54.46f, -7.1f, 26.11f) },
        { 17, new Vector3(62.25f, -7.1f, 26.11f) },

        // row 4
        { 18, new Vector3(11.43f, -7.1f, 19.36f) },
        { 19, new Vector3(19.26f, -7.1f, 19.36f) },
        { 20, new Vector3(27.1f, -7.1f, 19.36f) },
        { 21, new Vector3(34.87f, -7.1f, 19.36f) },
        { 22, new Vector3(42.71f, -7.1f, 19.36f) },
        { 23, new Vector3(50.51f, -7.1f, 19.36f) },
        { 24, new Vector3(58.35f, -7.1f, 19.36f) },
        { 25, new Vector3(66.19f, -7.1f, 19.36f) },

        // row 5
        { 26, new Vector3(7.51f, -7.1f, 12.6f) },
        { 27, new Vector3(15.36f, -7.1f, 12.6f) },
        { 28, new Vector3(23.13f, -7.1f, 12.6f) },
        { 29, new Vector3(30.95f, -7.1f, 12.6f) },
        { 30, new Vector3(38.81f, -7.1f, 12.6f) },
        { 31, new Vector3(46.6f, -7.1f, 12.6f) },
        { 32, new Vector3(54.46f, -7.1f, 12.6f) },
        { 33, new Vector3(62.25f, -7.1f, 12.6f) },
        { 34, new Vector3(70.1f, -7.1f, 12.6f) },

        // row 6
        { 35, new Vector3(11.43f, -7.1f, 5.82f) },
        { 36, new Vector3(19.26f, -7.1f, 5.82f) },
        { 37, new Vector3(27.1f, -7.1f, 5.82f) },
        { 38, new Vector3(34.87f, -7.1f, 5.82f) },
        { 39, new Vector3(42.71f, -7.1f, 5.82f) },
        { 40, new Vector3(50.51f, -7.1f, 5.82f) },
        { 41, new Vector3(58.35f, -7.1f, 5.82f) },
        { 42, new Vector3(66.19f, -7.1f, 5.82f) },

        //ow 7
        { 43, new Vector3(15.36f, -7.1f, -0.9f) },
        { 44, new Vector3(23.13f, -7.1f, -0.9f) },
        { 45, new Vector3(30.95f, -7.1f, -0.9f) },
        { 46, new Vector3(38.81f, -7.1f, -0.9f) },
        { 47, new Vector3(46.6f, -7.1f,  -0.9f) },
        { 48, new Vector3(54.46f, -7.1f, -0.9f) },
        { 49, new Vector3(62.25f, -7.1f, -0.9f) },

        //ow 8
        {  50, new Vector3(19.16f, -7.1f, -7.7f) },
        { 51, new Vector3(27.02f, -7.1f, -7.7f) },
        { 52, new Vector3(34.88f, -7.1f, -7.7f) },
        { 53, new Vector3(42.74f, -7.1f, -7.7f) },
        { 54, new Vector3(50.6f, -7.1f,  -7.7f) },
        { 55, new  Vector3(58.46f, -7.1f -7.7f) },

        //ow 9
        { 56, new Vector3(23.13f, -7.1f, -14.51f) },
        { 57, new Vector3(30.95f, -7.1f, -14.51f) },
        { 58, new Vector3(38.81f, -7.1f, -14.51f) },
        { 59, new Vector3(46.6f, -7.1f,  -14.51f) },
        { 60, new Vector3(54.43f, -7.1f, -14.51f) },
    };

    // stores the locations that a building can be inside the positions - since the axis is the middle of the piece, it can just be a rotation
    private Dictionary<int, Quaternion> possibleTileRotations = new Dictionary<int, Quaternion>
    {
        { 0, Quaternion.Euler(0, 0, 0) },
        { 1, Quaternion.Euler(0, 60, 0) },
        { 2, Quaternion.Euler(0, 120, 0) },
        { 3, Quaternion.Euler(0, 180, 0) },
        { 4, Quaternion.Euler(0, 240, 0) },
        { 5, Quaternion.Euler(0, 300, 0) }
    };

    // not using, was gonna use for moving the camera to where new buildings are added
    private Dictionary<int, Vector3> possibleCameraPositionws = new Dictionary<int, Vector3>
    {

        // row 1
        { 0, new Vector3(22, -3, 48) },
        { 1, new Vector3(31, 1, 48) },
        { 2, new Vector3(39, 1, 48) },
        { 3, new Vector3(47, 1, 48) },
        { 4, new Vector3(55, 1, 48) },

        // row 2
        { 5, new Vector3(19, 1, 41) },
        { 6, new Vector3(27, 1, 41) },
        { 7, new Vector3(35, 1, 41) },
        { 8, new Vector3(43, 1, 41) },
        { 9, new Vector3(51, 1, 41) },
        { 10, new Vector3(59,1, 41) },

        // row 3
        { 11, new Vector3(15, 1, 35f) },
        { 12, new Vector3(23, 1, 35f) },
        { 13, new Vector3(31, 1, 35f) },
        { 14, new Vector3(39, 1, 35f) },
        { 15, new Vector3(47, 1, 35f) },
        { 16, new Vector3(55, 1, 35f) },
        { 17, new Vector3(63, 1, 35f) },

        // row 4
        { 18, new Vector3(11, 1, 28) },
        { 19, new Vector3(19, 1, 28) },
        { 20, new Vector3(27, 1, 28) },
        { 21, new Vector3(35, 1, 28) },
        { 22, new Vector3(43, 1, 28) },
        { 23, new Vector3(51, 1, 28) },
        { 24, new Vector3(59, 1, 28) },
        { 25, new Vector3(67, 1, 28) },

        // row 5
        { 26, new Vector3(9, 1, 21) },
        { 27, new Vector3(15, 1, 21) },
        { 28, new Vector3(23, 1, 21) },
        { 29, new Vector3(31, 1, 21) },
        { 30, new Vector3(39, 1, 21) },
        { 31, new Vector3(47, 1, 21) },
        { 32, new Vector3(55, 1, 21) },
        { 33, new Vector3(63, 1, 21) },
        { 34, new Vector3(63, 1, 21) },
        { 35, new Vector3(63, 1, 21) },

        // row 6
        { 36, new Vector3(11, 1, 14) },
        { 37, new Vector3(19, 1, 14) },
        { 38, new Vector3(27, 1, 14) },
        { 39, new Vector3(35, 1, 14) },
        { 40, new Vector3(43, 1, 14) },
        { 41, new Vector3(51, 1, 14) },
        { 42, new Vector3(59, 1, 14) },
        { 43, new Vector3(67, 1, 14) },

        // row 7
        { 44, new Vector3(15, 1, 8f) },
        { 45, new Vector3(23, 1, 8f) },
        { 46, new Vector3(31, 1, 8f) },
        { 47, new Vector3(39, 1, 8f) },
        { 48, new Vector3(47, 1, 8f) },
        { 49, new Vector3(55, 1, 8f) },
        { 50, new Vector3(63, 1, 8f) },

        // row 8
        { 51, new Vector3(19, 1, 0.5f) },
        { 52, new Vector3(27, 1, 0.5f) },
        { 53, new Vector3(35, 1, 0.5f) },
        { 54, new Vector3(43, 1, 0.5f) },
        { 55, new Vector3(51, 1, 0.5f) },
        { 56, new Vector3(59, 1, 0.5f) },

        // row 9
        { 57, new Vector3(23, 1, -6) },
        { 58, new Vector3(31, 1, -6) },
        { 59, new Vector3(39, 1, -6) },
        { 60, new Vector3(47, 1, -6) },
        { 61, new Vector3(55, 1, -6) },
    };



    public void StartGameAction()
    {
        Debug.Log("Start Game Action Triggered");



        // directorForAlembic.Play();
        cinematicPlayer.GetComponent<VideoPlayer>().loopPointReached += OnVideoEnd;
        cinematicPlayer.GetComponent<VideoPlayer>().Play();

        isTutorialStarted = true;

        hyperrailObjects.transform.position = new Vector3(38.9f, -10.9f, 12.78f);
    }

    private IEnumerator AnimateCameraFOV(float targetFOV, float duration)
    {
        float startFOV = mainCamera.fieldOfView;
        float elapsedTime = 0f;

        float startTopFOV = 120;

        while (elapsedTime < duration)
        {
            // Calculate the normalized time (0 to 1)
            float t = elapsedTime / duration;

            // Apply an ease-out effect using Mathf.SmoothStep
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            // Interpolate the FOV using the eased time
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, easedT);
            topViewCam.fieldOfView = Mathf.Lerp(startTopFOV, 37, easedT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final FOV is set
        mainCamera.fieldOfView = targetFOV;
        topViewCam.fieldOfView = 37;
    }



    private Dictionary<int, float> possibleRowPositions = new Dictionary<int, float>
    {
        // row 1
        { 0, 39.73f},

        // row 2
        {1, 32.97f},

        // DO

        // row 3 
        {2, 26.5f},

        // row 4
        {3, 19.4f},
        
        // row 5
        {4, 12.66f},
        
        // row 6
        {5, 5.89f},
        
        // row 7
        {6, -0.9f},
        
        // row 8
        {7, -7.65f},
        
        // row 9
        {8, -14.5f},
    };

    private Dictionary<int, float> possibleHexPositionsRow1And9 = new Dictionary<int, float>
   {
        // row 1
        {0, 23.16f},
        {1, 30.89f},
        {2, 38.81f},
        {3, 46.67f},
        {4, 54.46f},
   };

    private Dictionary<int, float> possibleHexPositionsRow2And8 = new Dictionary<int, float>
   {
        {0, 19.24f},
        {1, 27.1f},
        {2, 34.83f},
        {3, 42.69f},
        {4, 50.61f},
        {5, 58.5f}
   };

    private Dictionary<int, float> possibleHexPositionsRow3And7 = new Dictionary<int, float>
   {

        // row 3
        {0, 15f},
        {1, 23f},
        {2, 31f},
        {3, 39f},
        {4, 47f},
        {5, 55f},
        {6, 63f},
   };

    private Dictionary<int, float> possibleHexPositionsRow4And6 = new Dictionary<int, float>
   {
        {0, 11f},
        {1, 19f},
        {2, 27f},
        {3, 35f},
        {4, 43f},
        {5, 51f},
        {6, 59f},
        {7, 67f},
   };

    private Dictionary<int, float> possibleHexPositionsRow5 = new Dictionary<int, float>
   {
        {0, 9f},
        {1, 15f},
        {2, 23f},
        {3, 31f},
        {4, 39f},
        {5, 47f},
        {6, 55f},
        {7, 63f},
        {8, 70f},

   };

    // for storing where a building has already been added
    private List<BuildingData> instantiatedBuildings = new List<BuildingData>();
    private List<BuildingDataFacades> instantiatedFacades = new List<BuildingDataFacades>();
    private List<RoofData> instantiatedRoofs = new List<RoofData>();

    private int potentiometerValue = 0;

    private bool isButtonOnCooldown = false; // Cooldown flag



    private int selectedRow = 0;
    private int selectedHexagon = 0;
    private int selectedFraction = 0;

    private int mode = 0;

    public TMP_Text modeText;

    private float selectedRowActualNumber = 0f;
    private float selectedHexagonActualNumber = 0f;

    private Vector3 highlighterPosition;


    SerialPort sp = new SerialPort("/dev/cu.usbserial-110", 9600);

    // Start is called before the first frame update
    void Start()
    {

        // directorForAlembic = tutorialAlembic.GetComponent<PlayableDirector>();
        tutorialAlembic.GetComponent<PlayableDirector>().Pause();
        tutorialAlembic.GetComponent<PlayableDirector>().Stop();
        tutorialAlembic.GetComponent<PlayableDirector>().time = 0;

        dialogueScript = GetComponent<Dialogue>();

        if (Receiver == null)
        {
            Debug.LogError("OSC Receiver is not assigned!");
            return;
        }

        Receiver.Bind(Address, ReceivedMessage);


        sp.Open();
        // might be too fast but it seemed to work fine for me? causes some timeout errors going this fast but slower made it really laggy waiting for the next update (should be on a different thread ig?)
        sp.ReadTimeout = 3;



        cinematicPlayer.GetComponent<VideoPlayer>().frame = 0;
        cinematicPlayer.GetComponent<VideoPlayer>().Pause();
        cinematicPlayer.GetComponent<VideoPlayer>().Prepare();


    }

    int facadeToAddNumber = 0;
    string facadeToAddString = "";
    string messageFromOSC = "";

    private void ReceivedMessage(OSCMessage message)
    {
        Debug.Log($"Received OSC message: {message}");
        messageFromOSC = message.ToString();

        if (message.ToString().Contains("ADD Building"))
        {
            string numbersString = message.Values[1].StringValue; // Get the numbers as a string
            string[] numbers = numbersString.Split(',');
            int row = int.Parse(numbers[0].Trim());
            int hex = int.Parse(numbers[1].Trim());
            int triangle = int.Parse(numbers[2].Trim());

            Debug.Log($"First Number: {row}, Second Number: {hex}, Third Number: {triangle}");

            AddBuildingToSpot(triangle, hex, row, 1);
        }
        else if (message.ToString().Contains("ADD Wall"))
        {
            string facadeToAdd = "";

            string numbersString = message.Values[2].StringValue; // Get the numbers as a string
            string[] numbers = numbersString.Split(',');

            int row = int.Parse(numbers[0].Trim());
            int hex = int.Parse(numbers[1].Trim());
            int triangle = int.Parse(numbers[2].Trim());

            Debug.Log($"First Number: {row}, Second Number: {hex}, Third Number: {triangle}");

            AddBuildingToSpot(triangle, hex, row, facadeToAddNumber);
        }

        if (message.ToString().Contains("ENDTUTORIAL"))
        {
            Destroy(tutorialText);
            Destroy(tutorialText2);
            Destroy(tutorialText3);
            dialogueScript.StartDialogue();


        }


        if (message.ToString().Contains("GODZILLA"))
        {

            Debug.Log("GODZILLA ATTACK DELETING");

            RemoveBuildingFromSpot(4, 4, 1);
            RemoveBuildingFromSpot(4, 4, 2);
            RemoveBuildingFromSpot(4, 4, 3);
            RemoveBuildingFromSpot(4, 4, 4);
            RemoveBuildingFromSpot(4, 4, 5);
            RemoveBuildingFromSpot(4, 4, 6);

        }



        if (message.ToString().Contains("REMOVE"))
        {
            string numbersString = message.Values[1].StringValue; // Get the numbers as a string
            string[] numbers = numbersString.Split(',');
            int row = int.Parse(numbers[0].Trim());
            int hex = int.Parse(numbers[1].Trim());
            int triangle = int.Parse(numbers[2].Trim());

            Debug.Log($"Removing building at Row: {row}, Hex: {hex}, Triangle: {triangle}");

            RemoveBuildingFromSpot(triangle, hex, row);
        }


        if (message.ToString().Contains("NEXTPROMPT"))
        {

            // addTheRoofs();
            indexOfCurrentPrompt += 1;

            // if hyperrail prompt completed, pop up the hyperrail
            if (indexOfCurrentPrompt == 2)
            {
                hyperrailObjects.transform.position = new Vector3(38.9f, -5.1f, 12.78f);
            }

        }

    }


    private void AddBuildingToSpot(int roationIndexFromWizard, int selectedHexFromWizard, int selectedRowFromWizard, int prefabToAdd)
    {

        // pick random tile position and rotation
        // int randomPositionIndex = UnityEngine.Random.Range(0, possibleTilePositions.Count);
        int randomRotationIndex = UnityEngine.Random.Range(0, possibleTileRotations.Count);

        // Vector3 selectedPosition = possibleTilePositions[randomPositionIndex]; this is from the old version

        selectedRowActualNumber = possibleRowPositions[selectedRowFromWizard - 1];
        int selectedRowFromWizardNormalized = selectedRowFromWizard - 1;

        if ((selectedRowFromWizardNormalized == 0) || (selectedRowFromWizardNormalized == 8))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow1And9[selectedHexFromWizard - 1];
        }

        if ((selectedRowFromWizardNormalized == 1) || (selectedRowFromWizardNormalized == 7))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow2And8[selectedHexFromWizard - 1];
        }

        if ((selectedRowFromWizardNormalized == 2) || (selectedRowFromWizardNormalized == 6))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow3And7[selectedHexFromWizard - 1];
        }

        if ((selectedRowFromWizardNormalized == 3) || (selectedRowFromWizardNormalized == 5))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow4And6[selectedHexFromWizard - 1];
        }

        if (selectedRowFromWizardNormalized == 4)
        {
            selectedHexagonActualNumber = possibleHexPositionsRow5[selectedHexFromWizard - 1];
        }

        selectedRow = selectedRowFromWizard - 1;



        if (selectedRowFromWizard == 1)
        {
            cameraRotation = Quaternion.Euler(0, -120, 0);
        }
        else if (selectedRowFromWizard == 2)
        {
            cameraRotation = Quaternion.Euler(0, -120, 0);
        }
        else if (selectedRowFromWizard == 3)
        {
            cameraRotation = Quaternion.Euler(0, -190, 0);
        }
        else if (selectedRowFromWizard == 4)
        {
            cameraRotation = Quaternion.Euler(0, -190, 0);
        }
        else if (selectedRowFromWizard == 5)
        {
            cameraRotation = Quaternion.Euler(0, -190, 0);
        }
        else if (selectedRowFromWizard == 6)
        {
            cameraRotation = Quaternion.Euler(0, -240, 0);
        }
        else if (selectedRowFromWizard == 7)
        {
            cameraRotation = Quaternion.Euler(0, -240, 0);
        }
        else if (selectedRowFromWizard == 8)
        {
            cameraRotation = Quaternion.Euler(0, -300, 0);
        }
        else if (selectedRowFromWizard == 9)
        {
            cameraRotation = Quaternion.Euler(0, -240, 0);
        }



        cameraPos = possibleCameraPositionws[selectedRowFromWizard - 1];


        Vector3 selectedPosition = new Vector3(selectedHexagonActualNumber, -7.1f, selectedRowActualNumber);

        Vector3 selectedPositionUnchanged = new Vector3(selectedHexagonActualNumber, -7.1f, selectedRowActualNumber);

        Quaternion selectedRotation = possibleTileRotations[roationIndexFromWizard - 1];

        // move the starting point down so it's below the ground
        selectedPosition.y = -10f;

        // check if a building already exists at the selected position and rotation



        if (prefabToAdd == 1)
        {

            if (!instantiatedBuildings.Exists(building => building.position == selectedPositionUnchanged && building.rotation == selectedRotation))
            {
                // select the scaffolding prefab (prefab 0)

                GameObject scaffoldingPrefab = buildingPrefabs[0];
                GameObject blankBuildingPrefab = buildingPrefabs[prefabToAdd]; //1 is the blank building

                // instantiate the scaffolding and blank building
                GameObject scaffolding = Instantiate(scaffoldingPrefab, selectedPosition, selectedRotation);
                GameObject blankBuilding = Instantiate(blankBuildingPrefab, selectedPosition, selectedRotation);

                // start raising both the scaffolding and the blank building at the same time
                StartCoroutine(RaiseBuilding(scaffolding.transform, selectedPositionUnchanged, 5.0f, () =>
                            {
                            }));

                moveCameraToLook(selectedPosition);

                StartCoroutine(RaiseBuilding(blankBuilding.transform, selectedPositionUnchanged, 2.0f, () =>
                {
                    // once the blank building is fully raised, add it to the instantiatedBuildings list
                    instantiatedBuildings.Add(new BuildingData
                    {
                        position = selectedPositionUnchanged,
                        rotation = selectedRotation,
                        buildingObject = blankBuilding
                    });

                    // move the scaffolding down after the building is raised
                    StartCoroutine(MoveScaffoldingDown(scaffolding.transform, selectedPosition - new Vector3(0, 10, 0), 5.0f, () =>
                    {
                        // destroy the scaffolding after it moves down
                        Destroy(scaffolding);
                    }));
                }));

                if (selectedRowFromWizard == 2 && selectedHexFromWizard == 3)
                {
                    if (selectedRotation == tutorialPiece1.transform.rotation)
                    {
                        tutorialPiece1.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece2.transform.rotation)
                    {
                        tutorialPiece2.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece3.transform.rotation)
                    {
                        tutorialPiece3.transform.position = new Vector3(0, -20, 0);
                    }
                    if (selectedRotation == tutorialPiece4.transform.rotation)
                    {
                        tutorialPiece4.transform.position = new Vector3(0, -20, 0);
                    }
                    if (selectedRotation == tutorialPiece5.transform.rotation)
                    {
                        tutorialPiece5.transform.position = new Vector3(0, -20, 0);
                    }
                    if (selectedRotation == tutorialPiece6.transform.rotation)
                    {
                        tutorialPiece6.transform.position = new Vector3(0, -20, 0);
                    }
                }


            }
            else
            {
                // select the scaffolding prefab (prefab 0)

                GameObject scaffoldingPrefab = buildingPrefabs[0];
                GameObject blankBuildingPrefab = buildingPrefabs[prefabToAdd]; //1 is the blank building

                selectedPositionUnchanged = new Vector3(selectedHexagonActualNumber, -3.1f, selectedRowActualNumber);

                // instantiate the scaffolding and blank building
                GameObject scaffolding = Instantiate(scaffoldingPrefab, selectedPosition, selectedRotation);
                GameObject blankBuilding = Instantiate(blankBuildingPrefab, selectedPosition, selectedRotation);

                // start raising both the scaffolding and the blank building at the same time
                StartCoroutine(RaiseBuilding(scaffolding.transform, selectedPositionUnchanged, 5.0f, () =>
                {
                }));


                StartCoroutine(RaiseBuilding(blankBuilding.transform, selectedPositionUnchanged, 2.0f, () =>
                {
                    // once the blank building is fully raised, add it to the instantiatedBuildings list
                    instantiatedFacades.Add(new BuildingDataFacades
                    {
                        position = selectedPositionUnchanged,
                        rotation = selectedRotation,
                        buildingObject = blankBuilding,
                        facadeType = facadeToAddString
                    });

                    // move the scaffolding down after the building is raised
                    StartCoroutine(MoveScaffoldingDown(scaffolding.transform, selectedPosition - new Vector3(0, 10, 0), 5.0f, () =>
                    {
                        // destroy the scaffolding after it moves down
                        Destroy(scaffolding);
                    }));
                }));


                if (selectedRowFromWizard == 2 && selectedHexFromWizard == 3)
                {
                    if (selectedRotation == tutorialPiece7.transform.rotation)
                    {
                        tutorialPiece7.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece8.transform.rotation)
                    {
                        tutorialPiece8.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece9.transform.rotation)
                    {
                        tutorialPiece9.transform.position = new Vector3(0, -20, 0);
                    }
                }

            }
        }
        else
        {

            if (!instantiatedFacades.Exists(building => building.position == selectedPositionUnchanged && building.rotation == selectedRotation))
            {
                // select the scaffolding prefab (prefab 0)

                GameObject scaffoldingPrefab = buildingPrefabs[0];

                if (messageFromOSC.ToString().Contains("Bricks"))
                {
                    facadeToAddNumber = 2;
                    facadeToAddString = "Bricks";
                }
                if (messageFromOSC.ToString().Contains("Column"))
                {
                    facadeToAddNumber = 4;
                    facadeToAddString = "Column";
                }
                if (messageFromOSC.ToString().Contains("Overhang"))
                {
                    //7 or 8
                    int randomFacadeInRange = UnityEngine.Random.Range(7, 9);
                    facadeToAddNumber = randomFacadeInRange;

                    facadeToAddString = "Overhang";
                }
                if (messageFromOSC.ToString().Contains("Garage"))
                {

                    // 11 or 12
                    int randomFacadeInRange = UnityEngine.Random.Range(11, 13);
                    facadeToAddNumber = randomFacadeInRange;
                    facadeToAddString = "Garage";
                }

                if (messageFromOSC.ToString().Contains("Hospital"))
                {

                    // 14 or 15
                    int randomFacadeInRange = UnityEngine.Random.Range(14, 16);

                    facadeToAddNumber = randomFacadeInRange;

                    facadeToAddString = "Hospital";
                }

                if (messageFromOSC.ToString().Contains("Plain"))
                {

                    facadeToAddNumber = 17;
                    facadeToAddString = "Plain";
                }
                if (messageFromOSC.ToString().Contains("Siding"))
                {

                    facadeToAddNumber = 13;
                    facadeToAddString = "Siding";
                }
                if (messageFromOSC.ToString().Contains("Vertical"))
                {

                    // 9 or 10
                    int randomFacadeInRange = UnityEngine.Random.Range(9, 11);
                    facadeToAddNumber = randomFacadeInRange;
                    facadeToAddString = "Vertical";
                }


                GameObject blankBuildingPrefab = buildingPrefabs[facadeToAddNumber]; //1 is the blank building

                // instantiate the scaffolding and blank building
                GameObject scaffolding = Instantiate(scaffoldingPrefab, selectedPosition, selectedRotation);
                GameObject blankBuilding = Instantiate(blankBuildingPrefab, selectedPosition, selectedRotation);

                // start raising both the scaffolding and the blank building at the same time
                StartCoroutine(RaiseBuilding(scaffolding.transform, selectedPositionUnchanged, 5.0f, () =>
                {
                }));


                StartCoroutine(RaiseBuilding(blankBuilding.transform, selectedPositionUnchanged, 2.0f, () =>
                {
                    // once the blank building is fully raised, add it to the instantiatedBuildings list
                    instantiatedFacades.Add(new BuildingDataFacades
                    {
                        position = selectedPositionUnchanged,
                        rotation = selectedRotation,
                        buildingObject = blankBuilding,
                        facadeType = facadeToAddString
                    });

                    // move the scaffolding down after the building is raised
                    StartCoroutine(MoveScaffoldingDown(scaffolding.transform, selectedPosition - new Vector3(0, 10, 0), 5.0f, () =>
                    {
                        // destroy the scaffolding after it moves down
                        Destroy(scaffolding);
                    }));
                }));


                if (selectedRowFromWizard == 2 && selectedHexFromWizard == 3)
                {
                    if (selectedRotation == tutorialPiece1.transform.rotation)
                    {
                        tutorialFacade1.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece2.transform.rotation)
                    {
                        tutorialFacade2.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece3.transform.rotation)
                    {
                        tutorialFacade3.transform.position = new Vector3(0, -20, 0);
                    }
                    if (selectedRotation == tutorialPiece4.transform.rotation)
                    {
                        tutorialFacade4.transform.position = new Vector3(0, -20, 0);
                    }
                    if (selectedRotation == tutorialPiece5.transform.rotation)
                    {
                        tutorialFacade5.transform.position = new Vector3(0, -20, 0);
                    }
                    if (selectedRotation == tutorialPiece6.transform.rotation)
                    {
                        tutorialFacade6.transform.position = new Vector3(0, -20, 0);
                    }
                }

            }
            else
            {
                // select the scaffolding prefab (prefab 0)

                if (messageFromOSC.ToString().Contains("Bricks"))
                {
                    facadeToAddNumber = 3;
                    facadeToAddString = "Bricks";
                }
                if (messageFromOSC.ToString().Contains("Column"))
                {
                    int randomFacadeInRange = UnityEngine.Random.Range(5, 7);
                    facadeToAddNumber = randomFacadeInRange;
                    facadeToAddString = "Column";
                }
                if (messageFromOSC.ToString().Contains("Overhang"))
                {

                    int randomFacadeInRange = UnityEngine.Random.Range(9, 11);
                    facadeToAddNumber = randomFacadeInRange;
                    facadeToAddString = "Overhang";
                }
                if (messageFromOSC.ToString().Contains("Garage"))
                {

                    facadeToAddNumber = 11;
                    facadeToAddString = "Garage";
                }

                if (messageFromOSC.ToString().Contains("Hospital"))
                {

                    facadeToAddNumber = 16;
                    facadeToAddString = "Hospital";
                }

                if (messageFromOSC.ToString().Contains("Plain"))
                {

                    facadeToAddNumber = 17;
                    facadeToAddString = "Plain";
                }
                if (messageFromOSC.ToString().Contains("Siding"))
                {

                    facadeToAddNumber = 18;
                    facadeToAddString = "Siding";
                }
                if (messageFromOSC.ToString().Contains("Vertical"))
                {
                    int randomFacadeInRange = UnityEngine.Random.Range(9, 11);

                    facadeToAddNumber = randomFacadeInRange;
                    facadeToAddString = "Vertical";
                }


                GameObject scaffoldingPrefab = buildingPrefabs[0];
                GameObject blankBuildingPrefab = buildingPrefabs[facadeToAddNumber]; //1 is the blank building

                selectedPositionUnchanged = new Vector3(selectedHexagonActualNumber, -3.1f, selectedRowActualNumber);

                // instantiate the scaffolding and blank building
                GameObject scaffolding = Instantiate(scaffoldingPrefab, selectedPosition, selectedRotation);
                GameObject blankBuilding = Instantiate(blankBuildingPrefab, selectedPosition, selectedRotation);

                // start raising both the scaffolding and the blank building at the same time
                StartCoroutine(RaiseBuilding(scaffolding.transform, selectedPositionUnchanged, 5.0f, () =>
                {
                }));


                StartCoroutine(RaiseBuilding(blankBuilding.transform, selectedPositionUnchanged, 2.0f, () =>
                {
                    // once the blank building is fully raised, add it to the instantiatedBuildings list
                    instantiatedFacades.Add(new BuildingDataFacades
                    {
                        position = selectedPositionUnchanged,
                        rotation = selectedRotation,
                        buildingObject = blankBuilding
                    });

                    // move the scaffolding down after the building is raised
                    StartCoroutine(MoveScaffoldingDown(scaffolding.transform, selectedPosition - new Vector3(0, 10, 0), 5.0f, () =>
                    {
                        // destroy the scaffolding after it moves down
                        Destroy(scaffolding);
                    }));
                }));


                if (selectedRowFromWizard == 2 && selectedHexFromWizard == 3)
                {
                    if (selectedRotation == tutorialPiece7.transform.rotation)
                    {

                        tutorialFacade7.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece8.transform.rotation)
                    {
                        tutorialFacade8.transform.position = new Vector3(0, -20, 0);
                    }

                    if (selectedRotation == tutorialPiece9.transform.rotation)
                    {
                        tutorialFacade9.transform.position = new Vector3(0, -20, 0);
                    }
                }

            }
        }
    }

    private void RemoveBuildingFromSpot(int rotationIndexFromWizard, int selectedHexFromWizard, int selectedRowFromWizard)
    {

        selectedRowActualNumber = possibleRowPositions[selectedRowFromWizard - 1];
        int selectedRowFromWizardNormalized = selectedRowFromWizard - 1;

        if ((selectedRowFromWizardNormalized == 0) || (selectedRowFromWizardNormalized == 8))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow1And9[selectedHexFromWizard - 1];
        }
        else if ((selectedRowFromWizardNormalized == 1) || (selectedRowFromWizardNormalized == 7))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow2And8[selectedHexFromWizard - 1];
        }
        else if ((selectedRowFromWizardNormalized == 2) || (selectedRowFromWizardNormalized == 6))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow3And7[selectedHexFromWizard - 1];
        }
        else if ((selectedRowFromWizardNormalized == 3) || (selectedRowFromWizardNormalized == 5))
        {
            selectedHexagonActualNumber = possibleHexPositionsRow4And6[selectedHexFromWizard - 1];
        }
        else if (selectedRowFromWizardNormalized == 4)
        {
            selectedHexagonActualNumber = possibleHexPositionsRow5[selectedHexFromWizard - 1];
        }

        Vector3 basePosition = new Vector3(selectedHexagonActualNumber, -7.1f, selectedRowActualNumber);
        Quaternion selectedRotation = possibleTileRotations[rotationIndexFromWizard - 1];


        Vector3 higherPosition = new Vector3(basePosition.x, -3.1f, basePosition.z);
        BuildingData? higherBuilding = instantiatedBuildings.Find(building =>
            building.position == higherPosition && building.rotation == selectedRotation);

        if (higherBuilding.HasValue)
        {

            Destroy(higherBuilding.Value.buildingObject);
            instantiatedBuildings.Remove(higherBuilding.Value);

        }


        BuildingData? baseBuilding = instantiatedBuildings.Find(building =>
            building.position == basePosition && building.rotation == selectedRotation);

        if (baseBuilding.HasValue)
        {
            Destroy(baseBuilding.Value.buildingObject);
            instantiatedBuildings.Remove(baseBuilding.Value);

        }
        else
        {
        }


        BuildingDataFacades? facade = instantiatedFacades.Find(f =>
            f.position == basePosition && f.rotation == selectedRotation);

        if (facade.HasValue)
        {
            Destroy(facade.Value.buildingObject);
            instantiatedFacades.Remove(facade.Value);

        }
    }

    // private void addTheRoofs()
    // {
    //     Debug.Log("ROOFS");
    //     Debug.Log(instantiatedFacades[instantiatedFacades.Count - 1].facadeType);

    //     // Group facades by position
    //     var facadesGroupedByPosition = new Dictionary<Vector3, List<BuildingDataFacades>>();

    //     foreach (BuildingDataFacades facade in instantiatedFacades)
    //     {
    //         if (!facadesGroupedByPosition.ContainsKey(facade.position))
    //         {
    //             facadesGroupedByPosition[facade.position] = new List<BuildingDataFacades>();
    //         }
    //         facadesGroupedByPosition[facade.position].Add(facade);
    //     }

    //     // Loop over each group of facades
    //     foreach (var group in facadesGroupedByPosition)
    //     {
    //         Vector3 position = group.Key;
    //         List<BuildingDataFacades> facadesAtPosition = group.Value;

    //         // Determine how many roofs to add based on the number of facades
    //         float roofHeight = facadesAtPosition.Count == 1 ? -3.1f : -0.9f;

    //         foreach (BuildingDataFacades facade in facadesAtPosition)
    //         {
    //             GameObject roofPrefab = null;

    //             // Check the facade type and assign the appropriate roof prefab
    //             if (facade.facadeType == "Bricks")
    //             {
    //                 roofPrefab = roofPrefabs[0];
    //             }
    //             else if (facade.facadeType == "Column")
    //             {
    //                 roofPrefab = roofPrefabs[1];
    //             }
    //             else if (facade.facadeType == "Overhang")
    //             {
    //                 roofPrefab = roofPrefabs[2];
    //             }
    //             else if (facade.facadeType == "Garage")
    //             {
    //                 roofPrefab = roofPrefabs[3];
    //             }
    //             else if (facade.facadeType == "Hospital")
    //             {
    //                 roofPrefab = roofPrefabs[4];
    //             }
    //             else if (facade.facadeType == "Plain")
    //             {
    //                 roofPrefab = roofPrefabs[5];
    //             }
    //             else if (facade.facadeType == "Siding")
    //             {
    //                 roofPrefab = roofPrefabs[6];
    //             }
    //             else if (facade.facadeType == "Vertical")
    //             {
    //                 roofPrefab = roofPrefabs[7];
    //             }

    //             if (roofPrefab != null)
    //             {

    //                 if (!instantiatedRoofs.Exists(roof => roof.position == new Vector3(facade.position.x, roofHeight, facade.position.z) && roof.rotation == facade.rotation))
    //                 {
    //                     // Instantiate the roof prefab at the correct position and height
    //                     GameObject roof = Instantiate(roofPrefab, facade.position, facade.rotation);
    //                     roof.transform.position = new Vector3(facade.position.x, roofHeight, facade.position.z);
    //                     roof.transform.rotation = facade.rotation;

    //                     // Add the roof to the instantiatedRoofs list
    //                     instantiatedRoofs.Add(new RoofData
    //                     {
    //                         position = roof.transform.position,
    //                         rotation = roof.transform.rotation,
    //                         facadeType = facade.facadeType
    //                     });
    //                 }
    //                 else
    //                 {
    //                     Debug.Log($"Roof already exists at position {facade.position} with rotation {facade.rotation}");
    //                 }
    //             }
    //         }
    //     }
    // }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Destroy the video player's GameObjects and UI canvas



        GameObject[] thingsToDestroy = GameObject.FindGameObjectsWithTag("Cinematic");

        foreach (GameObject cinematicThing in thingsToDestroy)
        {
            Destroy(cinematicThing);
        }




        // Start the coroutine for the camera FOV
        StartCoroutine(AnimateCameraFOV(44f, 2.0f));


        tutorialAlembic.GetComponent<AlembicStreamPlayer>().EndTime = 23.7f;
        // tutorialAlembic.GetComponent<AlembicStreamPlayer>().CurrentTime = 0;

        tutorialAlembic.GetComponent<PlayableDirector>().Play();
        // move tutorial text to y 150 in a coroutine


        StartCoroutine(moveTutorialTextUp());
        StartCoroutine(moveTutorialAlembicDown());

    }

    private void moveCameraToLook(Vector3 selectedPosition)
    {

        Vector3 lookAtVector = new Vector3(selectedPosition.x, selectedPosition.y - 5, selectedPosition.z);
        movableCamera.transform.LookAt(lookAtVector);

        StartCoroutine(moveCameraToLookSmoothly(selectedPosition));



    }

    private IEnumerator moveCameraToLookSmoothly(Vector3 selectedPosition)
    {
        Vector3 targetPosition = new Vector3(selectedPosition.x, 1, selectedPosition.z);
        Vector3 highlighterTarget = new Vector3(selectedPosition.x, -11.19f, selectedPosition.z);

        float speed = 40f; // Adjust the speed as needed
        while (newHighlighter.transform.position != highlighterTarget)
        {
            movableCamera.transform.position = Vector3.MoveTowards(movableCamera.transform.position, targetPosition, speed * Time.deltaTime);

            newHighlighter.transform.position = Vector3.MoveTowards(newHighlighter.transform.position, highlighterTarget, speed * Time.deltaTime);
            // speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }

        movableCamera.transform.position = targetPosition;
        newHighlighter.transform.position = highlighterTarget;
    }
    // this but in a coroutine so it moves movableCamera.transform.LookAt(selectedPosition);


    private IEnumerator moveTutorialAlembicDown()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 targetPosition = new Vector3(tutorialAlembic.transform.position.x, -22.4f, tutorialAlembic.transform.position.z);
        float speed = 1600f; // Adjust the speed as needed
        while (tutorialAlembic.transform.position != (Vector3)targetPosition)
        {
            tutorialAlembic.transform.position = Vector3.MoveTowards(tutorialAlembic.transform.position, targetPosition, speed * Time.deltaTime);
            speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }
    }
    private IEnumerator moveTutorialTextUp()
    {
        yield return new WaitForSeconds(1.5f);
        Vector2 targetPosition = new Vector2(tutorialText.transform.position.x, 150);
        float speed = 1600f; // Adjust the speed as needed
        while (tutorialText.transform.position != (Vector3)targetPosition)
        {
            tutorialText.transform.position = Vector2.MoveTowards(tutorialText.transform.position, targetPosition, speed * Time.deltaTime);
            speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }
        // wait a second
        yield return new WaitForSeconds(13f);
        // now do the same thing for tutorial text 2
        Vector2 targetPosition2 = new Vector2(tutorialText2.transform.position.x, 150);

        Vector2 targetPosition1New = new Vector2(tutorialText.transform.position.x, 300);

        speed = 1600f; // Adjust the speed as needed
        while (tutorialText2.transform.position != (Vector3)targetPosition2)
        {
            tutorialText2.transform.position = Vector2.MoveTowards(tutorialText2.transform.position, targetPosition2, speed * Time.deltaTime);
            tutorialText.transform.position = Vector2.MoveTowards(tutorialText.transform.position, targetPosition1New, speed * Time.deltaTime);
            speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }

        yield return new WaitForSeconds(10f);

        Vector2 targetPosition2New = new Vector2(tutorialText2.transform.position.x, 1300);
        Vector2 targetPosition1New2 = new Vector2(tutorialText.transform.position.x, 1600);
        Vector2 targetPosition3 = new Vector2(tutorialText.transform.position.x, 150);

        speed = 1600f; // Adjust the speed as needed
        while (tutorialText2.transform.position != (Vector3)targetPosition2)
        {
            tutorialText2.transform.position = Vector2.MoveTowards(tutorialText2.transform.position, targetPosition2New, speed * Time.deltaTime);
            tutorialText.transform.position = Vector2.MoveTowards(tutorialText.transform.position, targetPosition1New2, speed * Time.deltaTime);
            tutorialText3.transform.position = Vector2.MoveTowards(tutorialText.transform.position, targetPosition3, speed * Time.deltaTime);
            speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }
        yield return new WaitForSeconds(5f);

        while (tutorialText2.transform.position != (Vector3)targetPosition2)
        {
            tutorialText3.transform.position = Vector2.MoveTowards(tutorialText.transform.position, targetPosition2New, speed * Time.deltaTime);
            speed = Mathf.Max(speed - 40f, 50f);
            yield return null;
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cinematicPlayer.GetComponent<VideoPlayer>().frame = (long)cinematicPlayer.GetComponent<VideoPlayer>().frameCount - 24;
            cinematicPlayer.GetComponent<VideoPlayer>().Play();
        }

        if (sp.IsOpen)
        {
            try
            {

                string data = sp.ReadLine().Trim();
                Debug.Log($"Raw data: {data}");

                if (data.StartsWith("VALUE:"))
                {
                    potentiometerValue = int.Parse(data.Substring(6));
                    Debug.Log($"Potentiometer value: {potentiometerValue}");
                }
                else if (data.StartsWith("BUTTON:"))
                {
                    int button = int.Parse(data.Substring(7));
                    Debug.Log($"Button pressed: " + button);
                    if (button == 1)
                    {

                        if (!isButtonOnCooldown)
                        {
                            // Handle button 1

                            if (mode == 0)
                            {
                                mode = 1;
                                modeText.text = "Pick which spot";
                            }
                            else if (mode == 1)
                            {
                                mode = 2;
                                modeText.text = "Press the red button to add a building";
                            }
                            else if (mode == 2)
                            {
                                mode = 0;
                                modeText.text = "Pick which row to build in";
                            }

                            Debug.Log("MODE = " + mode);

                            StartCoroutine(ButtonCooldown());
                        }
                    }
                    else if (button == 2)
                    {
                        if (mode == 2)
                        {
                            // pick random tile position and rotation
                            // int randomPositionIndex = UnityEngine.Random.Range(0, possibleTilePositions.Count);
                            int randomRotationIndex = UnityEngine.Random.Range(0, possibleTileRotations.Count);

                            // Vector3 selectedPosition = possibleTilePositions[randomPositionIndex]; this is from the old version
                            Vector3 selectedPosition = new Vector3(selectedHexagonActualNumber, -7.1f, selectedRowActualNumber);

                            Vector3 selectedPositionUnchanged = new Vector3(selectedHexagonActualNumber, -7.1f, selectedRowActualNumber);

                            Quaternion selectedRotation = possibleTileRotations[randomRotationIndex];

                            // move the starting point down so it's below the ground
                            selectedPosition.y = -10f;

                            // check if a building already exists at the selected position and rotation
                            if (!instantiatedBuildings.Exists(building => building.position == selectedPositionUnchanged && building.rotation == selectedRotation))
                            {
                                // select the scaffolding prefab (prefab 0)
                                GameObject scaffoldingPrefab = buildingPrefabs[0];
                                GameObject blankBuildingPrefab = buildingPrefabs[1];

                                // instantiate the scaffolding and blank building
                                GameObject scaffolding = Instantiate(scaffoldingPrefab, selectedPosition, selectedRotation);
                                GameObject blankBuilding = Instantiate(blankBuildingPrefab, selectedPosition, selectedRotation);

                                // start raising both the scaffolding and the blank building at the same time
                                StartCoroutine(RaiseBuilding(scaffolding.transform, selectedPositionUnchanged, 5.0f, () =>
                                {
                                }));


                                StartCoroutine(RaiseBuilding(blankBuilding.transform, selectedPositionUnchanged, 2.0f, () =>
                                {
                                    // once the blank building is fully raised, add it to the instantiatedBuildings list
                                    instantiatedBuildings.Add(new BuildingData
                                    {
                                        position = selectedPositionUnchanged,
                                        rotation = selectedRotation,
                                        buildingObject = blankBuilding
                                    });

                                    // move the scaffolding down after the building is raised
                                    StartCoroutine(MoveScaffoldingDown(scaffolding.transform, selectedPosition - new Vector3(0, 10, 0), 5.0f, () =>
                                    {
                                        // destroy the scaffolding after it moves down
                                        Destroy(scaffolding);
                                    }));
                                }));


                            }
                            else
                            {
                                return;
                            }
                        }
                    }

                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    potentiometerValue = Mathf.Max(0, potentiometerValue - 10); // Decrease value
                    Debug.Log($"Potentiometer value (decreased): {potentiometerValue}");
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    potentiometerValue = Mathf.Min(1023, potentiometerValue + 10); // Increase value
                    Debug.Log($"Potentiometer value (increased): {potentiometerValue}");
                }



                if (mode == 0)
                {
                    if (potentiometerValue < 113)
                    {
                        selectedRow = 0;
                    }
                    else if (potentiometerValue < 226)
                    {
                        selectedRow = 1;
                    }
                    else if (potentiometerValue < 339)
                    {
                        selectedRow = 2;
                    }
                    else if (potentiometerValue < 452)
                    {
                        selectedRow = 3;
                    }
                    else if (potentiometerValue < 565)
                    {
                        selectedRow = 4;
                    }
                    else if (potentiometerValue < 678)
                    {
                        selectedRow = 5;
                    }
                    else if (potentiometerValue < 791)
                    {
                        selectedRow = 6;
                    }
                    else if (potentiometerValue < 904)
                    {
                        selectedRow = 7;
                    }
                    else if (potentiometerValue < 1023)
                    {
                        selectedRow = 8;
                    }

                    selectedRowActualNumber = possibleRowPositions[selectedRow];

                    // Debug.Log("SELECTED ROW VALUE = " + selectedRowActualNumber);

                    highlighterPosition = new Vector3(38.8f, -11.4f, selectedRowActualNumber);
                    highlighter.transform.position = highlighterPosition;
                }

                if (mode == 1)
                {
                    if (selectedRow == 0 || selectedRow == 8)
                    {
                        // 5 possible hexagons
                        if (potentiometerValue < 204)
                        {
                            selectedHexagon = 0;
                        }
                        else if (potentiometerValue < 408)
                        {
                            selectedHexagon = 1;
                        }
                        else if (potentiometerValue < 612)
                        {
                            selectedHexagon = 2;
                        }
                        else if (potentiometerValue < 816)
                        {
                            selectedHexagon = 3;
                        }
                        else if (potentiometerValue < 1023)
                        {
                            selectedHexagon = 4;
                        }
                    }
                    else if (selectedRow == 1 || selectedRow == 7)
                    {
                        // 6 possible hexagons
                        if (potentiometerValue < 170)
                        {
                            selectedHexagon = 0;
                        }
                        else if (potentiometerValue < 340)
                        {
                            selectedHexagon = 1;
                        }
                        else if (potentiometerValue < 510)
                        {
                            selectedHexagon = 2;
                        }
                        else if (potentiometerValue < 680)
                        {
                            selectedHexagon = 3;
                        }
                        else if (potentiometerValue < 850)
                        {
                            selectedHexagon = 4;
                        }
                        else if (potentiometerValue < 1023)
                        {
                            selectedHexagon = 5;
                        }
                    }
                    else if (selectedRow == 2 || selectedRow == 6)
                    {
                        // 7 possible hexagons
                        if (potentiometerValue < 146)
                        {
                            selectedHexagon = 0;
                        }
                        else if (potentiometerValue < 292)
                        {
                            selectedHexagon = 1;
                        }
                        else if (potentiometerValue < 438)
                        {
                            selectedHexagon = 2;
                        }
                        else if (potentiometerValue < 584)
                        {
                            selectedHexagon = 3;
                        }
                        else if (potentiometerValue < 730)
                        {
                            selectedHexagon = 4;
                        }
                        else if (potentiometerValue < 876)
                        {
                            selectedHexagon = 5;
                        }
                        else if (potentiometerValue < 1023)
                        {
                            selectedHexagon = 6;
                        }
                    }
                    else if (selectedRow == 3 || selectedRow == 5)
                    {
                        // 8 possible hexagons
                        if (potentiometerValue < 128)
                        {
                            selectedHexagon = 0;
                        }
                        else if (potentiometerValue < 256)
                        {
                            selectedHexagon = 1;
                        }
                        else if (potentiometerValue < 384)
                        {
                            selectedHexagon = 2;
                        }
                        else if (potentiometerValue < 512)
                        {
                            selectedHexagon = 3;
                        }
                        else if (potentiometerValue < 640)
                        {
                            selectedHexagon = 4;
                        }
                        else if (potentiometerValue < 768)
                        {
                            selectedHexagon = 5;
                        }
                        else if (potentiometerValue < 896)
                        {
                            selectedHexagon = 6;
                        }
                        else if (potentiometerValue < 1023)
                        {
                            selectedHexagon = 7;
                        }
                    }
                    else if (selectedRow == 4)
                    {
                        // 9 possible hexagons
                        if (potentiometerValue < 113)
                        {
                            selectedHexagon = 0;
                        }
                        else if (potentiometerValue < 226)
                        {
                            selectedHexagon = 1;
                        }
                        else if (potentiometerValue < 339)
                        {
                            selectedHexagon = 2;
                        }
                        else if (potentiometerValue < 452)
                        {
                            selectedHexagon = 3;
                        }
                        else if (potentiometerValue < 565)
                        {
                            selectedHexagon = 4;
                        }
                        else if (potentiometerValue < 678)
                        {
                            selectedHexagon = 5;
                        }
                        else if (potentiometerValue < 791)
                        {
                            selectedHexagon = 6;
                        }
                        else if (potentiometerValue < 904)
                        {
                            selectedHexagon = 7;
                        }
                        else if (potentiometerValue < 1023)
                        {
                            selectedHexagon = 8;
                        }
                    }
                }

                if (mode == 2)
                {
                    // Divide potentiometer by 6 because there are 6 possible fractions
                    if (potentiometerValue >= 0 && potentiometerValue < 170)
                    {
                        selectedFraction = 1;
                    }
                    else if (potentiometerValue >= 170 && potentiometerValue < 340)
                    {
                        selectedFraction = 2;
                    }
                    else if (potentiometerValue >= 340 && potentiometerValue < 510)
                    {
                        selectedFraction = 3;
                    }
                    else if (potentiometerValue >= 510 && potentiometerValue < 680)
                    {
                        selectedFraction = 4;
                    }
                    else if (potentiometerValue >= 680 && potentiometerValue < 850)
                    {
                        selectedFraction = 5;
                    }
                    else if (potentiometerValue >= 850 && potentiometerValue <= 1023)
                    {
                        selectedFraction = 6;
                    }

                    highlighterPosition = new Vector3(selectedHexagonActualNumber, -11.4f, selectedRowActualNumber);
                    highlighter.transform.position = highlighterPosition;
                }


                if (mode == 1)
                {

                    if ((selectedRow == 0) || (selectedRow == 8))
                    {
                        selectedHexagonActualNumber = possibleHexPositionsRow1And9[selectedHexagon];
                    }

                    if ((selectedRow == 1) || (selectedRow == 7))
                    {
                        selectedHexagonActualNumber = possibleHexPositionsRow2And8[selectedHexagon];
                    }

                    if ((selectedRow == 2) || (selectedRow == 6))
                    {
                        selectedHexagonActualNumber = possibleHexPositionsRow3And7[selectedHexagon];
                    }

                    if ((selectedRow == 3) || (selectedRow == 5))
                    {
                        selectedHexagonActualNumber = possibleHexPositionsRow4And6[selectedHexagon];
                    }

                    if (selectedRow == 4)
                    {
                        selectedHexagonActualNumber = possibleHexPositionsRow5[selectedHexagon];
                    }


                    // Debug.Log("BLOCK" + selectedHexagonActualNumber);

                    highlighterPosition = new Vector3(selectedHexagonActualNumber, -11.4f, selectedRowActualNumber);
                    highlighter.transform.position = highlighterPosition;
                }








                // // this is only for testing, we wont do it this way
                // if (sp.ReadByte() == 2)
                // {
                //     // pick a random tile position
                //     int randomBuildingIndex = UnityEngine.Random.Range(0, possibleTilePositions.Count);
                //     Vector3 selectedPosition = possibleTilePositions[randomBuildingIndex];

                //     // find all buildings at the selected position and store them
                //     var buildingsToReplace = instantiatedBuildings.FindAll(building => building.position == selectedPosition);

                //     // select a random prefab to use for all buildings at this position
                //     int randomPrefabIndex = UnityEngine.Random.Range(2, buildingPrefabs.Count);
                //     GameObject selectedPrefab = buildingPrefabs[randomPrefabIndex];

                //     // repeplace each building with the selected prefab
                //     foreach (var buildingData in buildingsToReplace)
                //     {
                //         // destroy the old building directly using the stored reference
                //         if (buildingData.buildingObject != null)
                //         {
                //             Destroy(buildingData.buildingObject);
                //         }

                //         // instantiate the selected prefab
                //         GameObject newBuilding = Instantiate(selectedPrefab, buildingData.position, buildingData.rotation);

                //         // update the instantiatedBuildings list
                //         instantiatedBuildings.Remove(buildingData);
                //         instantiatedBuildings.Add(new BuildingData
                //         {
                //             position = buildingData.position,
                //             rotation = buildingData.rotation,
                //             buildingObject = newBuilding // store the reference to the new building
                //         });
                //     }
                // }

            }
            catch (TimeoutException)
            {
                // Log a warning for timeout, but don't treat it as a critical error
                // Debug.LogWarning("Serial port read timed out.");
            }
            catch (System.Exception ex)
            {
                // Log other exceptions as errors
                Debug.LogError($"An error occurred: {ex.Message}");
            }
        }


        // move cameraaxisto the newHighlighter position
        // if at the outer hexagons, go higher otherwise stay at 9
        if (indexOfCurrentPrompt < 6)
        {
            if (selectedRow == 0 || selectedRow == 8)
            {
                targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 16, newHighlighter.transform.position.z);

            }
            else
            {
                if (selectedRow == 1 || selectedRow == 7)
                {
                    if (selectedHexagon == 0 || selectedHexagon == 5)
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 16, newHighlighter.transform.position.z);
                    }
                    else
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 5, newHighlighter.transform.position.z);
                    }
                }
                else if (selectedRow == 2 || selectedRow == 6)
                {
                    if (selectedHexagon == 0 || selectedHexagon == 6)
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 16, newHighlighter.transform.position.z);
                    }
                    else
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 5, newHighlighter.transform.position.z);
                    }
                }
                else if (selectedRow == 3 || selectedRow == 5)
                {
                    if (selectedHexagon == 0 || selectedHexagon == 7)
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 16, newHighlighter.transform.position.z);
                    }
                    else
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 5, newHighlighter.transform.position.z);
                    }
                }
                else if (selectedRow == 4)
                {
                    if (selectedHexagon == 0 || selectedHexagon == 8)
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 16, newHighlighter.transform.position.z);
                    }
                    else
                    {
                        targetCameraAxisPosition = new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 5, newHighlighter.transform.position.z);
                    }
                }
            }

        }
        else
        {
            targetCameraAxisPosition = new Vector3(31, 2, 42);
        }




        cameraInAxis.transform.localPosition = new Vector3(0, 1, 15);
        if (indexOfCurrentPrompt < 6)
        {
            cameraInAxis.transform.LookAt(new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 7, newHighlighter.transform.position.z));
        }
        else {
             cameraInAxis.transform.LookAt(new Vector3(31,-4,42));
        }


        float axisSpeed = 15f; // Adjust speed as needed
        cameraAxis.transform.position = Vector3.Lerp(cameraAxis.transform.position, targetCameraAxisPosition, axisSpeed * Time.deltaTime);


        // mainCamera.transform.position = Vector3.MoveTowards(cameraInAxis.transform.position, cameraPos, 0.5f);

        // always set cameraaxis's rotation to cameraRotation


        // cameraAxis.transform.LookAt(new Vector3(newHighlighter.transform.position.x, newHighlighter.transform.position.y + 20, newHighlighter.transform.position.z));

        //     if (shouldCameraMoveUp == true)
        //     {

        //         // move cameraInAxis to -5.5,-10.47,31.8
        //         cameraInAxis.transform.position = Vector3.MoveTowards(cameraInAxis.transform.position, new Vector3(-5.5f, -10.47f, 31.8f), 0.5f);
        //     }
        //     else
        //     {
        //         cameraInAxis.transform.position = Vector3.MoveTowards(cameraInAxis.transform.position, new Vector3(-5.5f, 2.4f, 51f), 0.5f);
        //     }

        StartCoroutine(alwaysSpinCamera());
    }





    private IEnumerator alwaysSpinCamera()
    {

        cameraAxis.transform.Rotate(Vector3.up * Time.deltaTime * 2f); // Adjust speed as needed
        yield return null; // Wait for the next frame

    }

    private IEnumerator MoveToTarget(Transform objectTransform, Vector3 targetPosition)
    {
        while (objectTransform.position != targetPosition)
        {
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, targetPosition, 10.0f * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator RaiseBuilding(Transform objectTransform, Vector3 targetPosition, float speed, System.Action onComplete)
    {
        while (objectTransform.position != targetPosition)
        {
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the exact target position
        objectTransform.position = targetPosition;

        // Invoke the callback if provided
        onComplete?.Invoke();
    }

    private IEnumerator MoveScaffoldingDown(Transform objectTransform, Vector3 targetPosition, float speed, System.Action onComplete)
    {
        while (objectTransform.position != targetPosition)
        {
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the object reaches the exact target position
        objectTransform.position = targetPosition;

        // Invoke the callback if provided
        onComplete?.Invoke();
    }

    private IEnumerator ButtonCooldown()
    {
        isButtonOnCooldown = true; // Activate cooldown
        yield return new WaitForSeconds(1.0f); // Cooldown duration (1 second)
        isButtonOnCooldown = false; // Deactivate cooldown
    }





    private struct BuildingData
    {
        public Vector3 position;
        public Quaternion rotation;

        public GameObject buildingObject;
    }

    private struct BuildingDataFacades
    {
        public Vector3 position;
        public Quaternion rotation;

        public GameObject buildingObject;
        public string facadeType;
    }

    private struct RoofData
    {
        public Vector3 position;
        public Quaternion rotation;
        public string facadeType;
    }
}
