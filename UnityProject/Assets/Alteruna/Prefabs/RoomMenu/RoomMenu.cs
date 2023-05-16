using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alteruna;
using System.Collections;
using System.Linq;
using Alteruna.Trinity;

public class RoomMenu : CommunicationBridge
{
	[SerializeField] private Text TitleText; // Room Title object
	[SerializeField] private ScrollRect ScrollRect; // Lobby menu scroll bar object
	[SerializeField] private GameObject LANEntryPrefab;
	[SerializeField] private GameObject WANEntryPrefab;
	[SerializeField] private GameObject CloudImage;
	[SerializeField] private GameObject ContentContainer;
	[SerializeField] private Button StartButton; // Create Game button
	[SerializeField] private Button LeaveButton;

	[SerializeField] public int oo = 0;

	[SerializeField] public GameObject InGameBG; // Background element for in-game pause menu
	[SerializeField] private Text InGameTitleText; // Room Title object for in-game pause menu
	[SerializeField] public GameObject InGameTitleBar; // Bar element for in-game Room Title
	[SerializeField] private Button InGameLeaveButton_; // Leave Room button for in-game pause menu
	[SerializeField] public GameObject InGameLeaveButton; // ^^ same as above, used to track the game object the button is attached to
	[SerializeField] public GameObject Crosshair_Obj; // Crosshair object

	[SerializeField] public int o0 = 0;

	[SerializeField] public GameObject BG; // main menu background element
	[SerializeField] public GameObject UIRoomMenu_Obj; // Lobby Menu element
	[SerializeField] public GameObject PLAY_BACK; // back button for Lobby Menu
	[SerializeField] public GameObject CONTROLS; // control scheme text that appears in the Options menu

	[SerializeField] public int Oo = 0;

	[SerializeField] public GameObject MAINMENU;

	public bool AutomaticallyRefresh = true;
	public float RefreshInterval = 5.0f;
	
	private List<Room> _rooms = new List<Room>();
	private List<RoomObject> _roomObjects = new List<RoomObject>();
	private float _refreshTime;

	private int count;
	private string _connectionMessage = "Connecting";
	private float _statusTextTime;
	private int _roomI = -1;

	public bool JoinRoom(string roomName, ushort password = 0)
	{
		roomName = roomName.ToLower();
		if (Multiplayer != null && Multiplayer.IsConnected)
		{
			foreach (var room in Multiplayer.AvailableRooms)
			{
				if (room.Name.ToLower() == roomName)
				{
					room.Join(password);
					return true;
				}
			}
		}
		return false;
	}

	private void Connected(Multiplayer multiplayer, Endpoint endpoint)
	{

		// if already connected to room
		if (multiplayer.InRoom)
		{
			JoinedRoom(multiplayer, multiplayer.CurrentRoom, multiplayer.Me);
			return;
		}
		
		MAINMENU.SetActive(true);
		StartButton.interactable = true;
		LeaveButton.interactable = false;
		
		if (TitleText != null)
		{
			TitleText.text = "Rooms";
		}
		if (InGameTitleText != null)
		{
			InGameTitleText.text = "Rooms";
		}
	}

	private void Disconnected(Multiplayer multiplayer, Endpoint endPoint)
	{
		StartButton.interactable = false;
		LeaveButton.interactable = false;

		_connectionMessage = "Reconnecting";
		if (TitleText != null)
		{
			TitleText.text = "Reconnecting";
		}
		if (InGameTitleText != null)
		{
			InGameTitleText.text = "Reconnecting";
		}
	}

