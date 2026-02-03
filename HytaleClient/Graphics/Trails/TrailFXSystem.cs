using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Core;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics.Trails
{
	// Token: 0x02000AAC RID: 2732
	internal class TrailFXSystem : Disposable
	{
		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x060055E5 RID: 21989 RVA: 0x001998C9 File Offset: 0x00197AC9
		public int BlendDrawCount
		{
			get
			{
				return this._trailBlendDrawCount + this._trailFPSDrawCount;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x060055E6 RID: 21990 RVA: 0x001998D8 File Offset: 0x00197AD8
		public bool HasDistortionTasks
		{
			get
			{
				return this._trailDistortionDrawCount > 0;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x060055E7 RID: 21991 RVA: 0x001998E3 File Offset: 0x00197AE3
		public bool HasErosionTasks
		{
			get
			{
				return this._trailErosionDrawCount > 0;
			}
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x001998F0 File Offset: 0x00197AF0
		public TrailFXSystem(GraphicsDevice graphics, float engineTimeStep)
		{
			this._graphics = graphics;
			this._engineTimeStep = engineTimeStep;
			this.Initialize();
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x001999A3 File Offset: 0x00197BA3
		public void InitializeFunction(UpdateTrailLightingFunc updateTrailLighting)
		{
			this._updateTrailLightingFunc = updateTrailLighting;
		}

		// Token: 0x060055EA RID: 21994 RVA: 0x001999AD File Offset: 0x00197BAD
		public void DisposeFunction()
		{
			this._updateTrailLightingFunc = null;
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x001999B7 File Offset: 0x00197BB7
		private void Initialize()
		{
			this.InitMemory();
			this.InitTrails();
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x001999C8 File Offset: 0x00197BC8
		private void InitMemory()
		{
			this._segmentMemoryPool = new FXMemoryPool<SegmentBuffers>();
			this._segmentMemoryPool.Initialize(256000);
		}

		// Token: 0x060055ED RID: 21997 RVA: 0x001999E7 File Offset: 0x00197BE7
		private void InitTrails()
		{
			this._trailProxies = new TrailProxy[50];
			this._trailProxyDistanceToCamera = new float[50];
			this._sortedTrailProxyIds = new ushort[50];
		}

		// Token: 0x060055EE RID: 21998 RVA: 0x00199A11 File Offset: 0x00197C11
		private void DisposeMemory()
		{
			this._segmentMemoryPool.Release();
			this._segmentMemoryPool = null;
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x00199A27 File Offset: 0x00197C27
		protected override void DoDispose()
		{
			this.DisposeTrails();
			this.DisposeMemory();
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x00199A38 File Offset: 0x00197C38
		private void DisposeTrails()
		{
			foreach (Trail trail in this._trails.Values)
			{
				trail.Dispose();
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x060055F1 RID: 22001 RVA: 0x00199A94 File Offset: 0x00197C94
		public ushort TrailProxyCount
		{
			get
			{
				return this._trailProxyCount;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x060055F2 RID: 22002 RVA: 0x00199A9C File Offset: 0x00197C9C
		public int TrailCount
		{
			get
			{
				return this._trails.Count;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x060055F3 RID: 22003 RVA: 0x00199AA9 File Offset: 0x00197CA9
		public unsafe SegmentBuffers SegmentBuffer
		{
			get
			{
				return *this._segmentMemoryPool.Storage;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x060055F4 RID: 22004 RVA: 0x00199ABB File Offset: 0x00197CBB
		public int MaxParticleDrawCount
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x060055F5 RID: 22005 RVA: 0x00199AC2 File Offset: 0x00197CC2
		public int SegmentBufferStorageMaxCount
		{
			get
			{
				return this._segmentMemoryPool.ItemMaxCount;
			}
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x00199ACF File Offset: 0x00197CCF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int RequestSegmentBufferStorage(int count)
		{
			return this._segmentMemoryPool.TakeSlots(count);
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x00199ADD File Offset: 0x00197CDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReleaseSegmentBufferStorage(int segmentStartIndex, int segmentCount)
		{
			this._segmentMemoryPool.ReleaseSlots(segmentStartIndex, segmentCount);
		}

		// Token: 0x060055F8 RID: 22008 RVA: 0x00199AF0 File Offset: 0x00197CF0
		public void UpdateProxies(Vector3 cameraPosition, bool useProxyCheck)
		{
			int i = 0;
			while (i < (int)this._trailProxyCount)
			{
				TrailProxy trailProxy = this._trailProxies[i];
				bool isExpired = trailProxy.IsExpired;
				if (isExpired)
				{
					bool flag = trailProxy.Trail != null;
					if (flag)
					{
						trailProxy.Trail.IsExpired = true;
						trailProxy.Trail = null;
					}
					this._trailProxyCount -= 1;
					this._trailProxies[i] = this._trailProxies[(int)this._trailProxyCount];
				}
				else
				{
					this._trailProxyDistanceToCamera[i] = (trailProxy.IsLocalPlayer ? 0f : Vector3.DistanceSquared(cameraPosition, trailProxy.Position));
					this._sortedTrailProxyIds[i] = (ushort)i;
					i++;
				}
			}
			Array.Sort<float, ushort>(this._trailProxyDistanceToCamera, this._sortedTrailProxyIds, 0, (int)this._trailProxyCount);
			int num = 0;
			int j = 0;
			while (j < (int)this._trailProxyCount)
			{
				ushort num2 = this._sortedTrailProxyIds[j];
				float num3 = this._trailProxyDistanceToCamera[j];
				TrailProxy trailProxy2 = this._trailProxies[(int)num2];
				bool flag2 = trailProxy2.Trail == null;
				if (!flag2)
				{
					trailProxy2.Trail.Position = trailProxy2.Position;
					trailProxy2.Trail.Rotation = trailProxy2.Rotation;
					bool flag3 = j >= this._maxTrailSpawned && num > 0;
					bool flag4 = (useProxyCheck && num3 > 1625f) || flag3;
					if (flag4)
					{
						trailProxy2.Trail.IsExpired = true;
						trailProxy2.Trail = null;
						num--;
					}
					goto IL_1BB;
				}
				bool flag5 = this._trails.Count == this._maxTrailSpawned;
				if (!flag5)
				{
					bool flag6 = !useProxyCheck || num3 < 1600f;
					if (flag6)
					{
						trailProxy2.Trail = this.CreateTrail(trailProxy2);
					}
					goto IL_1BB;
				}
				num++;
				IL_1E1:
				j++;
				continue;
				IL_1BB:
				bool flag7 = trailProxy2.Trail != null;
				if (flag7)
				{
					trailProxy2.Trail.Visible = trailProxy2.Visible;
				}
				goto IL_1E1;
			}
		}

		// Token: 0x060055F9 RID: 22009 RVA: 0x00199CF8 File Offset: 0x00197EF8
		public void UpdateSimulation(float deltaTime)
		{
			this._accumulatedDeltaTime += deltaTime;
			bool flag = this._accumulatedDeltaTime < this._engineTimeStep;
			if (flag)
			{
				foreach (Trail trail in this._trails.Values)
				{
					trail.LightUpdate();
				}
			}
			else
			{
				this._trailBlendDrawCount = 0;
				this._trailDistortionDrawCount = 0;
				this._trailErosionDrawCount = 0;
				this._trailFPSDrawCount = 0;
				int num = 0;
				foreach (Trail trail2 in this._trails.Values)
				{
					bool flag2 = trail2.NeedsUpdating();
					if (flag2)
					{
						trail2.Update();
						this._updateTrailLightingFunc(trail2);
						bool flag3 = !trail2.Visible;
						if (!flag3)
						{
							num += trail2.ParticleCount;
							bool flag4 = num > 10000;
							if (flag4)
							{
								break;
							}
							bool isDistortion = trail2.IsDistortion;
							if (isDistortion)
							{
								bool flag5 = this._trailDistortionDrawCount == this._trailDistortionDraw.Length;
								if (flag5)
								{
									Array.Resize<Trail>(ref this._trailDistortionDraw, this._trailDistortionDraw.Length + 10);
								}
								this._trailDistortionDraw[this._trailDistortionDrawCount] = trail2;
								this._trailDistortionDrawCount++;
							}
							else
							{
								bool flag6 = trail2.RenderMode == FXSystem.RenderMode.Erosion;
								if (flag6)
								{
									bool flag7 = this._trailErosionDrawCount == this._trailErosionDraw.Length;
									if (flag7)
									{
										Array.Resize<Trail>(ref this._trailErosionDraw, this._trailErosionDraw.Length + 10);
									}
									this._trailErosionDraw[this._trailErosionDrawCount] = trail2;
									this._trailErosionDrawCount++;
								}
								else
								{
									bool flag8 = !trail2.IsFirstPerson;
									if (flag8)
									{
										FXSystem.RenderMode renderMode = trail2.RenderMode;
										FXSystem.RenderMode renderMode2 = renderMode;
										if (renderMode2 <= FXSystem.RenderMode.BlendAdd)
										{
											bool flag9 = this._trailBlendDrawCount == this._trailBlendDraw.Length;
											if (flag9)
											{
												Array.Resize<Trail>(ref this._trailBlendDraw, this._trailBlendDraw.Length + 10);
											}
											this._trailBlendDraw[this._trailBlendDrawCount] = trail2;
											this._trailBlendDrawCount++;
										}
									}
									else
									{
										bool flag10 = this._trailFPSDrawCount == this._trailFPSDraw.Length;
										if (flag10)
										{
											Array.Resize<Trail>(ref this._trailFPSDraw, this._trailFPSDraw.Length + 4);
										}
										this._trailFPSDraw[this._trailFPSDrawCount] = trail2;
										this._trailFPSDrawCount++;
									}
								}
							}
						}
					}
					else
					{
						bool isExpired = trail2.IsExpired;
						if (isExpired)
						{
							this._expiredTrailIds.Add(trail2.Id);
						}
					}
				}
				for (int i = 0; i < this._expiredTrailIds.Count; i++)
				{
					this._trails[this._expiredTrailIds[i]].Dispose();
					this._trails.Remove(this._expiredTrailIds[i]);
				}
				this._expiredTrailIds.Clear();
				this._accumulatedDeltaTime = 0f;
			}
		}

		// Token: 0x060055FA RID: 22010 RVA: 0x0019A070 File Offset: 0x00198270
		public bool TrySpawnTrail(TrailSettings trailSettings, Vector2 textureAltasInverseSize, out TrailProxy trailProxy, bool isLocalPlayer = false)
		{
			trailProxy = null;
			bool flag = this._trailProxyCount == ushort.MaxValue;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ArrayUtils.GrowArrayIfNecessary<TrailProxy>(ref this._trailProxies, (int)(this._trailProxyCount + 1), 100);
				ArrayUtils.GrowArrayIfNecessary<float>(ref this._trailProxyDistanceToCamera, (int)(this._trailProxyCount + 1), 100);
				ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._sortedTrailProxyIds, (int)(this._trailProxyCount + 1), 100);
				trailProxy = new TrailProxy();
				trailProxy.Settings = trailSettings;
				trailProxy.TextureAltasInverseSize = textureAltasInverseSize;
				trailProxy.IsLocalPlayer = isLocalPlayer;
				this._trailProxies[(int)this._trailProxyCount] = trailProxy;
				this._trailProxyCount += 1;
				result = true;
			}
			return result;
		}

		// Token: 0x060055FB RID: 22011 RVA: 0x0019A120 File Offset: 0x00198320
		public void PrepareBlendVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._trailBlendDrawCount == 0;
			if (!flag)
			{
				int num = fXRenderer.BlendDrawParams.Count;
				for (int i = 0; i < this._trailBlendDrawCount; i++)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._trailBlendDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._trailBlendDraw[i].ParticleCount;
				}
				fXRenderer.BlendDrawParams.Count = num;
				bool flag2 = !fXRenderer.BlendDrawParams.IsStartOffsetSet;
				if (flag2)
				{
					fXRenderer.BlendDrawParams.StartOffset = (uint)this._trailBlendDraw[0].ParticleVertexDataStartIndex;
					fXRenderer.BlendDrawParams.IsStartOffsetSet = true;
				}
			}
		}

		// Token: 0x060055FC RID: 22012 RVA: 0x0019A1D8 File Offset: 0x001983D8
		public void PrepareFPVVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._trailFPSDrawCount == 0;
			if (!flag)
			{
				int num = fXRenderer.BlendFPVDrawParams.Count;
				for (int i = 0; i < this._trailFPSDrawCount; i++)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._trailFPSDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._trailFPSDraw[i].ParticleCount;
				}
				fXRenderer.BlendFPVDrawParams.Count = num;
				bool flag2 = !fXRenderer.BlendFPVDrawParams.IsStartOffsetSet;
				if (flag2)
				{
					fXRenderer.BlendFPVDrawParams.StartOffset = (uint)this._trailFPSDraw[0].ParticleVertexDataStartIndex;
					fXRenderer.BlendFPVDrawParams.IsStartOffsetSet = true;
				}
			}
		}

		// Token: 0x060055FD RID: 22013 RVA: 0x0019A290 File Offset: 0x00198490
		public void PrepareDistortionVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._trailDistortionDrawCount == 0;
			if (!flag)
			{
				int num = fXRenderer.DistortionDrawParams.Count;
				for (int i = 0; i < this._trailDistortionDrawCount; i++)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._trailDistortionDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._trailDistortionDraw[i].ParticleCount;
				}
				fXRenderer.DistortionDrawParams.Count = num;
				bool flag2 = !fXRenderer.DistortionDrawParams.IsStartOffsetSet;
				if (flag2)
				{
					fXRenderer.DistortionDrawParams.StartOffset = (uint)this._trailDistortionDraw[0].ParticleVertexDataStartIndex;
					fXRenderer.DistortionDrawParams.IsStartOffsetSet = true;
				}
			}
		}

		// Token: 0x060055FE RID: 22014 RVA: 0x0019A348 File Offset: 0x00198548
		public void PrepareErosionVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._trailErosionDrawCount == 0;
			if (!flag)
			{
				int num = fXRenderer.ErosionDrawParams.Count;
				for (int i = 0; i < this._trailErosionDrawCount; i++)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._trailErosionDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._trailErosionDraw[i].ParticleCount;
				}
				fXRenderer.ErosionDrawParams.Count = num;
				bool flag2 = !fXRenderer.ErosionDrawParams.IsStartOffsetSet;
				if (flag2)
				{
					fXRenderer.ErosionDrawParams.StartOffset = (uint)this._trailErosionDraw[0].ParticleVertexDataStartIndex;
					fXRenderer.ErosionDrawParams.IsStartOffsetSet = true;
				}
			}
		}

		// Token: 0x060055FF RID: 22015 RVA: 0x0019A400 File Offset: 0x00198600
		public void PrepareForDraw(FXRenderer fXRenderer, Vector3 cameraPosition, IntPtr gpuDrawDataPtr)
		{
			for (int i = 0; i < this._trailBlendDrawCount; i++)
			{
				this._trailBlendDraw[i].PrepareForDraw(cameraPosition, ref fXRenderer.FXVertexBuffer, gpuDrawDataPtr);
			}
			for (int j = 0; j < this._trailFPSDrawCount; j++)
			{
				this._trailFPSDraw[j].PrepareForDraw(cameraPosition, ref fXRenderer.FXVertexBuffer, gpuDrawDataPtr);
			}
			for (int k = 0; k < this._trailDistortionDrawCount; k++)
			{
				this._trailDistortionDraw[k].PrepareForDraw(cameraPosition, ref fXRenderer.FXVertexBuffer, gpuDrawDataPtr);
			}
			for (int l = 0; l < this._trailErosionDrawCount; l++)
			{
				this._trailErosionDraw[l].PrepareForDraw(cameraPosition, ref fXRenderer.FXVertexBuffer, gpuDrawDataPtr);
			}
		}

		// Token: 0x06005600 RID: 22016 RVA: 0x0019A4C8 File Offset: 0x001986C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Trail CreateTrail(TrailProxy trailProxy)
		{
			GraphicsDevice graphics = this._graphics;
			TrailSettings settings = trailProxy.Settings;
			Vector2 textureAltasInverseSize = trailProxy.TextureAltasInverseSize;
			int nextTrailId = this._nextTrailId;
			this._nextTrailId = nextTrailId + 1;
			Trail trail = new Trail(graphics, this, settings, textureAltasInverseSize, nextTrailId);
			bool flag = trail.Initialize();
			bool flag2 = flag;
			if (flag2)
			{
				trail.SetScale(trailProxy.Scale);
				trail.IsFirstPerson = trailProxy.IsFirstPerson;
				trail.Position = trailProxy.Position;
				trail.Rotation = trailProxy.Rotation;
				trail.SetSpawn();
				this._trails.Add(trail.Id, trail);
			}
			return trail;
		}

		// Token: 0x040032EF RID: 13039
		private const float DefaultCullDistanceSquared = 1600f;

		// Token: 0x040032F0 RID: 13040
		private GraphicsDevice _graphics;

		// Token: 0x040032F1 RID: 13041
		private float _engineTimeStep;

		// Token: 0x040032F2 RID: 13042
		private const int ProxyDefaultSize = 50;

		// Token: 0x040032F3 RID: 13043
		private const int ProxyGrowth = 100;

		// Token: 0x040032F4 RID: 13044
		private int _maxTrailSpawned = 300;

		// Token: 0x040032F5 RID: 13045
		private UpdateTrailLightingFunc _updateTrailLightingFunc;

		// Token: 0x040032F6 RID: 13046
		private ushort _trailProxyCount = 0;

		// Token: 0x040032F7 RID: 13047
		private TrailProxy[] _trailProxies;

		// Token: 0x040032F8 RID: 13048
		private float[] _trailProxyDistanceToCamera;

		// Token: 0x040032F9 RID: 13049
		private ushort[] _sortedTrailProxyIds;

		// Token: 0x040032FA RID: 13050
		private readonly List<int> _expiredTrailIds = new List<int>();

		// Token: 0x040032FB RID: 13051
		private readonly Dictionary<int, Trail> _trails = new Dictionary<int, Trail>();

		// Token: 0x040032FC RID: 13052
		private int _nextTrailId = 0;

		// Token: 0x040032FD RID: 13053
		private float _accumulatedDeltaTime = 0f;

		// Token: 0x040032FE RID: 13054
		private FXMemoryPool<SegmentBuffers> _segmentMemoryPool;

		// Token: 0x040032FF RID: 13055
		private const int _maxParticleDrawCount = 10000;

		// Token: 0x04003300 RID: 13056
		private int _trailBlendDrawCount = 0;

		// Token: 0x04003301 RID: 13057
		private int _trailFPSDrawCount = 0;

		// Token: 0x04003302 RID: 13058
		private int _trailDistortionDrawCount = 0;

		// Token: 0x04003303 RID: 13059
		private int _trailErosionDrawCount = 0;

		// Token: 0x04003304 RID: 13060
		private const int TrailDrawDefaultSize = 20;

		// Token: 0x04003305 RID: 13061
		private const int TrailDrawGrowth = 10;

		// Token: 0x04003306 RID: 13062
		private const int TrailFPSDrawDefaultSize = 2;

		// Token: 0x04003307 RID: 13063
		private const int TrailFPSDrawGrowth = 4;

		// Token: 0x04003308 RID: 13064
		private Trail[] _trailBlendDraw = new Trail[20];

		// Token: 0x04003309 RID: 13065
		private Trail[] _trailFPSDraw = new Trail[2];

		// Token: 0x0400330A RID: 13066
		private Trail[] _trailDistortionDraw = new Trail[20];

		// Token: 0x0400330B RID: 13067
		private Trail[] _trailErosionDraw = new Trail[20];
	}
}
