using System;

using CommunityPatch;

using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace Aragas.TextureImporting
{   // Character icon on Map:
    // ButtonWidget with child ImageIdentifierWidget
    // ImageId = CharacterId data
    // ImageTypeCode = 5
    // AdditionalArgs = ""
    // Texture - character_tableaue0
    // TextureProvider TaleWorlds.MountAndBlade.GauntletUI.ImageIdentifierTextureProvider


	public static class SpriteDataFactory
	{
		public static SpriteData CreateNewFromModule(string name, ResourceDepot resourceDepot)
		{
			var spriteData = new SpriteData(name);
            try
            {
                spriteData.Load(resourceDepot);
                foreach (var spriteCategory in spriteData.SpriteCategories)
                    spriteCategory.Value.LoadFromModules(resourceDepot);
            }
            catch(Exception ex)
            {
                CommunityPatchSubModule.Error(ex, "[Aragas.MercenaryContract]: Error while trying to initialize custom textures!");
            }

			return spriteData;
		}
	}
}