using System;
using System.Text;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200076A RID: 1898
	public struct SocketId
	{
		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x00049F28 File Offset: 0x00048128
		// (set) Token: 0x06003174 RID: 12660 RVA: 0x00049F70 File Offset: 0x00048170
		public string SocketName
		{
			get
			{
				bool cacheValid = this.m_CacheValid;
				string result;
				if (cacheValid)
				{
					result = this.m_CachedSocketName;
				}
				else
				{
					bool flag = this.m_AllBytes == null;
					if (flag)
					{
						result = null;
					}
					else
					{
						this.RebuildStringFromBuffer();
						result = this.m_CachedSocketName;
					}
				}
				return result;
			}
			set
			{
				this.m_CachedSocketName = value;
				bool flag = value == null;
				if (flag)
				{
					this.m_CacheValid = true;
				}
				else
				{
					this.EnsureStorage();
					int num = Math.Min(32, value.Length);
					Encoding.ASCII.GetBytes(value, 0, num, this.m_AllBytes, 4);
					this.m_AllBytes[num + 4] = 0;
					this.m_CacheValid = true;
				}
			}
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x00049FD3 File Offset: 0x000481D3
		internal void Set(ref SocketIdInternal other)
		{
			this.SocketName = other.SocketName;
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x00049FE4 File Offset: 0x000481E4
		internal bool PrepareForUpdate()
		{
			bool cacheValid = this.m_CacheValid;
			this.m_CacheValid = false;
			this.EnsureStorage();
			this.CopyIdToSwapBuffer();
			return cacheValid;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x0004A014 File Offset: 0x00048214
		internal void CheckIfChanged(bool wasCacheValid)
		{
			bool flag = !wasCacheValid || this.m_SwapBuffer == null || this.m_AllBytes == null;
			if (!flag)
			{
				bool flag2 = true;
				for (int i = 0; i < this.m_SwapBuffer.Length; i++)
				{
					bool flag3 = this.m_AllBytes[4 + i] != this.m_SwapBuffer[i];
					if (flag3)
					{
						flag2 = false;
						break;
					}
				}
				bool flag4 = flag2;
				if (flag4)
				{
					this.m_CacheValid = true;
				}
			}
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x0004A090 File Offset: 0x00048290
		private void RebuildStringFromBuffer()
		{
			this.EnsureStorage();
			int num = 4;
			while (num < this.m_AllBytes.Length && this.m_AllBytes[num] > 0)
			{
				num++;
			}
			this.m_CachedSocketName = Encoding.ASCII.GetString(this.m_AllBytes, 4, num - 4);
			this.m_CacheValid = true;
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x0004A0F0 File Offset: 0x000482F0
		private void EnsureStorage()
		{
			bool flag = this.m_AllBytes == null || this.m_AllBytes.Length < 37;
			if (flag)
			{
				this.m_AllBytes = new byte[37];
				this.m_SwapBuffer = new byte[33];
				Array.Copy(BitConverter.GetBytes(1), 0, this.m_AllBytes, 0, 4);
			}
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x0004A14A File Offset: 0x0004834A
		private void CopyIdToSwapBuffer()
		{
			Array.Copy(this.m_AllBytes, 4, this.m_SwapBuffer, 0, this.m_SwapBuffer.Length);
		}

		// Token: 0x04001623 RID: 5667
		public static readonly SocketId Empty = default(SocketId);

		// Token: 0x04001624 RID: 5668
		private const int MaxSocketNameLength = 32;

		// Token: 0x04001625 RID: 5669
		private const int ApiVersionLength = 4;

		// Token: 0x04001626 RID: 5670
		private const int NullTerminatorSpace = 1;

		// Token: 0x04001627 RID: 5671
		private const int TotalSizeInBytes = 37;

		// Token: 0x04001628 RID: 5672
		private bool m_CacheValid;

		// Token: 0x04001629 RID: 5673
		private string m_CachedSocketName;

		// Token: 0x0400162A RID: 5674
		internal byte[] m_AllBytes;

		// Token: 0x0400162B RID: 5675
		internal byte[] m_SwapBuffer;
	}
}
