using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using HytaleClient.Core;
using HytaleClient.Graphics.Batcher2D;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using NLog;
using SDL2;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A3C RID: 2620
	public class GraphicsDevice : Disposable
	{
		// Token: 0x0600522B RID: 21035
		[DllImport("nvapi64.dll", EntryPoint = "fake")]
		private static extern int LoadNvApi64();

		// Token: 0x0600522C RID: 21036
		[DllImport("nvapi.dll", EntryPoint = "fake")]
		private static extern int LoadNvApi32();

		// Token: 0x0600522D RID: 21037 RVA: 0x00168D90 File Offset: 0x00166F90
		public static void TryForceDedicatedNvGraphics()
		{
			try
			{
				bool is64BitProcess = Environment.Is64BitProcess;
				if (is64BitProcess)
				{
					GraphicsDevice.LoadNvApi64();
				}
				else
				{
					GraphicsDevice.LoadNvApi32();
				}
			}
			catch
			{
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x0600522E RID: 21038 RVA: 0x00168DD0 File Offset: 0x00166FD0
		// (set) Token: 0x0600522F RID: 21039 RVA: 0x00168DD8 File Offset: 0x00166FD8
		public GraphicsDevice.GPUInfos GPUInfo { get; private set; }

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x00168DE1 File Offset: 0x00166FE1
		// (set) Token: 0x06005231 RID: 21041 RVA: 0x00168DE9 File Offset: 0x00166FE9
		public bool IsGPULowEnd { get; private set; }

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x00168DF2 File Offset: 0x00166FF2
		// (set) Token: 0x06005233 RID: 21043 RVA: 0x00168DFA File Offset: 0x00166FFA
		public bool SupportsDrawBuffersBlend { get; private set; }

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x00168E03 File Offset: 0x00167003
		public GraphicsDevice.GPUMemory VideoMemory
		{
			get
			{
				return this._videoMemory;
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x00168E0C File Offset: 0x0016700C
		public int UpdateAvailableGPUMemory()
		{
			int num = 0;
			GraphicsDevice.GPUVendor vendor = this.GPUInfo.Vendor;
			GraphicsDevice.GPUVendor gpuvendor = vendor;
			if (gpuvendor != GraphicsDevice.GPUVendor.Nvidia)
			{
				if (gpuvendor == GraphicsDevice.GPUVendor.AMD)
				{
					this.GL.GetIntegerv((GL)34812U, this._tmp);
					num = this._tmp[0];
				}
			}
			else
			{
				this.GL.GetIntegerv((GL)36937U, this._tmp);
				num = this._tmp[0];
			}
			this._videoMemory.AvailableNow = num;
			return num;
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x00168E98 File Offset: 0x00167098
		public void CheckGPUMemoryStatsAtStartup()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.GPUInfo.Vendor)
			{
			case GraphicsDevice.GPUVendor.Nvidia:
			{
				bool flag = SDL.SDL_GL_ExtensionSupported("GL_NVX_gpu_memory_info") != SDL.SDL_bool.SDL_TRUE;
				if (flag)
				{
					stringBuilder.AppendLine("Expected GL extension GL_NVX_gpu_memory_info is not available.");
				}
				this.GL.GetIntegerv((GL)36935U, this._tmp);
				stringBuilder.AppendLine(string.Format("Video memory max: {0}", this._tmp[0]));
				this._videoMemory.Capacity = this._tmp[0];
				this.GL.GetIntegerv((GL)36936U, this._tmp);
				stringBuilder.AppendLine(string.Format("Video memory total available: {0}", this._tmp[0]));
				this.GL.GetIntegerv((GL)36937U, this._tmp);
				stringBuilder.AppendLine(string.Format("Video memory current available: {0}", this._tmp[0]));
				this._videoMemory.AvailableAtStartup = this._tmp[0];
				this._videoMemory.AvailableNow = this._tmp[0];
				this.GL.GetIntegerv((GL)36938U, this._tmp);
				stringBuilder.AppendLine(string.Format("Video memory eviction count: {0}", this._tmp[0]));
				this.GL.GetIntegerv((GL)36939U, this._tmp);
				stringBuilder.AppendLine(string.Format("Video memory evicted: {0}", this._tmp[0]));
				break;
			}
			case GraphicsDevice.GPUVendor.AMD:
			{
				bool flag2 = SDL.SDL_GL_ExtensionSupported("GL_ATI_meminfo") != SDL.SDL_bool.SDL_TRUE;
				if (flag2)
				{
					stringBuilder.AppendLine("Expected GL extension GL_ATI_meminfo is not available.");
				}
				this.GL.GetIntegerv((GL)34812U, this._tmp);
				stringBuilder.AppendLine(string.Format("Video memory current available: {0}", this._tmp[0]));
				this._videoMemory.AvailableAtStartup = this._tmp[0];
				this._videoMemory.AvailableNow = this._tmp[0];
				break;
			}
			}
			GraphicsDevice.Logger.Info<StringBuilder>(stringBuilder);
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06005237 RID: 21047 RVA: 0x001690E9 File Offset: 0x001672E9
		// (set) Token: 0x06005238 RID: 21048 RVA: 0x001690F1 File Offset: 0x001672F1
		public int CpuCoreCount { get; private set; }

		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x001690FA File Offset: 0x001672FA
		// (set) Token: 0x0600523A RID: 21050 RVA: 0x00169102 File Offset: 0x00167302
		public int RamSize { get; private set; }

		// Token: 0x0600523B RID: 21051 RVA: 0x0016910C File Offset: 0x0016730C
		public static void SetupGLAttributes()
		{
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_RED_SIZE, 8));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_GREEN_SIZE, 8));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_BLUE_SIZE, 8));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_ALPHA_SIZE, 8));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DEPTH_SIZE, 24));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_STENCIL_SIZE, 8));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, 1));
			GraphicsDevice.AssertNoSDLError(SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_FLAGS, 1));
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x001691B0 File Offset: 0x001673B0
		private static void AssertNoSDLError(int result)
		{
			bool flag = result == -1;
			if (flag)
			{
				Exception ex = new Exception("SDL_GetError: " + SDL.SDL_GetError());
				throw ex;
			}
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x001691E0 File Offset: 0x001673E0
		public GraphicsDevice(Window window, bool allowBatcher2dToGrow = false)
		{
			this.CpuCoreCount = SDL.SDL_GetCPUCount();
			this.RamSize = SDL.SDL_GetSystemRAM();
			IntPtr value = SDL.SDL_GL_CreateContext(window.Handle);
			bool flag = value == IntPtr.Zero;
			if (flag)
			{
				throw new Exception("Could not create GL context: " + SDL.SDL_GetError());
			}
			this.GL = new GLFunctions();
			Mesh.InitializeGL(this.GL);
			MeshProcessor.InitializeGL(this.GL);
			RenderTarget.InitializeGL(this.GL);
			Uniform.InitializeGL(this.GL);
			UniformBufferObject.InitializeGL(this.GL);
			GPUTimer.InitializeGL(this.GL);
			GPUProgram.InitializeGL(this.GL);
			GPUBuffer.InitializeGL(this.GL);
			GPUBufferTexture.InitializeGL(this.GL);
			Texture.InitializeGL(this.GL);
			this.GL.Hint(HytaleClient.Graphics.GL.FRAGMENT_SHADER_DERIVATIVE_HINT, HytaleClient.Graphics.GL.FASTEST);
			string text = Marshal.PtrToStringAnsi(this.GL.GetString(HytaleClient.Graphics.GL.VENDOR)).ToLower();
			bool flag2 = text.Contains("intel");
			GraphicsDevice.GPUInfos gpuinfos;
			if (flag2)
			{
				gpuinfos.Vendor = GraphicsDevice.GPUVendor.Intel;
			}
			else
			{
				bool flag3 = text.Contains("nvidia");
				if (flag3)
				{
					gpuinfos.Vendor = GraphicsDevice.GPUVendor.Nvidia;
				}
				else
				{
					bool flag4 = text.Contains("amd") || text.Contains("ati");
					if (flag4)
					{
						gpuinfos.Vendor = GraphicsDevice.GPUVendor.AMD;
					}
					else
					{
						gpuinfos.Vendor = GraphicsDevice.GPUVendor.Other;
					}
				}
			}
			gpuinfos.Version = Marshal.PtrToStringAnsi(this.GL.GetString(HytaleClient.Graphics.GL.VERSION));
			gpuinfos.Renderer = Marshal.PtrToStringAnsi(this.GL.GetString(HytaleClient.Graphics.GL.RENDERER));
			this.GPUInfo = gpuinfos;
			this.CheckGPUMemoryStatsAtStartup();
			this.IsGPULowEnd = (gpuinfos.Vendor != GraphicsDevice.GPUVendor.Nvidia && gpuinfos.Vendor != GraphicsDevice.GPUVendor.AMD);
			this.SupportsDrawBuffersBlend = (SDL.SDL_GL_ExtensionSupported("GL_ARB_draw_buffers_blend") == SDL.SDL_bool.SDL_TRUE);
			GraphicsDevice.<>c__DisplayClass79_0 CS$<>8__locals1;
			CS$<>8__locals1.temp = new int[1];
			int num = this.<.ctor>g__GetFramebufferParam|79_0(HytaleClient.Graphics.GL.BACK_LEFT, HytaleClient.Graphics.GL.FRAMEBUFFER_ATTACHMENT_RED_SIZE, ref CS$<>8__locals1);
			int num2 = this.<.ctor>g__GetFramebufferParam|79_0(HytaleClient.Graphics.GL.BACK_LEFT, HytaleClient.Graphics.GL.FRAMEBUFFER_ATTACHMENT_GREEN_SIZE, ref CS$<>8__locals1);
			int num3 = this.<.ctor>g__GetFramebufferParam|79_0(HytaleClient.Graphics.GL.BACK_LEFT, HytaleClient.Graphics.GL.FRAMEBUFFER_ATTACHMENT_BLUE_SIZE, ref CS$<>8__locals1);
			int num4 = this.<.ctor>g__GetFramebufferParam|79_0(HytaleClient.Graphics.GL.BACK_LEFT, HytaleClient.Graphics.GL.FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE, ref CS$<>8__locals1);
			int num5 = this.<.ctor>g__GetFramebufferParam|79_0(HytaleClient.Graphics.GL.DEPTH, HytaleClient.Graphics.GL.FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE, ref CS$<>8__locals1);
			int num6 = this.<.ctor>g__GetFramebufferParam|79_0(HytaleClient.Graphics.GL.STENCIL, HytaleClient.Graphics.GL.FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE, ref CS$<>8__locals1);
			this.GL.GetIntegerv(HytaleClient.Graphics.GL.DOUBLEBUFFER, CS$<>8__locals1.temp);
			int num7 = CS$<>8__locals1.temp[0];
			bool flag5 = num != 8;
			if (flag5)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_RED_SIZE should be {0} but is {1}", 8, num);
			}
			bool flag6 = num2 != 8;
			if (flag6)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_GREEN_SIZE should be {0} but is {1}", 8, num2);
			}
			bool flag7 = num3 != 8;
			if (flag7)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_BLUE_SIZE should be {0} but is {1}", 8, num3);
			}
			bool flag8 = num4 != 8;
			if (flag8)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_ALPHA_SIZE should be {0} but is {1}", 8, num4);
			}
			bool flag9 = num5 != 24;
			if (flag9)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_DEPTH_SIZE should be {0} but is {1}", 24, num5);
			}
			bool flag10 = num6 != 8;
			if (flag10)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_STENCIL_SIZE should be {0} but is {1}", 8, num6);
			}
			bool flag11 = num7 != 1;
			if (flag11)
			{
				GraphicsDevice.Logger.Warn<int, int>("SDL_GL_DOUBLEBUFFER should be {0} but is {1}", 1, num7);
			}
			this.GL.GetIntegerv(HytaleClient.Graphics.GL.MAX_VERTEX_UNIFORM_COMPONENTS, CS$<>8__locals1.temp);
			bool flag12 = CS$<>8__locals1.temp[0] < 1024;
			if (flag12)
			{
				throw new Exception(string.Format("Hardware not supported, MAX_VERTEX_UNIFORM_COMPONENTS is {0} but should be at least {1}", CS$<>8__locals1.temp[0], 1024));
			}
			this.GL.GetIntegerv(HytaleClient.Graphics.GL.MAX_UNIFORM_BLOCK_SIZE, CS$<>8__locals1.temp);
			bool flag13 = CS$<>8__locals1.temp[0] < 16384;
			if (flag13)
			{
				throw new Exception(string.Format("Hardware not supported, MAX_UNIFORM_BLOCK_SIZE is {0} but should be at least {1}", CS$<>8__locals1.temp[0], 16384));
			}
			this.MaxUniformBlockSize = CS$<>8__locals1.temp[0];
			this.GL.GetIntegerv(HytaleClient.Graphics.GL.MAX_TEXTURE_IMAGE_UNITS, CS$<>8__locals1.temp);
			bool flag14 = CS$<>8__locals1.temp[0] < 16;
			if (flag14)
			{
				throw new Exception(string.Format("Hardware not supported, MAX_TEXTURE_IMAGE_UNITS is {0} but should be at least {1}", CS$<>8__locals1.temp[0], 16));
			}
			this.MaxTextureImageUnits = CS$<>8__locals1.temp[0];
			this.GL.GetIntegerv(HytaleClient.Graphics.GL.MAX_TEXTURE_SIZE, CS$<>8__locals1.temp);
			bool flag15 = CS$<>8__locals1.temp[0] < 8192;
			if (flag15)
			{
				throw new Exception(string.Format("Hardware not supported, MAX_TEXTURE_SIZE is {0} but should be at least {1}", CS$<>8__locals1.temp[0], 8192));
			}
			this.MaxTextureSize = CS$<>8__locals1.temp[0];
			this.UseReverseZ = false;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("");
			stringBuilder.AppendLine("System informations");
			stringBuilder.AppendLine(string.Format(" CPU core count: {0}", this.CpuCoreCount));
			stringBuilder.AppendLine(string.Format(" RAM size: {0} MB", this.RamSize));
			stringBuilder.AppendLine(" OpenGL driver: " + gpuinfos.Version);
			stringBuilder.AppendLine(" GPU Renderer: " + gpuinfos.Renderer);
			stringBuilder.AppendLine(string.Format(" GPU Memory Total: {0} MB", this._videoMemory.Capacity / 1024));
			stringBuilder.AppendLine(string.Format(" GPU Memory Available: {0} MB", this._videoMemory.AvailableAtStartup / 1024));
			GraphicsDevice.Logger.Info<StringBuilder>(stringBuilder);
			this.GL.ClearStencil(1);
			this.GPUProgramStore = new GPUProgramStore(this);
			int width = window.Viewport.Width;
			int height = window.Viewport.Height;
			this.RTStore = new RenderTargetStore(this, width, height, new Vector2(2048f, 2048f), CascadedShadowMapping.DefaultDeferredShadowResolutionScale, SceneRenderer.DefaultSSAOResolutionScale);
			this.SamplerLinearMipmapLinearA = this.GL.GenSampler();
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearA, HytaleClient.Graphics.GL.TEXTURE_WRAP_S, HytaleClient.Graphics.GL.CLAMP_TO_EDGE);
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearA, HytaleClient.Graphics.GL.TEXTURE_WRAP_T, HytaleClient.Graphics.GL.CLAMP_TO_EDGE);
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearA, HytaleClient.Graphics.GL.TEXTURE_MIN_FILTER, HytaleClient.Graphics.GL.LINEAR_MIPMAP_LINEAR);
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearA, HytaleClient.Graphics.GL.TEXTURE_MAG_FILTER, HytaleClient.Graphics.GL.LINEAR);
			this.SamplerLinearMipmapLinearB = this.GL.GenSampler();
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearB, HytaleClient.Graphics.GL.TEXTURE_WRAP_S, HytaleClient.Graphics.GL.CLAMP_TO_EDGE);
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearB, HytaleClient.Graphics.GL.TEXTURE_WRAP_T, HytaleClient.Graphics.GL.CLAMP_TO_EDGE);
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearB, HytaleClient.Graphics.GL.TEXTURE_MIN_FILTER, HytaleClient.Graphics.GL.LINEAR_MIPMAP_LINEAR);
			this.GL.SamplerParameteri(this.SamplerLinearMipmapLinearB, HytaleClient.Graphics.GL.TEXTURE_MAG_FILTER, HytaleClient.Graphics.GL.LINEAR);
			this.WhitePixelTexture = new Texture(Texture.TextureTypes.Texture2D);
			this.WhitePixelTexture.CreateTexture2D(1, 1, new byte[]
			{
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue
			}, 5, HytaleClient.Graphics.GL.NEAREST, HytaleClient.Graphics.GL.NEAREST, HytaleClient.Graphics.GL.CLAMP_TO_EDGE, HytaleClient.Graphics.GL.CLAMP_TO_EDGE, HytaleClient.Graphics.GL.RGBA, HytaleClient.Graphics.GL.RGBA, HytaleClient.Graphics.GL.UNSIGNED_BYTE, false);
			this.ScreenQuadRenderer = new QuadRenderer(this, this.GPUProgramStore.BasicProgram.AttribPosition, this.GPUProgramStore.BasicProgram.AttribTexCoords);
			this.ScreenQuadRenderer.UpdateUVs(1f, 0f, 0f, 1f);
			this.ScreenTriangleRenderer = new FullscreenTriangleRenderer(this);
			this.Batcher2D = new Batcher2D(this, allowBatcher2dToGrow);
			this.Cursors = new Cursors();
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x00169B5C File Offset: 0x00167D5C
		protected override void DoDispose()
		{
			this.GPUProgramStore.Release();
			this.GL.DeleteSampler(this.SamplerLinearMipmapLinearB);
			this.GL.DeleteSampler(this.SamplerLinearMipmapLinearA);
			this.RTStore.Dispose();
			this.Cursors.Dispose();
			this.Batcher2D.Dispose();
			this.ScreenTriangleRenderer.Dispose();
			this.ScreenQuadRenderer.Dispose();
			this.WhitePixelTexture.Dispose();
			Texture.ReleaseGL();
			GPUBufferTexture.ReleaseGL();
			GPUBuffer.ReleaseGL();
			GPUProgram.ReleaseGL();
			GPUTimer.ReleaseGL();
			UniformBufferObject.ReleaseGL();
			Uniform.ReleaseGL();
			RenderTarget.ReleaseGL();
			MeshProcessor.ReleaseGL();
			Mesh.ReleaseGL();
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x0600523F RID: 21055 RVA: 0x00169C20 File Offset: 0x00167E20
		// (set) Token: 0x06005240 RID: 21056 RVA: 0x00169C38 File Offset: 0x00167E38
		public bool UseReverseZ
		{
			get
			{
				return this._useReverseZ;
			}
			set
			{
				if (value)
				{
					this.GL.DepthFunc(HytaleClient.Graphics.GL.GEQUAL);
					this.GL.ClearDepth(0.0);
				}
				else
				{
					this.GL.DepthFunc(HytaleClient.Graphics.GL.LEQUAL);
					this.GL.ClearDepth(1.0);
				}
				this._useReverseZ = value;
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x00169CB0 File Offset: 0x00167EB0
		public void CreatePerspectiveMatrix(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix result)
		{
			bool useReverseZ = this.UseReverseZ;
			if (useReverseZ)
			{
				Matrix.CreatePerspectiveFieldOfViewReverseZ(fieldOfView, aspectRatio, nearPlaneDistance, out result);
			}
			else
			{
				Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance, out result);
			}
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x00169CE2 File Offset: 0x00167EE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SaveColorMask()
		{
			this.GL.GetIntegerv(HytaleClient.Graphics.GL.COLOR_WRITEMASK, this._tmp);
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00169D01 File Offset: 0x00167F01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RestoreColorMask()
		{
			this.GL.ColorMask(this._tmp[0] == 1, this._tmp[1] == 1, this._tmp[2] == 1, this._tmp[3] == 1);
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x00169D44 File Offset: 0x00167F44
		public void SetVSyncEnabled(bool enabled)
		{
			if (enabled)
			{
				SDL.SDL_GL_SetSwapInterval(1);
			}
			else
			{
				SDL.SDL_GL_SetSwapInterval(0);
			}
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x00169D78 File Offset: 0x00167F78
		[CompilerGenerated]
		private int <.ctor>g__GetFramebufferParam|79_0(GL attachment, GL param, ref GraphicsDevice.<>c__DisplayClass79_0 A_3)
		{
			this.GL.GetFramebufferAttachmentParameteriv(HytaleClient.Graphics.GL.DRAW_FRAMEBUFFER, attachment, param, A_3.temp);
			return A_3.temp[0];
		}

		// Token: 0x04002D27 RID: 11559
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002D2B RID: 11563
		private GraphicsDevice.GPUMemory _videoMemory;

		// Token: 0x04002D2C RID: 11564
		private const int GPU_MEMORY_INFO_DEDICATED_VIDMEM_NVX = 36935;

		// Token: 0x04002D2D RID: 11565
		private const int GPU_MEMORY_INFO_TOTAL_AVAILABLE_MEMORY_NVX = 36936;

		// Token: 0x04002D2E RID: 11566
		private const int GPU_MEMORY_INFO_CURRENT_AVAILABLE_VIDMEM_NVX = 36937;

		// Token: 0x04002D2F RID: 11567
		private const int GPU_MEMORY_INFO_EVICTION_COUNT_NVX = 36938;

		// Token: 0x04002D30 RID: 11568
		private const int GPU_MEMORY_INFO_EVICTED_MEMORY_NVX = 36939;

		// Token: 0x04002D31 RID: 11569
		private const int VBO_FREE_MEMORY_ATI = 34811;

		// Token: 0x04002D32 RID: 11570
		private const int TEXTURE_FREE_MEMORY_ATI = 34812;

		// Token: 0x04002D33 RID: 11571
		private const int RENDERBUFFER_FREE_MEMORY_ATI = 34813;

		// Token: 0x04002D36 RID: 11574
		public readonly GLFunctions GL;

		// Token: 0x04002D37 RID: 11575
		internal readonly GPUProgramStore GPUProgramStore;

		// Token: 0x04002D38 RID: 11576
		internal readonly RenderTargetStore RTStore;

		// Token: 0x04002D39 RID: 11577
		public readonly GLSampler SamplerLinearMipmapLinearA;

		// Token: 0x04002D3A RID: 11578
		public readonly GLSampler SamplerLinearMipmapLinearB;

		// Token: 0x04002D3B RID: 11579
		public readonly Texture WhitePixelTexture;

		// Token: 0x04002D3C RID: 11580
		public readonly byte[] TransparentPixel = new byte[4];

		// Token: 0x04002D3D RID: 11581
		public readonly Vector3 WhiteColor = Vector3.One;

		// Token: 0x04002D3E RID: 11582
		public readonly Vector3 BlackColor = Vector3.Zero;

		// Token: 0x04002D3F RID: 11583
		public readonly Vector3 RedColor = new Vector3(1f, 0f, 0f);

		// Token: 0x04002D40 RID: 11584
		public readonly Vector3 GreenColor = new Vector3(0f, 1f, 0f);

		// Token: 0x04002D41 RID: 11585
		public readonly Vector3 BlueColor = new Vector3(0f, 0f, 1f);

		// Token: 0x04002D42 RID: 11586
		public readonly Vector3 CyanColor = new Vector3(0f, 1f, 1f);

		// Token: 0x04002D43 RID: 11587
		public readonly Vector3 MagentaColor = new Vector3(1f, 0f, 1f);

		// Token: 0x04002D44 RID: 11588
		public readonly Vector3 YellowColor = new Vector3(1f, 1f, 0f);

		// Token: 0x04002D45 RID: 11589
		public Matrix ScreenMatrix = Matrix.CreateTranslation(0f, 0f, -1f) * Matrix.CreateOrthographicOffCenter(0f, 1f, 0f, 1f, 0.1f, 1000f);

		// Token: 0x04002D46 RID: 11590
		public readonly QuadRenderer ScreenQuadRenderer;

		// Token: 0x04002D47 RID: 11591
		public readonly FullscreenTriangleRenderer ScreenTriangleRenderer;

		// Token: 0x04002D48 RID: 11592
		public readonly Batcher2D Batcher2D;

		// Token: 0x04002D49 RID: 11593
		public readonly Cursors Cursors;

		// Token: 0x04002D4A RID: 11594
		public const int ColorComponentSize = 8;

		// Token: 0x04002D4B RID: 11595
		public const int DepthSize = 24;

		// Token: 0x04002D4C RID: 11596
		public const int StencilSize = 8;

		// Token: 0x04002D4D RID: 11597
		public const int EnableDoubleBuffer = 1;

		// Token: 0x04002D4E RID: 11598
		public readonly int MaxTextureSize;

		// Token: 0x04002D4F RID: 11599
		public readonly int MaxUniformBlockSize;

		// Token: 0x04002D50 RID: 11600
		public readonly int MaxTextureImageUnits;

		// Token: 0x04002D51 RID: 11601
		public const int MaxDeferredLights = 1024;

		// Token: 0x04002D52 RID: 11602
		public readonly int OcclusionMapWidth = 512;

		// Token: 0x04002D53 RID: 11603
		public readonly int OcclusionMapHeight = 256;

		// Token: 0x04002D54 RID: 11604
		private bool _useReverseZ;

		// Token: 0x04002D55 RID: 11605
		private int[] _tmp = new int[4];

		// Token: 0x04002D56 RID: 11606
		public bool UseDeferredLight = true;

		// Token: 0x04002D57 RID: 11607
		public bool UseDownsampledZ = false;

		// Token: 0x04002D58 RID: 11608
		public bool UseLowResDeferredLighting = false;

		// Token: 0x04002D59 RID: 11609
		public bool UseLinearZ = false;

		// Token: 0x04002D5A RID: 11610
		public bool UseLinearZForLight = false;

		// Token: 0x02000EA8 RID: 3752
		public enum GPUVendor
		{
			// Token: 0x0400477E RID: 18302
			Intel,
			// Token: 0x0400477F RID: 18303
			Nvidia,
			// Token: 0x04004780 RID: 18304
			AMD,
			// Token: 0x04004781 RID: 18305
			Other
		}

		// Token: 0x02000EA9 RID: 3753
		public struct GPUInfos
		{
			// Token: 0x04004782 RID: 18306
			public GraphicsDevice.GPUVendor Vendor;

			// Token: 0x04004783 RID: 18307
			public string Renderer;

			// Token: 0x04004784 RID: 18308
			public string Version;
		}

		// Token: 0x02000EAA RID: 3754
		public struct GPUMemory
		{
			// Token: 0x04004785 RID: 18309
			public int Capacity;

			// Token: 0x04004786 RID: 18310
			public int AvailableAtStartup;

			// Token: 0x04004787 RID: 18311
			public int AvailableNow;
		}
	}
}
