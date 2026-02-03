using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000711 RID: 1809
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct InitializeOptionsInternal : ISettable<InitializeOptions>, IDisposable
	{
		// Token: 0x17000DE3 RID: 3555
		// (set) Token: 0x06002EAE RID: 11950 RVA: 0x000450F0 File Offset: 0x000432F0
		public IntPtr AllocateMemoryFunction
		{
			set
			{
				this.m_AllocateMemoryFunction = value;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (set) Token: 0x06002EAF RID: 11951 RVA: 0x000450FA File Offset: 0x000432FA
		public IntPtr ReallocateMemoryFunction
		{
			set
			{
				this.m_ReallocateMemoryFunction = value;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (set) Token: 0x06002EB0 RID: 11952 RVA: 0x00045104 File Offset: 0x00043304
		public IntPtr ReleaseMemoryFunction
		{
			set
			{
				this.m_ReleaseMemoryFunction = value;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (set) Token: 0x06002EB1 RID: 11953 RVA: 0x0004510E File Offset: 0x0004330E
		public Utf8String ProductName
		{
			set
			{
				Helper.Set(value, ref this.m_ProductName);
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (set) Token: 0x06002EB2 RID: 11954 RVA: 0x0004511E File Offset: 0x0004331E
		public Utf8String ProductVersion
		{
			set
			{
				Helper.Set(value, ref this.m_ProductVersion);
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (set) Token: 0x06002EB3 RID: 11955 RVA: 0x0004512E File Offset: 0x0004332E
		public IntPtr Reserved
		{
			set
			{
				this.m_Reserved = value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (set) Token: 0x06002EB4 RID: 11956 RVA: 0x00045138 File Offset: 0x00043338
		public IntPtr SystemInitializeOptions
		{
			set
			{
				this.m_SystemInitializeOptions = value;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (set) Token: 0x06002EB5 RID: 11957 RVA: 0x00045142 File Offset: 0x00043342
		public InitializeThreadAffinity? OverrideThreadAffinity
		{
			set
			{
				Helper.Set<InitializeThreadAffinity, InitializeThreadAffinityInternal>(ref value, ref this.m_OverrideThreadAffinity);
			}
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x00045154 File Offset: 0x00043354
		public void Set(ref InitializeOptions other)
		{
			this.m_ApiVersion = 4;
			this.AllocateMemoryFunction = other.AllocateMemoryFunction;
			this.ReallocateMemoryFunction = other.ReallocateMemoryFunction;
			this.ReleaseMemoryFunction = other.ReleaseMemoryFunction;
			this.ProductName = other.ProductName;
			this.ProductVersion = other.ProductVersion;
			this.Reserved = other.Reserved;
			this.SystemInitializeOptions = other.SystemInitializeOptions;
			this.OverrideThreadAffinity = other.OverrideThreadAffinity;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000451D4 File Offset: 0x000433D4
		public void Set(ref InitializeOptions? other)
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

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000452D0 File Offset: 0x000434D0
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

		// Token: 0x040014AC RID: 5292
		private int m_ApiVersion;

		// Token: 0x040014AD RID: 5293
		private IntPtr m_AllocateMemoryFunction;

		// Token: 0x040014AE RID: 5294
		private IntPtr m_ReallocateMemoryFunction;

		// Token: 0x040014AF RID: 5295
		private IntPtr m_ReleaseMemoryFunction;

		// Token: 0x040014B0 RID: 5296
		private IntPtr m_ProductName;

		// Token: 0x040014B1 RID: 5297
		private IntPtr m_ProductVersion;

		// Token: 0x040014B2 RID: 5298
		private IntPtr m_Reserved;

		// Token: 0x040014B3 RID: 5299
		private IntPtr m_SystemInitializeOptions;

		// Token: 0x040014B4 RID: 5300
		private IntPtr m_OverrideThreadAffinity;
	}
}
