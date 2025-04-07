using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System;
using UnityEditor.Rendering;
using TMPro;
// using System.Numerics;

public class AddBuilding : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;

    public Camera mainCamera;

    public GameObject highlighter;

    public GameObject cameraAxis;


    // stores all the places that a building can be spawned at
    private Dictionary<int, Vector3> possibleTilePositions = new Dictionary<int, Vector3>
    {

        // row 1
        { 0, new Vector3(23, -5.86f, 40) },
        { 1, new Vector3(31, -5.86f, 40) },
        { 2, new Vector3(39, -5.86f, 40) },
        { 3, new Vector3(47, -5.86f, 40) },
        { 4, new Vector3(55, -5.86f, 40) },

        // row 2
        { 5, new Vector3(19, -5.86f, 33) },
        { 6, new Vector3(27, -5.86f, 33) },
        { 7, new Vector3(35, -5.86f, 33) },
        { 8, new Vector3(43, -5.86f, 33) },
        { 9, new Vector3(51, -5.86f, 33) },
        { 10, new Vector3(59, -5.86f, 33) },

        // row 3
        { 11, new Vector3(15, -5.86f, 26.5f) },
        { 12, new Vector3(23, -5.86f, 26.5f) },
        { 13, new Vector3(31, -5.86f, 26.5f) },
        { 14, new Vector3(39, -5.86f, 26.5f) },
        { 15, new Vector3(47, -5.86f, 26.5f) },
        { 16, new Vector3(55, -5.86f, 26.5f) },
        { 17, new Vector3(63, -5.86f, 26.5f) },

        // row 4
        { 18, new Vector3(11, -5.86f, 20) },
        { 19, new Vector3(19, -5.86f, 20) },
        { 20, new Vector3(27, -5.86f, 20) },
        { 21, new Vector3(35, -5.86f, 20) },
        { 22, new Vector3(43, -5.86f, 20) },
        { 23, new Vector3(51, -5.86f, 20) },
        { 24, new Vector3(59, -5.86f, 20) },
        { 25, new Vector3(67, -5.86f, 20) },

        // row 5
        { 26, new Vector3(9, -5.86f, 13) },
        { 27, new Vector3(15, -5.86f, 13) },
        { 28, new Vector3(23, -5.86f, 13) },
        { 29, new Vector3(31, -5.86f, 13) },
        { 30, new Vector3(39, -5.86f, 13) },
        { 31, new Vector3(47, -5.86f, 13) },
        { 32, new Vector3(55, -5.86f, 13) },
        { 33, new Vector3(63, -5.86f, 13) },
        { 34, new Vector3(63, -5.86f, 13) },
        { 35, new Vector3(63, -5.86f, 13) },

        // row 6
        { 36, new Vector3(11, -5.86f, 6) },
        { 37, new Vector3(19, -5.86f, 6) },
        { 38, new Vector3(27, -5.86f, 6) },
        { 39, new Vector3(35, -5.86f, 6) },
        { 40, new Vector3(43, -5.86f, 6) },
        { 41, new Vector3(51, -5.86f, 6) },
        { 42, new Vector3(59, -5.86f, 6) },
        { 43, new Vector3(67, -5.86f, 6) },

        // row 7
        { 44, new Vector3(15, -5.86f, -0.5f) },
        { 45, new Vector3(23, -5.86f, -0.5f) },
        { 46, new Vector3(31, -5.86f, -0.5f) },
        { 47, new Vector3(39, -5.86f, -0.5f) },
        { 48, new Vector3(47, -5.86f, -0.5f) },
        { 49, new Vector3(55, -5.86f, -0.5f) },
        { 50, new Vector3(63, -5.86f, -0.5f) },

        // row 8
        { 51, new Vector3(19, -5.86f, -7.5f) },
        { 52, new Vector3(27, -5.86f, -7.5f) },
        { 53, new Vector3(35, -5.86f, -7.5f) },
        { 54, new Vector3(43, -5.86f, -7.5f) },
        { 55, new Vector3(51, -5.86f, -7.5f) },
        { 56, new Vector3(59, -5.86f, -7.5f) },

        // row 9
        { 57, new Vector3(23, -5.86f, -14) },
        { 58, new Vector3(31, -5.86f, -14) },
        { 59, new Vector3(39, -5.86f, -14) },
        { 60, new Vector3(47, -5.86f, -14) },
        { 61, new Vector3(55, -5.86f, -14) },
    };

    // stores the locations that a building can be inside the positions - since the axis is the middle of the piece, it can just be a rotation
    private Dictionary<int, Quaternion> possibleTileRotations = new Dictionary<int, Quaternion>
    {
        { 0, Quaternion.Euler(-90, 0, 0) },
        { 1, Quaternion.Euler(-90, 60, 0) },
        { 2, Quaternion.Euler(-90, 120, 0) },
        { 3, Quaternion.Euler(-90, 180, 0) },
        { 4, Quaternion.Euler(-90, 240, 0) },
        { 5, Quaternion.Euler(-90, 300, 0) }
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


    private Dictionary<int, float> possibleRowPositions = new Dictionary<int, float>
    {
        // row 1
        { 0, 40f},

        // row 2
        {1, 33f},

        // row 3 
        {2, 26.5f},

        // row 4
        {3, 20f},
        
        // row 5
        {4, 13f},
        
        // row 6
        {5, 6f},
        
        // row 7
        {6, -0.5f},
        
        // row 8
        {7, -7.5f},
        
        // row 9
        {8, -14f},
    };

    private Dictionary<int, float> possibleHexPositionsRow1And9 = new Dictionary<int, float>
   {
        // row 1
        {0, 23.3f},
        {1, 31f},
        {2, 39f},
        {3, 47f},
        {4, 55f},
   };

    private Dictionary<int, float> possibleHexPositionsRow2And8 = new Dictionary<int, float>
   {
        {0, 19f},
        {1, 27f},
        {2, 35f},
        {3, 43f},
        {4, 51f},
        {5, 59f}
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


    SerialPort sp = new SerialPort("/dev/cu.usbserial-10", 9600);

    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        // might be too fast but it seemed to work fine for me? causes some timeout errors going this fast but slower made it really laggy waiting for the next update (should be on a different thread ig?)
        sp.ReadTimeout = 10;
    }


    // Update is called once per frame
    void Update()
    {

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
                            Vector3 selectedPosition = new Vector3(selectedHexagonActualNumber, -5.86f, selectedRowActualNumber);

                            Vector3 selectedPositionUnchanged = new Vector3(selectedHexagonActualNumber, -5.86f, selectedRowActualNumber);

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
        cameraAxis.transform.Rotate(Vector3.up * Time.deltaTime * 5f); // Adjust speed as needed

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
}
