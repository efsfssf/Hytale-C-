using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000271 RID: 625
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateParticipantVolumeCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateParticipantVolumeCallbackInfo>, ISettable<UpdateParticipantVolumeCallbackInfo>, IDisposable
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x000193A0 File Offset: 0x000175A0
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x000193B8 File Offset: 0x000175B8
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

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x000193C4 File Offset: 0x000175C4
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x000193E5 File Offset: 0x000175E5
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

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x000193F8 File Offset: 0x000175F8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x00019410 File Offset: 0x00017610
		// (set) Token: 0x06001162 RID: 4450 RVA: 0x00019431 File Offset: 0x00017631
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

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x00019444 File Offset: 0x00017644
		// (set) Token: 0x06001164 RID: 4452 RVA: 0x00019465 File Offset: 0x00017665
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

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00019478 File Offset: 0x00017678
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x00019499 File Offset: 0x00017699
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

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x000194AC File Offset: 0x000176AC
		// (set) Token: 0x06001168 RID: 4456 RVA: 0x000194C4 File Offset: 0x000176C4
		public float Volume
		{
			get
			{
				return this.m_Volume;
			}
			set
			{
				this.m_Volume = value;
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000194D0 File Offset: 0x000176D0
		public void Set(ref UpdateParticipantVolumeCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.Volume = other.Volume;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x0001952C File Offset: 0x0001772C
		public void Set(ref UpdateParticipantVolumeCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.Volume = other.Value.Volume;
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000195C7 File Offset: 0x000177C7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000195FA File Offset: 0x000177FA
		public void Get(out UpdateParticipantVolumeCallbackInfo output)
		{
			output = default(UpdateParticipantVolumeCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040007A1 RID: 1953
		private Result m_ResultCode;

		// Token: 0x040007A2 RID: 1954
		private IntPtr m_ClientData;

		// Token: 0x040007A3 RID: 1955
		private IntPtr m_LocalUserId;

		// Token: 0x040007A4 RID: 1956
		private IntPtr m_RoomName;

		// Token: 0x040007A5 RID: 1957
		private IntPtr m_ParticipantId;

		// Token: 0x040007A6 RID: 1958
		private float m_Volume;
	}
}
