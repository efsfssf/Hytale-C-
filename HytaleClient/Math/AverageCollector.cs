using System;

namespace HytaleClient.Math
{
	// Token: 0x020007DD RID: 2013
	internal struct AverageCollector
	{
		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06003524 RID: 13604 RVA: 0x0005F025 File Offset: 0x0005D225
		// (set) Token: 0x06003525 RID: 13605 RVA: 0x0005F02D File Offset: 0x0005D22D
		public double Val { get; private set; }

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06003526 RID: 13606 RVA: 0x0005F036 File Offset: 0x0005D236
		// (set) Token: 0x06003527 RID: 13607 RVA: 0x0005F03E File Offset: 0x0005D23E
		public long Count { get; private set; }

		// Token: 0x06003528 RID: 13608 RVA: 0x0005F048 File Offset: 0x0005D248
		public double AddAndGet(double v)
		{
			this.Add(v);
			return this.Val;
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x0005F068 File Offset: 0x0005D268
		public void Add(double v)
		{
			long count = this.Count + 1L;
			this.Count = count;
			this.Val = this.Val - this.Val / (double)this.Count + v / (double)this.Count;
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x0005F0B0 File Offset: 0x0005D2B0
		public void Remove(double v)
		{
			bool flag = this.Count == 1L;
			if (flag)
			{
				this.Count = 0L;
				this.Val = 0.0;
			}
			else
			{
				bool flag2 = this.Count > 1L;
				if (flag2)
				{
					this.Val = (this.Val - v / (double)this.Count) / (1.0 - 1.0 / (double)this.Count);
					long count = this.Count - 1L;
					this.Count = count;
				}
			}
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x0005F13E File Offset: 0x0005D33E
		public void Reset()
		{
			this.Val = 0.0;
			this.Count = 0L;
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x0005F15C File Offset: 0x0005D35C
		public static double Add(double val, double v, int n)
		{
			return val - val / (double)n + v / (double)n;
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x0005F17C File Offset: 0x0005D37C
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}", new object[]
			{
				"Val",
				this.Val,
				"Count",
				this.Count
			});
		}
	}
}
