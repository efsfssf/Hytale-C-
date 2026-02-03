using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001B8 RID: 440
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinRoomCallbackInfo>, ISettable<JoinRoomCallbackInfo>, IDisposable
	{
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00012AE4 File Offset: 0x00010CE4
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x00012AFC File Offset: 0x00010CFC
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

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00012B08 File Offset: 0x00010D08
		// (set) Token: 0x06000CC7 RID: 3271 RVA: 0x00012B29 File Offset: 0x00010D29
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

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00012B3C File Offset: 0x00010D3C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00012B54 File Offset: 0x00010D54
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00012B75 File Offset: 0x00010D75
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

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00012B88 File Offset: 0x00010D88
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00012BA9 File Offset: 0x00010DA9
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

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00012BBC File Offset: 0x00010DBC
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00012BE3 File Offset: 0x00010DE3
		public Option[] RoomOptions
		{
			get
			{
				Option[] result;
				Helper.Get<OptionInternal, Option>(this.m_RoomOptions, out result, this.m_RoomOptionsCount);
				return result;
			}
			set
			{
				Helper.Set<Option, OptionInternal>(ref value, ref this.m_RoomOptions, out this.m_RoomOptionsCount);
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00012BFC File Offset: 0x00010DFC
		public void Set(ref JoinRoomCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.RoomOptions = other.RoomOptions;
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00012C4C File Offset: 0x00010E4C
		public void Set(ref JoinRoomCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.RoomOptions = other.Value.RoomOptions;
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00012CCF File Offset: 0x00010ECF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_RoomOptions);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00012D02 File Offset: 0x00010F02
		public void Get(out JoinRoomCallbackInfo output)
		{
			output = default(JoinRoomCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040005D8 RID: 1496
		private Result m_ResultCode;

		// Token: 0x040005D9 RID: 1497
		private IntPtr m_ClientData;

		// Token: 0x040005DA RID: 1498
		private IntPtr m_LocalUserId;

		// Token: 0x040005DB RID: 1499
		private IntPtr m_RoomName;

		// Token: 0x040005DC RID: 1500
		private uint m_RoomOptionsCount;

		// Token: 0x040005DD RID: 1501
		private IntPtr m_RoomOptions;
	}
}
