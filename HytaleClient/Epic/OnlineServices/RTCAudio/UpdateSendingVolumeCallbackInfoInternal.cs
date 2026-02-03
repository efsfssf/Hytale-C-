using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000281 RID: 641
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSendingVolumeCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSendingVolumeCallbackInfo>, ISettable<UpdateSendingVolumeCallbackInfo>, IDisposable
	{
		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x0001A49C File Offset: 0x0001869C
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x0001A4B4 File Offset: 0x000186B4
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

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0001A4C0 File Offset: 0x000186C0
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x0001A4E1 File Offset: 0x000186E1
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

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x0001A4F4 File Offset: 0x000186F4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x0001A50C File Offset: 0x0001870C
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x0001A52D File Offset: 0x0001872D
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

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x0001A540 File Offset: 0x00018740
		// (set) Token: 0x0600120C RID: 4620 RVA: 0x0001A561 File Offset: 0x00018761
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

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x0001A574 File Offset: 0x00018774
		// (set) Token: 0x0600120E RID: 4622 RVA: 0x0001A58C File Offset: 0x0001878C
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

		// Token: 0x0600120F RID: 4623 RVA: 0x0001A598 File Offset: 0x00018798
		public void Set(ref UpdateSendingVolumeCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Volume = other.Volume;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0001A5E8 File Offset: 0x000187E8
		public void Set(ref UpdateSendingVolumeCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Volume = other.Value.Volume;
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x0001A66B File Offset: 0x0001886B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0001A692 File Offset: 0x00018892
		public void Get(out UpdateSendingVolumeCallbackInfo output)
		{
			output = default(UpdateSendingVolumeCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040007EC RID: 2028
		private Result m_ResultCode;

		// Token: 0x040007ED RID: 2029
		private IntPtr m_ClientData;

		// Token: 0x040007EE RID: 2030
		private IntPtr m_LocalUserId;

		// Token: 0x040007EF RID: 2031
		private IntPtr m_RoomName;

		// Token: 0x040007F0 RID: 2032
		private float m_Volume;
	}
}
