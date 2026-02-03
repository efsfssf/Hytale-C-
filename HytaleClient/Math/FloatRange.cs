using System;

namespace HytaleClient.Math
{
	// Token: 0x020007E4 RID: 2020
	internal struct FloatRange
	{
		// Token: 0x060035D2 RID: 13778 RVA: 0x0006434D File Offset: 0x0006254D
		public FloatRange(float inclusiveMin, float inclusiveMax)
		{
			this._inclusiveMin = inclusiveMin;
			this._inclusiveMax = inclusiveMax;
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x00064360 File Offset: 0x00062560
		public bool Includes(float value)
		{
			return value >= this._inclusiveMin && value <= this._inclusiveMax;
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x0006438C File Offset: 0x0006258C
		public float Clamp(float value)
		{
			return MathHelper.Clamp(value, this._inclusiveMin, this._inclusiveMax);
		}

		// Token: 0x040017ED RID: 6125
		private float _inclusiveMin;

		// Token: 0x040017EE RID: 6126
		private float _inclusiveMax;
	}
}
