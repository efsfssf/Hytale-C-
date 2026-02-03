using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x02000025 RID: 37
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BestDisplayNameInternal : IGettable<BestDisplayName>, ISettable<BestDisplayName>, IDisposable
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00004BC0 File Offset: 0x00002DC0
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00004BE1 File Offset: 0x00002DE1
		public EpicAccountId UserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_UserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_UserId);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00004BF4 File Offset: 0x00002DF4
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00004C15 File Offset: 0x00002E15
		public Utf8String DisplayName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DisplayName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DisplayName);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00004C28 File Offset: 0x00002E28
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00004C49 File Offset: 0x00002E49
		public Utf8String DisplayNameSanitized
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_DisplayNameSanitized, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DisplayNameSanitized);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00004C5C File Offset: 0x00002E5C
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00004C7D File Offset: 0x00002E7D
		public Utf8String Nickname
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Nickname, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Nickname);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00004C90 File Offset: 0x00002E90
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00004CA8 File Offset: 0x00002EA8
		public uint PlatformType
		{
			get
			{
				return this.m_PlatformType;
			}
			set
			{
				this.m_PlatformType = value;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00004CB4 File Offset: 0x00002EB4
		public void Set(ref BestDisplayName other)
		{
			this.m_ApiVersion = 1;
			this.UserId = other.UserId;
			this.DisplayName = other.DisplayName;
			this.DisplayNameSanitized = other.DisplayNameSanitized;
			this.Nickname = other.Nickname;
			this.PlatformType = other.PlatformType;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00004D0C File Offset: 0x00002F0C
		public void Set(ref BestDisplayName? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.UserId = other.Value.UserId;
				this.DisplayName = other.Value.DisplayName;
				this.DisplayNameSanitized = other.Value.DisplayNameSanitized;
				this.Nickname = other.Value.Nickname;
				this.PlatformType = other.Value.PlatformType;
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00004D96 File Offset: 0x00002F96
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_DisplayNameSanitized);
			Helper.Dispose(ref this.m_Nickname);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00004DC9 File Offset: 0x00002FC9
		public void Get(out BestDisplayName output)
		{
			output = default(BestDisplayName);
			output.Set(ref this);
		}

		// Token: 0x04000151 RID: 337
		private int m_ApiVersion;

		// Token: 0x04000152 RID: 338
		private IntPtr m_UserId;

		// Token: 0x04000153 RID: 339
		private IntPtr m_DisplayName;

		// Token: 0x04000154 RID: 340
		private IntPtr m_DisplayNameSanitized;

		// Token: 0x04000155 RID: 341
		private IntPtr m_Nickname;

		// Token: 0x04000156 RID: 342
		private uint m_PlatformType;
	}
}
