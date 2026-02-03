using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005C7 RID: 1479
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AuthExpirationCallbackInfoInternal : ICallbackInfoInternal, IGettable<AuthExpirationCallbackInfo>, ISettable<AuthExpirationCallbackInfo>, IDisposable
	{
		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06002667 RID: 9831 RVA: 0x00038730 File Offset: 0x00036930
		// (set) Token: 0x06002668 RID: 9832 RVA: 0x00038751 File Offset: 0x00036951
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

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x00038764 File Offset: 0x00036964
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x0600266A RID: 9834 RVA: 0x0003877C File Offset: 0x0003697C
		// (set) Token: 0x0600266B RID: 9835 RVA: 0x0003879D File Offset: 0x0003699D
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

		// Token: 0x0600266C RID: 9836 RVA: 0x000387AD File Offset: 0x000369AD
		public void Set(ref AuthExpirationCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000387CC File Offset: 0x000369CC
		public void Set(ref AuthExpirationCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x00038810 File Offset: 0x00036A10
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0003882B File Offset: 0x00036A2B
		public void Get(out AuthExpirationCallbackInfo output)
		{
			output = default(AuthExpirationCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400109D RID: 4253
		private IntPtr m_ClientData;

		// Token: 0x0400109E RID: 4254
		private IntPtr m_LocalUserId;
	}
}
