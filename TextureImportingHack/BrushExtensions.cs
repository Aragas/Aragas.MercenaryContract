using System.Linq;

using TaleWorlds.GauntletUI;

namespace Aragas.TextureImportingHack
{
	public static class BrushExtensions
	{
		public static void AppendFrom(this Brush brush, Brush appendBrush)
		{
			foreach (var layer in appendBrush.Layers)
			{
				if (brush.Layers.All(l => l.Name != layer.Name))
					brush.AddLayer(layer);
			}
			foreach (var style in appendBrush.Styles)
			{
				if (brush.Styles.All(s => s.Name != style.Name))
					brush.AddStyle(style);
			}
			foreach (var animation in appendBrush.GetAnimations())
			{
				if (brush.GetAnimations().All(a => a.Name != animation.Name))
					brush.AddAnimation(animation);
			}
		}
	}
}