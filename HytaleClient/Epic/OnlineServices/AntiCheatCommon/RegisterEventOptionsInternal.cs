using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C6 RID: 1734
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterEventOptionsInternal : ISettable<RegisterEventOptions>, IDisposable
	{
		// Token: 0x17000D73 RID: 3443
		// (set) Token: 0x06002D11 RID: 11537 RVA: 0x00042927 File Offset: 0x00040B27
		public uint EventId
		{
			set
			{
				this.m_EventId = value;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (set) Token: 0x06002D12 RID: 11538 RVA: 0x00042931 File Offset: 0x00040B31
		public Utf8String EventName
		{
			set
			{
				Helper.Set(value, ref this.m_EventName);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (set) Token: 0x06002D13 RID: 11539 RVA: 0x00042941 File Offset: 0x00040B41
		public AntiCheatCommonEventType EventType
		{
			set
			{
				this.m_EventType = value;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (set) Token: 0x06002D14 RID: 11540 RVA: 0x0004294B File Offset: 0x00040B4B
		public RegisterEventParamDef[] ParamDefs
		{
			set
			{
				Helper.Set<RegisterEventParamDef, RegisterEventParamDefInternal>(ref value, ref this.m_ParamDefs, out this.m_ParamDefsCount);
			}
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x00042962 File Offset: 0x00040B62
		public void Set(ref RegisterEventOptions other)
		{
			this.m_ApiVersion = 1;
			this.EventId = other.EventId;
			this.EventName = other.EventName;
			this.EventType = other.EventType;
			this.ParamDefs = other.ParamDefs;
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000429A0 File Offset: 0x00040BA0
		public void Set(ref RegisterEventOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.EventId = other.Value.EventId;
				this.EventName = other.Value.EventName;
				this.EventType = other.Value.EventType;
				this.ParamDefs = other.Value.ParamDefs;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x00042A15 File Offset: 0x00040C15
		public void Dispose()
		{
			Helper.Dispose(ref this.m_EventName);
			Helper.Dispose(ref this.m_ParamDefs);
		}

		// Token: 0x040013D3 RID: 5075
		private int m_ApiVersion;

		// Token: 0x040013D4 RID: 5076
		private uint m_EventId;

		// Token: 0x040013D5 RID: 5077
		private IntPtr m_EventName;

		// Token: 0x040013D6 RID: 5078
		private AntiCheatCommonEventType m_EventType;

		// Token: 0x040013D7 RID: 5079
		private uint m_ParamDefsCount;

		// Token: 0x040013D8 RID: 5080
		private IntPtr m_ParamDefs;
	}
}
