using System;
using HytaleClient.Graphics;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Map;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.InterfaceRenderPreview
{
	// Token: 0x0200092D RID: 2349
	internal class CharacterPreview : Preview
	{
		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x060047A8 RID: 18344 RVA: 0x0010F16D File Offset: 0x0010D36D
		public override ModelRenderer ModelRenderer
		{
			get
			{
				return this._playerModelRenderer;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x060047A9 RID: 18345 RVA: 0x0010F175 File Offset: 0x0010D375
		public override AnimatedBlockRenderer AnimatedBlockRenderer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x0010F178 File Offset: 0x0010D378
		public CharacterPreview(GameInstance gameInstance) : base(gameInstance)
		{
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0010F1A7 File Offset: 0x0010D3A7
		protected override void DoDispose()
		{
			this.DisposeModelRenderers();
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x0010F1B4 File Offset: 0x0010D3B4
		public void DisposeModelRenderers()
		{
			ModelRenderer playerModelRenderer = this._playerModelRenderer;
			if (playerModelRenderer != null)
			{
				playerModelRenderer.Dispose();
			}
			this._playerModelRenderer = null;
			for (int i = 0; i < this._playerItemModelRenderer.Length; i++)
			{
				ModelRenderer modelRenderer = this._playerItemModelRenderer[i];
				if (modelRenderer != null)
				{
					modelRenderer.Dispose();
				}
				this._playerItemModelRenderer[i] = null;
			}
			this._playerItemCount = 0;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0010F218 File Offset: 0x0010D418
		private void PrepareItemMatrix(ref CharacterPreview.ItemModelData item, ref Matrix baseModelMatrix, ref Matrix modelMatrix)
		{
			ref AnimatedRenderer.NodeTransform ptr = ref this.ModelRenderer.NodeTransforms[item.TargetNodeIndex];
			Matrix.Compose(ptr.Orientation, ptr.Position, out modelMatrix);
			Matrix.Multiply(ref item.RootOffsetMatrix, ref modelMatrix, out modelMatrix);
			Matrix.Multiply(ref modelMatrix, ref baseModelMatrix, out modelMatrix);
			Matrix.ApplyScale(ref modelMatrix, item.Scale);
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x0010F274 File Offset: 0x0010D474
		public override void PrepareForDraw(ref int blockyModelDrawTaskCount, ref int animatedBlockDrawTaskCount, ref InterfaceRenderPreviewModule.BlockyModelDrawTask[] blockyModelDrawTasks, ref InterfaceRenderPreviewModule.AnimatedBlockDrawTask[] animatedBlockDrawTasks)
		{
			this.UpdateRenderer();
			this._playerModelRenderer.CopyAllSlotAnimations(this._gameInstance.LocalPlayer.ModelRenderer);
			this._playerModelRenderer.UpdatePose();
			this._playerModelRenderer.SendDataToGPU();
			int num = blockyModelDrawTaskCount;
			base.PrepareForDraw(ref blockyModelDrawTaskCount, ref animatedBlockDrawTaskCount, ref blockyModelDrawTasks, ref animatedBlockDrawTasks);
			for (int i = 0; i < this._playerItemCount; i++)
			{
				ArrayUtils.GrowArrayIfNecessary<InterfaceRenderPreviewModule.BlockyModelDrawTask>(ref blockyModelDrawTasks, blockyModelDrawTaskCount, 10);
				int num2 = blockyModelDrawTaskCount;
				ref CharacterPreview.ItemModelData item = ref this._playerItemModelData[i];
				ModelRenderer modelRenderer = this._playerItemModelRenderer[i];
				blockyModelDrawTasks[num2].Viewport = blockyModelDrawTasks[num].Viewport;
				blockyModelDrawTasks[num2].ProjectionMatrix = blockyModelDrawTasks[num].ProjectionMatrix;
				this.PrepareItemMatrix(ref item, ref blockyModelDrawTasks[num].ModelMatrix, ref blockyModelDrawTasks[num2].ModelMatrix);
				blockyModelDrawTasks[num2].AnimationData = modelRenderer.NodeBuffer;
				blockyModelDrawTasks[num2].AnimationDataOffset = modelRenderer.NodeBufferOffset;
				blockyModelDrawTasks[num2].AnimationDataSize = modelRenderer.NodeCount * 64;
				blockyModelDrawTasks[num2].VertexArray = modelRenderer.VertexArray;
				blockyModelDrawTasks[num2].DataCount = modelRenderer.IndicesCount;
				modelRenderer.UpdatePose();
				modelRenderer.SendDataToGPU();
				blockyModelDrawTaskCount++;
			}
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x0010F3E4 File Offset: 0x0010D5E4
		public unsafe override void UpdateRenderer()
		{
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			bool flag = this._playerModelRenderer == null || this._playerModelRenderer.Timestamp != localPlayer.ModelRenderer.Timestamp;
			if (flag)
			{
				ModelRenderer playerModelRenderer = this._playerModelRenderer;
				if (playerModelRenderer != null)
				{
					playerModelRenderer.Dispose();
				}
				this._playerModelRenderer = new ModelRenderer(localPlayer.ModelRenderer.Model, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, localPlayer.ModelRenderer.Timestamp, true);
			}
			for (int i = 0; i < localPlayer.EntityItems.Count; i++)
			{
				Entity.EntityItem entityItem = localPlayer.EntityItems[i];
				bool flag2 = this._playerItemModelRenderer[i] == null || this._playerItemModelRenderer[i].Timestamp != entityItem.ModelRenderer.Timestamp;
				if (flag2)
				{
					ModelRenderer modelRenderer = this._playerItemModelRenderer[i];
					if (modelRenderer != null)
					{
						modelRenderer.Dispose();
					}
					this._playerItemModelRenderer[i] = new ModelRenderer(entityItem.ModelRenderer.Model, this._gameInstance.AtlasSizes, this._gameInstance.Engine.Graphics, entityItem.ModelRenderer.Timestamp, true);
					this._playerItemModelData[i].Scale = entityItem.Scale;
					this._playerItemModelData[i].TargetNodeIndex = entityItem.TargetNodeIndex;
					this._playerItemModelData[i].RootOffsetMatrix = *entityItem.RootOffsetMatrix;
				}
			}
			this._playerItemCount = localPlayer.EntityItems.Count;
		}

		// Token: 0x04002404 RID: 9220
		private readonly Matrix[] _itemMatrices = new Matrix[2];

		// Token: 0x04002405 RID: 9221
		private ModelRenderer _playerModelRenderer;

		// Token: 0x04002406 RID: 9222
		private ModelRenderer[] _playerItemModelRenderer = new ModelRenderer[2];

		// Token: 0x04002407 RID: 9223
		private CharacterPreview.ItemModelData[] _playerItemModelData = new CharacterPreview.ItemModelData[2];

		// Token: 0x04002408 RID: 9224
		private int _playerItemCount;

		// Token: 0x02000E04 RID: 3588
		private struct ItemModelData
		{
			// Token: 0x040044D7 RID: 17623
			public float Scale;

			// Token: 0x040044D8 RID: 17624
			public int TargetNodeIndex;

			// Token: 0x040044D9 RID: 17625
			public Matrix RootOffsetMatrix;
		}
	}
}
