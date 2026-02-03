using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000703 RID: 1795
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AndroidInitializeOptionsInternal : ISettable<AndroidInitializeOptions>, IDisposable
	{
		// Token: 0x17000DC5 RID: 3525
		// (set) Token: 0x06002E2E RID: 11822 RVA: 0x00044169 File Offset: 0x00042369
		public IntPtr AllocateMemoryFunction
		{
			set
			{
				this.m_AllocateMemoryFunction = value;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (set) Token: 0x06002E2F RID: 11823 RVA: 0x00044173 File Offset: 0x00042373
		public IntPtr ReallocateMemoryFunction
		{
			set
			{
				this.m_ReallocateMemoryFunction = value;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (set) Token: 0x06002E30 RID: 11824 RVA: 0x0004417D File Offset: 0x0004237D
		public IntPtr ReleaseMemoryFunction
		{
			set
			{
				this.m_ReleaseMemoryFunction = value;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (set) Token: 0x06002E31 RID: 11825 RVA: 0x00044187 File Offset: 0x00042387
		public Utf8String ProductName
		{
			set
			{
				Helper.Set(value, ref this.m_ProductName);
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (set) Token: 0x06002E32 RID: 11826 RVA: 0x00044197 File Offset: 0x00042397
		public Utf8String ProductVersion
		{
			set
			{
				Helper.Set(value, ref this.m_ProductVersion);
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (set) Token: 0x06002E33 RID: 11827 RVA: 0x000441A7 File Offset: 0x000423A7
		public IntPtr Reserved
		{
			set
			{
				this.m_Reserved = value;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (set) Token: 0x06002E34 RID: 11828 RVA: 0x000441B1 File Offset: 0x000423B1
		public AndroidInitializeOptionsSystemInitializeOptions? SystemInitializeOptions
		{
			set
			{
				Helper.Set<AndroidInitializeOptionsSystemInitializeOptions, AndroidInitializeOptionsSystemInitializeOptionsInternal>(ref value, ref this.m_SystemInitializeOptions);
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (set) Token: 0x06002E35 RID: 11829 RVA: 0x000441C2 File Offset: 0x000423C2
		public InitializeThreadAffinity? OverrideThreadAffinity
		{
			set
			{
				Helper.Set<InitializeThreadAffinity, InitializeThreadAffinityInternal>(ref value, ref this.m_OverrideThreadAffinity);
			}
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000441D4 File Offset: 0x000423D4
		public void Set(ref AndroidInitializeOptions other)
		{
			this.m_ApiVersion = 4;
			this.AllocateMemoryFunction = other.AllocateMemoryFunction;
			this.ReallocateMemoryFunction = other.ReallocateMemoryFunction;
			this.ReleaseMemoryFunction = other.ReleaseMemoryFunction;
			this.ProductName = other.ProductName;
			this.ProductVersion = other.ProductVersion;
			this.m_Reserved = other.Reserved;
			bool flag = this.m_Reserved == IntPtr.Zero;
			if (flag)
			{
				Helper.Set<int>(new int[]
				{
					1,
					1
				}, ref this.m_Reserved);
			}
			this.SystemInitializeOptions = other.SystemInitializeOptions;
			this.OverrideThreadAffinity = other.OverrideThreadAffinity;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x00044280 File Offset: 0x00042480
		public void Set(ref AndroidInitializeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 4;
				this.AllocateMemoryFunction = other.Value.AllocateMemoryFunction;
				this.ReallocateMemoryFunction = other.Value.ReallocateMemoryFunction;
				this.ReleaseMemoryFunction = other.Value.ReleaseMemoryFunction;
				this.ProductName = other.Value.ProductName;
				this.ProductVersion = other.Value.ProductVersion;
				this.m_Reserved = other.Value.Reserved;
				bool flag2 = this.m_Reserved == IntPtr.Zero;
				if (flag2)
				{
					Helper.Set<int>(new int[]
					{
						1,
						1
					}, ref this.m_Reserved);
				}
				this.SystemInitializeOptions = other.Value.SystemInitializeOptions;
				this.OverrideThreadAffinity = other.Value.OverrideThreadAffinity;
			}
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x0004437C File Offset: 0x0004257C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_AllocateMemoryFunction);
			Helper.Dispose(ref this.m_ReallocateMemoryFunction);
			Helper.Dispose(ref this.m_ReleaseMemoryFunction);
			Helper.Dispose(ref this.m_ProductName);
			Helper.Dispose(ref this.m_ProductVersion);
			Helper.Dispose(ref this.m_Reserved);
			Helper.Dispose(ref this.m_SystemInitializeOptions);
			Helper.Dispose(ref this.m_OverrideThreadAffinity);
		}

		// Token: 0x04001468 RID: 5224
		private int m_ApiVersion;

		// Token: 0x04001469 RID: 5225
		private IntPtr m_AllocateMemoryFunction;

		// Token: 0x0400146A RID: 5226
		private IntPtr m_ReallocateMemoryFunction;

		// Token: 0x0400146B RID: 5227
		private IntPtr m_ReleaseMemoryFunction;

		// Token: 0x0400146C RID: 5228
		private IntPtr m_ProductName;

		// Token: 0x0400146D RID: 5229
		private IntPtr m_ProductVersion;

		// Token: 0x0400146E RID: 5230
		private IntPtr m_Reserved;

		// Token: 0x0400146F RID: 5231
		private IntPtr m_SystemInitializeOptions;

		// Token: 0x04001470 RID: 5232
		private IntPtr m_OverrideThreadAffinity;
	}
}
