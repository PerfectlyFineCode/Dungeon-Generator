using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegenUI : MonoBehaviour
{
	public Button RegenerateButton;
	public DungeonManager Manager;
	public Camera currentCamera;

	private void Start()
	{
		Manager.Generator.RefreshStatechanged.AddListener(StateChanged);
		Manager.Generate();
		StateChanged(Manager.Generator.CanRefresh);
	}

	private void Update()
	{
		if (Manager.Generator.CurrentRooms == null) return;
		var bounds = new Bounds(Vector3.zero, Vector3.zero);
		Manager.Generator.CurrentRooms.ForEach(x => bounds.Encapsulate(x.Bounds));
		currentCamera.orthographicSize = Mathf.Lerp(currentCamera.orthographicSize, bounds.extents.magnitude / 1.5f,
			Time.deltaTime * 5);
	}

	private void StateChanged(bool state)
	{
		RegenerateButton
			.GetComponentInChildren<TMP_Text>().text = !state
			? "Generating .."
			: "Regenerate";

		RegenerateButton.interactable = state;
	}
}