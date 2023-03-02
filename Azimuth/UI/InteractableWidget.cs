using Raylib_cs;

using System.Numerics;

namespace Azimuth.UI
{
	public abstract class InteractableWidget : Widget
	{
		public InteractionState State { get; private set; }

		public bool Interactable { get; set; } = true;

		private ColorBlock colors;
		
		protected InteractableWidget(Vector2 _position, Vector2 _size, ColorBlock _colors) : base(_position, _size)
		{
			colors = _colors;
			State = InteractionState.Normal;
		}

		public override void Update(Vector2 _mousePos)
		{
			base.Update(_mousePos);

			bool clicked = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT);

			InteractionState oldState = State;

			if(State == InteractionState.Selected && !clicked)
			{
				State = focused ? InteractionState.Hovered : InteractionState.Normal;
			}
			else if(clicked && focused)
			{
				State = InteractionState.Selected;
			}
			else if(focused)
			{
				State = InteractionState.Hovered;
			}
			else
			{
				State = InteractionState.Normal;
			}

			if(!Interactable)
				State = InteractionState.Disabled;
			
			if(State != oldState)
				OnStateChange(State, oldState);
		}

		protected abstract void OnStateChange(InteractionState _state, InteractionState _oldState);
		
		protected Color ColorFromState()
		{
			switch(State)
			{
				case InteractionState.Normal:
					return colors.normal;
				case InteractionState.Hovered:
					return colors.hovered;
				case InteractionState.Selected:
					return colors.selected;
				case InteractionState.Disabled:
					return colors.disabled;
				default:
					return Color.BLANK;
			}
		}
	}
}