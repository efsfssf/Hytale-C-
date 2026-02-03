using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000339 RID: 825
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ModIdentifierInternal : IGettable<ModIdentifier>, ISettable<ModIdentifier>, IDisposable
	{
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600169C RID: 5788 RVA: 0x00020FB8 File Offset: 0x0001F1B8
		// (set) Token: 0x0600169D RID: 5789 RVA: 0x00020FD9 File Offset: 0x0001F1D9
		public Utf8String NamespaceId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_NamespaceId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_NamespaceId);
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x00020FEC File Offset: 0x0001F1EC
		// (set) Token: 0x0600169F RID: 5791 RVA: 0x0002100D File Offset: 0x0001F20D
		public Utf8String ItemId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ItemId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ItemId);
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x00021020 File Offset: 0x0001F220
		// (set) Token: 0x060016A1 RID: 5793 RVA: 0x00021041 File Offset: 0x0001F241
		public Utf8String ArtifactId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ArtifactId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ArtifactId);
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00021054 File Offset: 0x0001F254
		// (set) Token: 0x060016A3 RID: 5795 RVA: 0x00021075 File Offset: 0x0001F275
		public Utf8String Title
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Title, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Title);
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00021088 File Offset: 0x0001F288
		// (set) Token: 0x060016A5 RID: 5797 RVA: 0x000210A9 File Offset: 0x0001F2A9
		public Utf8String Version
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Version, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Version);
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000210BC File Offset: 0x0001F2BC
		public void Set(ref ModIdentifier other)
		{
			this.m_ApiVersion = 1;
			this.NamespaceId = other.NamespaceId;
			this.ItemId = other.ItemId;
			this.ArtifactId = other.ArtifactId;
			this.Title = other.Title;
			this.Version = other.Version;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00021114 File Offset: 0x0001F314
		public void Set(ref ModIdentifier? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.NamespaceId = other.Value.NamespaceId;
				this.ItemId = other.Value.ItemId;
				this.ArtifactId = other.Value.ArtifactId;
				this.Title = other.Value.Title;
				this.Version = other.Value.Version;
			}
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0002119E File Offset: 0x0001F39E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_NamespaceId);
			Helper.Dispose(ref this.m_ItemId);
			Helper.Dispose(ref this.m_ArtifactId);
			Helper.Dispose(ref this.m_Title);
			Helper.Dispose(ref this.m_Version);
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x000211DD File Offset: 0x0001F3DD
		public void Get(out ModIdentifier output)
		{
			output = default(ModIdentifier);
			output.Set(ref this);
		}

		// Token: 0x040009E1 RID: 2529
		private int m_ApiVersion;

		// Token: 0x040009E2 RID: 2530
		private IntPtr m_NamespaceId;

		// Token: 0x040009E3 RID: 2531
		private IntPtr m_ItemId;

		// Token: 0x040009E4 RID: 2532
		private IntPtr m_ArtifactId;

		// Token: 0x040009E5 RID: 2533
		private IntPtr m_Title;

		// Token: 0x040009E6 RID: 2534
		private IntPtr m_Version;
	}
}
