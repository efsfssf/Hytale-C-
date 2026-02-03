using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D7 RID: 215
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct StatInternal : IGettable<Stat>, ISettable<Stat>, IDisposable
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x0000B274 File Offset: 0x00009474
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x0000B295 File Offset: 0x00009495
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

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0000B2A8 File Offset: 0x000094A8
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x0000B2C9 File Offset: 0x000094C9
		public DateTimeOffset? StartTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_StartTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_StartTime);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0000B2DC File Offset: 0x000094DC
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0000B2FD File Offset: 0x000094FD
		public DateTimeOffset? EndTime
		{
			get
			{
				DateTimeOffset? result;
				Helper.Get(this.m_EndTime, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_EndTime);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0000B310 File Offset: 0x00009510
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0000B328 File Offset: 0x00009528
		public int Value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0000B332 File Offset: 0x00009532
		public void Set(ref Stat other)
		{
			this.m_ApiVersion = 1;
			this.Name = other.Name;
			this.StartTime = other.StartTime;
			this.EndTime = other.EndTime;
			this.Value = other.Value;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0000B370 File Offset: 0x00009570
		public void Set(ref Stat? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Name = other.Value.Name;
				this.StartTime = other.Value.StartTime;
				this.EndTime = other.Value.EndTime;
				this.Value = other.Value.Value;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0000B3E5 File Offset: 0x000095E5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Name);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0000B3F4 File Offset: 0x000095F4
		public void Get(out Stat output)
		{
			output = default(Stat);
			output.Set(ref this);
		}

		// Token: 0x040003B4 RID: 948
		private int m_ApiVersion;

		// Token: 0x040003B5 RID: 949
		private IntPtr m_Name;

		// Token: 0x040003B6 RID: 950
		private long m_StartTime;

		// Token: 0x040003B7 RID: 951
		private long m_EndTime;

		// Token: 0x040003B8 RID: 952
		private int m_Value;
	}
}
