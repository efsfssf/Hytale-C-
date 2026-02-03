using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001E9 RID: 489
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ParticipantUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantUpdatedCallbackInfo>, ISettable<ParticipantUpdatedCallbackInfo>, IDisposable
	{
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x000147AC File Offset: 0x000129AC
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x000147CD File Offset: 0x000129CD
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

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x000147E0 File Offset: 0x000129E0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x000147F8 File Offset: 0x000129F8
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x00014819 File Offset: 0x00012A19
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

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0001482C File Offset: 0x00012A2C
		// (set) Token: 0x06000E1E RID: 3614 RVA: 0x0001484D File Offset: 0x00012A4D
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

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00014860 File Offset: 0x00012A60
		// (set) Token: 0x06000E20 RID: 3616 RVA: 0x00014881 File Offset: 0x00012A81
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

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00014894 File Offset: 0x00012A94
		// (set) Token: 0x06000E22 RID: 3618 RVA: 0x000148AC File Offset: 0x00012AAC
		public RTCDataStatus DataStatus
		{
			get
			{
				return this.m_DataStatus;
			}
			set
			{
				this.m_DataStatus = value;
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000148B8 File Offset: 0x00012AB8
		public void Set(ref ParticipantUpdatedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.DataStatus = other.DataStatus;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00014908 File Offset: 0x00012B08
		public void Set(ref ParticipantUpdatedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.DataStatus = other.Value.DataStatus;
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0001498B File Offset: 0x00012B8B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x000149BE File Offset: 0x00012BBE
		public void Get(out ParticipantUpdatedCallbackInfo output)
		{
			output = default(ParticipantUpdatedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400065A RID: 1626
		private IntPtr m_ClientData;

		// Token: 0x0400065B RID: 1627
		private IntPtr m_LocalUserId;

		// Token: 0x0400065C RID: 1628
		private IntPtr m_RoomName;

		// Token: 0x0400065D RID: 1629
		private IntPtr m_ParticipantId;

		// Token: 0x0400065E RID: 1630
		private RTCDataStatus m_DataStatus;
	}
}
