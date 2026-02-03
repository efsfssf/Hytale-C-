using System;
using System.Diagnostics;
using HytaleClient.Core;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Map;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Map
{
	// Token: 0x02000906 RID: 2310
	internal class Chunk : Disposable
	{
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06004574 RID: 17780 RVA: 0x000F3FA5 File Offset: 0x000F21A5
		// (set) Token: 0x06004575 RID: 17781 RVA: 0x000F3FAD File Offset: 0x000F21AD
		public RenderedChunk Rendered { get; private set; }

		// Token: 0x06004576 RID: 17782 RVA: 0x000F3FB6 File Offset: 0x000F21B6
		public Chunk(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.Data = new ChunkData();
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x000F3FEC File Offset: 0x000F21EC
		protected override void DoDispose()
		{
			RenderedChunk rendered = this.Rendered;
			if (rendered != null)
			{
				rendered.Dispose();
			}
			bool flag = this.Data.SelfLightAmounts != null || this.Data.BorderedLightAmounts != null;
			if (flag)
			{
				throw new Exception("Chunk was not disposed properly before its column.");
			}
			bool flag2 = this.Data.CurrentInteractionStates != null;
			if (flag2)
			{
				throw new Exception("Chunk interaction was not disposed properly.");
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x000F4058 File Offset: 0x000F2258
		public void Initialize(GraphicsDevice graphics)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(Chunk).FullName);
			}
			this.Rendered = new RenderedChunk(graphics);
		}

		// Token: 0x040022D2 RID: 8914
		public readonly object DisposeLock = new object();

		// Token: 0x040022D3 RID: 8915
		public readonly int X;

		// Token: 0x040022D4 RID: 8916
		public readonly int Y;

		// Token: 0x040022D5 RID: 8917
		public readonly int Z;

		// Token: 0x040022D6 RID: 8918
		public bool IsUnderground;

		// Token: 0x040022D7 RID: 8919
		public int SolidPlaneMinY;

		// Token: 0x040022D8 RID: 8920
		public readonly ChunkData Data;
	}
}
