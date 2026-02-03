using System;
using System.Runtime.CompilerServices;

namespace HytaleClient.Utils
{
	// Token: 0x020007B2 RID: 1970
	public struct BitField3D
	{
		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x0004C606 File Offset: 0x0004A806
		// (set) Token: 0x06003300 RID: 13056 RVA: 0x0004C60E File Offset: 0x0004A80E
		public int MinX { get; private set; }

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x0004C617 File Offset: 0x0004A817
		// (set) Token: 0x06003302 RID: 13058 RVA: 0x0004C61F File Offset: 0x0004A81F
		public int MinY { get; private set; }

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x0004C628 File Offset: 0x0004A828
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x0004C630 File Offset: 0x0004A830
		public int MinZ { get; private set; }

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x0004C639 File Offset: 0x0004A839
		// (set) Token: 0x06003306 RID: 13062 RVA: 0x0004C641 File Offset: 0x0004A841
		public int MaxX { get; private set; }

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x0004C64A File Offset: 0x0004A84A
		// (set) Token: 0x06003308 RID: 13064 RVA: 0x0004C652 File Offset: 0x0004A852
		public int MaxY { get; private set; }

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x0004C65B File Offset: 0x0004A85B
		// (set) Token: 0x0600330A RID: 13066 RVA: 0x0004C663 File Offset: 0x0004A863
		public int MaxZ { get; private set; }

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x0004C66C File Offset: 0x0004A86C
		// (set) Token: 0x0600330C RID: 13068 RVA: 0x0004C674 File Offset: 0x0004A874
		public int SizeX { get; private set; }

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x0600330D RID: 13069 RVA: 0x0004C67D File Offset: 0x0004A87D
		// (set) Token: 0x0600330E RID: 13070 RVA: 0x0004C685 File Offset: 0x0004A885
		public int SizeY { get; private set; }

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x0600330F RID: 13071 RVA: 0x0004C68E File Offset: 0x0004A88E
		// (set) Token: 0x06003310 RID: 13072 RVA: 0x0004C696 File Offset: 0x0004A896
		public int SizeZ { get; private set; }

		// Token: 0x06003311 RID: 13073 RVA: 0x0004C69F File Offset: 0x0004A89F
		public void Initialize(int bufferSize)
		{
			this._bits = new uint[bufferSize];
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x0004C6B0 File Offset: 0x0004A8B0
		public void Setup(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			bool flag = minX > maxX || minY > maxY || minZ > maxZ;
			if (flag)
			{
				throw new Exception("Min Max values are incorrect.");
			}
			int num = maxX - minX + 1;
			int num2 = maxY - minY + 1;
			int num3 = maxZ - minZ + 1;
			int num4 = num * num2 * num3 / 32 + 1;
			bool flag2 = this._bits == null || this._bits.Length < num4;
			if (flag2)
			{
				this.Initialize(num4);
			}
			else
			{
				Array.Clear(this._bits, 0, num4);
			}
			this.MinX = minX;
			this.MinY = minY;
			this.MinZ = minZ;
			this.MaxX = maxX;
			this.MaxY = maxY;
			this.MaxZ = maxZ;
			this.SizeX = num;
			this.SizeY = num2;
			this.SizeZ = num3;
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x0004C784 File Offset: 0x0004A984
		public bool IsBitOn(int x, int y, int z)
		{
			bool flag = this.MinX > x || this.MaxX < x || this.MinY > y || this.MaxY < y || this.MinZ > z || this.MaxZ < z;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				uint num;
				uint num2;
				this.GetAccess(x, y, z, out num, out num2);
				bool flag2 = (this._bits[(int)num] & num2) > 0U;
				result = flag2;
			}
			return result;
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x0004C7F8 File Offset: 0x0004A9F8
		public void SwitchBitOn(int x, int y, int z)
		{
			bool flag = this.MinX > x || this.MaxX < x || this.MinY > y || this.MaxY < y || this.MinZ > z || this.MaxZ < z;
			if (flag)
			{
				throw new Exception("3D position out of bounds.");
			}
			uint num;
			uint num2;
			this.GetAccess(x, y, z, out num, out num2);
			uint num3 = this._bits[(int)num] | num2;
			this._bits[(int)num] = num3;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x0004C870 File Offset: 0x0004AA70
		public void SwitchBitOff(int x, int y, int z)
		{
			bool flag = this.MinX > x || this.MaxX < x || this.MinY > y || this.MaxY < y || this.MinZ > z || this.MaxZ < z;
			if (flag)
			{
				throw new Exception("3D position out of bounds.");
			}
			uint num;
			uint num2;
			this.GetAccess(x, y, z, out num, out num2);
			uint num3 = this._bits[(int)num] & ~num2;
			this._bits[(int)num] = num3;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x0004C8EC File Offset: 0x0004AAEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void GetAccess(int x, int y, int z, out uint slotID, out uint mask)
		{
			int num = x - this.MinX;
			int num2 = y - this.MinY;
			int num3 = z - this.MinZ;
			int num4 = num + num2 * this.SizeX + num3 * (this.SizeY * this.SizeX);
			slotID = (uint)(num4 / 32);
			mask = 1U << num4 % 32;
		}

		// Token: 0x040016ED RID: 5869
		private uint[] _bits;
	}
}
