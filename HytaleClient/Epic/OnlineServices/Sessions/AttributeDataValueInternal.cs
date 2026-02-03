using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000F1 RID: 241
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	internal struct AttributeDataValueInternal : IGettable<AttributeDataValue>, ISettable<AttributeDataValue>, IDisposable
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0000BEA0 File Offset: 0x0000A0A0
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
		public long? AsInt64
		{
			get
			{
				long? result;
				Helper.Get<long, AttributeType>(this.m_AsInt64, out result, this.m_ValueType, AttributeType.Int64);
				return result;
			}
			set
			{
				Helper.Set<long, AttributeType>(value, ref this.m_AsInt64, AttributeType.Int64, ref this.m_ValueType, this);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0000BEEC File Offset: 0x0000A0EC
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0000BF14 File Offset: 0x0000A114
		public double? AsDouble
		{
			get
			{
				double? result;
				Helper.Get<double, AttributeType>(this.m_AsDouble, out result, this.m_ValueType, AttributeType.Double);
				return result;
			}
			set
			{
				Helper.Set<double, AttributeType>(value, ref this.m_AsDouble, AttributeType.Double, ref this.m_ValueType, this);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0000BF38 File Offset: 0x0000A138
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0000BF60 File Offset: 0x0000A160
		public bool? AsBool
		{
			get
			{
				bool? result;
				Helper.Get<AttributeType>(this.m_AsBool, out result, this.m_ValueType, AttributeType.Boolean);
				return result;
			}
			set
			{
				Helper.Set<AttributeType>(value, ref this.m_AsBool, AttributeType.Boolean, ref this.m_ValueType, this);
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0000BF84 File Offset: 0x0000A184
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0000BFAC File Offset: 0x0000A1AC
		public Utf8String AsUtf8
		{
			get
			{
				Utf8String result;
				Helper.Get<AttributeType>(this.m_AsUtf8, out result, this.m_ValueType, AttributeType.String);
				return result;
			}
			set
			{
				Helper.Set<AttributeType>(value, ref this.m_AsUtf8, AttributeType.String, ref this.m_ValueType, this);
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0000BFCE File Offset: 0x0000A1CE
		public void Set(ref AttributeDataValue other)
		{
			this.AsInt64 = other.AsInt64;
			this.AsDouble = other.AsDouble;
			this.AsBool = other.AsBool;
			this.AsUtf8 = other.AsUtf8;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0000C008 File Offset: 0x0000A208
		public void Set(ref AttributeDataValue? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.AsInt64 = other.Value.AsInt64;
				this.AsDouble = other.Value.AsDouble;
				this.AsBool = other.Value.AsBool;
				this.AsUtf8 = other.Value.AsUtf8;
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0000C076 File Offset: 0x0000A276
		public void Dispose()
		{
			Helper.Dispose<AttributeType>(ref this.m_AsUtf8, this.m_ValueType, AttributeType.String);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0000C08C File Offset: 0x0000A28C
		public void Get(out AttributeDataValue output)
		{
			output = default(AttributeDataValue);
			output.Set(ref this);
		}

		// Token: 0x040003E6 RID: 998
		[FieldOffset(0)]
		private long m_AsInt64;

		// Token: 0x040003E7 RID: 999
		[FieldOffset(0)]
		private double m_AsDouble;

		// Token: 0x040003E8 RID: 1000
		[FieldOffset(0)]
		private int m_AsBool;

		// Token: 0x040003E9 RID: 1001
		[FieldOffset(0)]
		private IntPtr m_AsUtf8;

		// Token: 0x040003EA RID: 1002
		[FieldOffset(8)]
		private AttributeType m_ValueType;
	}
}
