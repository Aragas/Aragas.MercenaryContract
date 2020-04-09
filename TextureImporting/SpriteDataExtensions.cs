using System.Linq;
using System.Reflection;

using TaleWorlds.TwoDimension;

namespace Aragas.TextureImporting
{
	public static class SpriteDataExtensions
	{
		private static PropertyInfo NameProperty { get; } = typeof(SpriteCategory)
			.GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);

		private static FieldInfo CategoryProperty { get; } = typeof(SpritePart)
			.GetField("_category", BindingFlags.Instance | BindingFlags.NonPublic);

		/// <summary>
		/// Horrid masterpiece
		/// </summary>
		public static void AppendFrom(this SpriteData spriteData, SpriteData appendSpriteData)
		{
			foreach (var name in appendSpriteData.SpriteNames)
			{
				if (spriteData.SpriteNames.All(s => s.Key != name.Key))
					spriteData.SpriteNames.Add(name.Key, name.Value);
			}
			foreach (var category in appendSpriteData.SpriteCategories)
			{
				NameProperty.SetValue(category.Value, category.Value.Name.Replace("append_", ""));

				if (spriteData.SpriteCategories.FirstOrDefault(s => s.Value.Name == category.Value.Name) is var kvp && kvp.Value != null)
				{
					var sheetCount = kvp.Value.SpriteSheetCount;

					kvp.Value.SheetSizes = kvp.Value.SheetSizes.Concat(category.Value.SheetSizes).ToArray();

					kvp.Value.SpriteParts.AddRange(category.Value.SpriteParts.Select(p =>
					{
						p.SheetID += sheetCount;
						return p;
					}));
					kvp.Value.SpriteSheets.AddRange(category.Value.SpriteSheets);
				}
				else
					spriteData.SpriteCategories.Add(category.Key, category.Value);
			}
			foreach (var part in appendSpriteData.SpritePartNames)
			{
				if (spriteData.SpriteCategories.FirstOrDefault(s => s.Value.Name == part.Value.Category.Name) is var kvp && kvp.Value != null)
					CategoryProperty.SetValue(part.Value, kvp.Value);

				NameProperty.SetValue(part.Value.Category, part.Value.Category.Name.Replace("append_", ""));

				if (spriteData.SpritePartNames.All(s => s.Key != part.Key))
					spriteData.SpritePartNames.Add(part.Key, part.Value);
			}
		}

		public static void ImportFrom(this SpriteData spriteData, SpriteData importSpriteData)
		{
			foreach (var name in importSpriteData.SpriteNames)
			{
				if (spriteData.SpriteNames.All(s => s.Key != name.Key))
					spriteData.SpriteNames.Add(name.Key, name.Value);
			}
			foreach (var part in importSpriteData.SpritePartNames)
			{
				if (spriteData.SpritePartNames.All(s => s.Key != part.Key))
					spriteData.SpritePartNames.Add(part.Key, part.Value);
			}
			foreach (var category in importSpriteData.SpriteCategories)
			{
				if (spriteData.SpriteCategories.All(s => s.Key != category.Key))
					spriteData.SpriteCategories.Add(category.Key, category.Value);
			}
		}
	}
}