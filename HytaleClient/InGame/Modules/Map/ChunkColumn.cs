using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using HytaleClient.Core;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Map
{
	// Token: 0x02000907 RID: 2311
	internal class ChunkColumn : Disposable
	{
		// Token: 0x06004579 RID: 17785 RVA: 0x000F409D File Offset: 0x000F229D
		public ChunkColumn(int x, int z)
		{
			this.X = x;
			this.Z = z;
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x000F40CC File Offset: 0x000F22CC
		protected override void DoDispose()
		{
			foreach (Chunk chunk in this._chunks.Values)
			{
				bool flag = !chunk.Disposed;
				if (flag)
				{
					throw new Exception("Chunk was not disposed properly before its column.");
				}
			}
			this._chunks.Clear();
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x000F4140 File Offset: 0x000F2340
		public Chunk CreateChunk(int y)
		{
			Chunk chunk = new Chunk(this.X, y, this.Z);
			this._chunks.TryAdd(y, chunk);
			return chunk;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x000F4174 File Offset: 0x000F2374
		public Chunk GetChunk(int y)
		{
			Chunk result;
			this._chunks.TryGetValue(y, ref result);
			return result;
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x000F4198 File Offset: 0x000F2398
		public void DiscardRenderedChunks()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			foreach (Chunk chunk in this._chunks.Values)
			{
				object disposeLock = chunk.DisposeLock;
				lock (disposeLock)
				{
					bool flag2 = chunk.Rendered != null;
					if (flag2)
					{
						chunk.Rendered.Discard();
					}
					chunk.Data.SelfLightNeedsUpdate = true;
				}
			}
		}

		// Token: 0x040022DA RID: 8922
		public readonly object DisposeLock = new object();

		// Token: 0x040022DB RID: 8923
		public readonly int X;

		// Token: 0x040022DC RID: 8924
		public readonly int Z;

		// Token: 0x040022DD RID: 8925
		public uint[] Tints;

		// Token: 0x040022DE RID: 8926
		public ushort[] Heights;

		// Token: 0x040022DF RID: 8927
		public ushort[][] Environments;

		// Token: 0x040022E0 RID: 8928
		private readonly ConcurrentDictionary<int, Chunk> _chunks = new ConcurrentDictionary<int, Chunk>();
	}
}