	private void UpdateList(Multiplayer multiplayer)
	{
		if (ContentContainer == null) return;

		bool roomChange = false;
		{
			int roomI = Multiplayer.InRoom ? (int)Multiplayer.CurrentRoom.ID : -1;
			if (roomI != _roomI)
			{
				_roomI = roomI;
				roomChange = true;
			}
		}

		if (multiplayer.AvailableRooms.Count < _roomObjects.Count)
		{
			for (int i = _roomObjects.Count - 1; i >= multiplayer.AvailableRooms.Count; i--)
			{
				Destroy(_roomObjects[i].GameObject);
				_roomObjects.RemoveAt(i);
			}
		}

		for (int i = 0; i < multiplayer.AvailableRooms.Count; i++)
		{
			Room room = multiplayer.AvailableRooms[i];
			RoomObject entry;
			Button button;
			
			if (_roomObjects.Count > i)
			{
				if (room.Local != _roomObjects[i].Lan)
				{
					Destroy(_roomObjects[i].GameObject);
					if (room.Local)
					{
						entry = new RoomObject(Instantiate(LANEntryPrefab, ContentContainer.transform), true);
					}
					else
					{
						entry = new RoomObject(Instantiate(WANEntryPrefab, ContentContainer.transform));
					}
					_roomObjects[i] = entry;
					button = entry.GameObject.GetComponentInChildren<Button>();
				}
				else
				{
					// If unchanged, then skip to next room.
					if (roomChange || (room.ID == multiplayer.AvailableRooms[i].ID && room.Name == multiplayer.AvailableRooms[i].Name)) continue;
					entry = _roomObjects[i];
					button = entry.GameObject.GetComponentInChildren<Button>();
					button.onClick.RemoveAllListeners();
				}
			}
			else
			{
				if (room.Local)
				{
					entry = new RoomObject(Instantiate(LANEntryPrefab, ContentContainer.transform), true);
				}
				else
				{
					entry = new RoomObject(Instantiate(WANEntryPrefab, ContentContainer.transform));
				}
				
				button = entry.GameObject.GetComponentInChildren<Button>();
				
				_roomObjects.Add(entry);
			}

			// Hide private rooms.
			if (room.InviteOnly && room.ID != _roomI)
			{
				entry.GameObject.SetActive(false);
				continue;
			}
			
			entry.GameObject.SetActive(true);
			entry.GameObject.name = room.Name;
			entry.GameObject.GetComponentInChildren<Text>().text = room.Name;

			if (room.ID == _roomI)
			{
				button.interactable = false;
			}
			else
			{
				button.interactable = true;
				button.onClick.AddListener(() =>
				{
					room.Join();
					UpdateList(multiplayer);
				});
			}
		}
	}

	private void JoinedRoom(Multiplayer multiplayer, Room room, User user)
	{
		//this code executes after a user has JOINED a room (user is assigned a Player prefab and enters game world)
		StartButton.interactable = false;
		InGameLeaveButton_.interactable = true; // makes in-game pause menu button clickable
		BG.SetActive(false); // disables main menu background
		UIRoomMenu_Obj.SetActive(false); // disables Lobby Menu
		Crosshair_Obj.SetActive(true); // enables player crosshair
		Cursor.visible = false; // disables cursor

		if (TitleText != null) // updates Room Title with room name
		{
			TitleText.text = "In Room " + room.Name;
		}
		if (InGameTitleText != null) // updates in-game Room Title with room name
		{
			InGameTitleText.text = "In Room " + room.Name;
		}
	}

	private void LeftRoom(Multiplayer multiplayer)
	{
		//this code executes after a user has LEFT a room (user leaves game world and goes back to lobby menu)
		StartButton.interactable = true;
		InGameLeaveButton_.interactable = false;
		BG.SetActive(true); // enables main menu background
		InGameBG.SetActive(false); // disables in-game pause menu background
		UIRoomMenu_Obj.SetActive(true); // enables Lobby Menu
		Crosshair_Obj.SetActive(false); // disables Crosshair
		InGameTitleBar.SetActive(false); // disables in-game Room Title background
		InGameLeaveButton.SetActive(false); // disables pause menu leave button
		CONTROLS.SetActive(false); // disables pause menu control scheme text
		Cursor.visible = true; // enables cursor

		if (TitleText != null) // resets Room Title
		{
			TitleText.text = "Rooms";
		}
		if (InGameTitleText != null) // resets Room Title
		{
			InGameTitleText.text = "Rooms";
		}
	}

