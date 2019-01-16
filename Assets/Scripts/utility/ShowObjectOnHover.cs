using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ShowObjectOnHover : MonoBehaviour
{
	public GameObject hoverTarget;

    // Update is called once per frame
    void FixedUpdate()
    {
		if (EventSystem.current.currentSelectedGameObject == gameObject)
		{
			hoverTarget.SetActive(true);
		}

		else hoverTarget.SetActive(false);

    }
}
