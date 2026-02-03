using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000336 RID: 822
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct InstallModOptionsInternal : ISettable<InstallModOptions>, IDisposable
	{
		// Token: 0x1700062C RID: 1580
		// (set) Token: 0x0600168B RID: 5771 RVA: 0x00020E36 File Offset: 0x0001F036
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700062D RID: 1581
		// (set) Token: 0x0600168C RID: 5772 RVA: 0x00020E46 File Offset: 0x0001F046
		public ModIdentifier? Mod
		{
			set
			{
				Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref this.m_Mod);
			}
		}

		// Token: 0x1700062E RID: 1582
		// (set) Token: 0x0600168D RID: 5773 RVA: 0x00020E57 File Offset: 0x0001F057
		public bool RemoveAfterExit
		{
			set
			{
				Helper.Set(value, ref this.m_RemoveAfterExit);
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00020E67 File Offset: 0x0001F067
		public void Set(ref InstallModOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Mod = other.Mod;
			this.RemoveAfterExit = other.RemoveAfterExit;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00020E98 File Offset: 0x0001F098
		public void Set(ref InstallModOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Mod = other.Value.Mod;
				this.RemoveAfterExit = other.Value.RemoveAfterExit;
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00020EF8 File Offset: 0x0001F0F8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Mod);
		}

		// Token: 0x040009D5 RID: 2517
		private int m_ApiVersion;

		// Token: 0x040009D6 RID: 2518
		private IntPtr m_LocalUserId;

		// Token: 0x040009D7 RID: 2519
		private IntPtr m_Mod;

		// Token: 0x040009D8 RID: 2520
		private int m_RemoveAfterExit;
	}
}
