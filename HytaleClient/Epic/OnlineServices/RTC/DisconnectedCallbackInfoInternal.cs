using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B6 RID: 438
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DisconnectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<DisconnectedCallbackInfo>, ISettable<DisconnectedCallbackInfo>, IDisposable
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0001286C File Offset: 0x00010A6C
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00012884 File Offset: 0x00010A84
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

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00012890 File Offset: 0x00010A90
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x000128B1 File Offset: 0x00010AB1
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

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x000128C4 File Offset: 0x00010AC4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x000128DC File Offset: 0x00010ADC
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x000128FD File Offset: 0x00010AFD
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

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00012910 File Offset: 0x00010B10
		// (set) Token: 0x06000CB3 RID: 3251 RVA: 0x00012931 File Offset: 0x00010B31
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

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00012941 File Offset: 0x00010B41
		public void Set(ref DisconnectedCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00012978 File Offset: 0x00010B78
		public void Set(ref DisconnectedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000129E6 File Offset: 0x00010BE6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00012A0D File Offset: 0x00010C0D
		public void Get(out DisconnectedCallbackInfo output)
		{
			output = default(DisconnectedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040005CF RID: 1487
		private Result m_ResultCode;

		// Token: 0x040005D0 RID: 1488
		private IntPtr m_ClientData;

		// Token: 0x040005D1 RID: 1489
		private IntPtr m_LocalUserId;

		// Token: 0x040005D2 RID: 1490
		private IntPtr m_RoomName;
	}
}
