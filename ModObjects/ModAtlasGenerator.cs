using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPAtlasGenerationSystem;
using UnityEngine;
using TMPro;
using System.IO;

namespace BasicMod
{
    class ModAtlasGenerator : ItemsGroupAtlasGenerator
    {	/*
        public override string AtlasName => "ModAtlas";
        public override int MaxSize => 1024;

		public Texture2D atlasTexture;
		public TMP_SpriteAsset spriteAsset;
		private List<TMP_Sprite> tmpSprites;

		public override Texture2D[] AtlasTextures
		{
			get
			{
				List<Texture2D> list = (from salt in SaltFactory.allSalts
											  where salt.smallIcon != null
											  select salt.smallIcon.texture).ToList();
				return list.ToArray();
			}
		}


		public void MakeInitialAtlas(TMP_SpriteAsset to_copy)
        {
            spriteAsset = UnityEngine.Object.Instantiate(to_copy);
			spriteAsset.name = AtlasName;
			spriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(AtlasName);
			//spriteAsset.spriteInfoList.Clear();
		}


		public new void GenerateAtlasIfNecessary()
        {

			Debug.Log("Generate Atlas Begin");
			
			atlasTexture = new Texture2D(0, 0, TextureFormat.ARGB32, false, false);
			
			Debug.Log("Packing textures");
			var rects = atlasTexture.PackTextures(AtlasTextures, 2, MaxSize, false);

			
			int i = 0;


			List<ModSalt> list = (from salt in SaltFactory.allSalts
								  where salt.smallIcon != null
								  select salt).ToList();

			List<AtlasElement> elist = new List<AtlasElement>();

			foreach (Salt salt in list)
            {
				Debug.Log(salt.name);

				var element = new AtlasElement();
				element.name = salt.name;
				
				var rect = rects[i];
				element.rect = rect;


				float scaleW = (float)atlasTexture.width;
				float scaleH = (float)atlasTexture.height;

				var pixelRect = new Rect(rect.x * scaleW, rect.y * scaleH, rect.width * scaleW, rect.height * scaleH);

				element.tmpSprite = new TMP_Sprite();
				element.tmpSprite.name = element.name;
				element.tmpSprite.x = pixelRect.x;
				element.tmpSprite.y = pixelRect.y;
				element.tmpSprite.width = pixelRect.width;
				element.tmpSprite.height = pixelRect.height;
				element.tmpSprite.xAdvance = pixelRect.width;
				element.tmpSprite.xOffset = -2f;
				element.tmpSprite.yOffset = pixelRect.height * 0.8f;
				element.tmpSprite.id = i;
				element.tmpSprite.hashCode = TMP_TextUtilities.GetSimpleHashCode(element.tmpSprite.name);


				elist.Add(element);
				i++;
            }
			
			atlasTexture.Apply(false, false);
			File.WriteAllBytes("test_atlas.png", atlasTexture.EncodeToPNG());
			tmpSprites = elist.ConvertAll<TMP_Sprite>(e => e.tmpSprite);
			
			
			
	
		}

		public void PopulateSpriteAtlas()
        {
			spriteAsset.spriteSheet = atlasTexture;
			spriteAsset.spriteInfoList.AddRange(tmpSprites);
			
			//spriteAsset.UpdateLookupTables();


		}*/
	}

	/*
	class AtlasElement
    {
		public string name;

		public Rect rect;
		public TMP_Sprite tmpSprite;
	}
	*/

}
