using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using NLog;
using SDL2;

namespace HytaleClient.Graphics
{
	// Token: 0x020009AA RID: 2474
	public class GLFunctions
	{
		// Token: 0x06004F77 RID: 20343 RVA: 0x0016651B File Offset: 0x0016471B
		public void ResetDrawCallStats()
		{
			this.DrawCallsCount = 0;
			this.DrawnVertices = 0;
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x0016652C File Offset: 0x0016472C
		public GLFunctions()
		{
			bool flag = true;
			bool flag2 = false;
			foreach (FieldInfo fieldInfo in base.GetType().GetFields())
			{
				Type fieldType = fieldInfo.FieldType;
				bool flag3 = !fieldType.Name.StartsWith("gl");
				if (!flag3)
				{
					IntPtr intPtr = SDL.SDL_GL_GetProcAddress(fieldType.Name);
					bool flag4 = intPtr == IntPtr.Zero;
					if (flag4)
					{
						GLFunctions.Logger.Warn("{0} is not supported.", fieldType.Name);
						string text = SDL.SDL_GetError();
						bool flag5 = text != "";
						if (flag5)
						{
							GLFunctions.Logger.Warn("SDL_GetError(): \"{0}\"", text);
						}
						SDL.SDL_ClearError();
						bool flag6 = fieldInfo.GetCustomAttributes(typeof(GLFunctions.OptionalAttribute), false).Length == 0;
						if (flag6)
						{
							flag2 = true;
						}
						else
						{
							bool flag7 = fieldType.Name.StartsWith("glDebug");
							if (flag7)
							{
								flag = false;
							}
						}
					}
					else
					{
						fieldInfo.SetValue(this, Marshal.GetDelegateForFunctionPointer(intPtr, fieldType));
					}
				}
			}
			bool flag8 = flag2;
			if (flag8)
			{
				throw new Exception("Failed to find one or more required GL functions. View log for more info!");
			}
			bool flag9 = flag;
			if (flag9)
			{
				this.DebugMessageControlARB(GL.DONT_CARE, GL.DONT_CARE, GL.DONT_CARE, 0, IntPtr.Zero, true);
				this.DebugMessageControlARB(GL.DONT_CARE, GL.DEBUG_TYPE_OTHER_ARB, GL.DEBUG_SEVERITY_LOW_ARB, 0, IntPtr.Zero, false);
				this.DebugMessageControlARB(GL.DONT_CARE, GL.DEBUG_TYPE_OTHER_ARB, GL.DEBUG_SEVERITY_NOTIFICATION_ARB, 0, IntPtr.Zero, false);
				this.DebugMessageCallbackARB(GLFunctions.DebugCallbackDelegate, IntPtr.Zero);
			}
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x00166778 File Offset: 0x00164978
		private static void DebugCallback(GL source, GL type, uint id, GL severity, int length, IntPtr message, IntPtr userParam)
		{
			bool flag = id == 131186U || id == 131218U || source == GL.DEBUG_SOURCE_SHADER_COMPILER_ARB;
			if (!flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Marshal.PtrToStringAnsi(message));
				stringBuilder.AppendLine("\tSource: " + source.ToString());
				stringBuilder.AppendLine("\tType: " + type.ToString());
				stringBuilder.AppendLine("\tID: " + id.ToString());
				stringBuilder.AppendLine("\tSeverity: " + severity.ToString());
				GLFunctions.Logger.Info<StringBuilder>(stringBuilder);
			}
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x0016683C File Offset: 0x00164A3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CheckError(string headerMessage)
		{
			GL gl = this.GetError();
			bool flag = gl > GL.NO_ERROR;
			if (flag)
			{
				GLFunctions.Logger.Warn(headerMessage);
			}
			while (gl > GL.NO_ERROR)
			{
				GLFunctions.Logger.Warn("GL_" + gl.ToString() + " - ");
				gl = this.GetError();
			}
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001668A9 File Offset: 0x00164AA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Viewport(int x, int y, int width, int height)
		{
			this._viewport = new Rectangle(x, y, width, height);
			this.Internal_Viewport(x, y, width, height);
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x001668CD File Offset: 0x00164ACD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Viewport(Rectangle viewport)
		{
			this._viewport = viewport;
			this.Internal_Viewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x001668FB File Offset: 0x00164AFB
		public void AssertViewport(int x, int y, int width, int height)
		{
			this.AssertViewport(new Rectangle(x, y, width, height));
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x00166910 File Offset: 0x00164B10
		public void AssertViewport(Rectangle viewport)
		{
			Debug.Assert(this._viewport == viewport, string.Format("Expected viewport to be ({0}, {1}, {2}, {3}) but was ({4}, {5}, {6}, {7})", new object[]
			{
				viewport.X,
				viewport.Y,
				viewport.Width,
				viewport.Height,
				this._viewport.X,
				this._viewport.Y,
				this._viewport.Width,
				this._viewport.Height
			}));
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x001669C4 File Offset: 0x00164BC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgram(GPUProgram program)
		{
			this.Internal_UseProgram(program.ProgramId);
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x001669DC File Offset: 0x00164BDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetAttribLocation(GPUProgram program, string name)
		{
			return this.Internal_GetAttribLocation(program.ProgramId, name);
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x00166A00 File Offset: 0x00164C00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLVertexArray GenVertexArray()
		{
			uint id;
			this.Internal_GenVertexArrays(1, out id);
			return new GLVertexArray(id);
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x00166A27 File Offset: 0x00164C27
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexArray(GLVertexArray vertexArray)
		{
			this._activeVertexArray = vertexArray.InternalId;
			this.Internal_BindVertexArray(vertexArray.InternalId);
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00166A48 File Offset: 0x00164C48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArray(GLVertexArray vertexArray)
		{
			uint internalId = vertexArray.InternalId;
			this.Internal_DeleteVertexArrays(1, ref internalId);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x00166A6C File Offset: 0x00164C6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLBuffer GenBuffer()
		{
			uint id;
			this.Internal_GenBuffers(1, out id);
			return new GLBuffer(id);
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x00166A94 File Offset: 0x00164C94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteBuffer(GLBuffer buffer)
		{
			uint internalId = buffer.InternalId;
			this.Internal_DeleteBuffers(1, ref internalId);
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x00166AB8 File Offset: 0x00164CB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffer(GLVertexArray array, GL target, GLBuffer buffer)
		{
			Debug.Assert(this._activeVertexArray == array.InternalId, string.Format("Unexpected vertex array is bound, expected {0} but found ${1}.", array.InternalId, this._activeVertexArray));
			this.Internal_BindBuffer(target, buffer.InternalId);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x00166B0D File Offset: 0x00164D0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffer(GL target, GLBuffer buffer)
		{
			this.Internal_BindBuffer(target, buffer.InternalId);
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x00166B23 File Offset: 0x00164D23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribPointer(uint index, int size, GL type, bool normalized, int stride, IntPtr pointer)
		{
			this.Internal_VertexAttribPointer(index, size, type, normalized, stride, pointer);
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x00166B3B File Offset: 0x00164D3B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIPointer(uint index, int size, GL type, int stride, IntPtr pointer)
		{
			this.Internal_VertexAttribIPointer(index, size, type, stride, pointer);
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x00166B54 File Offset: 0x00164D54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebuffer GenFramebuffer()
		{
			uint id;
			this.Internal_GenFramebuffers(1, out id);
			return new GLFramebuffer(id);
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x00166B7B File Offset: 0x00164D7B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFramebuffer(GL target, GLFramebuffer framebuffer)
		{
			this._activeFramebufferId = framebuffer.InternalId;
			this.Internal_BindFramebuffer(target, framebuffer.InternalId);
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00166B9D File Offset: 0x00164D9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture2D(GL target, GL attachment, GL textarget, GLTexture texture, int level)
		{
			this.Internal_FramebufferTexture2D(target, attachment, textarget, texture.InternalId, level);
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x00166BB8 File Offset: 0x00164DB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffer(GLFramebuffer framebuffer)
		{
			uint internalId = framebuffer.InternalId;
			this.Internal_DeleteFramebuffers(1, ref internalId);
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x00166BDC File Offset: 0x00164DDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ActiveTexture(GL textureUnit)
		{
			this._activeTextureUnit = textureUnit - GL.TEXTURE0;
			this.Internal_ActiveTexture(textureUnit);
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x00166BF9 File Offset: 0x00164DF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssertActiveTexture(GL textureUnit)
		{
			Debug.Assert(textureUnit - GL.TEXTURE0 == this._activeTextureUnit, string.Format("Expected textureUnit must be GL.TEXTURE{0}, but it's GL.TEXTURE{1}", textureUnit, this._activeTextureUnit));
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00166C2C File Offset: 0x00164E2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLTexture GenTexture()
		{
			uint id;
			this.Internal_GenTextures(1, out id);
			return new GLTexture(id);
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x00166C53 File Offset: 0x00164E53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTexture(GL target, GLTexture texture)
		{
			this._boundTextureIds[(int)this._activeTextureUnit] = texture.InternalId;
			this.Internal_BindTexture(target, texture.InternalId);
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00166C7C File Offset: 0x00164E7C
		public void AssertTextureBound(GL textureUnit, GLTexture texture)
		{
			Debug.Assert(textureUnit >= GL.TEXTURE0, "textureUnit must be GL.TEXTURE0 or bigger");
			uint num = textureUnit - GL.TEXTURE0;
			uint num2 = this._boundTextureIds[(int)num];
			Debug.Assert(num2 == texture.InternalId, string.Format("Expected texture unit {0} to be bound to texture {1}, found {2} instead.", num, texture.InternalId, num2));
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00166CE4 File Offset: 0x00164EE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTexture(GLTexture texture)
		{
			uint internalId = texture.InternalId;
			this.Internal_DeleteTextures(1, ref internalId);
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x00166D08 File Offset: 0x00164F08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLSampler GenSampler()
		{
			uint id;
			this.Internal_GenSamplers(1, out id);
			return new GLSampler(id);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x00166D2F File Offset: 0x00164F2F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSampler(uint unit, GLSampler sampler)
		{
			this._boundSamplerIds[(int)this._activeTextureUnit] = sampler.InternalId;
			this.Internal_BindSampler(unit, sampler.InternalId);
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00166D58 File Offset: 0x00164F58
		public void AssertSamplerBound(uint unit, GLSampler sampler)
		{
			uint num = this._boundTextureIds[(int)unit];
			Debug.Assert(num == sampler.InternalId, string.Format("Expected texture unit {0} to be bound to texture {1}, found {2} instead.", unit, sampler.InternalId, num));
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x00166DA0 File Offset: 0x00164FA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSampler(GLSampler sampler)
		{
			uint internalId = sampler.InternalId;
			this.Internal_DeleteSamplers(1, ref internalId);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x00166DC4 File Offset: 0x00164FC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameteri(GLSampler sampler, GL pname, GL param)
		{
			uint internalId = sampler.InternalId;
			this.Internal_SamplerParameteri(internalId, pname, (int)param);
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x00166DE8 File Offset: 0x00164FE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLQuery GenQuery()
		{
			uint id;
			this.Internal_GenQueries(1, out id);
			return new GLQuery(id);
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00166E10 File Offset: 0x00165010
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteQuery(GLQuery query)
		{
			uint internalId = query.InternalId;
			this.Internal_DeleteQueries(1, ref internalId);
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x00166E34 File Offset: 0x00165034
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void QueryCounter(GLQuery query, GL target)
		{
			uint internalId = query.InternalId;
			this.Internal_QueryCounter(internalId, target);
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x00166E58 File Offset: 0x00165058
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQuery(GL target, GLQuery query)
		{
			uint internalId = query.InternalId;
			this.Internal_BeginQuery(target, internalId);
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x00166E7B File Offset: 0x0016507B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQuery(GL target)
		{
			this.Internal_EndQuery(target);
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x00166E8C File Offset: 0x0016508C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectuiv(GLQuery query, GL name, out uint param)
		{
			Debug.Assert(name == GL.QUERY_RESULT || name == GL.QUERY_RESULT_AVAILABLE, string.Format("Expected names are GL.QUERY_RESULT and GL.QUERY_RESULT_AVAILABLE , found {0} instead.", name));
			uint internalId = query.InternalId;
			this.Internal_GetQueryObjectuiv(internalId, name, out param);
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00166EDC File Offset: 0x001650DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui64v(GLQuery query, GL name, out ulong param)
		{
			Debug.Assert(name == GL.QUERY_RESULT || name == GL.QUERY_RESULT_AVAILABLE, string.Format("Expected names are GL.QUERY_RESULT and GL.QUERY_RESULT_AVAILABLE , found {0} instead.", name));
			uint internalId = query.InternalId;
			this.Internal_glGetQueryObjectui64v(internalId, name, out param);
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x00166F29 File Offset: 0x00165129
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArrays(GL mode, int first, int count)
		{
			this.DrawCallsCount++;
			this.DrawnVertices += count;
			this.Internal_DrawArrays(mode, first, count);
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00166F57 File Offset: 0x00165157
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElements(GL mode, int count, GL type, IntPtr indices)
		{
			this.DrawCallsCount++;
			this.DrawnVertices += count;
			this.Internal_DrawElements(mode, count, type, indices);
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x00166F87 File Offset: 0x00165187
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstanced(GL mode, int first, int count, int instancecount)
		{
			this.DrawCallsCount++;
			this.DrawnVertices += count * instancecount;
			this.Internal_DrawArraysInstanced(mode, first, count, instancecount);
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x00166FBA File Offset: 0x001651BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstanced(GL mode, int count, GL type, IntPtr indices, int instancecount)
		{
			this.DrawCallsCount++;
			this.DrawnVertices += count * instancecount;
			this.Internal_DrawElementsInstanced(mode, count, type, indices, instancecount);
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x00166FF0 File Offset: 0x001651F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetCapState(GL cap, bool enabled)
		{
			Debug.Assert(this._capStates.ContainsKey(cap), string.Format("GL.{0} isn't a known capability.", cap));
			this._capStates[cap] = enabled;
			if (enabled)
			{
				this.Internal_Enable(cap);
			}
			else
			{
				this.Internal_Disable(cap);
			}
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x0016704F File Offset: 0x0016524F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GL cap)
		{
			this.SetCapState(cap, true);
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x0016705B File Offset: 0x0016525B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GL cap)
		{
			this.SetCapState(cap, false);
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x00167068 File Offset: 0x00165268
		private void AssertCapState(GL cap, bool shouldBeEnabled)
		{
			Debug.Assert(this._capStates.ContainsKey(cap), string.Format("GL.{0} isn't a known capability.", cap));
			string arg = shouldBeEnabled ? "enabled" : "disabled";
			Debug.Assert(this._capStates[cap] == shouldBeEnabled, string.Format("Expected GL.{0} to be {1} but it wasn't.", cap, arg));
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x001670CE File Offset: 0x001652CE
		public void AssertEnabled(GL cap)
		{
			this.AssertCapState(cap, true);
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x001670DA File Offset: 0x001652DA
		public void AssertDisabled(GL cap)
		{
			this.AssertCapState(cap, false);
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x001670E6 File Offset: 0x001652E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthFunc(GL func)
		{
			this._depthFunc = func;
			this.Internal_DepthFunc(func);
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x001670FD File Offset: 0x001652FD
		public void AssertDepthFunc(GL func)
		{
			Debug.Assert(this._depthFunc == func, string.Format("Expected glDepthFunc to be ({0}) but it wasn't.", func));
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x0016711F File Offset: 0x0016531F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthMask(bool write)
		{
			this._depthMask = write;
			this.Internal_DepthMask(write);
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x00167136 File Offset: 0x00165336
		public void AssertDepthMask(bool write)
		{
			Debug.Assert(this._depthMask == write, string.Format("Expected glDepthMask to be ({0}) but it wasn't.", write));
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00167158 File Offset: 0x00165358
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFunc(GL sfactor, GL dfactor)
		{
			this._blendSourceRGB = sfactor;
			this._blendSourceAlpha = sfactor;
			this._blendDestinationRGB = dfactor;
			this._blendDestinationAlpha = dfactor;
			this.Internal_BlendFunc(sfactor, dfactor);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x00167188 File Offset: 0x00165388
		public void AssertBlendFunc(GL sfactor, GL dfactor)
		{
			Debug.Assert(this._blendSourceRGB == sfactor && this._blendSourceAlpha == sfactor && this._blendDestinationRGB == dfactor && this._blendDestinationAlpha == dfactor, string.Format("Expected glBlendFunc to be (GL.{0}, GL.{1}) but it (GL.{2}, GL.{3}) .", new object[]
			{
				sfactor,
				dfactor,
				this._blendSourceRGB,
				this._blendDestinationRGB
			}));
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00167201 File Offset: 0x00165401
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(GL srcRGB, GL dstRGB, GL srcAlpha, GL dstAlpha)
		{
			this._blendSourceRGB = srcRGB;
			this._blendDestinationRGB = dstRGB;
			this._blendSourceAlpha = srcAlpha;
			this._blendDestinationAlpha = dstAlpha;
			this.Internal_BlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha);
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x00167234 File Offset: 0x00165434
		public void AssertBlendFuncSeparate(GL srcRGB, GL dstRGB, GL srcAlpha, GL dstAlpha)
		{
			Debug.Assert(this._blendSourceRGB == srcRGB && this._blendDestinationRGB == dstRGB && this._blendSourceAlpha == srcAlpha && this._blendDestinationAlpha == dstAlpha, string.Format("Expected glBlendFuncSeparate to be (GL.{0}, GL.{1}, GL.{2}, GL.{3}, ) but it was (GL.{4}, GL.{5}, GL.{6}, GL.{7}) .", new object[]
			{
				srcRGB,
				dstRGB,
				srcAlpha,
				dstAlpha,
				this._blendSourceRGB,
				this._blendDestinationRGB,
				this._blendSourceAlpha,
				this._blendDestinationAlpha
			}));
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001672DD File Offset: 0x001654DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquation(GL mode)
		{
			this._blendEquationModeRGB = mode;
			this._blendEquationModeAlpha = mode;
			this.Internal_BlendEquation(mode);
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x001672FB File Offset: 0x001654FB
		public void AssertBlendEquation(GL mode)
		{
			Debug.Assert(this._blendEquationModeRGB == mode && this._blendEquationModeAlpha == mode, string.Format("Expected glBlendFunc to be (GL.{0}) but it was (GL.{1}) .", mode, this._blendEquationModeRGB));
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x00167334 File Offset: 0x00165534
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(GL modeRGB, GL modeAlpha)
		{
			this._blendEquationModeRGB = modeRGB;
			this._blendEquationModeAlpha = modeAlpha;
			this.Internal_BlendEquationSeparate(modeRGB, modeAlpha);
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x00167353 File Offset: 0x00165553
		public void AssertBlendEquationSeparate(GL modeRGB, GL modeAlpha)
		{
			Debug.Assert(this._blendEquationModeRGB == modeRGB && this._blendEquationModeAlpha == modeAlpha, string.Format("Expected glBlendFunc to be (GL.{0}, GL.{1}) but it wasn't.", modeRGB, modeAlpha));
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x00167387 File Offset: 0x00165587
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilFunc(GL func, int val, uint mask)
		{
			this._stencilFunc = func;
			this._stencilFuncVal = val;
			this._stencilFuncMask = mask;
			this.Internal_StencilFunc(func, val, mask);
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x001673B0 File Offset: 0x001655B0
		public void AssertStencilFunc(GL func, int val, uint mask)
		{
			Debug.Assert(this._stencilFunc == func && this._stencilFuncVal == val && this._stencilFuncMask == mask, string.Format("Expected glStencilFunc to be (GL.{0}, GL.{1}, , GL.{2}) but it wasn't.", func, val, mask));
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001673FE File Offset: 0x001655FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilOp(GL sfail, GL dpfail, GL dppass)
		{
			this._stencilOpSFail = sfail;
			this._stencilOpDPFail = dpfail;
			this._stencilOpDPPass = dppass;
			this.Internal_StencilOp(sfail, dpfail, dppass);
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x00167428 File Offset: 0x00165628
		public void AssertStencilOp(GL sfail, GL dpfail, GL dppass)
		{
			Debug.Assert(this._stencilOpSFail == sfail && this._stencilOpDPFail == dpfail && this._stencilOpDPPass == dppass, string.Format("Expected glStencilOp to be (GL.{0}, GL.{1}, , GL.{2}) but it wasn't.", sfail, dpfail, dppass));
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x00167476 File Offset: 0x00165676
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CullFace(GL mode)
		{
			this._cullFace = mode;
			this.Internal_CullFace(mode);
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x0016748D File Offset: 0x0016568D
		public void AssertCullFace(GL mode)
		{
			Debug.Assert(this._cullFace == mode, string.Format("Expected glCullFace to be (GL.{0}) but it wasn't.", mode));
		}

		// Token: 0x04002C16 RID: 11286
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002C17 RID: 11287
		public int DrawCallsCount = 0;

		// Token: 0x04002C18 RID: 11288
		public int DrawnVertices = 0;

		// Token: 0x04002C19 RID: 11289
		private static glDebugProc DebugCallbackDelegate = new glDebugProc(GLFunctions.DebugCallback);

		// Token: 0x04002C1A RID: 11290
		public readonly glGetError GetError;

		// Token: 0x04002C1B RID: 11291
		[GLFunctions.OptionalAttribute]
		public readonly glDebugMessageCallback DebugMessageCallbackARB;

		// Token: 0x04002C1C RID: 11292
		[GLFunctions.OptionalAttribute]
		public readonly glDebugMessageControl DebugMessageControlARB;

		// Token: 0x04002C1D RID: 11293
		public readonly glHint Hint;

		// Token: 0x04002C1E RID: 11294
		public readonly glGenQueries Internal_GenQueries;

		// Token: 0x04002C1F RID: 11295
		public readonly glDeleteQueries Internal_DeleteQueries;

		// Token: 0x04002C20 RID: 11296
		public readonly glQueryCounter Internal_QueryCounter;

		// Token: 0x04002C21 RID: 11297
		public readonly glBeginQuery Internal_BeginQuery;

		// Token: 0x04002C22 RID: 11298
		public readonly glEndQuery Internal_EndQuery;

		// Token: 0x04002C23 RID: 11299
		public readonly glGetQueryObjectuiv Internal_GetQueryObjectuiv;

		// Token: 0x04002C24 RID: 11300
		public readonly glGetQueryObjectui64v Internal_glGetQueryObjectui64v;

		// Token: 0x04002C25 RID: 11301
		public readonly glGetIntegerv GetIntegerv;

		// Token: 0x04002C26 RID: 11302
		public readonly glGetFloatv GetFloatv;

		// Token: 0x04002C27 RID: 11303
		public readonly glGetFramebufferAttachmentParameteriv GetFramebufferAttachmentParameteriv;

		// Token: 0x04002C28 RID: 11304
		public readonly glTransformFeedbackVaryings TransformFeedbackVaryings;

		// Token: 0x04002C29 RID: 11305
		public readonly glBeginTransformFeedback BeginTransformFeedback;

		// Token: 0x04002C2A RID: 11306
		public readonly glEndTransformFeedback EndTransformFeedback;

		// Token: 0x04002C2B RID: 11307
		public readonly glEnable Internal_Enable;

		// Token: 0x04002C2C RID: 11308
		public readonly glDisable Internal_Disable;

		// Token: 0x04002C2D RID: 11309
		public readonly glDepthFunc Internal_DepthFunc;

		// Token: 0x04002C2E RID: 11310
		public readonly glDepthMask Internal_DepthMask;

		// Token: 0x04002C2F RID: 11311
		public readonly glClearDepth ClearDepth;

		// Token: 0x04002C30 RID: 11312
		public readonly glColorMask ColorMask;

		// Token: 0x04002C31 RID: 11313
		public readonly glStencilFunc Internal_StencilFunc;

		// Token: 0x04002C32 RID: 11314
		public readonly glStencilOp Internal_StencilOp;

		// Token: 0x04002C33 RID: 11315
		public readonly glStencilFuncSeparate StencilFuncSeparate;

		// Token: 0x04002C34 RID: 11316
		public readonly glStencilOpSeparate StencilOpSeparate;

		// Token: 0x04002C35 RID: 11317
		public readonly glStencilMask StencilMask;

		// Token: 0x04002C36 RID: 11318
		public readonly glClearStencil ClearStencil;

		// Token: 0x04002C37 RID: 11319
		public readonly glBlendFunc Internal_BlendFunc;

		// Token: 0x04002C38 RID: 11320
		public readonly glBlendFuncSeparate Internal_BlendFuncSeparate;

		// Token: 0x04002C39 RID: 11321
		public readonly glBlendFunci BlendFunci;

		// Token: 0x04002C3A RID: 11322
		public readonly glBlendFuncSeparatei BlendFuncSeparatei;

		// Token: 0x04002C3B RID: 11323
		public readonly glBlendEquation Internal_BlendEquation;

		// Token: 0x04002C3C RID: 11324
		public readonly glBlendEquationSeparate Internal_BlendEquationSeparate;

		// Token: 0x04002C3D RID: 11325
		public readonly glCullFace Internal_CullFace;

		// Token: 0x04002C3E RID: 11326
		public readonly glPointSize PointSize;

		// Token: 0x04002C3F RID: 11327
		public readonly glClearColor ClearColor;

		// Token: 0x04002C40 RID: 11328
		public readonly glClear Clear;

		// Token: 0x04002C41 RID: 11329
		public readonly glClearBufferfv ClearBufferfv;

		// Token: 0x04002C42 RID: 11330
		public readonly glViewport Internal_Viewport;

		// Token: 0x04002C43 RID: 11331
		public readonly glPolygonMode PolygonMode;

		// Token: 0x04002C44 RID: 11332
		public readonly glPolygonOffset PolygonOffset;

		// Token: 0x04002C45 RID: 11333
		public readonly glScissor Scissor;

		// Token: 0x04002C46 RID: 11334
		public readonly glActiveTexture Internal_ActiveTexture;

		// Token: 0x04002C47 RID: 11335
		public readonly glGenTextures Internal_GenTextures;

		// Token: 0x04002C48 RID: 11336
		public readonly glDeleteTextures Internal_DeleteTextures;

		// Token: 0x04002C49 RID: 11337
		public readonly glBindTexture Internal_BindTexture;

		// Token: 0x04002C4A RID: 11338
		public readonly glTexBuffer TexBuffer;

		// Token: 0x04002C4B RID: 11339
		public readonly glTexParameteri TexParameteri;

		// Token: 0x04002C4C RID: 11340
		public readonly glTexParameterf TexParameterf;

		// Token: 0x04002C4D RID: 11341
		public readonly glTexParameterfv TexParameterfv;

		// Token: 0x04002C4E RID: 11342
		public readonly glTexImage1D TexImage1D;

		// Token: 0x04002C4F RID: 11343
		public readonly glTexImage2D TexImage2D;

		// Token: 0x04002C50 RID: 11344
		public readonly glTexImage3D TexImage3D;

		// Token: 0x04002C51 RID: 11345
		public readonly glGenerateMipmap GenerateMipmap;

		// Token: 0x04002C52 RID: 11346
		public readonly glTexSubImage2D TexSubImage2D;

		// Token: 0x04002C53 RID: 11347
		public readonly glTexSubImage3D TexSubImage3D;

		// Token: 0x04002C54 RID: 11348
		public readonly glCopyTexSubImage2D CopyTexSubImage2D;

		// Token: 0x04002C55 RID: 11349
		public readonly glGetTexImage GetTexImage;

		// Token: 0x04002C56 RID: 11350
		public readonly glTexImage2DMultisample TexImage2DMultisample;

		// Token: 0x04002C57 RID: 11351
		public readonly glGenSamplers Internal_GenSamplers;

		// Token: 0x04002C58 RID: 11352
		public readonly glDeleteSamplers Internal_DeleteSamplers;

		// Token: 0x04002C59 RID: 11353
		public readonly glBindSampler Internal_BindSampler;

		// Token: 0x04002C5A RID: 11354
		public readonly glSamplerParameteri Internal_SamplerParameteri;

		// Token: 0x04002C5B RID: 11355
		public readonly glGenFramebuffers Internal_GenFramebuffers;

		// Token: 0x04002C5C RID: 11356
		public readonly glDeleteFramebuffers Internal_DeleteFramebuffers;

		// Token: 0x04002C5D RID: 11357
		public readonly glBindFramebuffer Internal_BindFramebuffer;

		// Token: 0x04002C5E RID: 11358
		public readonly glFramebufferTexture2D Internal_FramebufferTexture2D;

		// Token: 0x04002C5F RID: 11359
		public readonly glCheckFramebufferStatus CheckFramebufferStatus;

		// Token: 0x04002C60 RID: 11360
		public readonly glDrawBuffer DrawBuffer;

		// Token: 0x04002C61 RID: 11361
		public readonly glDrawBuffers DrawBuffers;

		// Token: 0x04002C62 RID: 11362
		public readonly glBlitFramebuffer BlitFramebuffer;

		// Token: 0x04002C63 RID: 11363
		public readonly glGenVertexArrays Internal_GenVertexArrays;

		// Token: 0x04002C64 RID: 11364
		public readonly glDeleteVertexArrays Internal_DeleteVertexArrays;

		// Token: 0x04002C65 RID: 11365
		public readonly glBindVertexArray Internal_BindVertexArray;

		// Token: 0x04002C66 RID: 11366
		public readonly glGenBuffers Internal_GenBuffers;

		// Token: 0x04002C67 RID: 11367
		public readonly glDeleteBuffers Internal_DeleteBuffers;

		// Token: 0x04002C68 RID: 11368
		public readonly glBindBuffer Internal_BindBuffer;

		// Token: 0x04002C69 RID: 11369
		public readonly glBindBufferBase BindBufferBase;

		// Token: 0x04002C6A RID: 11370
		public readonly glBindBufferRange BindBufferRange;

		// Token: 0x04002C6B RID: 11371
		public readonly glBufferData BufferData;

		// Token: 0x04002C6C RID: 11372
		public readonly glBufferSubData BufferSubData;

		// Token: 0x04002C6D RID: 11373
		public readonly glGetBufferSubData GetBufferSubData;

		// Token: 0x04002C6E RID: 11374
		public readonly glMapBufferRange MapBufferRange;

		// Token: 0x04002C6F RID: 11375
		public readonly glUnmapBuffer UnmapBuffer;

		// Token: 0x04002C70 RID: 11376
		public readonly glCreateShader CreateShader;

		// Token: 0x04002C71 RID: 11377
		public readonly glShaderSource ShaderSource;

		// Token: 0x04002C72 RID: 11378
		public readonly glCompileShader CompileShader;

		// Token: 0x04002C73 RID: 11379
		public readonly glDeleteShader DeleteShader;

		// Token: 0x04002C74 RID: 11380
		public readonly glGetShaderiv GetShaderiv;

		// Token: 0x04002C75 RID: 11381
		public readonly glGetShaderInfoLog GetShaderInfoLog;

		// Token: 0x04002C76 RID: 11382
		public readonly glCreateProgram CreateProgram;

		// Token: 0x04002C77 RID: 11383
		public readonly glDeleteProgram DeleteProgram;

		// Token: 0x04002C78 RID: 11384
		public readonly glAttachShader AttachShader;

		// Token: 0x04002C79 RID: 11385
		public readonly glDetachShader DetachShader;

		// Token: 0x04002C7A RID: 11386
		public readonly glBindAttribLocation BindAttribLocation;

		// Token: 0x04002C7B RID: 11387
		public readonly glLinkProgram LinkProgram;

		// Token: 0x04002C7C RID: 11388
		public readonly glUseProgram Internal_UseProgram;

		// Token: 0x04002C7D RID: 11389
		public readonly glGetProgramiv GetProgramiv;

		// Token: 0x04002C7E RID: 11390
		public readonly glGetProgramInfoLog GetProgramInfoLog;

		// Token: 0x04002C7F RID: 11391
		public readonly glGetUniformBlockIndex GetUniformBlockIndex;

		// Token: 0x04002C80 RID: 11392
		public readonly glUniformBlockBinding UniformBlockBinding;

		// Token: 0x04002C81 RID: 11393
		public readonly glGetUniformLocation GetUniformLocation;

		// Token: 0x04002C82 RID: 11394
		public readonly glUniform1i Uniform1i;

		// Token: 0x04002C83 RID: 11395
		public readonly glUniform1iv Uniform1iv;

		// Token: 0x04002C84 RID: 11396
		public readonly glUniform2i Uniform2i;

		// Token: 0x04002C85 RID: 11397
		public readonly glUniform3i Uniform3i;

		// Token: 0x04002C86 RID: 11398
		public readonly glUniform4i Uniform4i;

		// Token: 0x04002C87 RID: 11399
		public readonly glUniform1f Uniform1f;

		// Token: 0x04002C88 RID: 11400
		public readonly glUniform2f Uniform2f;

		// Token: 0x04002C89 RID: 11401
		public readonly glUniform3f Uniform3f;

		// Token: 0x04002C8A RID: 11402
		public readonly glUniform4f Uniform4f;

		// Token: 0x04002C8B RID: 11403
		public readonly glUniform1fv Uniform1fv;

		// Token: 0x04002C8C RID: 11404
		public readonly glUniform2fv Uniform2fv;

		// Token: 0x04002C8D RID: 11405
		public readonly glUniform3fv Uniform3fv;

		// Token: 0x04002C8E RID: 11406
		public readonly glUniform4fv Uniform4fv;

		// Token: 0x04002C8F RID: 11407
		public readonly glUniformMatrix4fv UniformMatrix4fv;

		// Token: 0x04002C90 RID: 11408
		public readonly glGetAttribLocation Internal_GetAttribLocation;

		// Token: 0x04002C91 RID: 11409
		public readonly glEnableVertexAttribArray EnableVertexAttribArray;

		// Token: 0x04002C92 RID: 11410
		public readonly glVertexAttribPointer Internal_VertexAttribPointer;

		// Token: 0x04002C93 RID: 11411
		public readonly glVertexAttribIPointer Internal_VertexAttribIPointer;

		// Token: 0x04002C94 RID: 11412
		public readonly glVertexAttribI2i VertexAttribI2i;

		// Token: 0x04002C95 RID: 11413
		public readonly glDrawArrays Internal_DrawArrays;

		// Token: 0x04002C96 RID: 11414
		public readonly glDrawElements Internal_DrawElements;

		// Token: 0x04002C97 RID: 11415
		public readonly glDrawArraysInstanced Internal_DrawArraysInstanced;

		// Token: 0x04002C98 RID: 11416
		public readonly glDrawElementsInstanced Internal_DrawElementsInstanced;

		// Token: 0x04002C99 RID: 11417
		public readonly glReadBuffer ReadBuffer;

		// Token: 0x04002C9A RID: 11418
		public readonly glReadPixels ReadPixels;

		// Token: 0x04002C9B RID: 11419
		public readonly glFenceSync FenceSync;

		// Token: 0x04002C9C RID: 11420
		public readonly glDeleteSync DeleteSync;

		// Token: 0x04002C9D RID: 11421
		public readonly glGetSynciv GetSynciv;

		// Token: 0x04002C9E RID: 11422
		public readonly glGetString GetString;

		// Token: 0x04002C9F RID: 11423
		public readonly glFlush Flush;

		// Token: 0x04002CA0 RID: 11424
		private Rectangle _viewport;

		// Token: 0x04002CA1 RID: 11425
		private uint _activeVertexArray;

		// Token: 0x04002CA2 RID: 11426
		private const int MaxTextureUnits = 21;

		// Token: 0x04002CA3 RID: 11427
		private readonly uint[] _boundTextureIds = new uint[21];

		// Token: 0x04002CA4 RID: 11428
		private readonly uint[] _boundSamplerIds = new uint[21];

		// Token: 0x04002CA5 RID: 11429
		private uint _activeTextureUnit;

		// Token: 0x04002CA6 RID: 11430
		private uint _activeFramebufferId;

		// Token: 0x04002CA7 RID: 11431
		private readonly Dictionary<GL, bool> _capStates = new Dictionary<GL, bool>
		{
			{
				GL.CULL_FACE,
				false
			},
			{
				GL.BLEND,
				false
			},
			{
				GL.DEPTH_TEST,
				false
			},
			{
				GL.STENCIL_TEST,
				false
			},
			{
				GL.SCISSOR_TEST,
				false
			},
			{
				GL.POLYGON_OFFSET_FILL,
				false
			},
			{
				GL.RASTERIZER_DISCARD,
				false
			}
		};

		// Token: 0x04002CA8 RID: 11432
		private bool _depthMask;

		// Token: 0x04002CA9 RID: 11433
		private GL _depthFunc;

		// Token: 0x04002CAA RID: 11434
		private GL _blendSourceRGB;

		// Token: 0x04002CAB RID: 11435
		private GL _blendDestinationRGB;

		// Token: 0x04002CAC RID: 11436
		private GL _blendSourceAlpha;

		// Token: 0x04002CAD RID: 11437
		private GL _blendDestinationAlpha;

		// Token: 0x04002CAE RID: 11438
		private GL _blendEquationModeRGB;

		// Token: 0x04002CAF RID: 11439
		private GL _blendEquationModeAlpha;

		// Token: 0x04002CB0 RID: 11440
		private GL _stencilFunc;

		// Token: 0x04002CB1 RID: 11441
		private int _stencilFuncVal;

		// Token: 0x04002CB2 RID: 11442
		private uint _stencilFuncMask;

		// Token: 0x04002CB3 RID: 11443
		private GL _stencilOpSFail;

		// Token: 0x04002CB4 RID: 11444
		private GL _stencilOpDPFail;

		// Token: 0x04002CB5 RID: 11445
		private GL _stencilOpDPPass;

		// Token: 0x04002CB6 RID: 11446
		private GL _cullFace;

		// Token: 0x02000EA5 RID: 3749
		[AttributeUsage(AttributeTargets.Field)]
		public class OptionalAttribute : Attribute
		{
		}
	}
}
