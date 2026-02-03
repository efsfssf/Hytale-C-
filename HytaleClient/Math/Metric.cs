using System;

namespace HytaleClient.Math
{
	// Token: 0x020007E9 RID: 2025
	internal struct Metric
	{
		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x0006B0A7 File Offset: 0x000692A7
		// (set) Token: 0x060036AA RID: 13994 RVA: 0x0006B0AF File Offset: 0x000692AF
		public long Min { get; private set; }

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x060036AB RID: 13995 RVA: 0x0006B0B8 File Offset: 0x000692B8
		// (set) Token: 0x060036AC RID: 13996 RVA: 0x0006B0C0 File Offset: 0x000692C0
		public long Max { get; private set; }

		// Token: 0x060036AD RID: 13997 RVA: 0x0006B0C9 File Offset: 0x000692C9
		public Metric(AverageCollector averageCollector)
		{
			this.Min = long.MaxValue;
			this.Average = averageCollector;
			this.Max = long.MinValue;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x0006B0F4 File Offset: 0x000692F4
		public void Add(long value)
		{
			bool flag = value < this.Min;
			if (flag)
			{
				this.Min = value;
			}
			this.Average.Add((double)value);
			bool flag2 = value > this.Max;
			if (flag2)
			{
				this.Max = value;
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x0006B13A File Offset: 0x0006933A
		public void Remove(long value)
		{
			this.Average.Remove((double)value);
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0006B14C File Offset: 0x0006934C
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[]
			{
				"Average",
				this.Average,
				"Min",
				this.Min,
				"Max",
				this.Max
			});
		}

		// Token: 0x04001821 RID: 6177
		public AverageCollector Average;
	}
}
