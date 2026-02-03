using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B4 RID: 436
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BlockParticipantOptionsInternal : ISettable<BlockParticipantOptions>, IDisposable
	{
		// Token: 0x170002FD RID: 765
		// (set) Token: 0x06000C9A RID: 3226 RVA: 0x000126B8 File Offset: 0x000108B8
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170002FE RID: 766
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x000126C8 File Offset: 0x000108C8
		public Utf8String RoomName
		{
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170002FF RID: 767
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x000126D8 File Offset: 0x000108D8
		public ProductUserId ParticipantId
		{
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x17000300 RID: 768
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x000126E8 File Offset: 0x000108E8
		public bool Blocked
		{
			set
			{
				Helper.Set(value, ref this.m_Blocked);
			}
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x000126F8 File Offset: 0x000108F8
		public void Set(ref BlockParticipantOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Blocked = other.Blocked;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00012738 File Offset: 0x00010938
		public void Set(ref BlockParticipantOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.Blocked = other.Value.Blocked;
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000127AD File Offset: 0x000109AD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x040005C6 RID: 1478
		private int m_ApiVersion;

		// Token: 0x040005C7 RID: 1479
		private IntPtr m_LocalUserId;

		// Token: 0x040005C8 RID: 1480
		private IntPtr m_RoomName;

		// Token: 0x040005C9 RID: 1481
		private IntPtr m_ParticipantId;

		// Token: 0x040005CA RID: 1482
		private int m_Blocked;
	}
}
