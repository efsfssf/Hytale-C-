using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A97 RID: 2711
	internal class BoxEditorGizmo : Disposable
	{
		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06005551 RID: 21841 RVA: 0x0018FE26 File Offset: 0x0018E026
		// (set) Token: 0x06005552 RID: 21842 RVA: 0x0018FE2E File Offset: 0x0018E02E
		public bool Visible { get; private set; }

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06005553 RID: 21843 RVA: 0x0018FE37 File Offset: 0x0018E037
		// (set) Token: 0x06005554 RID: 21844 RVA: 0x0018FE3F File Offset: 0x0018E03F
		public bool IsCursorOverSelection { get; private set; }

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06005555 RID: 21845 RVA: 0x0018FE48 File Offset: 0x0018E048
		// (set) Token: 0x06005556 RID: 21846 RVA: 0x0018FE50 File Offset: 0x0018E050
		public bool IsResizing { get; private set; }

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06005557 RID: 21847 RVA: 0x0018FE59 File Offset: 0x0018E059
		// (set) Token: 0x06005558 RID: 21848 RVA: 0x0018FE61 File Offset: 0x0018E061
		public bool IsMoving { get; private set; }

		// Token: 0x06005559 RID: 21849 RVA: 0x0018FE6A File Offset: 0x0018E06A
		public bool NeedsDrawing()
		{
			return this.Visible;
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x0018FE74 File Offset: 0x0018E074
		public BoxEditorGizmo(GraphicsDevice graphics, BoxEditorGizmo.OnBoxChange onChange)
		{
			this._graphics = graphics;
			this._onBoxChange = onChange;
			this._boxRenderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._quadRenderer = new QuadRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram.AttribPosition, this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x0018FF35 File Offset: 0x0018E135
		protected override void DoDispose()
		{
			this._boxRenderer.Dispose();
			this._quadRenderer.Dispose();
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x0018FF50 File Offset: 0x0018E150
		public void Draw(ref Matrix viewProjectionMatrix, Vector3 positionOffset, Vector3 color)
		{
			bool flag = !this.NeedsDrawing();
			if (!flag)
			{
				this._graphics.GPUProgramStore.BasicProgram.AssertInUse();
				this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
				this._boxRenderer.Draw(this._origin, this._box, viewProjectionMatrix, color, 0.5f, color, 0.1f);
				bool flag2 = !this.IsCursorOverSelection && !this.IsResizing && !this.IsMoving;
				if (!flag2)
				{
					Vector3 size = this.GetSize();
					Vector3 vector = this.GetBounds().Min + size * 0.5f + positionOffset;
					bool flag3 = this._normal.Y != 0f;
					if (flag3)
					{
						vector += new Vector3(-size.X / 2f, size.Y / 2f * this._normal.Y, -size.Z / 2f);
						Matrix.CreateScale(size.X, size.Z, 1f, out this._matrix);
						Matrix.CreateRotationX(1.5707964f, out this._tempMatrix);
						Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
					}
					else
					{
						bool flag4 = this._normal.X != 0f;
						if (flag4)
						{
							vector += new Vector3(size.X / 2f * this._normal.X, -size.Y / 2f, -size.Z / 2f);
							Matrix.CreateScale(size.Z, size.Y, 1f, out this._matrix);
							Matrix.CreateRotationY(-1.5707964f, out this._tempMatrix);
							Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
						}
						else
						{
							bool flag5 = this._normal.Z != 0f;
							if (flag5)
							{
								vector += new Vector3(-size.X / 2f, -size.Y / 2f, size.Z / 2f * this._normal.Z);
								Matrix.CreateScale(size.X, size.Y, 1f, out this._matrix);
							}
						}
					}
					Matrix.CreateTranslation(vector.X, vector.Y, vector.Z, out this._tempMatrix);
					Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
					Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
					BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
					basicProgram.MVPMatrix.SetValue(ref this._matrix);
					basicProgram.Color.SetValue(color);
					basicProgram.Opacity.SetValue(0.4f);
					this._quadRenderer.Draw();
				}
			}
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x00190284 File Offset: 0x0018E484
		public void Tick(Ray viewRay, bool altDown = false)
		{
			bool flag = !this.Visible;
			if (!flag)
			{
				BoundingBox box = this._box;
				bool flag2 = HitDetection.CheckRayBoxCollision(this.GetBounds(), viewRay.Position, viewRay.Direction, out this._rayBoxHit, false);
				if (flag2)
				{
					this.IsCursorOverSelection = true;
					bool flag3 = !this.IsResizing && !this.IsMoving;
					if (flag3)
					{
						this._normal = this._rayBoxHit.Normal;
					}
				}
				else
				{
					this.IsCursorOverSelection = false;
				}
				bool isResizing = this.IsResizing;
				if (isResizing)
				{
					this.OnResize(viewRay, !altDown);
				}
				else
				{
					bool isMoving = this.IsMoving;
					if (isMoving)
					{
						this.OnMove(viewRay, !altDown);
					}
				}
			}
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x0019033C File Offset: 0x0018E53C
		public void Show(Vector3 origin, BoundingBox box, Vector3[] snapValues = null)
		{
			this._origin = origin;
			this._box = box;
			this._position1 = this._box.Min + origin;
			this._position2 = this._box.Max + origin;
			this._localSnapValues = (snapValues ?? new Vector3[0]);
			this.Visible = true;
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x0019039F File Offset: 0x0018E59F
		public void Hide()
		{
			this.Visible = false;
			this.IsResizing = false;
			this.IsMoving = false;
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x001903BC File Offset: 0x0018E5BC
		public bool InUse()
		{
			return this.IsMoving || this.IsResizing;
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x001903E0 File Offset: 0x0018E5E0
		public void ResetBox()
		{
			this._position1 = this._startBox.Min + this._origin;
			this._position2 = this._startBox.Max + this._origin;
			this.OnChange();
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x00190430 File Offset: 0x0018E630
		public void OnInteract(InteractionType interactionType, Ray viewRay, bool shiftDown, bool altDown)
		{
			bool flag = this.IsResizing || this.IsMoving;
			if (flag)
			{
				this.IsResizing = false;
				this.IsMoving = false;
				bool flag2 = interactionType == 1;
				if (flag2)
				{
					this.ResetBox();
				}
			}
			else
			{
				bool flag3 = interactionType == 0;
				if (flag3)
				{
					this._startBox = this._box;
					if (shiftDown)
					{
						this.IsMoving = true;
						this._resizeStart = this._rayBoxHit.Position;
						this._normal = this._rayBoxHit.Normal;
					}
					else
					{
						this.IsResizing = true;
						this._resizeStart = this._rayBoxHit.Position;
						this._normal = this._rayBoxHit.Normal;
					}
				}
				else
				{
					this.Hide();
				}
			}
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x001904F8 File Offset: 0x0018E6F8
		private void OnResize(Ray viewRay, bool useValueSnap)
		{
			Vector3 projectedCursorPosition = this.GetProjectedCursorPosition(viewRay);
			float num = (this._normal == Vector3.Up || this._normal == Vector3.Down) ? projectedCursorPosition.Y : ((this._normal == Vector3.Left || this._normal == Vector3.Right) ? projectedCursorPosition.X : projectedCursorPosition.Z);
			float[] array = new float[this._localSnapValues.Length];
			for (int i = 0; i < this._localSnapValues.Length; i++)
			{
				bool flag = this._normal == Vector3.Up || this._normal == Vector3.Down;
				if (flag)
				{
					array[i] = this._origin.Y + this._localSnapValues[i].Y;
				}
				else
				{
					bool flag2 = this._normal == Vector3.Left || this._normal == Vector3.Right;
					if (flag2)
					{
						array[i] = this._origin.X + this._localSnapValues[i].X;
					}
					else
					{
						array[i] = this._origin.Z + this._localSnapValues[i].Z;
					}
				}
			}
			float gridSnapValue = BoxEditorGizmo.GetGridSnapValue(num, useValueSnap ? this.MaxGridSnapDistance : this.MinGridSnapDistance);
			float localSnapValue = BoxEditorGizmo.GetLocalSnapValue(num, this.MaxLocalSnapDistance, array);
			num = ((useValueSnap && !float.IsInfinity(localSnapValue)) ? localSnapValue : gridSnapValue);
			bool flag3 = this._normal == Vector3.Up;
			if (flag3)
			{
				bool flag4 = this._position1.Y > this._position2.Y;
				if (flag4)
				{
					this._position1.Y = num;
				}
				else
				{
					this._position2.Y = num;
				}
			}
			else
			{
				bool flag5 = this._normal == Vector3.Down;
				if (flag5)
				{
					bool flag6 = this._position1.Y < this._position2.Y;
					if (flag6)
					{
						this._position1.Y = num;
					}
					else
					{
						this._position2.Y = num;
					}
				}
				else
				{
					bool flag7 = this._normal == Vector3.Left;
					if (flag7)
					{
						bool flag8 = this._position1.X < this._position2.X;
						if (flag8)
						{
							this._position1.X = num;
						}
						else
						{
							this._position2.X = num;
						}
					}
					else
					{
						bool flag9 = this._normal == Vector3.Right;
						if (flag9)
						{
							bool flag10 = this._position1.X > this._position2.X;
							if (flag10)
							{
								this._position1.X = num;
							}
							else
							{
								this._position2.X = num;
							}
						}
						else
						{
							bool flag11 = this._normal == Vector3.Forward;
							if (flag11)
							{
								bool flag12 = this._position1.Z > this._position2.Z;
								if (flag12)
								{
									this._position2.Z = num;
								}
								else
								{
									this._position1.Z = num;
								}
							}
							else
							{
								bool flag13 = this._normal == Vector3.Backward;
								if (flag13)
								{
									bool flag14 = this._position1.Z < this._position2.Z;
									if (flag14)
									{
										this._position2.Z = num;
									}
									else
									{
										this._position1.Z = num;
									}
								}
							}
						}
					}
				}
			}
			this.OnChange();
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x00190898 File Offset: 0x0018EA98
		private void OnMove(Ray viewRay, bool useValueSnap)
		{
			Vector3 size = this.GetSize();
			Vector3 projectedCursorPosition = this.GetProjectedCursorPosition(viewRay);
			float num = (this._normal == Vector3.Up || this._normal == Vector3.Down) ? projectedCursorPosition.Y : ((this._normal == Vector3.Left || this._normal == Vector3.Right) ? projectedCursorPosition.X : projectedCursorPosition.Z);
			float[] array = new float[this._localSnapValues.Length];
			for (int i = 0; i < this._localSnapValues.Length; i++)
			{
				bool flag = this._normal == Vector3.Up || this._normal == Vector3.Down;
				if (flag)
				{
					array[i] = this._origin.Y + this._localSnapValues[i].Y;
				}
				else
				{
					bool flag2 = this._normal == Vector3.Left || this._normal == Vector3.Right;
					if (flag2)
					{
						array[i] = this._origin.X + this._localSnapValues[i].X;
					}
					else
					{
						array[i] = this._origin.Z + this._localSnapValues[i].Z;
					}
				}
			}
			float gridSnapValue = BoxEditorGizmo.GetGridSnapValue(num, useValueSnap ? this.MaxGridSnapDistance : this.MinGridSnapDistance);
			float localSnapValue = BoxEditorGizmo.GetLocalSnapValue(num, this.MaxLocalSnapDistance, array);
			num = ((useValueSnap && !float.IsInfinity(localSnapValue)) ? localSnapValue : gridSnapValue);
			bool flag3 = this._normal == Vector3.Up;
			if (flag3)
			{
				bool flag4 = this._position1.Y > this._position2.Y;
				if (flag4)
				{
					this._position1.Y = num;
					this._position2.Y = this._position1.Y - size.Y;
				}
				else
				{
					this._position2.Y = num;
					this._position1.Y = this._position2.Y - size.Y;
				}
			}
			else
			{
				bool flag5 = this._normal == Vector3.Down;
				if (flag5)
				{
					bool flag6 = this._position1.Y < this._position2.Y;
					if (flag6)
					{
						this._position1.Y = num;
						this._position2.Y = this._position1.Y + size.Y;
					}
					else
					{
						this._position2.Y = num;
						this._position1.Y = this._position2.Y - size.Y;
					}
				}
				else
				{
					bool flag7 = this._normal == Vector3.Left;
					if (flag7)
					{
						bool flag8 = this._position1.X < this._position2.X;
						if (flag8)
						{
							this._position1.X = num;
							this._position2.X = this._position1.X + size.X;
						}
						else
						{
							this._position2.X = num;
							this._position1.X = this._position2.X - size.X;
						}
					}
					else
					{
						bool flag9 = this._normal == Vector3.Right;
						if (flag9)
						{
							bool flag10 = this._position1.X > this._position2.X;
							if (flag10)
							{
								this._position1.X = num;
								this._position2.X = this._position1.X - size.X;
							}
							else
							{
								this._position2.X = num;
								this._position1.X = this._position2.X - size.X;
							}
						}
						else
						{
							bool flag11 = this._normal == Vector3.Forward;
							if (flag11)
							{
								bool flag12 = this._position1.Z > this._position2.Z;
								if (flag12)
								{
									this._position2.Z = num;
									this._position1.Z = this._position2.Z + size.Z;
								}
								else
								{
									this._position1.Z = num;
									this._position2.Z = this._position1.Z - size.Z;
								}
							}
							else
							{
								bool flag13 = this._normal == Vector3.Backward;
								if (flag13)
								{
									bool flag14 = this._position1.Z < this._position2.Z;
									if (flag14)
									{
										this._position2.Z = num;
										this._position1.Z = this._position2.Z - size.Z;
									}
									else
									{
										this._position1.Z = num;
										this._position2.Z = this._position1.Z - size.Z;
									}
								}
							}
						}
					}
				}
			}
			this.OnChange();
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x00190DB8 File Offset: 0x0018EFB8
		private void OnChange()
		{
			this._box = new BoundingBox(Vector3.Min(this._position1, this._position2), Vector3.Max(this._position1, this._position2));
			this._box.Translate(-this._origin);
			this._box.Min.X = (float)Math.Round((double)this._box.Min.X, 2);
			this._box.Min.Y = (float)Math.Round((double)this._box.Min.Y, 2);
			this._box.Min.Z = (float)Math.Round((double)this._box.Min.Z, 2);
			this._box.Max.X = (float)Math.Round((double)this._box.Max.X, 2);
			this._box.Max.Y = (float)Math.Round((double)this._box.Max.Y, 2);
			this._box.Max.Z = (float)Math.Round((double)this._box.Max.Z, 2);
			BoxEditorGizmo.OnBoxChange onBoxChange = this._onBoxChange;
			if (onBoxChange != null)
			{
				onBoxChange(this._box);
			}
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x00190F14 File Offset: 0x0018F114
		private Vector3 GetSize()
		{
			bool flag = !this.Visible;
			Vector3 result;
			if (flag)
			{
				result = Vector3.Zero;
			}
			else
			{
				result = Vector3.Max(this._position1, this._position2) - Vector3.Min(this._position1, this._position2);
			}
			return result;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x00190F64 File Offset: 0x0018F164
		private Vector3 GetProjectedCursorPosition(Ray viewRay)
		{
			Vector3 vector = this._resizeStart - viewRay.Position;
			bool flag = this._normal == Vector3.Up || this._normal == Vector3.Down;
			Vector2 ray1Position;
			Vector2 ray1Direction;
			Vector2 ray2Position;
			Vector2 ray2Direction;
			if (flag)
			{
				float num = vector.X;
				vector.X = vector.Z;
				vector.Z = -num;
				ray1Position = new Vector2(viewRay.Position.X, viewRay.Position.Z);
				ray1Direction = new Vector2(viewRay.Direction.X, viewRay.Direction.Z);
				ray2Position = new Vector2(this._resizeStart.X, this._resizeStart.Z);
				ray2Direction = new Vector2(vector.X, vector.Z);
			}
			else
			{
				bool flag2 = this._normal == Vector3.Left || this._normal == Vector3.Right;
				if (flag2)
				{
					float num = vector.Y;
					vector.Y = vector.Z;
					vector.Z = -num;
					ray1Position = new Vector2(viewRay.Position.Y, viewRay.Position.Z);
					ray1Direction = new Vector2(viewRay.Direction.Y, viewRay.Direction.Z);
					ray2Position = new Vector2(this._resizeStart.Y, this._resizeStart.Z);
					ray2Direction = new Vector2(vector.Y, vector.Z);
				}
				else
				{
					float num = vector.X;
					vector.X = vector.Y;
					vector.Y = -num;
					ray1Position = new Vector2(viewRay.Position.X, viewRay.Position.Y);
					ray1Direction = new Vector2(viewRay.Direction.X, viewRay.Direction.Y);
					ray2Position = new Vector2(this._resizeStart.X, this._resizeStart.Y);
					ray2Direction = new Vector2(vector.X, vector.Y);
				}
			}
			Vector2 vector2;
			bool flag3 = HitDetection.Get2DRayIntersection(ray1Position, ray1Direction, ray2Position, ray2Direction, out vector2);
			Vector3 result;
			if (flag3)
			{
				bool flag4 = this._normal == Vector3.Up || this._normal == Vector3.Down;
				if (flag4)
				{
					float num2 = (vector2.X - viewRay.Position.X) / viewRay.Direction.X;
					result = new Vector3(vector2.X, num2 * viewRay.Direction.Y + viewRay.Position.Y, vector2.Y);
				}
				else
				{
					bool flag5 = this._normal == Vector3.Left || this._normal == Vector3.Right;
					if (flag5)
					{
						float num2 = (vector2.Y - viewRay.Position.Z) / viewRay.Direction.Z;
						result = new Vector3(num2 * viewRay.Direction.X + viewRay.Position.X, vector2.X, vector2.Y);
					}
					else
					{
						float num2 = (vector2.X - viewRay.Position.X) / viewRay.Direction.X;
						result = new Vector3(vector2.X, vector2.Y, num2 * viewRay.Direction.Z + viewRay.Position.Z);
					}
				}
			}
			else
			{
				result = viewRay.Position;
			}
			return result;
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x0019130C File Offset: 0x0018F50C
		private BoundingBox GetBounds()
		{
			BoundingBox box = this._box;
			box.Translate(this._origin);
			return box;
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x00191334 File Offset: 0x0018F534
		private static float GetGridSnapValue(float value, float increment)
		{
			float num = value % increment;
			bool flag = num == 0f;
			float result;
			if (flag)
			{
				result = value;
			}
			else
			{
				value -= num;
				bool flag2 = num * 2f >= increment;
				if (flag2)
				{
					value += increment;
				}
				else
				{
					bool flag3 = num * 2f < -increment;
					if (flag3)
					{
						value -= increment;
					}
				}
				result = value;
			}
			return result;
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x00191390 File Offset: 0x0018F590
		private static float GetLocalSnapValue(float value, float maxSnapDistance, float[] snapValues)
		{
			float result = float.PositiveInfinity;
			float num = float.NaN;
			for (int i = 0; i < snapValues.Length; i++)
			{
				float num2 = Math.Abs(snapValues[i] - value);
				bool flag = num2 <= maxSnapDistance && (num2 < num || float.IsNaN(num));
				if (flag)
				{
					result = snapValues[i];
					num = num2;
				}
			}
			return result;
		}

		// Token: 0x04003231 RID: 12849
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003232 RID: 12850
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x04003233 RID: 12851
		private readonly QuadRenderer _quadRenderer;

		// Token: 0x04003238 RID: 12856
		private BoundingBox _startBox;

		// Token: 0x04003239 RID: 12857
		private BoundingBox _box;

		// Token: 0x0400323A RID: 12858
		private BoxEditorGizmo.OnBoxChange _onBoxChange;

		// Token: 0x0400323B RID: 12859
		private HitDetection.RayBoxCollision _rayBoxHit;

		// Token: 0x0400323C RID: 12860
		private Vector3 _origin = Vector3.Zero;

		// Token: 0x0400323D RID: 12861
		private Vector3 _position1 = Vector3.NaN;

		// Token: 0x0400323E RID: 12862
		private Vector3 _position2 = Vector3.NaN;

		// Token: 0x0400323F RID: 12863
		private Vector3 _normal;

		// Token: 0x04003240 RID: 12864
		private Vector3 _resizeStart;

		// Token: 0x04003241 RID: 12865
		private Vector3[] _localSnapValues;

		// Token: 0x04003242 RID: 12866
		private Matrix _tempMatrix;

		// Token: 0x04003243 RID: 12867
		private Matrix _matrix;

		// Token: 0x04003244 RID: 12868
		public float MinGridSnapDistance = 0.01f;

		// Token: 0x04003245 RID: 12869
		public float MaxGridSnapDistance = 0.05f;

		// Token: 0x04003246 RID: 12870
		public float MaxLocalSnapDistance = 0.1f;

		// Token: 0x02000EE7 RID: 3815
		// (Invoke) Token: 0x06006807 RID: 26631
		public delegate void OnBoxChange(BoundingBox boundingBox);
	}
}
