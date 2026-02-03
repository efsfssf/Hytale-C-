using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001EF RID: 495
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateReceivingCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateReceivingCallbackInfo>, ISettable<UpdateReceivingCallbackInfo>, IDisposable
	{
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00014E88 File Offset: 0x00013088
		// (set) Token: 0x06000E4F RID: 3663 RVA: 0x00014EA0 File Offset: 0x000130A0
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

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00014EAC File Offset: 0x000130AC
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x00014ECD File Offset: 0x000130CD
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

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00014EE0 File Offset: 0x000130E0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x00014EF8 File Offset: 0x000130F8
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x00014F19 File Offset: 0x00013119
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

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00014F2C File Offset: 0x0001312C
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x00014F4D File Offset: 0x0001314D
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

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x00014F60 File Offset: 0x00013160
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00014F81 File Offset: 0x00013181
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

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00014F94 File Offset: 0x00013194
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x00014FB5 File Offset: 0x000131B5
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

		// Token: 0x06000E5B RID: 3675 RVA: 0x00014FC8 File Offset: 0x000131C8
		public void Set(ref UpdateReceivingCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.DataEnabled = other.DataEnabled;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00015024 File Offset: 0x00013224
		public void Set(ref UpdateReceivingCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.DataEnabled = other.Value.DataEnabled;
			}
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000150BF File Offset: 0x000132BF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000150F2 File Offset: 0x000132F2
		public void Get(out UpdateReceivingCallbackInfo output)
		{
			output = default(UpdateReceivingCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000677 RID: 1655
		private Result m_ResultCode;

		// Token: 0x04000678 RID: 1656
		private IntPtr m_ClientData;

		// Token: 0x04000679 RID: 1657
		private IntPtr m_LocalUserId;

		// Token: 0x0400067A RID: 1658
		private IntPtr m_RoomName;

		// Token: 0x0400067B RID: 1659
		private IntPtr m_ParticipantId;

		// Token: 0x0400067C RID: 1660
		private int m_DataEnabled;
	}
}
