using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	// Token: 0x0200029C RID: 668
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UserTokenInternal : IGettable<UserToken>, ISettable<UserToken>, IDisposable
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x0001B4E4 File Offset: 0x000196E4
		// (set) Token: 0x060012BA RID: 4794 RVA: 0x0001B505 File Offset: 0x00019705
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

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x0001B518 File Offset: 0x00019718
		// (set) Token: 0x060012BC RID: 4796 RVA: 0x0001B539 File Offset: 0x00019739
		public Utf8String Token
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Token, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Token);
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0001B549 File Offset: 0x00019749
		public void Set(ref UserToken other)
		{
			this.m_ApiVersion = 1;
			this.ProductUserId = other.ProductUserId;
			this.Token = other.Token;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0001B570 File Offset: 0x00019770
		public void Set(ref UserToken? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.ProductUserId = other.Value.ProductUserId;
				this.Token = other.Value.Token;
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0001B5BB File Offset: 0x000197BB
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ProductUserId);
			Helper.Dispose(ref this.m_Token);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0001B5D6 File Offset: 0x000197D6
		public void Get(out UserToken output)
		{
			output = default(UserToken);
			output.Set(ref this);
		}

		// Token: 0x04000834 RID: 2100
		private int m_ApiVersion;

		// Token: 0x04000835 RID: 2101
		private IntPtr m_ProductUserId;

		// Token: 0x04000836 RID: 2102
		private IntPtr m_Token;
	}
}
