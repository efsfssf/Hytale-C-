using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A49 RID: 2633
	internal class SceneView
	{
		// Token: 0x060053CD RID: 21453 RVA: 0x0017F878 File Offset: 0x0017DA78
		public void ResetCounters()
		{
			this._entitiesCount = 0;
			this._chunksCount = 0;
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x060053CE RID: 21454 RVA: 0x0017F889 File Offset: 0x0017DA89
		public int EntitiesCount
		{
			get
			{
				return this._entitiesCount;
			}
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x0017F894 File Offset: 0x0017DA94
		public void PrepareForIncomingEntities(int max)
		{
			ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._entitiesIds, max, 500);
			ArrayUtils.GrowArrayIfNecessary<Vector3>(ref this._entitiesPositions, max, 500);
			ArrayUtils.GrowArrayIfNecessary<float>(ref this._entitiesDistancesToCamera, max, 500);
			ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._sortedEntitiesIds, max, 500);
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x0017F8EC File Offset: 0x0017DAEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterEntity(int entityLocalId, Vector3 entityPosition)
		{
			int entitiesCount = this._entitiesCount;
			this._entitiesIds[entitiesCount] = (ushort)entityLocalId;
			this._entitiesPositions[entitiesCount] = entityPosition;
			this._entitiesDistancesToCamera[entitiesCount] = Vector3.DistanceSquared(entityPosition, this.Position);
			this._entitiesCount++;
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x0017F93C File Offset: 0x0017DB3C
		public void SortEntitiesByDistance()
		{
			Debug.Assert(this._entitiesCount < this._sortedEntitiesIds.Length, "Array is too small. Did you forget a call to PrepareForIncomingEntities()?");
			Array.Copy(this._entitiesIds, this._sortedEntitiesIds, this._entitiesCount);
			Array.Sort<float, ushort>(this._entitiesDistancesToCamera, this._sortedEntitiesIds, 0, this._entitiesCount);
		}

		// Token: 0x060053D2 RID: 21458 RVA: 0x0017F998 File Offset: 0x0017DB98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSortedEntityId(int id)
		{
			return (int)this._sortedEntitiesIds[id];
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x0017F9B2 File Offset: 0x0017DBB2
		public int ChunksCount
		{
			get
			{
				return this._chunksCount;
			}
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x0017F9BC File Offset: 0x0017DBBC
		public void PrepareForIncomingChunks(int max)
		{
			ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._chunksIds, max, 500);
			ArrayUtils.GrowArrayIfNecessary<Vector3>(ref this._chunksPositions, max, 500);
			ArrayUtils.GrowArrayIfNecessary<float>(ref this._chunksDistancesToCamera, max, 500);
			ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._sortedChunksIds, max, 500);
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x0017FA14 File Offset: 0x0017DC14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RegisterChunk(int chunkLocalId, Vector3 chunkPosition)
		{
			int chunksCount = this._chunksCount;
			this._chunksIds[chunksCount] = (ushort)chunkLocalId;
			this._chunksPositions[chunksCount] = chunkPosition;
			this._chunksDistancesToCamera[chunksCount] = Vector3.DistanceSquared(chunkPosition, this.Position);
			this._chunksCount++;
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x0017FA64 File Offset: 0x0017DC64
		public void SortChunksByDistance()
		{
			Debug.Assert(this._chunksCount < this._sortedChunksIds.Length, "Array is too small. Did you forget a call to PrepareForIncomingChunks()?");
			Array.Copy(this._chunksIds, this._sortedChunksIds, this._chunksCount);
			Array.Sort<float, ushort>(this._chunksDistancesToCamera, this._sortedChunksIds, 0, this._chunksCount);
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x0017FAC0 File Offset: 0x0017DCC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSortedChunkId(int id)
		{
			return (int)this._sortedChunksIds[id];
		}

		// Token: 0x04002EB5 RID: 11957
		public Vector3 Position;

		// Token: 0x04002EB6 RID: 11958
		public Vector3 Direction;

		// Token: 0x04002EB7 RID: 11959
		public BoundingFrustum Frustum;

		// Token: 0x04002EB8 RID: 11960
		public KDop KDopFrustum;

		// Token: 0x04002EB9 RID: 11961
		public bool UseKDopForCulling = false;

		// Token: 0x04002EBA RID: 11962
		public int IncomingEntityDrawTaskCount;

		// Token: 0x04002EBB RID: 11963
		private const int EntitiesDefaultSize = 1000;

		// Token: 0x04002EBC RID: 11964
		private const int EntitiesGrowth = 500;

		// Token: 0x04002EBD RID: 11965
		private int _entitiesCount;

		// Token: 0x04002EBE RID: 11966
		private ushort[] _entitiesIds = new ushort[1000];

		// Token: 0x04002EBF RID: 11967
		private Vector3[] _entitiesPositions = new Vector3[1000];

		// Token: 0x04002EC0 RID: 11968
		private float[] _entitiesDistancesToCamera = new float[1000];

		// Token: 0x04002EC1 RID: 11969
		private ushort[] _sortedEntitiesIds = new ushort[1000];

		// Token: 0x04002EC2 RID: 11970
		public bool[] EntitiesFrustumCullingResults = new bool[1000];

		// Token: 0x04002EC3 RID: 11971
		public int IncomingChunkDrawTaskCount;

		// Token: 0x04002EC4 RID: 11972
		private const int ChunksDefaultSize = 1000;

		// Token: 0x04002EC5 RID: 11973
		private const int ChunksGrowth = 500;

		// Token: 0x04002EC6 RID: 11974
		private int _chunksCount;

		// Token: 0x04002EC7 RID: 11975
		private ushort[] _chunksIds = new ushort[1000];

		// Token: 0x04002EC8 RID: 11976
		private Vector3[] _chunksPositions = new Vector3[1000];

		// Token: 0x04002EC9 RID: 11977
		private float[] _chunksDistancesToCamera = new float[1000];

		// Token: 0x04002ECA RID: 11978
		private ushort[] _sortedChunksIds = new ushort[1000];

		// Token: 0x04002ECB RID: 11979
		public bool[] ChunksFrustumCullingResults = new bool[1000];
	}
}
