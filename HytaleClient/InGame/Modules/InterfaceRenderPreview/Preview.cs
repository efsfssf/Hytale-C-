using System;
using HytaleClient.Core;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Map;
using HytaleClient.Math;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.InGame.Modules.InterfaceRenderPreview
{
	// Token: 0x02000931 RID: 2353
	internal abstract class Preview : Disposable
	{
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x00110840 File Offset: 0x0010EA40
		// (set) Token: 0x060047CE RID: 18382 RVA: 0x00110848 File Offset: 0x0010EA48
		public Rectangle Viewport { get; private set; }

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x060047CF RID: 18383 RVA: 0x00110851 File Offset: 0x0010EA51
		// (set) Token: 0x060047D0 RID: 18384 RVA: 0x00110859 File Offset: 0x0010EA59
		public Matrix ProjectionMatrix { get; private set; }

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x060047D1 RID: 18385
		public abstract ModelRenderer ModelRenderer { get; }

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x060047D2 RID: 18386
		public abstract AnimatedBlockRenderer AnimatedBlockRenderer { get; }

		// Token: 0x060047D3 RID: 18387 RVA: 0x00110862 File Offset: 0x0010EA62
		protected Preview(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x0011089C File Offset: 0x0010EA9C
		public void SetBaseParams(InterfaceRenderPreviewModule.PreviewParams parameters)
		{
			this._isOrtho = parameters.Ortho;
			this._translate = new Vector3(parameters.Translation[0], parameters.Translation[1], (parameters.Translation.Length >= 2) ? parameters.Translation[2] : 0f);
			this._rotation = new Vector3(parameters.Rotation[0], parameters.Rotation[1], parameters.Rotation[2]);
			this._scale = parameters.Scale;
			this._isRotatable = parameters.Rotatable;
			this.Viewport = new Rectangle(parameters.Viewport.X, parameters.Viewport.Y, parameters.Viewport.Width, parameters.Viewport.Height);
			this._lerpModelAngleY = this._rotation.Y;
			bool flag = parameters.ZoomRange != null;
			if (flag)
			{
				this._minZoom = parameters.ZoomRange[0];
				this._maxZoom = parameters.ZoomRange[1];
			}
			else
			{
				this._minZoom = (this._maxZoom = -1f);
			}
			float aspectRatio = (float)this.Viewport.Width / (float)this.Viewport.Height;
			bool isOrtho = this._isOrtho;
			if (isOrtho)
			{
				this.ProjectionMatrix = Matrix.CreateTranslation(0f, 0f, -500f) * Matrix.CreateOrthographic(1f, (float)this.Viewport.Height / (float)this.Viewport.Width, 0.1f, 1000f);
			}
			else
			{
				this._translate.Z = this._translate.Z - 1.5f;
				Matrix projectionMatrix;
				this._gameInstance.Engine.Graphics.CreatePerspectiveMatrix(0.7853982f, aspectRatio, 0.1f, 1000f, out projectionMatrix);
				this.ProjectionMatrix = projectionMatrix;
			}
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x00110A70 File Offset: 0x0010EC70
		public void OnUserInput(SDL.SDL_Event evt)
		{
			bool flag = this._minZoom > 0f && this._maxZoom > 0f;
			switch (evt.type)
			{
			case SDL.SDL_EventType.SDL_MOUSEMOTION:
			{
				bool flag2 = !this._isRotatable && !flag;
				if (!flag2)
				{
					this._isMouseOver = this.IsInsideViewport(evt.motion.x, evt.motion.y);
					bool isMouseDragging = this._isMouseDragging;
					if (isMouseDragging)
					{
						this._rotation.Y = this._dragStartModelAngleY + (float)(evt.motion.x - this._dragStartMouseX);
					}
				}
				break;
			}
			case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
			{
				bool flag3 = !this._isRotatable && !flag;
				if (!flag3)
				{
					bool flag4 = evt.button.button == 1 && this.IsInsideViewport(evt.button.x, evt.button.y);
					if (flag4)
					{
						this._isMouseDragging = true;
						this._dragStartMouseX = evt.button.x;
						this._dragStartModelAngleY = this._rotation.Y;
					}
				}
				break;
			}
			case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
			{
				bool flag5 = !this._isRotatable && !flag;
				if (!flag5)
				{
					bool flag6 = evt.button.button == 1 && this._isMouseDragging;
					if (flag6)
					{
						this._isMouseDragging = false;
					}
				}
				break;
			}
			case SDL.SDL_EventType.SDL_MOUSEWHEEL:
			{
				bool flag7 = this._isMouseOver && flag && evt.wheel.y != 0;
				if (flag7)
				{
					this._zoom = MathHelper.Clamp(this._zoom + (float)evt.wheel.y / 120f, this._minZoom, this._maxZoom);
				}
				break;
			}
			}
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x00110C4C File Offset: 0x0010EE4C
		private bool IsInsideViewport(int x, int y)
		{
			Engine engine = this._gameInstance.Engine;
			return this.Viewport.Contains(engine.Window.TransformSDLToViewportCoords(x, y));
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x00110C85 File Offset: 0x0010EE85
		public void Update(float deltaTime)
		{
			this._lerpModelAngleY = MathHelper.Lerp(this._lerpModelAngleY, this._rotation.Y, MathHelper.Min(1f, 10f * deltaTime));
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00110CB8 File Offset: 0x0010EEB8
		public virtual void PrepareModelMatrix(ref Matrix modelMatrix)
		{
			Matrix matrix = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this._lerpModelAngleY), MathHelper.ToRadians(this._rotation.X), MathHelper.ToRadians(this._rotation.Z));
			Matrix matrix2 = Matrix.CreateScale(this._scale * this._zoom / 32f);
			Matrix matrix3 = Matrix.CreateTranslation(this._translate);
			modelMatrix = matrix * matrix3 * matrix2;
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00110D30 File Offset: 0x0010EF30
		public virtual void PrepareForDraw(ref int blockyModelDrawTaskCount, ref int animatedBlockDrawTaskCount, ref InterfaceRenderPreviewModule.BlockyModelDrawTask[] blockyModelDrawTasks, ref InterfaceRenderPreviewModule.AnimatedBlockDrawTask[] animatedBlockDrawTasks)
		{
			bool flag = this.AnimatedBlockRenderer != null;
			if (flag)
			{
				ArrayUtils.GrowArrayIfNecessary<InterfaceRenderPreviewModule.AnimatedBlockDrawTask>(ref animatedBlockDrawTasks, animatedBlockDrawTaskCount, 10);
				int num = animatedBlockDrawTaskCount;
				animatedBlockDrawTasks[num].Viewport = this.Viewport;
				animatedBlockDrawTasks[num].ProjectionMatrix = this.ProjectionMatrix;
				this.PrepareModelMatrix(ref animatedBlockDrawTasks[num].ModelMatrix);
				animatedBlockDrawTasks[num].AnimationData = this.AnimatedBlockRenderer.NodeBuffer;
				animatedBlockDrawTasks[num].AnimationDataOffset = this.AnimatedBlockRenderer.NodeBufferOffset;
				animatedBlockDrawTasks[num].AnimationDataSize = this.AnimatedBlockRenderer.NodeCount * 64;
				animatedBlockDrawTasks[num].VertexArray = this.AnimatedBlockRenderer.VertexArray;
				animatedBlockDrawTasks[num].DataCount = this.AnimatedBlockRenderer.IndicesCount;
				animatedBlockDrawTaskCount++;
			}
			else
			{
				bool flag2 = this.ModelRenderer != null;
				if (flag2)
				{
					ArrayUtils.GrowArrayIfNecessary<InterfaceRenderPreviewModule.BlockyModelDrawTask>(ref blockyModelDrawTasks, blockyModelDrawTaskCount, 10);
					int num2 = blockyModelDrawTaskCount;
					blockyModelDrawTasks[num2].Viewport = this.Viewport;
					blockyModelDrawTasks[num2].ProjectionMatrix = this.ProjectionMatrix;
					this.PrepareModelMatrix(ref blockyModelDrawTasks[num2].ModelMatrix);
					blockyModelDrawTasks[num2].AnimationData = this.ModelRenderer.NodeBuffer;
					blockyModelDrawTasks[num2].AnimationDataOffset = this.ModelRenderer.NodeBufferOffset;
					blockyModelDrawTasks[num2].AnimationDataSize = this.ModelRenderer.NodeCount * 64;
					blockyModelDrawTasks[num2].VertexArray = this.ModelRenderer.VertexArray;
					blockyModelDrawTasks[num2].DataCount = this.ModelRenderer.IndicesCount;
					blockyModelDrawTaskCount++;
				}
			}
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x00110F04 File Offset: 0x0010F104
		protected override void DoDispose()
		{
			AnimatedBlockRenderer animatedBlockRenderer = this.AnimatedBlockRenderer;
			if (animatedBlockRenderer != null)
			{
				animatedBlockRenderer.Dispose();
			}
			ModelRenderer modelRenderer = this.ModelRenderer;
			if (modelRenderer != null)
			{
				modelRenderer.Dispose();
			}
		}

		// Token: 0x060047DB RID: 18395
		public abstract void UpdateRenderer();

		// Token: 0x0400241E RID: 9246
		private float _scale;

		// Token: 0x0400241F RID: 9247
		private Vector3 _translate;

		// Token: 0x04002420 RID: 9248
		private Vector3 _rotation;

		// Token: 0x04002421 RID: 9249
		private float _minZoom = -1f;

		// Token: 0x04002422 RID: 9250
		private float _maxZoom = -1f;

		// Token: 0x04002423 RID: 9251
		private float _zoom = 1f;

		// Token: 0x04002424 RID: 9252
		private bool _isOrtho;

		// Token: 0x04002425 RID: 9253
		private bool _isRotatable = true;

		// Token: 0x04002426 RID: 9254
		private bool _isMouseOver;

		// Token: 0x04002427 RID: 9255
		private float _lerpModelAngleY;

		// Token: 0x04002428 RID: 9256
		private float _dragStartModelAngleY;

		// Token: 0x04002429 RID: 9257
		private bool _isMouseDragging;

		// Token: 0x0400242A RID: 9258
		private int _dragStartMouseX;

		// Token: 0x0400242B RID: 9259
		protected GameInstance _gameInstance;
	}
}
