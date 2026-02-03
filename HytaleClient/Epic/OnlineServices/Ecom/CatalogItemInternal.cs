using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x02000500 RID: 1280
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CatalogItemInternal : IGettable<CatalogItem>, ISettable<CatalogItem>, IDisposable
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x00030A80 File Offset: 0x0002EC80
		// (set) Token: 0x06002147 RID: 8519 RVA: 0x00030AA1 File Offset: 0x0002ECA1
		public Utf8String CatalogNamespace
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CatalogNamespace, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CatalogNamespace);
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x00030AB4 File Offset: 0x0002ECB4
		// (set) Token: 0x06002149 RID: 8521 RVA: 0x00030AD5 File Offset: 0x0002ECD5
		public Utf8String Id
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Id, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Id);
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x00030AE8 File Offset: 0x0002ECE8
		// (set) Token: 0x0600214B RID: 8523 RVA: 0x00030B09 File Offset: 0x0002ED09
		public Utf8String EntitlementName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_EntitlementName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_EntitlementName);
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x00030B1C File Offset: 0x0002ED1C
		// (set) Token: 0x0600214D RID: 8525 RVA: 0x00030B3D File Offset: 0x0002ED3D
		public Utf8String TitleText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TitleText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TitleText);
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x00030B50 File Offset: 0x0002ED50
		// (set) Token: 0x0600214F RID: 8527 RVA: 0x00030B71 File Offset: 0x0002ED71
		public Utf8String DescriptionText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DescriptionText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DescriptionText);
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x00030B84 File Offset: 0x0002ED84
		// (set) Token: 0x06002151 RID: 8529 RVA: 0x00030BA5 File Offset: 0x0002EDA5
		public Utf8String LongDescriptionText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_LongDescriptionText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LongDescriptionText);
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x00030BB8 File Offset: 0x0002EDB8
		// (set) Token: 0x06002153 RID: 8531 RVA: 0x00030BD9 File Offset: 0x0002EDD9
		public Utf8String TechnicalDetailsText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TechnicalDetailsText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TechnicalDetailsText);
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x00030BEC File Offset: 0x0002EDEC
		// (set) Token: 0x06002155 RID: 8533 RVA: 0x00030C0D File Offset: 0x0002EE0D
		public Utf8String DeveloperText
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DeveloperText, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DeveloperText);
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x00030C20 File Offset: 0x0002EE20
		// (set) Token: 0x06002157 RID: 8535 RVA: 0x00030C38 File Offset: 0x0002EE38
		public EcomItemType ItemType
		{
			get
			{
				return this.m_ItemType;
			}
			set
			{
				this.m_ItemType = value;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x00030C44 File Offset: 0x0002EE44
		// (set) Token: 0x06002159 RID: 8537 RVA: 0x00030C5C File Offset: 0x0002EE5C
		public long EntitlementEndTimestamp
		{
			get
			{
				return this.m_EntitlementEndTimestamp;
			}
			set
			{
				this.m_EntitlementEndTimestamp = value;
			}
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00030C68 File Offset: 0x0002EE68
		public void Set(ref CatalogItem other)
		{
			this.m_ApiVersion = 1;
			this.CatalogNamespace = other.CatalogNamespace;
			this.Id = other.Id;
			this.EntitlementName = other.EntitlementName;
			this.TitleText = other.TitleText;
			this.DescriptionText = other.DescriptionText;
			this.LongDescriptionText = other.LongDescriptionText;
			this.TechnicalDetailsText = other.TechnicalDetailsText;
			this.DeveloperText = other.DeveloperText;
			this.ItemType = other.ItemType;
			this.EntitlementEndTimestamp = other.EntitlementEndTimestamp;
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00030D00 File Offset: 0x0002EF00
		public void Set(ref CatalogItem? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.CatalogNamespace = other.Value.CatalogNamespace;
				this.Id = other.Value.Id;
				this.EntitlementName = other.Value.EntitlementName;
				this.TitleText = other.Value.TitleText;
				this.DescriptionText = other.Value.DescriptionText;
				this.LongDescriptionText = other.Value.LongDescriptionText;
				this.TechnicalDetailsText = other.Value.TechnicalDetailsText;
				this.DeveloperText = other.Value.DeveloperText;
				this.ItemType = other.Value.ItemType;
				this.EntitlementEndTimestamp = other.Value.EntitlementEndTimestamp;
			}
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00030DF8 File Offset: 0x0002EFF8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_CatalogNamespace);
			Helper.Dispose(ref this.m_Id);
			Helper.Dispose(ref this.m_EntitlementName);
			Helper.Dispose(ref this.m_TitleText);
			Helper.Dispose(ref this.m_DescriptionText);
			Helper.Dispose(ref this.m_LongDescriptionText);
			Helper.Dispose(ref this.m_TechnicalDetailsText);
			Helper.Dispose(ref this.m_DeveloperText);
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00030E66 File Offset: 0x0002F066
		public void Get(out CatalogItem output)
		{
			output = default(CatalogItem);
			output.Set(ref this);
		}

		// Token: 0x04000E68 RID: 3688
		private int m_ApiVersion;

		// Token: 0x04000E69 RID: 3689
		private IntPtr m_CatalogNamespace;

		// Token: 0x04000E6A RID: 3690
		private IntPtr m_Id;

		// Token: 0x04000E6B RID: 3691
		private IntPtr m_EntitlementName;

		// Token: 0x04000E6C RID: 3692
		private IntPtr m_TitleText;

		// Token: 0x04000E6D RID: 3693
		private IntPtr m_DescriptionText;

		// Token: 0x04000E6E RID: 3694
		private IntPtr m_LongDescriptionText;

		// Token: 0x04000E6F RID: 3695
		private IntPtr m_TechnicalDetailsText;

		// Token: 0x04000E70 RID: 3696
		private IntPtr m_DeveloperText;

		// Token: 0x04000E71 RID: 3697
		private EcomItemType m_ItemType;

		// Token: 0x04000E72 RID: 3698
		private long m_EntitlementEndTimestamp;
	}
}
