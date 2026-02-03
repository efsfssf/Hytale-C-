using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A9F RID: 2719
	internal class TranslationGizmo : Disposable
	{
		// Token: 0x0600559F RID: 21919 RVA: 0x00195FF4 File Offset: 0x001941F4
		public TranslationGizmo(GraphicsDevice graphics, TranslationGizmo.OnPositionChange onChange)
		{
			this._graphics = graphics;
			this._onChange = onChange;
			this._quadRenderer = new QuadRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram.AttribPosition, this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			this._modelRenderer = new PrimitiveModelRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._lineRenderer = new LineRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				new Vector3(0f, -500f, 0f),
				new Vector3(0f, 500f, 0f)
			});
			bool flag = TranslationGizmo._modelData == null;
			if (flag)
			{
				TranslationGizmo.BuildModelData();
			}
			this._modelRenderer.UpdateModelData(TranslationGizmo._modelData);
			this._size *= 0.45f;
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x0019617F File Offset: 0x0019437F
		protected override void DoDispose()
		{
			this._quadRenderer.Dispose();
			this._modelRenderer.Dispose();
			this._lineRenderer.Dispose();
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x001961A8 File Offset: 0x001943A8
		public void Draw(ref Matrix viewProjectionMatrix, Vector3 renderPositionOffset)
		{
			bool flag = !this.Visible;
			if (!flag)
			{
				GLFunctions gl = this._graphics.GL;
				BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
				gl.BindTexture(GL.TEXTURE_2D, this._graphics.WhitePixelTexture.GLTexture);
				basicProgram.AssertInUse();
				gl.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
				gl.DepthFunc(GL.ALWAYS);
				Vector3 vector = Vector3.One;
				foreach (KeyValuePair<TranslationGizmo.Axis, float> keyValuePair in TranslationGizmo._axisDirections)
				{
					bool flag2 = keyValuePair.Key != TranslationGizmo.Axis.PlaneX && keyValuePair.Key != TranslationGizmo.Axis.PlaneY && keyValuePair.Key != TranslationGizmo.Axis.PlaneZ;
					if (!flag2)
					{
						bool flag3 = this._selectedAxis != TranslationGizmo.Axis.None && this._selectedAxis != keyValuePair.Key;
						if (!flag3)
						{
							basicProgram.Opacity.SetValue((this._highlightedAxis == keyValuePair.Key) ? 0.7f : 0.3f);
							this._tempMatrix = Matrix.CreateTranslation(0f, 0f, 0f);
							this._drawMatrix = Matrix.CreateScale(this._size.X, this._size.Y, this._size.Z);
							Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
							switch (keyValuePair.Key)
							{
							case TranslationGizmo.Axis.PlaneX:
								this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + 1.5707964f, 0f, 0f);
								vector = this._graphics.RedColor;
								break;
							case TranslationGizmo.Axis.PlaneY:
								this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + 3.1415927f, 1.5707964f, 0f);
								vector = this._graphics.GreenColor;
								break;
							case TranslationGizmo.Axis.PlaneZ:
								this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + 3.1415927f, 0f, 0f);
								vector = this._graphics.BlueColor;
								break;
							}
							Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
							Vector3 position = this._position + renderPositionOffset;
							position.Y += 0.01f;
							this._tempMatrix = Matrix.CreateTranslation(position);
							Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
							Matrix.Multiply(ref this._drawMatrix, ref viewProjectionMatrix, out this._drawMatrix);
							basicProgram.MVPMatrix.SetValue(ref this._drawMatrix);
							basicProgram.Color.SetValue(vector);
							this._quadRenderer.Draw();
						}
					}
				}
				foreach (KeyValuePair<TranslationGizmo.Axis, float> keyValuePair2 in TranslationGizmo._axisDirections)
				{
					bool flag4 = keyValuePair2.Key != TranslationGizmo.Axis.X && keyValuePair2.Key != TranslationGizmo.Axis.Y && keyValuePair2.Key != TranslationGizmo.Axis.Z;
					if (!flag4)
					{
						bool flag5 = this._selectedAxis != TranslationGizmo.Axis.None && this._selectedAxis != keyValuePair2.Key;
						if (!flag5)
						{
							float opacity = (this._highlightedAxis == keyValuePair2.Key) ? 0.7f : 0.3f;
							this._tempMatrix = Matrix.CreateTranslation(0f, 0f, 0f);
							this._drawMatrix = Matrix.CreateScale(0.1f, 0.3f, 0.1f);
							Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
							switch (keyValuePair2.Key)
							{
							case TranslationGizmo.Axis.X:
								this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y - 1.5707964f, 1.5707964f, 0f);
								vector = this._graphics.RedColor;
								break;
							case TranslationGizmo.Axis.Y:
								this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + 3.1415927f, 0f, 0f);
								vector = this._graphics.GreenColor;
								break;
							case TranslationGizmo.Axis.Z:
								this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + 3.1415927f, 1.5707964f, 0f);
								vector = this._graphics.BlueColor;
								break;
							}
							Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
							Vector3 position2 = this._position + renderPositionOffset;
							this._tempMatrix = Matrix.CreateTranslation(position2);
							Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
							basicProgram.Color.SetValue(vector);
							this._modelRenderer.Draw(viewProjectionMatrix, this._drawMatrix, vector, opacity, GL.TRIANGLES);
							bool flag6 = this._selectedAxis == keyValuePair2.Key;
							if (flag6)
							{
								Matrix.Multiply(ref this._drawMatrix, ref viewProjectionMatrix, out this._drawMatrix);
								this._lineRenderer.Draw(ref this._drawMatrix, vector, 0.5f);
							}
						}
					}
				}
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			}
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x00196770 File Offset: 0x00194970
		public void Tick(Ray playerViewRay)
		{
			bool flag = !this.Visible;
			if (!flag)
			{
				float num = float.PositiveInfinity;
				this._highlightedAxis = TranslationGizmo.Axis.None;
				foreach (TranslationGizmo.Axis axis in TranslationGizmo._axisDirections.Keys)
				{
					Vector3 vector;
					bool flag2 = this.CheckRayIntersection(playerViewRay, axis, out vector);
					if (flag2)
					{
						bool flag3 = this._selectedAxis != TranslationGizmo.Axis.None && axis != this._selectedAxis;
						if (!flag3)
						{
							float num2 = Vector3.Distance(vector, playerViewRay.Position);
							bool flag4 = num == float.PositiveInfinity || num > num2;
							if (flag4)
							{
								this._highlightedAxis = axis;
								num = num2;
								this._lastHitPosition = vector;
							}
						}
					}
				}
				bool flag5 = this._selectedAxis != TranslationGizmo.Axis.None;
				if (flag5)
				{
					this.UpdatePosition(playerViewRay, this._selectedAxis);
				}
			}
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x00196874 File Offset: 0x00194A74
		public void Show(Vector3 position, Vector3 rotation, TranslationGizmo.OnPositionChange onChange = null)
		{
			this._position = position;
			this._rotation = rotation;
			this.Visible = true;
			bool flag = onChange != null;
			if (flag)
			{
				this._onChange = onChange;
			}
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x001968A6 File Offset: 0x00194AA6
		public void Hide()
		{
			this.Visible = false;
		}

		// Token: 0x060055A5 RID: 21925 RVA: 0x001968B0 File Offset: 0x00194AB0
		public void OnInteract(Ray playerViewRay, InteractionType interactionType)
		{
			long num = DateTime.UtcNow.Ticks / 10000L;
			bool flag = num - this._timeOfLastToolInteraction < 250L;
			if (!flag)
			{
				this._timeOfLastToolInteraction = num;
				bool flag2 = interactionType == 0;
				if (flag2)
				{
					bool flag3 = this._highlightedAxis != TranslationGizmo.Axis.None && this._selectedAxis == TranslationGizmo.Axis.None;
					if (flag3)
					{
						this._selectedAxis = this._highlightedAxis;
						this._lastPosition = this._position;
						this._startPosition = this._lastHitPosition;
						this._startPosition += this._position - this.GetProjectedCursorPosition(playerViewRay, this._selectedAxis);
						this._startPosition = this.GetProjectedCursorPosition(playerViewRay, this._selectedAxis);
						this._startOffset = this._position - this._startPosition;
					}
					else
					{
						bool flag4 = this._selectedAxis != TranslationGizmo.Axis.None;
						if (flag4)
						{
							this._selectedAxis = TranslationGizmo.Axis.None;
						}
					}
				}
				else
				{
					bool flag5 = this._selectedAxis == TranslationGizmo.Axis.None;
					if (flag5)
					{
						this.Visible = false;
					}
					else
					{
						this._position = this._lastPosition;
						this._selectedAxis = TranslationGizmo.Axis.None;
						this._onChange(this._position);
					}
				}
			}
		}

		// Token: 0x060055A6 RID: 21926 RVA: 0x001969F4 File Offset: 0x00194BF4
		private void UpdatePosition(Ray playerViewRay, TranslationGizmo.Axis axis)
		{
			this._position = this.GetProjectedCursorPosition(playerViewRay, axis) + this._startOffset;
			this._onChange(this._position);
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x00196A24 File Offset: 0x00194C24
		private bool CheckRayIntersection(Ray viewRay, TranslationGizmo.Axis axis, out Vector3 intersection)
		{
			Vector3 vector = viewRay.Position - this._position;
			float num = (axis == TranslationGizmo.Axis.PlaneZ) ? 3.1415927f : 1.5707964f;
			bool flag = axis == TranslationGizmo.Axis.PlaneY;
			if (flag)
			{
				Matrix.CreateFromYawPitchRoll(3.1415927f - this._rotation.Y, 0f, 0f, out this._tempMatrix);
				Matrix.CreateFromYawPitchRoll(0f, -num, 0f, out this._drawMatrix);
				Matrix.Multiply(ref this._tempMatrix, ref this._drawMatrix, out this._tempMatrix);
			}
			else
			{
				Matrix.CreateFromYawPitchRoll(-(this._rotation.Y + num), 0f, 0f, out this._tempMatrix);
			}
			Matrix.CreateTranslation(ref vector, out this._drawMatrix);
			Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
			Vector3 direction = Vector3.Transform(viewRay.Direction, this._tempMatrix);
			Ray ray = new Ray(this._drawMatrix.Translation + this._position, direction);
			bool flag2 = axis == TranslationGizmo.Axis.PlaneX || axis == TranslationGizmo.Axis.PlaneY || axis == TranslationGizmo.Axis.PlaneZ;
			if (flag2)
			{
				float scaleFactor = (ray.Direction.Z >= 0f) ? ((ray.Position.Z - this._position.Z) / -ray.Direction.Z) : ((this._position.Z - ray.Position.Z) / ray.Direction.Z);
				Vector3 vector2 = ray.Position + ray.Direction * scaleFactor;
				bool flag3 = vector2.Y >= this._position.Y && vector2.Y <= this._position.Y + this._size.Y && vector2.X >= this._position.X && vector2.X <= this._position.X + this._size.X;
				if (flag3)
				{
					intersection = viewRay.Position + viewRay.Direction * scaleFactor;
					return true;
				}
			}
			else
			{
				bool flag4 = axis == TranslationGizmo.Axis.X;
				BoundingBox box;
				if (flag4)
				{
					box = new BoundingBox(new Vector3(-0.3f, -0.3f, -1.4f), new Vector3(0.3f, 0.3f, -0.5f));
				}
				else
				{
					bool flag5 = axis == TranslationGizmo.Axis.Y;
					if (flag5)
					{
						box = new BoundingBox(new Vector3(-0.3f, 0.5f, -0.3f), new Vector3(0.3f, 1.4f, 0.3f));
					}
					else
					{
						box = new BoundingBox(new Vector3(0.5f, -0.3f, -0.3f), new Vector3(1.4f, 0.3f, 0.3f));
					}
				}
				box.Translate(this._position);
				HitDetection.RayBoxCollision rayBoxCollision;
				bool flag6 = HitDetection.CheckRayBoxCollision(box, ray.Position, ray.Direction, out rayBoxCollision, false);
				if (flag6)
				{
					intersection = rayBoxCollision.Position;
					return true;
				}
			}
			intersection = Vector3.Zero;
			return false;
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x00196D5C File Offset: 0x00194F5C
		private Vector3 GetProjectedCursorPosition(Ray playerViewRay, TranslationGizmo.Axis axis)
		{
			Vector3 vector = this._startPosition - playerViewRay.Position;
			bool flag = axis == TranslationGizmo.Axis.Y;
			Vector2 ray1Position;
			Vector2 ray1Direction;
			Vector2 ray2Position;
			Vector2 ray2Direction;
			if (flag)
			{
				float x = vector.X;
				vector.X = vector.Z;
				vector.Z = -x;
				ray1Position = new Vector2(playerViewRay.Position.X, playerViewRay.Position.Z);
				ray1Direction = new Vector2(playerViewRay.Direction.X, playerViewRay.Direction.Z);
				ray2Position = new Vector2(this._startPosition.X, this._startPosition.Z);
				ray2Direction = new Vector2(vector.X, vector.Z);
			}
			else
			{
				bool flag2 = axis == TranslationGizmo.Axis.PlaneY;
				if (flag2)
				{
					vector = Vector3.Transform(Vector3.Forward, Quaternion.CreateFromYawPitchRoll(this._rotation.Y + 3.1415927f, 0f, 0f));
					ray1Position = new Vector2(playerViewRay.Position.X, playerViewRay.Position.Y);
					ray1Direction = new Vector2(playerViewRay.Direction.X, playerViewRay.Direction.Y);
					ray2Position = new Vector2(this._startPosition.X, this._startPosition.Y);
					ray2Direction = new Vector2(vector.X, vector.Y);
				}
				else
				{
					bool flag3 = axis == TranslationGizmo.Axis.X || axis == TranslationGizmo.Axis.PlaneZ;
					if (flag3)
					{
						vector = Vector3.Transform(Vector3.Forward, Quaternion.CreateFromYawPitchRoll(this._rotation.Y - 1.5707964f, 0f, 0f));
					}
					else
					{
						vector = Vector3.Transform(Vector3.Forward, Quaternion.CreateFromYawPitchRoll(this._rotation.Y + 3.1415927f, 0f, 0f));
					}
					ray1Position = new Vector2(playerViewRay.Position.X, playerViewRay.Position.Z);
					ray1Direction = new Vector2(playerViewRay.Direction.X, playerViewRay.Direction.Z);
					ray2Position = new Vector2(this._startPosition.X, this._startPosition.Z);
					ray2Direction = new Vector2(vector.X, vector.Z);
				}
			}
			Vector2 vector2;
			bool flag4 = HitDetection.Get2DRayIntersection(ray1Position, ray1Direction, ray2Position, ray2Direction, out vector2);
			if (flag4)
			{
				float num = (vector2.X - playerViewRay.Position.X) / playerViewRay.Direction.X;
				switch (axis)
				{
				case TranslationGizmo.Axis.X:
				case TranslationGizmo.Axis.Z:
					return new Vector3(vector2.X, this._startPosition.Y, vector2.Y);
				case TranslationGizmo.Axis.Y:
					return new Vector3(this._startPosition.X, num * playerViewRay.Direction.Y + playerViewRay.Position.Y, this._startPosition.Z);
				case TranslationGizmo.Axis.PlaneX:
				case TranslationGizmo.Axis.PlaneZ:
					return new Vector3(vector2.X, num * playerViewRay.Direction.Y + playerViewRay.Position.Y, vector2.Y);
				case TranslationGizmo.Axis.PlaneY:
					return new Vector3(vector2.X, this._startPosition.Y, num * playerViewRay.Direction.Z + playerViewRay.Position.Z);
				}
			}
			return playerViewRay.Position;
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x001970CC File Offset: 0x001952CC
		public bool InUse()
		{
			return this._selectedAxis != TranslationGizmo.Axis.None;
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x001970EC File Offset: 0x001952EC
		private static void BuildModelData()
		{
			PrimitiveModelData primitiveModelData = CylinderModel.BuildModelData(0.125f, 4f, 8);
			primitiveModelData.OffsetVertices(new Vector3(0f, 2f, 0f));
			PrimitiveModelData primitiveModelData2 = ConeModel.BuildModelData(1f, 1f, 8);
			primitiveModelData2.OffsetVertices(new Vector3(0f, 4.25f, 0f));
			TranslationGizmo._modelData = PrimitiveModelData.CombineData(primitiveModelData2, primitiveModelData);
		}

		// Token: 0x040032AA RID: 12970
		private static PrimitiveModelData _modelData;

		// Token: 0x040032AB RID: 12971
		private readonly GraphicsDevice _graphics;

		// Token: 0x040032AC RID: 12972
		private readonly QuadRenderer _quadRenderer;

		// Token: 0x040032AD RID: 12973
		private readonly PrimitiveModelRenderer _modelRenderer;

		// Token: 0x040032AE RID: 12974
		private readonly LineRenderer _lineRenderer;

		// Token: 0x040032AF RID: 12975
		public bool Visible = false;

		// Token: 0x040032B0 RID: 12976
		private TranslationGizmo.OnPositionChange _onChange;

		// Token: 0x040032B1 RID: 12977
		private Vector3 _position = Vector3.Zero;

		// Token: 0x040032B2 RID: 12978
		private Vector3 _rotation = Vector3.Zero;

		// Token: 0x040032B3 RID: 12979
		private Vector3 _size = Vector3.One;

		// Token: 0x040032B4 RID: 12980
		private Vector3 _lastHitPosition = Vector3.Zero;

		// Token: 0x040032B5 RID: 12981
		private Vector3 _lastPosition = Vector3.Zero;

		// Token: 0x040032B6 RID: 12982
		private Vector3 _startPosition = Vector3.Zero;

		// Token: 0x040032B7 RID: 12983
		private Vector3 _startOffset = Vector3.Zero;

		// Token: 0x040032B8 RID: 12984
		private Matrix _tempMatrix;

		// Token: 0x040032B9 RID: 12985
		private Matrix _drawMatrix;

		// Token: 0x040032BA RID: 12986
		private TranslationGizmo.Axis _highlightedAxis = TranslationGizmo.Axis.None;

		// Token: 0x040032BB RID: 12987
		private TranslationGizmo.Axis _selectedAxis = TranslationGizmo.Axis.None;

		// Token: 0x040032BC RID: 12988
		private static readonly Dictionary<TranslationGizmo.Axis, float> _axisDirections = new Dictionary<TranslationGizmo.Axis, float>
		{
			{
				TranslationGizmo.Axis.X,
				1.5707964f
			},
			{
				TranslationGizmo.Axis.Y,
				1.5707964f
			},
			{
				TranslationGizmo.Axis.Z,
				3.1415927f
			},
			{
				TranslationGizmo.Axis.PlaneX,
				1.5707964f
			},
			{
				TranslationGizmo.Axis.PlaneY,
				1.5707964f
			},
			{
				TranslationGizmo.Axis.PlaneZ,
				3.1415927f
			}
		};

		// Token: 0x040032BD RID: 12989
		private long _timeOfLastToolInteraction = 0L;

		// Token: 0x02000EED RID: 3821
		// (Invoke) Token: 0x0600681E RID: 26654
		public delegate void OnPositionChange(Vector3 position);

		// Token: 0x02000EEE RID: 3822
		private enum Axis
		{
			// Token: 0x04004935 RID: 18741
			X,
			// Token: 0x04004936 RID: 18742
			Y,
			// Token: 0x04004937 RID: 18743
			Z,
			// Token: 0x04004938 RID: 18744
			PlaneX,
			// Token: 0x04004939 RID: 18745
			PlaneY,
			// Token: 0x0400493A RID: 18746
			PlaneZ,
			// Token: 0x0400493B RID: 18747
			None
		}
	}
}
