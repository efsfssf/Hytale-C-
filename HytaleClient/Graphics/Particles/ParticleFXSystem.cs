using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HytaleClient.Core;
using HytaleClient.Graphics.Map;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB3 RID: 2739
	internal class ParticleFXSystem : Disposable
	{
		// Token: 0x0600560A RID: 22026 RVA: 0x0019A8E8 File Offset: 0x00198AE8
		public bool UseParallelExecution(bool enable)
		{
			this._useParallelExecution = enable;
			return enable;
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x0019A8FF File Offset: 0x00198AFF
		public void SetPaused(bool enable)
		{
			this._isPaused = enable;
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x0600560C RID: 22028 RVA: 0x0019A909 File Offset: 0x00198B09
		public bool IsPaused
		{
			get
			{
				return this._isPaused;
			}
		}

		// Token: 0x0600560D RID: 22029 RVA: 0x0019A911 File Offset: 0x00198B11
		public bool DebugInfoNeedsDrawing()
		{
			return this._particleSystemDebugs.Count != 0;
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x0600560E RID: 22030 RVA: 0x0019A921 File Offset: 0x00198B21
		public bool HasDistortionTasks
		{
			get
			{
				return this._spawnerDistortionDrawCount > 0;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x0600560F RID: 22031 RVA: 0x0019A92C File Offset: 0x00198B2C
		public bool HasErosionTasks
		{
			get
			{
				return this._spawnerErosionDrawCount > 0;
			}
		}

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06005610 RID: 22032 RVA: 0x0019A937 File Offset: 0x00198B37
		public int HighResDrawCount
		{
			get
			{
				return this._spawnerFPVDrawCount + this._spawnerBlendDrawCount;
			}
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06005611 RID: 22033 RVA: 0x0019A946 File Offset: 0x00198B46
		public int LowResDrawCount
		{
			get
			{
				return this._spawnerLowResDrawCount;
			}
		}

		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06005612 RID: 22034 RVA: 0x0019A94E File Offset: 0x00198B4E
		public int ParticleSpawnerDrawCount
		{
			get
			{
				return this.HighResDrawCount + this.LowResDrawCount;
			}
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06005613 RID: 22035 RVA: 0x0019A95D File Offset: 0x00198B5D
		public ParticleSystemProxy[] ParticleSystemProxies
		{
			get
			{
				return this._particleSystemProxies;
			}
		}

		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06005614 RID: 22036 RVA: 0x0019A965 File Offset: 0x00198B65
		public int ParticleSystemProxyCount
		{
			get
			{
				return (int)this._particleSystemProxyCount;
			}
		}

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06005615 RID: 22037 RVA: 0x0019A96D File Offset: 0x00198B6D
		public Dictionary<int, ParticleSystem> ParticleSystems
		{
			get
			{
				return this._particleSystems;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06005616 RID: 22038 RVA: 0x0019A975 File Offset: 0x00198B75
		public int ParticleSystemCount
		{
			get
			{
				return this._particleSystems.Count;
			}
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x06005617 RID: 22039 RVA: 0x0019A982 File Offset: 0x00198B82
		public int MaxParticleCount
		{
			get
			{
				return this._particleMemoryPool.ItemMaxCount;
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x06005618 RID: 22040 RVA: 0x0019A98F File Offset: 0x00198B8F
		public int MaxParticleDrawCount
		{
			get
			{
				return 20000;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06005619 RID: 22041 RVA: 0x0019A996 File Offset: 0x00198B96
		public int MaxParticleSystemSpawned
		{
			get
			{
				return this._maxParticleSystemSpawned;
			}
		}

		// Token: 0x0600561A RID: 22042 RVA: 0x0019A9A0 File Offset: 0x00198BA0
		public void SetMaxParticleSystemSpawned(int max)
		{
			this._maxParticleSystemSpawned = max;
			ArrayUtils.GrowArrayIfNecessary<ParticleFXSystem.UpdateTask>(ref this._updateTasks, this._maxParticleSystemSpawned, 0);
			ArrayUtils.GrowArrayIfNecessary<ParticleSystem>(ref this._drawTasks, this._maxParticleSystemSpawned, 0);
			ArrayUtils.GrowArrayIfNecessary<float>(ref this._distanceSquaredToCamera, this._maxParticleSystemSpawned, 0);
			ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._sortedDrawTaskIds, this._maxParticleSystemSpawned, 0);
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x0019AA04 File Offset: 0x00198C04
		public ParticleFXSystem(GraphicsDevice graphics, float engineTimeStep)
		{
			this._graphics = graphics;
			this._engineTimeStep = engineTimeStep;
			this.Initialize();
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x0019AADA File Offset: 0x00198CDA
		public void InitializeFunctions(UpdateSpawnerLightingFunc updateSpawnerLighting, UpdateParticleCollisionFunc updateCollision, InitParticleFunc initParticle)
		{
			this._updateParticleSpawnerLightingFunc = updateSpawnerLighting;
			this._updateParticleCollisionFunc = updateCollision;
			this._initParticleFunc = initParticle;
		}

		// Token: 0x0600561D RID: 22045 RVA: 0x0019AAF2 File Offset: 0x00198CF2
		public void DisposeFunctions()
		{
			this._updateParticleSpawnerLightingFunc = null;
			this._updateParticleCollisionFunc = null;
			this._initParticleFunc = null;
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x0019AB0A File Offset: 0x00198D0A
		public void Initialize()
		{
			MeshProcessor.CreateSphere(ref this._debugSphereMesh, 5, 8, 1f, 0, -1, -1);
			this.InitMemory();
			this.InitParticleSystems();
		}

		// Token: 0x0600561F RID: 22047 RVA: 0x0019AB31 File Offset: 0x00198D31
		protected override void DoDispose()
		{
			this.DisposeParticleSystems();
			this.DisposeMemory();
			this._debugSphereMesh.Dispose();
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x0019AB50 File Offset: 0x00198D50
		private void InitParticleSystems()
		{
			this._particleSystemProxies = new ParticleSystemProxy[50];
			this._particleSystemProxyDistanceToCamera = new float[50];
			this._sortedParticleSystemProxyIds = new ushort[50];
			this._updateTasks = new ParticleFXSystem.UpdateTask[this._maxParticleSystemSpawned];
			this._drawTasks = new ParticleSystem[this._maxParticleSystemSpawned];
			this._distanceSquaredToCamera = new float[this._maxParticleSystemSpawned];
			this._sortedDrawTaskIds = new ushort[this._maxParticleSystemSpawned];
			this._expiredParticleSystemIds = new ConcurrentQueue<int>();
			this._particleSystemDebugs = new Dictionary<int, ParticleSystemDebug>();
			this._particleSystems = new Dictionary<int, ParticleSystem>();
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x0019ABEC File Offset: 0x00198DEC
		private void DisposeParticleSystems()
		{
			foreach (ParticleSystem particleSystem in this._particleSystems.Values)
			{
				particleSystem.Dispose();
			}
			foreach (ParticleSystemDebug particleSystemDebug in this._particleSystemDebugs.Values)
			{
				particleSystemDebug.Dispose();
			}
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x0019AC90 File Offset: 0x00198E90
		private void InitMemory()
		{
			this._particleMemoryPool = new FXMemoryPool<ParticleBuffers>();
			this._particleMemoryPool.Initialize(256000);
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x0019ACAF File Offset: 0x00198EAF
		private void DisposeMemory()
		{
			this._particleMemoryPool.Release();
			this._particleMemoryPool = null;
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x0019ACC5 File Offset: 0x00198EC5
		public void BeginFrame()
		{
			this._updateTaskCount = 0;
			this._drawTaskCount = 0;
			this._sortedDrawTaskCount = 0;
			this.ResetMapFXTaskCounters();
			this.ResetDrawCounters();
		}

		// Token: 0x06005625 RID: 22053 RVA: 0x0019ACEC File Offset: 0x00198EEC
		public bool TrySpawnDebugSystem(ParticleSystemSettings settings, Vector2 textureAltasInverseSize, out ParticleSystem particleSystem)
		{
			bool isGPULowEnd = this._graphics.IsGPULowEnd;
			UpdateSpawnerLightingFunc updateParticleSpawnerLightingFunc = this._updateParticleSpawnerLightingFunc;
			UpdateParticleCollisionFunc updateParticleCollisionFunc = this._updateParticleCollisionFunc;
			InitParticleFunc initParticleFunc = this._initParticleFunc;
			int nextParticleSystemId = this._nextParticleSystemId;
			this._nextParticleSystemId = nextParticleSystemId + 1;
			particleSystem = new ParticleSystem(this, isGPULowEnd, updateParticleSpawnerLightingFunc, updateParticleCollisionFunc, initParticleFunc, textureAltasInverseSize, nextParticleSystemId, settings);
			return particleSystem.Initialize();
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x0019AD40 File Offset: 0x00198F40
		public bool TrySpawnParticleSystemProxy(ParticleSystemSettings settings, Vector2 textureAltasInverseSize, out ParticleSystemProxy particleSystemProxy, bool isLocalPlayer = false, bool isTracked = false)
		{
			particleSystemProxy = null;
			bool flag = this._particleSystemProxyCount == ushort.MaxValue;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ArrayUtils.GrowArrayIfNecessary<ParticleSystemProxy>(ref this._particleSystemProxies, (int)(this._particleSystemProxyCount + 1), 100);
				ArrayUtils.GrowArrayIfNecessary<float>(ref this._particleSystemProxyDistanceToCamera, (int)(this._particleSystemProxyCount + 1), 100);
				ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._sortedParticleSystemProxyIds, (int)(this._particleSystemProxyCount + 1), 100);
				particleSystemProxy = new ParticleSystemProxy();
				particleSystemProxy.Settings = settings;
				particleSystemProxy.TextureAltasInverseSize = textureAltasInverseSize;
				particleSystemProxy.IsLocalPlayer = isLocalPlayer;
				particleSystemProxy.IsTracked = isTracked;
				this._particleSystemProxies[(int)this._particleSystemProxyCount] = particleSystemProxy;
				this._particleSystemProxyCount += 1;
				result = true;
			}
			return result;
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x0019ADF8 File Offset: 0x00198FF8
		public void ClearParticleSystems()
		{
			foreach (ParticleSystem particleSystem in this._particleSystems.Values)
			{
				particleSystem.Dispose();
			}
			this._particleSystems.Clear();
			this.ClearParticleSystemDebugs();
			this._nextParticleSystemId = 0;
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x0019AE70 File Offset: 0x00199070
		public void ClearParticleSystemDebugs()
		{
			foreach (ParticleSystemDebug particleSystemDebug in this._particleSystemDebugs.Values)
			{
				particleSystemDebug.Dispose();
			}
			this._particleSystemDebugs.Clear();
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x0019AED8 File Offset: 0x001990D8
		private void ResetDrawCounters()
		{
			this.PreviousFrameErosionDrawCount = this._spawnerErosionDrawCount;
			this.PreviousFrameDistortionDrawCount = this._spawnerDistortionDrawCount;
			this.PreviousFrameBlendDrawCount = this.ParticleSpawnerDrawCount;
			this._spawnerErosionDrawCount = 0;
			this._spawnerDistortionDrawCount = 0;
			this._spawnerLowResDrawCount = 0;
			this._spawnerBlendDrawCount = 0;
			this._spawnerFPVDrawCount = 0;
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x0019AF30 File Offset: 0x00199130
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterTask(ParticleSystem particleSystem, bool isVisible, float distanceSquared)
		{
			this.RegisterUpdateTask(particleSystem, isVisible);
			if (isVisible)
			{
				this.RegisterDrawTask(particleSystem, distanceSquared);
			}
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x0019AF55 File Offset: 0x00199155
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RegisterUpdateTask(ParticleSystem particleSystem, bool isVisible)
		{
			this._updateTasks[this._updateTaskCount].ParticleSystem = particleSystem;
			this._updateTasks[this._updateTaskCount].IsVisible = isVisible;
			this._updateTaskCount++;
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x0019AF94 File Offset: 0x00199194
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RegisterDrawTask(ParticleSystem particleSystem, float distanceSquared)
		{
			this._drawTasks[this._drawTaskCount] = particleSystem;
			this._drawTaskCount++;
			this._distanceSquaredToCamera[(int)this._sortedDrawTaskCount] = distanceSquared;
			this._sortedDrawTaskIds[(int)this._sortedDrawTaskCount] = this._sortedDrawTaskCount;
			this._sortedDrawTaskCount += 1;
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x0019AFF0 File Offset: 0x001991F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ParticleSystem CreateParticleSystem(ParticleSystemProxy proxy)
		{
			bool isGPULowEnd = this._graphics.IsGPULowEnd;
			UpdateSpawnerLightingFunc updateParticleSpawnerLightingFunc = this._updateParticleSpawnerLightingFunc;
			UpdateParticleCollisionFunc updateParticleCollisionFunc = this._updateParticleCollisionFunc;
			InitParticleFunc initParticleFunc = this._initParticleFunc;
			Vector2 textureAltasInverseSize = proxy.TextureAltasInverseSize;
			int nextParticleSystemId = this._nextParticleSystemId;
			this._nextParticleSystemId = nextParticleSystemId + 1;
			ParticleSystem particleSystem = new ParticleSystem(this, isGPULowEnd, updateParticleSpawnerLightingFunc, updateParticleCollisionFunc, initParticleFunc, textureAltasInverseSize, nextParticleSystemId, proxy.Settings);
			bool flag = particleSystem.Initialize();
			bool flag2 = flag;
			if (flag2)
			{
				particleSystem.Proxy = proxy;
				particleSystem.Scale = proxy.Scale;
				particleSystem.DefaultColor = proxy.DefaultColor;
				particleSystem.IsOvergroundOnly = proxy.IsOvergroundOnly;
				particleSystem.Position = proxy.Position;
				particleSystem.Rotation = proxy.Rotation;
				particleSystem.SetFirstPerson(proxy.IsFirstPerson);
				this._particleSystems.Add(particleSystem.Id, particleSystem);
			}
			return particleSystem;
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x0019B0B8 File Offset: 0x001992B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DeleteSystem(int systemId)
		{
			this._particleSystems[systemId].Dispose();
			this._particleSystems.Remove(systemId);
			ParticleSystemDebug particleSystemDebug;
			bool flag = this._particleSystemDebugs.TryGetValue(systemId, out particleSystemDebug);
			if (flag)
			{
				particleSystemDebug.Dispose();
				this._particleSystemDebugs.Remove(systemId);
			}
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x0019B110 File Offset: 0x00199310
		public void CleanDeadProxies()
		{
			int i = 0;
			while (i < (int)this._particleSystemProxyCount)
			{
				ParticleSystemProxy particleSystemProxy = this._particleSystemProxies[i];
				bool flag = particleSystemProxy.IsExpired || (!particleSystemProxy.IsTracked && particleSystemProxy.ParticleSystem != null && particleSystemProxy.ParticleSystem.IsExpired);
				if (flag)
				{
					bool flag2 = particleSystemProxy.ParticleSystem != null;
					if (flag2)
					{
						particleSystemProxy.ParticleSystem.Expire(particleSystemProxy.HasInstantExpire);
						particleSystemProxy.ParticleSystem = null;
					}
					this._particleSystemProxyCount -= 1;
					this._particleSystemProxies[i] = this._particleSystemProxies[(int)this._particleSystemProxyCount];
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x0019B1C8 File Offset: 0x001993C8
		public void UpdateProxies(Vector3 cameraPosition, bool useProxyCheck)
		{
			for (int i = 0; i < (int)this._particleSystemProxyCount; i++)
			{
				ParticleSystemProxy particleSystemProxy = this._particleSystemProxies[i];
				this._particleSystemProxyDistanceToCamera[i] = (particleSystemProxy.IsLocalPlayer ? 0f : Vector3.DistanceSquared(cameraPosition, particleSystemProxy.Position));
				this._sortedParticleSystemProxyIds[i] = (ushort)i;
			}
			Array.Sort<float, ushort>(this._particleSystemProxyDistanceToCamera, this._sortedParticleSystemProxyIds, 0, (int)this._particleSystemProxyCount);
			int num = 0;
			int j = 0;
			while (j < (int)this._particleSystemProxyCount)
			{
				ushort num2 = this._sortedParticleSystemProxyIds[j];
				float num3 = this._particleSystemProxyDistanceToCamera[j];
				ParticleSystemProxy particleSystemProxy2 = this._particleSystemProxies[(int)num2];
				bool flag = particleSystemProxy2.ParticleSystem == null;
				if (flag)
				{
					bool flag2 = this._particleSystems.Count == this.MaxParticleSystemSpawned;
					if (flag2)
					{
						num++;
					}
					else
					{
						bool flag3 = !useProxyCheck || num3 < particleSystemProxy2.Settings.CullDistanceSquared + 125f;
						if (flag3)
						{
							particleSystemProxy2.ParticleSystem = this.CreateParticleSystem(particleSystemProxy2);
						}
						else
						{
							bool flag4 = !particleSystemProxy2.IsTracked;
							if (flag4)
							{
								particleSystemProxy2.Expire(true);
							}
						}
					}
				}
				else
				{
					particleSystemProxy2.ParticleSystem.Position = particleSystemProxy2.Position;
					particleSystemProxy2.ParticleSystem.Rotation = particleSystemProxy2.Rotation;
					bool flag5 = j >= this.MaxParticleSystemSpawned && num > 0;
					bool flag6 = (useProxyCheck && num3 > particleSystemProxy2.Settings.CullDistanceSquared + 125f) || flag5;
					if (flag6)
					{
						particleSystemProxy2.ParticleSystem.Expire(true);
						particleSystemProxy2.ParticleSystem = null;
						num--;
						bool flag7 = !particleSystemProxy2.IsTracked;
						if (flag7)
						{
							particleSystemProxy2.Expire(true);
						}
					}
				}
				IL_1B1:
				j++;
				continue;
				goto IL_1B1;
			}
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x0019B3A0 File Offset: 0x001995A0
		public void UpdateSimulationOnSingleCore(float deltaTime)
		{
			bool isPaused = this._isPaused;
			if (!isPaused)
			{
				this._accumulatedDeltaTime += deltaTime;
				bool flag = this._accumulatedDeltaTime < this._engineTimeStep;
				if (flag)
				{
					for (int i = 0; i < this._updateTaskCount; i++)
					{
						ParticleSystem particleSystem = this._updateTasks[i].ParticleSystem;
						bool flag2 = this._updateTasks[i].IsVisible || particleSystem.IsImportant;
						if (flag2)
						{
							particleSystem.LightUpdate();
						}
					}
				}
				else
				{
					deltaTime = MathHelper.Min(0.033f, this._accumulatedDeltaTime);
					for (int j = 0; j < this._updateTaskCount; j++)
					{
						ParticleSystem particleSystem2 = this._updateTasks[j].ParticleSystem;
						bool flag3 = this._updateTasks[j].IsVisible || particleSystem2.IsImportant;
						if (flag3)
						{
							particleSystem2.Update(deltaTime);
							bool isExpired = particleSystem2.IsExpired;
							if (isExpired)
							{
								this._expiredParticleSystemIds.Enqueue(particleSystem2.Id);
							}
						}
						else
						{
							particleSystem2.UpdateLife(deltaTime);
							bool isExpired2 = particleSystem2.IsExpired;
							if (isExpired2)
							{
								this._expiredParticleSystemIds.Enqueue(particleSystem2.Id);
							}
						}
					}
					int count = this._expiredParticleSystemIds.Count;
					for (int k = 0; k < count; k++)
					{
						int systemId;
						bool flag4 = this._expiredParticleSystemIds.TryDequeue(out systemId);
						this.DeleteSystem(systemId);
					}
					this._accumulatedDeltaTime = 0f;
				}
			}
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x0019B554 File Offset: 0x00199754
		public void UpdateSimulationOnMultiCore(float deltaTime)
		{
			bool isPaused = this._isPaused;
			if (!isPaused)
			{
				this._accumulatedDeltaTime += deltaTime;
				bool flag = this._accumulatedDeltaTime < this._engineTimeStep;
				if (flag)
				{
					Parallel.For(0, this._updateTaskCount, delegate(int i)
					{
						ParticleSystem particleSystem = this._updateTasks[i].ParticleSystem;
						bool flag3 = this._updateTasks[i].IsVisible || particleSystem.IsImportant;
						if (flag3)
						{
							particleSystem.LightUpdate();
						}
					});
				}
				else
				{
					deltaTime = MathHelper.Min(0.033f, this._accumulatedDeltaTime);
					Parallel.For(0, this._updateTaskCount, delegate(int i)
					{
						ParticleSystem particleSystem = this._updateTasks[i].ParticleSystem;
						bool flag3 = this._updateTasks[i].IsVisible || particleSystem.IsImportant;
						if (flag3)
						{
							particleSystem.Update(deltaTime);
							bool isExpired = particleSystem.IsExpired;
							if (isExpired)
							{
								this._expiredParticleSystemIds.Enqueue(particleSystem.Id);
							}
						}
						else
						{
							particleSystem.UpdateLife(deltaTime);
							bool isExpired2 = particleSystem.IsExpired;
							if (isExpired2)
							{
								this._expiredParticleSystemIds.Enqueue(particleSystem.Id);
							}
						}
					});
					int count = this._expiredParticleSystemIds.Count;
					for (int j = 0; j < count; j++)
					{
						int systemId;
						bool flag2 = this._expiredParticleSystemIds.TryDequeue(out systemId);
						this.DeleteSystem(systemId);
					}
					this._accumulatedDeltaTime = 0f;
				}
			}
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x0019B640 File Offset: 0x00199840
		public void UpdateSimulation(float deltaTime)
		{
			bool useParallelExecution = this._useParallelExecution;
			if (useParallelExecution)
			{
				this.UpdateSimulationOnMultiCore(deltaTime);
			}
			else
			{
				this.UpdateSimulationOnSingleCore(deltaTime);
			}
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x0019B670 File Offset: 0x00199870
		public void DispatchSpawnersDrawTasks(bool sort = true)
		{
			if (sort)
			{
				Array.Sort<float, ushort>(this._distanceSquaredToCamera, this._sortedDrawTaskIds, 0, (int)this._sortedDrawTaskCount);
			}
			int num = 0;
			for (int i = 0; i < (int)this._sortedDrawTaskCount; i++)
			{
				ushort num2 = this._sortedDrawTaskIds[i];
				ParticleSystem particleSystem = this._drawTasks[(int)num2];
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				bool flag = true;
				for (int j = 0; j < particleSystem.AliveSpawnerCount; j++)
				{
					ParticleSpawner particleSpawner = particleSystem.SystemSpawners[j].ParticleSpawner;
					bool flag2 = particleSpawner.ActiveParticles != 0;
					if (flag2)
					{
						num += particleSpawner.ParticleDrawCount;
						bool flag3 = num < 20000;
						if (!flag3)
						{
							flag = false;
							break;
						}
						bool isDistortion = particleSpawner.IsDistortion;
						if (isDistortion)
						{
							ArrayUtils.GrowArrayIfNecessary<ParticleSpawner>(ref this._spawnerDistortionDraw, this._spawnerDistortionDrawCount + 1, 50);
							this._spawnerDistortionDraw[this._spawnerDistortionDrawCount] = particleSpawner;
							this._spawnerDistortionDrawCount++;
							num4++;
						}
						else
						{
							bool flag4 = particleSpawner.RenderMode == FXSystem.RenderMode.Erosion;
							if (flag4)
							{
								ArrayUtils.GrowArrayIfNecessary<ParticleSpawner>(ref this._spawnerErosionDraw, this._spawnerErosionDrawCount + 1, 100);
								this._spawnerErosionDraw[this._spawnerErosionDrawCount] = particleSpawner;
								this._spawnerErosionDrawCount++;
								num3++;
							}
							else
							{
								bool flag5 = this.IsLowResRenderingEnabled && particleSpawner.IsLowRes;
								if (flag5)
								{
									FXSystem.RenderMode renderMode = particleSpawner.RenderMode;
									FXSystem.RenderMode renderMode2 = renderMode;
									if (renderMode2 <= FXSystem.RenderMode.BlendAdd)
									{
										ArrayUtils.GrowArrayIfNecessary<ParticleSpawner>(ref this._spawnerLowResDraw, this._spawnerLowResDrawCount + 1, 200);
										this._spawnerLowResDraw[this._spawnerLowResDrawCount] = particleSpawner;
										this._spawnerLowResDrawCount++;
										num5++;
									}
								}
								else
								{
									bool flag6 = !particleSpawner.IsFirstPerson;
									if (flag6)
									{
										FXSystem.RenderMode renderMode3 = particleSpawner.RenderMode;
										FXSystem.RenderMode renderMode4 = renderMode3;
										if (renderMode4 <= FXSystem.RenderMode.BlendAdd)
										{
											ArrayUtils.GrowArrayIfNecessary<ParticleSpawner>(ref this._spawnerBlendDraw, this._spawnerBlendDrawCount + 1, 200);
											this._spawnerBlendDraw[this._spawnerBlendDrawCount] = particleSpawner;
											this._spawnerBlendDrawCount++;
											num6++;
										}
									}
									else
									{
										ArrayUtils.GrowArrayIfNecessary<ParticleSpawner>(ref this._spawnerFPVDraw, this._spawnerFPVDrawCount + 1, 5);
										this._spawnerFPVDraw[this._spawnerFPVDrawCount] = particleSpawner;
										this._spawnerFPVDrawCount++;
										num7++;
									}
								}
							}
						}
					}
				}
				bool flag7 = !flag;
				if (flag7)
				{
					this._spawnerErosionDrawCount -= num3;
					this._spawnerDistortionDrawCount -= num4;
					this._spawnerLowResDrawCount -= num5;
					this._spawnerBlendDrawCount -= num6;
					this._spawnerFPVDrawCount -= num7;
					this._sortedDrawTaskCount = (ushort)i;
					break;
				}
			}
			Debug.Assert(this._spawnerErosionDrawCount >= 0, string.Format("_spawnerErosionDrawCount ({0})should not be negative.", this._spawnerErosionDrawCount));
			Debug.Assert(this._spawnerDistortionDrawCount >= 0, string.Format("_spawnerDistortionDrawCount ({0})should not be negative.", this._spawnerDistortionDrawCount));
			Debug.Assert(this._spawnerLowResDrawCount >= 0, string.Format("_spawnerLowResDrawCount({0})should not be negative.", this._spawnerLowResDrawCount));
			Debug.Assert(this._spawnerBlendDrawCount >= 0, string.Format("_spawnerBlendDrawCount ({0})should not be negative.", this._spawnerBlendDrawCount));
			Debug.Assert(this._spawnerFPVDrawCount >= 0, string.Format("_spawnerFPVDrawCount ({0})should not be negative.", this._spawnerFPVDrawCount));
			Debug.Assert((int)this._sortedDrawTaskCount <= this._drawTaskCount);
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x0019BA44 File Offset: 0x00199C44
		public void PrepareErosionVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._spawnerErosionDrawCount == 0;
			if (!flag)
			{
				int num = 0;
				for (int i = 0; i < this._spawnerErosionDrawCount; i++)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._spawnerErosionDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._spawnerErosionDraw[i].ParticleDrawCount;
				}
				fXRenderer.ErosionDrawParams.Count = num;
				fXRenderer.ErosionDrawParams.StartOffset = (uint)this._spawnerErosionDraw[0].ParticleVertexDataStartIndex;
			}
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x0019BACC File Offset: 0x00199CCC
		public void PrepareBlendVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._spawnerBlendDrawCount == 0;
			if (!flag)
			{
				int num = 0;
				for (int i = this._spawnerBlendDrawCount - 1; i >= 0; i--)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._spawnerBlendDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._spawnerBlendDraw[i].ParticleDrawCount;
				}
				fXRenderer.BlendDrawParams.Count = num;
				fXRenderer.BlendDrawParams.StartOffset = (uint)this._spawnerBlendDraw[this._spawnerBlendDrawCount - 1].ParticleVertexDataStartIndex;
				fXRenderer.BlendDrawParams.IsStartOffsetSet = true;
			}
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x0019BB70 File Offset: 0x00199D70
		public void PrepareFPVVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._spawnerFPVDrawCount == 0;
			if (!flag)
			{
				int num = 0;
				for (int i = this._spawnerFPVDrawCount - 1; i >= 0; i--)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._spawnerFPVDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._spawnerFPVDraw[i].ParticleDrawCount;
				}
				fXRenderer.BlendFPVDrawParams.Count = num;
				fXRenderer.BlendFPVDrawParams.StartOffset = (uint)this._spawnerFPVDraw[this._spawnerFPVDrawCount - 1].ParticleVertexDataStartIndex;
				fXRenderer.BlendFPVDrawParams.IsStartOffsetSet = true;
			}
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x0019BC14 File Offset: 0x00199E14
		public void PrepareLowResVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._spawnerLowResDrawCount == 0;
			if (!flag)
			{
				int num = 0;
				for (int i = this._spawnerLowResDrawCount - 1; i >= 0; i--)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._spawnerLowResDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._spawnerLowResDraw[i].ParticleDrawCount;
				}
				fXRenderer.BlendLowResDrawParams.Count = num;
				fXRenderer.BlendLowResDrawParams.StartOffset = (uint)this._spawnerLowResDraw[this._spawnerLowResDrawCount - 1].ParticleVertexDataStartIndex;
			}
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x0019BCA8 File Offset: 0x00199EA8
		public void PrepareDistortionVertexDataStorage(FXRenderer fXRenderer)
		{
			bool flag = this._spawnerDistortionDrawCount == 0;
			if (!flag)
			{
				int num = 0;
				for (int i = 0; i < this._spawnerDistortionDrawCount; i++)
				{
					ushort drawId = fXRenderer.ReserveDrawTask();
					this._spawnerDistortionDraw[i].ReserveVertexDataStorage(ref fXRenderer.FXVertexBuffer, drawId);
					num += this._spawnerDistortionDraw[i].ParticleDrawCount;
				}
				fXRenderer.DistortionDrawParams.Count = num;
				fXRenderer.DistortionDrawParams.StartOffset = (uint)this._spawnerDistortionDraw[0].ParticleVertexDataStartIndex;
				fXRenderer.DistortionDrawParams.IsStartOffsetSet = true;
			}
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x0019BD3C File Offset: 0x00199F3C
		private void PrepareForDrawOnSingleCore(FXRenderer fXRenderer, Vector3 cameraPosition, IntPtr dataPtr)
		{
			for (int i = 0; i < (int)this._sortedDrawTaskCount; i++)
			{
				ushort num = this._sortedDrawTaskIds[i];
				this._drawTasks[(int)num].PrepareForDraw(cameraPosition, ref fXRenderer.FXVertexBuffer, dataPtr);
			}
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x0019BD80 File Offset: 0x00199F80
		private void PrepareForDrawOnMultiCore(FXRenderer fXRenderer, Vector3 cameraPosition, IntPtr gpuDrawDataPtr)
		{
			Parallel.For(0, (int)this._sortedDrawTaskCount, delegate(int i)
			{
				ushort num = this._sortedDrawTaskIds[i];
				this._drawTasks[(int)num].PrepareForDraw(cameraPosition, ref fXRenderer.FXVertexBuffer, gpuDrawDataPtr);
			});
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x0019BDCC File Offset: 0x00199FCC
		public void PrepareForDraw(FXRenderer fXRenderer, Vector3 cameraPosition, IntPtr dataPtr)
		{
			bool useParallelExecution = this._useParallelExecution;
			if (useParallelExecution)
			{
				this.PrepareForDrawOnMultiCore(fXRenderer, cameraPosition, dataPtr);
			}
			else
			{
				this.PrepareForDrawOnSingleCore(fXRenderer, cameraPosition, dataPtr);
			}
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x0019BDFE File Offset: 0x00199FFE
		public void AddParticleSystemDebug(ParticleSystem particleSystem)
		{
			this._particleSystemDebugs[particleSystem.Id] = new ParticleSystemDebug(this._graphics, particleSystem);
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x0019BE20 File Offset: 0x0019A020
		public void DrawDebugInfo(ref Matrix viewRotationProjectionMatrix)
		{
			foreach (ParticleSystemDebug particleSystemDebug in this._particleSystemDebugs.Values)
			{
				particleSystemDebug.Draw(viewRotationProjectionMatrix);
			}
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x0019BE84 File Offset: 0x0019A084
		public void DrawDebugBoundingVolumes(ref Vector3 cameraPosition, ref Matrix viewRotationProjectionMatrix)
		{
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._debugSphereMesh.VertexArray);
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			gl.UseProgram(basicProgram);
			basicProgram.Opacity.SetValue(1f);
			Vector3 value = new Vector3(1f, 1f, 1f);
			Vector3 vector = new Vector3(0.65f, 0.65f, 0.65f);
			Vector3 vector2 = new Vector3(0.5f, 1f, 1f);
			Vector3 vector3 = new Vector3(0f, 1f, 0f);
			int i = 0;
			while (i < (int)this._particleSystemProxyCount)
			{
				ParticleSystemProxy particleSystemProxy = this._particleSystemProxies[i];
				float boundingRadius = particleSystemProxy.Settings.BoundingRadius;
				Vector3 vector4 = particleSystemProxy.Position;
				BoundingSphere boundingSphere;
				boundingSphere.Center = vector4;
				boundingSphere.Radius = boundingRadius;
				vector4 -= cameraPosition;
				Matrix matrix;
				Matrix.CreateScale(boundingRadius, out matrix);
				Matrix.AddTranslation(ref matrix, vector4.X, vector4.Y, vector4.Z);
				Matrix.Multiply(ref matrix, ref viewRotationProjectionMatrix, out matrix);
				bool flag = particleSystemProxy.ParticleSystem == null;
				if (flag)
				{
					value = vector;
					goto IL_17E;
				}
				ParticleSystem particleSystem = particleSystemProxy.ParticleSystem;
				bool isFirstPerson = particleSystem.IsFirstPerson;
				if (!isFirstPerson)
				{
					float num = Vector3.DistanceSquared(cameraPosition, particleSystem.Position);
					value = ((num < particleSystem.CullDistanceSquared) ? vector3 : vector2);
					goto IL_17E;
				}
				IL_1B9:
				i++;
				continue;
				IL_17E:
				basicProgram.Color.SetValue(value);
				basicProgram.MVPMatrix.SetValue(ref matrix);
				gl.DrawElements(GL.TRIANGLES, this._debugSphereMesh.Count, GL.UNSIGNED_SHORT, (IntPtr)0);
				goto IL_1B9;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06005640 RID: 22080 RVA: 0x0019C063 File Offset: 0x0019A263
		public unsafe ParticleBuffers ParticleBuffer
		{
			get
			{
				return *this._particleMemoryPool.Storage;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x06005641 RID: 22081 RVA: 0x0019C075 File Offset: 0x0019A275
		public int ParticleBufferStorageMaxCount
		{
			get
			{
				return this._particleMemoryPool.ItemMaxCount;
			}
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x0019C082 File Offset: 0x0019A282
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int RequestParticleBufferStorage(int count)
		{
			return this._particleMemoryPool.TakeSlots(count);
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x0019C090 File Offset: 0x0019A290
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReleaseParticleBufferStorage(int particleStartIndex, int particleCount)
		{
			this._particleMemoryPool.ReleaseSlots(particleStartIndex, particleCount);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x0019C0A0 File Offset: 0x0019A2A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearParticleBufferStorage()
		{
			this._particleMemoryPool.Clear();
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x0019C0AE File Offset: 0x0019A2AE
		public void ResetDrawStates()
		{
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x0019C0B1 File Offset: 0x0019A2B1
		private void ResetMapFXTaskCounters()
		{
			this._incomingMapParticlesTaskCount = 0;
			this._animatedBlockParticleUpdateTaskCount = 0;
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x0019C0C2 File Offset: 0x0019A2C2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingAnimatedBlockParticlesTasks(int size)
		{
			this._incomingMapParticlesTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<ParticleFXSystem.AnimatedBlockParticleUpdateTask>(ref this._animatedBlockParticleUpdateTasks, this._animatedBlockParticleUpdateTaskCount + size, 25);
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x0019C0E9 File Offset: 0x0019A2E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterFXAnimatedBlockParticlesTask(AnimatedBlockRenderer animatedBlockRenderer, RenderedChunk.MapParticle mapParticle)
		{
			this._animatedBlockParticleUpdateTasks[this._animatedBlockParticleUpdateTaskCount].AnimatedBlockRenderer = animatedBlockRenderer;
			this._animatedBlockParticleUpdateTasks[this._animatedBlockParticleUpdateTaskCount].MapParticle = mapParticle;
			this._animatedBlockParticleUpdateTaskCount++;
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x0019C128 File Offset: 0x0019A328
		public void UpdateAnimatedBlockParticles()
		{
			for (int i = 0; i < this._animatedBlockParticleUpdateTaskCount; i++)
			{
				RenderedChunk.MapParticle mapParticle = this._animatedBlockParticleUpdateTasks[i].MapParticle;
				ref AnimatedRenderer.NodeTransform ptr = ref this._animatedBlockParticleUpdateTasks[i].AnimatedBlockRenderer.NodeTransforms[mapParticle.TargetNodeIndex];
				mapParticle.ParticleSystemProxy.Position = mapParticle.Position + Vector3.Transform(ptr.Position * 0.03125f * mapParticle.BlockScale, this._animatedBlockParticleUpdateTasks[i].MapParticle.Rotation) + Vector3.Transform(mapParticle.PositionOffset * mapParticle.BlockScale, this._animatedBlockParticleUpdateTasks[i].MapParticle.Rotation * ptr.Orientation);
				mapParticle.ParticleSystemProxy.Rotation = this._animatedBlockParticleUpdateTasks[i].MapParticle.Rotation * ptr.Orientation * mapParticle.RotationOffset;
			}
		}

		// Token: 0x04003344 RID: 13124
		private const float MaxDeltaTime = 0.033f;

		// Token: 0x04003345 RID: 13125
		public bool IsLowResRenderingEnabled = true;

		// Token: 0x04003346 RID: 13126
		public int PreviousFrameBlendDrawCount;

		// Token: 0x04003347 RID: 13127
		public int PreviousFrameErosionDrawCount;

		// Token: 0x04003348 RID: 13128
		public int PreviousFrameDistortionDrawCount;

		// Token: 0x04003349 RID: 13129
		private GraphicsDevice _graphics;

		// Token: 0x0400334A RID: 13130
		private float _engineTimeStep;

		// Token: 0x0400334B RID: 13131
		private bool _isPaused;

		// Token: 0x0400334C RID: 13132
		private bool _useParallelExecution = true;

		// Token: 0x0400334D RID: 13133
		private const int ProxyDefaultSize = 50;

		// Token: 0x0400334E RID: 13134
		private const int ProxyGrowth = 100;

		// Token: 0x0400334F RID: 13135
		private int _maxParticleSystemSpawned = 300;

		// Token: 0x04003350 RID: 13136
		private UpdateSpawnerLightingFunc _updateParticleSpawnerLightingFunc;

		// Token: 0x04003351 RID: 13137
		private UpdateParticleCollisionFunc _updateParticleCollisionFunc;

		// Token: 0x04003352 RID: 13138
		private InitParticleFunc _initParticleFunc;

		// Token: 0x04003353 RID: 13139
		private ConcurrentQueue<int> _expiredParticleSystemIds;

		// Token: 0x04003354 RID: 13140
		private ushort _particleSystemProxyCount = 0;

		// Token: 0x04003355 RID: 13141
		private ParticleSystemProxy[] _particleSystemProxies;

		// Token: 0x04003356 RID: 13142
		private float[] _particleSystemProxyDistanceToCamera;

		// Token: 0x04003357 RID: 13143
		private ushort[] _sortedParticleSystemProxyIds;

		// Token: 0x04003358 RID: 13144
		private Dictionary<int, ParticleSystemDebug> _particleSystemDebugs;

		// Token: 0x04003359 RID: 13145
		private Dictionary<int, ParticleSystem> _particleSystems;

		// Token: 0x0400335A RID: 13146
		private Mesh _debugSphereMesh;

		// Token: 0x0400335B RID: 13147
		private int _nextParticleSystemId = 0;

		// Token: 0x0400335C RID: 13148
		private float _accumulatedDeltaTime;

		// Token: 0x0400335D RID: 13149
		private ParticleFXSystem.UpdateTask[] _updateTasks;

		// Token: 0x0400335E RID: 13150
		private int _updateTaskCount = 0;

		// Token: 0x0400335F RID: 13151
		private ParticleSystem[] _drawTasks;

		// Token: 0x04003360 RID: 13152
		private int _drawTaskCount = 0;

		// Token: 0x04003361 RID: 13153
		private float[] _distanceSquaredToCamera;

		// Token: 0x04003362 RID: 13154
		private ushort[] _sortedDrawTaskIds;

		// Token: 0x04003363 RID: 13155
		private ushort _sortedDrawTaskCount;

		// Token: 0x04003364 RID: 13156
		private FXMemoryPool<ParticleBuffers> _particleMemoryPool;

		// Token: 0x04003365 RID: 13157
		private const int _maxParticleDrawCount = 20000;

		// Token: 0x04003366 RID: 13158
		private const int SpawnerDrawDefaultSize = 200;

		// Token: 0x04003367 RID: 13159
		private const int SpawnerDrawGrowth = 200;

		// Token: 0x04003368 RID: 13160
		private const int SpawnerFPVDrawDefaultSize = 10;

		// Token: 0x04003369 RID: 13161
		private const int SpawnerFPVDrawGrowth = 5;

		// Token: 0x0400336A RID: 13162
		private const int SpawnerDistortionDrawDefaultSize = 50;

		// Token: 0x0400336B RID: 13163
		private const int SpawnerDistortionDrawGrowth = 50;

		// Token: 0x0400336C RID: 13164
		private const int SpawnerErosionDrawDefaultSize = 100;

		// Token: 0x0400336D RID: 13165
		private const int SpawnerErosionDrawGrowth = 100;

		// Token: 0x0400336E RID: 13166
		private ParticleSpawner[] _spawnerErosionDraw = new ParticleSpawner[100];

		// Token: 0x0400336F RID: 13167
		private ParticleSpawner[] _spawnerLowResDraw = new ParticleSpawner[200];

		// Token: 0x04003370 RID: 13168
		private ParticleSpawner[] _spawnerBlendDraw = new ParticleSpawner[200];

		// Token: 0x04003371 RID: 13169
		private ParticleSpawner[] _spawnerFPVDraw = new ParticleSpawner[10];

		// Token: 0x04003372 RID: 13170
		private ParticleSpawner[] _spawnerDistortionDraw = new ParticleSpawner[50];

		// Token: 0x04003373 RID: 13171
		private int _spawnerBlendDrawCount = 0;

		// Token: 0x04003374 RID: 13172
		private int _spawnerLowResDrawCount = 0;

		// Token: 0x04003375 RID: 13173
		private int _spawnerFPVDrawCount = 0;

		// Token: 0x04003376 RID: 13174
		private int _spawnerDistortionDrawCount = 0;

		// Token: 0x04003377 RID: 13175
		private int _spawnerErosionDrawCount = 0;

		// Token: 0x04003378 RID: 13176
		private int _incomingMapParticlesTaskCount;

		// Token: 0x04003379 RID: 13177
		private const int AnimatedBlockParticleUpdateTasksDefaultSize = 100;

		// Token: 0x0400337A RID: 13178
		private const int AnimatedBlockParticleUpdateTasksGrowth = 25;

		// Token: 0x0400337B RID: 13179
		private ParticleFXSystem.AnimatedBlockParticleUpdateTask[] _animatedBlockParticleUpdateTasks = new ParticleFXSystem.AnimatedBlockParticleUpdateTask[100];

		// Token: 0x0400337C RID: 13180
		private int _animatedBlockParticleUpdateTaskCount;

		// Token: 0x02000EF2 RID: 3826
		public enum ParticleRotationInfluence
		{
			// Token: 0x04004954 RID: 18772
			None,
			// Token: 0x04004955 RID: 18773
			Billboard,
			// Token: 0x04004956 RID: 18774
			BillboardY,
			// Token: 0x04004957 RID: 18775
			BillboardVelocity,
			// Token: 0x04004958 RID: 18776
			Velocity
		}

		// Token: 0x02000EF3 RID: 3827
		public enum ParticleCollisionBlockType
		{
			// Token: 0x0400495A RID: 18778
			None,
			// Token: 0x0400495B RID: 18779
			Air,
			// Token: 0x0400495C RID: 18780
			Solid,
			// Token: 0x0400495D RID: 18781
			All
		}

		// Token: 0x02000EF4 RID: 3828
		public enum ParticleCollisionAction
		{
			// Token: 0x0400495F RID: 18783
			Expire,
			// Token: 0x04004960 RID: 18784
			LastFrame,
			// Token: 0x04004961 RID: 18785
			Linger
		}

		// Token: 0x02000EF5 RID: 3829
		private struct UpdateTask
		{
			// Token: 0x04004962 RID: 18786
			public ParticleSystem ParticleSystem;

			// Token: 0x04004963 RID: 18787
			public bool IsVisible;
		}

		// Token: 0x02000EF6 RID: 3830
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct AnimatedBlockParticleUpdateTask
		{
			// Token: 0x04004964 RID: 18788
			public AnimatedBlockRenderer AnimatedBlockRenderer;

			// Token: 0x04004965 RID: 18789
			public RenderedChunk.MapParticle MapParticle;
		}
	}
}
