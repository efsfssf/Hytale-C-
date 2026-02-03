using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000F0 RID: 240
	public struct AttributeDataValue
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0000BC74 File Offset: 0x00009E74
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0000BC9C File Offset: 0x00009E9C
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

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0000BCDC File Offset: 0x00009EDC
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

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0000BD1C File Offset: 0x00009F1C
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

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0000BD34 File Offset: 0x00009F34
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x0000BD5C File Offset: 0x00009F5C
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

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0000BD74 File Offset: 0x00009F74
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0000BD8C File Offset: 0x00009F8C
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

		// Token: 0x06000827 RID: 2087 RVA: 0x0000BD98 File Offset: 0x00009F98
		public static implicit operator AttributeDataValue(long value)
		{
			return new AttributeDataValue
			{
				AsInt64 = new long?(value)
			};
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0000BDC4 File Offset: 0x00009FC4
		public static implicit operator AttributeDataValue(double value)
		{
			return new AttributeDataValue
			{
				AsDouble = new double?(value)
			};
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		public static implicit operator AttributeDataValue(bool value)
		{
			return new AttributeDataValue
			{
				AsBool = new bool?(value)
			};
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0000BE1C File Offset: 0x0000A01C
		public static implicit operator AttributeDataValue(Utf8String value)
		{
			return new AttributeDataValue
			{
				AsUtf8 = value
			};
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000BE40 File Offset: 0x0000A040
		public static implicit operator AttributeDataValue(string value)
		{
			return new AttributeDataValue
			{
				AsUtf8 = value
			};
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0000BE69 File Offset: 0x0000A069
		internal void Set(ref AttributeDataValueInternal other)
		{
			this.AsInt64 = other.AsInt64;
			this.AsDouble = other.AsDouble;
			this.AsBool = other.AsBool;
			this.AsUtf8 = other.AsUtf8;
		}

		// Token: 0x040003E1 RID: 993
		private long? m_AsInt64;

		// Token: 0x040003E2 RID: 994
		private double? m_AsDouble;

		// Token: 0x040003E3 RID: 995
		private bool? m_AsBool;

		// Token: 0x040003E4 RID: 996
		private Utf8String m_AsUtf8;

		// Token: 0x040003E5 RID: 997
		private AttributeType m_ValueType;
	}
}
