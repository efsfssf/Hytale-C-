using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006A6 RID: 1702
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogEventParamPairInternal : IGettable<LogEventParamPair>, ISettable<LogEventParamPair>, IDisposable
	{
		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06002BBF RID: 11199 RVA: 0x0004066C File Offset: 0x0003E86C
		// (set) Token: 0x06002BC0 RID: 11200 RVA: 0x0004068D File Offset: 0x0003E88D
		public LogEventParamPairParamValue ParamValue
		{
			get
			{
				LogEventParamPairParamValue result;
				Helper.Get<LogEventParamPairParamValueInternal, LogEventParamPairParamValue>(ref this.m_ParamValue, out result);
				return result;
			}
			set
			{
				Helper.Set<LogEventParamPairParamValue, LogEventParamPairParamValueInternal>(ref value, ref this.m_ParamValue);
			}
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x0004069E File Offset: 0x0003E89E
		public void Set(ref LogEventParamPair other)
		{
			this.ParamValue = other.ParamValue;
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000406B0 File Offset: 0x0003E8B0
		public void Set(ref LogEventParamPair? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ParamValue = other.Value.ParamValue;
			}
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000406DF File Offset: 0x0003E8DF
		public void Dispose()
		{
			Helper.Dispose<LogEventParamPairParamValueInternal>(ref this.m_ParamValue);
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000406EE File Offset: 0x0003E8EE
		public void Get(out LogEventParamPair output)
		{
			output = default(LogEventParamPair);
			output.Set(ref this);
		}

		// Token: 0x04001330 RID: 4912
		private LogEventParamPairParamValueInternal m_ParamValue;
	}
}
