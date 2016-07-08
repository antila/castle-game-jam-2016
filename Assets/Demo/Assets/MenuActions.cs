using UnityEngine;
using UnityEngine.InputNew;

// GENERATED FILE - DO NOT EDIT MANUALLY
public class MenuActions : ActionMapInput {
	public MenuActions (ActionMap actionMap) : base (actionMap) { }
	
	public ButtonInputControl @select { get { return (ButtonInputControl)this[0]; } }
	public ButtonInputControl @cancel { get { return (ButtonInputControl)this[1]; } }
	public AxisInputControl @moveX { get { return (AxisInputControl)this[2]; } }
	public AxisInputControl @moveY { get { return (AxisInputControl)this[3]; } }
	public Vector2InputControl @move { get { return (Vector2InputControl)this[4]; } }
	public ButtonInputControl @rotate { get { return (ButtonInputControl)this[5]; } }
	public ButtonInputControl @start { get { return (ButtonInputControl)this[6]; } }
}
