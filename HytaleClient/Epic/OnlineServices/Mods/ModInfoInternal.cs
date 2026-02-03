using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x0200033B RID: 827
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ModInfoInternal : IGettable<ModInfo>, ISettable<ModInfo>, IDisposable
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x00021230 File Offset: 0x0001F430
		// (set) Token: 0x060016B0 RID: 5808 RVA: 0x00021257 File Offset: 0x0001F457
		public ModIdentifier[] Mods
		{
			get
			{
				ModIdentifier[] result;
				Helper.Get<ModIdentifierInternal, ModIdentifier>(this.m_Mods, out result, this.m_ModsCount);
				return result;
			}
			set
			{
				Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref this.m_Mods, out this.m_ModsCount);
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x00021270 File Offset: 0x0001F470
		// (set) Token: 0x060016B2 RID: 5810 RVA: 0x00021288 File Offset: 0x0001F488
		public ModEnumerationType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00021292 File Offset: 0x0001F492
		public void Set(ref ModInfo other)
		{
			this.m_ApiVersion = 1;
			this.Mods = other.Mods;
			this.Type = other.Type;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x000212B8 File Offset: 0x0001F4B8
		public void Set(ref ModInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Mods = other.Value.Mods;
				this.Type = other.Value.Type;
			}
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x00021303 File Offset: 0x0001F503
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Mods);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00021312 File Offset: 0x0001F512
		public void Get(out ModInfo output)
		{
			output = default(ModInfo);
			output.Set(ref this);
		}

		// Token: 0x040009E9 RID: 2537
		private int m_ApiVersion;

		// Token: 0x040009EA RID: 2538
		private int m_ModsCount;

		// Token: 0x040009EB RID: 2539
		private IntPtr m_Mods;

		// Token: 0x040009EC RID: 2540
		private ModEnumerationType m_Type;
	}
}
