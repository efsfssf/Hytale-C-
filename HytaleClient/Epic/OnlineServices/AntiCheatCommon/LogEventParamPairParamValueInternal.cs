using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006A8 RID: 1704
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	internal struct LogEventParamPairParamValueInternal : IGettable<LogEventParamPairParamValue>, ISettable<LogEventParamPairParamValue>, IDisposable
	{
		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x00040B8C File Offset: 0x0003ED8C
		// (set) Token: 0x06002BE5 RID: 11237 RVA: 0x00040BB4 File Offset: 0x0003EDB4
		public IntPtr? ClientHandle
		{
			get
			{
				IntPtr? result;
				Helper.Get<AntiCheatCommonEventParamType>(this.m_ClientHandle, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.ClientHandle);
				return result;
			}
			set
			{
				Helper.Set<IntPtr, AntiCheatCommonEventParamType>(value, ref this.m_ClientHandle, AntiCheatCommonEventParamType.ClientHandle, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06002BE6 RID: 11238 RVA: 0x00040BD8 File Offset: 0x0003EDD8
		// (set) Token: 0x06002BE7 RID: 11239 RVA: 0x00040C00 File Offset: 0x0003EE00
		public Utf8String String
		{
			get
			{
				Utf8String result;
				Helper.Get<AntiCheatCommonEventParamType>(this.m_String, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.String);
				return result;
			}
			set
			{
				Helper.Set<AntiCheatCommonEventParamType>(value, ref this.m_String, AntiCheatCommonEventParamType.String, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x00040C24 File Offset: 0x0003EE24
		// (set) Token: 0x06002BE9 RID: 11241 RVA: 0x00040C4C File Offset: 0x0003EE4C
		public uint? UInt32
		{
			get
			{
				uint? result;
				Helper.Get<uint, AntiCheatCommonEventParamType>(this.m_UInt32, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.UInt32);
				return result;
			}
			set
			{
				Helper.Set<uint, AntiCheatCommonEventParamType>(value, ref this.m_UInt32, AntiCheatCommonEventParamType.UInt32, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06002BEA RID: 11242 RVA: 0x00040C70 File Offset: 0x0003EE70
		// (set) Token: 0x06002BEB RID: 11243 RVA: 0x00040C98 File Offset: 0x0003EE98
		public int? Int32
		{
			get
			{
				int? result;
				Helper.Get<int, AntiCheatCommonEventParamType>(this.m_Int32, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Int32);
				return result;
			}
			set
			{
				Helper.Set<int, AntiCheatCommonEventParamType>(value, ref this.m_Int32, AntiCheatCommonEventParamType.Int32, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06002BEC RID: 11244 RVA: 0x00040CBC File Offset: 0x0003EEBC
		// (set) Token: 0x06002BED RID: 11245 RVA: 0x00040CE4 File Offset: 0x0003EEE4
		public ulong? UInt64
		{
			get
			{
				ulong? result;
				Helper.Get<ulong, AntiCheatCommonEventParamType>(this.m_UInt64, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.UInt64);
				return result;
			}
			set
			{
				Helper.Set<ulong, AntiCheatCommonEventParamType>(value, ref this.m_UInt64, AntiCheatCommonEventParamType.UInt64, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06002BEE RID: 11246 RVA: 0x00040D08 File Offset: 0x0003EF08
		// (set) Token: 0x06002BEF RID: 11247 RVA: 0x00040D30 File Offset: 0x0003EF30
		public long? Int64
		{
			get
			{
				long? result;
				Helper.Get<long, AntiCheatCommonEventParamType>(this.m_Int64, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Int64);
				return result;
			}
			set
			{
				Helper.Set<long, AntiCheatCommonEventParamType>(value, ref this.m_Int64, AntiCheatCommonEventParamType.Int64, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x00040D54 File Offset: 0x0003EF54
		// (set) Token: 0x06002BF1 RID: 11249 RVA: 0x00040D7C File Offset: 0x0003EF7C
		public Vec3f Vec3f
		{
			get
			{
				Vec3f result;
				Helper.Get<Vec3fInternal, Vec3f, AntiCheatCommonEventParamType>(ref this.m_Vec3f, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Vector3f);
				return result;
			}
			set
			{
				Helper.Set<Vec3f, AntiCheatCommonEventParamType, Vec3fInternal>(ref value, ref this.m_Vec3f, AntiCheatCommonEventParamType.Vector3f, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x00040DA0 File Offset: 0x0003EFA0
		// (set) Token: 0x06002BF3 RID: 11251 RVA: 0x00040DC8 File Offset: 0x0003EFC8
		public Quat Quat
		{
			get
			{
				Quat result;
				Helper.Get<QuatInternal, Quat, AntiCheatCommonEventParamType>(ref this.m_Quat, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Quat);
				return result;
			}
			set
			{
				Helper.Set<Quat, AntiCheatCommonEventParamType, QuatInternal>(ref value, ref this.m_Quat, AntiCheatCommonEventParamType.Quat, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x00040DEC File Offset: 0x0003EFEC
		// (set) Token: 0x06002BF5 RID: 11253 RVA: 0x00040E15 File Offset: 0x0003F015
		public float? Float
		{
			get
			{
				float? result;
				Helper.Get<float, AntiCheatCommonEventParamType>(this.m_Float, out result, this.m_ParamValueType, AntiCheatCommonEventParamType.Float);
				return result;
			}
			set
			{
				Helper.Set<float, AntiCheatCommonEventParamType>(value, ref this.m_Float, AntiCheatCommonEventParamType.Float, ref this.m_ParamValueType, this);
			}
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00040E38 File Offset: 0x0003F038
		public void Set(ref LogEventParamPairParamValue other)
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

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00040EBC File Offset: 0x0003F0BC
		public void Set(ref LogEventParamPairParamValue? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientHandle = other.Value.ClientHandle;
				this.String = other.Value.String;
				this.UInt32 = other.Value.UInt32;
				this.Int32 = other.Value.Int32;
				this.UInt64 = other.Value.UInt64;
				this.Int64 = other.Value.Int64;
				this.Vec3f = other.Value.Vec3f;
				this.Quat = other.Value.Quat;
				this.Float = other.Value.Float;
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00040F98 File Offset: 0x0003F198
		public void Dispose()
		{
			Helper.Dispose<AntiCheatCommonEventParamType>(ref this.m_ClientHandle, this.m_ParamValueType, AntiCheatCommonEventParamType.ClientHandle);
			Helper.Dispose<AntiCheatCommonEventParamType>(ref this.m_String, this.m_ParamValueType, AntiCheatCommonEventParamType.String);
			Helper.Dispose<Vec3fInternal>(ref this.m_Vec3f);
			Helper.Dispose<QuatInternal>(ref this.m_Quat);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00040FE4 File Offset: 0x0003F1E4
		public void Get(out LogEventParamPairParamValue output)
		{
			output = default(LogEventParamPairParamValue);
			output.Set(ref this);
		}

		// Token: 0x0400133B RID: 4923
		[FieldOffset(0)]
		private AntiCheatCommonEventParamType m_ParamValueType;

		// Token: 0x0400133C RID: 4924
		[FieldOffset(8)]
		private IntPtr m_ClientHandle;

		// Token: 0x0400133D RID: 4925
		[FieldOffset(8)]
		private IntPtr m_String;

		// Token: 0x0400133E RID: 4926
		[FieldOffset(8)]
		private uint m_UInt32;

		// Token: 0x0400133F RID: 4927
		[FieldOffset(8)]
		private int m_Int32;

		// Token: 0x04001340 RID: 4928
		[FieldOffset(8)]
		private ulong m_UInt64;

		// Token: 0x04001341 RID: 4929
		[FieldOffset(8)]
		private long m_Int64;

		// Token: 0x04001342 RID: 4930
		[FieldOffset(8)]
		private Vec3fInternal m_Vec3f;

		// Token: 0x04001343 RID: 4931
		[FieldOffset(8)]
		private QuatInternal m_Quat;

		// Token: 0x04001344 RID: 4932
		[FieldOffset(8)]
		private float m_Float;
	}
}
