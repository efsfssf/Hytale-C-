using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using HytaleClient.Application;
using HytaleClient.Audio;
using HytaleClient.Graphics;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.Core
{
	// Token: 0x02000B7C RID: 2940
	internal class Engine : Disposable
	{
		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06005A3E RID: 23102 RVA: 0x001C142D File Offset: 0x001BF62D
		// (set) Token: 0x06005A3F RID: 23103 RVA: 0x001C1435 File Offset: 0x001BF635
		public Window Window { get; private set; }

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06005A40 RID: 23104 RVA: 0x001C143E File Offset: 0x001BF63E
		// (set) Token: 0x06005A41 RID: 23105 RVA: 0x001C1446 File Offset: 0x001BF646
		public GraphicsDevice Graphics { get; private set; }

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x06005A42 RID: 23106 RVA: 0x001C144F File Offset: 0x001BF64F
		// (set) Token: 0x06005A43 RID: 23107 RVA: 0x001C1457 File Offset: 0x001BF657
		public AudioDevice Audio { get; private set; }

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x06005A44 RID: 23108 RVA: 0x001C1460 File Offset: 0x001BF660
		// (set) Token: 0x06005A45 RID: 23109 RVA: 0x001C1468 File Offset: 0x001BF668
		public AnimationSystem AnimationSystem { get; private set; }

		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06005A46 RID: 23110 RVA: 0x001C1471 File Offset: 0x001BF671
		// (set) Token: 0x06005A47 RID: 23111 RVA: 0x001C1479 File Offset: 0x001BF679
		public FXSystem FXSystem { get; private set; }

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06005A48 RID: 23112 RVA: 0x001C1482 File Offset: 0x001BF682
		// (set) Token: 0x06005A49 RID: 23113 RVA: 0x001C148A File Offset: 0x001BF68A
		public Profiling Profiling { get; private set; }

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06005A4A RID: 23114 RVA: 0x001C1493 File Offset: 0x001BF693
		// (set) Token: 0x06005A4B RID: 23115 RVA: 0x001C149B File Offset: 0x001BF69B
		public OcclusionCulling OcclusionCulling { get; private set; }

		// Token: 0x06005A4C RID: 23116 RVA: 0x001C14A4 File Offset: 0x001BF6A4
		public static void Initialize()
		{
			bool flag = BuildInfo.Platform == Platform.Windows;
			if (flag)
			{
				bool flag2 = WindowsDPIHelper.TryEnableDpiAwareness();
				Trace.WriteLine(string.Format("TryEnableDpiAwareness returned {0}", flag2), "Engine");
			}
			bool flag3 = SDL.SDL_Init(32U) < 0;
			if (flag3)
			{
				throw new Exception("SDL_Init failed: " + SDL.SDL_GetError());
			}
			bool flag4 = SDL_ttf.TTF_Init() < 0;
			if (flag4)
			{
				throw new Exception("TTF_Init failed: " + SDL.SDL_GetError());
			}
			SDL.SDL_StartTextInput();
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x001C152C File Offset: 0x001BF72C
		public Engine(Window.WindowSettings windowSettings, bool allowBatcher2dToGrow = false)
		{
			GraphicsDevice.SetupGLAttributes();
			this.Window = new Window(windowSettings);
			this.Graphics = new GraphicsDevice(this.Window, allowBatcher2dToGrow);
			GLFunctions gl = this.Graphics.GL;
			gl.Viewport(this.Window.Viewport);
			gl.ClearColor(0f, 0f, 0f, 1f);
			gl.Clear(GL.COLOR_BUFFER_BIT);
			SDL.SDL_GL_SwapWindow(this.Window.Handle);
			this.Profiling = new Profiling(this.Graphics.GL);
			this.AnimationSystem = new AnimationSystem(this.Graphics.GL);
			this.FXSystem = new FXSystem(this.Graphics, this.Profiling, 0.016666668f);
			this.OcclusionCulling = new OcclusionCulling(this.Graphics, this.Profiling);
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x001C164B File Offset: 0x001BF84B
		public void InitializeAudio(uint outputDeviceId, string masterVolumeRTPC, float masterVolume, string[] categoryRTPCs, float[] categoryVolumes)
		{
			Debug.Assert(this.Audio == null);
			this.Audio = new AudioDevice(outputDeviceId, masterVolumeRTPC, masterVolume, categoryRTPCs, categoryVolumes, Enum.GetNames(typeof(App.SoundGroupType)).Length);
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x001C1684 File Offset: 0x001BF884
		protected override void DoDispose()
		{
			AudioDevice audio = this.Audio;
			if (audio != null)
			{
				audio.Dispose();
			}
			OcclusionCulling occlusionCulling = this.OcclusionCulling;
			if (occlusionCulling != null)
			{
				occlusionCulling.Dispose();
			}
			FXSystem fxsystem = this.FXSystem;
			if (fxsystem != null)
			{
				fxsystem.Dispose();
			}
			AnimationSystem animationSystem = this.AnimationSystem;
			if (animationSystem != null)
			{
				animationSystem.Dispose();
			}
			Profiling profiling = this.Profiling;
			if (profiling != null)
			{
				profiling.Dispose();
			}
			GraphicsDevice graphics = this.Graphics;
			if (graphics != null)
			{
				graphics.Dispose();
			}
			this.Window.Dispose();
			SDL_ttf.TTF_Quit();
			SDL.SDL_Quit();
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x001C1716 File Offset: 0x001BF916
		public void SetMouseRelativeModeRaw(bool isRawMode)
		{
			SDL.SDL_SetHint("SDL_MOUSE_RELATIVE_MODE_WARP", isRawMode ? "0" : "1");
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06005A51 RID: 23121 RVA: 0x001C1733 File Offset: 0x001BF933
		public double TimeSpentInQueuedActions
		{
			get
			{
				return this._timeSpentInQueuedActions;
			}
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x001C173C File Offset: 0x001BF93C
		public void RunOnMainThread(Disposable disposeGate, Action action, bool allowCallFromMainThread = false, bool distributed = false)
		{
			Debug.Assert(allowCallFromMainThread || !ThreadHelper.IsMainThread());
			if (distributed)
			{
				this._distributedMainThreadActionQueue.Enqueue(Tuple.Create<Disposable, Action>(disposeGate, action));
			}
			else
			{
				this._mainThreadActionQueue.Enqueue(Tuple.Create<Disposable, Action>(disposeGate, action));
			}
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x001C178C File Offset: 0x001BF98C
		public void Temp_ProcessQueuedActions()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			this._stopwatch.Restart();
			Tuple<Disposable, Action> tuple;
			while (this._mainThreadActionQueue.TryDequeue(out tuple))
			{
				bool flag = !tuple.Item1.Disposed;
				if (flag)
				{
					tuple.Item2();
				}
			}
			bool flag2 = !this._distributedMainThreadActionQueue.IsEmpty;
			if (flag2)
			{
				while (this._stopwatch.ElapsedMilliseconds < 8L && this._distributedMainThreadActionQueue.TryDequeue(out tuple))
				{
					bool flag3 = !tuple.Item1.Disposed;
					if (flag3)
					{
						tuple.Item2();
					}
				}
				int count = this._distributedMainThreadActionQueue.Count;
				bool flag4 = count > 500;
				if (flag4)
				{
					Engine.Logger.Warn("Distributed task queue is getting too large: {0}", count);
					while (this._distributedMainThreadActionQueue.Count > 500 && this._distributedMainThreadActionQueue.TryDequeue(out tuple))
					{
						bool flag5 = !tuple.Item1.Disposed;
						if (flag5)
						{
							tuple.Item2();
						}
					}
				}
			}
			this._timeSpentInQueuedActions = stopwatch.Elapsed.TotalMilliseconds;
		}

		// Token: 0x04003864 RID: 14436
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003865 RID: 14437
		public const float TimeStep = 0.016666668f;

		// Token: 0x04003866 RID: 14438
		public const int TimeStepMilliseconds = 16;

		// Token: 0x04003867 RID: 14439
		public const float MaxAccumulatedTickTime = 0.083333336f;

		// Token: 0x0400386F RID: 14447
		private const int MaxDistributedTaskTime = 8;

		// Token: 0x04003870 RID: 14448
		private const int DistributedTaskQueueBackPressureLength = 500;

		// Token: 0x04003871 RID: 14449
		private readonly ConcurrentQueue<Tuple<Disposable, Action>> _mainThreadActionQueue = new ConcurrentQueue<Tuple<Disposable, Action>>();

		// Token: 0x04003872 RID: 14450
		private readonly ConcurrentQueue<Tuple<Disposable, Action>> _distributedMainThreadActionQueue = new ConcurrentQueue<Tuple<Disposable, Action>>();

		// Token: 0x04003873 RID: 14451
		private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

		// Token: 0x04003874 RID: 14452
		private double _timeSpentInQueuedActions;
	}
}
