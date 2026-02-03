using System;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x02000957 RID: 2391
	internal class CollisionTracker : BlockTracker
	{
		// Token: 0x06004AC6 RID: 19142 RVA: 0x001317E8 File Offset: 0x0012F9E8
		public CollisionTracker()
		{
			for (int i = 0; i < 4; i++)
			{
				this._blockData[i] = new BlockData();
				this._contactData[i] = new BlockContactData();
			}
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x00131844 File Offset: 0x0012FA44
		public BlockData GetBlockData(int index)
		{
			return this._blockData[index];
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x00131860 File Offset: 0x0012FA60
		public BlockContactData GetContactData(int index)
		{
			return this._contactData[index];
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x0013187C File Offset: 0x0012FA7C
		public override void Reset()
		{
			base.Reset();
			for (int i = 0; i < this.Count; i++)
			{
				this._blockData[i].Clear();
				this._contactData[i].Clear();
			}
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x001318C4 File Offset: 0x0012FAC4
		public bool Track(int x, int y, int z, BlockContactData contactData, BlockData blockData)
		{
			bool flag = base.IsTracked(x, y, z);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				this.TrackNew(x, y, z, contactData, blockData);
				result = false;
			}
			return result;
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x001318F8 File Offset: 0x0012FAF8
		public BlockContactData TrackNew(int x, int y, int z, BlockContactData contactData, BlockData blockData)
		{
			base.TrackNew(x, y, z);
			this._blockData[this.Count - 1].Assign(blockData);
			BlockContactData blockContactData = this._contactData[this.Count - 1];
			blockContactData.Assign(contactData);
			return blockContactData;
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x00131948 File Offset: 0x0012FB48
		public override void Untrack(int index)
		{
			base.Untrack(index);
			bool flag = this.Count == 0;
			if (flag)
			{
				this._blockData[0].Clear();
				this._contactData[0].Clear();
			}
			else
			{
				int length = this.Count - index;
				BlockData blockData = this._blockData[index];
				blockData.Clear();
				Array.Copy(this._blockData, index + 1, this._blockData, index, length);
				this._blockData[this.Count] = null;
				BlockContactData blockContactData = this._contactData[index];
				blockContactData.Clear();
				Array.Copy(this._contactData, index + 1, this._contactData, index, length);
				this._contactData[this.Count] = null;
			}
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x00131A00 File Offset: 0x0012FC00
		public BlockContactData GetContactData(int x, int y, int z)
		{
			int index = base.GetIndex(x, y, z);
			bool flag = index == -1;
			BlockContactData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this._contactData[index];
			}
			return result;
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x00131A30 File Offset: 0x0012FC30
		protected override void Alloc()
		{
			base.Alloc();
			int num = this._blockData.Length + 4;
			Array.Resize<BlockData>(ref this._blockData, num);
			Array.Resize<BlockContactData>(ref this._contactData, num);
			for (int i = this.Count; i < num; i++)
			{
				this._blockData[i] = new BlockData();
				this._contactData[i] = new BlockContactData();
			}
		}

		// Token: 0x04002677 RID: 9847
		protected BlockData[] _blockData = new BlockData[4];

		// Token: 0x04002678 RID: 9848
		protected BlockContactData[] _contactData = new BlockContactData[4];
	}
}
