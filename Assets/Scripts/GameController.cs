using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[Header("Game bindings Settings")]
	public GameData gameData;
	public SnapGrid grid;
	public GameObject framePrefab;
	public GameObject citizenPrefab;
	
	private SceneObjectData buildOjbect;
	private SceneObjectBehavior selectedObject;
	private Vector3 finalPosition = new Vector3();
    private bool isMobile; // check if mobile or pc
    private Touch touch;
	private bool inBuildMode;
	private SaveSystem saveSystem;
	private FieldGenerator fieldGenerator;
	private Transform spawnedObjectsParent;
	private FrameMover frameMover;
	private ResourcePerTime food = new ResourcePerTime() { resource = new Resource {type = ResourceType.food, amount = 1 }, perSeconds = 60, isGathering = false };
	private bool isGrowingCitizen = false;
	private Transform spawnedCitizenParent;

	[Header("Camera Settings")]
	public Camera cameraComponent;
	public Transform playerTransform;
	public float zoomSpeed; // camera zoom speed
	public float touchZoomSpeed; // 
	public float camSpeed; // camera move speed
	public float touchSpeed; // 
	public float orthoMinZ; // vertical camera limit
	public float minCamZ;
	public float maxCamZ;

	private void Start()
    {
		cameraComponent = Camera.main;
		playerTransform = cameraComponent.transform.parent;

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
		foreach(Resource res in selectedObject.data.cost)
		{
			if (gameData.GetResourceByType(res.type)+res.amount <= gameData.GetResourceCapacity(res.type))
			gameData.AddBalanceByType(res.type, res.amount);
		}

		Destroy(selectedObject.gameObject);

		foreach(Resource res in selectedObject.data.capacity)
		{
			gameData.ReduceCapacityByType(res.type, res.amount);
		}

		EventManager.ExitSelectMode();
	}

	public void RotateSelectedObject()
	{
		selectedObject.transform.Rotate(Vector3.up, 90);
	}

	public void ChangePlayerBalance(List<Resource> res)
	{
		foreach(Resource r in res)
		{
			gameData.AddBalanceByType(r.type, r.amount);
		}	
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

	void CameraMovementHandler()
	{
		playerTransform.position -= playerTransform.right * Input.GetAxis("Mouse X") * camSpeed;
		playerTransform.position -= playerTransform.forward * Input.GetAxis("Mouse Y") * camSpeed;
	}

	void CameraZoomHandler()
	{
		// camera ortho zoom
		if(cameraComponent.orthographic)
		{
			if (cameraComponent.orthographicSize >= orthoMinZ)
			{
				cameraComponent.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
			}

			// push to zlimit if camera is too close
			if (cameraComponent.orthographicSize < orthoMinZ)
			{
				cameraComponent.orthographicSize = orthoMinZ;
			}
		}
		else if(!cameraComponent.orthographic)
		{
			playerTransform.Translate(Vector3.up * -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);

			if(playerTransform.position.y < minCamZ)
			{
				playerTransform.position = new Vector3(playerTransform.position.x, minCamZ, playerTransform.position.z);
			}

			if(playerTransform.position.y > maxCamZ)
			{
				playerTransform.position = new Vector3(playerTransform.position.x, maxCamZ, playerTransform.position.z);
			}
		}
		
	}

	void CameraController()
	{
		// ray
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// always casting
		if (Physics.Raycast(ray, out hit))
		{
			finalPosition = grid.GetNearestPointOnGrid(hit.point);
		}

		if (!isMobile)
		{
			// camera dnd
			if (Input.GetMouseButton(2))
			{
				CameraMovementHandler();
			}

			CameraZoomHandler();

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

		if (isMobile)
		{
			if (Input.touchCount > 0)
			{
				if (IsPointerOverUIObject())
				{
					return; // leave cycle if is over ui
				}
				//if there is any touch
				touch = Input.GetTouch(0);

				// Move the cube if the screen has the finger moving.
				if (touch.phase == TouchPhase.Moved && Input.touchCount == 1)
				{
					playerTransform.position -= playerTransform.right * touch.deltaPosition.x * touchSpeed;
					playerTransform.position -= playerTransform.forward * touch.deltaPosition.y * touchSpeed;
				}

				else if (Input.touchCount == 2)
				{
					// Store both touches.
					Touch touchOne = Input.GetTouch(1);

					// Find the position in the previous frame of each touch.
					Vector2 touchZeroPrevPos = touch.position - touch.deltaPosition;
					Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

					// Find the magnitude of the vector (the distance) between the touches in each frame.
					float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
					float touchDeltaMag = (touch.position - touchOne.position).magnitude;

					// Find the difference in the distances between each frame.
					float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

					// ... change the orthographic size based on the change in distance between the touches.
					cameraComponent.orthographicSize += deltaMagnitudeDiff * touchZoomSpeed;
					
					playerTransform.Translate(Vector3.up * deltaMagnitudeDiff * touchZoomSpeed);

					// Make sure the orthographic size never drops below zero.
					cameraComponent.orthographicSize = Mathf.Max(cameraComponent.orthographicSize, 0.5f);

					if(playerTransform.position.y < minCamZ)
					{
						playerTransform.position = new Vector3(playerTransform.position.x, minCamZ, playerTransform.position.z);
					}

					if(playerTransform.position.y > maxCamZ)
					{
						playerTransform.position = new Vector3(playerTransform.position.x, maxCamZ, playerTransform.position.z);
					}					
				}

				//making sure it only check the touch once && it was a short touch/tap and not a dragging.
				else
				if
					(
					touch.phase == TouchPhase.Ended &&
					touch.deltaPosition == new Vector2(0, 0) &&
					touch.tapCount == 1 &&
					!EventSystem.current.IsPointerOverGameObject()
					)
				{
					Transform go = hit.transform;
					if (go != null) HandleObjectsInteraction(go.gameObject, hit.point);
				}
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
				if (gameData.CanBuild(buildOjbect.cost))
				{
					
					EventManager.Message = "Built: " + buildOjbect.objectName;
					ObjectPlacer.PlaceObject(buildOjbect, grid.GetNearestPointOnGrid(point), Quaternion.identity, spawnedObjectsParent);
					
					
					foreach(Resource res in buildOjbect.cost)
					{
						gameData.ReduceBalanceByType(res.type, res.amount);
					}
					
					foreach(Resource res in buildOjbect.capacity)
					{
						gameData.AddCapacityByType(res.type, res.amount);
					}

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
		saveSystem.SaveGame(spawnedObjectsParent, gameData.GetBalance(), gameData.GetResourcesCapacity());
	}

	public void LoadGame()
	{
		EventManager.Message = ("Loading game");
		WipeScene();
		
		System.Tuple<List<SceneObjectSaveData>, Resources, Resources> loadedData = saveSystem.LoadGame();

		gameData.SetBalance(loadedData.Item2);
		gameData.SetCapacity(loadedData.Item3);

		foreach (SceneObjectSaveData so in loadedData.Item1)
		{
			ObjectPlacer.PlaceObject(so.data, so.position, so.rotation,spawnedObjectsParent);
		}

		EventManager.Message = ("Game Loaded");
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

		if (gameData.GetBalance().citizen < gameData.GetResourcesCapacity().citizen && !isGrowingCitizen)
		{
			StartCoroutine(GrowCitizen(1));
		}

		if (gameData.GetBalance().citizen > gameData.GetResourcesCapacity().citizen && !isGrowingCitizen)
		{
			EventManager.Message = "There is not enough houses, some citizen will leave our town soon";
			StartCoroutine(GrowCitizen(-1));
		}
	}

	IEnumerator GrowCitizen(int i)
	{
		isGrowingCitizen = true;
		Debug.Log("Started growing citizen cycle: " + i);

		yield return new WaitForSeconds(10);

		gameData.AddBalanceByType(ResourceType.citizen, i);
		//GameObject citizen = Instantiate(citizenPrefab, GameObject.FindWithTag("TownHall").transform);
		//citizen.transform.Translate(new Vector3(0,0,-1));
		//citizen.transform.Rotate(Vector3.up, -90);
		isGrowingCitizen = false;
	}

	private void Update()
	{
		CameraController();
	}

	void StartFoodReduction()
	{		
		food.resource.amount = gameData.GetBalance().citizen;
		StartCoroutine(CitizenEatTimer());
		food.isGathering = true;
	}

	IEnumerator CitizenEatTimer()
	{
		Debug.Log("Start reduce food by 1 each " + food.perSeconds/ food.resource.amount + "seconds");
		yield return new WaitForSeconds(food.perSeconds/ food.resource.amount);

		if (gameData.GetBalance().food - food.resource.amount >= 0)
		{
			gameData.ReduceBalanceByType (ResourceType.food, 1);			
		} else EventManager.Message = "Our citizen are starving!";
		
		food.isGathering = false;
	}
}