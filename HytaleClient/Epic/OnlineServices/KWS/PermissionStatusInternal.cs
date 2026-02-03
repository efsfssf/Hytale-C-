using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x0200049E RID: 1182
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PermissionStatusInternal : IGettable<PermissionStatus>, ISettable<PermissionStatus>, IDisposable
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x0002CEC8 File Offset: 0x0002B0C8
		// (set) Token: 0x06001EC2 RID: 7874 RVA: 0x0002CEE9 File Offset: 0x0002B0E9
		public Utf8String Name
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Name, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Name);
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x0002CEFC File Offset: 0x0002B0FC
		// (set) Token: 0x06001EC4 RID: 7876 RVA: 0x0002CF14 File Offset: 0x0002B114
		public KWSPermissionStatus Status
		{
			get
			{
				return this.m_Status;
			}
			set
			{
				this.m_Status = value;
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0002CF1E File Offset: 0x0002B11E
		public void Set(ref PermissionStatus other)
		{
			this.m_ApiVersion = 1;
			this.Name = other.Name;
			this.Status = other.Status;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0002CF44 File Offset: 0x0002B144
		public void Set(ref PermissionStatus? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Name = other.Value.Name;
				this.Status = other.Value.Status;
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0002CF8F File Offset: 0x0002B18F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Name);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0002CF9E File Offset: 0x0002B19E
		public void Get(out PermissionStatus output)
		{
			output = default(PermissionStatus);
			output.Set(ref this);
		}

		// Token: 0x04000D60 RID: 3424
		private int m_ApiVersion;

		// Token: 0x04000D61 RID: 3425
		private IntPtr m_Name;

		// Token: 0x04000D62 RID: 3426
		private KWSPermissionStatus m_Status;
	}
}
