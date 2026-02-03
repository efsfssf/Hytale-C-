using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C8 RID: 1736
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterEventParamDefInternal : IGettable<RegisterEventParamDef>, ISettable<RegisterEventParamDef>, IDisposable
	{
		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06002D1D RID: 11549 RVA: 0x00042A70 File Offset: 0x00040C70
		// (set) Token: 0x06002D1E RID: 11550 RVA: 0x00042A91 File Offset: 0x00040C91
		public Utf8String ParamName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ParamName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParamName);
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06002D1F RID: 11551 RVA: 0x00042AA4 File Offset: 0x00040CA4
		// (set) Token: 0x06002D20 RID: 11552 RVA: 0x00042ABC File Offset: 0x00040CBC
		public AntiCheatCommonEventParamType ParamType
		{
			get
			{
				return this.m_ParamType;
			}
			set
			{
				this.m_ParamType = value;
			}
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x00042AC6 File Offset: 0x00040CC6
		public void Set(ref RegisterEventParamDef other)
		{
			this.ParamName = other.ParamName;
			this.ParamType = other.ParamType;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x00042AE4 File Offset: 0x00040CE4
		public void Set(ref RegisterEventParamDef? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ParamName = other.Value.ParamName;
				this.ParamType = other.Value.ParamType;
			}
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x00042B28 File Offset: 0x00040D28
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ParamName);
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x00042B37 File Offset: 0x00040D37
		public void Get(out RegisterEventParamDef output)
		{
			output = default(RegisterEventParamDef);
			output.Set(ref this);
		}

		// Token: 0x040013DB RID: 5083
		private IntPtr m_ParamName;

		// Token: 0x040013DC RID: 5084
		private AntiCheatCommonEventParamType m_ParamType;
	}
}
