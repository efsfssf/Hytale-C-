using System;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Data.Items;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Map;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.InterfaceRenderPreview
{
	// Token: 0x0200092E RID: 2350
	internal class EntityModelPreview : Preview
	{
		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x0010F58D File Offset: 0x0010D78D
		public override ModelRenderer ModelRenderer
		{
			get
			{
				return this._modelRenderer;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x060047B1 RID: 18353 RVA: 0x0010F595 File Offset: 0x0010D795
		public override AnimatedBlockRenderer AnimatedBlockRenderer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0010F598 File Offset: 0x0010D798
		public EntityModelPreview(Model model, string itemInHand, GameInstance gameInstance) : base(gameInstance)
		{
			this.UpdateModelRenderer(model, itemInHand);
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0010F5AC File Offset: 0x0010D7AC
		public override void UpdateRenderer()
		{
			this.UpdateModelRenderer();
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x0010F5B6 File Offset: 0x0010D7B6
		public virtual void UpdateModelRenderer()
		{
			this.UpdateModelRenderer(null, null);
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0010F5C4 File Offset: 0x0010D7C4
		public void UpdateModelRenderer(Model model, string itemInHand)
		{
			bool flag = this._modelRenderer != null;
			if (flag)
			{
				this._modelRenderer.Dispose();
				this._modelRenderer = null;
			}
			bool flag2 = model != null;
			if (flag2)
			{
				this._model = model;
				this._itemInHand = itemInHand;
			}
			bool flag3 = this._model == null || this._model.Model_ == null || this._model.Texture == null;
			if (!flag3)
			{
				string hash;
				BlockyModel blockyModel;
				bool flag4 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(this._model.Model_, ref hash) || !this._gameInstance.EntityStoreModule.GetModel(hash, out blockyModel);
				if (!flag4)
				{
					string key;
					bool flag5 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(this._model.Texture, ref key);
					if (!flag5)
					{
						CharacterPartStore characterPartStore = this._gameInstance.App.CharacterPartStore;
						Point offset;
						bool flag6 = characterPartStore.ImageLocations.TryGetValue(this._model.Texture, out offset);
						byte atlasIndex;
						if (flag6)
						{
							atlasIndex = 2;
						}
						else
						{
							bool flag7 = !this._gameInstance.EntityStoreModule.ImageLocations.TryGetValue(key, out offset);
							if (flag7)
							{
								return;
							}
							atlasIndex = 1;
						}
						BlockyModel blockyModel2 = blockyModel.Clone();
						blockyModel2.SetAtlasIndex(atlasIndex);
						blockyModel2.OffsetUVs(offset);
						CharacterPartGradientSet characterPartGradientSet;
						CharacterPartTintColor characterPartTintColor;
						bool flag8 = this._model.GradientSet != null && this._model.GradientId != null && characterPartStore.GradientSets.TryGetValue(this._model.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.TryGetValue(this._model.GradientId, out characterPartTintColor);
						if (flag8)
						{
							blockyModel2.SetGradientId(characterPartTintColor.GradientId);
						}
						bool flag9 = this._model.Attachments != null;
						if (flag9)
						{
							foreach (ModelAttachment modelAttachment in this._model.Attachments)
							{
								BlockyModel blockyModel3;
								byte value;
								Point value2;
								bool flag10 = !this.LoadAttachmentModel(modelAttachment.Model, modelAttachment.Texture, out blockyModel3, out value, out value2);
								if (!flag10)
								{
									bool flag11 = modelAttachment.GradientSet != null && modelAttachment.GradientId != null && characterPartStore.GradientSets.TryGetValue(modelAttachment.GradientSet, out characterPartGradientSet) && characterPartGradientSet.Gradients.TryGetValue(modelAttachment.GradientId, out characterPartTintColor);
									if (flag11)
									{
										blockyModel3.GradientId = characterPartTintColor.GradientId;
									}
									blockyModel2.Attach(blockyModel3, this._gameInstance.EntityStoreModule.NodeNameManager, new byte?(value), new Point?(value2), -1);
								}
							}
						}
						bool flag12 = this._itemInHand != null;
						if (flag12)
						{
							ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem(this._itemInHand);
							bool flag13 = item != null;
							if (flag13)
							{
								this.AttachItem(item, blockyModel2, CharacterPartStore.RightAttachmentNodeNameId);
							}
						}
						this._modelRenderer = new ModelRenderer(blockyModel2, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, 0U, true);
						this._modelRenderer.UpdatePose();
						this._modelRenderer.SendDataToGPU();
					}
				}
			}
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0010F90C File Offset: 0x0010DB0C
		private void AttachItem(ClientItemBase item, BlockyModel parentModel, int defaultTargetNodeNameId)
		{
			BlockyModel blockyModel = (item.BlockId != 0) ? this._gameInstance.MapModule.ClientBlockTypes[item.BlockId].FinalBlockyModel : item.Model;
			int forcedTargetNodeNameId = (item.Armor != null) ? defaultTargetNodeNameId : ((blockyModel.RootNodes.Count == 1 && blockyModel.AllNodes[blockyModel.RootNodes[0]].IsPiece) ? blockyModel.AllNodes[blockyModel.RootNodes[0]].NameId : defaultTargetNodeNameId);
			parentModel.Attach(blockyModel, this._gameInstance.EntityStoreModule.NodeNameManager, null, null, forcedTargetNodeNameId);
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0010F9CC File Offset: 0x0010DBCC
		private bool LoadAttachmentModel(string modelPath, string texturePath, out BlockyModel model, out byte atlasIndex, out Point uvOffset)
		{
			model = null;
			atlasIndex = 0;
			uvOffset = Point.Zero;
			bool flag = modelPath == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._gameInstance.App.CharacterPartStore.Models.TryGetValue("Common/" + modelPath, out model);
				if (!flag2)
				{
					string hash;
					bool flag3 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(modelPath, ref hash) || !this._gameInstance.EntityStoreModule.GetModel(hash, out model);
					if (flag3)
					{
						return false;
					}
				}
				bool flag4 = texturePath == null;
				if (flag4)
				{
					result = false;
				}
				else
				{
					bool flag5 = this._gameInstance.App.CharacterPartStore.ImageLocations.TryGetValue(texturePath, out uvOffset);
					if (flag5)
					{
						atlasIndex = 2;
					}
					else
					{
						string key;
						bool flag6 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(texturePath, ref key);
						if (flag6)
						{
							return false;
						}
						bool flag7 = !this._gameInstance.EntityStoreModule.ImageLocations.TryGetValue(key, out uvOffset);
						if (flag7)
						{
							return false;
						}
						atlasIndex = 1;
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0010FAF4 File Offset: 0x0010DCF4
		private void AttachItem(BlockyModel model, ClientItemBase item, int defaultTargetAttachmentNameId)
		{
			Entity.EntityItem entityItem = new Entity.EntityItem(this._gameInstance);
			BlockyModel blockyModel = (item.BlockId != 0) ? this._gameInstance.MapModule.ClientBlockTypes[item.BlockId].FinalBlockyModel : item.Model;
			bool flag = item.Armor == null && blockyModel.RootNodes.Count == 1 && blockyModel.AllNodes[blockyModel.RootNodes[0]].IsPiece;
			if (flag)
			{
				entityItem.TargetNodeNameId = blockyModel.AllNodes[blockyModel.RootNodes[0]].NameId;
				entityItem.SetRootOffsets(Vector3.Negate(blockyModel.AllNodes[blockyModel.RootNodes[0]].Position), Quaternion.Inverse(blockyModel.AllNodes[blockyModel.RootNodes[0]].Orientation));
			}
			else
			{
				entityItem.TargetNodeNameId = defaultTargetAttachmentNameId;
			}
			bool flag2 = !model.NodeIndicesByNameId.TryGetValue(entityItem.TargetNodeNameId, out entityItem.TargetNodeIndex);
			if (flag2)
			{
				entityItem.TargetNodeIndex = 0;
			}
			entityItem.ModelRenderer = new ModelRenderer(blockyModel, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, 0U, true);
			BlockyAnimation animation = (item.BlockId != 0) ? this._gameInstance.MapModule.ClientBlockTypes[item.BlockId].BlockyAnimation : ((item != null) ? item.Animation : null);
			entityItem.ModelRenderer.SetSlotAnimation(0, animation, true, 1f, 0f, 0f, null, false);
		}

		// Token: 0x04002409 RID: 9225
		private Model _model;

		// Token: 0x0400240A RID: 9226
		private string _itemInHand;

		// Token: 0x0400240B RID: 9227
		private ModelRenderer _modelRenderer;
	}
}
