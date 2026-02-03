using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000EF RID: 239
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AttributeDataInternal : IGettable<AttributeData>, ISettable<AttributeData>, IDisposable
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0000BB70 File Offset: 0x00009D70
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x0000BB91 File Offset: 0x00009D91
		public Utf8String Key
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Key, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0000BBA4 File Offset: 0x00009DA4
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0000BBC5 File Offset: 0x00009DC5
		public AttributeDataValue Value
		{
			get
			{
				AttributeDataValue result;
				Helper.Get<AttributeDataValueInternal, AttributeDataValue>(ref this.m_Value, out result);
				return result;
			}
			set
			{
				Helper.Set<AttributeDataValue, AttributeDataValueInternal>(ref value, ref this.m_Value);
			}
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0000BBD6 File Offset: 0x00009DD6
		public void Set(ref AttributeData other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.Value = other.Value;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public void Set(ref AttributeData? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
				this.Value = other.Value.Value;
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0000BC47 File Offset: 0x00009E47
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
			Helper.Dispose<AttributeDataValueInternal>(ref this.m_Value);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0000BC62 File Offset: 0x00009E62
		public void Get(out AttributeData output)
		{
			output = default(AttributeData);
			output.Set(ref this);
		}

		// Token: 0x040003DE RID: 990
		private int m_ApiVersion;

		// Token: 0x040003DF RID: 991
		private IntPtr m_Key;

		// Token: 0x040003E0 RID: 992
		private AttributeDataValueInternal m_Value;
	}
}
