using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004A0 RID: 1184
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PermissionsUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<PermissionsUpdateReceivedCallbackInfo>, ISettable<PermissionsUpdateReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x0002D090 File Offset: 0x0002B290
		// (set) Token: 0x06001ED8 RID: 7896 RVA: 0x0002D0B1 File Offset: 0x0002B2B1
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

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x0002D0C4 File Offset: 0x0002B2C4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0002D0DC File Offset: 0x0002B2DC
		// (set) Token: 0x06001EDB RID: 7899 RVA: 0x0002D0FD File Offset: 0x0002B2FD
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

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x0002D110 File Offset: 0x0002B310
		// (set) Token: 0x06001EDD RID: 7901 RVA: 0x0002D131 File Offset: 0x0002B331
		public Utf8String KWSUserId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_KWSUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_KWSUserId);
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06001EDE RID: 7902 RVA: 0x0002D144 File Offset: 0x0002B344
		// (set) Token: 0x06001EDF RID: 7903 RVA: 0x0002D165 File Offset: 0x0002B365
		public Utf8String DateOfBirth
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DateOfBirth, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DateOfBirth);
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x0002D178 File Offset: 0x0002B378
		// (set) Token: 0x06001EE1 RID: 7905 RVA: 0x0002D199 File Offset: 0x0002B399
		public bool IsMinor
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsMinor, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsMinor);
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x0002D1AC File Offset: 0x0002B3AC
		// (set) Token: 0x06001EE3 RID: 7907 RVA: 0x0002D1CD File Offset: 0x0002B3CD
		public Utf8String ParentEmail
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ParentEmail, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParentEmail);
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0002D1E0 File Offset: 0x0002B3E0
		public void Set(ref PermissionsUpdateReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.KWSUserId = other.KWSUserId;
			this.DateOfBirth = other.DateOfBirth;
			this.IsMinor = other.IsMinor;
			this.ParentEmail = other.ParentEmail;
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0002D23C File Offset: 0x0002B43C
		public void Set(ref PermissionsUpdateReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.KWSUserId = other.Value.KWSUserId;
				this.DateOfBirth = other.Value.DateOfBirth;
				this.IsMinor = other.Value.IsMinor;
				this.ParentEmail = other.Value.ParentEmail;
			}
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0002D2D7 File Offset: 0x0002B4D7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_KWSUserId);
			Helper.Dispose(ref this.m_DateOfBirth);
			Helper.Dispose(ref this.m_ParentEmail);
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0002D316 File Offset: 0x0002B516
		public void Get(out PermissionsUpdateReceivedCallbackInfo output)
		{
			output = default(PermissionsUpdateReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D69 RID: 3433
		private IntPtr m_ClientData;

		// Token: 0x04000D6A RID: 3434
		private IntPtr m_LocalUserId;

		// Token: 0x04000D6B RID: 3435
		private IntPtr m_KWSUserId;

		// Token: 0x04000D6C RID: 3436
		private IntPtr m_DateOfBirth;

		// Token: 0x04000D6D RID: 3437
		private int m_IsMinor;

		// Token: 0x04000D6E RID: 3438
		private IntPtr m_ParentEmail;
	}
}
