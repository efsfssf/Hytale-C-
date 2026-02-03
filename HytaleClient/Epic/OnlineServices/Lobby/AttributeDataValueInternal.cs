using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000378 RID: 888
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	internal struct AttributeDataValueInternal : IGettable<AttributeDataValue>, ISettable<AttributeDataValue>, IDisposable
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060017CD RID: 6093 RVA: 0x00022CAC File Offset: 0x00020EAC
		// (set) Token: 0x060017CE RID: 6094 RVA: 0x00022CD4 File Offset: 0x00020ED4
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

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00022CF8 File Offset: 0x00020EF8
		// (set) Token: 0x060017D0 RID: 6096 RVA: 0x00022D20 File Offset: 0x00020F20
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

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00022D44 File Offset: 0x00020F44
		// (set) Token: 0x060017D2 RID: 6098 RVA: 0x00022D6C File Offset: 0x00020F6C
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

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00022D90 File Offset: 0x00020F90
		// (set) Token: 0x060017D4 RID: 6100 RVA: 0x00022DB8 File Offset: 0x00020FB8
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

		// Token: 0x060017D5 RID: 6101 RVA: 0x00022DDA File Offset: 0x00020FDA
		public void Set(ref AttributeDataValue other)
		{
			this.AsInt64 = other.AsInt64;
			this.AsDouble = other.AsDouble;
			this.AsBool = other.AsBool;
			this.AsUtf8 = other.AsUtf8;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00022E14 File Offset: 0x00021014
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

		// Token: 0x060017D7 RID: 6103 RVA: 0x00022E82 File Offset: 0x00021082
		public void Dispose()
		{
			Helper.Dispose<AttributeType>(ref this.m_AsUtf8, this.m_ValueType, AttributeType.String);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00022E98 File Offset: 0x00021098
		public void Get(out AttributeDataValue output)
		{
			output = default(AttributeDataValue);
			output.Set(ref this);
		}

		// Token: 0x04000A7F RID: 2687
		[FieldOffset(0)]
		private long m_AsInt64;

		// Token: 0x04000A80 RID: 2688
		[FieldOffset(0)]
		private double m_AsDouble;

		// Token: 0x04000A81 RID: 2689
		[FieldOffset(0)]
		private int m_AsBool;

		// Token: 0x04000A82 RID: 2690
		[FieldOffset(0)]
		private IntPtr m_AsUtf8;

		// Token: 0x04000A83 RID: 2691
		[FieldOffset(8)]
		private AttributeType m_ValueType;
	}
}
