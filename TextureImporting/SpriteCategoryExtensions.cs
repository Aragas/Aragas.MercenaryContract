using System;
using System.IO;
using System.Reflection;
using CommunityPatch;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace Aragas.TextureImporting
{
	public static class SpriteCategoryExtensions
	{
		private static PropertyInfo IsLoadedProperty { get; } = typeof(SpriteCategory).GetProperty("IsLoaded", BindingFlags.Instance | BindingFlags.Public);
		public static void LoadFromModules(this SpriteCategory spriteCategory, ResourceDepot resourceDepot)
		{
			if (!spriteCategory.IsLoaded)
			{
				IsLoadedProperty.SetValue(spriteCategory, true);

				for (var i = 1; i <= spriteCategory.SpriteSheetCount; i++)
				{
					try
					{
						var filePath = resourceDepot.GetFilePath($"SpriteSheets\\{spriteCategory.Name}\\{spriteCategory.Name}_{i}.png");
						var fileInfo = new FileInfo(filePath);
						spriteCategory.SpriteSheets.Add(
							new Texture(
								new EngineTexture(
									TaleWorlds.Engine.Texture.CreateTextureFromPath(fileInfo.Directory.FullName, fileInfo.Name))));
					}
					catch (Exception ex)
					{
                        CommunityPatchSubModule.Error(ex, "[Aragas.MercenaryContract]: Error while trying to load custom textures!");
					}
				}
			}
		}
	}
}