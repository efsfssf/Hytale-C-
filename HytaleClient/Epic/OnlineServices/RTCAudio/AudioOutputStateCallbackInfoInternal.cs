using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000211 RID: 529
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioOutputStateCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioOutputStateCallbackInfo>, ISettable<AudioOutputStateCallbackInfo>, IDisposable
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00016910 File Offset: 0x00014B10
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x00016931 File Offset: 0x00014B31
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

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x00016944 File Offset: 0x00014B44
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0001695C File Offset: 0x00014B5C
		// (set) Token: 0x06000F5D RID: 3933 RVA: 0x0001697D File Offset: 0x00014B7D
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

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x00016990 File Offset: 0x00014B90
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x000169B1 File Offset: 0x00014BB1
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

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x000169C4 File Offset: 0x00014BC4
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x000169DC File Offset: 0x00014BDC
		public RTCAudioOutputStatus Status
		{
			get
			{
				return this.m_Status;
			}
			set
			{
				this.m_Status = value;
			}
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x000169E6 File Offset: 0x00014BE6
		public void Set(ref AudioOutputStateCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Status = other.Status;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00016A20 File Offset: 0x00014C20
		public void Set(ref AudioOutputStateCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Status = other.Value.Status;
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00016A8E File Offset: 0x00014C8E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00016AB5 File Offset: 0x00014CB5
		public void Get(out AudioOutputStateCallbackInfo output)
		{
			output = default(AudioOutputStateCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040006E9 RID: 1769
		private IntPtr m_ClientData;

		// Token: 0x040006EA RID: 1770
		private IntPtr m_LocalUserId;

		// Token: 0x040006EB RID: 1771
		private IntPtr m_RoomName;

		// Token: 0x040006EC RID: 1772
		private RTCAudioOutputStatus m_Status;
	}
}
