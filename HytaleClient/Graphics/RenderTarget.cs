using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A44 RID: 2628
	internal class RenderTarget : Disposable
	{
		// Token: 0x060052CA RID: 21194 RVA: 0x0016FEB0 File Offset: 0x0016E0B0
		public static void InitializeGL(GLFunctions gl)
		{
			RenderTarget._gl = gl;
			int[] array = new int[1];
			gl.GetIntegerv(GL.MAX_SAMPLES, array);
			RenderTarget.MaxSampleCount = array[0];
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x0016FEE5 File Offset: 0x0016E0E5
		public static void ReleaseGL()
		{
			RenderTarget._gl = null;
		}

		// Token: 0x170012D0 RID: 4816
		// (get) Token: 0x060052CC RID: 21196 RVA: 0x0016FEEE File Offset: 0x0016E0EE
		public int Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x060052CD RID: 21197 RVA: 0x0016FEF6 File Offset: 0x0016E0F6
		public int Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x060052CE RID: 21198 RVA: 0x0016FEFE File Offset: 0x0016E0FE
		public float InvWidth
		{
			get
			{
				return this._invWidth;
			}
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x060052CF RID: 21199 RVA: 0x0016FF06 File Offset: 0x0016E106
		public float InvHeight
		{
			get
			{
				return this._invHeight;
			}
		}

		// Token: 0x170012D4 RID: 4820
		// (get) Token: 0x060052D0 RID: 21200 RVA: 0x0016FF0E File Offset: 0x0016E10E
		public Vector2 Resolution
		{
			get
			{
				return new Vector2((float)this._width, (float)this._height);
			}
		}

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x060052D1 RID: 21201 RVA: 0x0016FF23 File Offset: 0x0016E123
		public Vector2 InvResolution
		{
			get
			{
				return new Vector2(this._invWidth, this._invHeight);
			}
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x0016FF38 File Offset: 0x0016E138
		public RenderTarget(int width, int height, string name)
		{
			this._width = width;
			this._height = height;
			this._invWidth = (float)(1.0 / (double)width);
			this._invHeight = (float)(1.0 / (double)height);
			this._framebuffer = RenderTarget._gl.GenFramebuffer();
			this._name = name;
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x0016FFA4 File Offset: 0x0016E1A4
		public void Resize(int width, int height, bool forceResizeExternalTextures = false)
		{
			this._width = width;
			this._height = height;
			this._invWidth = (float)(1.0 / (double)width);
			this._invHeight = (float)(1.0 / (double)height);
			for (int i = 0; i < this._targetData.Length; i++)
			{
				bool flag = GLTexture.None != this._targetData[i].Texture && (forceResizeExternalTextures || !this._targetData[i].IsTextureExternal);
				if (flag)
				{
					RenderTarget._gl.BindTexture(GL.TEXTURE_2D, this._targetData[i].Texture);
					RenderTarget._gl.TexImage2D(GL.TEXTURE_2D, 0, (int)this._targetData[i].InternalFormat, width, height, 0, this._targetData[i].Format, this._targetData[i].Type, IntPtr.Zero);
					bool flag2 = this._targetData[i].MipLevelCount > 1;
					if (flag2)
					{
						RenderTarget._gl.GenerateMipmap(GL.TEXTURE_2D);
					}
				}
			}
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001700E8 File Offset: 0x0016E2E8
		public void SetClearBits(bool clearColor, bool clearDepth, bool clearStencil)
		{
			GL gl = GL.NO_ERROR;
			if (clearColor)
			{
				gl |= GL.COLOR_BUFFER_BIT;
			}
			if (clearDepth)
			{
				gl |= GL.DEPTH_BUFFER_BIT;
			}
			if (clearStencil)
			{
				gl |= GL.STENCIL_BUFFER_BIT;
			}
			this._maskClearBits = gl;
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x00170128 File Offset: 0x0016E328
		public void AddTexture(RenderTarget.Target target, GL internalFormat, GL format, GL type, GL minFilter, GL magFilter, GL clampMode = GL.CLAMP_TO_EDGE, bool requestCompareModeForDepth = false, bool requestMipMapChain = false, int sampleCount = 1)
		{
			Debug.Assert(!requestMipMapChain || sampleCount <= 1, "RenderTarget cannot have both multisampling and mipmaps.");
			sampleCount = ((sampleCount < RenderTarget.MaxSampleCount) ? sampleCount : RenderTarget.MaxSampleCount);
			GL target2 = (sampleCount > 1) ? GL.TEXTURE_2D_MULTISAMPLE : GL.TEXTURE_2D;
			GLTexture texture = RenderTarget._gl.GenTexture();
			RenderTarget._gl.BindTexture(target2, texture);
			bool flag = sampleCount == 1;
			if (flag)
			{
				RenderTarget._gl.TexParameteri(target2, GL.TEXTURE_MIN_FILTER, (int)minFilter);
				RenderTarget._gl.TexParameteri(target2, GL.TEXTURE_MAG_FILTER, (int)magFilter);
				RenderTarget._gl.TexParameteri(target2, GL.TEXTURE_WRAP_S, (int)clampMode);
				RenderTarget._gl.TexParameteri(target2, GL.TEXTURE_WRAP_T, (int)clampMode);
			}
			bool flag2 = target == RenderTarget.Target.Depth && requestCompareModeForDepth;
			if (flag2)
			{
				RenderTarget._gl.TexParameteri(target2, GL.TEXTURE_COMPARE_MODE, 34894);
				RenderTarget._gl.TexParameteri(target2, GL.TEXTURE_COMPARE_FUNC, 515);
			}
			bool flag3 = clampMode == GL.CLAMP_TO_BORDER && sampleCount == 1;
			if (flag3)
			{
				float[] param = new float[]
				{
					1f,
					1f,
					1f,
					1f
				};
				RenderTarget._gl.TexParameterfv(target2, GL.TEXTURE_BORDER_COLOR, param);
			}
			bool flag4 = sampleCount == 1;
			if (flag4)
			{
				RenderTarget._gl.TexImage2D(GL.TEXTURE_2D, 0, (int)internalFormat, this.Width, this.Height, 0, format, type, IntPtr.Zero);
			}
			else
			{
				RenderTarget._gl.TexImage2DMultisample(GL.TEXTURE_2D_MULTISAMPLE, sampleCount, (int)internalFormat, this.Width, this.Height, false);
			}
			int num = 1;
			if (requestMipMapChain)
			{
				num += (int)Math.Floor(Math.Log((double)Math.Max(this.Width, this.Height), 2.0));
				RenderTarget._gl.GenerateMipmap(GL.TEXTURE_2D);
			}
			this.UseAsRenderTexture(texture, false, target, internalFormat, format, type, num, sampleCount);
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x00170340 File Offset: 0x0016E540
		public void UseAsRenderTexture(GLTexture texture, bool skipDispose, RenderTarget.Target target, GL internalFormat, GL format, GL type, int levelCount = 1, int sampleCount = 1)
		{
			Debug.Assert(levelCount <= 1 || sampleCount <= 1, "RenderTarget cannot have both multisampling and mipmaps.");
			this._targetData[(int)target].Texture = texture;
			this._targetData[(int)target].IsTextureExternal = skipDispose;
			this._targetData[(int)target].InternalFormat = internalFormat;
			this._targetData[(int)target].Format = format;
			this._targetData[(int)target].Type = type;
			this._targetData[(int)target].MipLevelCount = levelCount;
			this._targetData[(int)target].SampleCount = sampleCount;
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001703EC File Offset: 0x0016E5EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLTexture GetTexture(RenderTarget.Target target)
		{
			return this._targetData[(int)target].Texture;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x00170410 File Offset: 0x0016E610
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetTextureMipLevelCount(RenderTarget.Target target)
		{
			return this._targetData[(int)target].MipLevelCount;
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x00170434 File Offset: 0x0016E634
		public void FinalizeSetup()
		{
			RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, this._framebuffer);
			bool flag = GLTexture.None != this._targetData[0].Texture;
			if (flag)
			{
				GL internalFormat = this._targetData[0].InternalFormat;
				GL gl = internalFormat;
				if (gl <= GL.DEPTH24_STENCIL8)
				{
					if (gl - GL.DEPTH_COMPONENT16 <= 2U)
					{
						goto IL_A3;
					}
					if (gl != GL.DEPTH24_STENCIL8)
					{
						goto IL_BD;
					}
				}
				else
				{
					if (gl == GL.DEPTH_COMPONENT32F)
					{
						goto IL_A3;
					}
					if (gl != GL.DEPTH32F_STENCIL8)
					{
						goto IL_BD;
					}
				}
				this._maskClearBits |= GL.INVALID_ENUM;
				GL attachment = GL.DEPTH_STENCIL_ATTACHMENT;
				goto IL_C8;
				IL_A3:
				this._maskClearBits |= GL.DEPTH_BUFFER_BIT;
				attachment = GL.DEPTH_ATTACHMENT;
				goto IL_C8;
				IL_BD:
				throw new Exception("RenderTarget DepthTexture format not properly handled.");
				IL_C8:
				GL textarget = (this._targetData[0].SampleCount > 1) ? GL.TEXTURE_2D_MULTISAMPLE : GL.TEXTURE_2D;
				RenderTarget._gl.FramebufferTexture2D(GL.FRAMEBUFFER, attachment, textarget, this._targetData[0].Texture, 0);
			}
			for (int i = 1; i < 5; i++)
			{
				bool flag2 = GLTexture.None != this._targetData[i].Texture;
				if (flag2)
				{
					this._maskClearBits |= GL.COLOR_BUFFER_BIT;
					GL attachment2 = GL.COLOR_ATTACHMENT0 + (uint)(i - 1);
					GL textarget2 = (this._targetData[i].SampleCount > 1) ? GL.TEXTURE_2D_MULTISAMPLE : GL.TEXTURE_2D;
					RenderTarget._gl.FramebufferTexture2D(GL.FRAMEBUFFER, attachment2, textarget2, this._targetData[i].Texture, 0);
				}
			}
			this.SetupDrawBuffers();
			GL gl2 = RenderTarget._gl.CheckFramebufferStatus(GL.FRAMEBUFFER);
			bool flag3 = gl2 != GL.FRAMEBUFFER_COMPLETE;
			if (flag3)
			{
				throw new Exception("Incomplete Framebuffer object, status: " + gl2.ToString());
			}
			RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, GLFramebuffer.None);
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x0017065C File Offset: 0x0016E85C
		public void SetupDrawBuffers()
		{
			int num = 0;
			GL[] array = new GL[4];
			for (int i = 1; i < 5; i++)
			{
				bool flag = GLTexture.None != this._targetData[i].Texture;
				if (flag)
				{
					GL gl = GL.COLOR_ATTACHMENT0 + (uint)(i - 1);
					array[num] = gl;
					num++;
				}
			}
			RenderTarget._gl.DrawBuffers(num, array);
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x001706CE File Offset: 0x0016E8CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BindHardwareFramebuffer()
		{
			RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, GLFramebuffer.None);
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001706E8 File Offset: 0x0016E8E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Bind(bool clear, bool setupViewport)
		{
			RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, this._framebuffer);
			if (clear)
			{
				RenderTarget._gl.Clear(this._maskClearBits);
			}
			if (setupViewport)
			{
				RenderTarget._gl.Viewport(0, 0, this.Width, this.Height);
			}
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x00170748 File Offset: 0x0016E948
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Unbind()
		{
			bool forceUnbind = RenderTarget.ForceUnbind;
			if (forceUnbind)
			{
				RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, GLFramebuffer.None);
			}
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x00170774 File Offset: 0x0016E974
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyDepthStencilTo(RenderTarget destination, bool bindSource)
		{
			if (bindSource)
			{
				RenderTarget._gl.BindFramebuffer(GL.READ_FRAMEBUFFER, this._framebuffer);
			}
			RenderTarget._gl.BindFramebuffer(GL.DRAW_FRAMEBUFFER, destination._framebuffer);
			RenderTarget._gl.BlitFramebuffer(0, 0, this.Width, this.Height, 0, 0, destination.Width, destination.Height, GL.INVALID_ENUM, GL.NEAREST);
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x001707EC File Offset: 0x0016E9EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyColorTo(RenderTarget destination, GL sourceColorAttachment, GL destinationColorAttachment, GL filteringMode, bool bindSource, bool rebindSourceAfter)
		{
			if (bindSource)
			{
				RenderTarget._gl.BindFramebuffer(GL.READ_FRAMEBUFFER, this._framebuffer);
			}
			RenderTarget._gl.ReadBuffer(sourceColorAttachment);
			RenderTarget._gl.BindFramebuffer(GL.DRAW_FRAMEBUFFER, destination._framebuffer);
			GL[] array = new GL[4];
			array[0] = destinationColorAttachment;
			GL[] buffers = array;
			RenderTarget._gl.DrawBuffers(1, buffers);
			RenderTarget._gl.BlitFramebuffer(0, 0, this.Width, this.Height, 0, 0, destination.Width, destination.Height, GL.COLOR_BUFFER_BIT, filteringMode);
			if (rebindSourceAfter)
			{
				RenderTarget._gl.BindFramebuffer(GL.DRAW_FRAMEBUFFER, this._framebuffer);
			}
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x001708AC File Offset: 0x0016EAAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyColorToScreen(int screenWidth, int screenHeight, GL sourceColorAttachment, GL filteringMode, bool bindSource, bool rebindSourceAfter)
		{
			if (bindSource)
			{
				RenderTarget._gl.BindFramebuffer(GL.READ_FRAMEBUFFER, this._framebuffer);
			}
			RenderTarget._gl.ReadBuffer(sourceColorAttachment);
			RenderTarget._gl.BindFramebuffer(GL.DRAW_FRAMEBUFFER, GLFramebuffer.None);
			RenderTarget._gl.DrawBuffer(GL.BACK);
			RenderTarget._gl.BlitFramebuffer(0, 0, this.Width, this.Height, 0, 0, screenWidth, screenHeight, GL.COLOR_BUFFER_BIT, filteringMode);
			if (rebindSourceAfter)
			{
				RenderTarget._gl.BindFramebuffer(GL.DRAW_FRAMEBUFFER, this._framebuffer);
			}
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x00170956 File Offset: 0x0016EB56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveTo(RenderTarget destination, GL sourceColorAttachment, GL destinationColorAttachment, GL filteringMode, bool bindSource, bool rebindSourceAfter)
		{
			this.CopyColorTo(destination, sourceColorAttachment, destinationColorAttachment, filteringMode, bindSource, rebindSourceAfter);
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x00170969 File Offset: 0x0016EB69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveToScreen(int screenWidth, int screenHeight, GL sourceColorAttachment, GL filteringMode, bool bindSource, bool rebindSourceAfter)
		{
			this.CopyColorToScreen(screenWidth, screenHeight, sourceColorAttachment, filteringMode, bindSource, rebindSourceAfter);
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x0017097C File Offset: 0x0016EB7C
		protected override void DoDispose()
		{
			foreach (RenderTarget.TextureTargetData textureTargetData in this._targetData)
			{
				bool flag = GLTexture.None != textureTargetData.Texture && !textureTargetData.IsTextureExternal;
				if (flag)
				{
					RenderTarget._gl.DeleteTexture(textureTargetData.Texture);
				}
			}
			RenderTarget._gl.DeleteFramebuffer(this._framebuffer);
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x001709F0 File Offset: 0x0016EBF0
		public unsafe byte[] ReadPixels(int colorTarget, GL readColorType, bool bindBeforeRead = false)
		{
			if (bindBeforeRead)
			{
				RenderTarget._gl.BindFramebuffer(GL.READ_FRAMEBUFFER, this._framebuffer);
			}
			GL source = GL.COLOR_ATTACHMENT0 + (uint)(colorTarget - 1);
			RenderTarget._gl.ReadBuffer(source);
			int num = 4;
			int num2 = this.Width * this.Height;
			byte[] array = new byte[num2 * num];
			byte[] array2 = new byte[num2 * num];
			byte[] array3;
			byte* value;
			if ((array3 = array) == null || array3.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array3[0];
			}
			RenderTarget._gl.ReadPixels(0, 0, this.Width, this.Height, readColorType, this._targetData[colorTarget].Type, (IntPtr)((void*)value));
			array3 = null;
			byte* value2;
			if ((array3 = array) == null || array3.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array3[0];
			}
			for (int i = 0; i < this.Height; i++)
			{
				Marshal.Copy((IntPtr)((void*)value2) + i * this.Width * num, array2, (this.Height - i - 1) * this.Width * num, this.Width * num);
			}
			array3 = null;
			return array2;
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x00170B34 File Offset: 0x0016ED34
		public unsafe void WriteToFile(string filePath, bool colors, bool depth, bool skipBind = false)
		{
			bool flag = !skipBind;
			if (flag)
			{
				RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, this._framebuffer);
			}
			GL gl = RenderTarget._gl.CheckFramebufferStatus(GL.FRAMEBUFFER);
			bool flag2 = gl != GL.FRAMEBUFFER_COMPLETE;
			if (flag2)
			{
				throw new Exception("Incomplete Framebuffer object, status: " + gl.ToString());
			}
			int num = this.Width * this.Height;
			if (depth)
			{
				bool flag3 = GLTexture.None != this._targetData[0].Texture;
				if (flag3)
				{
					GL gl2 = GL.DEPTH_ATTACHMENT;
					int num2 = 4;
					float[] array = new float[num];
					byte[] array2 = new byte[num * num2];
					float[] data = new float[1];
					RenderTarget._gl.GetFloatv(GL.DEPTH_CLEAR_VALUE, data);
					float[] array3;
					float* value;
					if ((array3 = array) == null || array3.Length == 0)
					{
						value = null;
					}
					else
					{
						value = &array3[0];
					}
					RenderTarget._gl.ReadPixels(0, 0, this.Width, this.Height, GL.DEPTH_COMPONENT, GL.FLOAT, (IntPtr)((void*)value));
					array3 = null;
					for (int i = 0; i < this.Height; i++)
					{
						for (int j = 0; j < this.Width; j++)
						{
							int num3 = 4 * (i * this.Width + j);
							int num4 = (this.Height - i - 1) * this.Width + j;
							float num5 = array[num4];
							float num6 = 0.1f;
							float num7 = 16f;
							num5 = 2f * num6 / (num7 + num6 - num5 * (num7 - num6));
							array2[num3] = (byte)(num5 * 255f);
							array2[num3 + 1] = (byte)(num5 * num5 * num5 * 255f);
							array2[num3 + 2] = (byte)(num5 * num5 * num5 * 255f);
							array2[num3 + 3] = (byte)(num5 * 255f);
						}
					}
					new Image(this.Width, this.Height, array2).SavePNG(filePath + "_" + gl2.ToString() + ".png", 16711680U, 65280U, 255U, 0U);
				}
			}
			if (colors)
			{
				for (int k = 1; k < 5; k++)
				{
					bool flag4 = GLTexture.None != this._targetData[k].Texture;
					if (flag4)
					{
						GL gl3 = GL.COLOR_ATTACHMENT0 + (uint)(k - 1);
						byte[] pixels = this.ReadPixels(k, GL.BGRA, false);
						new Image(this.Width, this.Height, pixels).SavePNG(filePath + "_" + gl3.ToString() + ".png", 16711680U, 65280U, 255U, 4278190080U);
					}
				}
			}
			bool flag5 = !skipBind;
			if (flag5)
			{
				RenderTarget._gl.BindFramebuffer(GL.FRAMEBUFFER, GLFramebuffer.None);
			}
		}

		// Token: 0x04002DB1 RID: 11697
		private int _width;

		// Token: 0x04002DB2 RID: 11698
		private int _height;

		// Token: 0x04002DB3 RID: 11699
		private float _invWidth;

		// Token: 0x04002DB4 RID: 11700
		private float _invHeight;

		// Token: 0x04002DB5 RID: 11701
		private static int MaxSampleCount;

		// Token: 0x04002DB6 RID: 11702
		private static GLFunctions _gl;

		// Token: 0x04002DB7 RID: 11703
		private readonly GLFramebuffer _framebuffer;

		// Token: 0x04002DB8 RID: 11704
		private RenderTarget.TextureTargetData[] _targetData = new RenderTarget.TextureTargetData[5];

		// Token: 0x04002DB9 RID: 11705
		private GL _maskClearBits;

		// Token: 0x04002DBA RID: 11706
		private static bool ForceUnbind;

		// Token: 0x04002DBB RID: 11707
		private string _name;

		// Token: 0x02000EBA RID: 3770
		public enum Target
		{
			// Token: 0x040047E6 RID: 18406
			Depth,
			// Token: 0x040047E7 RID: 18407
			Color0,
			// Token: 0x040047E8 RID: 18408
			Color1,
			// Token: 0x040047E9 RID: 18409
			Color2,
			// Token: 0x040047EA RID: 18410
			Color3,
			// Token: 0x040047EB RID: 18411
			MAX
		}

		// Token: 0x02000EBB RID: 3771
		private struct TextureTargetData
		{
			// Token: 0x040047EC RID: 18412
			public GLTexture Texture;

			// Token: 0x040047ED RID: 18413
			public bool IsTextureExternal;

			// Token: 0x040047EE RID: 18414
			public GL InternalFormat;

			// Token: 0x040047EF RID: 18415
			public GL Format;

			// Token: 0x040047F0 RID: 18416
			public GL Type;

			// Token: 0x040047F1 RID: 18417
			public int MipLevelCount;

			// Token: 0x040047F2 RID: 18418
			public int SampleCount;
		}
	}
}
