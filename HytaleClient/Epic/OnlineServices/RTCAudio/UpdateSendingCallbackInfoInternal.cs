using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200027D RID: 637
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSendingCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSendingCallbackInfo>, ISettable<UpdateSendingCallbackInfo>, IDisposable
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x0001A0C4 File Offset: 0x000182C4
		// (set) Token: 0x060011DE RID: 4574 RVA: 0x0001A0DC File Offset: 0x000182DC
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

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x0001A0E8 File Offset: 0x000182E8
		// (set) Token: 0x060011E0 RID: 4576 RVA: 0x0001A109 File Offset: 0x00018309
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

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0001A11C File Offset: 0x0001831C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x0001A134 File Offset: 0x00018334
		// (set) Token: 0x060011E3 RID: 4579 RVA: 0x0001A155 File Offset: 0x00018355
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

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x0001A168 File Offset: 0x00018368
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x0001A189 File Offset: 0x00018389
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

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0001A19C File Offset: 0x0001839C
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x0001A1B4 File Offset: 0x000183B4
		public RTCAudioStatus AudioStatus
		{
			get
			{
				return this.m_AudioStatus;
			}
			set
			{
				this.m_AudioStatus = value;
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0001A1C0 File Offset: 0x000183C0
		public void Set(ref UpdateSendingCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.AudioStatus = other.AudioStatus;
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0001A210 File Offset: 0x00018410
		public void Set(ref UpdateSendingCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.AudioStatus = other.Value.AudioStatus;
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0001A293 File Offset: 0x00018493
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0001A2BA File Offset: 0x000184BA
		public void Get(out UpdateSendingCallbackInfo output)
		{
			output = default(UpdateSendingCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040007DB RID: 2011
		private Result m_ResultCode;

		// Token: 0x040007DC RID: 2012
		private IntPtr m_ClientData;

		// Token: 0x040007DD RID: 2013
		private IntPtr m_LocalUserId;

		// Token: 0x040007DE RID: 2014
		private IntPtr m_RoomName;

		// Token: 0x040007DF RID: 2015
		private RTCAudioStatus m_AudioStatus;
	}
}
