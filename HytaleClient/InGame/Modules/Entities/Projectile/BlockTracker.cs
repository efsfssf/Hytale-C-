using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000950 RID: 2384
	internal class BlockTracker
	{
		// Token: 0x06004A96 RID: 19094 RVA: 0x00130848 File Offset: 0x0012EA48
		public BlockTracker()
		{
			for (int i = 0; i < this._positions.Length; i++)
			{
				this._positions[i] = default(IntVector3);
			}
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x00130894 File Offset: 0x0012EA94
		public IntVector3 GetPosition(int index)
		{
			return this._positions[index];
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x001308B2 File Offset: 0x0012EAB2
		public virtual void Reset()
		{
			this.Count = 0;
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x001308BC File Offset: 0x0012EABC
		public bool Track(int x, int y, int z)
		{
			bool flag = this.IsTracked(x, y, z);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				this.TrackNew(x, y, z);
				result = false;
			}
			return result;
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x001308EC File Offset: 0x0012EAEC
		public void TrackNew(int x, int y, int z)
		{
			bool flag = this.Count >= this._positions.Length;
			if (flag)
			{
				this.Alloc();
			}
			IntVector3[] positions = this._positions;
			int count = this.Count;
			this.Count = count + 1;
			positions[count] = new IntVector3(x, y, z);
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x00130940 File Offset: 0x0012EB40
		public bool IsTracked(int x, int y, int z)
		{
			return this.GetIndex(x, y, z) >= 0;
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x00130964 File Offset: 0x0012EB64
		public void Untrack(int x, int y, int z)
		{
			int index = this.GetIndex(x, y, z);
			bool flag = index >= 0;
			if (flag)
			{
				this.Untrack(index);
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x00130990 File Offset: 0x0012EB90
		public virtual void Untrack(int index)
		{
			bool flag = this.Count <= 0;
			if (flag)
			{
				throw new Exception("Calling untrack on empty tracker");
			}
			this.Count--;
			bool flag2 = this.Count == 0;
			if (!flag2)
			{
				IntVector3 intVector = this._positions[index];
				Array.Copy(this._positions, index + 1, this._positions, index, this.Count - index);
				this._positions[this.Count] = intVector;
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00130A14 File Offset: 0x0012EC14
		public int GetIndex(int x, int y, int z)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				ref IntVector3 ptr = ref this._positions[i];
				bool flag = ptr.X == x && ptr.Y == y && ptr.Z == z;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x00130A78 File Offset: 0x0012EC78
		protected virtual void Alloc()
		{
			Array.Resize<IntVector3>(ref this._positions, this._positions.Length + 4);
		}

		// Token: 0x04002652 RID: 9810
		public const int NotFound = -1;

		// Token: 0x04002653 RID: 9811
		protected const int AllocSize = 4;

		// Token: 0x04002654 RID: 9812
		protected IntVector3[] _positions = new IntVector3[4];

		// Token: 0x04002655 RID: 9813
		public int Count;
	}
}