	private void FixedUpdate()
	{
		if (Multiplayer.IsConnected)
		{
			if (!AutomaticallyRefresh || (_refreshTime += Time.fixedDeltaTime) < RefreshInterval) return;
			_refreshTime -= RefreshInterval;

			Multiplayer.RefreshRoomList();

			if (TitleText == null) return;
			if (InGameTitleText == null) return;
			
			ResponseCode blockedReason = Multiplayer.GetLastBlockResponse();
			
			if (blockedReason == ResponseCode.NaN) return;
			
			string str = blockedReason.ToString();
			str = string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
			TitleText.text = str;
			InGameTitleText.text = str;
		}
		else if ((_statusTextTime += Time.fixedDeltaTime) >= 1)
		{
			_statusTextTime -= 1;
			ResponseCode blockedReason = Multiplayer.GetLastBlockResponse();
			if (blockedReason != ResponseCode.NaN)
			{
				string str = blockedReason.ToString();
				str = string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
				TitleText.text = str;
				InGameTitleText.text = str;
				return;
			}

			switch (count)
			{
				case 0:
					TitleText.text = _connectionMessage + ".  ";
					break;
				case 1:
					TitleText.text = _connectionMessage + ".. ";
					break;
				default:
					TitleText.text = _connectionMessage + "...";
					count = -1;
					break;
			}

			count++;
		}
	}

	private void Start()
	{
		if (Multiplayer == null)
		{
			Multiplayer = FindObjectOfType<Multiplayer>();
		}

		if (Multiplayer == null)
		{
			
			Debug.LogError("Unable to find a active object of type Multiplayer.");
			if (TitleText != null) TitleText.text = "Missing Multiplayer Component";
			enabled = false;
		}
		else
		{
			Multiplayer.Connected.AddListener(Connected);
			Multiplayer.Disconnected.AddListener(Disconnected);
			Multiplayer.RoomListUpdated.AddListener(UpdateList);
			Multiplayer.RoomJoined.AddListener(JoinedRoom);
			Multiplayer.RoomLeft.AddListener(LeftRoom);
			
			StartButton.onClick.AddListener(() =>
			{
				Multiplayer.JoinOnDemandRoom();
				_refreshTime = RefreshInterval;
			});
			
			LeaveButton.onClick.AddListener(() => { 
				Multiplayer.CurrentRoom?.Leave();
				_refreshTime = RefreshInterval;
			});

			InGameLeaveButton_.onClick.AddListener(() => { 
				Multiplayer.CurrentRoom?.Leave();
				_refreshTime = RefreshInterval;
			});
			
			if (TitleText != null)
			{
				ResponseCode blockedReason = Multiplayer.GetLastBlockResponse();
				if (blockedReason != ResponseCode.NaN)
				{
					string str = blockedReason.ToString();
					str = string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
					TitleText.text = str;
				}
				else
				{
					TitleText.text = "Connecting";
				}
			}

			// if already connected
			if (Multiplayer.IsConnected)
			{
				Connected(Multiplayer, null);
			}
		}

		StartButton.interactable = false;
		LeaveButton.interactable = false;
	}

	private void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)){ //if ESC button is pressed
			InGamePauseUI();
			if (InGameBG.activeSelf == true){ //checks if element is active
				Cursor.lockState = CursorLockMode.None; //unlocks mouse to application
				Cursor.visible = true; //makes cursor visible
			} else {
				Cursor.lockState = CursorLockMode.Locked; //locks mouse to application
				Cursor.visible = false; //hides cursor
			}
		}
	}

	private void InGamePauseUI() {
		InGameTitleBar.SetActive(!InGameTitleBar.activeSelf); //toggles In-Game Room Title
		InGameLeaveButton.SetActive(!InGameLeaveButton.activeSelf); //toggles In-Game Leave Button
		InGameBG.SetActive(!InGameBG.activeSelf); //toggles In-Game background
		CONTROLS.SetActive(!CONTROLS.activeSelf); //toggles control scheme
	}

	private struct RoomObject
	{
		public readonly GameObject GameObject;
		public readonly bool Lan;

		public RoomObject(GameObject obj, bool lan = false)
		{
			GameObject = obj;
			Lan = lan;
		}
	}
}