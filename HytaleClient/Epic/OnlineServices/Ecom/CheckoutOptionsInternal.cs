using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200050A RID: 1290
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CheckoutOptionsInternal : ISettable<CheckoutOptions>, IDisposable
	{
		// Token: 0x170009C5 RID: 2501
		// (set) Token: 0x060021F0 RID: 8688 RVA: 0x00031D1F File Offset: 0x0002FF1F
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x00031D2F File Offset: 0x0002FF2F
		public Utf8String OverrideCatalogNamespace
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideCatalogNamespace);
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (set) Token: 0x060021F2 RID: 8690 RVA: 0x00031D3F File Offset: 0x0002FF3F
		public CheckoutEntry[] Entries
		{
			set
			{
				Helper.Set<CheckoutEntry, CheckoutEntryInternal>(ref value, ref this.m_Entries, out this.m_EntryCount);
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x00031D56 File Offset: 0x0002FF56
		public CheckoutOrientation PreferredOrientation
		{
			set
			{
				this.m_PreferredOrientation = value;
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00031D60 File Offset: 0x0002FF60
		public void Set(ref CheckoutOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.OverrideCatalogNamespace = other.OverrideCatalogNamespace;
			this.Entries = other.Entries;
			this.PreferredOrientation = other.PreferredOrientation;
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00031DA0 File Offset: 0x0002FFA0
		public void Set(ref CheckoutOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.OverrideCatalogNamespace = other.Value.OverrideCatalogNamespace;
				this.Entries = other.Value.Entries;
				this.PreferredOrientation = other.Value.PreferredOrientation;
			}
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00031E15 File Offset: 0x00030015
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_OverrideCatalogNamespace);
			Helper.Dispose(ref this.m_Entries);
		}

		// Token: 0x04000EB6 RID: 3766
		private int m_ApiVersion;

		// Token: 0x04000EB7 RID: 3767
		private IntPtr m_LocalUserId;

		// Token: 0x04000EB8 RID: 3768
		private IntPtr m_OverrideCatalogNamespace;

		// Token: 0x04000EB9 RID: 3769
		private uint m_EntryCount;

		// Token: 0x04000EBA RID: 3770
		private IntPtr m_Entries;

		// Token: 0x04000EBB RID: 3771
		private CheckoutOrientation m_PreferredOrientation;
	}
}
