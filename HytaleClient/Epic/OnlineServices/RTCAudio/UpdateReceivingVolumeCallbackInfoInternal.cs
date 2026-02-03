using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000279 RID: 633
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateReceivingVolumeCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateReceivingVolumeCallbackInfo>, ISettable<UpdateReceivingVolumeCallbackInfo>, IDisposable
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00019CEC File Offset: 0x00017EEC
		// (set) Token: 0x060011B7 RID: 4535 RVA: 0x00019D04 File Offset: 0x00017F04
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

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x00019D10 File Offset: 0x00017F10
		// (set) Token: 0x060011B9 RID: 4537 RVA: 0x00019D31 File Offset: 0x00017F31
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

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00019D44 File Offset: 0x00017F44
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00019D5C File Offset: 0x00017F5C
		// (set) Token: 0x060011BC RID: 4540 RVA: 0x00019D7D File Offset: 0x00017F7D
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

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x00019D90 File Offset: 0x00017F90
		// (set) Token: 0x060011BE RID: 4542 RVA: 0x00019DB1 File Offset: 0x00017FB1
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

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x00019DC4 File Offset: 0x00017FC4
		// (set) Token: 0x060011C0 RID: 4544 RVA: 0x00019DDC File Offset: 0x00017FDC
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

		// Token: 0x060011C1 RID: 4545 RVA: 0x00019DE8 File Offset: 0x00017FE8
		public void Set(ref UpdateReceivingVolumeCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Volume = other.Volume;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00019E38 File Offset: 0x00018038
		public void Set(ref UpdateReceivingVolumeCallbackInfo? other)
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

		// Token: 0x060011C3 RID: 4547 RVA: 0x00019EBB File Offset: 0x000180BB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00019EE2 File Offset: 0x000180E2
		public void Get(out UpdateReceivingVolumeCallbackInfo output)
		{
			output = default(UpdateReceivingVolumeCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040007CA RID: 1994
		private Result m_ResultCode;

		// Token: 0x040007CB RID: 1995
		private IntPtr m_ClientData;

		// Token: 0x040007CC RID: 1996
		private IntPtr m_LocalUserId;

		// Token: 0x040007CD RID: 1997
		private IntPtr m_RoomName;

		// Token: 0x040007CE RID: 1998
		private float m_Volume;
	}
}
