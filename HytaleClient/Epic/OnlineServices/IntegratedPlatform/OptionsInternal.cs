using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004C4 RID: 1220
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OptionsInternal : IGettable<Options>, ISettable<Options>, IDisposable
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x0002E454 File Offset: 0x0002C654
		// (set) Token: 0x06001FA5 RID: 8101 RVA: 0x0002E475 File Offset: 0x0002C675
		public Utf8String Type
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Type, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Type);
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x0002E488 File Offset: 0x0002C688
		// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x0002E4A0 File Offset: 0x0002C6A0
		public IntegratedPlatformManagementFlags Flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0002E4AC File Offset: 0x0002C6AC
		// (set) Token: 0x06001FA9 RID: 8105 RVA: 0x0002E4C4 File Offset: 0x0002C6C4
		public IntPtr InitOptions
		{
			get
			{
				return this.m_InitOptions;
			}
			set
			{
				this.m_InitOptions = value;
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0002E4CE File Offset: 0x0002C6CE
		public void Set(ref Options other)
		{
			this.m_ApiVersion = 1;
			this.Type = other.Type;
			this.Flags = other.Flags;
			this.InitOptions = other.InitOptions;
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0002E500 File Offset: 0x0002C700
		public void Set(ref Options? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Type = other.Value.Type;
				this.Flags = other.Value.Flags;
				this.InitOptions = other.Value.InitOptions;
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0002E560 File Offset: 0x0002C760
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Type);
			Helper.Dispose(ref this.m_InitOptions);
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0002E57B File Offset: 0x0002C77B
		public void Get(out Options output)
		{
			output = default(Options);
			output.Set(ref this);
		}

		// Token: 0x04000DC6 RID: 3526
		private int m_ApiVersion;

		// Token: 0x04000DC7 RID: 3527
		private IntPtr m_Type;

		// Token: 0x04000DC8 RID: 3528
		private IntegratedPlatformManagementFlags m_Flags;

		// Token: 0x04000DC9 RID: 3529
		private IntPtr m_InitOptions;
	}
}
