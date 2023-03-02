using Raylib_cs;
using System.Numerics;

namespace Azimuth.UI
{
	public class Button : InteractableWidget
	{
		public delegate void OnClickEvent();
		
		public class RenderSettings
		{
			public static RenderSettings normal = new RenderSettings();
			
			public ColorBlock colors = new ColorBlock()
			{
				disabled = new Color(255, 255, 255, 128),
				hovered = Color.DARKGRAY,
				normal = Color.LIGHTGRAY,
				selected = Color.BLACK
			};
			public string text = "Button";
			public float roundedness = 0.1f;
			public int fontSize = 20;
			public float fontSpacing = 1;
			public string? fontId = null;
			public Color textColor = Color.WHITE;
		}

		private OnClickEvent? onClick;
		
		private readonly float roundedness;
		private readonly string text;
		private readonly int fontSize;
		private readonly float fontSpacing;
		private readonly Font font;
		private readonly Color textColor;
		private readonly Vector2 textSize;
		
		public Button(Vector2 _position, Vector2 _size, RenderSettings _settings) : base(_position, _size, _settings.colors)
		{
			roundedness = _settings.roundedness;
			text = _settings.text;
			fontSize = _settings.fontSize;
			fontSpacing = _settings.fontSpacing;
			textColor = _settings.textColor;

			font = string.IsNullOrEmpty(_settings.fontId) ? Raylib.GetFontDefault() : Assets.Find<Font>(_settings.fontId);
			textSize = Raylib.MeasureTextEx(font, text, fontSize, fontSpacing) * 0.5f;
		}

		public void AddListener(OnClickEvent _event)
		{
			if(onClick == null)
			{
				onClick = _event;
			}
			else
			{
				onClick += _event;
			}
		}

		public void RemoveListener(OnClickEvent _event)
		{
			if(onClick != null)
				onClick -= _event;
		}
		
		public override void Draw()
		{
			Raylib.DrawRectangleRounded(Bounds, roundedness, 5, ColorFromState());
			Raylib.DrawTextPro(font, text, position + textSize, Vector2.Zero, 0f, fontSize, fontSpacing, textColor);
		}

		protected override void OnStateChange(InteractionState _state, InteractionState _oldState)
		{
			if(_state != InteractionState.Selected && _oldState == InteractionState.Selected)
			{
				// The button is no longer being click, so do the event.
				onClick?.Invoke();
			}
		}
	}
}