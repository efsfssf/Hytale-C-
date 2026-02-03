using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Interface.Previews
{
	// Token: 0x02000B9A RID: 2970
	internal class ModelPreview : AssetPreview
	{
		// Token: 0x06005BC6 RID: 23494 RVA: 0x001CBEFD File Offset: 0x001CA0FD
		public ModelPreview(AssetEditorOverlay assetEditorOverlay, Element parent) : base(assetEditorOverlay, parent)
		{
		}

		// Token: 0x06005BC7 RID: 23495 RVA: 0x001CBF0C File Offset: 0x001CA10C
		public void Setup(Model model, AssetEditorPreviewCameraSettings cameraSettings)
		{
			bool flag = this._needsUpdateAfterRendererDisposal || this.NeedsUpdate(model);
			this._modelData = model;
			this._cameraSettings = cameraSettings;
			bool flag2 = flag;
			if (flag2)
			{
				base.TrySetupRenderer();
			}
		}

		// Token: 0x06005BC8 RID: 23496 RVA: 0x001CBF48 File Offset: 0x001CA148
		private bool NeedsUpdate(Model model)
		{
			Model modelData = this._modelData;
			bool flag = model == null && modelData == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = model != modelData && (model == null || modelData == null);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = !string.Equals(model.Model_, modelData.Model_);
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = !string.Equals(model.Texture, modelData.Texture);
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = !string.Equals(model.GradientId, modelData.GradientId);
							if (flag5)
							{
								result = true;
							}
							else
							{
								bool flag6 = !string.Equals(model.GradientSet, modelData.GradientSet);
								if (flag6)
								{
									result = true;
								}
								else
								{
									bool flag7 = model.Attachments != modelData.Attachments && (model.Attachments == null || modelData.Attachments == null);
									if (flag7)
									{
										result = true;
									}
									else
									{
										bool flag8 = model.Attachments != null;
										if (flag8)
										{
											bool flag9 = model.Attachments.Length != modelData.Attachments.Length;
											if (flag9)
											{
												return true;
											}
											for (int i = 0; i < model.Attachments.Length; i++)
											{
												ModelAttachment modelAttachment = model.Attachments[i];
												ModelAttachment modelAttachment2 = modelData.Attachments[i];
												bool flag10 = !string.Equals(modelAttachment.Model, modelAttachment2.Model);
												if (flag10)
												{
													return true;
												}
												bool flag11 = !string.Equals(modelAttachment.Texture, modelAttachment2.Texture);
												if (flag11)
												{
													return true;
												}
												bool flag12 = !string.Equals(modelAttachment.GradientId, modelAttachment2.GradientId);
												if (flag12)
												{
													return true;
												}
												bool flag13 = !string.Equals(modelAttachment.GradientSet, modelAttachment2.GradientSet);
												if (flag13)
												{
													return true;
												}
											}
										}
										result = false;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005BC9 RID: 23497 RVA: 0x001CC140 File Offset: 0x001CA340
		protected override bool AreMinimumRequiredAssetsAvailable()
		{
			TrackedAsset trackedAsset = this._assetEditorOverlay.TrackedAssets[AssetPathUtils.GetAssetPathWithCommon(this._modelData.Model_)];
			return trackedAsset.IsAvailable;
		}

		// Token: 0x06005BCA RID: 23498 RVA: 0x001CC179 File Offset: 0x001CA379
		protected override bool IsAssetValid()
		{
			return this._modelData.Model_ != null;
		}

		// Token: 0x06005BCB RID: 23499 RVA: 0x001CC18C File Offset: 0x001CA38C
		protected override void GatherRequiredAssets(HashSet<string> requiredTextures, HashSet<string> requiredModels)
		{
			requiredModels.Add(this._modelData.Model_);
			bool flag = this._modelData.Texture != null;
			if (flag)
			{
				requiredTextures.Add(this._modelData.Texture);
			}
			bool flag2 = this._modelData.Attachments != null;
			if (flag2)
			{
				foreach (ModelAttachment modelAttachment in this._modelData.Attachments)
				{
					bool flag3 = modelAttachment.Model == null;
					if (!flag3)
					{
						requiredModels.Add(modelAttachment.Model);
						bool flag4 = modelAttachment.Texture != null;
						if (flag4)
						{
							requiredTextures.Add(modelAttachment.Texture);
						}
					}
				}
			}
		}

		// Token: 0x06005BCC RID: 23500 RVA: 0x001CC244 File Offset: 0x001CA444
		protected override void SetupModelData()
		{
			AssetEditorApp app = this._assetEditorOverlay.Interface.App;
			CharacterPartStore characterPartStore = app.CharacterPartStore;
			Model modelData = this._modelData;
			bool flag = ((modelData != null) ? modelData.Model_ : null) == null;
			if (!flag)
			{
				BlockyModel blockyModel;
				Point offset;
				byte atlasIndex;
				this.TryGetModelAndTexture(modelData.Model_, modelData.Texture, out blockyModel, out offset, out atlasIndex);
				CharacterPartGradientSet characterPartGradientSet;
				CharacterPartTintColor characterPartTintColor;
				bool flag2 = modelData.GradientSet != null && modelData.GradientId != null && characterPartStore.GradientSets.TryGetValue(modelData.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.TryGetValue(modelData.GradientId, out characterPartTintColor);
				if (flag2)
				{
					blockyModel.SetGradientId(characterPartTintColor.GradientId);
				}
				blockyModel.SetAtlasIndex(atlasIndex);
				blockyModel.OffsetUVs(offset);
				bool flag3 = modelData.Attachments != null;
				if (flag3)
				{
					foreach (ModelAttachment modelAttachment in modelData.Attachments)
					{
						BlockyModel blockyModel2;
						Point value;
						byte value2;
						bool flag4 = !this.TryGetModelAndTexture(modelAttachment.Model, modelAttachment.Texture, out blockyModel2, out value, out value2);
						if (!flag4)
						{
							bool flag5 = modelAttachment.GradientSet != null && modelAttachment.GradientId != null && characterPartStore.GradientSets.TryGetValue(modelAttachment.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.TryGetValue(modelAttachment.GradientId, out characterPartTintColor);
							if (flag5)
							{
								blockyModel2.GradientId = characterPartTintColor.GradientId;
							}
							BlockyModel blockyModel3 = blockyModel;
							BlockyModel attachment = blockyModel2;
							NodeNameManager characterNodeNameManager = characterPartStore.CharacterNodeNameManager;
							Point? uvOffset = new Point?(value);
							blockyModel3.Attach(attachment, characterNodeNameManager, new byte?(value2), uvOffset, -1);
						}
					}
				}
				this._model = blockyModel;
			}
		}

		// Token: 0x06005BCD RID: 23501 RVA: 0x001CC3EC File Offset: 0x001CA5EC
		private bool TryGetModelAndTexture(string modelPath, string texturePath, out BlockyModel blockyModel, out Point uvOffset, out byte textureAtlasIndex)
		{
			uvOffset = Point.Zero;
			textureAtlasIndex = 0;
			blockyModel = null;
			bool flag = modelPath == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				TrackedAsset trackedAsset = this._assetEditorOverlay.TrackedAssets[AssetPathUtils.GetAssetPathWithCommon(modelPath)];
				bool flag2 = !trackedAsset.IsAvailable;
				if (flag2)
				{
					result = false;
				}
				else
				{
					blockyModel = new BlockyModel(BlockyModel.MaxNodeCount);
					BlockyModelInitializer.Parse((JObject)trackedAsset.Data, this._assetEditorOverlay.Interface.App.CharacterPartStore.CharacterNodeNameManager, ref blockyModel);
					Point point;
					bool flag3 = texturePath != null && this._textureLocations.TryGetValue(texturePath, out point);
					if (flag3)
					{
						uvOffset = point;
					}
					else
					{
						textureAtlasIndex = 1;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04003964 RID: 14692
		private Model _modelData;
	}
}
