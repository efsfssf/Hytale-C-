using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000374 RID: 884
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AttributeInternal : IGettable<Attribute>, ISettable<Attribute>, IDisposable
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x00022854 File Offset: 0x00020A54
		// (set) Token: 0x060017A9 RID: 6057 RVA: 0x00022875 File Offset: 0x00020A75
		public AttributeData? Data
		{
			get
			{
				AttributeData? result;
				Helper.Get<AttributeDataInternal, AttributeData>(this.m_Data, out result);
				return result;
			}
			set
			{
				Helper.Set<AttributeData, AttributeDataInternal>(ref value, ref this.m_Data);
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x00022888 File Offset: 0x00020A88
		// (set) Token: 0x060017AB RID: 6059 RVA: 0x000228A0 File Offset: 0x00020AA0
		public LobbyAttributeVisibility Visibility
		{
			get
			{
				return this.m_Visibility;
			}
			set
			{
				this.m_Visibility = value;
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000228AA File Offset: 0x00020AAA
		public void Set(ref Attribute other)
		{
			this.m_ApiVersion = 1;
			this.Data = other.Data;
			this.Visibility = other.Visibility;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x000228D0 File Offset: 0x00020AD0
		public void Set(ref Attribute? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Data = other.Value.Data;
				this.Visibility = other.Value.Visibility;
			}
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0002291B File Offset: 0x00020B1B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x0002292A File Offset: 0x00020B2A
		public void Get(out Attribute output)
		{
			output = default(Attribute);
			output.Set(ref this);
		}

		// Token: 0x04000A72 RID: 2674
		private int m_ApiVersion;

		// Token: 0x04000A73 RID: 2675
		private IntPtr m_Data;

		// Token: 0x04000A74 RID: 2676
		private LobbyAttributeVisibility m_Visibility;
	}
}
