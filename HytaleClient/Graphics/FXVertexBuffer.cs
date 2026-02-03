using System;
using System.Runtime.CompilerServices;
using System.Threading;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A7 RID: 2471
	internal struct FXVertexBuffer
	{
		// Token: 0x06004F63 RID: 20323 RVA: 0x00165DCC File Offset: 0x00163FCC
		public void Initialize(int maxParticleCount)
		{
			this._particleVertices = new FXVertex[maxParticleCount * 4];
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00165DDD File Offset: 0x00163FDD
		public void Dispose()
		{
			this._particleVertices = null;
		}

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x00165DE7 File Offset: 0x00163FE7
		public FXVertex[] ParticleVertices
		{
			get
			{
				return this._particleVertices;
			}
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x00165DEF File Offset: 0x00163FEF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearVertexDataStorage()
		{
			this._particleCount = 0;
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x00165DFC File Offset: 0x00163FFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetVertexDataStored()
		{
			return this._particleCount;
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x00165E14 File Offset: 0x00164014
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReserveVertexDataStorage(int count)
		{
			int num = Interlocked.Add(ref this._particleCount, count);
			return num - count;
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00165E38 File Offset: 0x00164038
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetVertexDataConfig(int particleIndex, uint config)
		{
			this._particleVertices[particleIndex * 4].Config = config;
			this._particleVertices[particleIndex * 4 + 1].Config = config;
			this._particleVertices[particleIndex * 4 + 2].Config = config;
			this._particleVertices[particleIndex * 4 + 3].Config = config;
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x00165E9C File Offset: 0x0016409C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetParticleVertexDataPositionAndScale(int particleIndex, Vector3 position, Vector2 scale)
		{
			this._particleVertices[particleIndex * 4].Position = position;
			this._particleVertices[particleIndex * 4 + 1].Position = position;
			this._particleVertices[particleIndex * 4 + 2].Position = position;
			this._particleVertices[particleIndex * 4 + 3].Position = position;
			this._particleVertices[particleIndex * 4].Scale = scale;
			this._particleVertices[particleIndex * 4 + 1].Scale = scale;
			this._particleVertices[particleIndex * 4 + 2].Scale = scale;
			this._particleVertices[particleIndex * 4 + 3].Scale = scale;
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x00165F58 File Offset: 0x00164158
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetParticleVertexDataTextureInfo(int particleIndex, uint textureInfo)
		{
			this._particleVertices[particleIndex * 4].TextureInfo = textureInfo;
			this._particleVertices[particleIndex * 4 + 1].TextureInfo = textureInfo;
			this._particleVertices[particleIndex * 4 + 2].TextureInfo = textureInfo;
			this._particleVertices[particleIndex * 4 + 3].TextureInfo = textureInfo;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x00165FBC File Offset: 0x001641BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetParticleVertexDataColor(int particleIndex, uint color)
		{
			this._particleVertices[particleIndex * 4].Color = color;
			this._particleVertices[particleIndex * 4 + 1].Color = color;
			this._particleVertices[particleIndex * 4 + 2].Color = color;
			this._particleVertices[particleIndex * 4 + 3].Color = color;
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00166020 File Offset: 0x00164220
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetParticleVertexDataVelocityAndRotation(int particleIndex, Vector3 velocity, Vector4 rotation)
		{
			this._particleVertices[particleIndex * 4].Velocity = velocity;
			this._particleVertices[particleIndex * 4 + 1].Velocity = velocity;
			this._particleVertices[particleIndex * 4 + 2].Velocity = velocity;
			this._particleVertices[particleIndex * 4 + 3].Velocity = velocity;
			this._particleVertices[particleIndex * 4].Rotation = rotation;
			this._particleVertices[particleIndex * 4 + 1].Rotation = rotation;
			this._particleVertices[particleIndex * 4 + 2].Rotation = rotation;
			this._particleVertices[particleIndex * 4 + 3].Rotation = rotation;
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x001660DC File Offset: 0x001642DC
		public void SetParticleVertexDataSeedAndLifeRatio(int particleIndex, uint seedAndLifeRatio)
		{
			this._particleVertices[particleIndex * 4].SeedAndLifeRatio = seedAndLifeRatio;
			this._particleVertices[particleIndex * 4 + 1].SeedAndLifeRatio = seedAndLifeRatio;
			this._particleVertices[particleIndex * 4 + 2].SeedAndLifeRatio = seedAndLifeRatio;
			this._particleVertices[particleIndex * 4 + 3].SeedAndLifeRatio = seedAndLifeRatio;
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x00166140 File Offset: 0x00164340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrailFirstSegmentVertexLength(int segmentIndex, float length)
		{
			this._particleVertices[segmentIndex * 4].Length = length;
			this._particleVertices[segmentIndex * 4 + 1].Length = length;
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x0016616D File Offset: 0x0016436D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrailLastSegmentVertexLength(int segmentIndex, float length)
		{
			this._particleVertices[(segmentIndex - 2) * 4 + 2].Length = length;
			this._particleVertices[(segmentIndex - 2) * 4 + 3].Length = length;
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x001661A0 File Offset: 0x001643A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrailSegmentVertexLength(int segmentIndex, float length)
		{
			segmentIndex--;
			this._particleVertices[2 + segmentIndex * 4].Length = length;
			this._particleVertices[2 + segmentIndex * 4 + 1].Length = length;
			this._particleVertices[2 + segmentIndex * 4 + 2].Length = length;
			this._particleVertices[2 + segmentIndex * 4 + 3].Length = length;
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x00166214 File Offset: 0x00164414
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrailFirstSegmentVertexPosition(int segmentIndex, Vector3 position, Quaternion rotation, float length)
		{
			Vector3 topLeftPosition = position + Vector3.Transform(new Vector3(0f, length, 0f), rotation);
			Vector3 bottomLeftPosition = position + Vector3.Transform(new Vector3(0f, -length, 0f), rotation);
			ref FXVertex ptr = ref this._particleVertices[segmentIndex * 4];
			ptr.TopLeftPosition = topLeftPosition;
			ptr.BottomLeftPosition = bottomLeftPosition;
			ref FXVertex ptr2 = ref this._particleVertices[segmentIndex * 4 + 1];
			ptr2.TopLeftPosition = topLeftPosition;
			ptr2.BottomLeftPosition = bottomLeftPosition;
			ref FXVertex ptr3 = ref this._particleVertices[segmentIndex * 4 + 2];
			ptr3.TopLeftPosition = topLeftPosition;
			ptr3.BottomLeftPosition = bottomLeftPosition;
			ref FXVertex ptr4 = ref this._particleVertices[segmentIndex * 4 + 3];
			ptr4.TopLeftPosition = topLeftPosition;
			ptr4.BottomLeftPosition = bottomLeftPosition;
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001662E0 File Offset: 0x001644E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrailLastSegmentVertexPosition(int segmentIndex, Vector3 position, Quaternion rotation, float length)
		{
			Vector3 topRightPosition = position + Vector3.Transform(new Vector3(0f, length, 0f), rotation);
			Vector3 bottomRightPosition = position + Vector3.Transform(new Vector3(0f, -length, 0f), rotation);
			ref FXVertex ptr = ref this._particleVertices[(segmentIndex - 2) * 4];
			ptr.TopRightPosition = topRightPosition;
			ptr.BottomRightPosition = bottomRightPosition;
			ref FXVertex ptr2 = ref this._particleVertices[(segmentIndex - 2) * 4 + 1];
			ptr2.TopRightPosition = topRightPosition;
			ptr2.BottomRightPosition = bottomRightPosition;
			ref FXVertex ptr3 = ref this._particleVertices[(segmentIndex - 2) * 4 + 2];
			ptr3.TopRightPosition = topRightPosition;
			ptr3.BottomRightPosition = bottomRightPosition;
			ref FXVertex ptr4 = ref this._particleVertices[(segmentIndex - 2) * 4 + 3];
			ptr4.TopRightPosition = topRightPosition;
			ptr4.BottomRightPosition = bottomRightPosition;
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x001663B4 File Offset: 0x001645B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTrailSegmentVertexPosition(int segmentIndex, Vector3 position, Quaternion rotation, float length)
		{
			Vector3 vector = position + Vector3.Transform(new Vector3(0f, length, 0f), rotation);
			Vector3 vector2 = position + Vector3.Transform(new Vector3(0f, -length, 0f), rotation);
			ref FXVertex ptr = ref this._particleVertices[(segmentIndex - 1) * 4];
			ptr.TopRightPosition = vector;
			ptr.BottomRightPosition = vector2;
			ref FXVertex ptr2 = ref this._particleVertices[(segmentIndex - 1) * 4 + 1];
			ptr2.TopRightPosition = vector;
			ptr2.BottomRightPosition = vector2;
			ref FXVertex ptr3 = ref this._particleVertices[(segmentIndex - 1) * 4 + 2];
			ptr3.TopRightPosition = vector;
			ptr3.BottomRightPosition = vector2;
			ref FXVertex ptr4 = ref this._particleVertices[(segmentIndex - 1) * 4 + 3];
			ptr4.TopRightPosition = vector;
			ptr4.BottomRightPosition = vector2;
			ref FXVertex ptr5 = ref this._particleVertices[(segmentIndex - 1) * 4 + 4];
			ptr5.TopLeftPosition = vector;
			ptr5.BottomLeftPosition = vector2;
			ref FXVertex ptr6 = ref this._particleVertices[(segmentIndex - 1) * 4 + 5];
			ptr6.TopLeftPosition = vector;
			ptr6.BottomLeftPosition = vector2;
			ref FXVertex ptr7 = ref this._particleVertices[(segmentIndex - 1) * 4 + 6];
			ptr7.TopLeftPosition = vector;
			ptr7.BottomLeftPosition = vector2;
			ref FXVertex ptr8 = ref this._particleVertices[(segmentIndex - 1) * 4 + 7];
			ptr8.TopLeftPosition = vector;
			ptr8.BottomLeftPosition = vector2;
		}

		// Token: 0x04002AB4 RID: 10932
		private FXVertex[] _particleVertices;

		// Token: 0x04002AB5 RID: 10933
		private int _particleCount;
	}
}
