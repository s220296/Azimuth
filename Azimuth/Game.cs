namespace Azimuth
{
	public abstract class Game
	{
		public abstract void Load();
		public virtual void Draw() {}
		public virtual void Update(float _deltaTime) {}
		public abstract void Unload();
	}
}