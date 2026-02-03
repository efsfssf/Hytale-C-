using System;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Map;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.InterfaceRenderPreview
{
	// Token: 0x02000930 RID: 2352
	internal class ItemPreview : Preview
	{
		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x060047C7 RID: 18375 RVA: 0x0011067D File Offset: 0x0010E87D
		public override AnimatedBlockRenderer AnimatedBlockRenderer
		{
			get
			{
				return this._animatedBlockRenderer;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x00110685 File Offset: 0x0010E885
		public override ModelRenderer ModelRenderer
		{
			get
			{
				return this._modelRenderer;
			}
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x0011068D File Offset: 0x0010E88D
		public ItemPreview(string itemId, GameInstance gameInstance) : base(gameInstance)
		{
			this.UpdateItemRenderer(itemId);
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x001106A0 File Offset: 0x0010E8A0
		public override void UpdateRenderer()
		{
			this.UpdateItemRenderer(null);
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x001106AC File Offset: 0x0010E8AC
		public void UpdateItemRenderer(string itemId = null)
		{
			bool flag = this._animatedBlockRenderer != null;
			if (flag)
			{
				this._animatedBlockRenderer.Dispose();
				this._animatedBlockRenderer = null;
			}
			bool flag2 = this._modelRenderer != null;
			if (flag2)
			{
				this._modelRenderer.Dispose();
				this._modelRenderer = null;
			}
			bool flag3 = itemId != null;
			if (flag3)
			{
				this._itemId = itemId;
			}
			ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem(this._itemId);
			bool flag4 = item == null;
			if (!flag4)
			{
				this._itemScale = item.Scale;
				bool flag5 = item.BlockId != 0;
				if (flag5)
				{
					ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[item.BlockId];
					this._itemScale *= clientBlockType.BlockyModelScale;
					this._animatedBlockRenderer = new AnimatedBlockRenderer(clientBlockType.FinalBlockyModel, this._gameInstance.AtlasSizes, clientBlockType.VertexData, this._gameInstance.Engine.Graphics, true);
					this._animatedBlockRenderer.UpdatePose();
					this._animatedBlockRenderer.SendDataToGPU();
				}
				else
				{
					this._modelRenderer = new ModelRenderer(item.Model, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, 0U, true);
					this._modelRenderer.UpdatePose();
					this._modelRenderer.SendDataToGPU();
				}
			}
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x00110814 File Offset: 0x0010EA14
		public override void PrepareModelMatrix(ref Matrix modelMatrix)
		{
			base.PrepareModelMatrix(ref modelMatrix);
			Matrix matrix = Matrix.CreateScale(this._itemScale);
			Matrix.Multiply(ref modelMatrix, ref matrix, out modelMatrix);
		}

		// Token: 0x04002418 RID: 9240
		private string _itemId;

		// Token: 0x04002419 RID: 9241
		private float _itemScale;

		// Token: 0x0400241A RID: 9242
		private AnimatedBlockRenderer _animatedBlockRenderer;

		// Token: 0x0400241B RID: 9243
		private ModelRenderer _modelRenderer;
	}
}
