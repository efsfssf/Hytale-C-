using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Collision;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;

namespace HytaleClient.Data.ClientInteraction.Selector
{
	// Token: 0x02000B1A RID: 2842
	internal class HitDetectionExecutor
	{
		// Token: 0x060058C6 RID: 22726 RVA: 0x001B23EE File Offset: 0x001B05EE
		public HitDetectionExecutor(Random random)
		{
			this._random = random;
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x001B241D File Offset: 0x001B061D
		public void SetOrigin(Vector3 origin)
		{
			this._origin = new Vector4(origin);
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x001B242C File Offset: 0x001B062C
		public Vector4 GetHitLocation()
		{
			return this._buffer.HitPosition;
		}

		// Token: 0x060058C9 RID: 22729 RVA: 0x001B244C File Offset: 0x001B064C
		public bool Test(GameInstance gameInstance, Quad4[] model, Matrix modelMatrix)
		{
			this.SetupMatrices(modelMatrix);
			return this.TestModel(gameInstance, model);
		}

		// Token: 0x060058CA RID: 22730 RVA: 0x001B246E File Offset: 0x001B066E
		private void SetupMatrices(Matrix modelMatrix)
		{
			this._pvmMatrix = this.ViewMatrix * this.ProjectionMatrix;
			this._invPvMatrix = Matrix.Invert(this._pvmMatrix);
			this._pvmMatrix = modelMatrix * this._pvmMatrix;
		}

		// Token: 0x060058CB RID: 22731 RVA: 0x001B24AC File Offset: 0x001B06AC
		private bool TestModel(GameInstance gameInstance, Quad4[] model)
		{
			int num = 0;
			double num2 = double.PositiveInfinity;
			int i = 0;
			while (i < model.Length)
			{
				Quad4 quad = model[i];
				bool flag = num++ == this._maxRayTests;
				if (flag)
				{
					return false;
				}
				this._buffer.TransformedQuad = quad.Multiply(this._pvmMatrix);
				bool flag2 = this.InsideFrustum();
				if (flag2)
				{
					bool containsFully = this._buffer.ContainsFully;
					Vector4 vector;
					if (containsFully)
					{
						vector = this._buffer.TransformedQuad.GetRandom(this._random);
					}
					else
					{
						vector = this._buffer.VisibleTriangle.GetRandom(this._random);
					}
					vector = Vector4.Transform(vector, this._invPvMatrix).PerspectiveTransform();
					double num3 = (double)(this._origin.X - vector.X);
					double num4 = (double)(this._origin.Y - vector.Y);
					double num5 = (double)(this._origin.Z - vector.Z);
					double num6 = num3 * num3 + num4 * num4 + num5 * num5;
					bool flag3 = num6 >= num2;
					if (!flag3)
					{
						bool flag4 = this.LosProvider(gameInstance, this._origin.X, this._origin.Y, this._origin.Z, vector.X, vector.Y, vector.Z);
						if (flag4)
						{
							num2 = num6;
							this._buffer.HitPosition = vector;
						}
					}
				}
				IL_186:
				i++;
				continue;
				goto IL_186;
			}
			return !double.IsPositiveInfinity(num2);
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x001B265C File Offset: 0x001B085C
		private bool InsideFrustum()
		{
			Quad4 transformedQuad = this._buffer.TransformedQuad;
			bool flag = transformedQuad.IsFullyInsideFrustum();
			bool result;
			if (flag)
			{
				this._buffer.ContainsFully = true;
				result = true;
			}
			else
			{
				this._buffer.ContainsFully = false;
				List<Vector4> list = new List<Vector4>();
				list.Add(transformedQuad.A);
				list.Add(transformedQuad.B);
				list.Add(transformedQuad.C);
				list.Add(transformedQuad.D);
				bool flag2 = this.ClipPolygonAxis(list, 0) && this.ClipPolygonAxis(list, 1) && this.ClipPolygonAxis(list, 2);
				if (flag2)
				{
					Vector4 a = list[0];
					this._buffer.VisibleTriangle = new Triangle4(a, list[1], list[2]);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x001B2734 File Offset: 0x001B0934
		private bool ClipPolygonAxis(List<Vector4> vertices, int componentIndex)
		{
			List<Vector4> list = this.ClipPolygonComponent(vertices, componentIndex, 1f);
			vertices.Clear();
			bool flag = list.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<Vector4> list2 = this.ClipPolygonComponent(list, componentIndex, -1f);
				vertices.AddRange(list2);
				result = (list2.Count != 0);
			}
			return result;
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x001B278C File Offset: 0x001B098C
		private List<Vector4> ClipPolygonComponent(List<Vector4> vertices, int componentIndex, float componentFactor)
		{
			List<Vector4> list = new List<Vector4>();
			Vector4 vector = vertices[vertices.Count - 1];
			float num = vector.Get(componentIndex) * componentFactor;
			bool flag = num <= vector.W;
			foreach (Vector4 vector2 in vertices)
			{
				float num2 = vector2.Get(componentIndex) * componentFactor;
				bool flag2 = num2 <= vector2.W;
				bool flag3 = flag2 ^ flag;
				if (flag3)
				{
					float amount = (vector.W - num) / (vector.W - num - (vector2.W - num2));
					list.Add(Vector4.Lerp(vector, vector2, amount));
				}
				bool flag4 = flag2;
				if (flag4)
				{
					list.Add(vector2);
				}
				vector = vector2;
				num = num2;
				flag = flag2;
			}
			return list;
		}

		// Token: 0x04003737 RID: 14135
		public static LineOfSightProvider DefaultLineOfSightTrue = (GameInstance gameInstance, float x, float y, float z, float toX, float toY, float toZ) => true;

		// Token: 0x04003738 RID: 14136
		public static LineOfSightProvider DefaultLineOfSightSolid = delegate(GameInstance gameInstance, float x, float y, float z, float toX, float toY, float toZ)
		{
			Vector3 vector = new Vector3(x, y, z);
			Vector3 direction = new Vector3(toX, toY, toZ) - vector;
			float maxDistance = direction.Length();
			BlockIterator blockIterator = new BlockIterator(vector, direction, maxDistance);
			BlockAccessor blockAccessor = new BlockAccessor(gameInstance.MapModule);
			while (blockIterator.HasNext())
			{
				IntVector3 block;
				Vector3 vector2;
				Vector3 vector3;
				IntVector3 intVector;
				blockIterator.Step(out block, out vector2, out vector3, out intVector);
				ClientBlockType blockTypeFiller = blockAccessor.GetBlockTypeFiller(block);
				bool flag = blockTypeFiller.CollisionMaterial == 1;
				if (flag)
				{
					return false;
				}
			}
			return true;
		};

		// Token: 0x04003739 RID: 14137
		private static Vector4[] VERTEX_POINTS = new Vector4[]
		{
			new Vector4(new Vector3(0f, 1f, 1f), 1f),
			new Vector4(new Vector3(0f, 1f, 0f), 1f),
			new Vector4(new Vector3(1f, 1f, 1f), 1f),
			new Vector4(new Vector3(1f, 1f, 0f), 1f),
			new Vector4(new Vector3(0f, 0f, 1f), 1f),
			new Vector4(new Vector3(0f, 0f, 0f), 1f),
			new Vector4(new Vector3(1f, 0f, 1f), 1f),
			new Vector4(new Vector3(1f, 0f, 0f), 1f)
		};

		// Token: 0x0400373A RID: 14138
		public static Quad4[] CUBE_QUADS = new Quad4[]
		{
			new Quad4(HitDetectionExecutor.VERTEX_POINTS, 0, 1, 3, 2),
			new Quad4(HitDetectionExecutor.VERTEX_POINTS, 0, 4, 5, 1),
			new Quad4(HitDetectionExecutor.VERTEX_POINTS, 4, 5, 7, 6),
			new Quad4(HitDetectionExecutor.VERTEX_POINTS, 2, 3, 7, 6),
			new Quad4(HitDetectionExecutor.VERTEX_POINTS, 1, 3, 7, 5),
			new Quad4(HitDetectionExecutor.VERTEX_POINTS, 0, 2, 6, 4)
		};

		// Token: 0x0400373B RID: 14139
		private Matrix _pvmMatrix;

		// Token: 0x0400373C RID: 14140
		private Matrix _invPvMatrix;

		// Token: 0x0400373D RID: 14141
		private Vector4 _origin;

		// Token: 0x0400373E RID: 14142
		private readonly HitDetectionBuffer _buffer = new HitDetectionBuffer();

		// Token: 0x0400373F RID: 14143
		public Matrix ProjectionMatrix;

		// Token: 0x04003740 RID: 14144
		public Matrix ViewMatrix;

		// Token: 0x04003741 RID: 14145
		public LineOfSightProvider LosProvider = HitDetectionExecutor.DefaultLineOfSightSolid;

		// Token: 0x04003742 RID: 14146
		private int _maxRayTests = 10;

		// Token: 0x04003743 RID: 14147
		private readonly Random _random;
	}
}
