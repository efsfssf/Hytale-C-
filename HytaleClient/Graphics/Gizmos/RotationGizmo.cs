using System;
using System.Collections.Generic;
using System.IO;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A9D RID: 2717
	internal class RotationGizmo : Disposable
	{
		// Token: 0x06005584 RID: 21892 RVA: 0x00193164 File Offset: 0x00191364
		public unsafe RotationGizmo(GraphicsDevice graphics, Font font, RotationGizmo.OnRotationChange onChange, float snapAngle = 0.2617994f)
		{
			this._graphics = graphics;
			this._font = font;
			this._onChange = onChange;
			this._quadRenderer = new QuadRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram.AttribPosition, this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			this._textRenderer = new TextRenderer(this._graphics, this._font, "Rotation", uint.MaxValue, 4278190080U);
			this._lineRenderer = new LineRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._lineRenderer.UpdateLineData(new Vector3[]
			{
				new Vector3(0f, 0f, -500f),
				new Vector3(0f, 0f, 500f)
			});
			GLFunctions gl = this._graphics.GL;
			this._texture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this._texture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
			Image image = new Image(File.ReadAllBytes(Path.Combine(Paths.GameData, "Tools/RotateGizmo.png")));
			byte[] array;
			byte* value;
			if ((array = image.Pixels) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.TexImage2D(GL.TEXTURE_2D, 0, 6408, image.Width, image.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, (IntPtr)((void*)value));
			array = null;
			this.snapAngle = snapAngle;
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x001933D2 File Offset: 0x001915D2
		protected override void DoDispose()
		{
			this._graphics.GL.DeleteTexture(this._texture);
			this._quadRenderer.Dispose();
			this._textRenderer.Dispose();
			this._lineRenderer.Dispose();
		}

		// Token: 0x06005586 RID: 21894 RVA: 0x00193410 File Offset: 0x00191610
		public void PrepareForDraw(ref Matrix viewProjectionMatrix, ICameraController cameraController)
		{
			bool flag = !this.Visible;
			if (flag)
			{
				throw new Exception("PrepareForDraw called when it was not required. Please check with Visible first before calling this.");
			}
			float scale = 0.2f / (float)this._font.BaseSize;
			int spread = this._font.Spread;
			float num = 1f / (float)spread;
			Vector3 position = cameraController.Position;
			this._textPosition = this._position - position;
			float num2 = Vector3.Distance(this._position, position);
			this._fillBlurThreshold = MathHelper.Clamp(2f * num2 * 0.1f, 1f, (float)spread) * num;
			Matrix.CreateTranslation(-this._textRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Center), -this._textRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Middle), 0f, out this._tempMatrix);
			Matrix.CreateScale(scale, out this._drawMatrix);
			Matrix.Multiply(ref this._tempMatrix, ref this._drawMatrix, out this._drawMatrix);
			Vector3 rotation = cameraController.Rotation;
			Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f, out this._tempMatrix);
			Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
			Matrix.AddTranslation(ref this._drawMatrix, this._position.X, this._position.Y, this._position.Z);
			Matrix.Multiply(ref this._drawMatrix, ref viewProjectionMatrix, out this._textMatrix);
		}

		// Token: 0x06005587 RID: 21895 RVA: 0x00193578 File Offset: 0x00191778
		public void Draw(ref Matrix viewProjectionMatrix, ICameraController cameraController, Vector3 renderPositionOffset)
		{
			bool flag = !this.Visible;
			if (flag)
			{
				throw new Exception("PrepareForDraw called when it was not required. Please check with Visible first before calling this.");
			}
			GLFunctions gl = this._graphics.GL;
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			gl.BindTexture(GL.TEXTURE_2D, this._texture);
			basicProgram.AssertInUse();
			gl.AssertTextureBound(GL.TEXTURE0, this._texture);
			Vector3 vector = Vector3.One;
			foreach (KeyValuePair<RotationGizmo.Axis, float> keyValuePair in RotationGizmo._axisDirections)
			{
				bool flag2 = this._selectedAxis != RotationGizmo.Axis.None && this._selectedAxis != keyValuePair.Key;
				if (!flag2)
				{
					basicProgram.Opacity.SetValue((this._highlightedAxis == keyValuePair.Key) ? 0.7f : 0.4f);
					this._tempMatrix = Matrix.CreateTranslation(-this._size.X / 2f, -this._size.Y / 2f, 0f);
					this._drawMatrix = Matrix.CreateScale(this._size.X, this._size.Y, this._size.Z);
					Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
					switch (keyValuePair.Key)
					{
					case RotationGizmo.Axis.X:
						this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + this._rotationOffset.Y + 1.5707964f, 0f, 0f);
						vector = this._graphics.RedColor;
						break;
					case RotationGizmo.Axis.Y:
						this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotationOffset.Y, 1.5707964f, 0f);
						vector = this._graphics.GreenColor;
						break;
					case RotationGizmo.Axis.Z:
						this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + this._rotationOffset.Y + 3.1415927f, 0f, 0f);
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
					gl.DepthFunc(GL.ALWAYS);
					this._quadRenderer.Draw();
					bool flag3 = this._selectedAxis == keyValuePair.Key;
					if (flag3)
					{
						gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
					}
				}
			}
			gl.BindTexture(GL.TEXTURE_2D, this._graphics.WhitePixelTexture.GLTexture);
			foreach (KeyValuePair<RotationGizmo.Axis, float> keyValuePair2 in RotationGizmo._axisDirections)
			{
				bool flag4 = this._selectedAxis != RotationGizmo.Axis.None && this._selectedAxis != keyValuePair2.Key;
				if (!flag4)
				{
					basicProgram.Opacity.SetValue((this._highlightedAxis == keyValuePair2.Key) ? 0.7f : 0.4f);
					this._tempMatrix = Matrix.CreateTranslation(0.015625f, 0f, 0.02f);
					this._drawMatrix = Matrix.CreateScale(-0.03125f, 1.1f, 1f);
					Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
					switch (keyValuePair2.Key)
					{
					case RotationGizmo.Axis.X:
						this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + this._rotationOffset.Y + 1.5707964f, 0f, this._rotation.X + this._rotationOffset.X - 1.5707964f);
						vector = this._graphics.RedColor;
						break;
					case RotationGizmo.Axis.Y:
						this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + this._rotationOffset.Y + 3.1415927f, 1.5707964f, 0f);
						vector = this._graphics.GreenColor;
						break;
					case RotationGizmo.Axis.Z:
						this._tempMatrix = Matrix.CreateFromYawPitchRoll(this._rotation.Y + this._rotationOffset.Y + 3.1415927f, 0f, -this._rotation.Z + this._rotationOffset.Z);
						vector = this._graphics.BlueColor;
						break;
					}
					Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
					Vector3 position2 = this._position + renderPositionOffset;
					this._tempMatrix = Matrix.CreateTranslation(position2);
					Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
					Matrix.Multiply(ref this._drawMatrix, ref viewProjectionMatrix, out this._drawMatrix);
					basicProgram.MVPMatrix.SetValue(ref this._drawMatrix);
					basicProgram.Color.SetValue(vector);
					bool flag5 = this._selectedAxis == keyValuePair2.Key;
					if (flag5)
					{
						gl.DepthFunc(GL.ALWAYS);
					}
					this._quadRenderer.Draw();
					bool flag6 = this._selectedAxis == keyValuePair2.Key;
					if (flag6)
					{
						this._lineRenderer.Draw(ref this._drawMatrix, vector, 0.5f);
					}
				}
			}
			this.PrepareForDraw(ref viewProjectionMatrix, cameraController);
		}

		// Token: 0x06005588 RID: 21896 RVA: 0x00193BB4 File Offset: 0x00191DB4
		public void DrawText()
		{
			GLFunctions gl = this._graphics.GL;
			TextProgram textProgram = this._graphics.GPUProgramStore.TextProgram;
			bool flag = !this.Visible;
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with Visible first before calling this.");
			}
			textProgram.AssertInUse();
			textProgram.Position.SetValue(this._textPosition);
			textProgram.FillBlurThreshold.SetValue(this._fillBlurThreshold);
			textProgram.MVPMatrix.SetValue(ref this._textMatrix);
			gl.DepthFunc(GL.ALWAYS);
			this._textRenderer.Draw();
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
		}

		// Token: 0x06005589 RID: 21897 RVA: 0x00193C6C File Offset: 0x00191E6C
		public void Tick(Ray playerViewRay, float targetBlockHitDistance)
		{
			bool flag = !this.Visible;
			if (!flag)
			{
				float num = float.PositiveInfinity;
				this._highlightedAxis = RotationGizmo.Axis.None;
				foreach (RotationGizmo.Axis axis in RotationGizmo._axisDirections.Keys)
				{
					bool flag2 = this._selectedAxis != RotationGizmo.Axis.None && axis != this._selectedAxis;
					if (!flag2)
					{
						Vector3 vector;
						bool flag3 = this.CheckRayIntersection(playerViewRay, axis, out vector, this._selectedAxis != RotationGizmo.Axis.None);
						if (flag3)
						{
							float num2 = Vector3.Distance(vector, playerViewRay.Position);
							bool flag4 = num == float.PositiveInfinity || num > num2 || axis == this._selectedAxis;
							if (flag4)
							{
								float num3 = Vector3.Distance(vector, this._position);
								bool flag5 = axis == this._selectedAxis || (num3 > 0.77f && num3 < 1f);
								if (flag5)
								{
									this._highlightedAxis = axis;
									num = num2;
									this._lastHitPosition = vector;
									bool flag6 = float.IsNaN(this._axisOffsetAngle);
									if (flag6)
									{
										float num4 = (axis == RotationGizmo.Axis.X) ? this._rotation.X : ((axis == RotationGizmo.Axis.Y) ? this._rotation.Y : this._rotation.Z);
										this._axisOffsetAngle = num4 - this.GetAngleFromAxisHit(this._lastHitPosition, axis);
									}
								}
							}
						}
					}
				}
				this._textRenderer.Text = this.GetDisplayText();
			}
		}

		// Token: 0x0600558A RID: 21898 RVA: 0x00193E24 File Offset: 0x00192024
		public void Show(Vector3 position, Vector3? rotation = null, RotationGizmo.OnRotationChange onChange = null, Vector3? rotationOffset = null)
		{
			this._position = position;
			bool flag = rotation != null;
			if (flag)
			{
				this._rotation = rotation.Value;
			}
			this.Visible = true;
			this._rotationOffset = ((rotationOffset != null) ? rotationOffset.Value : Vector3.Zero);
			bool flag2 = onChange != null;
			if (flag2)
			{
				this._onChange = onChange;
			}
		}

		// Token: 0x0600558B RID: 21899 RVA: 0x00193E84 File Offset: 0x00192084
		public void Hide()
		{
			this.Visible = false;
		}

		// Token: 0x0600558C RID: 21900 RVA: 0x00193E90 File Offset: 0x00192090
		public void OnInteract(InteractionType interactionType)
		{
			bool flag = interactionType == 0;
			if (flag)
			{
				bool flag2 = this._highlightedAxis != RotationGizmo.Axis.None && this._selectedAxis == RotationGizmo.Axis.None;
				if (flag2)
				{
					this._selectedAxis = this._highlightedAxis;
					this._lastRotation = this._rotation;
					this._axisOffsetAngle = float.NaN;
				}
				else
				{
					bool flag3 = this._selectedAxis != RotationGizmo.Axis.None;
					if (flag3)
					{
						this._selectedAxis = RotationGizmo.Axis.None;
					}
				}
			}
			else
			{
				bool flag4 = this._selectedAxis == RotationGizmo.Axis.None;
				if (flag4)
				{
					this.Visible = false;
				}
				else
				{
					this._rotation = this._lastRotation;
					this._selectedAxis = RotationGizmo.Axis.None;
					this._onChange(this._rotation);
				}
			}
		}

		// Token: 0x0600558D RID: 21901 RVA: 0x00193F44 File Offset: 0x00192144
		public void UpdateRotation(bool snapValue)
		{
			switch (this._selectedAxis)
			{
			case RotationGizmo.Axis.X:
				this._rotation.X = this.GetAngleFromAxisHit(this._lastHitPosition, RotationGizmo.Axis.X);
				if (snapValue)
				{
					this._rotation.X = RotationGizmo.GetSnapValue(this._rotation.X, this.snapAngle);
				}
				break;
			case RotationGizmo.Axis.Y:
				this._rotation.Y = this.GetAngleFromAxisHit(this._lastHitPosition, RotationGizmo.Axis.Y);
				if (snapValue)
				{
					this._rotation.Y = RotationGizmo.GetSnapValue(this._rotation.Y, this.snapAngle);
				}
				break;
			case RotationGizmo.Axis.Z:
				this._rotation.Z = this.GetAngleFromAxisHit(this._lastHitPosition, RotationGizmo.Axis.Z);
				if (snapValue)
				{
					this._rotation.Z = RotationGizmo.GetSnapValue(this._rotation.Z, this.snapAngle);
				}
				break;
			case RotationGizmo.Axis.None:
				return;
			}
			this._onChange(this._rotation);
		}

		// Token: 0x0600558E RID: 21902 RVA: 0x00194054 File Offset: 0x00192254
		private bool CheckRayIntersection(Ray viewRay, RotationGizmo.Axis axis, out Vector3 intersection, bool force = false)
		{
			Vector3 vector = viewRay.Position - this._position;
			Vector3 vector2 = new Vector3(this._size.X / 2f, this._size.Y / 2f, 0f);
			float num = (axis == RotationGizmo.Axis.Z) ? 3.1415927f : 1.5707964f;
			bool flag = axis == RotationGizmo.Axis.Y;
			if (flag)
			{
				Matrix.CreateFromYawPitchRoll(0f, -num, 0f, out this._tempMatrix);
			}
			else
			{
				Matrix.CreateFromYawPitchRoll(-(this._rotation.Y + this._rotationOffset.Y + num), 0f, 0f, out this._tempMatrix);
			}
			Matrix.CreateTranslation(ref vector, out this._drawMatrix);
			Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
			Vector3 direction = Vector3.Transform(viewRay.Direction, this._tempMatrix);
			Matrix.CreateTranslation(ref vector2, out this._tempMatrix);
			Matrix.Multiply(ref this._drawMatrix, ref this._tempMatrix, out this._drawMatrix);
			Ray ray = new Ray(this._drawMatrix.Translation + this._position, direction);
			float scaleFactor = (ray.Direction.Z >= 0f) ? ((ray.Position.Z - this._position.Z) / -ray.Direction.Z) : ((this._position.Z - ray.Position.Z) / ray.Direction.Z);
			Vector3 vector3 = ray.Position + ray.Direction * scaleFactor;
			bool flag2 = force || (vector3.Y >= this._position.Y && vector3.Y <= this._position.Y + this._size.Y && vector3.X >= this._position.X && vector3.X <= this._position.X + this._size.X);
			bool result;
			if (flag2)
			{
				intersection = viewRay.Position + viewRay.Direction * scaleFactor;
				result = true;
			}
			else
			{
				intersection = Vector3.Zero;
				result = false;
			}
			return result;
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x001942B4 File Offset: 0x001924B4
		private float GetAngleFromAxisHit(Vector3 hitPosition, RotationGizmo.Axis axis)
		{
			Vector3 position = this._lastHitPosition - this._position;
			float result;
			switch (axis)
			{
			case RotationGizmo.Axis.X:
			{
				Matrix.CreateFromYawPitchRoll(-this._rotation.Y - this._rotationOffset.Y + 1.5707964f + 3.1415927f, 0f, 0f, out this._tempMatrix);
				Vector3 vector = Vector3.Transform(position, this._tempMatrix);
				result = MathHelper.WrapAngle((float)Math.Atan2((double)vector.Y, (double)vector.X) + (float.IsNaN(this._axisOffsetAngle) ? 0f : this._axisOffsetAngle));
				break;
			}
			case RotationGizmo.Axis.Y:
			{
				Matrix.CreateFromYawPitchRoll(0f, -1.5707964f, 0f, out this._tempMatrix);
				Vector3 vector = Vector3.Transform(position, this._tempMatrix);
				result = MathHelper.WrapAngle((float)Math.Atan2((double)vector.X, (double)vector.Y) - 3.1415927f + (float.IsNaN(this._axisOffsetAngle) ? 0f : this._axisOffsetAngle));
				break;
			}
			case RotationGizmo.Axis.Z:
			{
				Matrix.CreateFromYawPitchRoll(-this._rotation.Y - this._rotationOffset.Y + 3.1415927f, 0f, 0f, out this._tempMatrix);
				Vector3 vector = Vector3.Transform(position, this._tempMatrix);
				result = -MathHelper.WrapAngle((float)Math.Atan2((double)vector.Y, (double)vector.X) - 1.5707964f - (float.IsNaN(this._axisOffsetAngle) ? 0f : this._axisOffsetAngle));
				break;
			}
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x00194468 File Offset: 0x00192668
		private static float GetSnapValue(float value, float increment)
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

		// Token: 0x06005591 RID: 21905 RVA: 0x001944C4 File Offset: 0x001926C4
		private string GetDisplayText()
		{
			string result = "";
			bool flag = this._highlightedAxis != RotationGizmo.Axis.None && this._selectedAxis != this._highlightedAxis;
			if (flag)
			{
				switch (this._highlightedAxis)
				{
				case RotationGizmo.Axis.X:
				{
					float num = (float)(Math.Round((double)(MathHelper.ToDegrees(this._rotation.X) * 100f)) / 100.0);
					result = string.Format("Pitch: {0}°", num);
					break;
				}
				case RotationGizmo.Axis.Y:
				{
					float num = (float)(Math.Round((double)(MathHelper.ToDegrees(this._rotation.Y) * 100f)) / 100.0);
					result = string.Format("Yaw: {0}°", num);
					break;
				}
				case RotationGizmo.Axis.Z:
				{
					float num = (float)(Math.Round((double)(MathHelper.ToDegrees(this._rotation.Z) * 100f)) / 100.0);
					result = string.Format("Roll: {0}°", num);
					break;
				}
				}
			}
			else
			{
				bool flag2 = this._highlightedAxis == RotationGizmo.Axis.None;
				if (flag2)
				{
					this._textRenderer.Text = "";
				}
				else
				{
					bool flag3 = this._selectedAxis != RotationGizmo.Axis.None;
					if (flag3)
					{
						switch (this._selectedAxis)
						{
						case RotationGizmo.Axis.X:
						{
							float num2 = (float)(Math.Round((double)(MathHelper.ToDegrees(this._rotation.X) * 100f)) / 100.0);
							result = string.Format("{0}°", num2);
							break;
						}
						case RotationGizmo.Axis.Y:
						{
							float num2 = (float)(Math.Round((double)(MathHelper.ToDegrees(this._rotation.Y) * 100f)) / 100.0);
							result = string.Format("{0}°", num2);
							break;
						}
						case RotationGizmo.Axis.Z:
						{
							float num2 = (float)(Math.Round((double)(MathHelper.ToDegrees(this._rotation.Z) * 100f)) / 100.0);
							result = string.Format("{0}°", num2);
							break;
						}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x00194704 File Offset: 0x00192904
		public bool InUse()
		{
			return this._selectedAxis != RotationGizmo.Axis.None;
		}

		// Token: 0x0400327D RID: 12925
		private const float DefaultSnapAngle = 0.2617994f;

		// Token: 0x0400327E RID: 12926
		private readonly GraphicsDevice _graphics;

		// Token: 0x0400327F RID: 12927
		private readonly Font _font;

		// Token: 0x04003280 RID: 12928
		private readonly QuadRenderer _quadRenderer;

		// Token: 0x04003281 RID: 12929
		private readonly GLTexture _texture;

		// Token: 0x04003282 RID: 12930
		private readonly TextRenderer _textRenderer;

		// Token: 0x04003283 RID: 12931
		private readonly LineRenderer _lineRenderer;

		// Token: 0x04003284 RID: 12932
		private readonly float snapAngle;

		// Token: 0x04003285 RID: 12933
		public bool Visible = false;

		// Token: 0x04003286 RID: 12934
		private RotationGizmo.OnRotationChange _onChange;

		// Token: 0x04003287 RID: 12935
		private Vector3 _position = Vector3.Zero;

		// Token: 0x04003288 RID: 12936
		private Vector3 _rotation = Vector3.Zero;

		// Token: 0x04003289 RID: 12937
		private Vector3 _rotationOffset = Vector3.Zero;

		// Token: 0x0400328A RID: 12938
		private Vector3 _size = new Vector3(2f, 2f, 2f);

		// Token: 0x0400328B RID: 12939
		private Vector3 _lastHitPosition = Vector3.Zero;

		// Token: 0x0400328C RID: 12940
		private Vector3 _lastRotation = Vector3.Zero;

		// Token: 0x0400328D RID: 12941
		private Vector3 _textPosition = Vector3.Zero;

		// Token: 0x0400328E RID: 12942
		private float _fillBlurThreshold;

		// Token: 0x0400328F RID: 12943
		private float _axisOffsetAngle;

		// Token: 0x04003290 RID: 12944
		private Matrix _tempMatrix;

		// Token: 0x04003291 RID: 12945
		private Matrix _drawMatrix;

		// Token: 0x04003292 RID: 12946
		private Matrix _textMatrix;

		// Token: 0x04003293 RID: 12947
		private RotationGizmo.Axis _highlightedAxis = RotationGizmo.Axis.None;

		// Token: 0x04003294 RID: 12948
		private RotationGizmo.Axis _selectedAxis = RotationGizmo.Axis.None;

		// Token: 0x04003295 RID: 12949
		private static readonly Dictionary<RotationGizmo.Axis, float> _axisDirections = new Dictionary<RotationGizmo.Axis, float>
		{
			{
				RotationGizmo.Axis.X,
				1.5707964f
			},
			{
				RotationGizmo.Axis.Y,
				1.5707964f
			},
			{
				RotationGizmo.Axis.Z,
				3.1415927f
			}
		};

		// Token: 0x02000EEA RID: 3818
		// (Invoke) Token: 0x0600681A RID: 26650
		public delegate void OnRotationChange(Vector3 rotation);

		// Token: 0x02000EEB RID: 3819
		private enum Axis
		{
			// Token: 0x0400492C RID: 18732
			X,
			// Token: 0x0400492D RID: 18733
			Y,
			// Token: 0x0400492E RID: 18734
			Z,
			// Token: 0x0400492F RID: 18735
			None
		}
	}
}
