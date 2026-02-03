using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006A7 RID: 1703
	public struct LogEventParamPairParamValue
	{
		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x00040700 File Offset: 0x0003E900
		// (set) Token: 0x06002BC6 RID: 11206 RVA: 0x00040718 File Offset: 0x0003E918
		public AntiCheatCommonEventParamType ParamValueType
		{
			get
			{
				return this.m_ParamValueType;
			}
			private set
			{
				this.m_ParamValueType = value;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x00040724 File Offset: 0x0003E924
		// (set) Token: 0x06002BC8 RID: 11208 RVA: 0x0004074C File Offset: 0x0003E94C
		public IntPtr? ClientHandle
		{
			get
			{
				IntPtr? result;
				Helper.Get<IntPtr?, AntiCheatCommonEventParamType>(this.m_ClientHandle, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.ClientHandle);
				return result;
			}
			set
			{
				Helper.Set<IntPtr?, AntiCheatCommonEventParamType>(value, ref this.m_ClientHandle, AntiCheatCommonEventParamType.ClientHandle, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x00040764 File Offset: 0x0003E964
		// (set) Token: 0x06002BCA RID: 11210 RVA: 0x0004078C File Offset: 0x0003E98C
		public Utf8String String
		{
			get
			{
				Utf8String result;
				Helper.Get<Utf8String, AntiCheatCommonEventParamType>(this.m_String, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.String);
				return result;
			}
			set
			{
				Helper.Set<Utf8String, AntiCheatCommonEventParamType>(value, ref this.m_String, AntiCheatCommonEventParamType.String, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000407A4 File Offset: 0x0003E9A4
		// (set) Token: 0x06002BCC RID: 11212 RVA: 0x000407CC File Offset: 0x0003E9CC
		public uint? UInt32
		{
			get
			{
				uint? result;
				Helper.Get<uint?, AntiCheatCommonEventParamType>(this.m_UInt32, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.UInt32);
				return result;
			}
			set
			{
				Helper.Set<uint?, AntiCheatCommonEventParamType>(value, ref this.m_UInt32, AntiCheatCommonEventParamType.UInt32, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000407E4 File Offset: 0x0003E9E4
		// (set) Token: 0x06002BCE RID: 11214 RVA: 0x0004080C File Offset: 0x0003EA0C
		public int? Int32
		{
			get
			{
				int? result;
				Helper.Get<int?, AntiCheatCommonEventParamType>(this.m_Int32, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Int32);
				return result;
			}
			set
			{
				Helper.Set<int?, AntiCheatCommonEventParamType>(value, ref this.m_Int32, AntiCheatCommonEventParamType.Int32, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x00040824 File Offset: 0x0003EA24
		// (set) Token: 0x06002BD0 RID: 11216 RVA: 0x0004084C File Offset: 0x0003EA4C
		public ulong? UInt64
		{
			get
			{
				ulong? result;
				Helper.Get<ulong?, AntiCheatCommonEventParamType>(this.m_UInt64, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.UInt64);
				return result;
			}
			set
			{
				Helper.Set<ulong?, AntiCheatCommonEventParamType>(value, ref this.m_UInt64, AntiCheatCommonEventParamType.UInt64, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x00040864 File Offset: 0x0003EA64
		// (set) Token: 0x06002BD2 RID: 11218 RVA: 0x0004088C File Offset: 0x0003EA8C
		public long? Int64
		{
			get
			{
				long? result;
				Helper.Get<long?, AntiCheatCommonEventParamType>(this.m_Int64, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Int64);
				return result;
			}
			set
			{
				Helper.Set<long?, AntiCheatCommonEventParamType>(value, ref this.m_Int64, AntiCheatCommonEventParamType.Int64, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x000408A4 File Offset: 0x0003EAA4
		// (set) Token: 0x06002BD4 RID: 11220 RVA: 0x000408CC File Offset: 0x0003EACC
		public Vec3f Vec3f
		{
			get
			{
				Vec3f result;
				Helper.Get<Vec3f, AntiCheatCommonEventParamType>(this.m_Vec3f, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Vector3f);
				return result;
			}
			set
			{
				Helper.Set<Vec3f, AntiCheatCommonEventParamType>(value, ref this.m_Vec3f, AntiCheatCommonEventParamType.Vector3f, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x000408E4 File Offset: 0x0003EAE4
		// (set) Token: 0x06002BD6 RID: 11222 RVA: 0x0004090C File Offset: 0x0003EB0C
		public Quat Quat
		{
			get
			{
				Quat result;
				Helper.Get<Quat, AntiCheatCommonEventParamType>(this.m_Quat, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Quat);
				return result;
			}
			set
			{
				Helper.Set<Quat, AntiCheatCommonEventParamType>(value, ref this.m_Quat, AntiCheatCommonEventParamType.Quat, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x00040924 File Offset: 0x0003EB24
		// (set) Token: 0x06002BD8 RID: 11224 RVA: 0x0004094D File Offset: 0x0003EB4D
		public float? Float
		{
			get
			{
				float? result;
				Helper.Get<float?, AntiCheatCommonEventParamType>(this.m_Float, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Float);
				return result;
			}
			set
			{
				Helper.Set<float?, AntiCheatCommonEventParamType>(value, ref this.m_Float, AntiCheatCommonEventParamType.Float, ref this.m_ParamValueType, null);
			}
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x00040968 File Offset: 0x0003EB68
		public static implicit operator LogEventParamPairParamValue(IntPtr value)
		{
			return new LogEventParamPairParamValue
			{
				ClientHandle = new IntPtr?(value)
			};
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00040994 File Offset: 0x0003EB94
		public static implicit operator LogEventParamPairParamValue(Utf8String value)
		{
			return new LogEventParamPairParamValue
			{
				String = value
			};
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000409B8 File Offset: 0x0003EBB8
		public static implicit operator LogEventParamPairParamValue(string value)
		{
			return new LogEventParamPairParamValue
			{
				String = value
			};
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000409E4 File Offset: 0x0003EBE4
		public static implicit operator LogEventParamPairParamValue(uint value)
		{
			return new LogEventParamPairParamValue
			{
				UInt32 = new uint?(value)
			};
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00040A10 File Offset: 0x0003EC10
		public static implicit operator LogEventParamPairParamValue(int value)
		{
			return new LogEventParamPairParamValue
			{
				Int32 = new int?(value)
			};
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x00040A3C File Offset: 0x0003EC3C
		public static implicit operator LogEventParamPairParamValue(ulong value)
		{
			return new LogEventParamPairParamValue
			{
				UInt64 = new ulong?(value)
			};
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00040A68 File Offset: 0x0003EC68
		public static implicit operator LogEventParamPairParamValue(long value)
		{
			return new LogEventParamPairParamValue
			{
				Int64 = new long?(value)
			};
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x00040A94 File Offset: 0x0003EC94
		public static implicit operator LogEventParamPairParamValue(Vec3f value)
		{
			return new LogEventParamPairParamValue
			{
				Vec3f = value
			};
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x00040AB8 File Offset: 0x0003ECB8
		public static implicit operator LogEventParamPairParamValue(Quat value)
		{
			return new LogEventParamPairParamValue
			{
				Quat = value
			};
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x00040ADC File Offset: 0x0003ECDC
		public static implicit operator LogEventParamPairParamValue(float value)
		{
			return new LogEventParamPairParamValue
			{
				Float = new float?(value)
			};
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00040B08 File Offset: 0x0003ED08
		internal void Set(ref LogEventParamPairParamValueInternal other)
		{
			this.ClientHandle = other.ClientHandle;
			this.String = other.String;
			this.UInt32 = other.UInt32;
			this.Int32 = other.Int32;
			this.UInt64 = other.UInt64;
			this.Int64 = other.Int64;
			this.Vec3f = other.Vec3f;
			this.Quat = other.Quat;
			this.Float = other.Float;
		}

		// Token: 0x04001331 RID: 4913
		private AntiCheatCommonEventParamType m_ParamValueType;

		// Token: 0x04001332 RID: 4914
		private IntPtr? m_ClientHandle;

		// Token: 0x04001333 RID: 4915
		private Utf8String m_String;

		// Token: 0x04001334 RID: 4916
		private uint? m_UInt32;

		// Token: 0x04001335 RID: 4917
		private int? m_Int32;

		// Token: 0x04001336 RID: 4918
		private ulong? m_UInt64;

		// Token: 0x04001337 RID: 4919
		private long? m_Int64;

		// Token: 0x04001338 RID: 4920
		private Vec3f m_Vec3f;

		// Token: 0x04001339 RID: 4921
		private Quat m_Quat;

		// Token: 0x0400133A RID: 4922
		private float? m_Float;
	}
}
