using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A9E RID: 2718
	internal class SelectionToolRenderer : Disposable
	{
		// Token: 0x06005594 RID: 21908 RVA: 0x00194758 File Offset: 0x00192958
		public SelectionToolRenderer(GraphicsDevice graphics, Font font)
		{
			this._graphics = graphics;
			this._font = font;
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			gl.EnableVertexAttribArray(basicProgram.AttribPosition.Index);
			gl.VertexAttribPointer(basicProgram.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(basicProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(basicProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
			this._faceHighlightRenderer = new QuadRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram.AttribPosition, this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			this._boxRenderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._textRenderer = new TextRenderer(this._graphics, this._font, "Entity", uint.MaxValue, 4278190080U);
			ForceFieldProgram builderToolProgram = this._graphics.GPUProgramStore.BuilderToolProgram;
			MeshProcessor.CreateBox(ref this._meshBox, 2f, (int)builderToolProgram.AttribPosition.Index, (int)builderToolProgram.AttribTexCoords.Index, (int)builderToolProgram.AttribNormal.Index);
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x00194960 File Offset: 0x00192B60
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteVertexArray(this._vertexArray);
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
			this._faceHighlightRenderer.Dispose();
			this._boxRenderer.Dispose();
			this._textRenderer.Dispose();
			this._meshBox.Dispose();
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x001949D4 File Offset: 0x00192BD4
		public unsafe void UpdateSelection(Vector3 pos1, Vector3 pos2)
		{
			this._pos1 = pos1;
			this._pos2 = pos2;
			int num = (int)MathHelper.Min(pos1.X, pos2.X);
			int num2 = (int)MathHelper.Min(pos1.Y, pos2.Y);
			int num3 = (int)MathHelper.Min(pos1.Z, pos2.Z);
			int num4 = (int)MathHelper.Max(pos1.X, pos2.X) + 1;
			int num5 = (int)MathHelper.Max(pos1.Y, pos2.Y) + 1;
			int num6 = (int)MathHelper.Max(pos1.Z, pos2.Z) + 1;
			int num7 = num4 - num;
			int num8 = num5 - num2;
			int num9 = num6 - num3;
			this._centerPos = new Vector3((float)num + (float)num7 / 2f, (float)num2 + (float)num8 / 2f, (float)num3 + (float)num9 / 2f);
			this._selectionSize = new Vector3((float)num7, (float)num8, (float)num9);
			int num10 = (num7 + 1) * 4;
			int num11 = (num8 + 1) * 4;
			int num12 = (num9 + 1) * 4;
			int num13 = num10 + num11 + num12;
			this._vertices = new float[num13 * 8];
			this._indices = new ushort[num13 * 2];
			Vector3 zero = Vector3.Zero;
			int num14 = 0;
			ushort num15 = 0;
			for (int i = 0; i <= num7; i++)
			{
				this.BuildLineLoop(ref num14, ref num15, new Vector3((float)i, zero.Y, zero.Z), new Vector3((float)i, (float)num8, zero.Z), new Vector3((float)i, (float)num8, (float)num9), new Vector3((float)i, zero.Y, (float)num9));
			}
			for (int j = 0; j <= num9; j++)
			{
				this.BuildLineLoop(ref num14, ref num15, new Vector3(zero.X, zero.Y, (float)j), new Vector3(zero.X, (float)num8, (float)j), new Vector3((float)num7, (float)num8, (float)j), new Vector3((float)num7, zero.Y, (float)j));
			}
			for (int k = 0; k <= num8; k++)
			{
				this.BuildLineLoop(ref num14, ref num15, new Vector3(zero.X, (float)k, zero.Z), new Vector3(zero.X, (float)k, (float)num9), new Vector3((float)num7, (float)k, (float)num9), new Vector3((float)num7, (float)k, zero.Z));
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = this._vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * 4), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array2;
			ushort* value2;
			if ((array2 = this._indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
			this._selectionBox = new BoundingBox(Vector3.Min(this._pos1, this._pos2), Vector3.Max(this._pos1, this._pos2) + Vector3.One);
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x00194D84 File Offset: 0x00192F84
		private void BuildLineLoop(ref int vertexInc, ref ushort indexInc, Vector3 vec0, Vector3 vec1, Vector3 vec2, Vector3 vec3)
		{
			int num = vertexInc / 8;
			this.AddLineVertex(ref vertexInc, vec0);
			this.AddLineVertex(ref vertexInc, vec1);
			this.AddLineVertex(ref vertexInc, vec2);
			this.AddLineVertex(ref vertexInc, vec3);
			ushort[] indices = this._indices;
			ushort num2 = indexInc;
			indexInc = num2 + 1;
			indices[(int)num2] = (ushort)num;
			ushort[] indices2 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices2[(int)num2] = (ushort)(num + 1);
			ushort[] indices3 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices3[(int)num2] = (ushort)(num + 1);
			ushort[] indices4 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices4[(int)num2] = (ushort)(num + 2);
			ushort[] indices5 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices5[(int)num2] = (ushort)(num + 2);
			ushort[] indices6 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices6[(int)num2] = (ushort)(num + 3);
			ushort[] indices7 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices7[(int)num2] = (ushort)(num + 3);
			ushort[] indices8 = this._indices;
			num2 = indexInc;
			indexInc = num2 + 1;
			indices8[(int)num2] = (ushort)num;
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x00194E64 File Offset: 0x00193064
		private void AddLineVertex(ref int vertexInc, Vector3 pos)
		{
			float[] vertices = this._vertices;
			int num = vertexInc;
			vertexInc = num + 1;
			vertices[num] = pos.X;
			float[] vertices2 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices2[num] = pos.Y;
			float[] vertices3 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices3[num] = pos.Z;
			float[] vertices4 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices4[num] = 0f;
			float[] vertices5 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices5[num] = 0f;
			float[] vertices6 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices6[num] = 0f;
			float[] vertices7 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices7[num] = 0f;
			float[] vertices8 = this._vertices;
			num = vertexInc;
			vertexInc = num + 1;
			vertices8[num] = 0f;
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x00194F20 File Offset: 0x00193120
		public void DrawGrid(ref Matrix viewProjectionMatrix, Vector3 positionOffset, Vector3 color, float opacity, SelectionToolRenderer.SelectionDrawMode drawMode)
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			basicProgram.Color.SetValue(color);
			Vector3 value = this._centerPos - this._selectionSize * new Vector3(0.5f);
			Vector3 vector = value + positionOffset;
			Matrix.CreateTranslation(ref vector, out this._matrix);
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			basicProgram.MVPMatrix.SetValue(ref this._matrix);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			basicProgram.Opacity.SetValue(opacity);
			bool flag = drawMode == SelectionToolRenderer.SelectionDrawMode.Normal;
			if (flag)
			{
				basicProgram.Color.SetValue(this._graphics.WhiteColor);
			}
			else
			{
				basicProgram.Color.SetValue(this._graphics.CyanColor);
			}
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			gl.DrawElements(GL.ONE, this._indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
			bool flag2 = drawMode > SelectionToolRenderer.SelectionDrawMode.Normal;
			if (flag2)
			{
				basicProgram.Opacity.SetValue(opacity);
				basicProgram.Color.SetValue(this._graphics.WhiteColor);
				gl.DrawElements(GL.ONE, this._indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x001950C4 File Offset: 0x001932C4
		public void DrawOutlineBox(ref Matrix viewProjectionMatrix, ref Matrix viewMatrix, Vector3 positionOffset, Vector2 viewportSize, Vector3 lineColor, Vector3 quadColor, float lineOpacity, float quadOpacity, bool drawIntersectionHighlight = true)
		{
			BoundingBox selectionBox = this._selectionBox;
			bool flag = true;
			if (flag)
			{
				ForceFieldProgram builderToolProgram = this._graphics.GPUProgramStore.BuilderToolProgram;
				GLFunctions gl = this._graphics.GL;
				gl.UseProgram(builderToolProgram);
				Vector3 value = this._selectionBox.GetSize() * new Vector3(0.5f, 0.5f, 0.5f);
				Vector3 value2 = this._selectionBox.Min + value;
				value2 += positionOffset;
				Matrix.CreateScale(ref value, out this._matrix);
				Matrix.CreateTranslation(ref value2, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				Matrix matrix = Matrix.Transpose(Matrix.Invert(this._matrix));
				builderToolProgram.ModelMatrix.SetValue(ref this._matrix);
				builderToolProgram.ColorOpacity.SetValue(quadColor.X, quadColor.Y, quadColor.Z, quadOpacity);
				builderToolProgram.ViewMatrix.SetValue(ref viewMatrix);
				builderToolProgram.ViewProjectionMatrix.SetValue(ref viewProjectionMatrix);
				builderToolProgram.CurrentInvViewportSize.SetValue(Vector2.One / viewportSize);
				builderToolProgram.NormalMatrix.SetValue(ref matrix);
				builderToolProgram.UVAnimationSpeed.SetValue(0f, 0f);
				builderToolProgram.OutlineMode.SetValue(builderToolProgram.OutlineModeNone);
				builderToolProgram.DrawAndBlendMode.SetValue(builderToolProgram.DrawModeColor, builderToolProgram.BlendModeLinear);
				Vector4 value3 = new Vector4(1f, 1f, 1f, 0.4f);
				float value4 = drawIntersectionHighlight ? 0.5f : 0f;
				builderToolProgram.IntersectionHighlightColorOpacity.SetValue(value3);
				builderToolProgram.IntersectionHighlightThickness.SetValue(value4);
				gl.BindVertexArray(this._meshBox.VertexArray);
				gl.DrawArrays(GL.TRIANGLES, 0, this._meshBox.Count);
				gl.UseProgram(this._graphics.GPUProgramStore.BasicProgram);
				this._boxRenderer.Draw(positionOffset, this._selectionBox, viewProjectionMatrix, lineColor, lineOpacity, quadColor, 0f);
			}
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x001952F0 File Offset: 0x001934F0
		public void DrawCornerBoxes(ref Matrix viewProjectionMatrix, Vector3 positionOffset, Vector3 pos1Color, Vector3 pos2Color, float pos1Opacity = 0.05f, float pos2Opacity = 0.05f)
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			GLFunctions gl = this._graphics.GL;
			gl.DepthFunc(GL.ALWAYS);
			this._boxRenderer.Draw(this._pos1 + positionOffset, this._originBox, viewProjectionMatrix, pos1Color, 0.4f, this._graphics.GreenColor, pos1Opacity);
			this._boxRenderer.Draw(this._pos2 + positionOffset, this._originBox, viewProjectionMatrix, pos2Color, 0.4f, this._graphics.RedColor, pos2Opacity);
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x001953E4 File Offset: 0x001935E4
		public void DrawResizeGizmoForFace(Vector3 playerPosition, ref Matrix viewProjectionMatrix, Vector3 selectionNormal, Vector3 color, float minGizmoSize, float maxGizmoSize, float percentageOfSelectionLengthGizmoShouldRender)
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Color.SetValue(color);
			float num = MathHelper.Clamp(this._selectionSize.X * percentageOfSelectionLengthGizmoShouldRender, minGizmoSize, maxGizmoSize);
			float num2 = MathHelper.Clamp(this._selectionSize.Y * percentageOfSelectionLengthGizmoShouldRender, minGizmoSize, maxGizmoSize);
			float num3 = MathHelper.Clamp(this._selectionSize.Z * percentageOfSelectionLengthGizmoShouldRender, minGizmoSize, maxGizmoSize);
			Vector3 vector = this._centerPos - playerPosition;
			bool flag = selectionNormal.Y != 0f;
			if (flag)
			{
				bool flag2 = (double)selectionNormal.Y < -0.1 && playerPosition.Y < this._centerPos.Y - this._selectionSize.Y / 2f;
				if (!flag2)
				{
					bool flag3 = (double)selectionNormal.Y > 0.1 && playerPosition.Y > this._centerPos.Y + this._selectionSize.Y / 2f;
					if (flag3)
					{
					}
				}
				Matrix.CreateScale(num, num3, num2, out this._matrix);
				Matrix.CreateRotationX(1.5707964f, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				Matrix.CreateTranslation(vector.X - num * 0.5f, 0.01f * selectionNormal.Y + vector.Y + this._selectionSize.Y / 2f * selectionNormal.Y, vector.Z - num3 * 0.5f, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
			}
			else
			{
				bool flag4 = selectionNormal.X != 0f;
				if (flag4)
				{
					bool flag5 = (double)selectionNormal.X < -0.1 && playerPosition.X < this._centerPos.X - this._selectionSize.X / 2f;
					if (!flag5)
					{
						bool flag6 = (double)selectionNormal.X > 0.1 && playerPosition.X > this._centerPos.X + this._selectionSize.X / 2f;
						if (flag6)
						{
						}
					}
					Matrix.CreateScale(num3, num2, num, out this._matrix);
					Matrix.CreateRotationY(-1.5707964f, out this._tempMatrix);
					Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
					Matrix.CreateTranslation(0.01f * selectionNormal.X + vector.X + this._selectionSize.X / 2f * selectionNormal.X, vector.Y - num2 * 0.5f, vector.Z - num3 * 0.5f, out this._tempMatrix);
					Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				}
				else
				{
					bool flag7 = selectionNormal.Z != 0f;
					if (flag7)
					{
						bool flag8 = (double)selectionNormal.Z < -0.1 && playerPosition.Z < this._centerPos.Z - this._selectionSize.Z / 2f;
						if (!flag8)
						{
							bool flag9 = (double)selectionNormal.Z > 0.1 && playerPosition.Z > this._centerPos.Z + this._selectionSize.Z / 2f;
							if (flag9)
							{
							}
						}
						Matrix.CreateScale(num, num2, num3, out this._matrix);
						Matrix.CreateTranslation(vector.X - num * 0.5f, vector.Y - num2 * 0.5f, 0.01f * selectionNormal.Z + vector.Z + this._selectionSize.Z / 2f * selectionNormal.Z, out this._tempMatrix);
						Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
					}
				}
			}
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			basicProgram.MVPMatrix.SetValue(ref this._matrix);
			GLFunctions gl = this._graphics.GL;
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.GEQUAL : GL.LEQUAL);
			basicProgram.Opacity.SetValue(0.15f);
			this._faceHighlightRenderer.Draw();
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			basicProgram.Opacity.SetValue(0.3f);
			this._faceHighlightRenderer.Draw();
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001958E0 File Offset: 0x00193AE0
		public void DrawFaceHighlight(ref Matrix viewProjectionMatrix, Vector3 selectionNormal, Vector3 color, Vector3 positionOffset)
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Color.SetValue(color);
			Vector3 vector = this._centerPos + positionOffset;
			bool flag = selectionNormal.Y != 0f;
			if (flag)
			{
				Matrix.CreateScale(this._selectionSize.X, this._selectionSize.Z, 1f, out this._matrix);
				Matrix.CreateRotationX(1.5707964f, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				Matrix.CreateTranslation(vector.X - this._selectionSize.X / 2f, vector.Y + (this._selectionSize.Y / 2f + 0.005f) * selectionNormal.Y, vector.Z - this._selectionSize.Z / 2f, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
			}
			else
			{
				bool flag2 = selectionNormal.X != 0f;
				if (flag2)
				{
					Matrix.CreateScale(this._selectionSize.Z, this._selectionSize.Y, 1f, out this._matrix);
					Matrix.CreateRotationY(-1.5707964f, out this._tempMatrix);
					Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
					Matrix.CreateTranslation(vector.X + (this._selectionSize.X / 2f + 0.005f) * selectionNormal.X, vector.Y - this._selectionSize.Y / 2f, vector.Z - this._selectionSize.Z / 2f, out this._tempMatrix);
					Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				}
				else
				{
					bool flag3 = selectionNormal.Z != 0f;
					if (flag3)
					{
						Matrix.CreateScale(this._selectionSize.X, this._selectionSize.Y, 1f, out this._matrix);
						Matrix.CreateTranslation(vector.X - this._selectionSize.X / 2f, vector.Y - this._selectionSize.Y / 2f, vector.Z + (this._selectionSize.Z / 2f + 0.005f) * selectionNormal.Z, out this._tempMatrix);
						Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
					}
				}
			}
			Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
			basicProgram.MVPMatrix.SetValue(ref this._matrix);
			GLFunctions gl = this._graphics.GL;
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.GEQUAL : GL.LEQUAL);
			basicProgram.Opacity.SetValue(0.15f);
			this._faceHighlightRenderer.Draw();
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			basicProgram.Opacity.SetValue(0.3f);
			this._faceHighlightRenderer.Draw();
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x00195C58 File Offset: 0x00193E58
		public void DrawText(ref Matrix viewProjectionMatrix, ICameraController cameraController)
		{
			GLFunctions gl = this._graphics.GL;
			TextProgram textProgram = this._graphics.GPUProgramStore.TextProgram;
			textProgram.AssertInUse();
			float scale = 0.4f / (float)this._font.BaseSize;
			int spread = this._font.Spread;
			float num = 1f / (float)spread;
			Vector3 position = cameraController.Position;
			gl.DepthFunc(GL.ALWAYS);
			Vector3[] array = new Vector3[3];
			Vector3 vector = (this._pos1 - this._pos2 + Vector3.One) * 0.5f;
			Vector3 vector2 = new Vector3((vector.X < 0f) ? 1f : 0f, (vector.Y < 0f) ? 1f : 0f, (vector.Z < 0f) ? 1f : 0f);
			float y = Math.Max(this._pos1.Y, this._pos2.Y) - this._pos2.Y + 1f - num / 2f;
			array[0] = this._pos2 + new Vector3(vector.X, y, vector2.Z);
			array[1] = this._pos2 + new Vector3(vector2.X, vector.Y - num / 2f, vector2.Z);
			array[2] = this._pos2 + new Vector3(vector2.X, y, vector.Z);
			string[] array2 = new string[]
			{
				this._selectionSize.X.ToString(),
				this._selectionSize.Y.ToString(),
				this._selectionSize.Z.ToString()
			};
			for (int i = 0; i < 3; i++)
			{
				Vector3 value = array[i] - position;
				float num2 = Vector3.Distance(array[i], position);
				float value2 = MathHelper.Clamp(2f * num2 * 0.1f, 1f, (float)spread) * num;
				Matrix.CreateTranslation(-this._textRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Center), -this._textRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Middle), 0f, out this._tempMatrix);
				Matrix.CreateScale(scale, out this._matrix);
				Matrix.Multiply(ref this._tempMatrix, ref this._matrix, out this._matrix);
				Vector3 rotation = cameraController.Rotation;
				Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f, out this._tempMatrix);
				Matrix.Multiply(ref this._matrix, ref this._tempMatrix, out this._matrix);
				Matrix.AddTranslation(ref this._matrix, array[i].X, array[i].Y, array[i].Z);
				Matrix.Multiply(ref this._matrix, ref viewProjectionMatrix, out this._matrix);
				textProgram.Position.SetValue(value);
				textProgram.FillBlurThreshold.SetValue(value2);
				textProgram.MVPMatrix.SetValue(ref this._matrix);
				this._textRenderer.Text = array2[i];
				this._textRenderer.Draw();
			}
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
		}

		// Token: 0x04003296 RID: 12950
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003297 RID: 12951
		private readonly Font _font;

		// Token: 0x04003298 RID: 12952
		private float[] _vertices;

		// Token: 0x04003299 RID: 12953
		private ushort[] _indices;

		// Token: 0x0400329A RID: 12954
		private readonly GLVertexArray _vertexArray;

		// Token: 0x0400329B RID: 12955
		private readonly GLBuffer _verticesBuffer;

		// Token: 0x0400329C RID: 12956
		private readonly GLBuffer _indicesBuffer;

		// Token: 0x0400329D RID: 12957
		private readonly QuadRenderer _faceHighlightRenderer;

		// Token: 0x0400329E RID: 12958
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x0400329F RID: 12959
		private Mesh _meshBox;

		// Token: 0x040032A0 RID: 12960
		private readonly BoundingBox _originBox = new BoundingBox(new Vector3(-0.02f, -0.02f, -0.02f), new Vector3(1.02f, 1.02f, 1.02f));

		// Token: 0x040032A1 RID: 12961
		private BoundingBox _selectionBox;

		// Token: 0x040032A2 RID: 12962
		private readonly TextRenderer _textRenderer;

		// Token: 0x040032A3 RID: 12963
		private Vector3 _centerPos;

		// Token: 0x040032A4 RID: 12964
		private Vector3 _selectionSize;

		// Token: 0x040032A5 RID: 12965
		private Vector3 _pos1;

		// Token: 0x040032A6 RID: 12966
		private Vector3 _pos2;

		// Token: 0x040032A7 RID: 12967
		private Matrix _tempMatrix;

		// Token: 0x040032A8 RID: 12968
		private Matrix _matrix;

		// Token: 0x040032A9 RID: 12969
		private const float GizmoOffset = 0.01f;

		// Token: 0x02000EEC RID: 3820
		public enum SelectionDrawMode
		{
			// Token: 0x04004931 RID: 18737
			Normal,
			// Token: 0x04004932 RID: 18738
			Combine,
			// Token: 0x04004933 RID: 18739
			Subtract
		}
	}
}
