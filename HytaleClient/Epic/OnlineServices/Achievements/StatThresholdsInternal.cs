using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x0200075F RID: 1887
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct StatThresholdsInternal : IGettable<StatThresholds>, ISettable<StatThresholds>, IDisposable
	{
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x0600311B RID: 12571 RVA: 0x00048F18 File Offset: 0x00047118
		// (set) Token: 0x0600311C RID: 12572 RVA: 0x00048F39 File Offset: 0x00047139
		public Utf8String Name
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Name, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Name);
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x0600311D RID: 12573 RVA: 0x00048F4C File Offset: 0x0004714C
		// (set) Token: 0x0600311E RID: 12574 RVA: 0x00048F64 File Offset: 0x00047164
		public int Threshold
		{
			get
			{
				return this.m_Threshold;
			}
			set
			{
				this.m_Threshold = value;
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x00048F6E File Offset: 0x0004716E
		public void Set(ref StatThresholds other)
		{
			this.m_ApiVersion = 1;
			this.Name = other.Name;
			this.Threshold = other.Threshold;
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x00048F94 File Offset: 0x00047194
		public void Set(ref StatThresholds? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Name = other.Value.Name;
				this.Threshold = other.Value.Threshold;
			}
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x00048FDF File Offset: 0x000471DF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Name);
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x00048FEE File Offset: 0x000471EE
		public void Get(out StatThresholds output)
		{
			output = default(StatThresholds);
			output.Set(ref this);
		}

		// Token: 0x040015E6 RID: 5606
		private int m_ApiVersion;

		// Token: 0x040015E7 RID: 5607
		private IntPtr m_Name;

		// Token: 0x040015E8 RID: 5608
		private int m_Threshold;
	}
}
