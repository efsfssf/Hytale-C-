using System;
using HytaleClient.Core;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Trails
{
	// Token: 0x02000AAB RID: 2731
	internal class Trail : Disposable
	{
		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x060055D0 RID: 21968 RVA: 0x001986D7 File Offset: 0x001968D7
		public int ParticleVertexDataStartIndex
		{
			get
			{
				return this._particleVertexDataStartIndex;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x060055D1 RID: 21969 RVA: 0x001986DF File Offset: 0x001968DF
		public int ParticleCount
		{
			get
			{
				return this._segmentCount - 1;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x060055D2 RID: 21970 RVA: 0x001986E9 File Offset: 0x001968E9
		public bool IsDistortion
		{
			get
			{
				return this.RenderMode == FXSystem.RenderMode.Distortion;
			}
		}

		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x060055D3 RID: 21971 RVA: 0x001986F4 File Offset: 0x001968F4
		public float LightInfluence
		{
			get
			{
				return this._trailSettings.LightInfluence;
			}
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x00198701 File Offset: 0x00196901
		public bool NeedsUpdating()
		{
			return this.IsSpawned && !this.IsExpired;
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x060055D5 RID: 21973 RVA: 0x00198717 File Offset: 0x00196917
		// (set) Token: 0x060055D6 RID: 21974 RVA: 0x0019871F File Offset: 0x0019691F
		public bool IsSpawned { get; private set; } = false;

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x060055D7 RID: 21975 RVA: 0x00198728 File Offset: 0x00196928
		public string SettingsId
		{
			get
			{
				return this._trailSettings.Id;
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x060055D8 RID: 21976 RVA: 0x00198735 File Offset: 0x00196935
		public FXSystem.RenderMode RenderMode
		{
			get
			{
				return this._trailSettings.RenderMode;
			}
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x00198744 File Offset: 0x00196944
		public Trail(GraphicsDevice graphics, TrailFXSystem trailFXSystem, TrailSettings trailSettings, Vector2 textureAltasInverseSize, int id)
		{
			this._graphics = graphics;
			this._trailFXSystem = trailFXSystem;
			this._segmentBuffer = this._trailFXSystem.SegmentBuffer;
			this._trailSettings = trailSettings;
			this.Id = id;
			this._lifeSpan = ((this._trailSettings.LifeSpan != 0) ? this._trailSettings.LifeSpan : 10);
			bool smooth = this._trailSettings.Smooth;
			if (smooth)
			{
				this._lifeSpan = (int)MathHelper.Min((float)this._lifeSpan, 40f);
				this._segmentCount = this._lifeSpan * 5;
			}
			else
			{
				this._lifeSpan = (int)MathHelper.Min((float)this._lifeSpan, 200f);
				this._segmentCount = this._lifeSpan;
			}
			this._segmentCount++;
			this._startWidth = this._trailSettings.Start.Width;
			this._endWidth = this._trailSettings.End.Width;
			this._intersectionHighlight = new Vector4(this._trailSettings.IntersectionHighlightColor.X, this._trailSettings.IntersectionHighlightColor.Y, this._trailSettings.IntersectionHighlightColor.Z, this._trailSettings.IntersectionHighlightThreshold);
			this.UpdateTexture(textureAltasInverseSize);
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x001988FC File Offset: 0x00196AFC
		public bool Initialize()
		{
			this._segmentBufferStartIndex = this._trailFXSystem.RequestSegmentBufferStorage(this._segmentCount);
			bool flag = this._segmentBufferStartIndex >= 0 && this._segmentBufferStartIndex < this._trailFXSystem.SegmentBufferStorageMaxCount;
			bool flag2 = flag;
			if (flag2)
			{
				this._segmentBuffer.Life[this._segmentBufferStartIndex] = this._lifeSpan;
			}
			return flag;
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x00198968 File Offset: 0x00196B68
		private void UpdateTexture(Vector2 textureAltasInverseSize)
		{
			this._textureAltasInverseSize = textureAltasInverseSize;
			Rectangle imageLocation = this._trailSettings.ImageLocation;
			Point frameSize = this._trailSettings.FrameSize;
			this._frameSize = ((frameSize.X == 0 || frameSize.Y == 0) ? new Point(imageLocation.Width, imageLocation.Height) : new Point(frameSize.X, frameSize.Y));
			this._tilesPerRow = imageLocation.Width / this._frameSize.X;
			Point frameRange = this._trailSettings.FrameRange;
			this._hasAnimation = (frameRange.X != frameRange.Y);
			this._targetTextureIndex = frameRange.X;
			this._textureCoords = new Vector4((float)(imageLocation.X + this._frameSize.X * (this._targetTextureIndex % this._tilesPerRow)) * this._textureAltasInverseSize.X, (float)(imageLocation.Y + this._frameSize.Y * (this._targetTextureIndex / this._tilesPerRow)) * this._textureAltasInverseSize.Y, (float)(imageLocation.X + (this._frameSize.X * (this._targetTextureIndex % this._tilesPerRow) + this._frameSize.X)) * this._textureAltasInverseSize.X, (float)(imageLocation.Y + (this._frameSize.Y * (this._targetTextureIndex / this._tilesPerRow) + this._frameSize.Y)) * this._textureAltasInverseSize.Y);
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x00198AED File Offset: 0x00196CED
		public void SetScale(float scale)
		{
			this._startWidth = this._trailSettings.Start.Width * scale;
			this._endWidth = this._trailSettings.End.Width * scale;
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x00198B20 File Offset: 0x00196D20
		protected override void DoDispose()
		{
			this.Release();
			this._trailFXSystem = null;
			this._graphics = null;
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x00198B38 File Offset: 0x00196D38
		public void SetSpawn()
		{
			for (int i = 0; i < this._segmentCount; i++)
			{
				this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + i] = this.Position;
				this._segmentBuffer.Rotation[this._segmentBufferStartIndex + i] = this.Rotation * Quaternion.CreateFromYawPitchRoll(0f, 0f, MathHelper.ToRadians(this._trailSettings.Roll));
			}
			this.IsSpawned = true;
		}

		// Token: 0x060055DF RID: 21983 RVA: 0x00198BC5 File Offset: 0x00196DC5
		public void UpdateLight(Vector4 staticLightColor)
		{
			staticLightColor.W = this._trailSettings.LightInfluence;
			this._staticLightColorAndInfluence = staticLightColor;
		}

		// Token: 0x060055E0 RID: 21984 RVA: 0x00198BE4 File Offset: 0x00196DE4
		public void LightUpdate()
		{
			bool flag = this._wasFirstPerson != this.IsFirstPerson;
			if (flag)
			{
				this.SetSpawn();
			}
			ref Vector3 ptr = ref this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex];
			ptr = this.Position;
			this._segmentBuffer.Rotation[this._segmentBufferStartIndex] = this.Rotation * Quaternion.CreateFromYawPitchRoll(0f, 0f, MathHelper.ToRadians(this._trailSettings.Roll));
			this._segmentBuffer.Length[this._segmentBufferStartIndex + 1] = Vector3.Distance(ptr, this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + 1]);
			this._wasFirstPerson = this.IsFirstPerson;
			this._lastPosition = this.Position;
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x00198CC0 File Offset: 0x00196EC0
		public void Update()
		{
			this._trailTailLength = 0f;
			this._lastSegment = this._segmentCount - 1;
			ref Vector3 ptr = ref this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex];
			ref Quaternion ptr2 = ref this._segmentBuffer.Rotation[this._segmentBufferStartIndex];
			ref Vector3 ptr3 = ref this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + 1];
			ref float ptr4 = ref this._segmentBuffer.Length[this._segmentBufferStartIndex + 1];
			bool flag = this._lastPosition != this.Position;
			if (flag)
			{
				int num = (!this._trailSettings.Smooth || ptr4 <= 0.25f) ? 1 : Math.Min((int)(ptr4 / 0.25f), 5);
				for (int i = this._segmentCount - 1; i > num; i--)
				{
					ref int ptr5 = ref this._segmentBuffer.Life[this._segmentBufferStartIndex + i];
					ref float ptr6 = ref this._segmentBuffer.Length[this._segmentBufferStartIndex + i];
					ptr5 = this._segmentBuffer.Life[this._segmentBufferStartIndex + i - num];
					bool flag2 = ptr5 > 0;
					if (flag2)
					{
						ptr5--;
					}
					bool flag3 = ptr5 == 0;
					if (flag3)
					{
						this._lastSegment = i - 1;
					}
					else
					{
						this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + i] = this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + i - num];
						ptr6 = this._segmentBuffer.Length[this._segmentBufferStartIndex + i - num];
						this._segmentBuffer.Rotation[this._segmentBufferStartIndex + i] = this._segmentBuffer.Rotation[this._segmentBufferStartIndex + i - num];
						bool flag4 = num == 1 || i != num + 1;
						if (flag4)
						{
							this._trailTailLength += ptr6;
						}
					}
				}
				ref int ptr7 = ref this._segmentBuffer.Life[this._segmentBufferStartIndex];
				bool flag5 = num > 1;
				if (flag5)
				{
					ref Vector3 ptr8 = ref this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + num + 1];
					ref Quaternion ptr9 = ref this._segmentBuffer.Rotation[this._segmentBufferStartIndex + num + 1];
					Vector3 vector = this._lastRealPositions[1] - this._lastRealPositions[0];
					Vector3 value = (vector != Vector3.Zero) ? Vector3.Normalize(vector) : Vector3.Zero;
					Vector3 value2 = (vector != Vector3.Zero) ? Vector3.Normalize(-vector) : Vector3.Zero;
					Vector3 normal = (this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex] != this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + 1]) ? Vector3.Normalize(this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex] - this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + 1]) : Vector3.Zero;
					Vector3 tangent = value * ptr4 * 0.75f;
					Vector3 tangent2 = Vector3.Reflect(value2 * ptr4 * 0.75f, normal);
					for (int j = num; j > 1; j--)
					{
						float amount = (float)(j - 1) / (float)num;
						Vector3 vector2 = Vector3.Hermite(ptr, tangent2, ptr8, tangent, amount);
						Quaternion quaternion = Quaternion.Slerp(ptr2, ptr9, amount);
						this._segmentBuffer.Life[this._segmentBufferStartIndex + j] = ptr7;
						this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + j] = vector2;
						this._segmentBuffer.Rotation[this._segmentBufferStartIndex + j] = quaternion;
						this._segmentBuffer.Length[this._segmentBufferStartIndex + j + 1] = Vector3.Distance(this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + j], this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + j + 1]);
						this._trailTailLength += this._segmentBuffer.Length[this._segmentBufferStartIndex + j + 1];
					}
					this._segmentBuffer.Length[this._segmentBufferStartIndex + 2] = Vector3.Distance(this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + 2], ptr);
					this._trailTailLength += this._segmentBuffer.Length[this._segmentBufferStartIndex + 2];
				}
				ptr4 = 0f;
				this._segmentBuffer.Life[this._segmentBufferStartIndex + 1] = ptr7;
				ptr3 = ptr;
				this._segmentBuffer.Rotation[this._segmentBufferStartIndex + 1] = ptr2;
				this._lastRealPositions[1] = this._lastRealPositions[0];
				this._lastRealPositions[0] = ptr3;
			}
			else
			{
				for (int k = this._segmentCount - 1; k > 0; k--)
				{
					ref int ptr10 = ref this._segmentBuffer.Life[this._segmentBufferStartIndex + k];
					bool flag6 = ptr10 > 0;
					if (flag6)
					{
						ptr10--;
					}
					bool flag7 = ptr10 == 0;
					if (flag7)
					{
						this._lastSegment = k - 1;
					}
					else
					{
						bool flag8 = k > 1;
						if (flag8)
						{
							this._trailTailLength += this._segmentBuffer.Length[this._segmentBufferStartIndex + k];
						}
					}
				}
			}
			bool hasAnimation = this._hasAnimation;
			if (hasAnimation)
			{
				this._frameTimer++;
				bool flag9 = this._frameTimer > this._trailSettings.FrameLifeSpan;
				if (flag9)
				{
					this._targetTextureIndex++;
					bool flag10 = this._targetTextureIndex > this._trailSettings.FrameRange.Y;
					if (flag10)
					{
						this._targetTextureIndex = this._trailSettings.FrameRange.X;
					}
					ref Rectangle ptr11 = ref this._trailSettings.ImageLocation;
					this._textureCoords.X = (float)(ptr11.X + this._frameSize.X * (this._targetTextureIndex % this._tilesPerRow)) * this._textureAltasInverseSize.X;
					this._textureCoords.Y = (float)(ptr11.Y + this._frameSize.Y * (this._targetTextureIndex / this._tilesPerRow)) * this._textureAltasInverseSize.Y;
					this._textureCoords.Z = (float)(ptr11.X + (this._frameSize.X * (this._targetTextureIndex % this._tilesPerRow) + this._frameSize.X)) * this._textureAltasInverseSize.X;
					this._textureCoords.W = (float)(ptr11.Y + (this._frameSize.Y * (this._targetTextureIndex / this._tilesPerRow) + this._frameSize.Y)) * this._textureAltasInverseSize.Y;
					this._frameTimer = 0;
				}
			}
			bool flag11 = this._wasFirstPerson != this.IsFirstPerson;
			if (flag11)
			{
				this.SetSpawn();
			}
			ptr = this.Position;
			ptr2 = this.Rotation * Quaternion.CreateFromYawPitchRoll(0f, 0f, MathHelper.ToRadians(this._trailSettings.Roll));
			ptr4 = Vector3.Distance(ptr, ptr3);
			this._wasFirstPerson = this.IsFirstPerson;
			this._lastPosition = this.Position;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x001994E3 File Offset: 0x001976E3
		public void ReserveVertexDataStorage(ref FXVertexBuffer vertexBuffer, ushort drawId)
		{
			this._drawId = drawId;
			this._particleVertexDataStartIndex = vertexBuffer.ReserveVertexDataStorage(this.ParticleCount);
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x00199500 File Offset: 0x00197700
		public unsafe void PrepareForDraw(Vector3 cameraPosition, ref FXVertexBuffer vertexBuffer, IntPtr gpuDrawDataPtr)
		{
			float num = 0f;
			float num2 = 1f / (this._segmentBuffer.Length[this._segmentBufferStartIndex + 1] + this._trailTailLength);
			uint num3 = (uint)((uint)this._trailSettings.RenderMode << (FXVertex.ConfigBitShiftBlendMode & 31));
			num3 |= (this.IsFirstPerson ? 1U : 0U) << FXVertex.ConfigBitShiftIsFirstPerson;
			num3 |= (uint)((uint)this._drawId << FXVertex.ConfigBitShiftDrawId);
			vertexBuffer.SetVertexDataConfig(this._particleVertexDataStartIndex, num3);
			vertexBuffer.SetTrailFirstSegmentVertexPosition(this._particleVertexDataStartIndex, Vector3.Zero, this._segmentBuffer.Rotation[this._segmentBufferStartIndex], MathHelper.Lerp(this._startWidth, this._endWidth, num * num2));
			vertexBuffer.SetTrailFirstSegmentVertexLength(this._particleVertexDataStartIndex, 0f);
			for (int i = 1; i < this._segmentCount - 1; i++)
			{
				int num4 = (i <= this._lastSegment) ? i : this._lastSegment;
				bool flag = i <= this._lastSegment;
				if (flag)
				{
					num += this._segmentBuffer.Length[this._segmentBufferStartIndex + num4];
				}
				vertexBuffer.SetTrailSegmentVertexPosition(this._particleVertexDataStartIndex + i, this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + num4] - this.Position, this._segmentBuffer.Rotation[this._segmentBufferStartIndex + num4], MathHelper.Lerp(this._startWidth, this._endWidth, num * num2));
				vertexBuffer.SetTrailSegmentVertexLength(this._particleVertexDataStartIndex + i, num * num2);
				vertexBuffer.SetVertexDataConfig(this._particleVertexDataStartIndex + i, num3);
			}
			vertexBuffer.SetTrailLastSegmentVertexPosition(this._particleVertexDataStartIndex + this._segmentCount, this._segmentBuffer.TrailPosition[this._segmentBufferStartIndex + this._lastSegment] - this.Position, this._segmentBuffer.Rotation[this._segmentBufferStartIndex + this._lastSegment], MathHelper.Lerp(this._startWidth, this._endWidth, num * num2));
			vertexBuffer.SetTrailLastSegmentVertexLength(this._particleVertexDataStartIndex + this._segmentCount, 1f);
			bool isFirstPerson = this.IsFirstPerson;
			Matrix matrix;
			if (isFirstPerson)
			{
				Matrix.CreateTranslation(ref this.Position, out matrix);
			}
			else
			{
				Vector3 vector = this.Position - cameraPosition;
				Matrix.CreateTranslation(ref vector, out matrix);
			}
			IntPtr pointer = IntPtr.Add(gpuDrawDataPtr, (int)this._drawId * FXRenderer.DrawDataSize);
			Matrix* ptr = (Matrix*)pointer.ToPointer();
			*ptr = matrix;
			Vector4* ptr2 = (Vector4*)IntPtr.Add(pointer, sizeof(Matrix)).ToPointer();
			*ptr2 = this._staticLightColorAndInfluence;
			ptr2[1] = this._trailSettings.Start.Color;
			ptr2[2] = this._textureCoords;
			ptr2[3] = new Vector4(0f, 0f, 0f, 1f);
			ptr2[4] = this._trailSettings.End.Color;
			ptr2[5] = this._intersectionHighlight;
			ptr2[6] = Vector4.Zero;
			ptr2[7] = Vector4.Zero;
			this._wasFirstPerson = this.IsFirstPerson;
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x00199894 File Offset: 0x00197A94
		private void Release()
		{
			bool flag = this._segmentCount > 0;
			if (flag)
			{
				this._trailFXSystem.ReleaseSegmentBufferStorage(this._segmentBufferStartIndex, this._segmentCount);
			}
		}

		// Token: 0x040032CA RID: 13002
		private const int DefaultLifeSpan = 10;

		// Token: 0x040032CB RID: 13003
		private const int MaxNewSegments = 5;

		// Token: 0x040032CC RID: 13004
		private const float SmoothStep = 0.25f;

		// Token: 0x040032CD RID: 13005
		private const int MaxLifeSpan = 200;

		// Token: 0x040032CE RID: 13006
		private const int MaxSmoothLifeSpan = 40;

		// Token: 0x040032CF RID: 13007
		private GraphicsDevice _graphics;

		// Token: 0x040032D0 RID: 13008
		private TrailFXSystem _trailFXSystem;

		// Token: 0x040032D1 RID: 13009
		private readonly TrailSettings _trailSettings;

		// Token: 0x040032D2 RID: 13010
		public readonly int Id;

		// Token: 0x040032D3 RID: 13011
		public Vector3 Position;

		// Token: 0x040032D4 RID: 13012
		public Quaternion Rotation = Quaternion.Identity;

		// Token: 0x040032D5 RID: 13013
		private Vector3 _lastPosition;

		// Token: 0x040032D6 RID: 13014
		private Vector4 _intersectionHighlight;

		// Token: 0x040032D7 RID: 13015
		private int _segmentBufferStartIndex;

		// Token: 0x040032D8 RID: 13016
		private int _segmentCount;

		// Token: 0x040032D9 RID: 13017
		private readonly SegmentBuffers _segmentBuffer;

		// Token: 0x040032DA RID: 13018
		private ushort _drawId;

		// Token: 0x040032DB RID: 13019
		private int _particleVertexDataStartIndex;

		// Token: 0x040032DC RID: 13020
		private int _lastSegment;

		// Token: 0x040032DD RID: 13021
		private float _trailTailLength = 0f;

		// Token: 0x040032DE RID: 13022
		private Vector4 _staticLightColorAndInfluence;

		// Token: 0x040032DF RID: 13023
		private float _startWidth = 1f;

		// Token: 0x040032E0 RID: 13024
		private float _endWidth = 1f;

		// Token: 0x040032E2 RID: 13026
		public bool IsExpired = false;

		// Token: 0x040032E3 RID: 13027
		private bool _hasAnimation = false;

		// Token: 0x040032E4 RID: 13028
		private Vector2 _textureAltasInverseSize;

		// Token: 0x040032E5 RID: 13029
		private Point _frameSize;

		// Token: 0x040032E6 RID: 13030
		private int _tilesPerRow;

		// Token: 0x040032E7 RID: 13031
		private int _targetTextureIndex = 0;

		// Token: 0x040032E8 RID: 13032
		private int _frameTimer = 0;

		// Token: 0x040032E9 RID: 13033
		private Vector4 _textureCoords;

		// Token: 0x040032EA RID: 13034
		private Vector3[] _lastRealPositions = new Vector3[2];

		// Token: 0x040032EB RID: 13035
		public bool Visible;

		// Token: 0x040032EC RID: 13036
		public bool IsFirstPerson = false;

		// Token: 0x040032ED RID: 13037
		private bool _wasFirstPerson = false;

		// Token: 0x040032EE RID: 13038
		private int _lifeSpan;
	}
}
