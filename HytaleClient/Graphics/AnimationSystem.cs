using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x0200099B RID: 2459
	internal class AnimationSystem
	{
		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06004E91 RID: 20113 RVA: 0x0015DD12 File Offset: 0x0015BF12
		// (set) Token: 0x06004E92 RID: 20114 RVA: 0x0015DD1A File Offset: 0x0015BF1A
		public bool HasProcessed { get; private set; } = false;

		// Token: 0x06004E93 RID: 20115 RVA: 0x0015DD23 File Offset: 0x0015BF23
		public void SetEnabled(bool enable)
		{
			this._isEnabled = enable;
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06004E94 RID: 20116 RVA: 0x0015DD2D File Offset: 0x0015BF2D
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x0015DD38 File Offset: 0x0015BF38
		public bool UseParallelExecution(bool enable)
		{
			this._useParallelExecution = enable;
			return enable;
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x0015DD4F File Offset: 0x0015BF4F
		public void SetTransferMethod(AnimationSystem.TransferMethod methodId)
		{
			Debug.Assert(methodId < AnimationSystem.TransferMethod.MAX, string.Format("Animation data transfer methodId {0} is invalid. We only have 3 methods to transfer the animation data.", methodId));
			this._transferMethodId = methodId;
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06004E97 RID: 20119 RVA: 0x0015DD73 File Offset: 0x0015BF73
		public GLBuffer NodeBuffer
		{
			get
			{
				return this._nodeBuffer.Current;
			}
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x0015DD80 File Offset: 0x0015BF80
		public AnimationSystem(GLFunctions gl)
		{
			this._gl = gl;
			int[] array = new int[1];
			this._gl.GetIntegerv(GL.UNIFORM_BUFFER_OFFSET_ALIGNMENT, array);
			this.UniformBufferOffsetAlignment = array[0];
			this.CreateGPUData();
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x0015DE01 File Offset: 0x0015C001
		public void Dispose()
		{
			this.DestroyGPUData();
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x0015DE0B File Offset: 0x0015C00B
		private void CreateGPUData()
		{
			this._nodeBuffer.CreateStorage(GL.UNIFORM_BUFFER, GL.STREAM_DRAW, this._useDoubleBuffering, this._nodeBufferSize, 8192000U, GPUBuffer.GrowthPolicy.GrowthManual, 0U);
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x0015DE37 File Offset: 0x0015C037
		private void DestroyGPUData()
		{
			this._nodeBuffer.DestroyStorage();
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x0015DE46 File Offset: 0x0015C046
		public void BeginFrame()
		{
			this._animationTaskCount = 0;
			this._incomingAnimationTaskCount = 0;
			this._nodeTransferSize = 0U;
			this._processedAnimationTaskCount = 0;
			this._processedNodeTransferSize = 0U;
			this.PingPongBuffers();
			this.HasProcessed = false;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x0015DE7C File Offset: 0x0015C07C
		private void PingPongBuffers()
		{
			bool useDoubleBuffering = this._useDoubleBuffering;
			if (useDoubleBuffering)
			{
				this._nodeBuffer.Swap();
			}
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x0015DEA0 File Offset: 0x0015C0A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrepareForIncomingTasks(int size)
		{
			this._incomingAnimationTaskCount += size;
			ArrayUtils.GrowArrayIfNecessary<AnimationSystem.AnimationTask>(ref this._animationTasks, this._incomingAnimationTaskCount, 250);
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x0015DEC8 File Offset: 0x0015C0C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterAnimationTask(AnimatedRenderer renderer, bool skipUpdate)
		{
			Debug.Assert(!renderer.SelfManagedNodeBuffer, "AnimationSystem cannot process an AnimatedRenderer that is self managing its node buffer.");
			this._animationTasks[this._animationTaskCount].Renderer = renderer;
			this._animationTasks[this._animationTaskCount].SkipUpdate = skipUpdate;
			this._animationTaskCount++;
			renderer.NodeBufferOffset = this.ReserveStorage((uint)renderer.NodeCount);
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x0015DF38 File Offset: 0x0015C138
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private uint ReserveStorage(uint nodeCount)
		{
			uint nodeTransferSize = this._nodeTransferSize;
			this._nodeTransferSize += nodeCount * 64U;
			long num = (long)((ulong)this._nodeTransferSize % (ulong)((long)this.UniformBufferOffsetAlignment));
			this._nodeTransferSize += ((num != 0L) ? ((uint)((long)this.UniformBufferOffsetAlignment - num)) : 0U);
			return nodeTransferSize;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x0015DF90 File Offset: 0x0015C190
		private unsafe void SendDataToGPU(bool sendInParallel = false)
		{
			uint num = this._nodeTransferSize - this._processedNodeTransferSize;
			bool flag = num > 0U;
			if (flag)
			{
				bool flag2 = this._nodeBuffer.GrowStorageIfNecessary(this._nodeTransferSize);
				num = (flag2 ? this._nodeTransferSize : num);
				uint transferStartOffset = flag2 ? 0U : this._processedNodeTransferSize;
				int num2 = flag2 ? 0 : this._processedAnimationTaskCount;
				IntPtr dataPtr = this._nodeBuffer.BeginTransfer(num, transferStartOffset);
				Debug.Assert(dataPtr != IntPtr.Zero);
				bool flag3 = !sendInParallel;
				if (flag3)
				{
					Matrix* ptr = (Matrix*)dataPtr.ToPointer();
					for (int k = num2; k < this._animationTaskCount; k++)
					{
						AnimationSystem.AnimationTask animationTask = this._animationTasks[k];
						uint num3 = (animationTask.Renderer.NodeBufferOffset - this._processedNodeTransferSize) / 64U;
						for (int j = 0; j < (int)animationTask.Renderer.NodeCount; j++)
						{
							ptr[((ulong)num3 + (ulong)((long)j)) * (ulong)((long)sizeof(Matrix)) / (ulong)sizeof(Matrix)] = animationTask.Renderer.NodeMatrices[j];
						}
					}
				}
				else
				{
					Parallel.For(num2, this._animationTaskCount, delegate(int i)
					{
						Matrix* ptr2 = (Matrix*)dataPtr.ToPointer();
						AnimationSystem.AnimationTask animationTask2 = this._animationTasks[i];
						uint num4 = (animationTask2.Renderer.NodeBufferOffset - this._processedNodeTransferSize) / 64U;
						for (int l = 0; l < (int)animationTask2.Renderer.NodeCount; l++)
						{
							ptr2[((ulong)num4 + (ulong)((long)l)) * (ulong)((long)sizeof(Matrix)) / (ulong)sizeof(Matrix)] = animationTask2.Renderer.NodeMatrices[l];
						}
					});
				}
				this._nodeBuffer.EndTransfer();
			}
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x0015E10C File Offset: 0x0015C30C
		private void ProcessAnimationTask(int i)
		{
			bool flag = !this._animationTasks[i].SkipUpdate;
			if (flag)
			{
				this._animationTasks[i].Renderer.UpdatePose();
			}
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x0015E14C File Offset: 0x0015C34C
		private void ProcessAnimationTasksOnSingleCore()
		{
			bool isEnabled = this._isEnabled;
			if (isEnabled)
			{
				for (int i = this._processedAnimationTaskCount; i < this._animationTaskCount; i++)
				{
					this.ProcessAnimationTask(i);
				}
			}
			this.SendDataToGPU(false);
			this.HasProcessed = true;
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x0015E19C File Offset: 0x0015C39C
		private unsafe void ProcessAnimationTasksOnMultiCore()
		{
			AnimationSystem.TransferMethod transferMethodId = this._transferMethodId;
			AnimationSystem.TransferMethod transferMethod = transferMethodId;
			if (transferMethod > AnimationSystem.TransferMethod.ParallelSeparate)
			{
				if (transferMethod == AnimationSystem.TransferMethod.ParallelInterleaved)
				{
					uint num = this._nodeTransferSize - this._processedNodeTransferSize;
					bool flag = num > 0U;
					if (flag)
					{
						bool flag2 = this._nodeBuffer.GrowStorageIfNecessary(this._nodeTransferSize);
						num = (flag2 ? this._nodeTransferSize : num);
						uint transferStartOffset = flag2 ? 0U : this._processedNodeTransferSize;
						int num2 = flag2 ? 0 : this._processedAnimationTaskCount;
						IntPtr dataPtr = this._nodeBuffer.BeginTransfer(num, transferStartOffset);
						bool isEnabled = this._isEnabled;
						if (isEnabled)
						{
							Parallel.For(num2, this._animationTaskCount, delegate(int i)
							{
								bool flag3 = i >= this._processedAnimationTaskCount;
								if (flag3)
								{
									this.ProcessAnimationTask(i);
								}
								Matrix* ptr = (Matrix*)dataPtr.ToPointer();
								AnimationSystem.AnimationTask animationTask = this._animationTasks[i];
								uint num3 = (animationTask.Renderer.NodeBufferOffset - this._processedNodeTransferSize) / 64U;
								for (int j = 0; j < (int)animationTask.Renderer.NodeCount; j++)
								{
									ptr[((ulong)num3 + (ulong)((long)j)) * (ulong)((long)sizeof(Matrix)) / (ulong)sizeof(Matrix)] = animationTask.Renderer.NodeMatrices[j];
								}
							});
						}
						else
						{
							Parallel.For(num2, this._animationTaskCount, delegate(int i)
							{
								Matrix* ptr = (Matrix*)dataPtr.ToPointer();
								AnimationSystem.AnimationTask animationTask = this._animationTasks[i];
								uint num3 = (animationTask.Renderer.NodeBufferOffset - this._processedNodeTransferSize) / 64U;
								for (int j = 0; j < (int)animationTask.Renderer.NodeCount; j++)
								{
									ptr[((ulong)num3 + (ulong)((long)j)) * (ulong)((long)sizeof(Matrix)) / (ulong)sizeof(Matrix)] = animationTask.Renderer.NodeMatrices[j];
								}
							});
						}
						this._nodeBuffer.EndTransfer();
					}
				}
			}
			else
			{
				bool isEnabled2 = this._isEnabled;
				if (isEnabled2)
				{
					Parallel.For(this._processedAnimationTaskCount, this._animationTaskCount, delegate(int i)
					{
						this.ProcessAnimationTask(i);
					});
				}
				this.SendDataToGPU(this._transferMethodId > AnimationSystem.TransferMethod.Sequential);
			}
			this.HasProcessed = true;
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x0015E2E0 File Offset: 0x0015C4E0
		public void ProcessAnimationTasks()
		{
			bool useParallelExecution = this._useParallelExecution;
			if (useParallelExecution)
			{
				this.ProcessAnimationTasksOnMultiCore();
			}
			else
			{
				this.ProcessAnimationTasksOnSingleCore();
			}
			this._processedAnimationTaskCount = this._animationTaskCount;
			this._processedNodeTransferSize = this._nodeTransferSize;
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x0015E324 File Offset: 0x0015C524
		public void ProcessHitBlockAnimation(float hitTimer, ref Matrix baseMatrix, out Matrix animatedMatrix)
		{
			Matrix matrix = Matrix.CreateScale(0.98f);
			bool flag = hitTimer > 0.3f;
			if (flag)
			{
				animatedMatrix = Matrix.CreateScale(0f);
			}
			else
			{
				bool flag2 = hitTimer > 0f;
				if (flag2)
				{
					float num = hitTimer / 0.3f;
					float num2 = num * 9.424778f;
					float num3 = 0.04f * (1f - (float)Math.Pow((double)(num - 1f), 4.0));
					float pitch = num3 * (float)Math.Sin((double)num2);
					float roll = num3 * (float)Math.Cos((double)num2);
					Matrix.CreateFromYawPitchRoll(0f, pitch, roll, out animatedMatrix);
					Matrix matrix2 = Matrix.CreateTranslation(0f, 16f, 0f);
					Matrix matrix3 = Matrix.Invert(matrix2);
					Matrix.Multiply(ref matrix3, ref animatedMatrix, out animatedMatrix);
					Matrix.Multiply(ref animatedMatrix, ref matrix2, out animatedMatrix);
					Matrix.Multiply(ref matrix, ref animatedMatrix, out animatedMatrix);
				}
				else
				{
					animatedMatrix = Matrix.Identity;
				}
			}
			Matrix.Multiply(ref animatedMatrix, ref baseMatrix, out animatedMatrix);
		}

		// Token: 0x040029E8 RID: 10728
		private const int AnimationTasksDefaultSize = 500;

		// Token: 0x040029E9 RID: 10729
		private const int AnimationTasksGrowth = 250;

		// Token: 0x040029EA RID: 10730
		private int _incomingAnimationTaskCount;

		// Token: 0x040029EB RID: 10731
		private int _animationTaskCount;

		// Token: 0x040029EC RID: 10732
		private AnimationSystem.AnimationTask[] _animationTasks = new AnimationSystem.AnimationTask[500];

		// Token: 0x040029ED RID: 10733
		private AnimationSystem.TransferMethod _transferMethodId;

		// Token: 0x040029EE RID: 10734
		private int _processedAnimationTaskCount;

		// Token: 0x040029EF RID: 10735
		private uint _processedNodeTransferSize;

		// Token: 0x040029F0 RID: 10736
		private uint _nodeTransferSize;

		// Token: 0x040029F1 RID: 10737
		private bool _useDoubleBuffering = true;

		// Token: 0x040029F2 RID: 10738
		private GPUBuffer _nodeBuffer;

		// Token: 0x040029F3 RID: 10739
		private const uint NodeBufferDefaultSize = 16384000U;

		// Token: 0x040029F4 RID: 10740
		private const uint NodeBufferGrowth = 8192000U;

		// Token: 0x040029F5 RID: 10741
		private uint _nodeBufferSize = 16384000U;

		// Token: 0x040029F6 RID: 10742
		private readonly int UniformBufferOffsetAlignment;

		// Token: 0x040029F7 RID: 10743
		private bool _isEnabled = true;

		// Token: 0x040029F8 RID: 10744
		private bool _useParallelExecution = true;

		// Token: 0x040029F9 RID: 10745
		private readonly GLFunctions _gl;

		// Token: 0x02000E8D RID: 3725
		public enum TransferMethod
		{
			// Token: 0x04004706 RID: 18182
			Sequential,
			// Token: 0x04004707 RID: 18183
			ParallelSeparate,
			// Token: 0x04004708 RID: 18184
			ParallelInterleaved,
			// Token: 0x04004709 RID: 18185
			MAX
		}

		// Token: 0x02000E8E RID: 3726
		private struct AnimationTask
		{
			// Token: 0x0400470A RID: 18186
			public AnimatedRenderer Renderer;

			// Token: 0x0400470B RID: 18187
			public bool SkipUpdate;
		}
	}
}
