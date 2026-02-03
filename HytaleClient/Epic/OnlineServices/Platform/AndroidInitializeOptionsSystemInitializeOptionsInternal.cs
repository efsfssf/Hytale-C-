using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000705 RID: 1797
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AndroidInitializeOptionsSystemInitializeOptionsInternal : IGettable<AndroidInitializeOptionsSystemInitializeOptions>, ISettable<AndroidInitializeOptionsSystemInitializeOptions>, IDisposable
	{
		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x00044448 File Offset: 0x00042648
		// (set) Token: 0x06002E41 RID: 11841 RVA: 0x00044460 File Offset: 0x00042660
		public IntPtr Reserved
		{
			get
			{
				return this.m_Reserved;
			}
			set
			{
				this.m_Reserved = value;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x0004446C File Offset: 0x0004266C
		// (set) Token: 0x06002E43 RID: 11843 RVA: 0x0004448D File Offset: 0x0004268D
		public Utf8String OptionalInternalDirectory
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_OptionalInternalDirectory, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OptionalInternalDirectory);
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06002E44 RID: 11844 RVA: 0x000444A0 File Offset: 0x000426A0
		// (set) Token: 0x06002E45 RID: 11845 RVA: 0x000444C1 File Offset: 0x000426C1
		public Utf8String OptionalExternalDirectory
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_OptionalExternalDirectory, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OptionalExternalDirectory);
			}
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000444D1 File Offset: 0x000426D1
		public void Set(ref AndroidInitializeOptionsSystemInitializeOptions other)
		{
			this.m_ApiVersion = 2;
			this.Reserved = other.Reserved;
			this.OptionalInternalDirectory = other.OptionalInternalDirectory;
			this.OptionalExternalDirectory = other.OptionalExternalDirectory;
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x00044504 File Offset: 0x00042704
		public void Set(ref AndroidInitializeOptionsSystemInitializeOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
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
				this.OptionalInternalDirectory = other.Value.OptionalInternalDirectory;
				this.OptionalExternalDirectory = other.Value.OptionalExternalDirectory;
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x00044591 File Offset: 0x00042791
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Reserved);
			Helper.Dispose(ref this.m_OptionalInternalDirectory);
			Helper.Dispose(ref this.m_OptionalExternalDirectory);
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000445B8 File Offset: 0x000427B8
		public void Get(out AndroidInitializeOptionsSystemInitializeOptions output)
		{
			output = default(AndroidInitializeOptionsSystemInitializeOptions);
			output.Set(ref this);
		}

		// Token: 0x04001474 RID: 5236
		private int m_ApiVersion;

		// Token: 0x04001475 RID: 5237
		private IntPtr m_Reserved;

		// Token: 0x04001476 RID: 5238
		private IntPtr m_OptionalInternalDirectory;

		// Token: 0x04001477 RID: 5239
		private IntPtr m_OptionalExternalDirectory;
	}
}
