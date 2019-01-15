using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMover : MonoBehaviour
{
	public GameObject framePrefab;

	private GameObject frame;
    // Start is called before the first frame update
    void Start()
    {
		frame = GameObject.Instantiate(framePrefab);
		frame.name = "SelectionFrame";
		DisableFrame();
	}

	void OnEnable()
	{
		EventManager.OnSelection += SelectObject;
		EventManager.OnExitSelectMode += DisableFrame;
	}

	void OnDisable()
	{
		EventManager.OnSelection -= SelectObject;
		EventManager.OnExitSelectMode -= DisableFrame;
	}

	void SelectObject(SceneObjectBehavior sob)
	{
		EnableFrame();
		MoveFrameTo(sob.transform.position);
	}

	void EnableFrame()
	{
		frame.SetActive(true);
	}

	void DisableFrame()
	{
		frame.SetActive(false);
	}
	void MoveFrameTo(Vector3 position)
	{
		frame.transform.position = position;
	}
}
