using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000275 RID: 629
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateReceivingCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateReceivingCallbackInfo>, ISettable<UpdateReceivingCallbackInfo>, IDisposable
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00019848 File Offset: 0x00017A48
		// (set) Token: 0x0600118B RID: 4491 RVA: 0x00019860 File Offset: 0x00017A60
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

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x0001986C File Offset: 0x00017A6C
		// (set) Token: 0x0600118D RID: 4493 RVA: 0x0001988D File Offset: 0x00017A8D
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

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x000198A0 File Offset: 0x00017AA0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x000198B8 File Offset: 0x00017AB8
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x000198D9 File Offset: 0x00017AD9
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

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x000198EC File Offset: 0x00017AEC
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x0001990D File Offset: 0x00017B0D
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

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x00019920 File Offset: 0x00017B20
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x00019941 File Offset: 0x00017B41
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

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00019954 File Offset: 0x00017B54
		// (set) Token: 0x06001196 RID: 4502 RVA: 0x00019975 File Offset: 0x00017B75
		public bool AudioEnabled
		{
			get
			{
				bool result;
				Helper.Get(this.m_AudioEnabled, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AudioEnabled);
			}
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00019988 File Offset: 0x00017B88
		public void Set(ref UpdateReceivingCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.AudioEnabled = other.AudioEnabled;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000199E4 File Offset: 0x00017BE4
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
				this.AudioEnabled = other.Value.AudioEnabled;
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00019A7F File Offset: 0x00017C7F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00019AB2 File Offset: 0x00017CB2
		public void Get(out UpdateReceivingCallbackInfo output)
		{
			output = default(UpdateReceivingCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040007B6 RID: 1974
		private Result m_ResultCode;

		// Token: 0x040007B7 RID: 1975
		private IntPtr m_ClientData;

		// Token: 0x040007B8 RID: 1976
		private IntPtr m_LocalUserId;

		// Token: 0x040007B9 RID: 1977
		private IntPtr m_RoomName;

		// Token: 0x040007BA RID: 1978
		private IntPtr m_ParticipantId;

		// Token: 0x040007BB RID: 1979
		private int m_AudioEnabled;
	}
}
