using TaleWorlds.GauntletUI;

namespace Aragas
{
	public static class BrushFactoryExtensions
	{
		public static void ImportAndAppend(this BrushFactory brushFactory, string brushName, string brushFile, string appendBrushName)
		{
			brushFactory.LoadBrushFile(brushFile);
			var brush = brushFactory.GetBrush(brushName);
			var appendBrush = brushFactory.GetBrush(appendBrushName);
			brush.AppendFrom(appendBrush);
		}
	}
}