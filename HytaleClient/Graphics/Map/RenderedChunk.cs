using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics.Particles;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics.Map
{
	// Token: 0x02000A92 RID: 2706
	internal class RenderedChunk : Disposable
	{
		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06005537 RID: 21815 RVA: 0x0018D5E4 File Offset: 0x0018B7E4
		public int AlphaTestedIndicesCount
		{
			get
			{
				return this.AlphaTestedAnimatedLowLODIndicesCount + this.AlphaTestedLowLODIndicesCount + this.AlphaTestedHighLODIndicesCount + this.AlphaTestedAnimatedHighLODIndicesCount;
			}
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x0018D604 File Offset: 0x0018B804
		public RenderedChunk(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			GLFunctions gl = graphics.GL;
			this.OpaqueVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this.OpaqueVertexArray);
			this.OpaqueVerticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.OpaqueVertexArray, GL.ARRAY_BUFFER, this.OpaqueVerticesBuffer);
			this.OpaqueIndicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.OpaqueVertexArray, GL.ELEMENT_ARRAY_BUFFER, this.OpaqueIndicesBuffer);
			this.SetupVertexAttributes();
			this.AlphaBlendedVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this.AlphaBlendedVertexArray);
			this.AlphaBlendedVerticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.AlphaBlendedVertexArray, GL.ARRAY_BUFFER, this.AlphaBlendedVerticesBuffer);
			this.AlphaBlendedIndicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.AlphaBlendedVertexArray, GL.ELEMENT_ARRAY_BUFFER, this.AlphaBlendedIndicesBuffer);
			this.SetupVertexAttributes();
			this.AlphaTestedVertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this.AlphaTestedVertexArray);
			this.AlphaTestedVerticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.AlphaTestedVertexArray, GL.ARRAY_BUFFER, this.AlphaTestedVerticesBuffer);
			this.AlphaTestedIndicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this.AlphaTestedVertexArray, GL.ELEMENT_ARRAY_BUFFER, this.AlphaTestedIndicesBuffer);
			this.SetupVertexAttributes();
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x0018D768 File Offset: 0x0018B968
		private void SetupVertexAttributes()
		{
			GLFunctions gl = this._graphics.GL;
			MapChunkNearProgram mapChunkNearOpaqueProgram = this._graphics.GPUProgramStore.MapChunkNearOpaqueProgram;
			IntPtr pointer = IntPtr.Zero;
			gl.EnableVertexAttribArray(mapChunkNearOpaqueProgram.AttribPositionAndDoubleSidedAndBlockId.Index);
			gl.VertexAttribIPointer(mapChunkNearOpaqueProgram.AttribPositionAndDoubleSidedAndBlockId.Index, 4, GL.SHORT, ChunkVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(mapChunkNearOpaqueProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(mapChunkNearOpaqueProgram.AttribTexCoords.Index, 4, GL.UNSIGNED_SHORT, true, ChunkVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(mapChunkNearOpaqueProgram.AttribDataPacked.Index);
			gl.VertexAttribIPointer(mapChunkNearOpaqueProgram.AttribDataPacked.Index, 4, GL.UNSIGNED_INT, ChunkVertex.Size, pointer);
			pointer += 16;
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x0018D854 File Offset: 0x0018BA54
		public void Discard()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = this.UpdateTask != null;
			if (flag)
			{
				bool flag2 = this.UpdateTask.AnimatedBlocks != null;
				if (flag2)
				{
					for (int i = 0; i < this.UpdateTask.AnimatedBlocks.Length; i++)
					{
						this.UpdateTask.AnimatedBlocks[i].Renderer.Dispose();
						this.UpdateTask.AnimatedBlocks[i].MapParticleIndices = null;
					}
				}
				bool flag3 = this.UpdateTask.MapParticles != null;
				if (flag3)
				{
					for (int j = 0; j < this.UpdateTask.MapParticles.Length; j++)
					{
						bool flag4 = this.UpdateTask.MapParticles[j].ParticleSystemProxy != null;
						if (flag4)
						{
							this.UpdateTask.MapParticles[j].ParticleSystemProxy.Expire(true);
							this.UpdateTask.MapParticles[j].ParticleSystemProxy = null;
						}
					}
				}
			}
			this.UpdateTask = null;
			this.RebuildState = RenderedChunk.ChunkRebuildState.UpdateReady;
			this.BufferUpdateCount = 0;
			this.GeometryNeedsUpdate = true;
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x0018D998 File Offset: 0x0018BB98
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteBuffer(this.OpaqueVerticesBuffer);
			gl.DeleteBuffer(this.OpaqueIndicesBuffer);
			gl.DeleteVertexArray(this.OpaqueVertexArray);
			gl.DeleteBuffer(this.AlphaBlendedVerticesBuffer);
			gl.DeleteBuffer(this.AlphaBlendedIndicesBuffer);
			gl.DeleteVertexArray(this.AlphaBlendedVertexArray);
			gl.DeleteBuffer(this.AlphaTestedVerticesBuffer);
			gl.DeleteBuffer(this.AlphaTestedIndicesBuffer);
			gl.DeleteVertexArray(this.AlphaTestedVertexArray);
			bool flag = this.AnimatedBlocks != null;
			if (flag)
			{
				for (int i = 0; i < this.AnimatedBlocks.Length; i++)
				{
					this.AnimatedBlocks[i].Renderer.Dispose();
				}
				this.AnimatedBlocks = null;
			}
			bool flag2 = this.MapParticles != null;
			if (flag2)
			{
				for (int j = 0; j < this.MapParticles.Length; j++)
				{
					bool flag3 = this.MapParticles[j].ParticleSystemProxy != null;
					if (flag3)
					{
						this.MapParticles[j].ParticleSystemProxy.Expire(true);
						this.MapParticles[j].ParticleSystemProxy = null;
					}
				}
			}
			bool flag4 = this.SoundObjects != null;
			if (flag4)
			{
				throw new Exception("SoundObjects not disposed properly before its column.");
			}
			bool flag5 = this.UpdateTask != null;
			if (flag5)
			{
				throw new Exception("Chunk was not disposed properly before its column.");
			}
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x0018DB1C File Offset: 0x0018BD1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetAlphaTestedData(bool useAnimatedBlocks, float levelOfDetailFactor, out int dataCount, out IntPtr dataOffset)
		{
			if (useAnimatedBlocks)
			{
				float num = (float)this.AlphaTestedHighLODIndicesCount * levelOfDetailFactor;
				int num2 = (int)((float)this.AlphaTestedLowLODIndicesCount + num);
				dataCount = num2;
				dataOffset = (IntPtr)(this.AlphaTestedAnimatedLowLODIndicesCount * 4);
			}
			else
			{
				float num3 = (float)(this.AlphaTestedHighLODIndicesCount + this.AlphaTestedAnimatedHighLODIndicesCount) * levelOfDetailFactor;
				int num4 = (int)((float)(this.AlphaTestedAnimatedLowLODIndicesCount + this.AlphaTestedLowLODIndicesCount) + num3);
				dataCount = num4;
				dataOffset = IntPtr.Zero;
			}
		}

		// Token: 0x040031EE RID: 12782
		public volatile bool GeometryNeedsUpdate = true;

		// Token: 0x040031EF RID: 12783
		public volatile RenderedChunk.ChunkRebuildState RebuildState;

		// Token: 0x040031F0 RID: 12784
		public RenderedChunk.ChunkUpdateTask UpdateTask;

		// Token: 0x040031F1 RID: 12785
		public int OpaqueIndicesCount;

		// Token: 0x040031F2 RID: 12786
		public readonly GLVertexArray OpaqueVertexArray;

		// Token: 0x040031F3 RID: 12787
		public readonly GLBuffer OpaqueVerticesBuffer;

		// Token: 0x040031F4 RID: 12788
		public readonly GLBuffer OpaqueIndicesBuffer;

		// Token: 0x040031F5 RID: 12789
		public int AlphaBlendedIndicesCount;

		// Token: 0x040031F6 RID: 12790
		public readonly GLVertexArray AlphaBlendedVertexArray;

		// Token: 0x040031F7 RID: 12791
		public readonly GLBuffer AlphaBlendedVerticesBuffer;

		// Token: 0x040031F8 RID: 12792
		public readonly GLBuffer AlphaBlendedIndicesBuffer;

		// Token: 0x040031F9 RID: 12793
		public int AlphaTestedAnimatedLowLODIndicesCount;

		// Token: 0x040031FA RID: 12794
		public int AlphaTestedLowLODIndicesCount;

		// Token: 0x040031FB RID: 12795
		public int AlphaTestedHighLODIndicesCount;

		// Token: 0x040031FC RID: 12796
		public int AlphaTestedAnimatedHighLODIndicesCount;

		// Token: 0x040031FD RID: 12797
		public readonly GLVertexArray AlphaTestedVertexArray;

		// Token: 0x040031FE RID: 12798
		public readonly GLBuffer AlphaTestedVerticesBuffer;

		// Token: 0x040031FF RID: 12799
		public readonly GLBuffer AlphaTestedIndicesBuffer;

		// Token: 0x04003200 RID: 12800
		public RenderedChunk.AnimatedBlock[] AnimatedBlocks;

		// Token: 0x04003201 RID: 12801
		public RenderedChunk.MapParticle[] MapParticles;

		// Token: 0x04003202 RID: 12802
		public RenderedChunk.MapSoundObject[] SoundObjects;

		// Token: 0x04003203 RID: 12803
		public int BufferUpdateCount;

		// Token: 0x04003204 RID: 12804
		private readonly GraphicsDevice _graphics;

		// Token: 0x02000EE1 RID: 3809
		public enum ChunkRebuildState
		{
			// Token: 0x040048F6 RID: 18678
			Waiting,
			// Token: 0x040048F7 RID: 18679
			ReadyForRebuild,
			// Token: 0x040048F8 RID: 18680
			Rebuilding,
			// Token: 0x040048F9 RID: 18681
			UpdateReady
		}

		// Token: 0x02000EE2 RID: 3810
		public class ChunkUpdateTask
		{
			// Token: 0x040048FA RID: 18682
			public ChunkGeometryData OpaqueData;

			// Token: 0x040048FB RID: 18683
			public ChunkGeometryData AlphaBlendedData;

			// Token: 0x040048FC RID: 18684
			public ChunkGeometryData AlphaTestedData;

			// Token: 0x040048FD RID: 18685
			public int AlphaTestedAnimatedLowLODIndicesCount;

			// Token: 0x040048FE RID: 18686
			public int AlphaTestedLowLODIndicesCount;

			// Token: 0x040048FF RID: 18687
			public int AlphaTestedHighLODIndicesCount;

			// Token: 0x04004900 RID: 18688
			public int AlphaTestedAnimatedHighLODIndicesCount;

			// Token: 0x04004901 RID: 18689
			public RenderedChunk.AnimatedBlock[] AnimatedBlocks;

			// Token: 0x04004902 RID: 18690
			public RenderedChunk.MapParticle[] MapParticles;

			// Token: 0x04004903 RID: 18691
			public RenderedChunk.MapSoundObject[] SoundObjects;

			// Token: 0x04004904 RID: 18692
			public int SolidPlaneMinY;

			// Token: 0x04004905 RID: 18693
			public bool IsUnderground;
		}

		// Token: 0x02000EE3 RID: 3811
		public struct MapSoundObject
		{
			// Token: 0x04004906 RID: 18694
			public int BlockIndex;

			// Token: 0x04004907 RID: 18695
			public Vector3 Position;

			// Token: 0x04004908 RID: 18696
			public uint SoundEventIndex;

			// Token: 0x04004909 RID: 18697
			public AudioDevice.SoundEventReference SoundEventReference;
		}

		// Token: 0x02000EE4 RID: 3812
		public struct MapParticle
		{
			// Token: 0x0400490A RID: 18698
			public int BlockIndex;

			// Token: 0x0400490B RID: 18699
			public string ParticleSystemId;

			// Token: 0x0400490C RID: 18700
			public Vector3 Position;

			// Token: 0x0400490D RID: 18701
			public Quaternion Rotation;

			// Token: 0x0400490E RID: 18702
			public Vector3 PositionOffset;

			// Token: 0x0400490F RID: 18703
			public Quaternion RotationOffset;

			// Token: 0x04004910 RID: 18704
			public UInt32Color Color;

			// Token: 0x04004911 RID: 18705
			public float BlockScale;

			// Token: 0x04004912 RID: 18706
			public float Scale;

			// Token: 0x04004913 RID: 18707
			public int TargetNodeIndex;

			// Token: 0x04004914 RID: 18708
			public ParticleSystemProxy ParticleSystemProxy;
		}

		// Token: 0x02000EE5 RID: 3813
		public struct AnimatedBlock
		{
			// Token: 0x04004915 RID: 18709
			public bool IsBeingHit;

			// Token: 0x04004916 RID: 18710
			public int Index;

			// Token: 0x04004917 RID: 18711
			public Vector3 Position;

			// Token: 0x04004918 RID: 18712
			public BoundingBox BoundingBox;

			// Token: 0x04004919 RID: 18713
			public Matrix Matrix;

			// Token: 0x0400491A RID: 18714
			public AnimatedBlockRenderer Renderer;

			// Token: 0x0400491B RID: 18715
			public BlockyAnimation Animation;

			// Token: 0x0400491C RID: 18716
			public float AnimationTimeOffset;

			// Token: 0x0400491D RID: 18717
			public int[] MapParticleIndices;
		}
	}
}
