using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputNew;
using UnityEngine.Serialization;

// This sizer does not use the UI event system,
// since it's not integrated with the Input System prototype at this point.
// UI is only used for graphics.

public class CubeSizer : MonoBehaviour
{
	MenuActions m_Actions;
	
	public PlayerInput playerInput;
	public GameObject menu;
	public Slider slider;
	
	public float size { get { return slider.value; } }

	public void OpenMenu()
	{
		enabled = true;
		m_Actions = playerInput.GetActions<MenuActions>();
		m_Actions.active = true;
		menu.SetActive(true);
	}
	
	public void CloseMenu()
	{
		menu.SetActive(false);
		m_Actions.active = false;
		enabled = false;
	}
	
	public void ToggleMenu()
	{
		if (enabled)
			CloseMenu();
		else
			OpenMenu();
	}
	
	void Update()
	{
		if (m_Actions.moveX.negative.wasJustPressed)
			slider.value -= 0.1f;
		if (m_Actions.moveX.positive.wasJustPressed)
			slider.value += 0.1f;
		if (m_Actions.select.wasJustPressed)
			CloseMenu();
	}
}
