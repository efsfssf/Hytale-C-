using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Previews
{
	// Token: 0x02000B9C RID: 2972
	internal abstract class RendererPreviewElement : Element
	{
		// Token: 0x06005BCE RID: 23502 RVA: 0x001CC4AF File Offset: 0x001CA6AF
		public RendererPreviewElement(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06005BCF RID: 23503 RVA: 0x001CC4D0 File Offset: 0x001CA6D0
		protected override void OnMounted()
		{
			this._renderTarget = new RenderTarget(1, 1, "RendererPreviewElement");
			this._renderTarget.AddTexture(RenderTarget.Target.Depth, GL.DEPTH24_STENCIL8, GL.DEPTH_STENCIL, GL.UNSIGNED_INT_24_8, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, false, false, 1);
			this._renderTarget.AddTexture(RenderTarget.Target.Color0, GL.RGBA8, GL.RGBA, GL.UNSIGNED_BYTE, GL.LINEAR, GL.LINEAR, GL.CLAMP_TO_EDGE, false, false, 1);
			this._renderTarget.FinalizeSetup();
			this._isFrontPressed = false;
			this._isBackPressed = false;
			this._isLeftPressed = false;
			this._isRightPressed = false;
			this._isUpPressed = false;
			this._isDownPressed = false;
			this._isMiddleMouseButtonPressed = false;
			this.UpdateViewMatrices();
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005BD0 RID: 23504 RVA: 0x001CC5AC File Offset: 0x001CA7AC
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			TextureArea textureArea = this._textureArea;
			if (textureArea != null)
			{
				textureArea.Texture.Dispose();
			}
			this._textureArea = null;
			this._renderTarget.Dispose();
		}

		// Token: 0x06005BD1 RID: 23505 RVA: 0x001CC600 File Offset: 0x001CA800
		protected override void LayoutSelf()
		{
			base.LayoutSelf();
			int width = base.AnchoredRectangle.Width;
			int height = base.AnchoredRectangle.Height;
			this._renderTarget.Resize(width, height, false);
			TextureArea textureArea = this._textureArea;
			if (textureArea != null)
			{
				textureArea.Texture.Dispose();
			}
			Texture texture = new Texture(Texture.TextureTypes.Texture2D);
			texture.CreateTexture2D(width, height, null, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this._textureArea = new TextureArea(texture, 0, 0, width, height, 1);
			float aspectRatio = (float)width / (float)height;
			bool flag = this.CameraType == CameraType.Camera3D;
			if (flag)
			{
				this.Desktop.Graphics.CreatePerspectiveMatrix(0.7853982f, aspectRatio, 0.1f, 1000f, out this._projectionMatrix);
			}
			else
			{
				this._projectionMatrix = Matrix.CreateTranslation(0f, 0f, -500f) * Matrix.CreateOrthographic(1f, (float)height / (float)width, 0.1f, 1000f);
			}
			this.UpdateViewMatrices();
		}

		// Token: 0x06005BD2 RID: 23506 RVA: 0x001CC718 File Offset: 0x001CA918
		public Vector2 GetProjectedMousePosition2D(Point mousePosition)
		{
			bool flag = this.CameraType != CameraType.Camera2D;
			if (flag)
			{
				throw new Exception("GetProjectedMousePosition2D can only be used with a Camera2D type defined.");
			}
			return new Vector2
			{
				X = ((float)(mousePosition.X - base.AnchoredRectangle.Left - base.AnchoredRectangle.Width / 2) + this.CameraPosition.X) / this.CameraScale,
				Y = ((float)(mousePosition.Y - base.AnchoredRectangle.Top - base.AnchoredRectangle.Height / 2) - this.CameraPosition.Y) / this.CameraScale
			};
		}

		// Token: 0x06005BD3 RID: 23507 RVA: 0x001CC7CC File Offset: 0x001CA9CC
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			bool flag = keyCode == SDL.SDL_Keycode.SDLK_z || keyCode == SDL.SDL_Keycode.SDLK_w;
			if (flag)
			{
				this._isFrontPressed = true;
			}
			else
			{
				bool flag2 = keyCode == SDL.SDL_Keycode.SDLK_s;
				if (flag2)
				{
					this._isBackPressed = true;
				}
				else
				{
					bool flag3 = keyCode == SDL.SDL_Keycode.SDLK_q || keyCode == SDL.SDL_Keycode.SDLK_a;
					if (flag3)
					{
						this._isLeftPressed = true;
					}
					else
					{
						bool flag4 = keyCode == SDL.SDL_Keycode.SDLK_d;
						if (flag4)
						{
							this._isRightPressed = true;
						}
						else
						{
							bool flag5 = keyCode == SDL.SDL_Keycode.SDLK_SPACE;
							if (flag5)
							{
								this._isUpPressed = true;
							}
							else
							{
								bool flag6 = keyCode == SDL.SDL_Keycode.SDLK_c;
								if (flag6)
								{
									this._isDownPressed = true;
								}
								else
								{
									bool flag7 = keyCode == SDL.SDL_Keycode.SDLK_LSHIFT;
									if (flag7)
									{
										this.NeedsRendering = true;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005BD4 RID: 23508 RVA: 0x001CC870 File Offset: 0x001CAA70
		protected internal override void OnKeyUp(SDL.SDL_Keycode keyCode)
		{
			bool flag = keyCode == SDL.SDL_Keycode.SDLK_z || keyCode == SDL.SDL_Keycode.SDLK_w;
			if (flag)
			{
				this._isFrontPressed = false;
			}
			else
			{
				bool flag2 = keyCode == SDL.SDL_Keycode.SDLK_s;
				if (flag2)
				{
					this._isBackPressed = false;
				}
				else
				{
					bool flag3 = keyCode == SDL.SDL_Keycode.SDLK_q || keyCode == SDL.SDL_Keycode.SDLK_a;
					if (flag3)
					{
						this._isLeftPressed = false;
					}
					else
					{
						bool flag4 = keyCode == SDL.SDL_Keycode.SDLK_d;
						if (flag4)
						{
							this._isRightPressed = false;
						}
						else
						{
							bool flag5 = keyCode == SDL.SDL_Keycode.SDLK_SPACE;
							if (flag5)
							{
								this._isUpPressed = false;
							}
							else
							{
								bool flag6 = keyCode == SDL.SDL_Keycode.SDLK_c;
								if (flag6)
								{
									this._isDownPressed = false;
								}
								else
								{
									bool flag7 = keyCode == SDL.SDL_Keycode.SDLK_LSHIFT;
									if (flag7)
									{
										this.NeedsRendering = true;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005BD5 RID: 23509 RVA: 0x001CC914 File Offset: 0x001CAB14
		public override Element HitTest(Point position)
		{
			bool flag = !base.Visible || !this._rectangleAfterPadding.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06005BD6 RID: 23510 RVA: 0x001CC94C File Offset: 0x001CAB4C
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this.Desktop.FocusElement(this, false);
			bool flag = (long)evt.Button == 1L;
			if (flag)
			{
				bool flag2 = evt.Clicks == 1;
				if (flag2)
				{
					Action onLeftClickDown = this.OnLeftClickDown;
					if (onLeftClickDown != null)
					{
						onLeftClickDown();
					}
				}
			}
			else
			{
				bool flag3 = (long)evt.Button == 3L;
				if (flag3)
				{
					bool flag4 = evt.Clicks == 1;
					if (flag4)
					{
						Action onRightClickDown = this.OnRightClickDown;
						if (onRightClickDown != null)
						{
							onRightClickDown();
						}
					}
				}
				else
				{
					bool flag5 = (long)evt.Button == 2L;
					if (flag5)
					{
						this._isMiddleMouseButtonPressed = true;
					}
				}
			}
			this._previousMousePosition = this.Desktop.MousePosition;
		}

		// Token: 0x06005BD7 RID: 23511 RVA: 0x001CC9F8 File Offset: 0x001CABF8
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button == 1L;
			if (flag)
			{
				Action onLeftClickUp = this.OnLeftClickUp;
				if (onLeftClickUp != null)
				{
					onLeftClickUp();
				}
			}
			else
			{
				bool flag2 = (long)evt.Button == 3L;
				if (flag2)
				{
					Action onRightClickUp = this.OnRightClickUp;
					if (onRightClickUp != null)
					{
						onRightClickUp();
					}
				}
				else
				{
					bool flag3 = (long)evt.Button == 2L;
					if (flag3)
					{
						this._isMiddleMouseButtonPressed = false;
					}
				}
			}
		}

		// Token: 0x06005BD8 RID: 23512 RVA: 0x001CCA68 File Offset: 0x001CAC68
		protected internal override bool OnMouseWheel(Point offset)
		{
			bool flag = this.Desktop.IsCtrlKeyDown || !this.EnableCameraScaleControls;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.CameraType == CameraType.Camera3D;
				if (!flag2)
				{
					Vector2 projectedMousePosition2D = this.GetProjectedMousePosition2D(this.Desktop.MousePosition);
					this.CameraScale = MathHelper.Clamp(this.CameraScale * (1f + (float)offset.Y * 0.2f), 0.1f, 10f);
					Vector2 projectedMousePosition2D2 = this.GetProjectedMousePosition2D(this.Desktop.MousePosition);
					this.CameraPosition.X = this.CameraPosition.X + (projectedMousePosition2D.X - projectedMousePosition2D2.X) * this.CameraScale;
					this.CameraPosition.Y = this.CameraPosition.Y - (projectedMousePosition2D.Y - projectedMousePosition2D2.Y) * this.CameraScale;
				}
				this.UpdateViewMatrices();
				result = true;
			}
			return result;
		}

		// Token: 0x06005BD9 RID: 23513 RVA: 0x001CCB58 File Offset: 0x001CAD58
		protected override void OnMouseEnter()
		{
			Action onMousePositionChanged = this.OnMousePositionChanged;
			if (onMousePositionChanged != null)
			{
				onMousePositionChanged();
			}
			this.NeedsRendering = true;
		}

		// Token: 0x06005BDA RID: 23514 RVA: 0x001CCB74 File Offset: 0x001CAD74
		protected override void OnMouseLeave()
		{
			Action onMouseLeaved = this.OnMouseLeaved;
			if (onMouseLeaved != null)
			{
				onMouseLeaved();
			}
			this.NeedsRendering = true;
		}

		// Token: 0x06005BDB RID: 23515 RVA: 0x001CCB90 File Offset: 0x001CAD90
		protected override void OnMouseMove()
		{
			Point point = this.Desktop.MousePosition - this._previousMousePosition;
			bool isMiddleMouseButtonPressed = this._isMiddleMouseButtonPressed;
			if (isMiddleMouseButtonPressed)
			{
				bool flag = this.CameraType == CameraType.Camera3D;
				if (flag)
				{
					bool flag2 = this.EnableCameraOrientationControls && point != Point.Zero;
					if (flag2)
					{
						this.CameraOrientation.Yaw = this.CameraOrientation.Yaw - (float)point.X * 0.01f;
						this.CameraOrientation.Pitch = this.CameraOrientation.Pitch - (float)point.Y * 0.01f;
						this.UpdateViewMatrices();
					}
				}
				else
				{
					bool enableCameraPositionControls = this.EnableCameraPositionControls;
					if (enableCameraPositionControls)
					{
						Vector2 projectedMousePosition2D = this.GetProjectedMousePosition2D(this._previousMousePosition);
						Vector2 projectedMousePosition2D2 = this.GetProjectedMousePosition2D(this.Desktop.MousePosition);
						this.CameraPosition.X = this.CameraPosition.X + (projectedMousePosition2D.X - projectedMousePosition2D2.X) * this.CameraScale;
						this.CameraPosition.Y = this.CameraPosition.Y - (projectedMousePosition2D.Y - projectedMousePosition2D2.Y) * this.CameraScale;
						this.UpdateViewMatrices();
					}
				}
				this._previousMousePosition = this.Desktop.MousePosition;
			}
			bool flag3 = point != Point.Zero;
			if (flag3)
			{
				Action onMousePositionChanged = this.OnMousePositionChanged;
				if (onMousePositionChanged != null)
				{
					onMousePositionChanged();
				}
			}
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x001CCCF1 File Offset: 0x001CAEF1
		protected override void PrepareForDrawSelf()
		{
			this.Desktop.Batcher2D.RequestDrawTexture(this._textureArea.Texture, this._textureArea.Rectangle, this._anchoredRectangle, UInt32Color.White);
		}

		// Token: 0x06005BDD RID: 23517 RVA: 0x001CCD28 File Offset: 0x001CAF28
		private void UpdateCamera3D(float deltaTime)
		{
			Vector3 vector = default(Vector3);
			bool isFrontPressed = this._isFrontPressed;
			if (isFrontPressed)
			{
				vector.Z += 1f;
			}
			bool isBackPressed = this._isBackPressed;
			if (isBackPressed)
			{
				vector.Z -= 1f;
			}
			bool isLeftPressed = this._isLeftPressed;
			if (isLeftPressed)
			{
				vector.X += 1f;
			}
			bool isRightPressed = this._isRightPressed;
			if (isRightPressed)
			{
				vector.X -= 1f;
			}
			bool isUpPressed = this._isUpPressed;
			if (isUpPressed)
			{
				vector.Y += 1f;
			}
			bool isDownPressed = this._isDownPressed;
			if (isDownPressed)
			{
				vector.Y -= 1f;
			}
			bool flag = vector != Vector3.Zero;
			if (flag)
			{
				Quaternion rotation = Quaternion.CreateFromYawPitchRoll(3.1415927f + this.CameraOrientation.Yaw, -this.CameraOrientation.Pitch, this.CameraOrientation.Roll);
				Vector3 value = Vector3.Transform(vector * deltaTime * 5f, rotation);
				this.CameraPosition += value;
				this.UpdateViewMatrices();
			}
		}

		// Token: 0x06005BDE RID: 23518 RVA: 0x001CCE58 File Offset: 0x001CB058
		private void UpdateCamera2D(float deltaTime)
		{
			bool flag = !this._isMiddleMouseButtonPressed;
			if (flag)
			{
				Vector3 vector = default(Vector3);
				bool isFrontPressed = this._isFrontPressed;
				if (isFrontPressed)
				{
					vector.Y += 1f;
				}
				bool isBackPressed = this._isBackPressed;
				if (isBackPressed)
				{
					vector.Y -= 1f;
				}
				bool isLeftPressed = this._isLeftPressed;
				if (isLeftPressed)
				{
					vector.X -= 1f;
				}
				bool isRightPressed = this._isRightPressed;
				if (isRightPressed)
				{
					vector.X += 1f;
				}
				bool flag2 = vector != Vector3.Zero;
				if (flag2)
				{
					this.CameraPosition += vector * 200f * deltaTime;
					this.UpdateViewMatrices();
				}
			}
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x001CCF28 File Offset: 0x001CB128
		public void UpdateViewMatrices()
		{
			this.ViewMatrix = Matrix.CreateFromYawPitchRoll(-this.CameraOrientation.Yaw, -this.CameraOrientation.Pitch, -this.CameraOrientation.Roll) * Matrix.CreateTranslation(-this.CameraPosition) * Matrix.CreateScale(this.CameraScale);
			this.ViewProjectionMatrix = this.ViewMatrix * this._projectionMatrix;
			this.NeedsRendering = true;
			Action onCameraMoved = this.OnCameraMoved;
			if (onCameraMoved != null)
			{
				onCameraMoved();
			}
		}

		// Token: 0x06005BE0 RID: 23520 RVA: 0x001CCFBC File Offset: 0x001CB1BC
		protected virtual void Animate(float deltaTime)
		{
			bool enableCameraPositionControls = this.EnableCameraPositionControls;
			if (enableCameraPositionControls)
			{
				bool flag = this.CameraType == CameraType.Camera3D;
				if (flag)
				{
					this.UpdateCamera3D(deltaTime);
				}
				else
				{
					this.UpdateCamera2D(deltaTime);
				}
			}
			bool needsRendering = this.NeedsRendering;
			if (needsRendering)
			{
				this.NeedsRendering = false;
				this._textureArea.Texture.UpdateTexture2D(this.RenderIntoRgbaByteArray());
			}
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x001CD020 File Offset: 0x001CB220
		protected byte[] RenderIntoRgbaByteArray()
		{
			GLFunctions gl = this.Desktop.Graphics.GL;
			this._renderTarget.Bind(false, true);
			PatchStyle background = this.Background;
			UInt32Color uint32Color = (background != null) ? background.Color : UInt32Color.Transparent;
			float red = (float)((byte)(uint32Color.ABGR & 255U)) / 255f;
			float green = (float)((byte)(uint32Color.ABGR >> 8) & byte.MaxValue) / 255f;
			float blue = (float)((byte)(uint32Color.ABGR >> 16) & byte.MaxValue) / 255f;
			float alpha = (float)((byte)(uint32Color.ABGR >> 24) & byte.MaxValue) / 255f;
			gl.ClearColor(red, green, blue, alpha);
			gl.Clear((GL)17664U);
			Action renderScene = this.RenderScene;
			if (renderScene != null)
			{
				renderScene();
			}
			this._renderTarget.Unbind();
			return this._renderTarget.ReadPixels(1, GL.RGBA, false);
		}

		// Token: 0x04003968 RID: 14696
		[UIMarkupProperty]
		public CameraType CameraType;

		// Token: 0x04003969 RID: 14697
		[UIMarkupProperty]
		public bool EnableCameraPositionControls = true;

		// Token: 0x0400396A RID: 14698
		[UIMarkupProperty]
		public bool EnableCameraOrientationControls = true;

		// Token: 0x0400396B RID: 14699
		[UIMarkupProperty]
		public bool EnableCameraScaleControls = true;

		// Token: 0x0400396C RID: 14700
		public Vector3 CameraPosition;

		// Token: 0x0400396D RID: 14701
		public Vector3 CameraOrientation;

		// Token: 0x0400396E RID: 14702
		public float CameraScale;

		// Token: 0x0400396F RID: 14703
		public Matrix ViewMatrix;

		// Token: 0x04003970 RID: 14704
		public Matrix ViewProjectionMatrix;

		// Token: 0x04003971 RID: 14705
		public Action OnLeftClickDown;

		// Token: 0x04003972 RID: 14706
		public Action OnLeftClickUp;

		// Token: 0x04003973 RID: 14707
		public Action OnRightClickDown;

		// Token: 0x04003974 RID: 14708
		public Action OnRightClickUp;

		// Token: 0x04003975 RID: 14709
		public Action OnMousePositionChanged;

		// Token: 0x04003976 RID: 14710
		public Action OnMouseLeaved;

		// Token: 0x04003977 RID: 14711
		public Action OnCameraMoved;

		// Token: 0x04003978 RID: 14712
		public bool NeedsRendering;

		// Token: 0x04003979 RID: 14713
		public Action RenderScene;

		// Token: 0x0400397A RID: 14714
		protected RenderTarget _renderTarget;

		// Token: 0x0400397B RID: 14715
		private TextureArea _textureArea;

		// Token: 0x0400397C RID: 14716
		private bool _isFrontPressed;

		// Token: 0x0400397D RID: 14717
		private bool _isBackPressed;

		// Token: 0x0400397E RID: 14718
		private bool _isLeftPressed;

		// Token: 0x0400397F RID: 14719
		private bool _isRightPressed;

		// Token: 0x04003980 RID: 14720
		private bool _isUpPressed;

		// Token: 0x04003981 RID: 14721
		private bool _isDownPressed;

		// Token: 0x04003982 RID: 14722
		private Point _previousMousePosition;

		// Token: 0x04003983 RID: 14723
		private bool _isMiddleMouseButtonPressed;

		// Token: 0x04003984 RID: 14724
		public Matrix _projectionMatrix;
	}
}
