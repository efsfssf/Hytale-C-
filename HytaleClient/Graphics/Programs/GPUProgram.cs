using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NLog;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A6A RID: 2666
	public abstract class GPUProgram
	{
		// Token: 0x06005476 RID: 21622 RVA: 0x00183F38 File Offset: 0x00182138
		public static void InitializeGL(GLFunctions gl)
		{
			GPUProgram._gl = gl;
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x00183F40 File Offset: 0x00182140
		public static void ReleaseGL()
		{
			GPUProgram._gl = null;
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x00183F48 File Offset: 0x00182148
		public static void SetShaderCodeDumpPolicy(GPUProgram.ShaderCodeDumpPolicy policy)
		{
			GPUProgram._dumpPolicy = policy;
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x00183F50 File Offset: 0x00182150
		public static void SetResourcePaths(string shaderResourceAssemblyPath, string shaderResourcePath, string shaderCodeDumpPath)
		{
			GPUProgram._shaderResourceAssemblyPath = shaderResourceAssemblyPath;
			GPUProgram._shaderResourcePath = shaderResourcePath;
			GPUProgram._shaderCodeDumpPath = shaderCodeDumpPath;
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x0600547A RID: 21626 RVA: 0x00183F65 File Offset: 0x00182165
		// (set) Token: 0x0600547B RID: 21627 RVA: 0x00183F6D File Offset: 0x0018216D
		public uint ProgramId { get; private set; }

		// Token: 0x0600547C RID: 21628 RVA: 0x00183F78 File Offset: 0x00182178
		protected static bool IsResourceBindingLayoutValid<T>(T layout)
		{
			bool flag = true;
			object obj = layout;
			foreach (FieldInfo fieldInfo in typeof(T).GetFields())
			{
				foreach (FieldInfo fieldInfo2 in typeof(T).GetFields())
				{
					flag = (flag && (fieldInfo.Name == fieldInfo2.Name || (byte)fieldInfo.GetValue(obj) != (byte)fieldInfo2.GetValue(obj)));
				}
			}
			return flag;
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x00184028 File Offset: 0x00182228
		protected GPUProgram(string vertexShaderFileName, string geometryShaderFileName, string fragmentShaderFileName, string variationName = null)
		{
			this._variationName = ((variationName != null) ? variationName : base.GetType().Name);
			this._vertexShaderResource.FileName = vertexShaderFileName;
			this._geometryShaderResource.FileName = geometryShaderFileName;
			this._fragmentShaderResource.FileName = fragmentShaderFileName;
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x0018407A File Offset: 0x0018227A
		protected GPUProgram(string vertexShaderFileName, string fragmentShaderFileName, string variationName = null) : this(vertexShaderFileName, null, fragmentShaderFileName, variationName)
		{
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x00184088 File Offset: 0x00182288
		public virtual bool Initialize()
		{
			GPUProgram.Logger.Info<string, string>("Building GPU Program \"{0} (class: {1})\"", this._variationName, base.GetType().Name);
			return true;
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x001840BC File Offset: 0x001822BC
		public virtual void Release()
		{
			List<string> includes = this._vertexShaderResource.Includes;
			if (includes != null)
			{
				includes.Clear();
			}
			List<string> includes2 = this._geometryShaderResource.Includes;
			if (includes2 != null)
			{
				includes2.Clear();
			}
			List<string> includes3 = this._fragmentShaderResource.Includes;
			if (includes3 != null)
			{
				includes3.Clear();
			}
			GPUProgram._gl.DeleteProgram(this.ProgramId);
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x00184128 File Offset: 0x00182328
		public bool Reset(bool forceReset = true)
		{
			bool flag = !forceReset && !this.HasChangedSinceLastCompile();
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				this.Release();
				result = this.Initialize();
			}
			return result;
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x0018415E File Offset: 0x0018235E
		protected virtual void InitUniforms()
		{
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x00184164 File Offset: 0x00182364
		public static bool CreateFallbacks()
		{
			GPUProgram._fallbackVertexShader = GPUProgram.CompileShader(GL.VERTEX_SHADER, "#version 150 core\n uniform mat4 uMVPMatrix; in vec3 vertPosition; void main() { gl_Position = uMVPMatrix * vec4(vertPosition, 1.0); }", 0, 0);
			GPUProgram._fallbackGeometryShader = 0U;
			GPUProgram._fallbackFragmentShader = GPUProgram.CompileShader(GL.FRAGMENT_SHADER, "#version 150 core\n out vec4 outColor; void main() { outColor = vec4(1,0,0,1); }", 0, 0);
			GPUProgram._fallbackProgram = GPUProgram._gl.CreateProgram();
			GPUProgram._gl.AttachShader(GPUProgram._fallbackProgram, GPUProgram._fallbackVertexShader);
			GPUProgram._gl.AttachShader(GPUProgram._fallbackProgram, GPUProgram._fallbackFragmentShader);
			return GPUProgram.LinkProgram("Fallback Program", ref GPUProgram._fallbackProgram);
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x00184201 File Offset: 0x00182401
		public static void DestroyFallbacks()
		{
			GPUProgram._gl.DeleteShader(GPUProgram._fallbackVertexShader);
			GPUProgram._gl.DeleteShader(GPUProgram._fallbackFragmentShader);
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x00184230 File Offset: 0x00182430
		public void AssertInUse()
		{
			int[] array = new int[1];
			GPUProgram._gl.GetIntegerv(GL.CURRENT_PROGRAM, array);
			bool flag = (ulong)this.ProgramId != (ulong)((long)array[0]);
			if (flag)
			{
				GPUProgram.Logger.Info<string, string>("Program {0} (class:{1}) isn't current!", this._variationName, base.GetType().Name);
			}
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x00184290 File Offset: 0x00182490
		protected bool MakeProgram(uint vertexShader, List<GPUProgram.AttribBindingInfo> attribLocations = null, bool ignoreMissingUniforms = false, string[] transformFeedbackVaryings = null)
		{
			return this.MakeProgram((int)vertexShader, -1, -1, attribLocations, ignoreMissingUniforms, transformFeedbackVaryings);
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x001842B0 File Offset: 0x001824B0
		protected bool MakeProgram(uint vertexShader, uint fragmentShader, List<GPUProgram.AttribBindingInfo> attribLocations = null, bool ignoreMissingUniforms = false, string[] transformFeedbackVaryings = null)
		{
			return this.MakeProgram((int)vertexShader, -1, (int)fragmentShader, attribLocations, ignoreMissingUniforms, transformFeedbackVaryings);
		}

		// Token: 0x06005488 RID: 21640 RVA: 0x001842D0 File Offset: 0x001824D0
		protected bool MakeProgram(uint vertexShader, uint geometryShader, uint fragmentShader, List<GPUProgram.AttribBindingInfo> attribLocations = null, bool ignoreMissingUniforms = false, string[] transformFeedbackVaryings = null)
		{
			return this.MakeProgram((int)vertexShader, (int)geometryShader, (int)fragmentShader, attribLocations, ignoreMissingUniforms, transformFeedbackVaryings);
		}

		// Token: 0x06005489 RID: 21641 RVA: 0x001842F4 File Offset: 0x001824F4
		private bool MakeProgram(int vertexShader, int geometryShader, int fragmentShader, List<GPUProgram.AttribBindingInfo> attribLocations = null, bool ignoreMissingUniforms = false, string[] transformFeedbackVaryings = null)
		{
			bool flag = vertexShader != 0 && geometryShader != 0 && fragmentShader != 0;
			bool flag2 = flag;
			if (flag2)
			{
				this.ProgramId = GPUProgram._gl.CreateProgram();
				bool flag3 = attribLocations != null;
				if (flag3)
				{
					foreach (GPUProgram.AttribBindingInfo attribBindingInfo in attribLocations)
					{
						GPUProgram._gl.BindAttribLocation(this.ProgramId, attribBindingInfo.Index, attribBindingInfo.Name);
					}
				}
				GPUProgram._gl.AttachShader(this.ProgramId, (uint)vertexShader);
				bool flag4 = geometryShader != -1;
				if (flag4)
				{
					GPUProgram._gl.AttachShader(this.ProgramId, (uint)geometryShader);
				}
				bool flag5 = fragmentShader != -1;
				if (flag5)
				{
					GPUProgram._gl.AttachShader(this.ProgramId, (uint)fragmentShader);
				}
				bool flag6 = transformFeedbackVaryings != null;
				if (flag6)
				{
					GPUProgram._gl.TransformFeedbackVaryings(this.ProgramId, transformFeedbackVaryings.Length, transformFeedbackVaryings, GL.INTERLEAVED_ATTRIBS);
				}
				uint programId = this.ProgramId;
				flag = GPUProgram.LinkProgram(base.GetType().Name, ref programId);
				this.ProgramId = programId;
				bool flag7 = vertexShader > 0;
				if (flag7)
				{
					GPUProgram._gl.DeleteShader((uint)vertexShader);
				}
				bool flag8 = geometryShader > 0;
				if (flag8)
				{
					GPUProgram._gl.DeleteShader((uint)geometryShader);
				}
				bool flag9 = fragmentShader > 0;
				if (flag9)
				{
					GPUProgram._gl.DeleteShader((uint)fragmentShader);
				}
				foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					bool flag10 = fieldInfo.FieldType == typeof(UniformBufferObject);
					if (flag10)
					{
						string text = "ubo" + fieldInfo.Name;
						uint num = GPUProgram._gl.GetUniformBlockIndex(this.ProgramId, text);
						bool flag11 = num == uint.MaxValue;
						if (flag11)
						{
							GPUProgram.Logger.Warn("- Could not find uniform buffer object {0}.", text);
							fieldInfo.SetValue(this, new UniformBufferObject(this, num, text));
						}
						fieldInfo.SetValue(this, new UniformBufferObject(this, num, text));
					}
					else
					{
						bool flag12 = fieldInfo.FieldType == typeof(Uniform);
						if (flag12)
						{
							string text2 = "u" + fieldInfo.Name;
							int num2 = GPUProgram._gl.GetUniformLocation(this.ProgramId, text2);
							bool flag13 = num2 == -1;
							if (flag13)
							{
								GPUProgram.Logger.Warn("- Could not find uniform {0}.", text2);
								fieldInfo.SetValue(this, new Uniform(-1, text2, this));
							}
							fieldInfo.SetValue(this, new Uniform(num2, text2, this));
						}
						else
						{
							bool flag14 = fieldInfo.FieldType == typeof(Attrib);
							if (flag14)
							{
								string text3 = "vert" + fieldInfo.Name.Substring("Attrib".Length);
								int attribLocation = GPUProgram._gl.GetAttribLocation(this, text3);
								bool flag15 = attribLocation == -1;
								if (flag15)
								{
									GPUProgram.Logger.Error("- Could not find attrib {0}.", text3);
									fieldInfo.SetValue(this, new Attrib(uint.MaxValue, text3));
									flag = false;
								}
								fieldInfo.SetValue(this, new Attrib((uint)attribLocation, text3));
							}
						}
					}
				}
				this.InitUniforms();
			}
			return flag;
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x001846C0 File Offset: 0x001828C0
		private static bool LinkProgram(string programName, ref uint program)
		{
			bool result = true;
			GPUProgram._gl.LinkProgram(program);
			int num = 0;
			GPUProgram._gl.GetProgramiv(program, GL.LINK_STATUS, out num);
			bool flag = num == 0;
			if (flag)
			{
				int num2;
				GPUProgram._gl.GetProgramiv(program, GL.INFO_LOG_LENGTH, out num2);
				byte[] array = new byte[num2];
				GPUProgram._gl.GetProgramInfoLog(program, num2, out num2, array);
				string @string = Encoding.UTF8.GetString(array);
				GPUProgram.Logger.Warn(@string);
				GPUProgram.Logger.Warn<uint, string>("- Program {0} {1} failed to link!", program, programName);
				GPUProgram.Logger.Warn("- Using Fallback program instead!");
				program = GPUProgram._fallbackProgram;
				result = false;
			}
			return result;
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x00184790 File Offset: 0x00182990
		public bool ResetUniforms()
		{
			uint programId = this.ProgramId;
			bool flag = GPUProgram.LinkProgram(base.GetType().Name, ref programId);
			this.ProgramId = programId;
			foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				bool flag2 = fieldInfo.FieldType != typeof(Uniform);
				if (!flag2)
				{
					Uniform uniform = (Uniform)fieldInfo.GetValue(this);
					uniform.Reset();
					fieldInfo.SetValue(this, uniform);
				}
			}
			bool flag3 = flag;
			if (flag3)
			{
				this.InitUniforms();
			}
			return flag;
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x00184840 File Offset: 0x00182A40
		private string GetShaderFullPath(string shaderName)
		{
			return Path.GetFullPath(Path.Combine(GPUProgram._shaderResourcePath, shaderName));
		}

		// Token: 0x0600548D RID: 21645 RVA: 0x00184864 File Offset: 0x00182A64
		private string GetShaderSource(string shaderName, out DateTime lastLoadtime)
		{
			string result;
			try
			{
				string shaderFullPath = this.GetShaderFullPath(shaderName);
				StreamReader streamReader = new StreamReader(shaderFullPath);
				result = streamReader.ReadToEnd();
				streamReader.Close();
				lastLoadtime = File.GetLastWriteTimeUtc(shaderFullPath);
			}
			catch (Exception innerException)
			{
				throw new Exception("- GetShaderSource failed! Could not load the source code for '" + shaderName + "'", innerException);
			}
			return result;
		}

		// Token: 0x0600548E RID: 21646 RVA: 0x001848D0 File Offset: 0x00182AD0
		protected void DumpShaderCodeToFile(string shaderName, string shaderSource)
		{
			string fullPath = Path.GetFullPath(GPUProgram._shaderCodeDumpPath);
			bool flag = !Directory.Exists(fullPath);
			if (flag)
			{
				Directory.CreateDirectory(fullPath);
			}
			string path = "Full_" + this._variationName + "_" + shaderName;
			string text = Path.Combine(fullPath, path);
			using (FileStream fileStream = File.Open(text, FileMode.Create))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.Write(shaderSource);
				}
			}
		}

		// Token: 0x0600548F RID: 21647 RVA: 0x00184974 File Offset: 0x00182B74
		protected bool HasChangedSinceLastCompile()
		{
			return false || this.HasChangedSinceLastCompile(this._vertexShaderResource) || this.HasChangedSinceLastCompile(this._geometryShaderResource) || this.HasChangedSinceLastCompile(this._fragmentShaderResource);
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x001849C4 File Offset: 0x00182BC4
		private bool HasChangedSinceLastCompile(GPUProgram.ShaderResource shaderResource)
		{
			bool flag = false;
			bool flag2 = shaderResource.FileName != null;
			if (flag2)
			{
				flag = (File.GetLastWriteTimeUtc(this.GetShaderFullPath(shaderResource.FileName)) > shaderResource.LastLoadTime);
				bool flag3 = !flag;
				if (flag3)
				{
					int num = 0;
					for (;;)
					{
						int num2 = num;
						List<string> includes = shaderResource.Includes;
						int? num3 = (includes != null) ? new int?(includes.Count) : null;
						if (!(num2 < num3.GetValueOrDefault() & num3 != null) || flag)
						{
							break;
						}
						flag = (File.GetLastWriteTimeUtc(this.GetShaderFullPath(shaderResource.Includes[num])) > shaderResource.LastLoadTime);
						num++;
					}
				}
			}
			return flag;
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x00184A84 File Offset: 0x00182C84
		protected uint CompileVertexShader(Dictionary<string, string> defines = null)
		{
			return this.CompileShaderResource(ref this._vertexShaderResource, GL.VERTEX_SHADER, defines);
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x00184AA8 File Offset: 0x00182CA8
		protected uint CompileGeometryShader(Dictionary<string, string> defines = null)
		{
			return this.CompileShaderResource(ref this._geometryShaderResource, GL.GEOMETRY_SHADER, defines);
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x00184ACC File Offset: 0x00182CCC
		protected uint CompileFragmentShader(Dictionary<string, string> defines = null)
		{
			return this.CompileShaderResource(ref this._fragmentShaderResource, GL.FRAGMENT_SHADER, defines);
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x00184AF0 File Offset: 0x00182CF0
		private uint CompileShaderResource(ref GPUProgram.ShaderResource shaderResource, GL shaderType, Dictionary<string, string> defines = null)
		{
			string text = this.GetShaderSource(shaderResource.FileName, out shaderResource.LastLoadTime);
			int definesLineCount;
			text = this.InjectDefines(shaderResource.FileName, text, defines, out definesLineCount);
			string regex = "#include\\s*\"(?<File>[^\"]*)\"";
			int includeLineCount = 0;
			text = this.InjectIncludes(ref shaderResource, text, regex, 0, ref includeLineCount);
			uint num = GPUProgram.CompileShader(shaderType, text, definesLineCount, includeLineCount);
			bool flag = num == 0U;
			bool flag2 = flag;
			if (flag2)
			{
				GPUProgram.Logger.Warn("ERROR : Shader compilation failed for resource {0}.", shaderResource.FileName);
			}
			bool flag3 = GPUProgram._dumpPolicy == GPUProgram.ShaderCodeDumpPolicy.Always || (GPUProgram._dumpPolicy == GPUProgram.ShaderCodeDumpPolicy.OnError && flag);
			if (flag3)
			{
				string shaderName = shaderResource.FileName + ((shaderType == GL.VERTEX_SHADER) ? ".vert" : ((shaderType == GL.FRAGMENT_SHADER) ? ".frag" : ".geom"));
				this.DumpShaderCodeToFile(shaderName, text);
			}
			return num;
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x00184BC8 File Offset: 0x00182DC8
		protected string InjectDefines(string shaderName, string shaderSource, Dictionary<string, string> defines, out int defineLineCount)
		{
			defineLineCount = 0;
			bool flag = defines != null;
			string result;
			if (flag)
			{
				string str = "//##################################### BEGIN: CPU - DEFINES #####################################\n";
				string str2 = "//##################################### END: CPU - DEFINES #####################################\n";
				string text = "";
				foreach (KeyValuePair<string, string> keyValuePair in defines)
				{
					text = string.Concat(new string[]
					{
						text,
						"#define ",
						keyValuePair.Key,
						" ",
						keyValuePair.Value,
						"\n"
					});
				}
				text = str + text + str2;
				defineLineCount = defines.Count + 2;
				int num = shaderSource.LastIndexOf("#extension");
				bool flag2 = num == -1;
				if (flag2)
				{
					shaderSource.LastIndexOf("#version");
				}
				bool flag3 = num == -1;
				if (flag3)
				{
					num = 0;
				}
				num = shaderSource.IndexOf('\n', num);
				result = shaderSource.Insert(num + 1, text);
			}
			else
			{
				result = shaderSource;
			}
			return result;
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x00184CE4 File Offset: 0x00182EE4
		protected string InjectIncludes(ref GPUProgram.ShaderResource shaderResource, string source, string regex, int includeDepth, ref int includeLineCount)
		{
			includeDepth++;
			bool flag = includeDepth > 5;
			if (flag)
			{
				throw new Exception("Too many includes!");
			}
			MatchCollection matchCollection = Regex.Matches(source, regex);
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				string value = match.Groups["File"].Value;
				DateTime dateTime;
				string text = this.GetShaderSource(value, out dateTime);
				shaderResource.LastLoadTime = ((shaderResource.LastLoadTime < dateTime) ? dateTime : shaderResource.LastLoadTime);
				bool flag2 = shaderResource.Includes == null;
				if (flag2)
				{
					shaderResource.Includes = new List<string>();
				}
				shaderResource.Includes.Add(value);
				string str = "//##################################### BEGIN: " + value + " #####################################\n";
				string str2 = "//##################################### END: " + value + " #####################################\n";
				text = str + text + str2;
				includeLineCount += text.Split(new char[]
				{
					'\n'
				}).Length - 1;
				text = this.InjectIncludes(ref shaderResource, text, regex, includeDepth, ref includeLineCount);
				source = source.Replace(match.Value, text);
			}
			return source;
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x00184E44 File Offset: 0x00183044
		protected static string PatchCompileErrorMessage(string errorMessage, string sourceCode, int definesLineCount, int includeLineCount)
		{
			int injectedLinesCount = includeLineCount + definesLineCount;
			string text = "\\((?<line>\\d+)\\)";
			string text2 = "ERROR: (?<fileid>\\d+):(?<line>\\d+)";
			string[] sourceCodeLines = sourceCode.Split(new char[]
			{
				'\n'
			});
			string text3 = Regex.Replace(errorMessage, text, (Match match) => string.Format("({0}) [line patched as {1}, error in \"{2}\"]\n =>", match.Groups["line"].ToString(), int.Parse(match.Groups["line"].ToString()) - injectedLinesCount, sourceCodeLines[int.Parse(match.Groups["line"].ToString()) - 1].Replace('\r', ' ')));
			text3 = Regex.Replace(text3, text2, (Match match) => string.Format("ERROR: {0}: {1} [line patched as {2}, error in \"{3}\"]\n =>", new object[]
			{
				match.Groups["fileid"].ToString(),
				match.Groups["line"].ToString(),
				int.Parse(match.Groups["line"].ToString()) - injectedLinesCount,
				sourceCodeLines[int.Parse(match.Groups["line"].ToString()) - 1].Replace('\r', ' ')
			}));
			return string.Format("- Following GLSL error message was patched : {0} lines for #define and {1} lines for #include\n", definesLineCount, includeLineCount) + text3;
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x00184ED0 File Offset: 0x001830D0
		protected static uint CompileShader(GL shaderType, string source, int definesLineCount, int includeLineCount)
		{
			uint num = GPUProgram._gl.CreateShader(shaderType);
			int length = source.Length;
			GPUProgram._gl.ShaderSource(num, 1, ref source, ref length);
			GPUProgram._gl.CompileShader(num);
			int num2 = 0;
			GPUProgram._gl.GetShaderiv(num, GL.COMPILE_STATUS, out num2);
			bool flag = num2 == 0;
			if (flag)
			{
				int num3;
				GPUProgram._gl.GetShaderiv(num, GL.INFO_LOG_LENGTH, out num3);
				byte[] array = new byte[num3];
				GPUProgram._gl.GetShaderInfoLog(num, num3, out num3, array);
				string text = Encoding.UTF8.GetString(array);
				text = GPUProgram.PatchCompileErrorMessage(text, source, definesLineCount, includeLineCount);
				GPUProgram.Logger.Warn(text);
				GPUProgram.Logger.Warn("- {0} compilation failed, using fallback since shader hot reloading is on.", shaderType.ToString());
				GL gl = shaderType;
				GL gl2 = gl;
				if (gl2 != GL.FRAGMENT_SHADER)
				{
					if (gl2 != GL.VERTEX_SHADER)
					{
						if (gl2 != GL.GEOMETRY_SHADER)
						{
							throw new NotImplementedException();
						}
						num = GPUProgram._fallbackGeometryShader;
					}
					else
					{
						num = GPUProgram._fallbackVertexShader;
					}
				}
				else
				{
					num = GPUProgram._fallbackFragmentShader;
				}
				num = 0U;
			}
			return num;
		}

		// Token: 0x04003024 RID: 12324
		protected static readonly NumberFormatInfo DecimalPointFormatting = new NumberFormatInfo
		{
			NumberDecimalSeparator = ".",
			NumberGroupSeparator = ""
		};

		// Token: 0x04003025 RID: 12325
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003026 RID: 12326
		private static GPUProgram.ShaderCodeDumpPolicy _dumpPolicy;

		// Token: 0x04003027 RID: 12327
		private static string _shaderResourceAssemblyPath;

		// Token: 0x04003028 RID: 12328
		private static string _shaderResourcePath;

		// Token: 0x04003029 RID: 12329
		private static string _shaderCodeDumpPath;

		// Token: 0x0400302A RID: 12330
		private const int MaxIncludeDepth = 5;

		// Token: 0x0400302C RID: 12332
		protected readonly string _variationName;

		// Token: 0x0400302D RID: 12333
		protected GPUProgram.ShaderResource _vertexShaderResource;

		// Token: 0x0400302E RID: 12334
		protected GPUProgram.ShaderResource _geometryShaderResource;

		// Token: 0x0400302F RID: 12335
		protected GPUProgram.ShaderResource _fragmentShaderResource;

		// Token: 0x04003030 RID: 12336
		protected static GLFunctions _gl;

		// Token: 0x04003031 RID: 12337
		private static uint _fallbackVertexShader;

		// Token: 0x04003032 RID: 12338
		private static uint _fallbackGeometryShader;

		// Token: 0x04003033 RID: 12339
		private static uint _fallbackFragmentShader;

		// Token: 0x04003034 RID: 12340
		private static uint _fallbackProgram;

		// Token: 0x02000ED9 RID: 3801
		public enum ShaderCodeDumpPolicy
		{
			// Token: 0x040048C7 RID: 18631
			Never,
			// Token: 0x040048C8 RID: 18632
			OnError,
			// Token: 0x040048C9 RID: 18633
			Always
		}

		// Token: 0x02000EDA RID: 3802
		protected struct ShaderResource
		{
			// Token: 0x040048CA RID: 18634
			public string FileName;

			// Token: 0x040048CB RID: 18635
			public DateTime LastLoadTime;

			// Token: 0x040048CC RID: 18636
			public List<string> Includes;
		}

		// Token: 0x02000EDB RID: 3803
		protected struct AttribBindingInfo
		{
			// Token: 0x06006800 RID: 26624 RVA: 0x0021A022 File Offset: 0x00218222
			public AttribBindingInfo(uint index, string name)
			{
				this.Index = index;
				this.Name = name;
			}

			// Token: 0x040048CD RID: 18637
			public readonly uint Index;

			// Token: 0x040048CE RID: 18638
			public readonly string Name;
		}
	}
}
