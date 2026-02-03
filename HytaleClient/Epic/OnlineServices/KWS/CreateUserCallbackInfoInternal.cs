using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x02000488 RID: 1160
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreateUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreateUserCallbackInfo>, ISettable<CreateUserCallbackInfo>, IDisposable
	{
		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x0002C5D0 File Offset: 0x0002A7D0
		// (set) Token: 0x06001E51 RID: 7761 RVA: 0x0002C5E8 File Offset: 0x0002A7E8
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

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x0002C5F4 File Offset: 0x0002A7F4
		// (set) Token: 0x06001E53 RID: 7763 RVA: 0x0002C615 File Offset: 0x0002A815
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

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x0002C628 File Offset: 0x0002A828
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x0002C640 File Offset: 0x0002A840
		// (set) Token: 0x06001E56 RID: 7766 RVA: 0x0002C661 File Offset: 0x0002A861
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

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0002C674 File Offset: 0x0002A874
		// (set) Token: 0x06001E58 RID: 7768 RVA: 0x0002C695 File Offset: 0x0002A895
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

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0002C6A8 File Offset: 0x0002A8A8
		// (set) Token: 0x06001E5A RID: 7770 RVA: 0x0002C6C9 File Offset: 0x0002A8C9
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

		// Token: 0x06001E5B RID: 7771 RVA: 0x0002C6DC File Offset: 0x0002A8DC
		public void Set(ref CreateUserCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.KWSUserId = other.KWSUserId;
			this.IsMinor = other.IsMinor;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0002C72C File Offset: 0x0002A92C
		public void Set(ref CreateUserCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.KWSUserId = other.Value.KWSUserId;
				this.IsMinor = other.Value.IsMinor;
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0002C7AF File Offset: 0x0002A9AF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_KWSUserId);
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0002C7D6 File Offset: 0x0002A9D6
		public void Get(out CreateUserCallbackInfo output)
		{
			output = default(CreateUserCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D3A RID: 3386
		private Result m_ResultCode;

		// Token: 0x04000D3B RID: 3387
		private IntPtr m_ClientData;

		// Token: 0x04000D3C RID: 3388
		private IntPtr m_LocalUserId;

		// Token: 0x04000D3D RID: 3389
		private IntPtr m_KWSUserId;

		// Token: 0x04000D3E RID: 3390
		private int m_IsMinor;
	}
}
