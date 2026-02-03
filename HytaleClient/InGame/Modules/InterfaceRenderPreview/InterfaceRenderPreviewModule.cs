using System;
using System.Collections.Generic;
using HytaleClient.Application;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Protocol;
using SDL2;

namespace HytaleClient.InGame.Modules.InterfaceRenderPreview
{
	// Token: 0x0200092F RID: 2351
	internal class InterfaceRenderPreviewModule : Module
	{
		// Token: 0x060047B9 RID: 18361 RVA: 0x0010FC94 File Offset: 0x0010DE94
		public InterfaceRenderPreviewModule(GameInstance gameInstance, bool useFXAA = false) : base(gameInstance)
		{
			PostEffectProgram inventoryPostEffectProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.InventoryPostEffectProgram;
			this._postEffectRenderer = new PostEffectRenderer(gameInstance.Engine.Graphics, gameInstance.Engine.Profiling, inventoryPostEffectProgram);
			this._useFXAA = useFXAA;
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			int width = this._gameInstance.Engine.Window.Viewport.Width;
			int height = this._gameInstance.Engine.Window.Viewport.Height;
			this._inventoryRenderTarget = new RenderTarget(width, height, "_inventoryRenderTarget");
			this._inventoryRenderTarget.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this._inventoryRenderTarget.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this._inventoryRenderTarget.FinalizeSetup();
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x0010FDDC File Offset: 0x0010DFDC
		protected override void DoDispose()
		{
			this._inventoryRenderTarget.Dispose();
			foreach (Preview preview in this._previews.Values)
			{
				preview.Dispose();
			}
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x0010FE44 File Offset: 0x0010E044
		public void Resize(int width, int height)
		{
			this._inventoryRenderTarget.Resize(width, height, false);
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x0010FE58 File Offset: 0x0010E058
		public void OnUserInput(SDL.SDL_Event evt)
		{
			bool flag = !this.ArePreviewsEnabled();
			if (!flag)
			{
				foreach (Preview preview in this._previews.Values)
				{
					preview.OnUserInput(evt);
				}
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0010FEC4 File Offset: 0x0010E0C4
		public void HandleAssetsChanged()
		{
			foreach (Preview preview in this._previews.Values)
			{
				preview.UpdateRenderer();
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0010FF20 File Offset: 0x0010E120
		public void RemovePreview(int id)
		{
			Preview preview;
			bool flag = this._previews.TryGetValue(id, out preview);
			if (flag)
			{
				preview.Dispose();
				this._previews.Remove(id);
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0010FF58 File Offset: 0x0010E158
		public void AddEntityModelPreview(InterfaceRenderPreviewModule.ModelPreviewParams parameters)
		{
			Model model = new Model();
			model.Model_ = parameters.Model;
			model.Texture = parameters.Texture;
			bool flag = parameters.Attachments != null;
			if (flag)
			{
				model.Attachments = new ModelAttachment[parameters.Attachments.Length];
				for (int i = 0; i < model.Attachments.Length; i++)
				{
					model.Attachments[i] = new ModelAttachment(parameters.Attachments[i][0], parameters.Attachments[i][1], parameters.Attachments[i][2], parameters.Attachments[i][3]);
				}
			}
			Preview preview;
			bool flag2 = this._previews.TryGetValue(parameters.Id, out preview);
			if (flag2)
			{
				((EntityModelPreview)preview).UpdateModelRenderer(model, parameters.ItemInHand);
			}
			else
			{
				EntityModelPreview value = new EntityModelPreview(model, parameters.ItemInHand, this._gameInstance);
				this._previews.Add(parameters.Id, value);
			}
			this._previews[parameters.Id].SetBaseParams(parameters);
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x00110068 File Offset: 0x0010E268
		public void AddItemPreview(InterfaceRenderPreviewModule.ItemPreviewParams parameters)
		{
			bool flag = this._previews.ContainsKey(parameters.Id);
			if (flag)
			{
				((ItemPreview)this._previews[parameters.Id]).UpdateItemRenderer(parameters.ItemId);
			}
			else
			{
				ItemPreview value = new ItemPreview(parameters.ItemId, this._gameInstance);
				this._previews.Add(parameters.Id, value);
			}
			this._previews[parameters.Id].SetBaseParams(parameters);
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x001100F0 File Offset: 0x0010E2F0
		public void AddCharacterPreview(InterfaceRenderPreviewModule.PreviewParams parameters)
		{
			bool flag = this._previews.ContainsKey(parameters.Id);
			if (flag)
			{
				((CharacterPreview)this._previews[parameters.Id]).SetBaseParams(parameters);
			}
			else
			{
				CharacterPreview characterPreview = new CharacterPreview(this._gameInstance);
				characterPreview.SetBaseParams(parameters);
				this._previews.Add(parameters.Id, characterPreview);
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x00110160 File Offset: 0x0010E360
		public void Update(float deltaTime)
		{
			foreach (Preview preview in this._previews.Values)
			{
				preview.Update(deltaTime);
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x001101BC File Offset: 0x0010E3BC
		public void PrepareForDraw()
		{
			this._blockyModelDrawTaskCount = 0;
			this._animatedBlockDrawTaskCount = 0;
			bool flag = !this.ArePreviewsEnabled();
			if (!flag)
			{
				foreach (Preview preview in this._previews.Values)
				{
					preview.PrepareForDraw(ref this._blockyModelDrawTaskCount, ref this._animatedBlockDrawTaskCount, ref this._blockyModelDrawTasks, ref this._animatedBlockDrawTasks);
				}
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x00110250 File Offset: 0x0010E450
		public bool ArePreviewsEnabled()
		{
			return this._gameInstance.App.InGame.CurrentOverlay == AppInGame.InGameOverlay.None;
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0011027A File Offset: 0x0010E47A
		public bool NeedsDrawing()
		{
			return this._animatedBlockDrawTaskCount > 0 || this._blockyModelDrawTaskCount > 0;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x00110294 File Offset: 0x0010E494
		public void Draw()
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			gl.ActiveTexture(GL.TEXTURE3);
			gl.BindTexture(GL.TEXTURE_2D, this._gameInstance.App.CharacterPartStore.CharacterGradientAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE2);
			gl.BindTexture(GL.TEXTURE_2D, this._gameInstance.App.CharacterPartStore.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE1);
			gl.BindTexture(GL.TEXTURE_2D, this._gameInstance.EntityStoreModule.TextureAtlas.GLTexture);
			gl.ActiveTexture(GL.TEXTURE0);
			gl.BindTexture(GL.TEXTURE_2D, this._gameInstance.MapModule.TextureAtlas.GLTexture);
			gl.Disable(GL.BLEND);
			bool useFXAA = this._useFXAA;
			if (useFXAA)
			{
				this._inventoryRenderTarget.Bind(true, false);
			}
			BlockyModelProgram blockyModelForwardProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BlockyModelForwardProgram;
			gl.UseProgram(blockyModelForwardProgram);
			blockyModelForwardProgram.NearScreendoorThreshold.SetValue(0.1f);
			int height = this._gameInstance.Engine.Window.Viewport.Height;
			for (int i = 0; i < this._blockyModelDrawTaskCount; i++)
			{
				ref InterfaceRenderPreviewModule.BlockyModelDrawTask ptr = ref this._blockyModelDrawTasks[i];
				gl.Viewport(ptr.Viewport.X, height - ptr.Viewport.Y - ptr.Viewport.Height, ptr.Viewport.Width, ptr.Viewport.Height);
				blockyModelForwardProgram.ViewProjectionMatrix.SetValue(ref ptr.ProjectionMatrix);
				blockyModelForwardProgram.ModelMatrix.SetValue(ref ptr.ModelMatrix);
				blockyModelForwardProgram.NodeBlock.SetBufferRange(ptr.AnimationData, ptr.AnimationDataOffset, (uint)ptr.AnimationDataSize);
				gl.BindVertexArray(ptr.VertexArray);
				gl.DrawElements(GL.TRIANGLES, ptr.DataCount, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
			MapBlockAnimatedProgram mapBlockAnimatedForwardProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.MapBlockAnimatedForwardProgram;
			gl.UseProgram(mapBlockAnimatedForwardProgram);
			for (int j = 0; j < this._animatedBlockDrawTaskCount; j++)
			{
				ref InterfaceRenderPreviewModule.AnimatedBlockDrawTask ptr2 = ref this._animatedBlockDrawTasks[j];
				gl.Viewport(ptr2.Viewport.X, height - ptr2.Viewport.Y - ptr2.Viewport.Height, ptr2.Viewport.Width, ptr2.Viewport.Height);
				mapBlockAnimatedForwardProgram.ViewProjectionMatrix.SetValue(ref ptr2.ProjectionMatrix);
				mapBlockAnimatedForwardProgram.ModelMatrix.SetValue(ref ptr2.ModelMatrix);
				mapBlockAnimatedForwardProgram.NodeBlock.SetBufferRange(ptr2.AnimationData, ptr2.AnimationDataOffset, (uint)ptr2.AnimationDataSize);
				gl.BindVertexArray(ptr2.VertexArray);
				gl.DrawElements(GL.TRIANGLES, ptr2.DataCount, GL.UNSIGNED_INT, (IntPtr)0);
			}
			bool useFXAA2 = this._useFXAA;
			if (useFXAA2)
			{
				this._inventoryRenderTarget.Unbind();
			}
			gl.Viewport(this._gameInstance.Engine.Window.Viewport);
			bool useFXAA3 = this._useFXAA;
			if (useFXAA3)
			{
				this._postEffectRenderer.Draw(this._inventoryRenderTarget.GetTexture(RenderTarget.Target.Color0), GLTexture.None, this._inventoryRenderTarget.Width, this._inventoryRenderTarget.Height, 1f, null);
			}
			gl.Enable(GL.BLEND);
		}

		// Token: 0x0400240C RID: 9228
		private readonly Dictionary<int, Preview> _previews = new Dictionary<int, Preview>();

		// Token: 0x0400240D RID: 9229
		private bool _useFXAA;

		// Token: 0x0400240E RID: 9230
		private RenderTarget _inventoryRenderTarget;

		// Token: 0x0400240F RID: 9231
		private const int BlockyModelTasksDefaultSize = 10;

		// Token: 0x04002410 RID: 9232
		public const int BlockyModelTasksGrowth = 10;

		// Token: 0x04002411 RID: 9233
		private InterfaceRenderPreviewModule.BlockyModelDrawTask[] _blockyModelDrawTasks = new InterfaceRenderPreviewModule.BlockyModelDrawTask[10];

		// Token: 0x04002412 RID: 9234
		private int _blockyModelDrawTaskCount;

		// Token: 0x04002413 RID: 9235
		public const int AnimatedBlockTasksDefaultSize = 10;

		// Token: 0x04002414 RID: 9236
		public const int AnimatedBlockTasksGrowth = 10;

		// Token: 0x04002415 RID: 9237
		private InterfaceRenderPreviewModule.AnimatedBlockDrawTask[] _animatedBlockDrawTasks = new InterfaceRenderPreviewModule.AnimatedBlockDrawTask[10];

		// Token: 0x04002416 RID: 9238
		private int _animatedBlockDrawTaskCount;

		// Token: 0x04002417 RID: 9239
		private PostEffectRenderer _postEffectRenderer;

		// Token: 0x02000E05 RID: 3589
		public struct BlockyModelDrawTask
		{
			// Token: 0x040044DA RID: 17626
			public Rectangle Viewport;

			// Token: 0x040044DB RID: 17627
			public Matrix ProjectionMatrix;

			// Token: 0x040044DC RID: 17628
			public Matrix ModelMatrix;

			// Token: 0x040044DD RID: 17629
			public GLBuffer AnimationData;

			// Token: 0x040044DE RID: 17630
			public uint AnimationDataOffset;

			// Token: 0x040044DF RID: 17631
			public ushort AnimationDataSize;

			// Token: 0x040044E0 RID: 17632
			public GLVertexArray VertexArray;

			// Token: 0x040044E1 RID: 17633
			public int DataCount;
		}

		// Token: 0x02000E06 RID: 3590
		public struct AnimatedBlockDrawTask
		{
			// Token: 0x040044E2 RID: 17634
			public Rectangle Viewport;

			// Token: 0x040044E3 RID: 17635
			public Matrix ProjectionMatrix;

			// Token: 0x040044E4 RID: 17636
			public Matrix ModelMatrix;

			// Token: 0x040044E5 RID: 17637
			public GLBuffer AnimationData;

			// Token: 0x040044E6 RID: 17638
			public uint AnimationDataOffset;

			// Token: 0x040044E7 RID: 17639
			public ushort AnimationDataSize;

			// Token: 0x040044E8 RID: 17640
			public GLVertexArray VertexArray;

			// Token: 0x040044E9 RID: 17641
			public int DataCount;
		}

		// Token: 0x02000E07 RID: 3591
		public class ModelPreviewParams : InterfaceRenderPreviewModule.PreviewParams
		{
			// Token: 0x040044EA RID: 17642
			public string Model;

			// Token: 0x040044EB RID: 17643
			public string Texture;

			// Token: 0x040044EC RID: 17644
			public string[][] Attachments;

			// Token: 0x040044ED RID: 17645
			public string Animation;

			// Token: 0x040044EE RID: 17646
			public string ItemInHand;
		}

		// Token: 0x02000E08 RID: 3592
		public class ItemPreviewParams : InterfaceRenderPreviewModule.PreviewParams
		{
			// Token: 0x040044EF RID: 17647
			public string ItemId;
		}

		// Token: 0x02000E09 RID: 3593
		public class PreviewParams
		{
			// Token: 0x040044F0 RID: 17648
			public int Id;

			// Token: 0x040044F1 RID: 17649
			public Rectangle Viewport;

			// Token: 0x040044F2 RID: 17650
			public bool Rotatable = true;

			// Token: 0x040044F3 RID: 17651
			public float Scale;

			// Token: 0x040044F4 RID: 17652
			public float[] Translation;

			// Token: 0x040044F5 RID: 17653
			public float[] Rotation;

			// Token: 0x040044F6 RID: 17654
			public bool Ortho;

			// Token: 0x040044F7 RID: 17655
			public float[] ZoomRange = new float[]
			{
				-1f,
				-1f
			};
		}
	}
}
