using System;
using System.Collections;
using System.Collections.Generic;

namespace HytaleClient.Utils
{
	// Token: 0x020007D0 RID: 2000
	public class SpiralIterator : IEnumerable<long>, IEnumerable
	{
		// Token: 0x0600342F RID: 13359 RVA: 0x00053B40 File Offset: 0x00051D40
		public void Initialize(int chunkX, int chunkZ, int radius)
		{
			this._chunkX = chunkX;
			this._chunkZ = chunkZ;
			int num = 1 + radius * 2;
			this._maxI = num * num;
			this._i = 0;
			this._x = (this._z = 0);
			this._dx = 0;
			this._dz = -1;
			this._isSetup = true;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00053B97 File Offset: 0x00051D97
		public void Reset()
		{
			this._isSetup = false;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x00053BA1 File Offset: 0x00051DA1
		public IEnumerator<long> GetEnumerator()
		{
			SpiralIterator.<GetEnumerator>d__11 <GetEnumerator>d__ = new SpiralIterator.<GetEnumerator>d__11(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x00053BB0 File Offset: 0x00051DB0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04001762 RID: 5986
		private bool _isSetup;

		// Token: 0x04001763 RID: 5987
		private int _chunkX;

		// Token: 0x04001764 RID: 5988
		private int _chunkZ;

		// Token: 0x04001765 RID: 5989
		private int _maxI;

		// Token: 0x04001766 RID: 5990
		private int _i;

		// Token: 0x04001767 RID: 5991
		private int _x;

		// Token: 0x04001768 RID: 5992
		private int _z;

		// Token: 0x04001769 RID: 5993
		private int _dx;

		// Token: 0x0400176A RID: 5994
		private int _dz;
	}
}
