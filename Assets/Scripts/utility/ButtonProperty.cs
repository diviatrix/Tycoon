using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProperty : MonoBehaviour
{
	public SceneObjectData data;
	public Image image;
	public Text text;

	public void Set()
	{
		EventManager.BuildObject = data; // enter build mode here
		EventManager.ExitSelectMode();
	}
}