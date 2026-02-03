using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.Graphics.Batcher2D
{
	// Token: 0x02000AC4 RID: 2756
	public class Batcher2D : Disposable
	{
		// Token: 0x060056E2 RID: 22242 RVA: 0x001A33E0 File Offset: 0x001A15E0
		public unsafe Batcher2D(GraphicsDevice graphics, bool allowBatcher2dToGrow = false)
		{
			this._graphics = graphics;
			this._transformationMatrix = Matrix.Identity;
			this._vertices = new Batcher2DVertex[this._maxQuads * 4];
			this._texturesPerQuad = new GLTexture[this._maxQuads];
			this._maskTexturesPerQuad = new GLTexture[this._maxQuads];
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			ushort[] array = new ushort[this._maxQuads * 6];
			for (int i = 0; i < this._maxQuads; i++)
			{
				array[i * 6] = (ushort)(i * 4);
				array[i * 6 + 1] = (ushort)(i * 4 + 1);
				array[i * 6 + 2] = (ushort)(i * 4 + 2);
				array[i * 6 + 3] = (ushort)(i * 4);
				array[i * 6 + 4] = (ushort)(i * 4 + 2);
				array[i * 6 + 5] = (ushort)(i * 4 + 3);
			}
			ushort[] array2;
			ushort* value;
			if ((array2 = array) == null || array2.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array.Length * 2), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array2 = null;
			Batcher2DProgram batcher2DProgram = this._graphics.GPUProgramStore.Batcher2DProgram;
			IntPtr pointer = IntPtr.Zero;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribPosition.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribPosition.Index, 3, GL.FLOAT, false, Batcher2DVertex.Size, pointer);
			pointer += 12;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribTexCoords.Index, 2, GL.UNSIGNED_SHORT, true, Batcher2DVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribScissor.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribScissor.Index, 4, GL.UNSIGNED_SHORT, false, Batcher2DVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribMaskTextureArea.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribMaskTextureArea.Index, 4, GL.FLOAT, false, Batcher2DVertex.Size, pointer);
			pointer += 16;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribMaskBounds.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribMaskBounds.Index, 4, GL.UNSIGNED_SHORT, false, Batcher2DVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribFillColor.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribFillColor.Index, 4, GL.UNSIGNED_BYTE, true, Batcher2DVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribOutlineColor.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribOutlineColor.Index, 4, GL.UNSIGNED_BYTE, true, Batcher2DVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribSDFSettings.Index);
			gl.VertexAttribPointer(batcher2DProgram.AttribSDFSettings.Index, 4, GL.UNSIGNED_BYTE, true, Batcher2DVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(batcher2DProgram.AttribFontId.Index);
			gl.VertexAttribIPointer(batcher2DProgram.AttribFontId.Index, 1, GL.UNSIGNED_INT, Batcher2DVertex.Size, pointer);
			pointer += 4;
			this._allowBatcher2dToGrow = allowBatcher2dToGrow;
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x001A380C File Offset: 0x001A1A0C
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteBuffer(this._indicesBuffer);
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x001A3850 File Offset: 0x001A1A50
		public unsafe void Draw()
		{
			bool flag = this._usedQuads == 0;
			if (!flag)
			{
				GLFunctions gl = this._graphics.GL;
				this._graphics.GPUProgramStore.Batcher2DProgram.AssertInUse();
				gl.BindVertexArray(this._vertexArray);
				gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
				Batcher2DVertex[] array;
				Batcher2DVertex* value;
				if ((array = this._vertices) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * Batcher2DVertex.Size), (IntPtr)((void*)value), GL.DYNAMIC_DRAW);
				array = null;
				gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
				GLTexture gltexture = this._maskTexturesPerQuad[0];
				bool flag2 = gltexture != GLTexture.None;
				if (flag2)
				{
					gl.ActiveTexture(GL.TEXTURE1);
					gl.BindTexture(GL.TEXTURE_2D, gltexture);
					gl.ActiveTexture(GL.TEXTURE0);
				}
				GLTexture gltexture2 = this._texturesPerQuad[0];
				gl.BindTexture(GL.TEXTURE_2D, gltexture2);
				int num = 0;
				for (int i = 0; i < this._usedQuads; i++)
				{
					GLTexture gltexture3 = this._maskTexturesPerQuad[i];
					bool flag3 = gltexture3 != GLTexture.None && gltexture3 != gltexture;
					if (flag3)
					{
						gl.ActiveTexture(GL.TEXTURE1);
						gl.BindTexture(GL.TEXTURE_2D, gltexture3);
						gl.ActiveTexture(GL.TEXTURE0);
					}
					GLTexture gltexture4 = this._texturesPerQuad[i];
					bool flag4 = gltexture4 != GLTexture.None && gltexture4 != gltexture2;
					if (flag4)
					{
						gl.DrawElements(GL.TRIANGLES, (i - num) * 6, GL.UNSIGNED_SHORT, (IntPtr)(num * 6 * 2));
						num = i;
						gltexture2 = gltexture4;
						gl.BindTexture(GL.TEXTURE_2D, gltexture2);
					}
				}
				gl.DrawElements(GL.TRIANGLES, (this._usedQuads - num) * 6, GL.UNSIGNED_SHORT, (IntPtr)(num * 6 * 2));
				this._usedQuads = 0;
			}
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x001A3A8C File Offset: 0x001A1C8C
		public void PushScissor(Rectangle scissor)
		{
			Rectangle rectangle = scissor;
			bool flag = this._scissorStack.Count > 0;
			if (flag)
			{
				Rectangle rectangle2 = this._scissorStack.Peek();
				int num = rectangle.X - rectangle2.X;
				bool flag2 = num < 0;
				if (flag2)
				{
					rectangle.X -= num;
					rectangle.Width += num;
				}
				int num2 = rectangle2.Right - rectangle.Right;
				bool flag3 = num2 < 0;
				if (flag3)
				{
					rectangle.Width += num2;
				}
				int num3 = rectangle.Y - rectangle2.Y;
				bool flag4 = num3 < 0;
				if (flag4)
				{
					rectangle.Y -= num3;
					rectangle.Height += num3;
				}
				int num4 = rectangle2.Bottom - rectangle.Bottom;
				bool flag5 = num4 < 0;
				if (flag5)
				{
					rectangle.Height += num4;
				}
			}
			this._scissorStack.Push(rectangle);
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x001A3B86 File Offset: 0x001A1D86
		public void PopScissor()
		{
			this._scissorStack.Pop();
		}

		// Token: 0x060056E7 RID: 22247 RVA: 0x001A3B98 File Offset: 0x001A1D98
		public void PushMask(TextureArea area, Rectangle rectangle, Rectangle viewportRectangle)
		{
			this._maskStack.Push(new Batcher2D.MaskSetup
			{
				Area = area,
				Bounds = new UShortVector4((ushort)(viewportRectangle.X + rectangle.X), (ushort)(viewportRectangle.Y + rectangle.Y), (ushort)rectangle.Width, (ushort)rectangle.Height)
			});
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x001A3BF3 File Offset: 0x001A1DF3
		public void PopMask()
		{
			this._maskStack.Pop();
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x001A3C02 File Offset: 0x001A1E02
		public void SetTransformationMatrix(Vector3 position, Quaternion orientation, float scale)
		{
			Matrix.Compose(scale, orientation, position, out this._transformationMatrix);
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x001A3C14 File Offset: 0x001A1E14
		public void SetTransformationMatrix(Matrix worldMatrix)
		{
			this._transformationMatrix = worldMatrix;
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x001A3C1E File Offset: 0x001A1E1E
		public void SetOpacityOverride(byte? opacity)
		{
			this._opacityOverride = opacity;
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x001A3C28 File Offset: 0x001A1E28
		public void RequestDrawTexture(GLTexture glTexture, int textureWidth, int textureHeight, Rectangle sourceRect, Vector3 position, float width, float height, UInt32Color color, bool flip = false)
		{
			int usedQuads = this._usedQuads;
			bool flag = usedQuads >= this._texturesPerQuad.Length;
			if (flag)
			{
				Batcher2D.Logger.Warn("Maximum quads {0} for UI reached!", this._vertices.Length);
				bool flag2 = !this.GrowArraysIfAllowed();
				if (flag2)
				{
					return;
				}
			}
			ushort x = (ushort)MathHelper.Round((float)(65535 * sourceRect.Left / textureWidth));
			ushort x2 = (ushort)MathHelper.Round((float)(65535 * sourceRect.Right / textureWidth));
			ushort num = (ushort)MathHelper.Round((float)(65535 * sourceRect.Top / textureHeight));
			ushort num2 = (ushort)MathHelper.Round((float)(65535 * sourceRect.Bottom / textureHeight));
			Vector3 position2 = new Vector3(position.X + width, position.Y, position.Z);
			Vector3 position3 = new Vector3(position.X, position.Y, position.Z);
			Vector3 position4 = new Vector3(position.X, position.Y + height, position.Z);
			Vector3 position5 = new Vector3(position.X + width, position.Y + height, position.Z);
			Rectangle rectangle = (this._scissorStack.Count > 0) ? this._scissorStack.Peek() : new Rectangle(0, 0, 65535, 65535);
			Vector3 vector = new Vector3((float)rectangle.X, (float)rectangle.Y, 0f);
			Vector3.Transform(ref vector, ref this._transformationMatrix, out vector);
			UShortVector4 ushortVector = new UShortVector4((ushort)Math.Max(0f, vector.X), (ushort)Math.Max(0f, vector.Y), (ushort)Math.Max(0, rectangle.Width), (ushort)Math.Max(0, rectangle.Height));
			Vector3 vector2 = Vector3.Transform(position2, this._transformationMatrix);
			Vector3 vector3 = Vector3.Transform(position3, this._transformationMatrix);
			Vector3 position6 = Vector3.Transform(position4, this._transformationMatrix);
			Vector3 vector4 = Vector3.Transform(position5, this._transformationMatrix);
			bool flag3 = vector2.X < (float)ushortVector.X;
			if (!flag3)
			{
				bool flag4 = vector4.Y < (float)ushortVector.Y;
				if (!flag4)
				{
					bool flag5 = vector3.X > (float)(ushortVector.X + ushortVector.Z);
					if (!flag5)
					{
						bool flag6 = vector2.Y > (float)(ushortVector.Y + ushortVector.W);
						if (!flag6)
						{
							Batcher2D.MaskSetup maskSetup = (this._maskStack.Count > 0) ? this._maskStack.Peek() : null;
							UShortVector4 maskBounds = (maskSetup != null) ? maskSetup.Bounds : UShortVector4.Zero;
							Vector4 zero = Vector4.Zero;
							bool flag7 = maskSetup != null;
							if (flag7)
							{
								Texture texture = maskSetup.Area.Texture;
								this._maskTexturesPerQuad[usedQuads] = texture.GLTexture;
								Rectangle rectangle2 = maskSetup.Area.Rectangle;
								float x3 = (float)rectangle2.X / (float)texture.Width;
								float y = (float)rectangle2.Y / (float)texture.Height;
								float z = (float)rectangle2.Width / (float)texture.Width;
								float w = (float)rectangle2.Height / (float)texture.Height;
								zero = new Vector4(x3, y, z, w);
							}
							if (flip)
							{
								ushort num3 = num;
								num = num2;
								num2 = num3;
							}
							bool flag8 = this._opacityOverride != null;
							if (flag8)
							{
								color.SetA(this._opacityOverride.GetValueOrDefault());
							}
							this._vertices[usedQuads * 4] = new Batcher2DVertex
							{
								Position = vector2,
								TextureCoordinates = new UShortVector2(x2, num),
								Scissor = ushortVector,
								MaskTextureArea = zero,
								MaskBounds = maskBounds,
								FillColor = color
							};
							this._vertices[usedQuads * 4 + 1] = new Batcher2DVertex
							{
								Position = vector3,
								TextureCoordinates = new UShortVector2(x, num),
								Scissor = ushortVector,
								MaskTextureArea = zero,
								MaskBounds = maskBounds,
								FillColor = color
							};
							this._vertices[usedQuads * 4 + 2] = new Batcher2DVertex
							{
								Position = position6,
								TextureCoordinates = new UShortVector2(x, num2),
								Scissor = ushortVector,
								MaskTextureArea = zero,
								MaskBounds = maskBounds,
								FillColor = color
							};
							this._vertices[usedQuads * 4 + 3] = new Batcher2DVertex
							{
								Position = vector4,
								TextureCoordinates = new UShortVector2(x2, num2),
								Scissor = ushortVector,
								MaskTextureArea = zero,
								MaskBounds = maskBounds,
								FillColor = color
							};
							this._texturesPerQuad[usedQuads] = glTexture;
							this._usedQuads++;
						}
					}
				}
			}
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x001A4130 File Offset: 0x001A2330
		public void RequestDrawTextureTriangle(GLTexture glTexture, int textureWidth, int textureHeight, Rectangle sourceRect, Vector3 topLeft, Vector3 bottomLeft, Vector3 bottomRight, UInt32Color color, bool flip)
		{
			int usedQuads = this._usedQuads;
			bool flag = usedQuads == this._maxQuads;
			if (flag)
			{
				Batcher2D.Logger.Warn("Maximum quads {0} for UI reached!", this._maxQuads);
			}
			else
			{
				ushort x = (ushort)MathHelper.Round((float)(65535 * sourceRect.Left / textureWidth));
				ushort x2 = (ushort)MathHelper.Round((float)(65535 * sourceRect.Right / textureWidth));
				ushort num = (ushort)MathHelper.Round((float)(65535 * sourceRect.Top / textureHeight));
				ushort num2 = (ushort)MathHelper.Round((float)(65535 * sourceRect.Bottom / textureHeight));
				Batcher2D.MaskSetup maskSetup = (this._maskStack.Count > 0) ? this._maskStack.Peek() : null;
				UShortVector4 maskBounds = (maskSetup != null) ? maskSetup.Bounds : UShortVector4.Zero;
				Vector4 zero = Vector4.Zero;
				bool flag2 = maskSetup != null;
				if (flag2)
				{
					Texture texture = maskSetup.Area.Texture;
					this._maskTexturesPerQuad[usedQuads] = texture.GLTexture;
					Rectangle rectangle = maskSetup.Area.Rectangle;
					float x3 = (float)rectangle.X / (float)texture.Width;
					float y = (float)rectangle.Y / (float)texture.Height;
					float z = (float)rectangle.Width / (float)texture.Width;
					float w = (float)rectangle.Height / (float)texture.Height;
					zero = new Vector4(x3, y, z, w);
				}
				Rectangle rectangle2 = (this._scissorStack.Count > 0) ? this._scissorStack.Peek() : new Rectangle(0, 0, 65535, 65535);
				UShortVector4 scissor = new UShortVector4((ushort)Math.Max(0, rectangle2.X), (ushort)Math.Max(0, rectangle2.Y), (ushort)Math.Max(0, rectangle2.Width), (ushort)Math.Max(0, rectangle2.Height));
				if (flip)
				{
					ushort num3 = num;
					num = num2;
					num2 = num3;
				}
				this._vertices[usedQuads * 4] = new Batcher2DVertex
				{
					Position = Vector3.Transform(bottomRight, this._transformationMatrix),
					TextureCoordinates = new UShortVector2(x2, num2),
					Scissor = scissor,
					MaskTextureArea = zero,
					MaskBounds = maskBounds,
					FillColor = color
				};
				this._vertices[usedQuads * 4 + 1] = new Batcher2DVertex
				{
					Position = Vector3.Transform(topLeft, this._transformationMatrix),
					TextureCoordinates = new UShortVector2(x, num),
					Scissor = scissor,
					MaskTextureArea = zero,
					MaskBounds = maskBounds,
					FillColor = color
				};
				this._vertices[usedQuads * 4 + 2] = new Batcher2DVertex
				{
					Position = Vector3.Transform(bottomLeft, this._transformationMatrix),
					TextureCoordinates = new UShortVector2(x, num2),
					Scissor = scissor,
					MaskTextureArea = zero,
					MaskBounds = maskBounds,
					FillColor = color
				};
				this._vertices[usedQuads * 4 + 3] = new Batcher2DVertex
				{
					Position = Vector3.Transform(bottomRight, this._transformationMatrix),
					TextureCoordinates = new UShortVector2(x2, num2),
					Scissor = scissor,
					MaskTextureArea = zero,
					MaskBounds = maskBounds,
					FillColor = color
				};
				this._texturesPerQuad[usedQuads] = glTexture;
				this._usedQuads++;
			}
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x001A44C0 File Offset: 0x001A26C0
		private unsafe bool GrowArraysIfAllowed()
		{
			bool flag = !this._allowBatcher2dToGrow;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Array.Resize<Batcher2DVertex>(ref this._vertices, this._vertices.Length * 2 * 4);
				Array.Resize<GLTexture>(ref this._texturesPerQuad, this._texturesPerQuad.Length * 2);
				Array.Resize<GLTexture>(ref this._maskTexturesPerQuad, this._maskTexturesPerQuad.Length * 2);
				ushort[] array = new ushort[this._texturesPerQuad.Length * 6];
				for (int i = 0; i < this._texturesPerQuad.Length; i++)
				{
					array[i * 6] = (ushort)(i * 4);
					array[i * 6 + 1] = (ushort)(i * 4 + 1);
					array[i * 6 + 2] = (ushort)(i * 4 + 2);
					array[i * 6 + 3] = (ushort)(i * 4);
					array[i * 6 + 4] = (ushort)(i * 4 + 2);
					array[i * 6 + 5] = (ushort)(i * 4 + 3);
				}
				this._graphics.GL.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
				ushort[] array2;
				ushort* value;
				if ((array2 = array) == null || array2.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array2[0];
				}
				this._graphics.GL.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(array.Length * 2), (IntPtr)((void*)value), GL.STATIC_DRAW);
				array2 = null;
				result = true;
			}
			return result;
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x001A4614 File Offset: 0x001A2814
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawTexture(Texture texture, Rectangle sourceRect, Vector3 position, float width, float height, UInt32Color color)
		{
			this.RequestDrawTexture(texture.GLTexture, texture.Width, texture.Height, sourceRect, position, width, height, color, false);
		}

		// Token: 0x060056F0 RID: 22256 RVA: 0x001A4644 File Offset: 0x001A2844
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawTextureTriangle(Texture texture, Rectangle sourceRect, Vector3 topLeft, Vector3 bottomLeft, Vector3 bottonRight, UInt32Color color)
		{
			this.RequestDrawTextureTriangle(texture.GLTexture, texture.Width, texture.Height, sourceRect, topLeft, bottomLeft, bottonRight, color, false);
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x001A4674 File Offset: 0x001A2874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawTexture(Texture texture, Rectangle sourceRect, Rectangle destRect, UInt32Color color)
		{
			this.RequestDrawTexture(texture.GLTexture, texture.Width, texture.Height, sourceRect, new Vector3((float)destRect.X, (float)destRect.Y, 0f), (float)destRect.Width, (float)destRect.Height, color, false);
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x001A46C8 File Offset: 0x001A28C8
		public void RequestDrawPatch(Texture texture, Rectangle sourceRect, int sourceHorizontalBorder, int sourceVerticalBorder, int sourceScale, Vector3 position, float width, float height, float borderScale, UInt32Color color)
		{
			float num = (float)sourceHorizontalBorder * borderScale;
			float num2 = (float)sourceVerticalBorder * borderScale;
			sourceHorizontalBorder *= sourceScale;
			sourceVerticalBorder *= sourceScale;
			Rectangle rectangle = new Rectangle(sourceRect.X + sourceHorizontalBorder, sourceRect.Y + sourceVerticalBorder, sourceRect.Width - sourceHorizontalBorder * 2, sourceRect.Height - sourceVerticalBorder * 2);
			Vector3 position2 = new Vector3(position.X + num, position.Y + num2, position.Z);
			float num3 = width - num * 2f;
			float num4 = height - num2 * 2f;
			bool flag = num * 2f < width && num2 * 2f < height;
			if (flag)
			{
				this.RequestDrawTexture(texture, rectangle, position2, num3, num4, color);
			}
			float num5 = Math.Min(num, width);
			float num6 = Math.Max(0f, Math.Min(num * 2f, width) - num);
			float num7 = Math.Min(num2, height);
			float num8 = Math.Max(0f, Math.Min(num2 * 2f, height) - num2);
			bool flag2 = this._opacityOverride != null;
			if (flag2)
			{
				color.SetA(this._opacityOverride.GetValueOrDefault());
			}
			bool flag3 = num != 0f && num2 != 0f;
			if (flag3)
			{
				Rectangle sourceRect2 = new Rectangle(sourceRect.X, sourceRect.Y, sourceHorizontalBorder, sourceVerticalBorder);
				this.RequestDrawTexture(texture, sourceRect2, position, num5, num7, color);
				Rectangle sourceRect3 = new Rectangle(sourceRect.Right - sourceHorizontalBorder, sourceRect.Y, sourceHorizontalBorder, sourceVerticalBorder);
				Vector3 position3 = new Vector3(position.X + width - num6, position.Y, position.Z);
				this.RequestDrawTexture(texture, sourceRect3, position3, num6, num7, color);
				Rectangle sourceRect4 = new Rectangle(sourceRect.X, sourceRect.Bottom - sourceVerticalBorder, sourceHorizontalBorder, sourceVerticalBorder);
				Vector3 position4 = new Vector3(position.X, position.Y + height - num8, position.Z);
				this.RequestDrawTexture(texture, sourceRect4, position4, num5, num8, color);
				Rectangle sourceRect5 = new Rectangle(sourceRect.Right - sourceHorizontalBorder, sourceRect.Bottom - sourceVerticalBorder, sourceHorizontalBorder, sourceVerticalBorder);
				Vector3 position5 = new Vector3(position.X + width - num6, position.Y + height - num8, position.Z);
				this.RequestDrawTexture(texture, sourceRect5, position5, num6, num8, color);
			}
			bool flag4 = num != 0f && num4 != 0f;
			if (flag4)
			{
				bool flag5 = num5 > 0f;
				if (flag5)
				{
					Rectangle sourceRect6 = new Rectangle(sourceRect.X, sourceRect.Y + sourceVerticalBorder, sourceHorizontalBorder, rectangle.Height);
					Vector3 position6 = new Vector3(position.X, position.Y + num2, position.Z);
					this.RequestDrawTexture(texture, sourceRect6, position6, num5, num4, color);
				}
				bool flag6 = num6 > 0f;
				if (flag6)
				{
					Rectangle sourceRect7 = new Rectangle(sourceRect.Right - sourceHorizontalBorder, sourceRect.Y + sourceVerticalBorder, sourceHorizontalBorder, rectangle.Height);
					Vector3 position7 = new Vector3(position.X + width - num6, position.Y + num2, position.Z);
					this.RequestDrawTexture(texture, sourceRect7, position7, num6, num4, color);
				}
			}
			bool flag7 = num2 != 0f && num3 != 0f;
			if (flag7)
			{
				bool flag8 = num7 > 0f;
				if (flag8)
				{
					Rectangle sourceRect8 = new Rectangle(sourceRect.X + sourceHorizontalBorder, sourceRect.Y, rectangle.Width, sourceVerticalBorder);
					Vector3 position8 = new Vector3(position.X + num, position.Y, position.Z);
					this.RequestDrawTexture(texture, sourceRect8, position8, num3, num7, color);
				}
				bool flag9 = num8 > 0f;
				if (flag9)
				{
					Rectangle sourceRect9 = new Rectangle(sourceRect.X + sourceHorizontalBorder, sourceRect.Bottom - sourceVerticalBorder, rectangle.Width, sourceVerticalBorder);
					Vector3 position9 = new Vector3(position.X + num, position.Y + height - num8, position.Z);
					this.RequestDrawTexture(texture, sourceRect9, position9, num3, num8, color);
				}
			}
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x001A4B04 File Offset: 0x001A2D04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawPatch(TexturePatch patch, Vector3 position, float width, float height, float borderScale)
		{
			this.RequestDrawPatch(patch.TextureArea.Texture, patch.TextureArea.Rectangle, patch.HorizontalBorder, patch.VerticalBorder, patch.TextureArea.Scale, position, width, height, borderScale, patch.Color);
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x001A4B54 File Offset: 0x001A2D54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawPatch(TexturePatch patch, Rectangle destRect, float borderScale)
		{
			this.RequestDrawPatch(patch.TextureArea.Texture, patch.TextureArea.Rectangle, patch.HorizontalBorder, patch.VerticalBorder, patch.TextureArea.Scale, new Vector3((float)destRect.X, (float)destRect.Y, 0f), (float)destRect.Width, (float)destRect.Height, borderScale, patch.Color);
		}

		// Token: 0x060056F5 RID: 22261 RVA: 0x001A4BC4 File Offset: 0x001A2DC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawPatch(TexturePatch patch, Rectangle destRect, float borderScale, UInt32Color colorOverride)
		{
			this.RequestDrawPatch(patch.TextureArea.Texture, patch.TextureArea.Rectangle, patch.HorizontalBorder, patch.VerticalBorder, patch.TextureArea.Scale, new Vector3((float)destRect.X, (float)destRect.Y, 0f), (float)destRect.Width, (float)destRect.Height, borderScale, colorOverride);
		}

		// Token: 0x060056F6 RID: 22262 RVA: 0x001A4C30 File Offset: 0x001A2E30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawPatch(Texture texture, Rectangle sourceRect, int sourceHorizontalBorder, int sourceVerticalBorder, int sourceScale, Rectangle destRect, float borderScale, UInt32Color color)
		{
			this.RequestDrawPatch(texture, sourceRect, sourceHorizontalBorder, sourceVerticalBorder, sourceScale, new Vector3((float)destRect.X, (float)destRect.Y, 0f), (float)destRect.Width, (float)destRect.Height, borderScale, color);
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x001A4C7C File Offset: 0x001A2E7C
		public void RequestDrawOutline(Texture texture, Rectangle sourceRect, Vector3 position, float width, float height, float borderSize, UInt32Color color)
		{
			this.RequestDrawTexture(texture, sourceRect, new Vector3(position.X, position.Y, position.Z), width, borderSize, color);
			this.RequestDrawTexture(texture, sourceRect, new Vector3(position.X, position.Y + height - borderSize, position.Z), width, borderSize, color);
			this.RequestDrawTexture(texture, sourceRect, new Vector3(position.X, position.Y, position.Z), borderSize, height, color);
			this.RequestDrawTexture(texture, sourceRect, new Vector3(position.X + width - borderSize, position.Y, position.Z), borderSize, height, color);
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x001A4D2E File Offset: 0x001A2F2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestDrawOutline(Texture texture, Rectangle sourceRect, Rectangle destRect, float borderSize, UInt32Color color)
		{
			this.RequestDrawOutline(texture, sourceRect, new Vector3((float)destRect.X, (float)destRect.Y, 0f), (float)destRect.Width, (float)destRect.Height, borderSize, color);
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x001A4D64 File Offset: 0x001A2F64
		public void RequestDrawText(Font font, float size, string text, Vector3 position, UInt32Color color, bool isBold = false, bool isItalics = false, float letterSpacing = 0f)
		{
			bool flag = this._usedQuads + text.Length >= this._texturesPerQuad.Length;
			if (flag)
			{
				Batcher2D.Logger.Warn("Maximum quads {0} for UI reached!", this._vertices.Length);
				bool flag2 = !this.GrowArraysIfAllowed();
				if (flag2)
				{
					return;
				}
			}
			int width = font.TextureAtlas.Width;
			int height = font.TextureAtlas.Height;
			float num = size / (float)font.BaseSize;
			byte fillThreshold = isBold ? 32 : 0;
			byte fillBlurAmount = (byte)(255f / ((float)font.Spread * num));
			byte outlineThreshold = 0;
			byte outlineBlurAmount = 0;
			float num2 = isItalics ? (size / 4f) : 0f;
			Batcher2D.MaskSetup maskSetup = (this._maskStack.Count > 0) ? this._maskStack.Peek() : null;
			UShortVector4 maskBounds = (maskSetup != null) ? maskSetup.Bounds : UShortVector4.Zero;
			Vector4 zero = Vector4.Zero;
			GLTexture gltexture = GLTexture.None;
			bool flag3 = maskSetup != null;
			if (flag3)
			{
				Texture texture = maskSetup.Area.Texture;
				gltexture = texture.GLTexture;
				Rectangle rectangle = maskSetup.Area.Rectangle;
				float x = (float)rectangle.X / (float)texture.Width;
				float y = (float)rectangle.Y / (float)texture.Height;
				float z = (float)rectangle.Width / (float)texture.Width;
				float w = (float)rectangle.Height / (float)texture.Height;
				zero = new Vector4(x, y, z, w);
			}
			Rectangle rectangle2 = (this._scissorStack.Count > 0) ? this._scissorStack.Peek() : new Rectangle(0, 0, 65535, 65535);
			Vector3 vector = new Vector3((float)rectangle2.X, (float)rectangle2.Y, 0f);
			Vector3.Transform(ref vector, ref this._transformationMatrix, out vector);
			UShortVector4 ushortVector = new UShortVector4((ushort)Math.Max(0f, vector.X), (ushort)Math.Max(0f, vector.Y), (ushort)Math.Max(0, rectangle2.Width), (ushort)Math.Max(0, rectangle2.Height));
			int num3 = this._usedQuads;
			position.X -= (float)font.Spread * num;
			position.Y -= (float)font.Spread * num;
			byte fontId = (byte)font.FontId;
			bool flag4 = this._opacityOverride != null;
			if (flag4)
			{
				color.SetA(this._opacityOverride.GetValueOrDefault());
			}
			foreach (ushort key in text)
			{
				Rectangle fallbackGlyphAtlasRectangle;
				bool flag5 = !font.GlyphAtlasRectangles.TryGetValue(key, out fallbackGlyphAtlasRectangle);
				if (flag5)
				{
					fallbackGlyphAtlasRectangle = font.FallbackGlyphAtlasRectangle;
				}
				float fallbackGlyphAdvance;
				bool flag6 = !font.GlyphAdvances.TryGetValue(key, out fallbackGlyphAdvance);
				if (flag6)
				{
					fallbackGlyphAdvance = font.FallbackGlyphAdvance;
				}
				Vector3 vector2 = new Vector3(position.X + (float)fallbackGlyphAtlasRectangle.Width * num + num2, position.Y, position.Z);
				Vector3 vector3 = new Vector3(position.X + num2, position.Y, position.Z);
				Vector3 position2 = new Vector3(position.X - num2, position.Y + (float)fallbackGlyphAtlasRectangle.Height * num, position.Z);
				Vector3 vector4 = new Vector3(position.X + (float)fallbackGlyphAtlasRectangle.Width * num - num2, position.Y + (float)fallbackGlyphAtlasRectangle.Height * num, position.Z);
				position.X += fallbackGlyphAdvance * num + letterSpacing;
				bool flag7 = vector2.X < (float)ushortVector.X;
				if (!flag7)
				{
					bool flag8 = vector4.Y < (float)ushortVector.Y;
					if (!flag8)
					{
						bool flag9 = vector3.X > (float)(ushortVector.X + ushortVector.Z);
						if (!flag9)
						{
							bool flag10 = vector3.Y > (float)(ushortVector.Y + ushortVector.W);
							if (!flag10)
							{
								ushort x2 = (ushort)((float)(65535 * fallbackGlyphAtlasRectangle.X) / (float)width);
								ushort x3 = (ushort)((float)(65535 * (fallbackGlyphAtlasRectangle.X + fallbackGlyphAtlasRectangle.Width)) / (float)width);
								ushort y2 = (ushort)((float)(65535 * fallbackGlyphAtlasRectangle.Y) / (float)height);
								ushort y3 = (ushort)((float)(65535 * (fallbackGlyphAtlasRectangle.Y + fallbackGlyphAtlasRectangle.Height)) / (float)height);
								this._vertices[num3 * 4].TextureCoordinates = new UShortVector2(x3, y2);
								this._vertices[num3 * 4 + 1].TextureCoordinates = new UShortVector2(x2, y2);
								this._vertices[num3 * 4 + 2].TextureCoordinates = new UShortVector2(x2, y3);
								this._vertices[num3 * 4 + 3].TextureCoordinates = new UShortVector2(x3, y3);
								this._vertices[num3 * 4].Scissor = ushortVector;
								this._vertices[num3 * 4 + 1].Scissor = ushortVector;
								this._vertices[num3 * 4 + 2].Scissor = ushortVector;
								this._vertices[num3 * 4 + 3].Scissor = ushortVector;
								this._vertices[num3 * 4].MaskTextureArea = zero;
								this._vertices[num3 * 4 + 1].MaskTextureArea = zero;
								this._vertices[num3 * 4 + 2].MaskTextureArea = zero;
								this._vertices[num3 * 4 + 3].MaskTextureArea = zero;
								this._vertices[num3 * 4].MaskBounds = maskBounds;
								this._vertices[num3 * 4 + 1].MaskBounds = maskBounds;
								this._vertices[num3 * 4 + 2].MaskBounds = maskBounds;
								this._vertices[num3 * 4 + 3].MaskBounds = maskBounds;
								this._vertices[num3 * 4].FontId = (uint)fontId;
								this._vertices[num3 * 4 + 1].FontId = (uint)fontId;
								this._vertices[num3 * 4 + 2].FontId = (uint)fontId;
								this._vertices[num3 * 4 + 3].FontId = (uint)fontId;
								this._vertices[num3 * 4].Position = Vector3.Transform(vector2, this._transformationMatrix);
								this._vertices[num3 * 4 + 1].Position = Vector3.Transform(vector3, this._transformationMatrix);
								this._vertices[num3 * 4 + 2].Position = Vector3.Transform(position2, this._transformationMatrix);
								this._vertices[num3 * 4 + 3].Position = Vector3.Transform(vector4, this._transformationMatrix);
								this._vertices[num3 * 4].FillColor = color;
								this._vertices[num3 * 4 + 1].FillColor = color;
								this._vertices[num3 * 4 + 2].FillColor = color;
								this._vertices[num3 * 4 + 3].FillColor = color;
								this._vertices[num3 * 4].OutlineColor = color;
								this._vertices[num3 * 4 + 1].OutlineColor = color;
								this._vertices[num3 * 4 + 2].OutlineColor = color;
								this._vertices[num3 * 4 + 3].OutlineColor = color;
								this._vertices[num3 * 4].FillThreshold = fillThreshold;
								this._vertices[num3 * 4].FillBlurAmount = fillBlurAmount;
								this._vertices[num3 * 4].OutlineThreshold = outlineThreshold;
								this._vertices[num3 * 4].OutlineBlurAmount = outlineBlurAmount;
								this._vertices[num3 * 4 + 1].FillThreshold = fillThreshold;
								this._vertices[num3 * 4 + 1].FillBlurAmount = fillBlurAmount;
								this._vertices[num3 * 4 + 1].OutlineThreshold = outlineThreshold;
								this._vertices[num3 * 4 + 1].OutlineBlurAmount = outlineBlurAmount;
								this._vertices[num3 * 4 + 2].FillThreshold = fillThreshold;
								this._vertices[num3 * 4 + 2].FillBlurAmount = fillBlurAmount;
								this._vertices[num3 * 4 + 2].OutlineThreshold = outlineThreshold;
								this._vertices[num3 * 4 + 2].OutlineBlurAmount = outlineBlurAmount;
								this._vertices[num3 * 4 + 3].FillThreshold = fillThreshold;
								this._vertices[num3 * 4 + 3].FillBlurAmount = fillBlurAmount;
								this._vertices[num3 * 4 + 3].OutlineThreshold = outlineThreshold;
								this._vertices[num3 * 4 + 3].OutlineBlurAmount = outlineBlurAmount;
								this._texturesPerQuad[num3] = GLTexture.None;
								this._maskTexturesPerQuad[num3] = gltexture;
								num3++;
								this._usedQuads = num3;
							}
						}
					}
				}
			}
		}

		// Token: 0x04003466 RID: 13414
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003467 RID: 13415
		private readonly int _maxQuads = 8192;

		// Token: 0x04003468 RID: 13416
		private readonly GraphicsDevice _graphics;

		// Token: 0x04003469 RID: 13417
		private readonly GLVertexArray _vertexArray;

		// Token: 0x0400346A RID: 13418
		private readonly GLBuffer _verticesBuffer;

		// Token: 0x0400346B RID: 13419
		private readonly GLBuffer _indicesBuffer;

		// Token: 0x0400346C RID: 13420
		private Batcher2DVertex[] _vertices;

		// Token: 0x0400346D RID: 13421
		private GLTexture[] _texturesPerQuad;

		// Token: 0x0400346E RID: 13422
		private GLTexture[] _maskTexturesPerQuad;

		// Token: 0x0400346F RID: 13423
		private int _usedQuads = 0;

		// Token: 0x04003470 RID: 13424
		private bool _allowBatcher2dToGrow;

		// Token: 0x04003471 RID: 13425
		private Matrix _transformationMatrix = Matrix.Identity;

		// Token: 0x04003472 RID: 13426
		private byte? _opacityOverride = null;

		// Token: 0x04003473 RID: 13427
		private readonly Stack<Batcher2D.MaskSetup> _maskStack = new Stack<Batcher2D.MaskSetup>();

		// Token: 0x04003474 RID: 13428
		private readonly Stack<Rectangle> _scissorStack = new Stack<Rectangle>();

		// Token: 0x02000F10 RID: 3856
		private class MaskSetup
		{
			// Token: 0x040049F6 RID: 18934
			public UShortVector4 Bounds;

			// Token: 0x040049F7 RID: 18935
			public TextureArea Area;
		}
	}
}
