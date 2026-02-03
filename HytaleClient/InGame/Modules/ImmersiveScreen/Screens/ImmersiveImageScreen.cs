using System;
using System.Diagnostics;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Interface;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.ImmersiveScreen.Screens
{
	// Token: 0x0200093F RID: 2367
	internal class ImmersiveImageScreen : BaseImmersiveScreen
	{
		// Token: 0x060048E2 RID: 18658 RVA: 0x0011AC48 File Offset: 0x00118E48
		public ImmersiveImageScreen(GameInstance gameInstance, Vector3 blockPosition, ImmersiveView.ViewScreen screen) : base(gameInstance, blockPosition, screen)
		{
			this._missingImage = BaseInterface.MakeMissingImage("");
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			this._quadRenderer = new QuadRenderer(graphics, graphics.GPUProgramStore.BasicProgram.AttribPosition, graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			GLFunctions gl = graphics.GL;
			this._texture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this._texture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x0011AD40 File Offset: 0x00118F40
		public void SetViewData(ImmersiveView viewData)
		{
			bool flag = viewData.Painting != null;
			if (flag)
			{
				try
				{
					ImmersiveView.PaintingView painting = viewData.Painting;
					this._imageWidth = painting.Width;
					this._imageHeight = painting.Height;
					byte[] array = (byte[])painting.Data;
					bool flag2 = array == null;
					if (flag2)
					{
						array = new byte[painting.Width * painting.Height * 4];
					}
					this.UpdatePaintingImageData(array);
				}
				catch (Exception ex)
				{
					this._gameInstance.App.DevTools.Error("Immersive image screen failed to update pixels: " + ex.Message);
					this._imageWidth = this._missingImage.Width;
					this._imageHeight = this._missingImage.Height;
					this.SetPixels(this._missingImage.Pixels);
				}
			}
			else
			{
				ImmersiveView.ImageView image = viewData.Image;
				string text = (image != null) ? image.File : null;
				try
				{
					bool flag3 = text == null;
					if (flag3)
					{
						throw new Exception("No image provided.");
					}
					bool flag4 = !text.StartsWith("ImmersiveScreens/");
					if (flag4)
					{
						throw new Exception("Invalid path prefix, must start with ImmersiveScreens/");
					}
					string hash;
					bool flag5 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(text, ref hash);
					if (flag5)
					{
						throw new Exception("Asset not found");
					}
					Image image2 = new Image(AssetManager.GetAssetUsingHash(hash, false));
					this._imageWidth = image2.Width;
					this._imageHeight = image2.Height;
					this.SetPixels(image2.Pixels);
				}
				catch (Exception ex2)
				{
					this._gameInstance.App.DevTools.Error("Immersive image screen failed to load image with path " + text + ": " + ex2.Message);
					this._imageWidth = this._missingImage.Width;
					this._imageHeight = this._missingImage.Height;
					this.SetPixels(this._missingImage.Pixels);
				}
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x0011AF50 File Offset: 0x00119150
		public void UpdatePaintingImageData(byte[] data)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = data.Length != this._imageWidth * this._imageHeight * 4;
			if (flag)
			{
				throw new Exception(string.Format("Byte array does not match desired length ${0} != {1}", data.Length, this._imageWidth * this._imageHeight * 4));
			}
			this._paintingImageData = data;
			this.SetPixels(data);
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0011AFBF File Offset: 0x001191BF
		protected override void DoDispose()
		{
			this._quadRenderer.Dispose();
			this._gameInstance.Engine.Graphics.GL.DeleteTexture(this._texture);
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0011AFF0 File Offset: 0x001191F0
		private unsafe void SetPixels(byte[] pixels)
		{
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			gl.BindTexture(GL.TEXTURE_2D, this._texture);
			fixed (byte[] array = pixels)
			{
				byte* value;
				if (pixels == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, this._imageWidth, this._imageHeight, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			}
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x0011B078 File Offset: 0x00119278
		public override void Draw()
		{
			bool flag = !base.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			BasicProgram basicProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Color.AssertValue(this._gameInstance.Engine.Graphics.WhiteColor);
			basicProgram.Opacity.AssertValue(1f);
			basicProgram.MVPMatrix.SetValue(ref this._mvpMatrix);
			gl.BindTexture(GL.TEXTURE_2D, this._texture);
			this._quadRenderer.Draw();
		}

		// Token: 0x040024E4 RID: 9444
		private int _imageWidth;

		// Token: 0x040024E5 RID: 9445
		private int _imageHeight;

		// Token: 0x040024E6 RID: 9446
		private readonly QuadRenderer _quadRenderer;

		// Token: 0x040024E7 RID: 9447
		private readonly GLTexture _texture;

		// Token: 0x040024E8 RID: 9448
		private byte[] _paintingImageData;

		// Token: 0x040024E9 RID: 9449
		private readonly Image _missingImage;
	}
}
