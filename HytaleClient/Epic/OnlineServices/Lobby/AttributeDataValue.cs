using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000377 RID: 887
	public struct AttributeDataValue
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x00022A80 File Offset: 0x00020C80
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x00022AA8 File Offset: 0x00020CA8
		public long? AsInt64
		{
			get
			{
				long? result;
				Helper.Get<long?, AttributeType>(this.m_AsInt64, out result, this.m_ValueType, AttributeType.Int64);
				return result;
			}
			set
			{
				Helper.Set<long?, AttributeType>(value, ref this.m_AsInt64, AttributeType.Int64, ref this.m_ValueType, null);
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x00022AC0 File Offset: 0x00020CC0
		// (set) Token: 0x060017C0 RID: 6080 RVA: 0x00022AE8 File Offset: 0x00020CE8
		public double? AsDouble
		{
			get
			{
				double? result;
				Helper.Get<double?, AttributeType>(this.m_AsDouble, out result, this.m_ValueType, AttributeType.Double);
				return result;
			}
			set
			{
				Helper.Set<double?, AttributeType>(value, ref this.m_AsDouble, AttributeType.Double, ref this.m_ValueType, null);
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00022B00 File Offset: 0x00020D00
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x00022B28 File Offset: 0x00020D28
		public bool? AsBool
		{
			get
			{
				bool? result;
				Helper.Get<bool?, AttributeType>(this.m_AsBool, out result, this.m_ValueType, AttributeType.Boolean);
				return result;
			}
			set
			{
				Helper.Set<bool?, AttributeType>(value, ref this.m_AsBool, AttributeType.Boolean, ref this.m_ValueType, null);
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00022B40 File Offset: 0x00020D40
		// (set) Token: 0x060017C4 RID: 6084 RVA: 0x00022B68 File Offset: 0x00020D68
		public Utf8String AsUtf8
		{
			get
			{
				Utf8String result;
				Helper.Get<Utf8String, AttributeType>(this.m_AsUtf8, out result, this.m_ValueType, AttributeType.String);
				return result;
			}
			set
			{
				Helper.Set<Utf8String, AttributeType>(value, ref this.m_AsUtf8, AttributeType.String, ref this.m_ValueType, null);
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x00022B80 File Offset: 0x00020D80
		// (set) Token: 0x060017C6 RID: 6086 RVA: 0x00022B98 File Offset: 0x00020D98
		public AttributeType ValueType
		{
			get
			{
				return this.m_ValueType;
			}
			private set
			{
				this.m_ValueType = value;
			}
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00022BA4 File Offset: 0x00020DA4
		public static implicit operator AttributeDataValue(long value)
		{
			return new AttributeDataValue
			{
				AsInt64 = new long?(value)
			};
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00022BD0 File Offset: 0x00020DD0
		public static implicit operator AttributeDataValue(double value)
		{
			return new AttributeDataValue
			{
				AsDouble = new double?(value)
			};
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00022BFC File Offset: 0x00020DFC
		public static implicit operator AttributeDataValue(bool value)
		{
			return new AttributeDataValue
			{
				AsBool = new bool?(value)
			};
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00022C28 File Offset: 0x00020E28
		public static implicit operator AttributeDataValue(Utf8String value)
		{
			return new AttributeDataValue
			{
				AsUtf8 = value
			};
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00022C4C File Offset: 0x00020E4C
		public static implicit operator AttributeDataValue(string value)
		{
			return new AttributeDataValue
			{
				AsUtf8 = value
			};
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00022C75 File Offset: 0x00020E75
		internal void Set(ref AttributeDataValueInternal other)
		{
			this.AsInt64 = other.AsInt64;
			this.AsDouble = other.AsDouble;
			this.AsBool = other.AsBool;
			this.AsUtf8 = other.AsUtf8;
		}

		// Token: 0x04000A7A RID: 2682
		private long? m_AsInt64;

		// Token: 0x04000A7B RID: 2683
		private double? m_AsDouble;

		// Token: 0x04000A7C RID: 2684
		private bool? m_AsBool;

		// Token: 0x04000A7D RID: 2685
		private Utf8String m_AsUtf8;

		// Token: 0x04000A7E RID: 2686
		private AttributeType m_ValueType;
	}
}
