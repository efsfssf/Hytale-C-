using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics;
using HytaleClient.Math;
using HytaleClient.Utils;
using Newtonsoft.Json;
using NLog;
using Zlib;

namespace HytaleClient.Data.Characters
{
	// Token: 0x02000B56 RID: 2902
	internal class CharacterPartStore : Disposable
	{
		// Token: 0x060059A5 RID: 22949 RVA: 0x001BADFB File Offset: 0x001B8FFB
		public CharacterPartStore(GLFunctions gl)
		{
			this.CreateGPUData();
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x001BAE30 File Offset: 0x001B9030
		public void CreateGPUData()
		{
			this.TextureAtlas = new Texture(Texture.TextureTypes.Texture2D);
			this.TextureAtlas.CreateTexture2D(4096, 32, null, 0, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this.CharacterGradientAtlas = new Texture(Texture.TextureTypes.Texture2D);
			this.CharacterGradientAtlas.CreateTexture2D(256, 256, null, 5, GL.NEAREST, GL.NEAREST, GL.MIRRORED_REPEAT, GL.MIRRORED_REPEAT, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x001BAECC File Offset: 0x001B90CC
		protected override void DoDispose()
		{
			Texture textureAtlas = this.TextureAtlas;
			if (textureAtlas != null)
			{
				textureAtlas.Dispose();
			}
			this.CharacterGradientAtlas.Dispose();
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x001BAEF0 File Offset: 0x001B90F0
		private void InitializeImportantNodeNames()
		{
			this.CharacterNodeNameManager = new NodeNameManager();
			CharacterPartStore.RightAttachmentNodeNameId = this.CharacterNodeNameManager.GetOrAddNameId("R-Attachment");
			CharacterPartStore.LeftAttachmentNodeNameId = this.CharacterNodeNameManager.GetOrAddNameId("L-Attachment");
			CharacterPartStore.RightArmNameId = this.CharacterNodeNameManager.GetOrAddNameId("R-Arm");
			CharacterPartStore.LeftArmNameId = this.CharacterNodeNameManager.GetOrAddNameId("L-Arm");
			CharacterPartStore.RightForearmNameId = this.CharacterNodeNameManager.GetOrAddNameId("R-Forearm");
			CharacterPartStore.LeftForearmNameId = this.CharacterNodeNameManager.GetOrAddNameId("L-Forearm");
			CharacterPartStore.RightThighNameId = this.CharacterNodeNameManager.GetOrAddNameId("R-Thigh");
			CharacterPartStore.LeftThighNameId = this.CharacterNodeNameManager.GetOrAddNameId("L-Thigh");
			CharacterPartStore.BlockNameId = this.CharacterNodeNameManager.GetOrAddNameId("Block");
			CharacterPartStore.SideMaskNameId = this.CharacterNodeNameManager.GetOrAddNameId("SideMask");
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x001BAFDC File Offset: 0x001B91DC
		public void LoadAssets(HashSet<string> updatedCosmeticsAssets, ref bool textureAtlasNeedsUpdate, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			this.InitializeImportantNodeNames();
			this._initializationCancellationToken = cancellationToken;
			List<CharacterPartTintColor> list = this.LoadConfig<CharacterPartTintColor>("EyeColors.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.EyeColors = new Dictionary<string, CharacterPartTintColor>();
			foreach (CharacterPartTintColor characterPartTintColor in list)
			{
				this.EyeColors[characterPartTintColor.Id] = characterPartTintColor;
			}
			List<CharacterPartGradientSet> list2 = this.LoadConfig<CharacterPartGradientSet>("GradientSets.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.GradientSets = new Dictionary<string, CharacterPartGradientSet>();
			foreach (CharacterPartGradientSet characterPartGradientSet in list2)
			{
				this.GradientSets[characterPartGradientSet.Id] = characterPartGradientSet;
			}
			this.Eyebrows = this.LoadConfig<CharacterPart>("Eyebrows.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Faces = this.LoadConfig<CharacterPart>("Faces.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Eyes = this.LoadConfig<CharacterPart>("Eyes.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Ears = this.LoadConfig<CharacterPart>("Ears.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Mouths = this.LoadConfig<CharacterPart>("Mouths.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.FacialHair = this.LoadConfig<CharacterPart>("FacialHair.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Pants = this.LoadConfig<CharacterPart>("Pants.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Overpants = this.LoadConfig<CharacterPart>("Overpants.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Undertops = this.LoadConfig<CharacterPart>("Undertops.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Overtops = this.LoadConfig<CharacterPart>("Overtops.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Haircuts = this.LoadConfig<CharacterHaircutPart>("Haircuts.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Shoes = this.LoadConfig<CharacterPart>("Shoes.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.HeadAccessory = this.LoadConfig<CharacterHeadAccessoryPart>("HeadAccessory.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.FaceAccessory = this.LoadConfig<CharacterPart>("FaceAccessory.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.EarAccessory = this.LoadConfig<CharacterPart>("EarAccessory.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.Gloves = this.LoadConfig<CharacterPart>("Gloves.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			this.SkinFeatures = this.LoadConfig<CharacterPart>("SkinFeatures.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
			foreach (CharacterPart characterPart in this.Eyes)
			{
				bool flag = characterPart.Textures == null;
				if (!flag)
				{
					foreach (KeyValuePair<string, CharacterPartTexture> keyValuePair in characterPart.Textures)
					{
						CharacterPartTintColor characterPartTintColor2;
						bool flag2 = this.EyeColors.TryGetValue(keyValuePair.Key, out characterPartTintColor2);
						if (flag2)
						{
							keyValuePair.Value.BaseColor = characterPartTintColor2.BaseColor;
						}
						else
						{
							keyValuePair.Value.BaseColor = new string[]
							{
								"#000000"
							};
							CharacterPartStore.Logger.Warn<string, string>("Eye asset '{0}' has reference to an eye color that does not exist ({1})", characterPart.Id, keyValuePair.Key);
						}
					}
				}
			}
			this.Emotes = this.LoadConfig<Emote>("Emotes.json", updatedCosmeticsAssets, ref textureAtlasNeedsUpdate);
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x001BB338 File Offset: 0x001B9538
		public void PrepareGradientAtlas(out byte[][] upcomingGradientAtlasPixelsPerLevel)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			byte[] array = new byte[this.CharacterGradientAtlas.Width * this.CharacterGradientAtlas.Height * 4];
			int num = 0;
			foreach (CharacterPartGradientSet characterPartGradientSet in this.GradientSets.Values)
			{
				foreach (KeyValuePair<string, CharacterPartTintColor> keyValuePair in characterPartGradientSet.Gradients)
				{
					Debug.Assert(num < this.CharacterGradientAtlas.Height, "Maximum number of gradients reached");
					int num2 = num;
					CharacterPartTintColor value = keyValuePair.Value;
					bool flag = ((value != null) ? value.Texture : null) == null;
					if (flag)
					{
						CharacterPartStore.Logger.Error("Gradient set has invalid color: " + keyValuePair.Key);
					}
					else
					{
						try
						{
							Image image = new Image(AssetManager.GetBuiltInAsset(Path.Combine("Common", keyValuePair.Value.Texture)));
							for (int i = 0; i < 1; i++)
							{
								int dstOffset = (num2 + i) * this.CharacterGradientAtlas.Width * 4;
								Buffer.BlockCopy(image.Pixels, i * image.Width * 4, array, dstOffset, image.Width * 4);
							}
						}
						catch (Exception exception)
						{
							CharacterPartStore.Logger.Error(exception, "Failed to load gradient texture: " + keyValuePair.Value.Texture);
						}
						keyValuePair.Value.GradientId = (byte)(num + 1);
						num++;
					}
				}
			}
			upcomingGradientAtlasPixelsPerLevel = Texture.BuildMipmapPixels(array, this.CharacterGradientAtlas.Width, 0);
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x001BB560 File Offset: 0x001B9760
		public void BuildGradientTexture(byte[][] upcomingFXAtlasPixelsPerLevel)
		{
			this.CharacterGradientAtlas.UpdateTexture2DMipMaps(upcomingFXAtlasPixelsPerLevel);
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x001BB570 File Offset: 0x001B9770
		public string GetBodyModelPath(CharacterBodyType bodyType)
		{
			return "Characters/Player.blockymodel";
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x001BB588 File Offset: 0x001B9788
		public Emote GetEmote(string id)
		{
			return Enumerable.FirstOrDefault<Emote>(this.Emotes, (Emote emote) => emote.Id == id);
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x001BB5C0 File Offset: 0x001B97C0
		public CharacterPart GetDefaultPartFor(CharacterBodyType bodyType, List<CharacterPart> assets)
		{
			return Enumerable.First<CharacterPart>(assets, (CharacterPart asset) => asset.DefaultFor == bodyType);
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x001BB5F4 File Offset: 0x001B97F4
		public CharacterPartId GetDefaultPartIdFor(CharacterBodyType bodyType, List<CharacterPart> assets)
		{
			CharacterPart defaultPartFor = this.GetDefaultPartFor(bodyType, assets);
			bool flag = defaultPartFor.Variants != null;
			CharacterPartId result;
			if (flag)
			{
				bool flag2 = defaultPartFor.GradientSet != null;
				if (flag2)
				{
					result = new CharacterPartId(defaultPartFor.Id, Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(defaultPartFor.Variants).Key, Enumerable.First<KeyValuePair<string, CharacterPartTintColor>>(this.GradientSets[defaultPartFor.GradientSet].Gradients).Key);
				}
				else
				{
					result = new CharacterPartId(defaultPartFor.Id, Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(defaultPartFor.Variants).Key, Enumerable.First<KeyValuePair<string, CharacterPartTexture>>(Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(defaultPartFor.Variants).Value.Textures).Key);
				}
			}
			else
			{
				bool flag3 = defaultPartFor.GradientSet != null;
				if (flag3)
				{
					result = new CharacterPartId(defaultPartFor.Id, Enumerable.First<KeyValuePair<string, CharacterPartTintColor>>(this.GradientSets[defaultPartFor.GradientSet].Gradients).Key);
				}
				else
				{
					result = new CharacterPartId(defaultPartFor.Id, Enumerable.First<KeyValuePair<string, CharacterPartTexture>>(defaultPartFor.Textures).Key);
				}
			}
			return result;
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x001BB724 File Offset: 0x001B9924
		public CharacterPart GetFacialHair(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.FacialHair, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x001BB75C File Offset: 0x001B995C
		public CharacterPart GetMouth(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Mouths, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x001BB794 File Offset: 0x001B9994
		public CharacterPart GetFace(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Faces, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x001BB7CC File Offset: 0x001B99CC
		public CharacterPart GetEyes(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Eyes, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x001BB804 File Offset: 0x001B9A04
		public CharacterPart GetEars(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Ears, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x001BB83C File Offset: 0x001B9A3C
		public CharacterPart GetEyebrows(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Eyebrows, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x001BB874 File Offset: 0x001B9A74
		public CharacterHaircutPart GetHaircut(string id)
		{
			return Enumerable.FirstOrDefault<CharacterHaircutPart>(this.Haircuts, (CharacterHaircutPart asset) => asset.Id == id);
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x001BB8AC File Offset: 0x001B9AAC
		public CharacterPart GetPants(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Pants, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x001BB8E4 File Offset: 0x001B9AE4
		public CharacterPart GetOverpants(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Overpants, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x001BB91C File Offset: 0x001B9B1C
		public CharacterPart GetOvertop(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Overtops, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x001BB954 File Offset: 0x001B9B54
		public CharacterPart GetUndertop(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Undertops, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x001BB98C File Offset: 0x001B9B8C
		public CharacterPart GetShoes(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Shoes, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x001BB9C4 File Offset: 0x001B9BC4
		public CharacterHeadAccessoryPart GetHeadAccessory(string id)
		{
			return Enumerable.FirstOrDefault<CharacterHeadAccessoryPart>(this.HeadAccessory, (CharacterHeadAccessoryPart asset) => asset.Id == id);
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x001BB9FC File Offset: 0x001B9BFC
		public CharacterPart GetFaceAccessory(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.FaceAccessory, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x001BBA34 File Offset: 0x001B9C34
		public CharacterPart GetEarAccessory(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.EarAccessory, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x001BBA6C File Offset: 0x001B9C6C
		public CharacterPart GetSkinFeature(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.SkinFeatures, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x001BBAA4 File Offset: 0x001B9CA4
		public CharacterPart GetGloves(string id)
		{
			return Enumerable.FirstOrDefault<CharacterPart>(this.Gloves, (CharacterPart asset) => asset.Id == id);
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x001BBADC File Offset: 0x001B9CDC
		public List<string> GetColorOptions(CharacterPart part, string variantId = null)
		{
			List<string> list = new List<string>();
			bool flag = part.GradientSet != null;
			if (flag)
			{
				list.AddRange(this.GradientSets[part.GradientSet].Gradients.Keys);
			}
			bool flag2 = variantId != null;
			if (flag2)
			{
				bool flag3 = part.Variants[variantId].Textures != null;
				if (flag3)
				{
					list.AddRange(part.Variants[variantId].Textures.Keys);
				}
			}
			else
			{
				bool flag4 = part.Textures != null;
				if (flag4)
				{
					list.AddRange(part.Textures.Keys);
				}
			}
			return list;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x001BBB90 File Offset: 0x001B9D90
		public bool TryGetGradientByIndex(byte index, out string gradientSetId, out string gradientId)
		{
			foreach (KeyValuePair<string, CharacterPartGradientSet> keyValuePair in this.GradientSets)
			{
				foreach (KeyValuePair<string, CharacterPartTintColor> keyValuePair2 in keyValuePair.Value.Gradients)
				{
					bool flag = keyValuePair2.Value.GradientId == index;
					if (flag)
					{
						gradientSetId = keyValuePair.Key;
						gradientId = keyValuePair2.Key;
						return true;
					}
				}
			}
			gradientSetId = null;
			gradientId = null;
			return false;
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x001BBC60 File Offset: 0x001B9E60
		public bool TryGetCharacterPart(PlayerSkinProperty property, string partId, out CharacterPart characterPart)
		{
			switch (property)
			{
			case PlayerSkinProperty.Haircut:
				characterPart = Enumerable.FirstOrDefault<CharacterHaircutPart>(this.Haircuts, (CharacterHaircutPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.FacialHair:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.FacialHair, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Eyebrows:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Eyebrows, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Eyes:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Eyes, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Pants:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Pants, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Overpants:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Overpants, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Undertop:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Undertops, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Overtop:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Overtops, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Shoes:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Shoes, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.HeadAccessory:
				characterPart = Enumerable.FirstOrDefault<CharacterHeadAccessoryPart>(this.HeadAccessory, (CharacterHeadAccessoryPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.FaceAccessory:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.FaceAccessory, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.EarAccessory:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.EarAccessory, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.SkinFeature:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.SkinFeatures, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			case PlayerSkinProperty.Gloves:
				characterPart = Enumerable.FirstOrDefault<CharacterPart>(this.Gloves, (CharacterPart p) => p.Id == partId);
				goto IL_1F7;
			}
			characterPart = null;
			return false;
			IL_1F7:
			return characterPart != null;
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x001BBE70 File Offset: 0x001BA070
		public List<CharacterAttachment> GetCharacterAttachments(ClientPlayerSkin skin)
		{
			bool flag = skin == null;
			List<CharacterAttachment> result;
			if (flag)
			{
				result = new List<CharacterAttachment>();
			}
			else
			{
				string text = skin.SkinTone;
				bool flag2 = text == null || !this.GradientSets["Skin"].Gradients.ContainsKey(text);
				if (flag2)
				{
					text = Enumerable.FirstOrDefault<KeyValuePair<string, CharacterPartTintColor>>(this.GradientSets["Skin"].Gradients).Key;
				}
				List<CharacterAttachment> list = new List<CharacterAttachment>();
				string partId = (skin.BodyType == CharacterBodyType.Feminine) ? "NormalFemale" : "NormalMale";
				this.GetAttachment(list, new CharacterPartId(skin.Face, skin.SkinTone), new Func<string, CharacterPart>(this.GetFace), false);
				this.GetAttachment(list, new CharacterPartId(partId, text), new Func<string, CharacterPart>(this.GetMouth), false);
				this.GetAttachment(list, new CharacterPartId("Normal", text), new Func<string, CharacterPart>(this.GetEars), false);
				this.GetAttachment(list, skin.Eyes, new Func<string, CharacterPart>(this.GetEyes), false);
				this.GetAttachment(list, skin.SkinFeature, new Func<string, CharacterPart>(this.GetSkinFeature), false);
				this.GetAttachment(list, skin.Pants, new Func<string, CharacterPart>(this.GetPants), false);
				this.GetAttachment(list, skin.Overpants, new Func<string, CharacterPart>(this.GetOverpants), false);
				this.GetAttachment(list, skin.Shoes, new Func<string, CharacterPart>(this.GetShoes), false);
				this.GetAttachment(list, skin.Undertop, new Func<string, CharacterPart>(this.GetUndertop), false);
				this.GetAttachment(list, skin.Overtop, new Func<string, CharacterPart>(this.GetOvertop), false);
				this.GetAttachment(list, skin.Gloves, new Func<string, CharacterPart>(this.GetGloves), false);
				this.GetAttachment(list, skin.Eyebrows, new Func<string, CharacterPart>(this.GetEyebrows), false);
				this.GetAttachment(list, skin.HeadAccessory, new Func<string, CharacterPart>(this.GetHeadAccessory), false);
				this.GetAttachment(list, skin.FaceAccessory, new Func<string, CharacterPart>(this.GetFaceAccessory), false);
				this.GetAttachment(list, skin.EarAccessory, new Func<string, CharacterPart>(this.GetEarAccessory), false);
				this.GetHaircutAttachment(list, skin);
				this.GetAttachment(list, skin.FacialHair, new Func<string, CharacterPart>(this.GetFacialHair), false);
				result = list;
			}
			return result;
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x001BC0D8 File Offset: 0x001BA2D8
		public List<CharacterPart> GetParts(PlayerSkinProperty property)
		{
			switch (property)
			{
			case PlayerSkinProperty.Haircut:
				return new List<CharacterPart>(this.Haircuts);
			case PlayerSkinProperty.FacialHair:
				return this.FacialHair;
			case PlayerSkinProperty.Eyebrows:
				return this.Eyebrows;
			case PlayerSkinProperty.Eyes:
				return this.Eyes;
			case PlayerSkinProperty.Face:
				return this.Faces;
			case PlayerSkinProperty.Pants:
				return this.Pants;
			case PlayerSkinProperty.Overpants:
				return this.Overpants;
			case PlayerSkinProperty.Undertop:
				return this.Undertops;
			case PlayerSkinProperty.Overtop:
				return this.Overtops;
			case PlayerSkinProperty.Shoes:
				return this.Shoes;
			case PlayerSkinProperty.HeadAccessory:
				return new List<CharacterPart>(this.HeadAccessory);
			case PlayerSkinProperty.FaceAccessory:
				return this.FaceAccessory;
			case PlayerSkinProperty.EarAccessory:
				return this.EarAccessory;
			case PlayerSkinProperty.Gloves:
				return this.Gloves;
			}
			return null;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x001BC1C4 File Offset: 0x001BA3C4
		public List<string> GetTags(List<CharacterPart> parts)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (CharacterPart characterPart in parts)
			{
				bool flag = characterPart.Tags == null;
				if (!flag)
				{
					foreach (string text in characterPart.Tags)
					{
						hashSet.Add(text);
					}
				}
			}
			List<string> list = Enumerable.ToList<string>(hashSet);
			list.Sort();
			return list;
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x001BC26C File Offset: 0x001BA46C
		private void GetHaircutAttachment(ICollection<CharacterAttachment> attachments, ClientPlayerSkin skin)
		{
			bool flag = skin.Haircut != null;
			if (flag)
			{
				bool flag2 = skin.HeadAccessory != null;
				if (flag2)
				{
					CharacterHeadAccessoryPart headAccessory = this.GetHeadAccessory(skin.HeadAccessory.PartId);
					bool flag3 = headAccessory != null;
					if (flag3)
					{
						bool flag4 = headAccessory.HeadAccessoryType == CharacterHeadAccessoryPart.CharacterHeadAccessoryType.HalfCovering;
						if (flag4)
						{
							CharacterHaircutPart haircut = this.GetHaircut(skin.Haircut.PartId);
							bool requiresGenericHaircut = haircut.RequiresGenericHaircut;
							if (requiresGenericHaircut)
							{
								CharacterHaircutPart baseHaircut = this.GetHaircut(string.Format("Generic{0}", haircut.HairType));
								List<string> colorOptions = this.GetColorOptions(baseHaircut, null);
								this.GetAttachment(attachments, new CharacterPartId(baseHaircut.Id, colorOptions.Contains(skin.Haircut.ColorId) ? skin.Haircut.ColorId : Enumerable.First<string>(colorOptions)), (string _) => baseHaircut, false);
								return;
							}
						}
						else
						{
							bool flag5 = headAccessory.HeadAccessoryType == CharacterHeadAccessoryPart.CharacterHeadAccessoryType.FullyCovering;
							if (flag5)
							{
								this.GetAttachment(attachments, skin.Haircut, new Func<string, CharacterPart>(this.GetHaircut), true);
								return;
							}
						}
					}
				}
				this.GetAttachment(attachments, skin.Haircut, new Func<string, CharacterPart>(this.GetHaircut), false);
			}
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x001BC3CC File Offset: 0x001BA5CC
		private void GetAttachment(ICollection<CharacterAttachment> attachments, CharacterPartId assetId, Func<string, CharacterPart> getterFunc, bool usesBaseModel = false)
		{
			bool flag = assetId == null;
			if (!flag)
			{
				CharacterPart characterPart = getterFunc(assetId.PartId);
				bool flag2 = characterPart != null;
				if (flag2)
				{
					Dictionary<string, CharacterPartTexture> textures = characterPart.Textures;
					string model = characterPart.Model;
					string greyscaleTexture = characterPart.GreyscaleTexture;
					bool flag3 = characterPart.Variants != null;
					if (flag3)
					{
						CharacterPartVariant characterPartVariant;
						bool flag4 = characterPart.Variants.TryGetValue((assetId.VariantId != null) ? assetId.VariantId : Enumerable.First<KeyValuePair<string, CharacterPartVariant>>(characterPart.Variants).Key, out characterPartVariant);
						if (!flag4)
						{
							CharacterPartStore.Logger.Warn<string, string>("Invalid variant for character part {0}: {1}", characterPart.Id, assetId.VariantId);
							return;
						}
						textures = characterPartVariant.Textures;
						model = characterPartVariant.Model;
						greyscaleTexture = characterPartVariant.GreyscaleTexture;
					}
					CharacterPartTexture characterPartTexture;
					bool flag5 = textures != null && textures.TryGetValue(assetId.ColorId, out characterPartTexture);
					if (flag5)
					{
						attachments.Add(new CharacterAttachment(model, characterPartTexture.Texture, usesBaseModel, 0));
					}
					else
					{
						CharacterPartTintColor characterPartTintColor;
						bool flag6 = characterPart.GradientSet != null && this.GradientSets[characterPart.GradientSet].Gradients.TryGetValue(assetId.ColorId, out characterPartTintColor);
						if (flag6)
						{
							attachments.Add(new CharacterAttachment(model, greyscaleTexture, usesBaseModel, characterPartTintColor.GradientId));
						}
						else
						{
							CharacterPartStore.Logger.Warn<string, string>("Invalid texture for character part {0}: {1}", characterPart.Id, assetId.ColorId);
						}
					}
				}
				else
				{
					CharacterPartStore.Logger.Warn<CharacterPartId>("Invalid texture for character part {0}", assetId);
				}
			}
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x001BC55C File Offset: 0x001BA75C
		private List<T> LoadConfig<T>(string file, HashSet<string> updatedAssets, ref bool textureAtlasNeedsUpdate)
		{
			bool flag = !textureAtlasNeedsUpdate && updatedAssets.Contains(Path.Combine("CharacterCreator", file));
			if (flag)
			{
				textureAtlasNeedsUpdate = true;
			}
			return JsonConvert.DeserializeObject<List<T>>(this.GetJson(file));
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x001BC59C File Offset: 0x001BA79C
		private string GetJson(string file)
		{
			return Encoding.UTF8.GetString(AssetManager.GetBuiltInAsset("Cosmetics/CharacterCreator/" + file));
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x060059CB RID: 22987 RVA: 0x001BC5C8 File Offset: 0x001BA7C8
		// (set) Token: 0x060059CC RID: 22988 RVA: 0x001BC5D0 File Offset: 0x001BA7D0
		public Texture TextureAtlas { get; private set; }

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x060059CD RID: 22989 RVA: 0x001BC5D9 File Offset: 0x001BA7D9
		// (set) Token: 0x060059CE RID: 22990 RVA: 0x001BC5E1 File Offset: 0x001BA7E1
		public Point[] AtlasSizes { get; private set; }

		// Token: 0x060059CF RID: 22991 RVA: 0x001BC5EC File Offset: 0x001BA7EC
		public void LoadModelData(Engine engine, HashSet<string> updatedCommonAssets, bool textureAtlasNeedsUpdate)
		{
			this.IdleAnimation = new BlockyAnimation();
			BlockyAnimationInitializer.Parse(AssetManager.GetBuiltInAsset("Common/Characters/Animations/Default/Idle.blockyanim"), this.CharacterNodeNameManager, ref this.IdleAnimation);
			this.LoadModel(this.GetBodyModelPath(CharacterBodyType.Masculine));
			this.LoadModel(this.GetBodyModelPath(CharacterBodyType.Feminine));
			IEnumerable<CharacterPart> enumerable = Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(Enumerable.Concat<CharacterPart>(this.Eyebrows, this.Eyes), this.Faces), this.Ears), this.Mouths), this.FacialHair), this.Pants), this.Overpants), this.Undertops), this.Overtops), this.Haircuts), this.Shoes), this.HeadAccessory), this.FaceAccessory), this.EarAccessory), this.Gloves), this.SkinFeatures);
			List<string> list = new List<string>
			{
				"Characters/Player_Textures/Masculine_Greyscale.png",
				"Characters/Player_Textures/Feminine_Greyscale.png"
			};
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			foreach (CharacterPart characterPart in enumerable)
			{
				bool flag = characterPart.Variants != null;
				if (flag)
				{
					foreach (CharacterPartVariant characterPartVariant in characterPart.Variants.Values)
					{
						this.LoadModel(characterPartVariant.Model);
						bool flag2 = characterPartVariant.Textures != null;
						if (flag2)
						{
							foreach (CharacterPartTexture characterPartTexture in characterPartVariant.Textures.Values)
							{
								list.Add(characterPartTexture.Texture);
							}
						}
						bool flag3 = characterPartVariant.GreyscaleTexture != null;
						if (flag3)
						{
							list.Add(characterPartVariant.GreyscaleTexture);
						}
					}
				}
				else
				{
					this.LoadModel(characterPart.Model);
					bool flag4 = characterPart.Textures != null;
					if (flag4)
					{
						foreach (CharacterPartTexture characterPartTexture2 in characterPart.Textures.Values)
						{
							list.Add(characterPartTexture2.Texture);
						}
					}
					bool flag5 = characterPart.GreyscaleTexture != null;
					if (flag5)
					{
						list.Add(characterPart.GreyscaleTexture);
					}
				}
			}
			foreach (Emote emote in this.Emotes)
			{
				BlockyAnimation value = new BlockyAnimation();
				BlockyAnimationInitializer.Parse(AssetManager.GetBuiltInAsset(Path.Combine("Common", emote.Animation)), this.CharacterNodeNameManager, ref value);
				this.Animations[emote.Animation] = value;
			}
			stopwatch.Stop();
			CharacterPartStore.Logger.Info("Took {0}s to load character part models", stopwatch.Elapsed.TotalMilliseconds / 1000.0);
			bool flag6 = textureAtlasNeedsUpdate;
			bool flag7 = !flag6;
			if (flag7)
			{
				string newValue = Path.DirectorySeparatorChar.ToString();
				foreach (string text in list)
				{
					bool flag8 = updatedCommonAssets.Contains(text.Replace("/", newValue));
					if (flag8)
					{
						flag6 = true;
						break;
					}
				}
			}
			bool flag9 = !flag6;
			if (flag9)
			{
				flag6 = !File.Exists(Path.Combine(Paths.BuiltInAssets, "CharacterTextureAtlasLocations.cache"));
				for (int i = 0; i < this.TextureAtlas.MipmapLevelCount + 1; i++)
				{
					bool flag10 = !File.Exists(Path.Combine(Paths.BuiltInAssets, "CharacterTextureAtlas" + i.ToString() + ".cache"));
					if (flag10)
					{
						flag6 = true;
						break;
					}
				}
			}
			bool flag11 = flag6 && !AssetManager.IsAssetsDirectoryImmutable;
			byte[][] upcomingAtlasPixelsPerLevel;
			if (flag11)
			{
				upcomingAtlasPixelsPerLevel = this.GenerateAtlas(engine, list);
			}
			else
			{
				try
				{
					upcomingAtlasPixelsPerLevel = this.LoadCachedAtlas(engine);
				}
				catch (Exception exception)
				{
					CharacterPartStore.Logger.Error(exception, "Failed to load cached character atlas:");
					upcomingAtlasPixelsPerLevel = this.GenerateAtlas(engine, list);
				}
			}
			engine.RunOnMainThread(this, delegate
			{
				bool isCancellationRequested = this._initializationCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					this.TextureAtlas.UpdateTexture2DMipMaps(upcomingAtlasPixelsPerLevel);
				}
			}, false, false);
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x001BCB84 File Offset: 0x001BAD84
		private byte[][] GenerateAtlas(Engine engine, List<string> textures)
		{
			CharacterPartStore.Logger.Info("Re-generating character part atlas...");
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			byte[][] result;
			this.PrepareAtlas(textures, out result);
			stopwatch.Stop();
			CharacterPartStore.Logger.Info("Took {0} to regenerate character part atlas", stopwatch.Elapsed.TotalMilliseconds / 1000.0);
			return result;
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x001BCBF0 File Offset: 0x001BADF0
		private byte[][] LoadCachedAtlas(Engine engine)
		{
			CharacterPartStore.Logger.Info("Loading cached character part atlas...");
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			string[] array = File.ReadAllLines(Path.Combine(Paths.BuiltInAssets, "CharacterTextureAtlasLocations.cache"));
			int x = int.Parse(array[0].Split(new char[]
			{
				' '
			})[0]);
			int y = int.Parse(array[0].Split(new char[]
			{
				' '
			})[1]);
			this.AtlasSizes = new Point[]
			{
				new Point(x, y)
			};
			this.ImageLocations.Clear();
			for (int i = 1; i < array.Length; i++)
			{
				string text = array[i];
				bool flag = text == "";
				if (!flag)
				{
					string[] array2 = text.Split(new char[]
					{
						' '
					}, 3);
					string key = array2[2];
					this.ImageLocations.Add(key, new Point(int.Parse(array2[0]), int.Parse(array2[1])));
				}
			}
			byte[][] array3 = new byte[this.TextureAtlas.MipmapLevelCount + 1][];
			for (int j = 0; j < this.TextureAtlas.MipmapLevelCount + 1; j++)
			{
				MemoryStream memoryStream = new MemoryStream();
				byte[] array4;
				using (FileStream fileStream = File.Open(Path.Combine(Paths.BuiltInAssets, "CharacterTextureAtlas" + j.ToString() + ".cache"), FileMode.Open))
				{
					bool disableCharacterAtlasCompression = OptionsHelper.DisableCharacterAtlasCompression;
					if (disableCharacterAtlasCompression)
					{
						fileStream.CopyTo(memoryStream);
						fileStream.Close();
						memoryStream.Position = 0L;
						array4 = memoryStream.ToArray();
					}
					else
					{
						using (ZLibStream zlibStream = new ZLibStream(fileStream, 0))
						{
							zlibStream.CopyTo(memoryStream);
							zlibStream.Close();
							memoryStream.Position = 0L;
							array4 = memoryStream.ToArray();
						}
					}
				}
				array3[j] = array4;
			}
			stopwatch.Stop();
			CharacterPartStore.Logger.Info("Took {0} to load cached character part atlas", stopwatch.Elapsed.TotalMilliseconds / 1000.0);
			return array3;
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x001BCE50 File Offset: 0x001BB050
		public BlockyAnimation GetAnimation(string path)
		{
			BlockyAnimation blockyAnimation;
			bool flag = !this.Animations.TryGetValue(path, out blockyAnimation);
			BlockyAnimation result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = blockyAnimation;
			}
			return result;
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x001BCE7C File Offset: 0x001BB07C
		public BlockyModel GetAndCloneModel(string path)
		{
			BlockyModel blockyModel;
			bool flag = !this.Models.TryGetValue(path, out blockyModel);
			BlockyModel result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = blockyModel.Clone();
			}
			return result;
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x001BCEB0 File Offset: 0x001BB0B0
		private void LoadModel(string path)
		{
			try
			{
				BlockyModel value = new BlockyModel(BlockyModel.MaxNodeCount);
				BlockyModelInitializer.Parse(AssetManager.GetBuiltInAsset(Path.Combine("Common", path)), this.CharacterNodeNameManager, ref value);
				this.Models[path] = value;
			}
			catch (Exception exception)
			{
				CharacterPartStore.Logger.Error(exception, "Failed to parse model " + path);
			}
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x001BCF28 File Offset: 0x001BB128
		public void PrepareAtlas(List<string> textures, out byte[][] upcomingAtlasPixelsPerLevel)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			this.ImageLocations.Clear();
			Dictionary<string, CharacterPartStore.ModelTextureInfo> dictionary = new Dictionary<string, CharacterPartStore.ModelTextureInfo>();
			foreach (string text in textures)
			{
				bool isCancellationRequested = this._initializationCancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				CharacterPartStore.ModelTextureInfo modelTextureInfo;
				bool flag = !dictionary.TryGetValue(text, out modelTextureInfo);
				if (flag)
				{
					modelTextureInfo = new CharacterPartStore.ModelTextureInfo
					{
						Path = text
					};
					bool flag2 = Image.TryGetPngDimensions(Path.Combine(Paths.BuiltInAssets, "Common", text), out modelTextureInfo.Width, out modelTextureInfo.Height);
					if (flag2)
					{
						dictionary[text] = modelTextureInfo;
						bool flag3 = modelTextureInfo.Width % 32 != 0 || modelTextureInfo.Height % 32 != 0 || modelTextureInfo.Width < 32 || modelTextureInfo.Height < 32;
						if (flag3)
						{
							CharacterPartStore.Logger.Info<string, int, int>("Texture width/height must be a multiple of 32 and at least 32x32: {0} ({1}x{2})", text, modelTextureInfo.Width, modelTextureInfo.Height);
						}
					}
					else
					{
						CharacterPartStore.Logger.Info("Failed to get PNG dimensions for: {0}", text);
					}
				}
			}
			List<CharacterPartStore.ModelTextureInfo> list = new List<CharacterPartStore.ModelTextureInfo>(dictionary.Values);
			list.Sort((CharacterPartStore.ModelTextureInfo a, CharacterPartStore.ModelTextureInfo b) => b.Height.CompareTo(a.Height));
			Point zero = Point.Zero;
			int num = 0;
			int num2 = 512;
			foreach (CharacterPartStore.ModelTextureInfo modelTextureInfo2 in list)
			{
				bool isCancellationRequested2 = this._initializationCancellationToken.IsCancellationRequested;
				if (isCancellationRequested2)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				bool flag4 = zero.X + modelTextureInfo2.Width > this.TextureAtlas.Width;
				if (flag4)
				{
					zero.X = 0;
					zero.Y = num;
				}
				while (zero.Y + modelTextureInfo2.Height > num2)
				{
					num2 <<= 1;
				}
				this.ImageLocations.Add(modelTextureInfo2.Path, zero);
				num = Math.Max(num, zero.Y + modelTextureInfo2.Height);
				zero.X += modelTextureInfo2.Width;
			}
			this.AtlasSizes = new Point[]
			{
				new Point(this.TextureAtlas.Width, num2)
			};
			byte[] array = new byte[this.TextureAtlas.Width * num2 * 4];
			zero = Point.Zero;
			foreach (CharacterPartStore.ModelTextureInfo modelTextureInfo3 in list)
			{
				bool isCancellationRequested3 = this._initializationCancellationToken.IsCancellationRequested;
				if (isCancellationRequested3)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				try
				{
					Image image = new Image(AssetManager.GetBuiltInAsset(Path.Combine("Common", modelTextureInfo3.Path)));
					for (int i = 0; i < image.Height; i++)
					{
						Point point = this.ImageLocations[modelTextureInfo3.Path];
						int dstOffset = ((point.Y + i) * this.TextureAtlas.Width + point.X) * 4;
						Buffer.BlockCopy(image.Pixels, i * image.Width * 4, array, dstOffset, image.Width * 4);
					}
				}
				catch (Exception exception)
				{
					CharacterPartStore.Logger.Error(exception, "Failed to load model texture: " + modelTextureInfo3.Path);
				}
			}
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Paths.BuiltInAssets, "CharacterTextureAtlasLocations.cache")))
			{
				streamWriter.WriteLine(this.TextureAtlas.Width.ToString() + " " + num2.ToString());
				foreach (KeyValuePair<string, Point> keyValuePair in this.ImageLocations)
				{
					TextWriter textWriter = streamWriter;
					string[] array2 = new string[5];
					int num3 = 0;
					Point value = keyValuePair.Value;
					array2[num3] = value.X.ToString();
					array2[1] = " ";
					int num4 = 2;
					value = keyValuePair.Value;
					array2[num4] = value.Y.ToString();
					array2[3] = " ";
					array2[4] = keyValuePair.Key;
					textWriter.WriteLine(string.Concat(array2));
				}
			}
			upcomingAtlasPixelsPerLevel = Texture.BuildMipmapPixels(array, this.TextureAtlas.Width, this.TextureAtlas.MipmapLevelCount);
			for (int j = 0; j < upcomingAtlasPixelsPerLevel.Length; j++)
			{
				using (FileStream fileStream = File.Create(Path.Combine(Paths.BuiltInAssets, string.Format("CharacterTextureAtlas{0}.cache", j))))
				{
					bool disableCharacterAtlasCompression = OptionsHelper.DisableCharacterAtlasCompression;
					if (disableCharacterAtlasCompression)
					{
						fileStream.Write(upcomingAtlasPixelsPerLevel[j], 0, upcomingAtlasPixelsPerLevel[j].Length);
					}
					else
					{
						using (ZLibStream zlibStream = new ZLibStream(fileStream, 1))
						{
							zlibStream.Write(upcomingAtlasPixelsPerLevel[j], 0, upcomingAtlasPixelsPerLevel[j].Length);
						}
					}
				}
			}
		}

		// Token: 0x04003791 RID: 14225
		public static int RightAttachmentNodeNameId = 0;

		// Token: 0x04003792 RID: 14226
		public static int LeftAttachmentNodeNameId = 1;

		// Token: 0x04003793 RID: 14227
		public static int RightArmNameId = 2;

		// Token: 0x04003794 RID: 14228
		public static int LeftArmNameId = 3;

		// Token: 0x04003795 RID: 14229
		public static int RightForearmNameId = 4;

		// Token: 0x04003796 RID: 14230
		public static int LeftForearmNameId = 5;

		// Token: 0x04003797 RID: 14231
		public static int RightThighNameId = 6;

		// Token: 0x04003798 RID: 14232
		public static int LeftThighNameId = 7;

		// Token: 0x04003799 RID: 14233
		public static int BlockNameId = 8;

		// Token: 0x0400379A RID: 14234
		public static int SideMaskNameId = 9;

		// Token: 0x0400379B RID: 14235
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400379C RID: 14236
		public NodeNameManager CharacterNodeNameManager;

		// Token: 0x0400379D RID: 14237
		public Dictionary<string, CharacterPartTintColor> EyeColors;

		// Token: 0x0400379E RID: 14238
		public Dictionary<string, CharacterPartGradientSet> GradientSets;

		// Token: 0x0400379F RID: 14239
		public List<CharacterPart> FacialHair;

		// Token: 0x040037A0 RID: 14240
		public List<CharacterPart> Ears;

		// Token: 0x040037A1 RID: 14241
		public List<CharacterPart> Eyes;

		// Token: 0x040037A2 RID: 14242
		public List<CharacterPart> Mouths;

		// Token: 0x040037A3 RID: 14243
		public List<CharacterPart> Eyebrows;

		// Token: 0x040037A4 RID: 14244
		public List<CharacterPart> Faces;

		// Token: 0x040037A5 RID: 14245
		public List<CharacterHaircutPart> Haircuts;

		// Token: 0x040037A6 RID: 14246
		public List<CharacterPart> Pants;

		// Token: 0x040037A7 RID: 14247
		public List<CharacterPart> Overpants;

		// Token: 0x040037A8 RID: 14248
		public List<CharacterPart> Undertops;

		// Token: 0x040037A9 RID: 14249
		public List<CharacterPart> Overtops;

		// Token: 0x040037AA RID: 14250
		public List<CharacterPart> Shoes;

		// Token: 0x040037AB RID: 14251
		public List<CharacterHeadAccessoryPart> HeadAccessory;

		// Token: 0x040037AC RID: 14252
		public List<CharacterPart> FaceAccessory;

		// Token: 0x040037AD RID: 14253
		public List<CharacterPart> EarAccessory;

		// Token: 0x040037AE RID: 14254
		public List<CharacterPart> SkinFeatures;

		// Token: 0x040037AF RID: 14255
		public List<CharacterPart> Gloves;

		// Token: 0x040037B0 RID: 14256
		public List<Emote> Emotes;

		// Token: 0x040037B1 RID: 14257
		private CancellationToken _initializationCancellationToken;

		// Token: 0x040037B2 RID: 14258
		public const string FeminineTexture = "Characters/Player_Textures/Feminine_Greyscale.png";

		// Token: 0x040037B3 RID: 14259
		public const string MasculineTexture = "Characters/Player_Textures/Masculine_Greyscale.png";

		// Token: 0x040037B4 RID: 14260
		public const string HairGradientSetId = "Hair";

		// Token: 0x040037B5 RID: 14261
		public const string SkinGradientSetId = "Skin";

		// Token: 0x040037B6 RID: 14262
		public BlockyAnimation IdleAnimation;

		// Token: 0x040037B9 RID: 14265
		public Texture CharacterGradientAtlas;

		// Token: 0x040037BA RID: 14266
		public readonly Dictionary<string, Point> ImageLocations = new Dictionary<string, Point>();

		// Token: 0x040037BB RID: 14267
		public readonly Dictionary<string, BlockyModel> Models = new Dictionary<string, BlockyModel>();

		// Token: 0x040037BC RID: 14268
		public readonly Dictionary<string, BlockyAnimation> Animations = new Dictionary<string, BlockyAnimation>();

		// Token: 0x02000F3F RID: 3903
		private class ModelTextureInfo
		{
			// Token: 0x04004A88 RID: 19080
			public string Path;

			// Token: 0x04004A89 RID: 19081
			public int Width;

			// Token: 0x04004A8A RID: 19082
			public int Height;
		}
	}
}
