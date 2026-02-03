using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000508 RID: 1288
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CheckoutEntryInternal : IGettable<CheckoutEntry>, ISettable<CheckoutEntry>, IDisposable
	{
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x00031C3C File Offset: 0x0002FE3C
		// (set) Token: 0x060021E3 RID: 8675 RVA: 0x00031C5D File Offset: 0x0002FE5D
		public Utf8String OfferId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_OfferId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OfferId);
			}
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00031C6D File Offset: 0x0002FE6D
		public void Set(ref CheckoutEntry other)
		{
			this.m_ApiVersion = 1;
			this.OfferId = other.OfferId;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x00031C84 File Offset: 0x0002FE84
		public void Set(ref CheckoutEntry? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.OfferId = other.Value.OfferId;
			}
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x00031CBA File Offset: 0x0002FEBA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_OfferId);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00031CC9 File Offset: 0x0002FEC9
		public void Get(out CheckoutEntry output)
		{
			output = default(CheckoutEntry);
			output.Set(ref this);
		}

		// Token: 0x04000EB0 RID: 3760
		private int m_ApiVersion;

		// Token: 0x04000EB1 RID: 3761
		private IntPtr m_OfferId;
	}
}
