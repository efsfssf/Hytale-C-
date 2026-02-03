using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Gizmos.Models;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Brush;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A99 RID: 2713
	internal class BrushToolRenderer : Disposable
	{
		// Token: 0x06005570 RID: 21872 RVA: 0x00191870 File Offset: 0x0018FA70
		public BrushToolRenderer(GraphicsDevice graphics)
		{
			this._graphics = graphics;
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ForceFieldProgram builderToolProgram = this._graphics.GPUProgramStore.BuilderToolProgram;
			gl.EnableVertexAttribArray(builderToolProgram.AttribPosition.Index);
			gl.VertexAttribPointer(builderToolProgram.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(builderToolProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(builderToolProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x0019197C File Offset: 0x0018FB7C
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteVertexArray(this._vertexArray);
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
		}

		// Token: 0x06005572 RID: 21874 RVA: 0x001919C0 File Offset: 0x0018FBC0
		public unsafe void UpdateBrushData(BrushData brushData, PrimitiveModelData modelData)
		{
			bool flag = modelData != null;
			if (flag)
			{
				this._modelData = modelData;
				this._brushData = brushData;
			}
			else
			{
				bool flag2 = brushData.Equals(this._brushData);
				if (flag2)
				{
					return;
				}
				this._brushData = brushData;
				switch (brushData.Shape)
				{
				case 1:
					this._modelData = SphereModel.BuildModelData((float)this._brushData.Width / 2f, (float)this._brushData.Height, 16, 16, 0f);
					break;
				case 2:
					this._modelData = CylinderModel.BuildModelData((float)this._brushData.Width / 2f, (float)this._brushData.Height, 16);
					break;
				case 3:
				case 4:
					this._modelData = ConeModel.BuildModelData((float)this._brushData.Width / 2f, (float)this._brushData.Height, 16);
					break;
				case 5:
				case 6:
					this._modelData = PyramidModel.BuildModelData((float)this._brushData.Width / 2f, (float)this._brushData.Height, 5);
					break;
				default:
					this._modelData = CubeModel.BuildModelData((float)this._brushData.Width / 2f, (float)this._brushData.Height / 2f, 0f);
					break;
				}
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = this._modelData.Vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._modelData.Vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = this._modelData.Indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._modelData.Indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
		}

		// Token: 0x06005573 RID: 21875 RVA: 0x00191C34 File Offset: 0x0018FE34
		public void Draw(ref Matrix viewProjectionMatrix, ref Matrix viewMatrix, Vector2 viewportSize, Vector3 position, Vector3 color, float opacity, bool drawIntersectionHighlight = false)
		{
			ForceFieldProgram builderToolProgram = this._graphics.GPUProgramStore.BuilderToolProgram;
			GLFunctions gl = this._graphics.GL;
			builderToolProgram.AssertInUse();
			bool flag;
			if (this._modelData != null)
			{
				ushort[] indices = this._modelData.Indices;
				flag = (indices != null && indices.Length == 0);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				float num = 0.5f;
				bool flag3 = this._brushData != null;
				if (flag3)
				{
					bool flag4 = this._brushData.Shape == 4 || this._brushData.Shape == 6;
					if (flag4)
					{
						Matrix.CreateRotationX(3.1415927f, out this._matrix);
					}
					else
					{
						float num2 = ((float)this._brushData.Width + 0.1f) / (float)this._brushData.Width;
						float yScale = ((float)this._brushData.Height + 0.1f) / (float)this._brushData.Height;
						Matrix.CreateScale(num2, yScale, num2, out this._matrix);
					}
					bool flag5 = this._brushData.Origin == 2;
					if (flag5)
					{
						num = (float)(-(float)this._brushData.Height) / 2f + 1f;
					}
					else
					{
						bool flag6 = this._brushData.Origin == 1;
						if (flag6)
						{
							num = (float)this._brushData.Height / 2f + 1f;
						}
					}
				}
				else
				{
					Matrix.CreateScale(1f, 1f, 1f, out this._matrix);
				}
				Matrix.CreateTranslation(position.X + 0.5f, position.Y + num, position.Z + 0.5f, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				Matrix matrix = Matrix.Transpose(Matrix.Invert(this._matrix));
				builderToolProgram.ModelMatrix.SetValue(ref this._matrix);
				builderToolProgram.ViewMatrix.SetValue(ref viewMatrix);
				builderToolProgram.ViewProjectionMatrix.SetValue(ref viewProjectionMatrix);
				builderToolProgram.CurrentInvViewportSize.SetValue(Vector2.One / viewportSize);
				builderToolProgram.NormalMatrix.SetValue(ref matrix);
				builderToolProgram.UVAnimationSpeed.SetValue(0f, 0f);
				builderToolProgram.OutlineMode.SetValue(builderToolProgram.OutlineModeNone);
				builderToolProgram.DrawAndBlendMode.SetValue(builderToolProgram.DrawModeColor, builderToolProgram.BlendModeLinear);
				Vector4 value = new Vector4(1f, 1f, 1f, 0.4f);
				float value2 = drawIntersectionHighlight ? 0.5f : 0f;
				builderToolProgram.IntersectionHighlightColorOpacity.SetValue(value);
				builderToolProgram.IntersectionHighlightThickness.SetValue(value2);
				gl.BindVertexArray(this._vertexArray);
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.GEQUAL : GL.LEQUAL);
				builderToolProgram.ColorOpacity.SetValue(color.X, color.Y, color.Z, opacity * 0.5f);
				gl.DrawElements(GL.TRIANGLES, this._modelData.Indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
				builderToolProgram.ColorOpacity.SetValue(color.X, color.Y, color.Z, opacity);
				gl.DrawElements(GL.TRIANGLES, this._modelData.Indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
				gl.DrawElements(GL.ONE, this._modelData.Indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
		}

		// Token: 0x04003252 RID: 12882
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003253 RID: 12883
		private readonly GLVertexArray _vertexArray;

		// Token: 0x04003254 RID: 12884
		private readonly GLBuffer _verticesBuffer;

		// Token: 0x04003255 RID: 12885
		private readonly GLBuffer _indicesBuffer;

		// Token: 0x04003256 RID: 12886
		private PrimitiveModelData _modelData;

		// Token: 0x04003257 RID: 12887
		private BrushData _brushData;

		// Token: 0x04003258 RID: 12888
		private Matrix _tempMatrix;

		// Token: 0x04003259 RID: 12889
		private Matrix _matrix;
	}
}
