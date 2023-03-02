using Azimuth;
using Azimuth.UI;
using System.Numerics;

namespace Azimuth_Test
{
	public class AzimuthTestGame : Game
	{
		private ImageWidget image;
		private Button button;
		private Button button2;

		private void OnClickButton()
		{
			Console.WriteLine("Hi");
		}
		
		public override void Load()
		{
			int counter = 0;
			
			image = new ImageWidget(Vector2.Zero, new Vector2(200, 400), "cheadle");
			button = new Button(new Vector2(150, 150), new Vector2(150, 75), Button.RenderSettings.normal);
			button2 = new Button(new Vector2(250, 250), new Vector2(150, 75), Button.RenderSettings.normal);
			
			button.AddListener(OnClickButton);
			button.AddListener(() =>
			{
				Console.WriteLine($"I am Don Cheadle, Iron Man");
				
				if (counter % 2 == 0)
					UIManager.Add(image);
				else
					UIManager.Remove(image);
				
				counter++;
			});
			button2.AddListener(() =>
			{
				Console.WriteLine("Press other button");
			});
			
			UIManager.Add(button);
			UIManager.Add(button2);
		}

		public override void Draw()
		{
			
		}

		public override void Update(float _deltaTime)
		{
			
		}

		public override void Unload()
		{
			
		}
	}
}