using Raylib_cs;

namespace Azimuth.UI
{
	// ReSharper disable once InconsistentNaming
	public class UIManager
	{
		private static List<Widget> widgets = new List<Widget>();

		public static void Add(Widget _widget)
		{
			if (!widgets.Contains(_widget))
				widgets.Add(_widget);
		}

		public static void Remove(Widget _widget)
		{
			if (widgets.Contains(_widget))
				widgets.Remove(_widget);
		}

		internal static void Update()
		{
			widgets.Sort();

			for(int index = 0; index < widgets.Count; index++)
			{
				Widget widget = widgets[index];
				widget.Update(Raylib.GetMousePosition());
			}
		}

		internal static void Draw()
		{
			foreach(Widget widget in widgets)
				widget.Draw();
		}
	}
}