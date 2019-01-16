using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public GameData gameData;
	public SnapGrid grid;
	public GameObject framePrefab;
    public SceneObjectData buildOjbect;
	public SceneObjectBehavior selectedObject;

    private Vector3 finalPosition = new Vector3();
    private bool isMobile; // check if mobile or pc
    private float touchDuration;
    private Touch touch;
	private bool inBuildMode;
	private SaveSystem saveSystem;
	private FieldGenerator fieldGenerator;
	private Transform spawnedObjectsParent;
	private FrameMover frameMover;
	private ResourcePerTime food = new ResourcePerTime() { resources = new Resources (), perSeconds = 60, isGathering = false };

	private void Start()
    {
		gameData.ResetResources();
		fieldGenerator = gameObject.AddComponent<FieldGenerator>();

		frameMover = gameObject.AddComponent<FrameMover>();
		frameMover.framePrefab = framePrefab;

		EventManager.GameData = gameData;

		saveSystem = new SaveSystem();
		
		spawnedObjectsParent = new GameObject("Spawned Objects").transform;

		isMobile = Application.isMobilePlatform;		
	}

	public void StartNewGame()
	{
		WipeScene();
		fieldGenerator.Generate(228, grid, spawnedObjectsParent);
		gameData.ResetResources();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	void OnEnable()
	{
		EventManager.OnEnterBuildMode += EnterBuildMode;
		EventManager.OnExitBuildMode += ExitBuildMode;
		EventManager.OnSelection += SelectObject;
		EventManager.OnExitSelectMode += ExitSelectMode;
		EventManager.AddToBalance += ChangePlayerBalance;
		EventManager.OnRecieveGameData += SetGameData;
	}

	void OnDisable()
	{
		EventManager.OnExitBuildMode -= ExitBuildMode;
		EventManager.OnEnterBuildMode -= EnterBuildMode;
		EventManager.OnSelection -= SelectObject;
		EventManager.OnExitSelectMode -= ExitSelectMode;
		EventManager.OnRecieveGameData -= SetGameData;
	}

	void SetGameData(GameData data)
	{
		gameData = data;
	}

	public void SellSelectedObject()
	{
		ChangePlayerBalance(selectedObject.data.cost);
		Destroy(selectedObject.gameObject);
		EventManager.ExitSelectMode();
	}

	public void RotateSelectedObject()
	{
		selectedObject.transform.Rotate(Vector3.up, 90);
	}

	public void ChangePlayerBalance(Resources res)
	{
		gameData.AddResources(res);
	}

	void SelectObject(SceneObjectBehavior sob)
	{
		selectedObject = sob;
		EventManager.Message = "Selected " + sob.data.objectName;
	}

	void EnterBuildMode(SceneObjectData data)
	{
		buildOjbect = data;
		inBuildMode = true;
	}

	void ExitSelectMode()
	{
		selectedObject = null;
		EventManager.Message = "";
	}

	void ExitBuildMode()
	{
		inBuildMode = false;
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

				//making sure it only check the touch once && it was a short touch/tap and not a dragging.
				if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f && !EventSystem.current.IsPointerOverGameObject()) 
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

            if (Input.GetMouseButtonDown(1)) // exit build mode or rmb #todo: rework this, dunno how
            {
				EventManager.ExitBuildMode();
				EventManager.ExitSelectMode();
            }
        }
    }

    public void HandleObjectsInteraction(GameObject go, Vector3 point)
    {
        // build stuff, check if clicked snapgrid
        SnapGrid clickedGrid = go.GetComponent<SnapGrid>();
        if (clickedGrid)
        {
			if (inBuildMode)
			{
				if (gameData.GetBalance() >= buildOjbect.cost)
				{
					gameData.ReduceResources(buildOjbect.cost);
					EventManager.Message = "Built: " + buildOjbect.objectName;
					ObjectPlacer.PlaceObject(buildOjbect, grid.GetNearestPointOnGrid(point), Quaternion.identity, spawnedObjectsParent);
					// relaunch OnEnterBuildMode event with selected object
					EventManager.BuildObject = buildOjbect;
				}
				else EventManager.Message = "Cant build " + buildOjbect.objectName + ", not enough resources";
			}
			else // deselect active object if not in build mode and clicked ground
			{
				EventManager.ExitSelectMode();
			}
        }

        // for interaction with SceneObjects
        SceneObjectBehavior so = go.GetComponent<SceneObjectBehavior>();
        if (so)
        {
            EventManager.SelectedObject = so; // Select object
			EventManager.ExitBuildMode();     // Leave Build mode if was
        }
    }

    private bool IsPointerOverUIObject()
    {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
		{
			position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
		};
		List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

	public void Save()
	{
		EventManager.Message = ("Saving game");
		saveSystem.SaveGame(spawnedObjectsParent, gameData.GetBalance());
	}

	public void LoadGame()
	{
		EventManager.Message = ("Loading game");
		WipeScene();
		
		System.Tuple<List<SceneObjectSaveData>, Resources> loadedData = saveSystem.LoadGame();

		gameData.SetBalance(loadedData.Item2);

		foreach (SceneObjectSaveData so in loadedData.Item1)
		{
			ObjectPlacer.PlaceObject(so.data, so.position, so.rotation,spawnedObjectsParent);
		}

		EventManager.Message = ("Game Loaded");
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
		if (touch.tapCount == 1)
		{
			if (!EventSystem.current.IsPointerOverGameObject()) // if not over ui
			{
				Transform go = hit.transform;
				if (go != null) HandleObjectsInteraction(go.gameObject, hit.point);
			}
		}

		else if (touch.tapCount == 2)
		{
			//this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
			StopCoroutine("singleOrDouble");
			Debug.Log("Double");
		}
	}

	public void WipeScene()
	{
		foreach (Transform child in spawnedObjectsParent)
		{
			Destroy(child.gameObject);
		}
	}

	private void FixedUpdate()
	{
		if (gameData.GetBalance().citizen != 0 && !food.isGathering)
		{
			 StartFoodReduction();			
		}
	}

	void StartFoodReduction()
	{
		food.resources.food = gameData.GetBalance().citizen;
		StartCoroutine(CitizenEatTimer());
		food.isGathering = true;
	}

	IEnumerator CitizenEatTimer()
	{
		Debug.Log("Consuming 1 food in " + food.perSeconds / food.resources.food + "seconds");
		Resources consumedFood = new Resources { food = 1 };			

		yield return new WaitForSeconds(food.perSeconds/ food.resources.food);

		if (gameData.GetBalance().food > 0)
		{
			gameData.ReduceResources(consumedFood);			
		} else EventManager.Message = "Our citizen are starving!";
		
		food.isGathering = false;
	}
}