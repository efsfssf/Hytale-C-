using System;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008EB RID: 2283
	internal class ScreenEffectRenderer : Disposable
	{
		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06004382 RID: 17282 RVA: 0x000D68E4 File Offset: 0x000D4AE4
		// (set) Token: 0x06004383 RID: 17283 RVA: 0x000D68EC File Offset: 0x000D4AEC
		public bool IsScreenEffectTextureLoading { get; private set; } = false;

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06004384 RID: 17284 RVA: 0x000D68F5 File Offset: 0x000D4AF5
		// (set) Token: 0x06004385 RID: 17285 RVA: 0x000D68FD File Offset: 0x000D4AFD
		public bool HasTexture { get; private set; } = false;

		// Token: 0x06004386 RID: 17286 RVA: 0x000D6908 File Offset: 0x000D4B08
		public ScreenEffectRenderer(Engine engine)
		{
			this._engine = engine;
			this.ScreenEffectTexture = this._engine.Graphics.GL.GenTexture();
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x000D6958 File Offset: 0x000D4B58
		protected override void DoDispose()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			gl.DeleteTexture(this.ScreenEffectTexture);
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x000D6984 File Offset: 0x000D4B84
		public unsafe void Initialize()
		{
			GLFunctions gl = this._engine.Graphics.GL;
			gl.BindTexture(GL.TEXTURE_2D, this.ScreenEffectTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 10497);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 10497);
			byte[] array;
			byte* value;
			if ((array = this._engine.Graphics.TransparentPixel) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, 1, 1, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			array = null;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x000D6A78 File Offset: 0x000D4C78
		public unsafe void RequestTextureUpdate(string targetScreenEffectTextureChecksum, bool forceUpdate = false)
		{
			bool flag = !forceUpdate && targetScreenEffectTextureChecksum == this._currentScreenEffectTextureChecksum;
			if (!flag)
			{
				this._currentScreenEffectTextureChecksum = targetScreenEffectTextureChecksum;
				GLFunctions gl = this._engine.Graphics.GL;
				bool flag2 = targetScreenEffectTextureChecksum == null;
				if (flag2)
				{
					this.IsScreenEffectTextureLoading = false;
					gl.BindTexture(GL.TEXTURE_2D, this.ScreenEffectTexture);
					byte[] array;
					byte* value;
					if ((array = this._engine.Graphics.TransparentPixel) == null || array.Length == 0)
					{
						value = null;
					}
					else
					{
						value = &array[0];
					}
					gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, 1, 1, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
					array = null;
					this.HasTexture = false;
				}
				else
				{
					this.IsScreenEffectTextureLoading = true;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						try
						{
							Image image = new Image(AssetManager.GetAssetUsingHash(targetScreenEffectTextureChecksum, false));
							this._engine.RunOnMainThread(this, delegate
							{
								gl.BindTexture(GL.TEXTURE_2D, this.ScreenEffectTexture);
								byte[] array2;
								byte* value2;
								if ((array2 = image.Pixels) == null || array2.Length == 0)
								{
									value2 = null;
								}
								else
								{
									value2 = &array2[0];
								}
								gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, image.Width, image.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value2));
								array2 = null;
								this.IsScreenEffectTextureLoading = false;
								this.HasTexture = true;
							}, false, false);
						}
						catch (Exception exception)
						{
							ScreenEffectRenderer.Logger.Error(exception, "Failed to load screen effect: " + AssetManager.GetAssetLocalPathUsingHash(targetScreenEffectTextureChecksum));
						}
					});
				}
			}
		}

		// Token: 0x04002143 RID: 8515
		private readonly Engine _engine;

		// Token: 0x04002144 RID: 8516
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002145 RID: 8517
		public readonly GLTexture ScreenEffectTexture;

		// Token: 0x04002148 RID: 8520
		private string _currentScreenEffectTextureChecksum;

		// Token: 0x04002149 RID: 8521
		public Vector4 Color = Vector4.Zero;
	}
}
