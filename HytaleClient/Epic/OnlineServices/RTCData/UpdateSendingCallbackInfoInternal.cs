using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001F3 RID: 499
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSendingCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSendingCallbackInfo>, ISettable<UpdateSendingCallbackInfo>, IDisposable
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0001532C File Offset: 0x0001352C
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x00015344 File Offset: 0x00013544
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

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00015350 File Offset: 0x00013550
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x00015371 File Offset: 0x00013571
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

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00015384 File Offset: 0x00013584
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0001539C File Offset: 0x0001359C
		// (set) Token: 0x06000E80 RID: 3712 RVA: 0x000153BD File Offset: 0x000135BD
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

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x000153D0 File Offset: 0x000135D0
		// (set) Token: 0x06000E82 RID: 3714 RVA: 0x000153F1 File Offset: 0x000135F1
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

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00015404 File Offset: 0x00013604
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x00015425 File Offset: 0x00013625
		public bool DataEnabled
		{
			get
			{
				bool result;
				Helper.Get(this.m_DataEnabled, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DataEnabled);
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00015438 File Offset: 0x00013638
		public void Set(ref UpdateSendingCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.DataEnabled = other.DataEnabled;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00015488 File Offset: 0x00013688
		public void Set(ref UpdateSendingCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.DataEnabled = other.Value.DataEnabled;
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0001550B File Offset: 0x0001370B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00015532 File Offset: 0x00013732
		public void Get(out UpdateSendingCallbackInfo output)
		{
			output = default(UpdateSendingCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400068B RID: 1675
		private Result m_ResultCode;

		// Token: 0x0400068C RID: 1676
		private IntPtr m_ClientData;

		// Token: 0x0400068D RID: 1677
		private IntPtr m_LocalUserId;

		// Token: 0x0400068E RID: 1678
		private IntPtr m_RoomName;

		// Token: 0x0400068F RID: 1679
		private int m_DataEnabled;
	}
}
