using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data.Characters;
using HytaleClient.Data.Map;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Previews
{
	// Token: 0x02000B99 RID: 2969
	internal class BlockPreview : AssetPreview
	{
		// Token: 0x06005BC1 RID: 23489 RVA: 0x001CBA65 File Offset: 0x001C9C65
		public BlockPreview(AssetEditorOverlay assetEditorOverlay, Element parent) : base(assetEditorOverlay, parent)
		{
		}

		// Token: 0x06005BC2 RID: 23490 RVA: 0x001CBA71 File Offset: 0x001C9C71
		public void Setup(BlockType blockType, AssetEditorPreviewCameraSettings cameraSettings)
		{
			this._blockData = blockType;
			this._cameraSettings = cameraSettings;
			base.TrySetupRenderer();
		}

		// Token: 0x06005BC3 RID: 23491 RVA: 0x001CBA8C File Offset: 0x001C9C8C
		protected override bool AreMinimumRequiredAssetsAvailable()
		{
			bool flag = this._blockData.Model == null;
			TrackedAsset trackedAsset;
			return flag || (this._assetEditorOverlay.TrackedAssets.TryGetValue(AssetPathUtils.GetAssetPathWithCommon(this._blockData.Model), out trackedAsset) && trackedAsset.IsAvailable);
		}

		// Token: 0x06005BC4 RID: 23492 RVA: 0x001CBAE4 File Offset: 0x001C9CE4
		protected override void SetupModelData()
		{
			BlockType blockData = this._blockData;
			JObject modelJson = null;
			bool flag = blockData.Model != null;
			if (flag)
			{
				modelJson = (JObject)this._assetEditorOverlay.TrackedAssets[AssetPathUtils.GetAssetPathWithCommon(blockData.Model)].Data;
			}
			ClientBlockType clientBlockType = ItemPreviewUtils.ToClientBlockType(blockData, modelJson, this._assetEditorOverlay.Interface.App.CharacterPartStore.CharacterNodeNameManager);
			foreach (ClientBlockType.CubeTexture cubeTexture in clientBlockType.CubeTextures)
			{
				for (int j = 0; j < cubeTexture.Names.Length; j++)
				{
					string key = cubeTexture.Names[j];
					Point point;
					bool flag2 = !this._textureLocations.TryGetValue(key, out point);
					if (!flag2)
					{
						cubeTexture.TileLinearPositionsInAtlas[j] = point.X / 32;
					}
				}
			}
			bool flag3 = clientBlockType.BlockyTextures != null && clientBlockType.BlockyTextures.Length != 0;
			if (flag3)
			{
				Point zero;
				Point zero2;
				bool flag4 = blockData.ModelTexture_ == null || blockData.ModelTexture_.Length == 0 || !this._textureSizes.TryGetValue(blockData.ModelTexture_[0].Texture, out zero) || !this._textureLocations.TryGetValue(clientBlockType.BlockyTextures[0].Name, out zero2);
				if (flag4)
				{
					zero = Point.Zero;
					zero2 = Point.Zero;
				}
				clientBlockType.OriginalBlockyModel.OffsetUVs(zero2);
				clientBlockType.RenderedBlockyModel.PrepareUVs(clientBlockType.OriginalBlockyModel, zero, new Point(this._textureAtlas.Width, this._textureAtlas.Height));
				clientBlockType.RenderedBlockyModelTextureOrigins = new Vector2[clientBlockType.BlockyTextures.Length];
				for (int k = 0; k < clientBlockType.BlockyTextures.Length; k++)
				{
					clientBlockType.RenderedBlockyModelTextureOrigins[k] = new Vector2((float)zero2.X / 32f, 0f);
				}
			}
			Point point2;
			bool flag5 = blockData.CubeSideMaskTexture != null && this._textureLocations.TryGetValue(blockData.CubeSideMaskTexture, out point2);
			if (flag5)
			{
				clientBlockType.CubeSideMaskTextureAtlasIndex = point2.X / 32;
			}
			bool shouldRenderCube = clientBlockType.ShouldRenderCube;
			if (shouldRenderCube)
			{
				clientBlockType.FinalBlockyModel.AddMapBlockNode(clientBlockType, CharacterPartStore.BlockNameId, CharacterPartStore.SideMaskNameId, this._textureAtlas.Width);
			}
			ItemPreviewUtils.CreateBlockGeometry(clientBlockType, this._textureAtlas);
			this._model = clientBlockType.OriginalBlockyModel;
			this._blockVertexData = clientBlockType.VertexData;
		}

		// Token: 0x06005BC5 RID: 23493 RVA: 0x001CBD74 File Offset: 0x001C9F74
		protected override void GatherRequiredAssets(HashSet<string> texturePaths, HashSet<string> modelPaths)
		{
			BlockType blockData = this._blockData;
			texturePaths.Add("BlockTextures/Unknown.png");
			foreach (BlockType.BlockTextures blockTextures in blockData.CubeTextures)
			{
				bool flag = blockTextures.Back != null;
				if (flag)
				{
					texturePaths.Add(blockTextures.Back);
				}
				bool flag2 = blockTextures.Bottom != null;
				if (flag2)
				{
					texturePaths.Add(blockTextures.Bottom);
				}
				bool flag3 = blockTextures.Front != null;
				if (flag3)
				{
					texturePaths.Add(blockTextures.Front);
				}
				bool flag4 = blockTextures.Left != null;
				if (flag4)
				{
					texturePaths.Add(blockTextures.Left);
				}
				bool flag5 = blockTextures.Right != null;
				if (flag5)
				{
					texturePaths.Add(blockTextures.Right);
				}
				bool flag6 = blockTextures.Top != null;
				if (flag6)
				{
					texturePaths.Add(blockTextures.Top);
				}
			}
			bool flag7 = blockData.CubeSideMaskTexture != null && !texturePaths.Contains(blockData.CubeSideMaskTexture);
			if (flag7)
			{
				texturePaths.Add(blockData.CubeSideMaskTexture);
			}
			bool flag8 = blockData.ModelTexture_ != null;
			if (flag8)
			{
				foreach (BlockType.ModelTexture modelTexture in blockData.ModelTexture_)
				{
					bool flag9 = modelTexture == null;
					if (!flag9)
					{
						texturePaths.Add(modelTexture.Texture);
					}
				}
			}
			bool flag10 = blockData.Model != null;
			if (flag10)
			{
				modelPaths.Add(blockData.Model);
			}
		}

		// Token: 0x04003963 RID: 14691
		private BlockType _blockData;
	}
}
