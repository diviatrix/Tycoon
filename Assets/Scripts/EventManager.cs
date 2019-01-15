using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "SceneObjectData",menuName = "Objects/EventManager")]
[System.Serializable]
public class EventManager : MonoBehaviour
{
	//
	//
	//	Events with SceneObjectData
	//
	//

	//Delegate for SceneObjectData
	public delegate void SceneObject(SceneObjectData selectedObject);     

	// Event onSelectToBuil
    public static event SceneObject OnEnterBuildMode;                      
    private static SceneObjectData buildObject;                           
    public static SceneObjectData BuildObject                             
    {               
        get{return buildObject;}
        set{buildObject = value; OnEnterBuildMode?.Invoke(BuildObject); }  
    } 

	// Event OnBuild
    public static event SceneObject OnBuild;
    private static SceneObjectData builtObject; 
    public static SceneObjectData BuiltObject 
    {               
        get{return builtObject;}
        set{builtObject = value; OnBuild?.Invoke(BuiltObject); }  
    }

	//
	//
	//	Events with SceneObjectBehavior
	//
	//

	//Delegate for SceneObjectData
	public delegate void SOBehavior(SceneObjectBehavior selectedObject);

	// Event OnSelection
	public static event SOBehavior OnSelection;
	private static SceneObjectBehavior selectedObject;
	public static SceneObjectBehavior SelectedObject
	{
		get { return selectedObject; }
		set { selectedObject = value; OnSelection?.Invoke(SelectedObject); OnExitBuildMode?.Invoke(); }
	}

	//Delegate for Event Action
	public delegate void EventAction();


	// Event OnExitBuildMode
	public static event EventAction OnExitBuildMode;
	public static void ExitBuildMode()
	{
		OnExitBuildMode?.Invoke();
	}

	// Event OnExitSelectMode
	public static event EventAction OnExitSelectMode;
	public static void ExitSelectMode()
	{
		OnExitSelectMode?.Invoke();
	}

	//
	//
	//	Events with string
	//
	//
	public delegate void Notification(string notification);

	// Event OnSendNotification
	public static event Notification OnSendNotification;
	private static string message;
	public static string Message
	{
		get { return message; }
		set { message = value; OnSendNotification?.Invoke(Message); }
	}

	//
	//
	//	Events with Resource
	//
	//
	public delegate void ResourceDelegate(Resources resources);

	// Event AddToBalance
	public static event ResourceDelegate AddToBalance;

	private static Resources amount;
	public static Resources Amount
	{
		get { return amount; }
		set { amount = value; AddToBalance?.Invoke(Amount); }
	}

	// Event OnChangeBalance
	public static event ResourceDelegate OnChangeBalance;

	private static Resources playerBalance;
	public static Resources PlayerBalance
	{
		get { return playerBalance; }
		set { playerBalance = value; OnChangeBalance?.Invoke(PlayerBalance); }
	}

	//
	//
	//	Events with GameData
	//
	//

	//Delegate for GameData
	public delegate void GDBehavior(GameData gameData);

	// Event OnSelection
	public static event GDBehavior OnRecieveGameData;
	private static GameData gameData;
	public static GameData GameData
	{
		get { return gameData; }
		set { gameData = value; OnRecieveGameData?.Invoke(GameData); }
	}
}
