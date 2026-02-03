using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005EA RID: 1514
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IdTokenInternal : IGettable<IdToken>, ISettable<IdToken>, IDisposable
	{
		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x0003A224 File Offset: 0x00038424
		// (set) Token: 0x0600274A RID: 10058 RVA: 0x0003A245 File Offset: 0x00038445
		public ProductUserId ProductUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ProductUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ProductUserId);
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x0003A258 File Offset: 0x00038458
		// (set) Token: 0x0600274C RID: 10060 RVA: 0x0003A279 File Offset: 0x00038479
		public Utf8String JsonWebToken
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_JsonWebToken, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_JsonWebToken);
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x0003A289 File Offset: 0x00038489
		public void Set(ref IdToken other)
		{
			this.m_ApiVersion = 1;
			this.ProductUserId = other.ProductUserId;
			this.JsonWebToken = other.JsonWebToken;
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x0003A2B0 File Offset: 0x000384B0
		public void Set(ref IdToken? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ProductUserId = other.Value.ProductUserId;
				this.JsonWebToken = other.Value.JsonWebToken;
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x0003A2FB File Offset: 0x000384FB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ProductUserId);
			Helper.Dispose(ref this.m_JsonWebToken);
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x0003A316 File Offset: 0x00038516
		public void Get(out IdToken output)
		{
			output = default(IdToken);
			output.Set(ref this);
		}

		// Token: 0x0400110C RID: 4364
		private int m_ApiVersion;

		// Token: 0x0400110D RID: 4365
		private IntPtr m_ProductUserId;

		// Token: 0x0400110E RID: 4366
		private IntPtr m_JsonWebToken;
	}
}
