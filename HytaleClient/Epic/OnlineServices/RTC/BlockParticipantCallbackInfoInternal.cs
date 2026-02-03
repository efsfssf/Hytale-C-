using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B2 RID: 434
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BlockParticipantCallbackInfoInternal : ICallbackInfoInternal, IGettable<BlockParticipantCallbackInfo>, ISettable<BlockParticipantCallbackInfo>, IDisposable
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x000123F8 File Offset: 0x000105F8
		// (set) Token: 0x06000C82 RID: 3202 RVA: 0x00012410 File Offset: 0x00010610
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0001241C File Offset: 0x0001061C
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x0001243D File Offset: 0x0001063D
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00012450 File Offset: 0x00010650
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00012468 File Offset: 0x00010668
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x00012489 File Offset: 0x00010689
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0001249C File Offset: 0x0001069C
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x000124BD File Offset: 0x000106BD
		public Utf8String RoomName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RoomName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x000124D0 File Offset: 0x000106D0
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x000124F1 File Offset: 0x000106F1
		public ProductUserId ParticipantId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ParticipantId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00012504 File Offset: 0x00010704
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x00012525 File Offset: 0x00010725
		public bool Blocked
		{
			get
			{
				bool result;
				Helper.Get(this.m_Blocked, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Blocked);
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00012538 File Offset: 0x00010738
		public void Set(ref BlockParticipantCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Blocked = other.Blocked;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00012594 File Offset: 0x00010794
		public void Set(ref BlockParticipantCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.Blocked = other.Value.Blocked;
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0001262F File Offset: 0x0001082F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00012662 File Offset: 0x00010862
		public void Get(out BlockParticipantCallbackInfo output)
		{
			output = default(BlockParticipantCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040005BC RID: 1468
		private Result m_ResultCode;

		// Token: 0x040005BD RID: 1469
		private IntPtr m_ClientData;

		// Token: 0x040005BE RID: 1470
		private IntPtr m_LocalUserId;

		// Token: 0x040005BF RID: 1471
		private IntPtr m_RoomName;

		// Token: 0x040005C0 RID: 1472
		private IntPtr m_ParticipantId;

		// Token: 0x040005C1 RID: 1473
		private int m_Blocked;
	}
}
