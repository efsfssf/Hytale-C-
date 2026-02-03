using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x020003CE RID: 974
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LobbyDetailsMemberInfoInternal : IGettable<LobbyDetailsMemberInfo>, ISettable<LobbyDetailsMemberInfo>, IDisposable
	{
		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x00026614 File Offset: 0x00024814
		// (set) Token: 0x06001A1A RID: 6682 RVA: 0x00026635 File Offset: 0x00024835
		public ProductUserId UserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_UserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x00026648 File Offset: 0x00024848
		// (set) Token: 0x06001A1C RID: 6684 RVA: 0x00026660 File Offset: 0x00024860
		public uint Platform
		{
			get
			{
				return this.m_Platform;
			}
			set
			{
				this.m_Platform = value;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x0002666C File Offset: 0x0002486C
		// (set) Token: 0x06001A1E RID: 6686 RVA: 0x0002668D File Offset: 0x0002488D
		public bool AllowsCrossplay
		{
			get
			{
				bool result;
				Helper.Get(this.m_AllowsCrossplay, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AllowsCrossplay);
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0002669D File Offset: 0x0002489D
		public void Set(ref LobbyDetailsMemberInfo other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.Platform = other.Platform;
			this.AllowsCrossplay = other.AllowsCrossplay;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x000266D0 File Offset: 0x000248D0
		public void Set(ref LobbyDetailsMemberInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.Platform = other.Value.Platform;
				this.AllowsCrossplay = other.Value.AllowsCrossplay;
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00026730 File Offset: 0x00024930
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0002673F File Offset: 0x0002493F
		public void Get(out LobbyDetailsMemberInfo output)
		{
			output = default(LobbyDetailsMemberInfo);
			output.Set(ref this);
		}

		// Token: 0x04000B99 RID: 2969
		private int m_ApiVersion;

		// Token: 0x04000B9A RID: 2970
		private IntPtr m_UserId;

		// Token: 0x04000B9B RID: 2971
		private uint m_Platform;

		// Token: 0x04000B9C RID: 2972
		private int m_AllowsCrossplay;
	}
}
