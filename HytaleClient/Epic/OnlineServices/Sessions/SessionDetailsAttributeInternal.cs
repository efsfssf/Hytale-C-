using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200014D RID: 333
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsAttributeInternal : IGettable<SessionDetailsAttribute>, ISettable<SessionDetailsAttribute>, IDisposable
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0000E16C File Offset: 0x0000C36C
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0000E18D File Offset: 0x0000C38D
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

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0000E1B8 File Offset: 0x0000C3B8
		public SessionAttributeAdvertisementType AdvertisementType
		{
			get
			{
				return this.m_AdvertisementType;
			}
			set
			{
				this.m_AdvertisementType = value;
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000E1C2 File Offset: 0x0000C3C2
		public void Set(ref SessionDetailsAttribute other)
		{
			this.m_ApiVersion = 1;
			this.Data = other.Data;
			this.AdvertisementType = other.AdvertisementType;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		public void Set(ref SessionDetailsAttribute? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Data = other.Value.Data;
				this.AdvertisementType = other.Value.AdvertisementType;
			}
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000E233 File Offset: 0x0000C433
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Data);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0000E242 File Offset: 0x0000C442
		public void Get(out SessionDetailsAttribute output)
		{
			output = default(SessionDetailsAttribute);
			output.Set(ref this);
		}

		// Token: 0x04000495 RID: 1173
		private int m_ApiVersion;

		// Token: 0x04000496 RID: 1174
		private IntPtr m_Data;

		// Token: 0x04000497 RID: 1175
		private SessionAttributeAdvertisementType m_AdvertisementType;
	}
}
