using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	// Token: 0x02000759 RID: 1881
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PlayerStatInfoInternal : IGettable<PlayerStatInfo>, ISettable<PlayerStatInfo>, IDisposable
	{
		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x00048BB8 File Offset: 0x00046DB8
		// (set) Token: 0x060030F8 RID: 12536 RVA: 0x00048BD9 File Offset: 0x00046DD9
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

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x00048BEC File Offset: 0x00046DEC
		// (set) Token: 0x060030FA RID: 12538 RVA: 0x00048C04 File Offset: 0x00046E04
		public int CurrentValue
		{
			get
			{
				return this.m_CurrentValue;
			}
			set
			{
				this.m_CurrentValue = value;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x00048C10 File Offset: 0x00046E10
		// (set) Token: 0x060030FC RID: 12540 RVA: 0x00048C28 File Offset: 0x00046E28
		public int ThresholdValue
		{
			get
			{
				return this.m_ThresholdValue;
			}
			set
			{
				this.m_ThresholdValue = value;
			}
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x00048C32 File Offset: 0x00046E32
		public void Set(ref PlayerStatInfo other)
		{
			this.m_ApiVersion = 1;
			this.Name = other.Name;
			this.CurrentValue = other.CurrentValue;
			this.ThresholdValue = other.ThresholdValue;
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x00048C64 File Offset: 0x00046E64
		public void Set(ref PlayerStatInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Name = other.Value.Name;
				this.CurrentValue = other.Value.CurrentValue;
				this.ThresholdValue = other.Value.ThresholdValue;
			}
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x00048CC4 File Offset: 0x00046EC4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Name);
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x00048CD3 File Offset: 0x00046ED3
		public void Get(out PlayerStatInfo output)
		{
			output = default(PlayerStatInfo);
			output.Set(ref this);
		}

		// Token: 0x040015D3 RID: 5587
		private int m_ApiVersion;

		// Token: 0x040015D4 RID: 5588
		private IntPtr m_Name;

		// Token: 0x040015D5 RID: 5589
		private int m_CurrentValue;

		// Token: 0x040015D6 RID: 5590
		private int m_ThresholdValue;
	}
}
