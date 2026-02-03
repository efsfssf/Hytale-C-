using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Logging
{
	// Token: 0x0200035C RID: 860
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LogMessageInternal : IGettable<LogMessage>, ISettable<LogMessage>, IDisposable
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x00022460 File Offset: 0x00020660
		// (set) Token: 0x0600176E RID: 5998 RVA: 0x00022481 File Offset: 0x00020681
		public Utf8String Category
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Category, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Category);
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x00022494 File Offset: 0x00020694
		// (set) Token: 0x06001770 RID: 6000 RVA: 0x000224B5 File Offset: 0x000206B5
		public Utf8String Message
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Message, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Message);
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x000224C8 File Offset: 0x000206C8
		// (set) Token: 0x06001772 RID: 6002 RVA: 0x000224E0 File Offset: 0x000206E0
		public LogLevel Level
		{
			get
			{
				return this.m_Level;
			}
			set
			{
				this.m_Level = value;
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x000224EA File Offset: 0x000206EA
		public void Set(ref LogMessage other)
		{
			this.Category = other.Category;
			this.Message = other.Message;
			this.Level = other.Level;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00022514 File Offset: 0x00020714
		public void Set(ref LogMessage? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.Category = other.Value.Category;
				this.Message = other.Value.Message;
				this.Level = other.Value.Level;
			}
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0002256D File Offset: 0x0002076D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Category);
			Helper.Dispose(ref this.m_Message);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00022588 File Offset: 0x00020788
		public void Get(out LogMessage output)
		{
			output = default(LogMessage);
			output.Set(ref this);
		}

		// Token: 0x04000A5F RID: 2655
		private IntPtr m_Category;

		// Token: 0x04000A60 RID: 2656
		private IntPtr m_Message;

		// Token: 0x04000A61 RID: 2657
		private LogLevel m_Level;
	}
}
