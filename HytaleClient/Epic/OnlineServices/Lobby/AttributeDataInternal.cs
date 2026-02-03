using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000376 RID: 886
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AttributeDataInternal : IGettable<AttributeData>, ISettable<AttributeData>, IDisposable
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x0002297C File Offset: 0x00020B7C
		// (set) Token: 0x060017B6 RID: 6070 RVA: 0x0002299D File Offset: 0x00020B9D
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

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x000229B0 File Offset: 0x00020BB0
		// (set) Token: 0x060017B8 RID: 6072 RVA: 0x000229D1 File Offset: 0x00020BD1
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

		// Token: 0x060017B9 RID: 6073 RVA: 0x000229E2 File Offset: 0x00020BE2
		public void Set(ref AttributeData other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
			this.Value = other.Value;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00022A08 File Offset: 0x00020C08
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

		// Token: 0x060017BB RID: 6075 RVA: 0x00022A53 File Offset: 0x00020C53
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
			Helper.Dispose<AttributeDataValueInternal>(ref this.m_Value);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00022A6E File Offset: 0x00020C6E
		public void Get(out AttributeData output)
		{
			output = default(AttributeData);
			output.Set(ref this);
		}

		// Token: 0x04000A77 RID: 2679
		private int m_ApiVersion;

		// Token: 0x04000A78 RID: 2680
		private IntPtr m_Key;

		// Token: 0x04000A79 RID: 2681
		private AttributeDataValueInternal m_Value;
	}
}
