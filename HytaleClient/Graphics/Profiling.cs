using System;
using System.Diagnostics;
using HytaleClient.Common.Collections;
using HytaleClient.Core;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A42 RID: 2626
	internal class Profiling : Disposable
	{
		// Token: 0x060052B0 RID: 21168 RVA: 0x0016F1E4 File Offset: 0x0016D3E4
		public ref Profiling.MeasureInfo GetMeasureInfo(int id)
		{
			return ref this._measureInfos[id];
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x0016F1F2 File Offset: 0x0016D3F2
		public ref Profiling.CPUMeasure GetCPUMeasure(int id)
		{
			return ref this._cpuMeasures[id];
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x0016F200 File Offset: 0x0016D400
		public ref Profiling.GPUMeasure GetGPUMeasure(int id)
		{
			return ref this._gpuMeasures[id];
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x0016F20E File Offset: 0x0016D40E
		public string GetMeasureName(int id)
		{
			return this._measureNames[id];
		}

		// Token: 0x170012CF RID: 4815
		// (get) Token: 0x060052B4 RID: 21172 RVA: 0x0016F218 File Offset: 0x0016D418
		// (set) Token: 0x060052B5 RID: 21173 RVA: 0x0016F220 File Offset: 0x0016D420
		public int MeasureCount { get; private set; }

		// Token: 0x060052B6 RID: 21174 RVA: 0x0016F229 File Offset: 0x0016D429
		public Profiling(GLFunctions gl = null)
		{
			this._gl = gl;
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x0016F248 File Offset: 0x0016D448
		public void Initialize(int maxMeasures, int iterationsBeforeReset = 200)
		{
			bool flag = this._maxMeasures != 0;
			if (flag)
			{
				this.Release();
			}
			Debug.Assert(this._maxMeasures == 0, "Profiling already Initialize()'d. Please Release() before re-using it.");
			this._iterationsBeforeReset = iterationsBeforeReset;
			this._maxMeasures = maxMeasures;
			this._measureNames = new string[this._maxMeasures];
			this._measureInfos = new Profiling.MeasureInfo[this._maxMeasures];
			this._cpuMeasures = new Profiling.CPUMeasure[this._maxMeasures];
			this._measureAtStart = new Profiling.MeasureAtStart[this._maxMeasures];
			this._gpuMeasures = new Profiling.GPUMeasure[this._maxMeasures];
			this._gpuTimers = new GPUTimer[this._maxMeasures];
			this._measureIdUsed = new NativeArray<bool>(this._maxMeasures, 0, 0);
			this._initialized = true;
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x0016F30C File Offset: 0x0016D50C
		private void Release()
		{
			Debug.Assert(this._maxMeasures != 0, "Profiling never Initialize()'d - or already Release()'d.");
			bool flag = !this._initialized;
			if (!flag)
			{
				this._measureIdUsed.Dispose();
				for (int i = 0; i < this.MeasureCount; i++)
				{
					bool hasGpuStats = this._measureInfos[i].HasGpuStats;
					if (hasGpuStats)
					{
						this._gpuTimers[i].DestroyStorage();
					}
				}
				this._gpuTimers = null;
				this._gpuMeasures = null;
				this._measureAtStart = null;
				this._cpuMeasures = null;
				this._measureInfos = null;
				this._measureNames = null;
				this.MeasureCount = 0;
				this._maxMeasures = 0;
			}
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x0016F3C8 File Offset: 0x0016D5C8
		protected override void DoDispose()
		{
			bool flag = this._maxMeasures != 0;
			if (flag)
			{
				this.Release();
			}
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x0016F3EC File Offset: 0x0016D5EC
		public void SwapMeasureBuffers()
		{
			for (int i = 0; i < this.MeasureCount; i++)
			{
				bool hasGpuStats = this._measureInfos[i].HasGpuStats;
				if (hasGpuStats)
				{
					this._gpuTimers[i].Swap();
				}
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x0016F43C File Offset: 0x0016D63C
		public void CreateMeasure(string name, int measureId, bool cpuOnly = false, bool alwaysEnabled = false, bool isExternal = false)
		{
			Debug.Assert(measureId < this._maxMeasures, "Total amount of Profiling measures has been exceeded.");
			Debug.Assert(!this._measureIdUsed[measureId], string.Format("Measure Id {0} is already in use.", measureId));
			Debug.Assert(this._gl != null || cpuOnly, "Trying to create a gpu measure when the gpu profiling is not enabled.");
			this._measureIdUsed[measureId] = true;
			this._measureNames[measureId] = name;
			this._measureInfos[measureId].HasGpuStats = !cpuOnly;
			this._measureInfos[measureId].IsAlwaysEnabled = alwaysEnabled;
			this._measureInfos[measureId].IsExternal = isExternal;
			bool flag = !cpuOnly;
			if (flag)
			{
				this._gpuTimers[measureId].CreateStorage(true);
			}
			int measureCount = this.MeasureCount;
			this.MeasureCount = measureCount + 1;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x0016F519 File Offset: 0x0016D719
		public void SetMeasureEnabled(int measureId, bool enabled)
		{
			this._measureInfos[measureId].IsEnabled = enabled;
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x0016F530 File Offset: 0x0016D730
		public void StartMeasure(int measureId)
		{
			Debug.Assert(!this._measureInfos[measureId].IsExternal);
			ref Profiling.MeasureInfo ptr = ref this._measureInfos[measureId];
			bool flag = !ptr.IsEnabled && !ptr.IsAlwaysEnabled;
			if (!flag)
			{
				ref Profiling.MeasureAtStart ptr2 = ref this._measureAtStart[measureId];
				bool hasGpuStats = ptr.HasGpuStats;
				if (hasGpuStats)
				{
					ptr2.DrawCalls = this._gl.DrawCallsCount;
					ptr2.DrawnVertices = this._gl.DrawnVertices;
					this._gpuTimers[measureId].RequestStart();
				}
				ptr2.ElapsedTime = (float)this._stopwatch.Elapsed.TotalMilliseconds;
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x0016F5E8 File Offset: 0x0016D7E8
		public void StopMeasure(int measureId)
		{
			Debug.Assert(!this._measureInfos[measureId].IsExternal);
			ref Profiling.MeasureInfo ptr = ref this._measureInfos[measureId];
			bool flag = !ptr.IsEnabled && !ptr.IsAlwaysEnabled;
			if (!flag)
			{
				ref Profiling.CPUMeasure ptr2 = ref this._cpuMeasures[measureId];
				ref Profiling.GPUMeasure ptr3 = ref this._gpuMeasures[measureId];
				ref Profiling.MeasureAtStart ptr4 = ref this._measureAtStart[measureId];
				bool flag2 = ptr.AccumulatedFrameCount > this._iterationsBeforeReset;
				bool flag3 = flag2;
				if (flag3)
				{
					ptr.AccumulatedFrameCount = 0;
					ptr2.AccumulatedElapsedTime = 0f;
					ptr3.AccumulatedElapsedTime = 0f;
				}
				float num = (float)this._stopwatch.Elapsed.TotalMilliseconds - ptr4.ElapsedTime;
				ptr2.ElapsedTime = num;
				ptr2.AccumulatedElapsedTime += num;
				ptr2.MaxElapsedTime = Math.Max(ptr2.MaxElapsedTime, num);
				bool hasGpuStats = ptr.HasGpuStats;
				if (hasGpuStats)
				{
					ptr3.DrawCalls = this._gl.DrawCallsCount - ptr4.DrawCalls;
					ptr3.DrawnVertices = this._gl.DrawnVertices - ptr4.DrawnVertices;
					this._gpuTimers[measureId].RequestStop();
					bool flag4 = ptr.AccumulatedFrameCount > 0 || flag2;
					if (flag4)
					{
						this._gpuTimers[measureId].FetchPreviousResultFromGPU();
						float num2 = (float)this._gpuTimers[measureId].ElapsedTimeInMilliseconds;
						ptr3.MaxElapsedTime = Math.Max(ptr3.MaxElapsedTime, num2);
						ptr3.AccumulatedElapsedTime += num2;
						ptr3.ElapsedTime = num2;
					}
				}
				ptr.AccumulatedFrameCount++;
			}
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x0016F7A0 File Offset: 0x0016D9A0
		public void RegisterExternalMeasure(int measureId, float cpuMeasuredTime, float gpuMeasuredTime = 0f)
		{
			Debug.Assert(this._measureInfos[measureId].IsExternal);
			ref Profiling.MeasureInfo ptr = ref this._measureInfos[measureId];
			ref Profiling.CPUMeasure ptr2 = ref this._cpuMeasures[measureId];
			ref Profiling.GPUMeasure ptr3 = ref this._gpuMeasures[measureId];
			bool flag = ptr.AccumulatedFrameCount > this._iterationsBeforeReset;
			bool flag2 = flag;
			if (flag2)
			{
				ptr.AccumulatedFrameCount = 0;
				ptr2.AccumulatedElapsedTime = 0f;
				ptr3.AccumulatedElapsedTime = 0f;
			}
			ptr2.AccumulatedElapsedTime += cpuMeasuredTime;
			ptr2.ElapsedTime = cpuMeasuredTime;
			ptr2.MaxElapsedTime = Math.Max(cpuMeasuredTime, ptr2.MaxElapsedTime);
			ptr3.AccumulatedElapsedTime += gpuMeasuredTime;
			ptr3.ElapsedTime = gpuMeasuredTime;
			ptr3.MaxElapsedTime = Math.Max(gpuMeasuredTime, ptr3.MaxElapsedTime);
			ptr.AccumulatedFrameCount++;
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x0016F874 File Offset: 0x0016DA74
		public void ClearMeasure(int measureId)
		{
			this._measureInfos[measureId].AccumulatedFrameCount = 0;
			this._cpuMeasures[measureId].AccumulatedElapsedTime = 0f;
			this._cpuMeasures[measureId].MaxElapsedTime = 0f;
			this._gpuMeasures[measureId].AccumulatedElapsedTime = 0f;
			this._gpuMeasures[measureId].MaxElapsedTime = 0f;
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x0016F8EC File Offset: 0x0016DAEC
		public void SkipMeasure(int measureId)
		{
			ref Profiling.MeasureInfo ptr = ref this._measureInfos[measureId];
			bool flag = !ptr.IsEnabled && !ptr.IsAlwaysEnabled;
			if (!flag)
			{
				ref Profiling.CPUMeasure ptr2 = ref this._cpuMeasures[measureId];
				ref Profiling.GPUMeasure ptr3 = ref this._gpuMeasures[measureId];
				ptr.AccumulatedFrameCount = 0;
				ptr2.AccumulatedElapsedTime = 0f;
				ptr2.ElapsedTime = 0f;
				ptr3.AccumulatedElapsedTime = 0f;
				ptr3.ElapsedTime = 0f;
				ptr3.DrawCalls = 0;
				ptr3.DrawnVertices = 0;
			}
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x0016F97C File Offset: 0x0016DB7C
		public string WriteMeasures()
		{
			string text = "Measure Name ............................ CPU avg (max) [ -- GPU avg -- Draw calls -- Triangles])";
			for (int i = 0; i < this.MeasureCount; i++)
			{
				bool flag = this._measureInfos[i].AccumulatedFrameCount > 1;
				if (flag)
				{
					int num = i;
					float num2 = (float)Math.Round((double)(this._cpuMeasures[num].AccumulatedElapsedTime / (float)this._measureInfos[num].AccumulatedFrameCount), 4);
					float num3 = (float)Math.Round((double)this._cpuMeasures[num].MaxElapsedTime, 4);
					string text2 = "\n" + i.ToString() + "." + this._measureNames[num];
					int num4 = 0;
					while ((double)num4 < 64.0 - (double)text2.Length * 0.8)
					{
						text2 += ".";
						num4++;
					}
					text2 += string.Format("{0} ({1})", num2, num3);
					bool hasGpuStats = this._measureInfos[i].HasGpuStats;
					if (hasGpuStats)
					{
						float num5 = (float)Math.Round((double)(this._gpuMeasures[num].AccumulatedElapsedTime / (float)this._measureInfos[num].AccumulatedFrameCount), 4);
						int num6 = this._gpuMeasures[num].DrawnVertices / 3;
						text2 += string.Format(" -- {0} -- {1} -- {2} => {3} tris/ms", new object[]
						{
							num5,
							this._gpuMeasures[num].DrawCalls,
							num6,
							(float)num6 / num5
						});
					}
					text += text2;
				}
			}
			return text;
		}

		// Token: 0x04002DA1 RID: 11681
		private GLFunctions _gl;

		// Token: 0x04002DA2 RID: 11682
		private int _maxMeasures;

		// Token: 0x04002DA3 RID: 11683
		private int _iterationsBeforeReset;

		// Token: 0x04002DA4 RID: 11684
		private bool _initialized;

		// Token: 0x04002DA5 RID: 11685
		private string[] _measureNames;

		// Token: 0x04002DA6 RID: 11686
		private Profiling.MeasureInfo[] _measureInfos;

		// Token: 0x04002DA7 RID: 11687
		private Profiling.CPUMeasure[] _cpuMeasures;

		// Token: 0x04002DA8 RID: 11688
		private Profiling.MeasureAtStart[] _measureAtStart;

		// Token: 0x04002DA9 RID: 11689
		private Profiling.GPUMeasure[] _gpuMeasures;

		// Token: 0x04002DAA RID: 11690
		private GPUTimer[] _gpuTimers;

		// Token: 0x04002DAB RID: 11691
		private NativeArray<bool> _measureIdUsed;

		// Token: 0x04002DAC RID: 11692
		private Stopwatch _stopwatch = Stopwatch.StartNew();

		// Token: 0x02000EB6 RID: 3766
		public struct MeasureInfo
		{
			// Token: 0x040047D5 RID: 18389
			public bool IsEnabled;

			// Token: 0x040047D6 RID: 18390
			public bool IsAlwaysEnabled;

			// Token: 0x040047D7 RID: 18391
			public bool IsExternal;

			// Token: 0x040047D8 RID: 18392
			public bool HasGpuStats;

			// Token: 0x040047D9 RID: 18393
			public int AccumulatedFrameCount;
		}

		// Token: 0x02000EB7 RID: 3767
		public struct CPUMeasure
		{
			// Token: 0x040047DA RID: 18394
			public float AccumulatedElapsedTime;

			// Token: 0x040047DB RID: 18395
			public float ElapsedTime;

			// Token: 0x040047DC RID: 18396
			public float MaxElapsedTime;
		}

		// Token: 0x02000EB8 RID: 3768
		public struct GPUMeasure
		{
			// Token: 0x040047DD RID: 18397
			public float AccumulatedElapsedTime;

			// Token: 0x040047DE RID: 18398
			public float ElapsedTime;

			// Token: 0x040047DF RID: 18399
			public float MaxElapsedTime;

			// Token: 0x040047E0 RID: 18400
			public int DrawCalls;

			// Token: 0x040047E1 RID: 18401
			public int DrawnVertices;
		}

		// Token: 0x02000EB9 RID: 3769
		private struct MeasureAtStart
		{
			// Token: 0x040047E2 RID: 18402
			public float ElapsedTime;

			// Token: 0x040047E3 RID: 18403
			public int DrawCalls;

			// Token: 0x040047E4 RID: 18404
			public int DrawnVertices;
		}
	}
}
