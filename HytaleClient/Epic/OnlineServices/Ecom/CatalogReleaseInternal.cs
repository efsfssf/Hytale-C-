using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000504 RID: 1284
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CatalogReleaseInternal : IGettable<CatalogRelease>, ISettable<CatalogRelease>, IDisposable
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x00031850 File Offset: 0x0002FA50
		// (set) Token: 0x060021BF RID: 8639 RVA: 0x00031878 File Offset: 0x0002FA78
		public Utf8String[] CompatibleAppIds
		{
			get
			{
				Utf8String[] result;
				Helper.Get<Utf8String>(this.m_CompatibleAppIds, out result, this.m_CompatibleAppIdCount, true);
				return result;
			}
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_CompatibleAppIds, true, out this.m_CompatibleAppIdCount);
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x00031890 File Offset: 0x0002FA90
		// (set) Token: 0x060021C1 RID: 8641 RVA: 0x000318B8 File Offset: 0x0002FAB8
		public Utf8String[] CompatiblePlatforms
		{
			get
			{
				Utf8String[] result;
				Helper.Get<Utf8String>(this.m_CompatiblePlatforms, out result, this.m_CompatiblePlatformCount, true);
				return result;
			}
			set
			{
				Helper.Set<Utf8String>(value, ref this.m_CompatiblePlatforms, true, out this.m_CompatiblePlatformCount);
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x000318D0 File Offset: 0x0002FAD0
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x000318F1 File Offset: 0x0002FAF1
		public Utf8String ReleaseNote
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ReleaseNote, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ReleaseNote);
			}
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x00031901 File Offset: 0x0002FB01
		public void Set(ref CatalogRelease other)
		{
			this.m_ApiVersion = 1;
			this.CompatibleAppIds = other.CompatibleAppIds;
			this.CompatiblePlatforms = other.CompatiblePlatforms;
			this.ReleaseNote = other.ReleaseNote;
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00031934 File Offset: 0x0002FB34
		public void Set(ref CatalogRelease? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.CompatibleAppIds = other.Value.CompatibleAppIds;
				this.CompatiblePlatforms = other.Value.CompatiblePlatforms;
				this.ReleaseNote = other.Value.ReleaseNote;
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00031994 File Offset: 0x0002FB94
		public void Dispose()
		{
			Helper.Dispose(ref this.m_CompatibleAppIds);
			Helper.Dispose(ref this.m_CompatiblePlatforms);
			Helper.Dispose(ref this.m_ReleaseNote);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000319BB File Offset: 0x0002FBBB
		public void Get(out CatalogRelease output)
		{
			output = default(CatalogRelease);
			output.Set(ref this);
		}

		// Token: 0x04000EA1 RID: 3745
		private int m_ApiVersion;

		// Token: 0x04000EA2 RID: 3746
		private uint m_CompatibleAppIdCount;

		// Token: 0x04000EA3 RID: 3747
		private IntPtr m_CompatibleAppIds;

		// Token: 0x04000EA4 RID: 3748
		private uint m_CompatiblePlatformCount;

		// Token: 0x04000EA5 RID: 3749
		private IntPtr m_CompatiblePlatforms;

		// Token: 0x04000EA6 RID: 3750
		private IntPtr m_ReleaseNote;
	}
}
