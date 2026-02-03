using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000348 RID: 840
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UninstallModOptionsInternal : ISettable<UninstallModOptions>, IDisposable
	{
		// Token: 0x17000648 RID: 1608
		// (set) Token: 0x060016FD RID: 5885 RVA: 0x0002181D File Offset: 0x0001FA1D
		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000649 RID: 1609
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x0002182D File Offset: 0x0001FA2D
		public ModIdentifier? Mod
		{
			set
			{
				Helper.Set<ModIdentifier, ModIdentifierInternal>(ref value, ref this.m_Mod);
			}
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0002183E File Offset: 0x0001FA3E
		public void Set(ref UninstallModOptions other)
		{
			this.m_ApiVersion = 1;
			this.LocalUserId = other.LocalUserId;
			this.Mod = other.Mod;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00021864 File Offset: 0x0001FA64
		public void Set(ref UninstallModOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.LocalUserId = other.Value.LocalUserId;
				this.Mod = other.Value.Mod;
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000218AF File Offset: 0x0001FAAF
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Mod);
		}

		// Token: 0x040009FE RID: 2558
		private int m_ApiVersion;

		// Token: 0x040009FF RID: 2559
		private IntPtr m_LocalUserId;

		// Token: 0x04000A00 RID: 2560
		private IntPtr m_Mod;
	}
}
