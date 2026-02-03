using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	// Token: 0x0200004B RID: 75
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UserInfoDataInternal : IGettable<UserInfoData>, ISettable<UserInfoData>, IDisposable
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000062C8 File Offset: 0x000044C8
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x000062E9 File Offset: 0x000044E9
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x000062FC File Offset: 0x000044FC
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x0000631D File Offset: 0x0000451D
		public Utf8String Country
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Country, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Country);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00006330 File Offset: 0x00004530
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00006351 File Offset: 0x00004551
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00006364 File Offset: 0x00004564
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00006385 File Offset: 0x00004585
		public Utf8String PreferredLanguage
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_PreferredLanguage, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_PreferredLanguage);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00006398 File Offset: 0x00004598
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x000063B9 File Offset: 0x000045B9
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000063CC File Offset: 0x000045CC
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x000063ED File Offset: 0x000045ED
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

		// Token: 0x0600046C RID: 1132 RVA: 0x00006400 File Offset: 0x00004600
		public void Set(ref UserInfoData other)
		{
			this.m_ApiVersion = 3;
			this.UserId = other.UserId;
			this.Country = other.Country;
			this.DisplayName = other.DisplayName;
			this.PreferredLanguage = other.PreferredLanguage;
			this.Nickname = other.Nickname;
			this.DisplayNameSanitized = other.DisplayNameSanitized;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00006464 File Offset: 0x00004664
		public void Set(ref UserInfoData? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 3;
				this.UserId = other.Value.UserId;
				this.Country = other.Value.Country;
				this.DisplayName = other.Value.DisplayName;
				this.PreferredLanguage = other.Value.PreferredLanguage;
				this.Nickname = other.Value.Nickname;
				this.DisplayNameSanitized = other.Value.DisplayNameSanitized;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00006508 File Offset: 0x00004708
		public void Dispose()
		{
			Helper.Dispose(ref this.m_UserId);
			Helper.Dispose(ref this.m_Country);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_PreferredLanguage);
			Helper.Dispose(ref this.m_Nickname);
			Helper.Dispose(ref this.m_DisplayNameSanitized);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000655E File Offset: 0x0000475E
		public void Get(out UserInfoData output)
		{
			output = default(UserInfoData);
			output.Set(ref this);
		}

		// Token: 0x040001C1 RID: 449
		private int m_ApiVersion;

		// Token: 0x040001C2 RID: 450
		private IntPtr m_UserId;

		// Token: 0x040001C3 RID: 451
		private IntPtr m_Country;

		// Token: 0x040001C4 RID: 452
		private IntPtr m_DisplayName;

		// Token: 0x040001C5 RID: 453
		private IntPtr m_PreferredLanguage;

		// Token: 0x040001C6 RID: 454
		private IntPtr m_Nickname;

		// Token: 0x040001C7 RID: 455
		private IntPtr m_DisplayNameSanitized;
	}
}
