using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001BD RID: 445
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LeaveRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveRoomCallbackInfo>, ISettable<LeaveRoomCallbackInfo>, IDisposable
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x0001303C File Offset: 0x0001123C
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x00013054 File Offset: 0x00011254
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

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00013060 File Offset: 0x00011260
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x00013081 File Offset: 0x00011281
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

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00013094 File Offset: 0x00011294
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x000130AC File Offset: 0x000112AC
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x000130CD File Offset: 0x000112CD
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

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x000130E0 File Offset: 0x000112E0
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x00013101 File Offset: 0x00011301
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

		// Token: 0x06000D01 RID: 3329 RVA: 0x00013111 File Offset: 0x00011311
		public void Set(ref LeaveRoomCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00013148 File Offset: 0x00011348
		public void Set(ref LeaveRoomCallbackInfo? other)
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

		// Token: 0x06000D03 RID: 3331 RVA: 0x000131B6 File Offset: 0x000113B6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000131DD File Offset: 0x000113DD
		public void Get(out LeaveRoomCallbackInfo output)
		{
			output = default(LeaveRoomCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040005F7 RID: 1527
		private Result m_ResultCode;

		// Token: 0x040005F8 RID: 1528
		private IntPtr m_ClientData;

		// Token: 0x040005F9 RID: 1529
		private IntPtr m_LocalUserId;

		// Token: 0x040005FA RID: 1530
		private IntPtr m_RoomName;
	}
}
