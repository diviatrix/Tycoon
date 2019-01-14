using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Object bindings")]
    public GameData gameData;
    public GameObject framePrefab;
    public GameObject buildFramePrefab;
    
    [Header("UI Object bindings")]
    
    
     
    public Text notificationText; 

    [Header("Object debug binding")]
    public GameObject chosenPrefabToBuild; // prefab for instantiation    
    public GameObject selectedGO;
    public ResourcePerTime food = new ResourcePerTime() {resource = Resource.food, perSeconds = 60, isGathering = true};

    // frames
    private GameObject buildingSelectionFrameObject;
    private GameObject buildFrameObject;
    // grid stuff
    private SnapGrid grid;
    
    private bool isMobile; // check if mobile or pc
    float touchDuration;
    Touch touch;
    private Vector3 finalPosition = new Vector3();

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
        CoreObjectsFindOnScene();
        GetComponent<BuildPanelController>().FillBuildPanel();
        
        InstantiateFrame();
        InstantiateBuildFrame();
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void CoreObjectsFindOnScene()
    {
        grid = FindObjectOfType<SnapGrid>();
    }

    public void SelectedDestroy()
    {
        if (!selectedGO) return;
        PlaceableObject bld = selectedGO.GetComponent<PlaceableObject>();
                
        bld.DestroyMe();
        ClearSelection();        
    }
    public void SelectedSell()
    {
        if (!selectedGO) return;
        PlaceableObject bld = selectedGO.GetComponent<PlaceableObject>();
        if (bld.canSell)
        {
            bld.Sell();
            ClearSelection();
            GetComponent<Notification>().text.text = "Sold " + bld.buildingName;
        } 
        else
        {
            GetComponent<Notification>().text.text = bld.buildingName + " is not sellable" ;
        }
                
    }
    public void SelectedRotate()
    {
        if (!selectedGO) return;
        selectedGO.GetComponent<PlaceableObject>().RotateMe(90);
    }

    public void EnterBuildMode()
    {
        ClearSelection();
        GetComponent<BuildActionPanelController>().EnterBuildMode(chosenPrefabToBuild.GetComponent<PlaceableObject>());   
    }

    public void ExitBuildMode()
    {
        GetComponent<BuildActionPanelController>().Disable();
        chosenPrefabToBuild = null; // clear building prefab
        buildFrameObject.SetActive(false);
    }
    void EnableBuildingSelectionFrameTo(GameObject go)
    {
        buildingSelectionFrameObject.transform.position = go.transform.position;
        buildingSelectionFrameObject.SetActive(true);
    }

    void DisableSelectionFrame()
    {
        buildingSelectionFrameObject.SetActive(false);
    }


    public void HandleObjectsInteraction(GameObject clickedGo, Vector3 point)
    {
        

        // build stuff, check if clicked snapgrid
        SnapGrid clickedGrid = clickedGo.GetComponent<SnapGrid>();
        if (clickedGrid)
        {            
            ClearSelection();

            if (!chosenPrefabToBuild) // leave if there is no prefab
            {
                return; // #todo: need to add some ui action like choose                
            }

            // check if can build, and build
            if (CanBuildIt(chosenPrefabToBuild.GetComponent<PlaceableObject>()))
            {
                gameData.resources = gameData.resources - chosenPrefabToBuild.GetComponent<PlaceableObject>().cost;
                PlaceObjectWithParams(chosenPrefabToBuild, point, Quaternion.identity);
            }
            EnterBuildMode();
        }

        // for interaction with anything, like.. Building?
        ClickableObject clickableGo = clickedGo.GetComponent<ClickableObject>();
        if (clickableGo)
        {
            PlaceableObject clickedBuilding = clickableGo.GetComponent<PlaceableObject>();
            if (clickedBuilding)
            {
                selectedGO = clickableGo.gameObject; // set clicked go as selected
                EnableBuildingSelectionFrameTo(selectedGO);
                ExitBuildMode();                
            }
            clickableGo.OnClick();
        }
    }

    // gameplay build
    public bool CanBuildIt(PlaceableObject bld)
    {
        bool can = false;

        // need fix
        if (bld.cost <= gameData.resources)
        {
            can = true;
        }
        return can;
    }


    void Update()
    {
        // ray
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // always casting
        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = grid.GetNearestPointOnGrid(hit.point);
        }

        if (isMobile)
        {
            if(Input.touchCount > 0)
            { //if there is any touch
                touchDuration += Time.deltaTime;
                touch = Input.GetTouch(0);
    
                if(touch.phase == TouchPhase.Ended && touchDuration < 0.2f) //making sure it only check the touch once && it was a short touch/tap and not a dragging.
                
                {
                    StartCoroutine("singleOrDouble");
                }
            }
            else
            {
                touchDuration = 0.0f;
            }                
        }
        if (Application.isEditor)
        {
            // click on objects
            if (!IsPointerOverUIObject()) // if not over ui
            {

                Transform go = hit.transform;
                if (Input.GetMouseButtonDown(0))
                {
                    if (go != null) HandleObjectsInteraction(go.gameObject, hit.point);
                }
            }

            if (chosenPrefabToBuild != null)
            {                
                buildFrameObject.transform.position = finalPosition;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                GenerateForest();
            }

            if (Input.GetMouseButtonDown(1)) // exit build mode or rmb #todo: rework this, dunno how
            {
                ExitBuildMode();
            }
        }
    }
    IEnumerator singleOrDouble()
    {
        // ray
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // always casting
        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = grid.GetNearestPointOnGrid(hit.point);
        }

        yield return new WaitForSeconds(0.15f);
        if(touch.tapCount == 1)
        {
            if (!IsPointerOverUIObject()) // if not over ui
                {
                    Transform go = hit.transform;
                    if (go != null) HandleObjectsInteraction(go.gameObject, hit.point);
                }
        }            

        else if(touch.tapCount == 2){
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");
            Debug.Log ("Double");
        }
    }
    public void GenerateForest()
    {
        WipeScene();
        GetComponent<FieldGenerator>().Generate();        
    }

    public void ClearSelection() // clear unit selection
    {
        DisableSelectionFrame();
        selectedGO = null;
        GetComponent<SelectionActionPanelController>().ShowActionPanel(false);
    }

    void InstantiateFrame()
    {
        buildingSelectionFrameObject = GameObject.Instantiate(framePrefab, Vector3.down * 100, Quaternion.identity);
        buildingSelectionFrameObject.SetActive(false);
    }

    void InstantiateBuildFrame()
    {
        buildFrameObject = GameObject.Instantiate(buildFramePrefab, Vector3.zero, Quaternion.identity);
        buildFrameObject.SetActive(false);
    }

    public void ChangeActiveBuildingTo(int id)
    {
        chosenPrefabToBuild = gameData.availableObjects[id];
        EnterBuildMode();        
    }

    public void WipeScene()
    {
        gameData.generatedObjects = new List<PlaceableObject>();
        foreach (Transform child in gameData.generatedObjectsGO.transform)
        {
            Destroy(child.gameObject);
        }
        gameData.generatedObjects = new List<PlaceableObject>();
        foreach (Transform child in gameData.generatedObjectsGO.transform)
        {
            Destroy(child.gameObject);
        }
    }
 
    // object placer
    public void PlaceObjectWithParams(GameObject prefab, Vector3 clickPoint, Quaternion rot)
    {
        Vector3 position = grid.GetNearestPointOnGrid(clickPoint);
        GameObject go = GameObject.Instantiate(prefab);
        PlaceableObject bld = go.GetComponent<PlaceableObject>();
        bld.gameData = gameData;
        bld.Initialize();

        go.name = bld.name;
        go.transform.position = position;
    }

    public int SearchIdByNameIn(string str, List<GameObject> list)
    {
        int id = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (str == list[i].GetComponent<PlaceableObject>().buildingName)
            {
                id = i;
            }
        }
        return id;
    }

    public void PlaceObjectfromSO(SerializableObject so)
    {
        GameObject go = GameObject.Instantiate(gameData.availableObjects[SearchIdByNameIn(so.name, gameData.availableObjects)]);
        PlaceableObject building = go.GetComponent<PlaceableObject>();
        building.gameData = gameData;
        building.type = so.type;
        building.InitializeWithSO(so);
    }

    private void FixedUpdate() 
    {
        if (gameData.resources.citizen != 0 && !food.isGathering)
        {
            StartFoodReduction();
        }
    }

    void StartFoodReduction()
    {
        food.amount = gameData.resources.citizen;
        StartCoroutine(CitizenEatTimer());
        food.isGathering = true;
    }

    IEnumerator CitizenEatTimer()
    {   
        yield return new WaitForSeconds(food.perSeconds);
        gameData.resources.AddResource(food.resource, -food.amount); 
        food.isGathering = false;
    }
}