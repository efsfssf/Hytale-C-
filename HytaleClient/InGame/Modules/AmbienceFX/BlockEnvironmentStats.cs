using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.AmbienceFX
{
	// Token: 0x02000999 RID: 2457
	public class BlockEnvironmentStats
	{
		// Token: 0x06004E72 RID: 20082 RVA: 0x0015C5C0 File Offset: 0x0015A7C0
		public BlockEnvironmentStats(int totalBlocks)
		{
			this.BlockSoundSetIndices = new int[20];
			this.Stats = new BlockEnvironmentStats.BlockStats[20];
			this._totalBlocks = new int[20];
			this._blocks = new Vector3[20][];
			for (int i = 0; i < 20; i++)
			{
				this._blocks[i] = new Vector3[200];
			}
			this._inverseTotalAnalyzedBlocks = 1f / (float)totalBlocks * 100f;
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x0015C64C File Offset: 0x0015A84C
		public void Initialize(int blockSoundSetIndex, int x, int y, int z)
		{
			bool flag = this.TotalStats >= this.BlockSoundSetIndices.Length;
			if (flag)
			{
				Array.Resize<int>(ref this.BlockSoundSetIndices, this.BlockSoundSetIndices.Length + 10);
				Array.Resize<BlockEnvironmentStats.BlockStats>(ref this.Stats, this.Stats.Length + 10);
				Array.Resize<int>(ref this._totalBlocks, this._totalBlocks.Length + 10);
				Array.Resize<Vector3[]>(ref this._blocks, this._blocks.Length + 10);
				for (int i = this.TotalStats; i < this._blocks.Length; i++)
				{
					this._blocks[i] = new Vector3[200];
				}
			}
			this.BlockSoundSetIndices[this.TotalStats] = blockSoundSetIndex;
			this._blocks[this.TotalStats][0] = new Vector3((float)x, (float)y, (float)z);
			ref BlockEnvironmentStats.BlockStats ptr = ref this.Stats[this.TotalStats];
			ptr.Percent = this._inverseTotalAnalyzedBlocks;
			ptr.LowestAltitude = y;
			ptr.HighestAltitude = y;
			this._totalBlocks[this.TotalStats] = 1;
			this.TotalStats++;
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x0015C778 File Offset: 0x0015A978
		public void Add(int index, int x, int y, int z)
		{
			ref Vector3[] ptr = ref this._blocks[index];
			ref int ptr2 = ref this._totalBlocks[index];
			ref BlockEnvironmentStats.BlockStats ptr3 = ref this.Stats[index];
			bool flag = ptr2 >= ptr.Length;
			if (flag)
			{
				Array.Resize<Vector3>(ref ptr, ptr.Length + 50);
			}
			ptr[ptr2] = new Vector3((float)x, (float)y, (float)z);
			ptr2++;
			ptr3.Percent = (float)ptr2 * this._inverseTotalAnalyzedBlocks;
			bool flag2 = y < ptr3.LowestAltitude;
			if (flag2)
			{
				ptr3.LowestAltitude = y;
			}
			bool flag3 = y > ptr3.HighestAltitude;
			if (flag3)
			{
				ptr3.HighestAltitude = y;
			}
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x0015C824 File Offset: 0x0015AA24
		public Vector3 GetClosestBlock(int index, Vector3 position)
		{
			ref Vector3[] ptr = ref this._blocks[index];
			Vector3 vector = ptr[0];
			float num = Vector3.DistanceSquared(position, vector);
			ref int ptr2 = ref this._totalBlocks[index];
			for (int i = 1; i < ptr2; i++)
			{
				ref Vector3 ptr3 = ref ptr[i];
				float num2 = Vector3.DistanceSquared(position, ptr3);
				bool flag = num2 < num;
				if (flag)
				{
					num = num2;
					vector = ptr3;
				}
			}
			return vector;
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x0015C8B0 File Offset: 0x0015AAB0
		public string GetDebugData(int index, string blockSoundSetId)
		{
			ref BlockEnvironmentStats.BlockStats ptr = ref this.Stats[index];
			int num = this._totalBlocks[index];
			double num2 = Math.Round((double)(ptr.Percent * 10f)) / 10.0;
			return string.Format("[ {0} = {1}% - Count={2} - Ymin={3} / Ymax={4} ]", new object[]
			{
				blockSoundSetId,
				num2,
				num,
				ptr.LowestAltitude,
				ptr.HighestAltitude
			});
		}

		// Token: 0x040029C2 RID: 10690
		private const int StatsArrayDefaultSize = 20;

		// Token: 0x040029C3 RID: 10691
		private const int StatsArrayGrowSize = 10;

		// Token: 0x040029C4 RID: 10692
		private const int BlocksArrayDefaultSize = 200;

		// Token: 0x040029C5 RID: 10693
		private const int BlocksArrayGrowSize = 50;

		// Token: 0x040029C6 RID: 10694
		private readonly float _inverseTotalAnalyzedBlocks;

		// Token: 0x040029C7 RID: 10695
		public int TotalStats = 0;

		// Token: 0x040029C8 RID: 10696
		public int[] BlockSoundSetIndices;

		// Token: 0x040029C9 RID: 10697
		private int[] _totalBlocks;

		// Token: 0x040029CA RID: 10698
		private Vector3[][] _blocks;

		// Token: 0x040029CB RID: 10699
		public BlockEnvironmentStats.BlockStats[] Stats;

		// Token: 0x02000E88 RID: 3720
		public struct BlockStats
		{
			// Token: 0x040046EF RID: 18159
			public float Percent;

			// Token: 0x040046F0 RID: 18160
			public int LowestAltitude;

			// Token: 0x040046F1 RID: 18161
			public int HighestAltitude;
		}
	}
}
