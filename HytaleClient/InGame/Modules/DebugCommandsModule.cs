using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using HytaleClient.Common.Memory;
using HytaleClient.Core;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Map;
using HytaleClient.InGame.Commands;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Entities.Projectile;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008EF RID: 2287
	internal class DebugCommandsModule : Module
	{
		// Token: 0x060043A9 RID: 17321 RVA: 0x000D8874 File Offset: 0x000D6A74
		public DebugCommandsModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._gameInstance.RegisterCommand("help", new GameInstance.Command(this._gameInstance.HelpCommand));
			this._gameInstance.RegisterCommand("allblocks", new GameInstance.Command(this.AllBlocksCommand));
			this._gameInstance.RegisterCommand("camoffset", new GameInstance.Command(this.CamOffsetCommand));
			this._gameInstance.RegisterCommand("fog", new GameInstance.Command(this.FogCommand));
			this._gameInstance.RegisterCommand("emotion", new GameInstance.Command(this.EmotionCommand));
			this._gameInstance.RegisterCommand("playanimation", new GameInstance.Command(this.PlayCommand));
			this._gameInstance.RegisterCommand("lod", new GameInstance.Command(this.LODCommand));
			this._gameInstance.RegisterCommand("mapambientlight", new GameInstance.Command(this.MapAmbientLightCommand));
			this._gameInstance.RegisterCommand("oit", new GameInstance.Command(this.OITCommand));
			this._gameInstance.RegisterCommand("particle", new GameInstance.Command(this.ParticleCommand));
			this._gameInstance.RegisterCommand("trail", new GameInstance.Command(this.TrailCommand));
			this._gameInstance.RegisterCommand("perf", new GameInstance.Command(this.PerfCommand));
			this._gameInstance.RegisterCommand("posdump", new GameInstance.Command(this.PosDumpCommand));
			this._gameInstance.RegisterCommand("profiling", new GameInstance.Command(this.ProfilingCommand));
			this._gameInstance.RegisterCommand("foliagefading", new GameInstance.Command(this.FoliageFadingCommand));
			this._gameInstance.RegisterCommand("chunkdebug", new GameInstance.Command(this.ChunkDebugCommand));
			this._gameInstance.RegisterCommand("mem", new GameInstance.Command(this.MemoryUsageCommand));
			this._gameInstance.RegisterCommand("entitydebug", new GameInstance.Command(this.EntityDebugCommand));
			this._gameInstance.RegisterCommand("entityeffect", new GameInstance.Command(this.EntityEffectCommand));
			this._gameInstance.RegisterCommand("playernamedebug", new GameInstance.Command(this.PlayerNameDebugCommand));
			this._gameInstance.RegisterCommand("entityuidebug", new GameInstance.Command(this.EntityUIDebugCommand));
			this._gameInstance.RegisterCommand("chunkdump", new GameInstance.Command(this.ChunkDumpCommand));
			this._gameInstance.RegisterCommand("crash", new GameInstance.Command(this.CrashCommand));
			this._gameInstance.RegisterCommand("viewdistance", new GameInstance.Command(this.ViewDistanceCommand));
			this._gameInstance.RegisterCommand("chunkymin", new GameInstance.Command(this.ChunkYMinCommand));
			this._gameInstance.RegisterCommand("occlusion", new GameInstance.Command(this.Occlusion));
			this._gameInstance.RegisterCommand("animation", new GameInstance.Command(this.AnimationCommand));
			this._gameInstance.RegisterCommand("debugmap", new GameInstance.Command(this.DebugMap));
			this._gameInstance.RegisterCommand("debugpixel", new GameInstance.Command(this.DebugPixelSetup));
			this._gameInstance.RegisterCommand("light", new GameInstance.Command(this.LightSetup));
			this._gameInstance.RegisterCommand("lbuffercompression", new GameInstance.Command(this.LightBufferCompressionSetup));
			this._gameInstance.RegisterCommand("shadowmap", new GameInstance.Command(this.ShadowMappingSetup));
			this._gameInstance.RegisterCommand("volsunshaft", new GameInstance.Command(this.VolumetricSunshaftSetup));
			this._gameInstance.RegisterCommand("water", new GameInstance.Command(this.WaterTestSetup));
			this._gameInstance.RegisterCommand("renderscale", new GameInstance.Command(this.ResolutionScaleSetup));
			this._gameInstance.RegisterCommand("dof", new GameInstance.Command(this.DepthOfFieldSetup));
			this._gameInstance.RegisterCommand("ssao", new GameInstance.Command(this.SSAOSetup));
			this._gameInstance.RegisterCommand("bloom", new GameInstance.Command(this.BloomSetup));
			this._gameInstance.RegisterCommand("blur", new GameInstance.Command(this.BlurSetup));
			this._gameInstance.RegisterCommand("skyambient", new GameInstance.Command(this.SkyAmbientSetup));
			this._gameInstance.RegisterCommand("caustics", new GameInstance.Command(this.UnderwaterCausticsSetup));
			this._gameInstance.RegisterCommand("clouduvmotion", new GameInstance.Command(this.CloudsUVMotionSetup));
			this._gameInstance.RegisterCommand("cloudshadow", new GameInstance.Command(this.CloudsShadowsSetup));
			this._gameInstance.RegisterCommand("sky", new GameInstance.Command(this.Sky));
			this._gameInstance.RegisterCommand("forcefield", new GameInstance.Command(this.ForceFieldSetup));
			this._gameInstance.RegisterCommand("sharpen", new GameInstance.Command(this.SharpenSetup));
			this._gameInstance.RegisterCommand("fxaa", new GameInstance.Command(this.FXAASetup));
			this._gameInstance.RegisterCommand("taa", new GameInstance.Command(this.TAASetup));
			this._gameInstance.RegisterCommand("distortion", new GameInstance.Command(this.DistortionSetup));
			this._gameInstance.RegisterCommand("postfx", new GameInstance.Command(this.PostFXSetup));
			this._gameInstance.RegisterCommand("movsettings", new GameInstance.Command(this.UpdateMovementSettings));
			this._gameInstance.RegisterCommand("speedo", new GameInstance.Command(this.UpdateSpeedometer));
			this._gameInstance.RegisterCommand("dmgindicatorangle", new GameInstance.Command(this.IndicatorAngle));
			this._gameInstance.RegisterCommand("speed", new GameInstance.Command(this.SpeedCommand));
			this._gameInstance.RegisterCommand("parallel", new GameInstance.Command(this.ParallelSetup));
			this._gameInstance.RegisterCommand("test", new GameInstance.Command(this.TestSetup));
			this._gameInstance.RegisterCommand("graphics", new GameInstance.Command(this.GraphicsSetup));
			this._gameInstance.RegisterCommand("packetstats", new GameInstance.Command(this.PacketStats));
			this._gameInstance.RegisterCommand("heartbeatsettings", new GameInstance.Command(this.UpdateHeartbeatSettings));
			this._gameInstance.RegisterCommand("hitdetection", new GameInstance.Command(this.HitDetection));
			this._gameInstance.RegisterCommand("renderplayers", new GameInstance.Command(this.RenderPlayers));
			this._gameInstance.RegisterCommand("blockpreview", new GameInstance.Command(this.BlockPreview));
			this._gameInstance.RegisterCommand("buildertool", new GameInstance.Command(this.BuilderTool));
			this._gameInstance.RegisterCommand("forcetint", new GameInstance.Command(this.ForceTint));
			this._gameInstance.RegisterCommand("warn", delegate(string[] args)
			{
				this._gameInstance.App.DevTools.Warn("This is a warning");
			});
			this._gameInstance.RegisterCommand("debugmove", new GameInstance.Command(this.DebugMove));
			this._gameInstance.RegisterCommand("debugforce", delegate(string[] args)
			{
				ApplyForceInteraction.DebugDisplay = !ApplyForceInteraction.DebugDisplay;
				this._gameInstance.Chat.Log(string.Format("Display local forces: {0}", ApplyForceInteraction.DebugDisplay));
			});
			this._gameInstance.RegisterCommand("debugprojectile", delegate(string[] args)
			{
				PredictedProjectile.DebugPrediction = !PredictedProjectile.DebugPrediction;
				this._gameInstance.Chat.Log(string.Format("Debug Projectile: {0}", PredictedProjectile.DebugPrediction));
			});
			this._gameInstance.RegisterCommand("batcher", new GameInstance.Command(this.StressTestBatcher));
			this._gameInstance.RegisterCommand("wireframe", new GameInstance.Command(this.WireframeCommand));
			this._gameInstance.RegisterCommand("render", new GameInstance.Command(this.RenderSetup));
			this._gameInstance.RegisterCommand("logdisposablesummary", new GameInstance.Command(this.LogDisposableSummaryCommand));
			this._gameInstance.RegisterCommand("mem2", new GameInstance.Command(this.NativeMemoryUsageCommand));
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x000D90D4 File Offset: 0x000D72D4
		[Usage("animation", new string[]
		{
			"[on|off]",
			"gpu_send_0 |gpu_send_1|gpu_send_2",
			"[list|reset|id] [slot]"
		})]
		private void AnimationCommand(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0];
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "gpu_send_0"))
					{
						if (!(a == "gpu_send_1"))
						{
							if (!(a == "gpu_send_2"))
							{
								if (!(a == "list"))
								{
									string text2 = args[0].Trim();
									AnimationSlot animationSlot = 0;
									bool flag2 = text2 == "reset";
									if (flag2)
									{
										text2 = null;
									}
									bool flag3 = args.Length > 1;
									if (flag3)
									{
										animationSlot = (AnimationSlot)Enum.Parse(typeof(AnimationSlot), args[1].Trim(), true);
									}
									this._gameInstance.InjectPacket(new PlayAnimation
									{
										EntityId = this._gameInstance.LocalPlayerNetworkId,
										AnimationId = text2,
										Slot = animationSlot
									});
									this._gameInstance.Chat.Log((text2 == null) ? string.Format("Resetting animation on slot {0}", animationSlot) : string.Format("Playing Animation {0} on slot {1}", text2, animationSlot));
								}
								else
								{
									AnimationSlot slot = 0;
									bool flag4 = args.Length > 1;
									if (flag4)
									{
										slot = (AnimationSlot)Enum.Parse(typeof(AnimationSlot), args[1].Trim(), true);
									}
									List<string> animationList = this._gameInstance.LocalPlayer.GetAnimationList(slot);
									this._gameInstance.Chat.Log(slot.ToString() + " Animations: " + string.Join(", ", animationList.ToArray()));
								}
							}
							else
							{
								this._gameInstance.Engine.AnimationSystem.SetTransferMethod(AnimationSystem.TransferMethod.ParallelInterleaved);
							}
						}
						else
						{
							this._gameInstance.Engine.AnimationSystem.SetTransferMethod(AnimationSystem.TransferMethod.ParallelSeparate);
						}
					}
					else
					{
						this._gameInstance.Engine.AnimationSystem.SetTransferMethod(AnimationSystem.TransferMethod.Sequential);
					}
				}
				else
				{
					this._gameInstance.Engine.AnimationSystem.SetEnabled(false);
					this._gameInstance.Chat.Log("Animations disabled");
				}
			}
			else
			{
				this._gameInstance.Engine.AnimationSystem.SetEnabled(true);
				this._gameInstance.Chat.Log("Animations enabled");
			}
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x000D9350 File Offset: 0x000D7550
		[Usage("allblocks", new string[]
		{
			"[-c] [-r] [-w] [-s=f|b] [-o]"
		})]
		[Description("Displays all blocks currently loaded by the client.")]
		private void AllBlocksCommand(string[] args)
		{
			bool flag = Enumerable.Contains<string>(args, "-c");
			bool flag2 = Enumerable.Contains<string>(args, "-r");
			bool flag3 = Enumerable.Contains<string>(args, "-w");
			bool flag4 = Enumerable.Contains<string>(args, "-s");
			bool sortBlocksForward = Enumerable.Contains<string>(args, "-s=f");
			bool flag5 = Enumerable.Contains<string>(args, "-s=b");
			bool flag6 = Enumerable.Contains<string>(args, "-o");
			bool flag7 = sortBlocksForward || flag5;
			if (flag7)
			{
				flag4 = true;
			}
			Vector3 value = new Vector3(32f, 32f, 32f);
			int num = (int)this._gameInstance.LocalPlayer.Position.X;
			int num2 = (int)Math.Max(Math.Min(this._gameInstance.LocalPlayer.Position.Y - 5f, (float)(ChunkHelper.Height - 10)), 10f);
			int num3 = (int)this._gameInstance.LocalPlayer.Position.Z;
			MapModule mapModule = this._gameInstance.MapModule;
			int clientBlockIdFromName = mapModule.GetClientBlockIdFromName("Rock_Stone_Cobble");
			int num4 = Enumerable.Count<ClientBlockType>(mapModule.ClientBlockTypes, (ClientBlockType blockType) => blockType != null);
			int num5 = 0;
			bool flag8 = flag || flag2 || flag6;
			if (flag8)
			{
				for (int i = 0; i < mapModule.ClientBlockTypes.Length; i++)
				{
					ClientBlockType clientBlockType = mapModule.ClientBlockTypes[i];
					bool flag9 = clientBlockType == null || (flag3 && clientBlockType.FluidBlockId != 0 && clientBlockType.CollisionMaterial != 2) || (flag2 && (clientBlockType.RotationPitch != null || clientBlockType.RotationYaw != null)) || (flag && clientBlockType.FinalBlockyModel != null && clientBlockType.FinalBlockyModel.NodeCount == 1 && clientBlockType.FinalBlockyModel.AllNodes[0].Size == new Vector3(32f, 32f, 32f)) || (flag6 && clientBlockType.Name.Contains("|"));
					if (flag9)
					{
						num5++;
					}
				}
			}
			num4 -= num5;
			int num6 = (int)Math.Ceiling((double)num4 / 1024.0);
			int num7 = num4 % 32;
			int num8 = num4 / 32;
			int num9 = num6 * 32 + (num6 - 1) * 3 + 6;
			int num10 = 69;
			num -= num9 / 2;
			num3 -= num10 / 2;
			for (int j = 0; j < num9; j++)
			{
				for (int k = 0; k < num10; k++)
				{
					this._gameInstance.MapModule.SetClientBlock(num + j, num2 - 1, num3 + k, clientBlockIdFromName);
					bool flag10 = j == 0 || k == 0 || j == num9 - 1 || k == num10 - 1;
					if (flag10)
					{
						bool flag11 = (j == num9 - 1 && k == 0) || (j == 0 && k == 0) || (j == 0 && k == num10 - 1) || (j == num9 - 1 && k == num10 - 1) || k == 0 || k == num10 - 1 || j == 0 || j == num9 - 1;
						if (flag11)
						{
							this._gameInstance.MapModule.SetClientBlock(num + j, num2, num3 + k, clientBlockIdFromName);
						}
					}
				}
			}
			ClientBlockType[] array = mapModule.ClientBlockTypes;
			bool flag12 = flag4;
			if (flag12)
			{
				array = (ClientBlockType[])array.Clone();
				Array.Sort<ClientBlockType>(array, delegate(ClientBlockType a, ClientBlockType b)
				{
					string[] array3 = a.Name.Split(new char[]
					{
						'|'
					});
					string[] array4 = b.Name.Split(new char[]
					{
						'|'
					});
					string[] array5 = array3[0].Split(new char[]
					{
						'_'
					});
					string[] array6 = array4[0].Split(new char[]
					{
						'_'
					});
					bool sortBlocksForward = sortBlocksForward;
					if (sortBlocksForward)
					{
						for (int m = 0; m < Math.Min(array5.Length, array6.Length); m++)
						{
							int num12 = string.Compare(array5[m], array6[m], StringComparison.InvariantCulture);
							bool flag14 = num12 != 0;
							if (flag14)
							{
								return num12;
							}
						}
					}
					else
					{
						for (int n = 0; n < Math.Min(array5.Length, array6.Length); n++)
						{
							int num13 = string.Compare(array5[array5.Length - n - 1], array6[array6.Length - n - 1], StringComparison.InvariantCulture);
							bool flag15 = num13 != 0;
							if (flag15)
							{
								return num13;
							}
						}
					}
					bool flag16 = array5.Length != array6.Length;
					int result;
					if (flag16)
					{
						result = array5.Length.CompareTo(array6.Length);
					}
					else
					{
						bool flag17 = array3.Length != array4.Length;
						if (flag17)
						{
							result = array3.Length.CompareTo(array4.Length);
						}
						else
						{
							for (int num14 = 0; num14 < array3.Length; num14++)
							{
								int num15 = string.Compare(array3[num14], array4[num14], StringComparison.InvariantCulture);
								bool flag18 = num15 != 0;
								if (flag18)
								{
									return num15;
								}
							}
							result = 0;
						}
					}
					return result;
				});
			}
			int num11 = 0;
			foreach (ClientBlockType clientBlockType2 in array)
			{
				bool flag13 = clientBlockType2 == null || (flag3 && clientBlockType2.FluidBlockId != 0 && clientBlockType2.CollisionMaterial != 2) || (flag2 && (clientBlockType2.RotationPitch != null || clientBlockType2.RotationYaw != null)) || (flag && clientBlockType2.FinalBlockyModel != null && clientBlockType2.FinalBlockyModel.NodeCount == 1 && clientBlockType2.FinalBlockyModel.AllNodes[0].Size == value) || (flag6 && clientBlockType2.Name.Contains("|"));
				if (!flag13)
				{
					num11++;
					num6 = num11 / 1024;
					num7 = num11 % 32;
					num8 = num11 / 32;
					int j = num6 * 35 + num7 + 3;
					int k = num8 % 32 * 2 + 3;
					this._gameInstance.MapModule.SetClientBlock(num + j, num2, num3 + k, clientBlockType2.Id);
				}
			}
			this._gameInstance.Chat.Log(string.Format("{0} block types placed, skipped {1}", num4, num5));
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x000D9840 File Offset: 0x000D7A40
		[Usage("camoffset", new string[]
		{
			"[x] [y] [z]"
		})]
		[Description("Change the position offset of the camera.")]
		private void CamOffsetCommand(string[] args)
		{
			bool flag = args.Length != 3;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			float x;
			bool flag2 = !float.TryParse(args[0], out x);
			if (flag2)
			{
				throw new InvalidCommandUsage();
			}
			float y;
			bool flag3 = !float.TryParse(args[1], out y);
			if (flag3)
			{
				throw new InvalidCommandUsage();
			}
			float z;
			bool flag4 = !float.TryParse(args[2], out z);
			if (flag4)
			{
				throw new InvalidCommandUsage();
			}
			ThirdPersonCameraController thirdPersonCameraController = this._gameInstance.CameraModule.Controller as ThirdPersonCameraController;
			bool flag5 = thirdPersonCameraController != null;
			if (flag5)
			{
				thirdPersonCameraController.PositionOffset = new Vector3(x, y, z);
			}
			else
			{
				FreeRotateCameraController freeRotateCameraController = this._gameInstance.CameraModule.Controller as FreeRotateCameraController;
				bool flag6 = freeRotateCameraController != null;
				if (flag6)
				{
					freeRotateCameraController.PositionOffset = new Vector3(x, y, z);
				}
				else
				{
					this._gameInstance.Chat.Log("Can only be used in third-person and free rotate view!");
				}
			}
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x000D992C File Offset: 0x000D7B2C
		[Usage("emotion", new string[]
		{
			"[Angry|Astonished|Fear|Laugh|Sad|Smile]"
		})]
		private void EmotionCommand(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.LocalPlayer.SetEmotionAnimation(args[0]);
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x000D9964 File Offset: 0x000D7B64
		[Usage("playanimation", new string[]
		{
			"None|(<animationId> <systemId> <nodeId>)"
		})]
		private void PlayCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0];
			bool flag2 = text == "Stop";
			if (flag2)
			{
				this._gameInstance.LocalPlayer.RebuildRenderers(false);
			}
			else
			{
				string particleSystemId = (args.Length > 1) ? args[1] : null;
				string nodeName = (args.Length > 2) ? args[2] : null;
				this._gameInstance.LocalPlayer.SetDebugAnimation(text, particleSystemId, nodeName);
			}
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x000D99DC File Offset: 0x000D7BDC
		[Usage("lod", new string[]
		{
			"[on|off|anim_on|anim_off||logic_on|logic_off|distance [start][range]|rotdistance [distance]]"
		})]
		[Description("Control the Level Of Detail mechanisms.")]
		private void LODCommand(string[] args)
		{
			bool flag = args.Length > 3;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0];
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 1630810064U)
			{
				if (num <= 528873205U)
				{
					if (num != 224394567U)
					{
						if (num == 528873205U)
						{
							if (text2 == "rotdistance")
							{
								float distanceToCameraBeforeRotation;
								bool flag2 = float.TryParse(args[1], out distanceToCameraBeforeRotation);
								if (flag2)
								{
									this._gameInstance.EntityStoreModule.CurrentSetup.DistanceToCameraBeforeRotation = distanceToCameraBeforeRotation;
									goto IL_27B;
								}
								throw new InvalidCommandUsage();
							}
						}
					}
					else if (text2 == "logic_off")
					{
						this._gameInstance.Chat.Log("LOD disabled for logic");
						this._gameInstance.EntityStoreModule.CurrentSetup.LogicalLoDUpdate = false;
						goto IL_27B;
					}
				}
				else if (num != 783488098U)
				{
					if (num == 1630810064U)
					{
						if (text2 == "on")
						{
							this._gameInstance.SetUseLOD(true);
							goto IL_27B;
						}
					}
				}
				else if (text2 == "distance")
				{
					uint range = 0U;
					uint distance;
					bool flag3 = uint.TryParse(args[1], out distance);
					if (flag3)
					{
						bool flag4 = args.Length > 2;
						if (flag4)
						{
							uint.TryParse(args[2], out range);
						}
						this._gameInstance.SetLODDistance(distance, range);
						goto IL_27B;
					}
					throw new InvalidCommandUsage();
				}
			}
			else if (num <= 2594777347U)
			{
				if (num != 2051696968U)
				{
					if (num == 2594777347U)
					{
						if (text2 == "logic_on")
						{
							this._gameInstance.Chat.Log("LOD enabled for logic");
							this._gameInstance.EntityStoreModule.CurrentSetup.LogicalLoDUpdate = true;
							goto IL_27B;
						}
					}
				}
				else if (text2 == "anim_on")
				{
					this._gameInstance.UseAnimationLOD = true;
					goto IL_27B;
				}
			}
			else if (num != 2872740362U)
			{
				if (num == 3239060706U)
				{
					if (text2 == "anim_off")
					{
						this._gameInstance.UseAnimationLOD = false;
						goto IL_27B;
					}
				}
			}
			else if (text2 == "off")
			{
				this._gameInstance.SetUseLOD(false);
				goto IL_27B;
			}
			throw new InvalidCommandUsage();
			IL_27B:
			this._gameInstance.PrintLODState();
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x000D9C70 File Offset: 0x000D7E70
		[Usage("fog", new string[]
		{
			"[dynamic|static|off| mood_[on|off]| dither_[on|off]| smooth_[on|off]]\n falloff [4-10]\n density [0-1]\n speed_factor [0-2]\n variation_scale [0-1]"
		})]
		[Description("Change current fog settings.")]
		private void FogCommand(string[] args)
		{
			bool flag = args.Length < 1 || args.Length > 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num = 0;
			string text = args[num++].ToLower();
			string text2 = text;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num2 <= 1924728219U)
			{
				if (num2 <= 616129863U)
				{
					if (num2 <= 76334129U)
					{
						if (num2 != 13026411U)
						{
							if (num2 == 76334129U)
							{
								if (text2 == "dither_sky_on")
								{
									this._gameInstance.UseMoodFogDitheringOnSkyAndClouds(true);
									goto IL_5FD;
								}
							}
						}
						else if (text2 == "dither_off")
						{
							this._gameInstance.UseMoodFogDithering(false);
							goto IL_5FD;
						}
					}
					else if (num2 != 421447818U)
					{
						if (num2 != 553650000U)
						{
							if (num2 == 616129863U)
							{
								if (text2 == "dither_on")
								{
									this._gameInstance.UseMoodFogDithering(true);
									goto IL_5FD;
								}
							}
						}
						else if (text2 == "mood_sky_on")
						{
							this._gameInstance.UseMoodFogOnSky(true);
							goto IL_5FD;
						}
					}
					else if (text2 == "mood_sky_off")
					{
						this._gameInstance.UseMoodFogOnSky(false);
						goto IL_5FD;
					}
				}
				else if (num2 <= 1330812590U)
				{
					if (num2 != 1271365643U)
					{
						if (num2 == 1330812590U)
						{
							if (text2 == "dynamic")
							{
								this._gameInstance.WeatherModule.ActiveFogMode = WeatherModule.FogMode.Dynamic;
								this._gameInstance.UseFog(WeatherModule.FogMode.Dynamic);
								goto IL_5FD;
							}
						}
					}
					else if (text2 == "variation_scale")
					{
						float moodFogDensityVariationScale;
						bool flag2 = !float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out moodFogDensityVariationScale);
						if (flag2)
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SetMoodFogDensityVariationScale(moodFogDensityVariationScale);
						goto IL_5FD;
					}
				}
				else if (num2 != 1414443264U)
				{
					if (num2 != 1481254926U)
					{
						if (num2 == 1924728219U)
						{
							if (text2 == "density")
							{
								float num3;
								bool flag3 = !float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
								if (flag3)
								{
									throw new InvalidCommandUsage();
								}
								this._gameInstance.SetMoodFogDensityUnderwater((float)Math.Exp((double)num3) - 1f);
								goto IL_5FD;
							}
						}
					}
					else if (text2 == "mood_on")
					{
						this._gameInstance.UseMoodFog(true);
						goto IL_5FD;
					}
				}
				else if (text2 == "custom_off")
				{
					this._gameInstance.UseCustomMoodFog(false);
					goto IL_5FD;
				}
			}
			else if (num2 <= 2974131623U)
			{
				if (num2 <= 2394903789U)
				{
					if (num2 != 1957206576U)
					{
						if (num2 == 2394903789U)
						{
							if (text2 == "custom_density")
							{
								float moodFogCustomDensity;
								bool flag4 = !float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out moodFogCustomDensity);
								if (flag4)
								{
									throw new InvalidCommandUsage();
								}
								this._gameInstance.SetMoodFogCustomDensity(moodFogCustomDensity);
								goto IL_5FD;
							}
						}
					}
					else if (text2 == "mood_off")
					{
						this._gameInstance.UseMoodFog(false);
						goto IL_5FD;
					}
				}
				else if (num2 != 2424677997U)
				{
					if (num2 != 2872740362U)
					{
						if (num2 == 2974131623U)
						{
							if (text2 == "custom_falloff")
							{
								float moodFogHeightCustomHeightFalloff;
								bool flag5 = !float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out moodFogHeightCustomHeightFalloff);
								if (flag5)
								{
									throw new InvalidCommandUsage();
								}
								this._gameInstance.SetMoodFogHeightCustomHeightFalloff(moodFogHeightCustomHeightFalloff);
								goto IL_5FD;
							}
						}
					}
					else if (text2 == "off")
					{
						this._gameInstance.WeatherModule.ActiveFogMode = WeatherModule.FogMode.Off;
						this._gameInstance.UseFog(WeatherModule.FogMode.Off);
						goto IL_5FD;
					}
				}
				else if (text2 == "dither_sky_off")
				{
					this._gameInstance.UseMoodFogDitheringOnSkyAndClouds(false);
					goto IL_5FD;
				}
			}
			else if (num2 <= 3532702267U)
			{
				if (num2 != 3244567033U)
				{
					if (num2 != 3383934149U)
					{
						if (num2 == 3532702267U)
						{
							if (text2 == "static")
							{
								this._gameInstance.WeatherModule.ActiveFogMode = WeatherModule.FogMode.Static;
								this._gameInstance.UseFog(WeatherModule.FogMode.Static);
								goto IL_5FD;
							}
						}
					}
					else if (text2 == "falloff")
					{
						float num4;
						bool flag6 = !float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num4);
						if (flag6)
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SetMoodFogHeightFalloffUnderwater(num4 * 0.01f);
						goto IL_5FD;
					}
				}
				else if (text2 == "smooth_on")
				{
					this._gameInstance.UseMoodFogSmoothColor(true);
					goto IL_5FD;
				}
			}
			else if (num2 != 3601414142U)
			{
				if (num2 != 3729790686U)
				{
					if (num2 == 4290938581U)
					{
						if (text2 == "smooth_off")
						{
							this._gameInstance.UseMoodFogSmoothColor(false);
							goto IL_5FD;
						}
					}
				}
				else if (text2 == "speed_factor")
				{
					float moodFogSpeedFactor;
					bool flag7 = !float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out moodFogSpeedFactor);
					if (flag7)
					{
						throw new InvalidCommandUsage();
					}
					this._gameInstance.SetMoodFogSpeedFactor(moodFogSpeedFactor);
					goto IL_5FD;
				}
			}
			else if (text2 == "custom_on")
			{
				this._gameInstance.UseCustomMoodFog(true);
				goto IL_5FD;
			}
			throw new InvalidCommandUsage();
			IL_5FD:
			this._gameInstance.PrintFogState();
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x000DA288 File Offset: 0x000D8488
		[Usage("mapambientlight", new string[]
		{
			"[0-0.5;default=0.05]"
		})]
		[Description("Change the ambient light applied to the world.")]
		private void MapAmbientLightCommand(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			float num;
			bool flag2 = !float.TryParse(args[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
			if (flag2)
			{
				throw new InvalidCommandUsage();
			}
			bool flag3 = num < 0f || (double)num > 0.5;
			if (flag3)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.MapModule.SetAmbientLight(num);
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x000DA2FC File Offset: 0x000D84FC
		[Usage("entityeffect", new string[]
		{
			"add [name]",
			"remove [name]",
			"clear",
			"debug_display_on",
			"debug_display_off"
		})]
		[Description("Manipulate entity effects by adding, removing or clearing them.")]
		public void EntityEffectCommand(string[] args)
		{
			int num = 0;
			string text = args[num++].ToLower();
			string a = text;
			if (!(a == "add"))
			{
				if (!(a == "remove"))
				{
					if (!(a == "clear"))
					{
						if (!(a == "debug_display_on"))
						{
							if (!(a == "debug_display_off"))
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.EntityStoreModule.CurrentSetup.DisplayDebugCommandsOnEntityEffect = false;
						}
						else
						{
							this._gameInstance.EntityStoreModule.CurrentSetup.DisplayDebugCommandsOnEntityEffect = true;
						}
					}
					else
					{
						this._gameInstance.LocalPlayer.ClearEffects();
					}
				}
				else
				{
					string text2 = args[num++];
					int networkEffectIndex;
					bool flag = !this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue(text2, out networkEffectIndex);
					if (flag)
					{
						this._gameInstance.Chat.Error("Entity Effect not found for ID: " + text2);
					}
					else
					{
						bool flag2 = this._gameInstance.LocalPlayer.RemoveEffect(networkEffectIndex);
						bool flag3 = flag2;
						if (flag3)
						{
							this._gameInstance.Chat.Log("Entity effect (id: " + text2 + ") removed!");
						}
						else
						{
							this._gameInstance.Chat.Error("Entity has no effect: " + text2);
						}
					}
				}
			}
			else
			{
				string text3 = args[num++];
				int num2;
				bool flag4 = !this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue(text3, out num2);
				if (flag4)
				{
					this._gameInstance.App.DevTools.Error("Could not find entity effect: " + text3);
				}
				else
				{
					Entity localPlayer = this._gameInstance.LocalPlayer;
					int networkEffectIndex2 = num2;
					bool? infinite = new bool?(true);
					localPlayer.AddEffect(networkEffectIndex2, null, infinite, null);
					this._gameInstance.Chat.Log("Entity effect (id: " + text3 + ") added!");
				}
			}
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x000DA510 File Offset: 0x000D8710
		[Usage("oit", new string[]
		{
			"off|wboit|wboit_e|poit|moit [1|2|4|8]",
			"lowres",
			"lowres_fixup_on|lowres_fixup_off"
		})]
		public void OITCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num = 0;
			string text = args[num++].ToLower();
			string text2 = text;
			string text3 = text2;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text3);
			if (num2 <= 2130973526U)
			{
				if (num2 <= 1036576988U)
				{
					if (num2 != 501384829U)
					{
						if (num2 == 1036576988U)
						{
							if (text3 == "wboit")
							{
								this._gameInstance.SetupOIT(OrderIndependentTransparency.Method.WBOIT);
								return;
							}
						}
					}
					else if (text3 == "chunks_on")
					{
						this._gameInstance.SetUseChunksOIT(true);
						return;
					}
				}
				else if (num2 != 1089096149U)
				{
					if (num2 != 2084307668U)
					{
						if (num2 == 2130973526U)
						{
							if (text3 == "lowres_fixup_off")
							{
								this._gameInstance.UseOITEdgeFixup(false, false);
								return;
							}
						}
					}
					else if (text3 == "wboit_e")
					{
						this._gameInstance.SetupOIT(OrderIndependentTransparency.Method.WBOITExt);
						return;
					}
				}
				else if (text3 == "lowres")
				{
					this._gameInstance.ChangeOITResolution();
					return;
				}
			}
			else if (num2 <= 2379114669U)
			{
				if (num2 != 2332197084U)
				{
					if (num2 == 2379114669U)
					{
						if (text3 == "poit")
						{
							this._gameInstance.SetupOIT(OrderIndependentTransparency.Method.POIT);
							return;
						}
					}
				}
				else if (text3 == "lowres_fixup_on")
				{
					this._gameInstance.UseOITEdgeFixup(true, true);
					return;
				}
			}
			else if (num2 != 2872740362U)
			{
				if (num2 != 4135930169U)
				{
					if (num2 == 4152964320U)
					{
						if (text3 == "moit")
						{
							this._gameInstance.SetupOIT(OrderIndependentTransparency.Method.MOIT);
							uint prepassInvScale = 1U;
							bool flag2 = args.Length == 2 && uint.TryParse(args[1], out prepassInvScale);
							if (flag2)
							{
								bool flag3 = this._gameInstance.SetupOITPrepassScale(prepassInvScale);
								bool flag4 = !flag3;
								if (flag4)
								{
									this._gameInstance.Chat.Error(".oit " + args[0] + " - invalid parameter. \n Only valid params are 1,2,4,8.");
								}
							}
							return;
						}
					}
				}
				else if (text3 == "chunks_off")
				{
					this._gameInstance.SetUseChunksOIT(false);
					return;
				}
			}
			else if (text3 == "off")
			{
				this._gameInstance.SetupOIT(OrderIndependentTransparency.Method.None);
				return;
			}
			throw new InvalidCommandUsage();
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x000DA7E0 File Offset: 0x000D89E0
		[Usage("particle", new string[]
		{
			"spawn [name] [quantity]",
			"remove [id]",
			"clear",
			"max [value]",
			"frustum_culling|distance_culling",
			"debug_overdraw|debug_quad|debug_bv|ztest"
		})]
		[Description("Experiment with the particle system.")]
		public void ParticleCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num = 0;
			string text = args[num++].ToLower();
			string text2 = text;
			string text3 = text2;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text3);
			if (num2 <= 1887753101U)
			{
				if (num2 <= 1005943109U)
				{
					if (num2 <= 514234044U)
					{
						if (num2 != 426628205U)
						{
							if (num2 != 514234044U)
							{
								goto IL_7D4;
							}
							if (!(text3 == "settings_stats"))
							{
								goto IL_7D4;
							}
							int num3;
							int num4;
							int num5;
							int num6;
							int num7;
							this._gameInstance.ParticleSystemStoreModule.GetSettingsStats(out num3, out num4, out num5, out num6, out num7);
							this._gameInstance.Chat.Log(string.Format("Particles settings stats:\n- particle system settings {0}\n- particle settings {1}\n- keyframe array {2}\n- keyframe array max{3}\n- keyframe {4}", new object[]
							{
								num3,
								num4,
								num5,
								num6,
								num7
							}));
							return;
						}
						else
						{
							if (!(text3 == "debug_overdraw"))
							{
								goto IL_7D4;
							}
							this._gameInstance.ToggleDebugParticleOverdraw();
							return;
						}
					}
					else if (num2 != 599565182U)
					{
						if (num2 != 975326616U)
						{
							if (num2 != 1005943109U)
							{
								goto IL_7D4;
							}
							if (!(text3 == "system_list"))
							{
								goto IL_7D4;
							}
							SDL.SDL_SetClipboardText(this._gameInstance.ParticleSystemStoreModule.GetSystemsList());
							this._gameInstance.Chat.Log("Placed ParticleSystem list in clipboard, ready to be pasted.");
							return;
						}
						else if (!(text3 == "spawn"))
						{
							goto IL_7D4;
						}
					}
					else
					{
						if (!(text3 == "debug_tex"))
						{
							goto IL_7D4;
						}
						goto IL_47B;
					}
				}
				else if (num2 <= 1334086842U)
				{
					if (num2 != 1213549819U)
					{
						if (num2 != 1334086842U)
						{
							goto IL_7D4;
						}
						if (!(text3 == "frustum_culling"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ParticleSystemStoreModule.FrustumCheck = !this._gameInstance.ParticleSystemStoreModule.FrustumCheck;
						string str = this._gameInstance.ParticleSystemStoreModule.FrustumCheck ? "on" : "off";
						this._gameInstance.Chat.Log("Particle frustum Culling is " + str + "!");
						return;
					}
					else
					{
						if (!(text3 == "culling"))
						{
							goto IL_7D4;
						}
						bool flag2 = !this._gameInstance.ParticleSystemStoreModule.FrustumCheck;
						this._gameInstance.ParticleSystemStoreModule.FrustumCheck = flag2;
						this._gameInstance.ParticleSystemStoreModule.DistanceCheck = flag2;
						string str = flag2 ? "on" : "off";
						this._gameInstance.Chat.Log("Particle frustum & distance Culling is " + str + "!");
						return;
					}
				}
				else if (num2 != 1483009432U)
				{
					if (num2 != 1550717474U)
					{
						if (num2 != 1887753101U)
						{
							goto IL_7D4;
						}
						if (!(text3 == "pause"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ToggleParticleSimulationPaused();
						return;
					}
					else
					{
						if (!(text3 == "clear"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ParticleSystemStoreModule.Clear();
						this._gameInstance.Chat.Log("All Particle Spawners removed!");
						return;
					}
				}
				else if (!(text3 == "debug"))
				{
					goto IL_7D4;
				}
				string systemId = args[num++];
				bool flag3 = !this._gameInstance.ParticleSystemStoreModule.CheckSettingsExist(systemId);
				if (flag3)
				{
					return;
				}
				float num8 = (args.Length > num) ? float.Parse(args[num++], CultureInfo.InvariantCulture) : 1f;
				this._gameInstance.ParticleSystemStoreModule.TrySpawnDebugSystem(systemId, this._gameInstance.LocalPlayer.Position, text == "debug", (int)num8);
				return;
			}
			else if (num2 <= 2967995286U)
			{
				if (num2 <= 2220029161U)
				{
					if (num2 != 2107770459U)
					{
						if (num2 != 2220029161U)
						{
							goto IL_7D4;
						}
						if (!(text3 == "debug_bv"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ToggleDebugParticleBoundingVolume();
						this._gameInstance.Chat.Log("BV color meaning:\n- grey: particle system not created\n- cyan: particle system created, not active\n- green: particle system created, and active");
						return;
					}
					else
					{
						if (!(text3 == "proxy"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ParticleSystemStoreModule.ProxyCheck = !this._gameInstance.ParticleSystemStoreModule.ProxyCheck;
						string str = this._gameInstance.ParticleSystemStoreModule.ProxyCheck ? "on" : "off";
						this._gameInstance.Chat.Log("Particle proxies are " + str + "!");
						return;
					}
				}
				else if (num2 != 2373018203U)
				{
					if (num2 != 2419770903U)
					{
						if (num2 != 2967995286U)
						{
							goto IL_7D4;
						}
						if (!(text3 == "clear_debug"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ParticleSystemStoreModule.ClearDebug();
						this._gameInstance.Chat.Log("All Particle Debugs removed!");
						return;
					}
					else
					{
						if (!(text3 == "lowres_on"))
						{
							goto IL_7D4;
						}
						this._gameInstance.SetParticleLowResRenderingEnabled(true);
						return;
					}
				}
				else
				{
					if (!(text3 == "lowres_off"))
					{
						goto IL_7D4;
					}
					this._gameInstance.SetParticleLowResRenderingEnabled(false);
					return;
				}
			}
			else if (num2 <= 3367077597U)
			{
				if (num2 != 2994640621U)
				{
					if (num2 != 3366741336U)
					{
						if (num2 != 3367077597U)
						{
							goto IL_7D4;
						}
						if (!(text3 == "distance_culling"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ParticleSystemStoreModule.DistanceCheck = !this._gameInstance.ParticleSystemStoreModule.DistanceCheck;
						string str = this._gameInstance.ParticleSystemStoreModule.DistanceCheck ? "on" : "off";
						this._gameInstance.Chat.Log("Particle distance Culling is " + str + "!");
						return;
					}
					else
					{
						if (!(text3 == "debug_uvmotion"))
						{
							goto IL_7D4;
						}
						this._gameInstance.ToggleDebugParticleUVMotion();
						return;
					}
				}
				else
				{
					if (!(text3 == "ztest"))
					{
						goto IL_7D4;
					}
					this._gameInstance.ToggleDebugParticleZTest();
					return;
				}
			}
			else if (num2 != 3617776409U)
			{
				if (num2 != 3683784189U)
				{
					if (num2 != 4121533390U)
					{
						goto IL_7D4;
					}
					if (!(text3 == "debug_quad"))
					{
						goto IL_7D4;
					}
				}
				else
				{
					if (!(text3 == "remove"))
					{
						goto IL_7D4;
					}
					int num9 = int.Parse(args[num++], CultureInfo.InvariantCulture);
					this._gameInstance.ParticleSystemStoreModule.DespawnDebugSystem(num9);
					this._gameInstance.Chat.Log(string.Format("Particle Spawner (id:{0}) removed!", num9));
					return;
				}
			}
			else
			{
				if (!(text3 == "max"))
				{
					goto IL_7D4;
				}
				int num10 = int.Parse(args[num++], CultureInfo.InvariantCulture);
				this._gameInstance.ParticleSystemStoreModule.SetMaxSpawnedSystems(num10);
				this._gameInstance.Chat.Log(string.Format("Set MaxSpawnedSystems to {0}", num10));
				return;
			}
			IL_47B:
			this._gameInstance.ToggleDebugParticleTexture();
			return;
			IL_7D4:
			throw new InvalidCommandUsage();
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x000DAFC8 File Offset: 0x000D91C8
		[Usage("trail", new string[]
		{
			"proxy"
		})]
		[Description("Experiment with the trail system.")]
		public void TrailCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num = 0;
			string text = args[num++].ToLower();
			string text2 = text;
			string a = text2;
			if (!(a == "proxy"))
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.TrailStoreModule.ProxyCheck = !this._gameInstance.TrailStoreModule.ProxyCheck;
			string str = this._gameInstance.TrailStoreModule.ProxyCheck ? "on" : "off";
			this._gameInstance.Chat.Log("Trail proxies are " + str + "!");
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x000DB078 File Offset: 0x000D9278
		[Usage("perf", new string[]
		{

		})]
		[Description("Generate a performance report to your clipboard")]
		private void PerfCommand(string[] args)
		{
			bool flag = args.Length != 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string branchName = BuildInfo.BranchName;
			string arg = Marshal.PtrToStringAnsi(this._gameInstance.Engine.Graphics.GL.GetString(GL.VENDOR));
			string arg2 = Marshal.PtrToStringAnsi(this._gameInstance.Engine.Graphics.GL.GetString(GL.RENDERER));
			float num = (float)Math.Round((double)this._gameInstance.TimeModule.GameDayProgressInHours, 2);
			int num2;
			int num3;
			SDL.SDL_GetWindowSize(this._gameInstance.Engine.Window.Handle, out num2, out num3);
			SDL.SDL_SetClipboardText(string.Concat(new string[]
			{
				string.Format("{0}\t{1}\t{2}\t", this._gameInstance.App.AuthManager.GetPlayerUuid(), this._gameInstance.App.Username, branchName),
				string.Format("{0} {1}\t{2}\t", arg, arg2, this._gameInstance.ProfilingModule.MeanFrameDuration),
				string.Format("{0}\t{1}\t", this._gameInstance.ProfilingModule.DrawnTriangles, this._gameInstance.ProfilingModule.DrawCallsCount),
				string.Format("{0} * {1}\t", num2, num3),
				string.Format("{0}\t{1}\t", this._gameInstance.LocalPlayer.Position, this._gameInstance.LocalPlayer.LookOrientation),
				string.Format("{0}\t{1}", num, this._gameInstance.WeatherModule.CurrentWeather.Id)
			}));
			this._gameInstance.Chat.Log("Placed performance data in clipboard, ready to be pasted in a spreadsheet.");
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x000DB260 File Offset: 0x000D9460
		[Description("Dumps position information to the chat.")]
		private void PosDumpCommand(string[] args)
		{
			ICameraController controller = this._gameInstance.CameraModule.Controller;
			this._gameInstance.Chat.Log("Camera Position: " + controller.Position.ToString());
			this._gameInstance.Chat.Log("Camera Rotation: " + controller.Rotation.ToString());
			this._gameInstance.Chat.Log("Camera Position Offset: " + controller.PositionOffset.ToString());
			this._gameInstance.Chat.Log("Camera Rotation Offset: " + controller.RotationOffset.ToString());
			this._gameInstance.Chat.Log("Camera Rotation MovementForceRotation: " + controller.MovementForceRotation.ToString());
			this._gameInstance.Chat.Log("Camera Rotation IsFirstPerson: " + controller.IsFirstPerson.ToString());
			Entity attachedTo = controller.AttachedTo;
			this._gameInstance.Chat.Log("Camera AttachedTo Position: " + attachedTo.Position.ToString());
			Chat chat = this._gameInstance.Chat;
			string str = "Camera AttachedTo Rotation: ";
			Vector3 lookOrientation = attachedTo.LookOrientation;
			chat.Log(str + lookOrientation.ToString());
			this._gameInstance.Chat.Log("Camera AttachedTo NetworkId: " + attachedTo.NetworkId.ToString());
			PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
			this._gameInstance.Chat.Log("Local Player Position: " + localPlayer.Position.ToString());
			Chat chat2 = this._gameInstance.Chat;
			string str2 = "Local Player Rotation: ";
			lookOrientation = localPlayer.LookOrientation;
			chat2.Log(str2 + lookOrientation.ToString());
			this._gameInstance.Chat.Log("Local Player NetworkId: " + localPlayer.NetworkId.ToString());
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x000DB4B0 File Offset: 0x000D96B0
		[Usage("profiling", new string[]
		{
			"on|off [all|id_0 id_1 ... id_n]",
			"clear [all|id_0 id_1 ... id_n]"
		})]
		[Description("Change profiling modes and filter stages.")]
		private void ProfilingCommand(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			bool flag2 = this._gameInstance.ProfilingModule.IsDetailedProfilingEnabled;
			bool flag3 = false;
			int i = 0;
			string text = args[i++].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "gpu"))
					{
						if (!(a == "cpu"))
						{
							if (!(a == "default"))
							{
								if (!(a == "clear"))
								{
									throw new InvalidCommandUsage();
								}
								flag3 = true;
							}
							else
							{
								flag2 = true;
								this._gameInstance.ProfilingModule.IsPartialRenderingProfilesEnabled = false;
							}
						}
						else
						{
							flag2 = true;
							this._gameInstance.ProfilingModule.IsCPUOnlyRenderingProfilesEnabled = true;
							this._gameInstance.ProfilingModule.IsPartialRenderingProfilesEnabled = true;
						}
					}
					else
					{
						flag2 = true;
						this._gameInstance.ProfilingModule.IsCPUOnlyRenderingProfilesEnabled = false;
						this._gameInstance.ProfilingModule.IsPartialRenderingProfilesEnabled = true;
					}
				}
				else
				{
					flag2 = false;
				}
			}
			else
			{
				flag2 = true;
			}
			Profiling profiling = this._gameInstance.Engine.Profiling;
			bool flag4 = false;
			while (i < args.Length)
			{
				string text2 = args[i].ToLower();
				string a2 = text2;
				if (!(a2 == "all"))
				{
					bool flag5 = args[i].Contains("-");
					if (flag5)
					{
						string[] array = args[i].Split(new char[]
						{
							'-'
						});
						int num;
						bool flag6 = int.TryParse(array[0], out num);
						int num2;
						bool flag7 = int.TryParse(array[1], out num2);
						flag4 = (flag4 || (flag6 && flag7));
						bool flag8 = flag6 && flag7;
						if (flag8)
						{
							for (int j = num; j <= num2; j++)
							{
								bool flag9 = flag3;
								if (flag9)
								{
									profiling.ClearMeasure(j);
								}
								else
								{
									profiling.SetMeasureEnabled(j, flag2);
								}
							}
						}
					}
					else
					{
						int j;
						bool flag10 = int.TryParse(args[i], out j);
						flag4 = (flag4 || flag10);
						bool flag11 = flag10;
						if (flag11)
						{
							bool flag12 = flag3;
							if (flag12)
							{
								profiling.ClearMeasure(j);
							}
							else
							{
								profiling.SetMeasureEnabled(j, flag2);
							}
						}
					}
				}
				else
				{
					flag4 = true;
					bool isPartialRenderingProfilesEnabled = this._gameInstance.ProfilingModule.IsPartialRenderingProfilesEnabled;
					if (isPartialRenderingProfilesEnabled)
					{
						for (int j = 0; j < profiling.MeasureCount; j++)
						{
							bool flag13 = profiling.GetMeasureInfo(j).HasGpuStats == !this._gameInstance.ProfilingModule.IsCPUOnlyRenderingProfilesEnabled;
							if (flag13)
							{
								bool flag14 = flag3;
								if (flag14)
								{
									profiling.ClearMeasure(j);
								}
								else
								{
									profiling.SetMeasureEnabled(j, flag2);
								}
							}
						}
					}
					else
					{
						for (int j = 0; j < profiling.MeasureCount; j++)
						{
							bool flag15 = flag3;
							if (flag15)
							{
								profiling.ClearMeasure(j);
							}
							else
							{
								profiling.SetMeasureEnabled(j, flag2);
							}
						}
					}
				}
				i++;
			}
			bool flag16 = !flag4 || flag2;
			if (flag16)
			{
				this._gameInstance.ProfilingModule.IsDetailedProfilingEnabled = flag2;
				bool flag17 = !flag2;
				if (flag17)
				{
					this._gameInstance.ProfilingModule.IsPartialRenderingProfilesEnabled = flag2;
				}
			}
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x000DB7E4 File Offset: 0x000D99E4
		[Usage("wireframe", new string[]
		{
			"off|on|entities|map_anim|map_opaque|map_alphatested|map_alphablended|sky"
		})]
		private void WireframeCommand(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 3014056611U)
			{
				if (num <= 1630810064U)
				{
					if (num != 1247493408U)
					{
						if (num == 1630810064U)
						{
							if (text2 == "on")
							{
								this._gameInstance.Wireframe = GameInstance.WireframePass.OnAll;
								return;
							}
						}
					}
					else if (text2 == "map_alphablended")
					{
						this._gameInstance.Wireframe = GameInstance.WireframePass.OnMapAlphaBlend;
						return;
					}
				}
				else if (num != 2872740362U)
				{
					if (num == 3014056611U)
					{
						if (text2 == "map_alphatested")
						{
							this._gameInstance.Wireframe = GameInstance.WireframePass.OnMapAlphaTested;
							return;
						}
					}
				}
				else if (text2 == "off")
				{
					this._gameInstance.Wireframe = GameInstance.WireframePass.Off;
					return;
				}
			}
			else if (num <= 3372870890U)
			{
				if (num != 3166336755U)
				{
					if (num == 3372870890U)
					{
						if (text2 == "entities")
						{
							this._gameInstance.Wireframe = GameInstance.WireframePass.OnEntities;
							return;
						}
					}
				}
				else if (text2 == "map_anim")
				{
					this._gameInstance.Wireframe = GameInstance.WireframePass.OnMapAnim;
					return;
				}
			}
			else if (num != 3442183990U)
			{
				if (num == 4000561537U)
				{
					if (text2 == "map_opaque")
					{
						this._gameInstance.Wireframe = GameInstance.WireframePass.OnMapOpaque;
						return;
					}
				}
			}
			else if (text2 == "sky")
			{
				this._gameInstance.Wireframe = GameInstance.WireframePass.OnSky;
				return;
			}
			throw new InvalidCommandUsage();
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x000DB994 File Offset: 0x000D9B94
		[Usage("foliagefading", new string[]
		{
			"on|off"
		})]
		[Description("Toggle foliage fading.")]
		private void FoliageFadingCommand(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			bool chunkUseFoliageFading;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				chunkUseFoliageFading = false;
			}
			else
			{
				chunkUseFoliageFading = true;
			}
			this._gameInstance.SetChunkUseFoliageFading(chunkUseFoliageFading);
			this._gameInstance.Chat.Log("Foliage fading: " + args[0]);
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x000DBA14 File Offset: 0x000D9C14
		[Usage("chunkdebug", new string[]
		{
			"on|off"
		})]
		[Description("Generates a red lattice over chunk borders.")]
		private void ChunkDebugCommand(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			bool debugChunkBoundaries;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				debugChunkBoundaries = false;
			}
			else
			{
				debugChunkBoundaries = true;
			}
			this._gameInstance.SetDebugChunkBoundaries(debugChunkBoundaries);
			this._gameInstance.Chat.Log("Chunk debug view: " + args[0]);
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x000DBA94 File Offset: 0x000D9C94
		[Description("Displays process memory.")]
		private void MemoryUsageCommand(string[] args)
		{
			Process currentProcess = Process.GetCurrentProcess();
			float num = (float)GC.GetTotalMemory(false) / 1024f / 1024f;
			float num2 = (float)currentProcess.WorkingSet64 / 1024f / 1024f;
			float num3 = (float)currentProcess.PrivateMemorySize64 / 1024f / 1024f;
			this._gameInstance.Chat.Log(string.Format("Managed: {0:f2} MB", num) + string.Format("\nPhysical: {0:f2} MB", num2) + string.Format("\nTotal committed: {0:f2} MB", num3));
			currentProcess.Dispose();
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x000DBB34 File Offset: 0x000D9D34
		[Description("Displays native allocated memory.")]
		private void NativeMemoryUsageCommand(string[] args)
		{
			NativeMemory.BumpDebugData bumpDebugData = default(NativeMemory.BumpDebugData);
			foreach (NativeMemory.BumpDebugData bumpDebugData2 in NativeMemory.BumpDebug.Values)
			{
				bumpDebugData.AllocatedBytes += bumpDebugData2.AllocatedBytes;
				bumpDebugData.UsedBytes += bumpDebugData2.UsedBytes;
			}
			float num = (float)NativeMemory.NewAllocatorBytes / 1024f;
			float num2 = (float)bumpDebugData.AllocatedBytes / 1024f;
			float num3 = (float)bumpDebugData.UsedBytes / 1024f;
			float num4 = (float)(NativeMemory.NewAllocatorBytes + bumpDebugData.AllocatedBytes) / 1024f;
			this._gameInstance.Chat.Log(string.Format("New: {0:f2} KB", num) + string.Format("\nBump used/total: {0:f2}/{1:f2} KB", num3, num2) + string.Format("\nTotal Native: {0:f2} KB", num4));
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x000DBC40 File Offset: 0x000D9E40
		private void EntityDebugCommand(string[] args)
		{
			bool flag = args.Length >= 1;
			if (flag)
			{
				string text = args[0].ToLower();
				string a = text;
				if (!(a == "ztest"))
				{
					if (!(a == "collided"))
					{
						if (!(a == "bounds"))
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.EntityStoreModule.DebugInfoBounds = !this._gameInstance.EntityStoreModule.DebugInfoBounds;
					}
					else
					{
						this._gameInstance.DebugCollisionOnlyCollided = !this._gameInstance.DebugCollisionOnlyCollided;
					}
				}
				else
				{
					this._gameInstance.DebugEntitiesZTest = !this._gameInstance.DebugEntitiesZTest;
				}
			}
			else
			{
				this._gameInstance.EntityStoreModule.DebugInfoNeedsDrawing = !this._gameInstance.EntityStoreModule.DebugInfoNeedsDrawing;
			}
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x000DBD1A File Offset: 0x000D9F1A
		private void PlayerNameDebugCommand(string[] args)
		{
			this._gameInstance.EntityStoreModule.CurrentSetup.DrawLocalPlayerName = !this._gameInstance.EntityStoreModule.CurrentSetup.DrawLocalPlayerName;
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x000DBD4A File Offset: 0x000D9F4A
		[Description("Force drawing entity UI attachments.")]
		private void EntityUIDebugCommand(string[] args)
		{
			this._gameInstance.EntityStoreModule.CurrentSetup.DebugUI = !this._gameInstance.EntityStoreModule.CurrentSetup.DebugUI;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x000DBD7C File Offset: 0x000D9F7C
		[Description("Dump chunk debug data to the log.")]
		private void ChunkDumpCommand(string[] args)
		{
			double num = Math.Round((double)this._gameInstance.LocalPlayer.Position.X, 3);
			double num2 = Math.Round((double)this._gameInstance.LocalPlayer.Position.Y, 3);
			double num3 = Math.Round((double)this._gameInstance.LocalPlayer.Position.Z, 3);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format("-- Start of chunk dump. Player feet at: ({0:##.000}, {1:##.000}, {2:##.000}), in column({3}, {4}). View distance: {5}.", new object[]
			{
				num,
				num2,
				num3,
				this._gameInstance.MapModule.StartChunkX,
				this._gameInstance.MapModule.StartChunkZ,
				this._gameInstance.App.Settings.ViewDistance
			}));
			this._gameInstance.MapModule.DoWithMapGeometryBuilderPaused(false, delegate
			{
				SpiralIterator spiralIterator = new SpiralIterator();
				spiralIterator.Initialize(this._gameInstance.MapModule.StartChunkX, this._gameInstance.MapModule.StartChunkZ, this._gameInstance.MapModule.ViewRadius);
				HashSet<long> hashSet = new HashSet<long>();
				foreach (long num4 in spiralIterator)
				{
					bool flag = !hashSet.Add(num4);
					if (flag)
					{
						throw new Exception("Spiral gave the same chunk column index twice.");
					}
				}
				List<long> allChunkColumnKeys = this._gameInstance.MapModule.GetAllChunkColumnKeys();
				sb.AppendLine(string.Format("Chunk columns: {0}", allChunkColumnKeys.Count));
				sb.AppendLine(string.Format("Chunk update tasks in queue: {0}", this._gameInstance.MapModule.GetChunkUpdateTaskQueueCount()));
				foreach (long num5 in allChunkColumnKeys)
				{
					ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(num5);
					sb.AppendLine(string.Format("  Chunk Column ({0}, {1}) InSpiral: {2}", ChunkHelper.XOfChunkColumnIndex(num5), ChunkHelper.ZOfChunkColumnIndex(num5), hashSet.Contains(num5)));
					for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
					{
						Chunk chunk = chunkColumn.GetChunk(i);
						bool flag2 = chunk == null;
						if (flag2)
						{
							sb.AppendLine(string.Format("    Chunk {0} — Not loaded", i));
						}
						else
						{
							bool flag3 = chunk.Rendered != null;
							if (flag3)
							{
								bool flag4 = this._gameInstance.MapModule.IsChunkReadyForDraw(chunk.X, chunk.Y, chunk.Z);
								string arg = string.Format("{0}{1}", chunk.Rendered.RebuildState, (chunk.Rendered.UpdateTask != null) ? " HasUpdateTask" : "");
								string arg2 = (flag4 ? string.Format(" isReadyForDraw(BufferUpdateCount: {0})", chunk.Rendered.BufferUpdateCount) : "") ?? "";
								sb.AppendLine(string.Format("    Chunk {0} — Rendered {1}{2}", i, arg, arg2));
							}
							else
							{
								sb.AppendLine(string.Format("    Chunk {0} — Not rendered", i));
							}
						}
					}
				}
			});
			sb.AppendLine("-- End of chunk dump.");
			DebugCommandsModule.Logger.Info<StringBuilder>(sb);
			this._gameInstance.Chat.Log("Dumped loaded chunks to log!");
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x000DBED3 File Offset: 0x000DA0D3
		[Description("Crash the client.")]
		private void CrashCommand(string[] args)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				throw new Exception("Forcefully crashed client!");
			});
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x000DBEFC File Offset: 0x000DA0FC
		[Usage("viewdistance", new string[]
		{
			"<distance>"
		})]
		[Description("Change the view distance without using the settings.")]
		private void ViewDistanceCommand(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num;
			bool flag2 = int.TryParse(args[0], out num);
			if (flag2)
			{
				this._gameInstance.App.Settings.ViewDistance = num;
				this._gameInstance.Connection.SendPacket(new ViewRadius(num));
				this._gameInstance.Chat.Log("View Distance set to: " + num.ToString());
			}
			else
			{
				this._gameInstance.Chat.Log(args[0] + " is not a valid int!");
			}
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x000DBFA0 File Offset: 0x000DA1A0
		[Usage("chunkymin", new string[]
		{
			"<y>"
		})]
		[Description("Only chunks above this Y will be rendered.")]
		private void ChunkYMinCommand(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int chunkYMin;
			bool flag2 = int.TryParse(args[0], out chunkYMin);
			if (flag2)
			{
				this._gameInstance.MapModule.ChunkYMin = chunkYMin;
				this._gameInstance.MapModule.DoWithMapGeometryBuilderPaused(true, null);
			}
			else
			{
				this._gameInstance.Chat.Log(args[0] + " is not a valid int!");
			}
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x000DC018 File Offset: 0x000DA218
		[Usage("occlusion", new string[]
		{
			"[on|off|draw_on|draw_off|reproject_on|reproject_off|...]>"
		})]
		private void Occlusion(string[] args)
		{
			bool flag = args.Length != 1 && args.Length != 4;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 1826513822U)
			{
				if (num <= 1162256192U)
				{
					if (num <= 614084496U)
					{
						if (num <= 38000240U)
						{
							if (num != 10978487U)
							{
								if (num == 38000240U)
								{
									if (text2 == "alphatested_off")
									{
										this._gameInstance.UseAlphaTestedChunkOccluders(false);
										return;
									}
								}
							}
							else if (text2 == "reproject_on")
							{
								this._gameInstance.UseOcclusionCullingReprojection(true);
								return;
							}
						}
						else if (num != 139251641U)
						{
							if (num == 614084496U)
							{
								if (text2 == "opaque_off")
								{
									this._gameInstance.UseOpaqueChunkOccluders(false);
									return;
								}
							}
						}
						else if (text2 == "debug_chunks_off")
						{
							this._gameInstance.DebugDrawOccludeeChunks = false;
							return;
						}
					}
					else if (num <= 981472812U)
					{
						if (num != 704875552U)
						{
							if (num == 981472812U)
							{
								if (text2 == "debug_particles_on")
								{
									this._gameInstance.DebugDrawOccludeeParticles = true;
									return;
								}
							}
						}
						else if (text2 == "for_entities_anim_off")
						{
							this._gameInstance.UseOcclusionCullingForEntitiesAnimations = false;
							return;
						}
					}
					else if (num != 1102433060U)
					{
						if (num == 1162256192U)
						{
							if (text2 == "fill_on")
							{
								this._gameInstance.UseOcclusionCullingReprojectionHoleFilling(true);
								return;
							}
						}
					}
					else if (text2 == "for_lights_on")
					{
						this._gameInstance.UseOcclusionCullingForLights = true;
						return;
					}
				}
				else if (num <= 1501235565U)
				{
					if (num <= 1370916102U)
					{
						if (num != 1212719694U)
						{
							if (num == 1370916102U)
							{
								if (text2 == "debug_particles_off")
								{
									this._gameInstance.DebugDrawOccludeeParticles = false;
									return;
								}
							}
						}
						else if (text2 == "alphatested_on")
						{
							this._gameInstance.UseAlphaTestedChunkOccluders(true);
							return;
						}
					}
					else if (num != 1466627581U)
					{
						if (num == 1501235565U)
						{
							if (text2 == "draw_on")
							{
								this._gameInstance.DrawOcclusionMap = true;
								return;
							}
						}
					}
					else if (text2 == "debug_chunks_on")
					{
						this._gameInstance.DebugDrawOccludeeChunks = true;
						return;
					}
				}
				else if (num <= 1630810064U)
				{
					if (num != 1545268118U)
					{
						if (num == 1630810064U)
						{
							if (text2 == "on")
							{
								this._gameInstance.SetOcclusionCulling(true);
								return;
							}
						}
					}
					else if (text2 == "debug_lights_off")
					{
						this._gameInstance.DebugDrawOccludeeLights = false;
						return;
					}
				}
				else if (num != 1681939482U)
				{
					if (num == 1826513822U)
					{
						if (text2 == "for_entities_anim_on")
						{
							this._gameInstance.UseOcclusionCullingForEntitiesAnimations = true;
							return;
						}
					}
				}
				else if (text2 == "fill_off")
				{
					this._gameInstance.UseOcclusionCullingReprojectionHoleFilling(false);
					return;
				}
			}
			else if (num <= 3273362014U)
			{
				if (num <= 2712386967U)
				{
					if (num <= 2185691355U)
					{
						if (num != 2152230684U)
						{
							if (num == 2185691355U)
							{
								if (text2 == "plane_off")
								{
									this._gameInstance.UseChunkOccluderPlanes(false);
									return;
								}
							}
						}
						else if (text2 == "debug_lights_on")
						{
							this._gameInstance.DebugDrawOccludeeLights = true;
							return;
						}
					}
					else if (num != 2664701816U)
					{
						if (num == 2712386967U)
						{
							if (text2 == "plane_on")
							{
								this._gameInstance.UseChunkOccluderPlanes(true);
								return;
							}
						}
					}
					else if (text2 == "player_on")
					{
						this._gameInstance.UseLocalPlayerOccluder = true;
						return;
					}
				}
				else if (num <= 3010017010U)
				{
					if (num != 2872740362U)
					{
						if (num == 3010017010U)
						{
							if (text2 == "for_entities_on")
							{
								this._gameInstance.UseOcclusionCullingForEntities = true;
								return;
							}
						}
					}
					else if (text2 == "off")
					{
						this._gameInstance.SetOcclusionCulling(false);
						return;
					}
				}
				else if (num != 3102347292U)
				{
					if (num == 3273362014U)
					{
						if (text2 == "for_lights_off")
						{
							this._gameInstance.UseOcclusionCullingForLights = false;
							return;
						}
					}
				}
				else if (text2 == "debug_entities_off")
				{
					this._gameInstance.DebugDrawOccludeeEntities = false;
					return;
				}
			}
			else if (num <= 3902973563U)
			{
				if (num <= 3371029838U)
				{
					if (num != 3339664682U)
					{
						if (num == 3371029838U)
						{
							if (text2 == "for_particles_off")
							{
								this._gameInstance.UseOcclusionCullingForParticles = false;
								return;
							}
						}
					}
					else if (text2 == "debug_entities_on")
					{
						this._gameInstance.DebugDrawOccludeeEntities = true;
						return;
					}
				}
				else if (num != 3378807160U)
				{
					if (num == 3902973563U)
					{
						if (text2 == "reproject_off")
						{
							this._gameInstance.UseOcclusionCullingReprojection(false);
							return;
						}
					}
				}
				else if (text2 == "break")
				{
					int x;
					int y;
					int z;
					bool flag2 = int.TryParse(args[1], out x) && int.TryParse(args[2], out y) && int.TryParse(args[3], out z);
					if (flag2)
					{
						this._gameInstance.MapModule.RegisterDestroyedBlock(x, y, z);
					}
					return;
				}
			}
			else if (num <= 4072008836U)
			{
				if (num != 4055701742U)
				{
					if (num == 4072008836U)
					{
						if (text2 == "for_entities_off")
						{
							this._gameInstance.UseOcclusionCullingForEntities = false;
							return;
						}
					}
				}
				else if (text2 == "opaque_on")
				{
					this._gameInstance.UseOpaqueChunkOccluders(true);
					return;
				}
			}
			else if (num != 4124649714U)
			{
				if (num != 4154183113U)
				{
					if (num == 4264618740U)
					{
						if (text2 == "for_particles_on")
						{
							this._gameInstance.UseOcclusionCullingForParticles = true;
							return;
						}
					}
				}
				else if (text2 == "draw_off")
				{
					this._gameInstance.DrawOcclusionMap = false;
					return;
				}
			}
			else if (text2 == "player_off")
			{
				this._gameInstance.UseLocalPlayerOccluder = false;
				return;
			}
			int opaqueOccludersCount;
			bool flag3 = int.TryParse(args[0], out opaqueOccludersCount);
			if (!flag3)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.SetOpaqueOccludersCount(opaqueOccludersCount);
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x000DC850 File Offset: 0x000DAA50
		[Usage("debugmap", new string[]
		{
			"[on|off|list|<map_name>]"
		})]
		private void DebugMap(string[] args)
		{
			string text = null;
			RenderTargetStore rtstore = this._gameInstance.Engine.Graphics.RTStore;
			string text2 = args[0].ToLower();
			bool verticalDisplay = false;
			bool flag = false;
			string text3 = text2;
			string a = text3;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "list"))
					{
						if (!(a == "v"))
						{
							if (!(a == "h"))
							{
								string[] array = new string[]
								{
									text2
								};
								int num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
								bool flag2 = num == 1 && args.Length == 3;
								if (flag2)
								{
									string text4 = args[1];
									string a2 = text4;
									if (!(a2 == "scale"))
									{
										float num2;
										float num3;
										bool flag3 = float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2) && float.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
										if (flag3)
										{
											Rectangle viewport = this._gameInstance.Engine.Window.Viewport;
											num2 = MathHelper.Clamp(num2, 0f, 1f);
											num3 = MathHelper.Clamp(num3, 0f, 1f);
											rtstore.SetDebugMapViewport(text2, num2, num3);
										}
									}
									else
									{
										float scale = 1f;
										bool flag4 = float.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out scale);
										if (flag4)
										{
											rtstore.SetDebugMapScale(text2, scale);
										}
									}
								}
								bool flag5 = num == 0;
								if (flag5)
								{
									this._gameInstance.Chat.Log("Unknown map(s) name(s)");
								}
							}
							else
							{
								flag = true;
								verticalDisplay = false;
							}
						}
						else
						{
							flag = true;
							verticalDisplay = true;
						}
					}
					else
					{
						text = rtstore.GetDebugMapList();
					}
				}
				else
				{
					this._gameInstance.DebugMap = false;
				}
			}
			else
			{
				this._gameInstance.DebugMap = true;
			}
			bool flag6 = flag;
			if (flag6)
			{
				string text5 = args[1];
				string text6 = text5;
				uint num4 = <PrivateImplementationDetails>.ComputeStringHash(text6);
				string[] array;
				int num;
				if (num4 <= 1811195782U)
				{
					if (num4 <= 1284960763U)
					{
						if (num4 != 482419122U)
						{
							if (num4 == 1284960763U)
							{
								if (text6 == "preset_wboit_e_lowres")
								{
									array = new string[]
									{
										"blend_accu_lowres",
										"blend_weight_lowres",
										"blend_reveal_lowres",
										"blend_add_lowres"
									};
									num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
									goto IL_602;
								}
							}
						}
						else if (text6 == "preset_moit")
						{
							array = new string[]
							{
								"blend_moment",
								"blend_tod",
								"blend_accu",
								"blend_weight",
								"blend_reveal"
							};
							num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
							goto IL_602;
						}
					}
					else if (num4 != 1366418079U)
					{
						if (num4 != 1462085794U)
						{
							if (num4 == 1811195782U)
							{
								if (text6 == "preset_wboit_e")
								{
									array = new string[]
									{
										"blend_accu",
										"blend_weight",
										"blend_reveal",
										"blend_add"
									};
									num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
									goto IL_602;
								}
							}
						}
						else if (text6 == "preset_wboit")
						{
							array = new string[]
							{
								"blend_accu",
								"blend_weight",
								"blend_reveal"
							};
							num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
							goto IL_602;
						}
					}
					else if (text6 == "preset_poit")
					{
						array = new string[]
						{
							"blend_accu",
							"blend_weight",
							"blend_beta"
						};
						num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
						goto IL_602;
					}
				}
				else if (num4 <= 2220816247U)
				{
					if (num4 != 2211780260U)
					{
						if (num4 == 2220816247U)
						{
							if (text6 == "preset_moit_lowres")
							{
								array = new string[]
								{
									"blend_moment",
									"blend_tod",
									"blend_accu_lowres",
									"blend_weight_lowres",
									"blend_reveal_lowres"
								};
								num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
								goto IL_602;
							}
						}
					}
					else if (text6 == "preset_poit_lowres")
					{
						array = new string[]
						{
							"blend_accu_lowres",
							"blend_weight_lowres",
							"blend_beta_lowres"
						};
						num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
						goto IL_602;
					}
				}
				else if (num4 != 2545589383U)
				{
					if (num4 != 3605956854U)
					{
						if (num4 == 3926075733U)
						{
							if (text6 == "preset_ssao")
							{
								array = new string[]
								{
									"ssao",
									"linear_z"
								};
								num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
								goto IL_602;
							}
						}
					}
					else if (text6 == "preset_gbuffer")
					{
						array = new string[]
						{
							"gbuffer_albedo",
							"gbuffer_normal",
							"gbuffer_light",
							"gbuffer_sun"
						};
						num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
						goto IL_602;
					}
				}
				else if (text6 == "preset_wboit_lowres")
				{
					array = new string[]
					{
						"blend_accu_lowres",
						"blend_weight_lowres",
						"blend_reveal_lowres"
					};
					num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
					goto IL_602;
				}
				int num5 = args.Length - 1;
				array = new string[num5];
				for (int i = 0; i < num5; i++)
				{
					string text7 = args[i + 1];
					array[i] = text7;
				}
				num = this._gameInstance.SelectDebugMaps(array, verticalDisplay);
				IL_602:
				bool flag7 = num == 0;
				if (flag7)
				{
					this._gameInstance.Chat.Log("Unknown map(s) name(s)");
				}
			}
			bool flag8 = text != null;
			if (flag8)
			{
				this._gameInstance.Chat.Log("Available maps:\n" + text);
			}
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x000DCEAC File Offset: 0x000DB0AC
		[Usage("debugpixel", new string[]
		{
			"[off|list|some_info_name]"
		})]
		private void DebugPixelSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = null;
			bool flag2 = false;
			string text2 = args[0].ToLower().Trim();
			string text3 = text2;
			string a = text3;
			if (!(a == "list"))
			{
				if (!(a == "off"))
				{
					flag2 = this._gameInstance.SetDebugPixelInfo(true, text2);
				}
				else
				{
					flag2 = this._gameInstance.SetDebugPixelInfo(false, null);
				}
			}
			else
			{
				text = this._gameInstance.GetDebugPixelInfoList();
			}
			bool flag3 = text != null;
			if (flag3)
			{
				this._gameInstance.Chat.Log("Available pixels info:\n" + text);
			}
			bool flag4 = flag2;
			if (flag4)
			{
				this._gameInstance.Chat.Log("Debug pixel info changed to: " + args[0]);
			}
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x000DCF84 File Offset: 0x000DB184
		[Usage("light", new string[]
		{
			"[stencil_on|stencil_off|linear_on|linear_off|res_high|res_low|blend_add|blend_max|merge_on|merge_off|debug_on|debug_off|send_0|send_1]>"
		})]
		private void LightSetup(string[] args)
		{
			bool flag = args.Length != 1 && args.Length != 4;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 2589681232U)
			{
				if (num <= 542204885U)
				{
					if (num <= 206345642U)
					{
						if (num <= 59366265U)
						{
							if (num != 35336040U)
							{
								if (num == 59366265U)
								{
									if (text2 == "res_dynamic_on")
									{
										this._gameInstance.SceneRenderer.UseDynamicLightResolutionSelection = true;
										return;
									}
								}
							}
							else if (text2 == "res_low")
							{
								this._gameInstance.SceneRenderer.SetLightingResolution(SceneRenderer.LightingResolution.LOW);
								return;
							}
						}
						else if (num != 164339603U)
						{
							if (num == 206345642U)
							{
								if (text2 == "res_high")
								{
									this._gameInstance.SceneRenderer.SetLightingResolution(SceneRenderer.LightingResolution.FULL);
									return;
								}
							}
						}
						else if (text2 == "grid_doublebuffer_off")
						{
							this._gameInstance.UseClusteredLightingDoubleBuffering(false);
							return;
						}
					}
					else if (num <= 309300777U)
					{
						if (num != 240193581U)
						{
							if (num == 309300777U)
							{
								if (text2 == "stencil_off")
								{
									this._gameInstance.SceneRenderer.ClassicDeferredLighting.UseStencilForOuterLights = false;
									return;
								}
							}
						}
						else if (text2 == "merge_off")
						{
							this._gameInstance.SceneRenderer.ClassicDeferredLighting.UseLightGroups = false;
							return;
						}
					}
					else if (num != 345197720U)
					{
						if (num == 542204885U)
						{
							if (text2 == "grid_direct_on")
							{
								this._gameInstance.UseClusteredLightingDirectAccess(true);
								return;
							}
						}
					}
					else if (text2 == "grid_gpumap_off")
					{
						this._gameInstance.UseClusteredLightingMappedGPUBuffers(false);
						return;
					}
				}
				else if (num <= 1566910130U)
				{
					if (num <= 1161484084U)
					{
						if (num != 603280778U)
						{
							if (num == 1161484084U)
							{
								if (text2 == "blend_add")
								{
									this._gameInstance.SceneRenderer.UseLightBlendMax = false;
									return;
								}
							}
						}
						else if (text2 == "debug_off")
						{
							this._gameInstance.DebugDrawLight = false;
							return;
						}
					}
					else if (num != 1376239559U)
					{
						if (num == 1566910130U)
						{
							if (text2 == "linear_off")
							{
								this._gameInstance.SceneRenderer.SetLinearZForLight(false, false);
								return;
							}
						}
					}
					else if (text2 == "grid_custom_off")
					{
						this._gameInstance.UseClusteredLightingCustomZDistribution(false);
						return;
					}
				}
				else if (num <= 1775409148U)
				{
					if (num != 1741474801U)
					{
						if (num == 1775409148U)
						{
							if (text2 == "res_mix")
							{
								this._gameInstance.SceneRenderer.SetLightingResolution(SceneRenderer.LightingResolution.MIXED);
								return;
							}
						}
					}
					else if (text2 == "merge_on")
					{
						this._gameInstance.SceneRenderer.ClassicDeferredLighting.UseLightGroups = true;
						return;
					}
				}
				else if (num != 2201182545U)
				{
					if (num == 2589681232U)
					{
						if (text2 == "debug_on")
						{
							this._gameInstance.DebugDrawLight = true;
							return;
						}
					}
				}
				else if (text2 == "grid_direct_off")
				{
					this._gameInstance.UseClusteredLightingDirectAccess(false);
					return;
				}
			}
			else if (num <= 3134341859U)
			{
				if (num <= 2839584207U)
				{
					if (num <= 2702766933U)
					{
						if (num != 2665162233U)
						{
							if (num == 2702766933U)
							{
								if (text2 == "res_dynamic_off")
								{
									this._gameInstance.SceneRenderer.UseDynamicLightResolutionSelection = false;
									return;
								}
							}
						}
						else if (text2 == "grid_refine_on")
						{
							this._gameInstance.UseClusteredLightingRefinedVoxelization(true);
							return;
						}
					}
					else if (num != 2719038165U)
					{
						if (num == 2839584207U)
						{
							if (text2 == "grid_doublebuffer_on")
							{
								this._gameInstance.UseClusteredLightingDoubleBuffering(true);
								return;
							}
						}
					}
					else if (text2 == "grid_refine_off")
					{
						this._gameInstance.UseClusteredLightingRefinedVoxelization(false);
						return;
					}
				}
				else if (num <= 2988946390U)
				{
					if (num != 2944866961U)
					{
						if (num == 2988946390U)
						{
							if (text2 == "grid_gpumap_on")
							{
								this._gameInstance.UseClusteredLightingMappedGPUBuffers(true);
								return;
							}
						}
					}
					else if (text2 == "grid")
					{
						try
						{
							uint width = uint.Parse(args[1]);
							uint height = uint.Parse(args[2]);
							uint depth = uint.Parse(args[3]);
							this._gameInstance.SetClusteredLightingGridResolution(width, height, depth);
						}
						catch (Exception)
						{
						}
						return;
					}
				}
				else if (num != 3091844287U)
				{
					if (num == 3134341859U)
					{
						if (text2 == "grid_pbo_on")
						{
							this._gameInstance.UseClusteredLightingPBO(true);
							return;
						}
					}
				}
				else if (text2 == "grid_debug_on")
				{
					this._gameInstance.SetDebugLightClusters(true);
					return;
				}
			}
			else if (num <= 3334333747U)
			{
				if (num <= 3317556128U)
				{
					if (num != 3267692892U)
					{
						if (num == 3317556128U)
						{
							if (text2 == "send_0")
							{
								this._gameInstance.SceneRenderer.ClassicDeferredLighting.LightDataTransferMethod = 0;
								return;
							}
						}
					}
					else if (text2 == "clustered_on")
					{
						this._gameInstance.UseClusteredLighting(true);
						return;
					}
				}
				else if (num != 3326145750U)
				{
					if (num == 3334333747U)
					{
						if (text2 == "send_1")
						{
							this._gameInstance.SceneRenderer.ClassicDeferredLighting.LightDataTransferMethod = 1;
							return;
						}
					}
				}
				else if (text2 == "clustered_off")
				{
					this._gameInstance.UseClusteredLighting(false);
					return;
				}
			}
			else if (num <= 3774726712U)
			{
				if (num != 3457321639U)
				{
					if (num == 3774726712U)
					{
						if (text2 == "linear_on")
						{
							this._gameInstance.SceneRenderer.SetLinearZForLight(true, false);
							return;
						}
					}
				}
				else if (text2 == "grid_pbo_off")
				{
					this._gameInstance.UseClusteredLightingPBO(false);
					return;
				}
			}
			else if (num != 3827241177U)
			{
				if (num != 3846236195U)
				{
					if (num == 4048159565U)
					{
						if (text2 == "stencil_on")
						{
							this._gameInstance.SceneRenderer.ClassicDeferredLighting.UseStencilForOuterLights = true;
							return;
						}
					}
				}
				else if (text2 == "grid_debug_off")
				{
					this._gameInstance.SetDebugLightClusters(false);
					return;
				}
			}
			else if (text2 == "blend_max")
			{
				this._gameInstance.SceneRenderer.UseLightBlendMax = true;
				return;
			}
			throw new InvalidCommandUsage();
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x000DD7B4 File Offset: 0x000DB9B4
		[Usage("lbuffercompression", new string[]
		{
			"[on|off]"
		})]
		private void LightBufferCompressionSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.SetLightBufferCompression(false);
			}
			else
			{
				this._gameInstance.SetLightBufferCompression(true);
			}
			this._gameInstance.Chat.Log("LBuffer compression state : " + args[0]);
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x000DD840 File Offset: 0x000DBA40
		[Usage("shadowmap", new string[]
		{
			"on|off|",
			"dir_topdown|dir_sun|dir_custom [x,y,z]",
			"intensity [0.0-1.0]",
			"cascade [1|2|3|4]",
			"debug_cascade|debug_frustum|debug_frustum_split|debug_cascade_frustum"
		})]
		private void ShadowMappingSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			bool flag2 = true;
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 2016490230U)
			{
				if (num <= 1141889376U)
				{
					if (num <= 553362250U)
					{
						if (num <= 263456517U)
						{
							if (num != 10564565U)
							{
								if (num != 170209365U)
								{
									if (num != 263456517U)
									{
										goto IL_112F;
									}
									if (!(text2 == "info"))
									{
										goto IL_112F;
									}
								}
								else
								{
									if (!(text2 == "cascade"))
									{
										goto IL_112F;
									}
									int sunShadowsCascadeCount;
									bool flag3 = int.TryParse(args[1], out sunShadowsCascadeCount);
									if (flag3)
									{
										this._gameInstance.SceneRenderer.SetSunShadowsCascadeCount(sunShadowsCascadeCount);
									}
									else
									{
										flag2 = false;
										this._gameInstance.Chat.Error("Invalid cascade count provided.");
									}
								}
							}
							else
							{
								if (!(text2 == "debug_cascade"))
								{
									goto IL_112F;
								}
								this._gameInstance.SceneRenderer.ToggleSunShadowMapCascadeDebug();
							}
						}
						else if (num <= 347417478U)
						{
							if (num != 285629897U)
							{
								if (num != 347417478U)
								{
									goto IL_112F;
								}
								if (!(text2 == "stable_on"))
								{
									goto IL_112F;
								}
								this._gameInstance.SceneRenderer.SetSunShadowMappingStableProjectionEnabled(true);
							}
							else
							{
								if (!(text2 == "fade_off"))
								{
									goto IL_112F;
								}
								this._gameInstance.SceneRenderer.SetDeferredShadowsFadingEnabled(false);
							}
						}
						else if (num != 501384829U)
						{
							if (num != 553362250U)
							{
								goto IL_112F;
							}
							if (!(text2 == "safe_angle_off"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.SetSunShadowsSafeAngleEnabled(false);
						}
						else
						{
							if (!(text2 == "chunks_on"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.SetSunShadowsWithChunks(true);
						}
					}
					else if (num <= 818349986U)
					{
						if (num <= 804546073U)
						{
							if (num != 762713488U)
							{
								if (num != 804546073U)
								{
									goto IL_112F;
								}
								if (!(text2 == "res"))
								{
									goto IL_112F;
								}
								uint width;
								bool flag4 = uint.TryParse(args[1], out width);
								if (flag4)
								{
									uint height = 0U;
									uint.TryParse(args[2], out height);
									this._gameInstance.SceneRenderer.SetSunShadowMapResolution(width, height);
								}
								else
								{
									flag2 = false;
									this._gameInstance.Chat.Error("Invalid resolution values provided.");
								}
							}
							else
							{
								if (!(text2 == "bias_1"))
								{
									goto IL_112F;
								}
								this._gameInstance.SceneRenderer.ToggleSunShadowsBiasMethod1();
							}
						}
						else if (num != 813046345U)
						{
							if (num != 818349986U)
							{
								goto IL_112F;
							}
							if (!(text2 == "freeze"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.ToggleFreeze();
						}
						else
						{
							if (!(text2 == "bias_2"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.ToggleSunShadowsBiasMethod2();
						}
					}
					else if (num <= 902478281U)
					{
						if (num != 886407365U)
						{
							if (num != 902478281U)
							{
								goto IL_112F;
							}
							if (!(text2 == "particle_on"))
							{
								goto IL_112F;
							}
							this._gameInstance.UseParticleSunShadows(true);
						}
						else
						{
							if (!(text2 == "debug_frustum"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.ToggleCameraFrustumDebug();
						}
					}
					else if (num != 905370805U)
					{
						if (num != 1141889376U)
						{
							goto IL_112F;
						}
						if (!(text2 == "cull_underground_chunks_off"))
						{
							goto IL_112F;
						}
						this._gameInstance.CullUndergroundChunkShadowCasters = false;
					}
					else
					{
						if (!(text2 == "lod_backface_on"))
						{
							goto IL_112F;
						}
						this._gameInstance.SetUseShadowBackfaceLODDistance(true, -1f);
					}
				}
				else if (num <= 1529282937U)
				{
					if (num <= 1260975919U)
					{
						if (num != 1157204043U)
						{
							if (num != 1203949697U)
							{
								if (num != 1260975919U)
								{
									goto IL_112F;
								}
								if (!(text2 == "cull_small_entities_off"))
								{
									goto IL_112F;
								}
								this._gameInstance.CullSmallEntityShadowCasters = false;
							}
							else
							{
								if (!(text2 == "draw_instanced"))
								{
									goto IL_112F;
								}
								this._gameInstance.SceneRenderer.ToggleSunShadowCastersDrawInstanced();
							}
						}
						else
						{
							if (!(text2 == "cache_off"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.SetSunShadowMapCachingEnabled(false);
						}
					}
					else if (num <= 1457797354U)
					{
						if (num != 1295367429U)
						{
							if (num != 1457797354U)
							{
								goto IL_112F;
							}
							if (!(text2 == "camera_bias_on"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.SetDeferredShadowsCameraBiasEnabled(true);
						}
						else
						{
							if (!(text2 == "particle_off"))
							{
								goto IL_112F;
							}
							this._gameInstance.UseParticleSunShadows(false);
						}
					}
					else if (num != 1487491948U)
					{
						if (num != 1529282937U)
						{
							goto IL_112F;
						}
						if (!(text2 == "manual_off"))
						{
							goto IL_112F;
						}
						this._gameInstance.SceneRenderer.SetDeferredShadowsManualModeEnabled(false);
					}
					else
					{
						if (!(text2 == "dir_topdown"))
						{
							goto IL_112F;
						}
						this._gameInstance.SceneRenderer.SetSunShadowsDirectionTopDown();
					}
				}
				else if (num <= 1697748444U)
				{
					if (num <= 1630810064U)
					{
						if (num != 1589560606U)
						{
							if (num != 1630810064U)
							{
								goto IL_112F;
							}
							if (!(text2 == "on"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.SetSunShadowsEnabled(true);
						}
						else
						{
							if (!(text2 == "linearz_on"))
							{
								goto IL_112F;
							}
							this._gameInstance.SceneRenderer.SetSunShadowMappingUseLinearZ(true);
						}
					}
					else if (num != 1646609324U)
					{
						if (num != 1697748444U)
						{
							goto IL_112F;
						}
						if (!(text2 == "camera_bias_off"))
						{
							goto IL_112F;
						}
						this._gameInstance.SceneRenderer.SetDeferredShadowsCameraBiasEnabled(false);
					}
					else
					{
						if (!(text2 == "debug_cascade_frustum"))
						{
							goto IL_112F;
						}
						this._gameInstance.SceneRenderer.ToggleShadowCascadeFrustumDebug();
					}
				}
				else if (num <= 1990924993U)
				{
					if (num != 1849493664U)
					{
						if (num != 1990924993U)
						{
							goto IL_112F;
						}
						if (!(text2 == "cull_underground_entities_on"))
						{
							goto IL_112F;
						}
						this._gameInstance.CullUndergroundEntityShadowCasters = true;
					}
					else
					{
						if (!(text2 == "linearz_off"))
						{
							goto IL_112F;
						}
						this._gameInstance.SceneRenderer.SetSunShadowMappingUseLinearZ(false);
					}
				}
				else if (num != 2012977835U)
				{
					if (num != 2016490230U)
					{
						goto IL_112F;
					}
					if (!(text2 == "state"))
					{
						goto IL_112F;
					}
				}
				else
				{
					if (!(text2 == "cascade_smart"))
					{
						goto IL_112F;
					}
					this._gameInstance.SceneRenderer.SetSunShadowCastersSmartCascadeDispatchEnabled(!this._gameInstance.SceneRenderer.UseSunShadowsSmartCascadeDispatch);
				}
				goto IL_1135;
			}
			if (num <= 2969317968U)
			{
				if (num <= 2306437984U)
				{
					if (num <= 2169733819U)
					{
						if (num != 2020825311U)
						{
							if (num != 2132061994U)
							{
								if (num == 2169733819U)
								{
									if (text2 == "dir_sun_clean")
									{
										this._gameInstance.SceneRenderer.SetSunShadowsDirectionSun(true);
										goto IL_1135;
									}
								}
							}
							else if (text2 == "dir_custom")
							{
								float x;
								float y;
								float z;
								bool flag5 = args.Length == 4 && float.TryParse(args[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out x) && float.TryParse(args[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out y) && float.TryParse(args[3], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out z);
								if (flag5)
								{
									this._gameInstance.SceneRenderer.SetSunShadowsDirectionCustom(new Vector3(x, y, z));
								}
								else
								{
									flag2 = false;
									this._gameInstance.Chat.Error(".shadowmap " + args[0] + " - invalid parameters.");
								}
								goto IL_1135;
							}
						}
						else if (text2 == "slope_scale_bias")
						{
							float factor;
							float units;
							bool flag6 = args.Length == 3 && float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out factor) && float.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out units);
							if (flag6)
							{
								this._gameInstance.SceneRenderer.SetSunShadowsSlopeScaleBias(factor, units);
							}
							else
							{
								flag2 = false;
								this._gameInstance.Chat.Error(".shadowmap " + args[0] + " - invalid parameters.");
							}
							goto IL_1135;
						}
					}
					else if (num <= 2252834397U)
					{
						if (num != 2237916426U)
						{
							if (num == 2252834397U)
							{
								if (text2 == "cull_underground_entities_off")
								{
									this._gameInstance.CullUndergroundEntityShadowCasters = false;
									goto IL_1135;
								}
							}
						}
						else if (text2 == "intensity")
						{
							float num2;
							bool flag7 = float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2) && num2 >= 0f && num2 <= 1f;
							if (flag7)
							{
								this._gameInstance.SceneRenderer.SetSunShadowsIntensity(num2);
							}
							else
							{
								flag2 = false;
								this._gameInstance.Chat.Error("Invalid intensity value provided.");
							}
							goto IL_1135;
						}
					}
					else if (num != 2277381309U)
					{
						if (num == 2306437984U)
						{
							if (text2 == "normal_bias_off")
							{
								this._gameInstance.SceneRenderer.SetDeferredShadowsNormalBiasEnabled(false);
								goto IL_1135;
							}
						}
					}
					else if (text2 == "manual_on")
					{
						this._gameInstance.SceneRenderer.SetDeferredShadowsManualModeEnabled(true);
						goto IL_1135;
					}
				}
				else if (num <= 2818407847U)
				{
					if (num <= 2597550352U)
					{
						if (num != 2408885917U)
						{
							if (num == 2597550352U)
							{
								if (text2 == "safe_angle_on")
								{
									this._gameInstance.SceneRenderer.SetSunShadowsSafeAngleEnabled(true);
									goto IL_1135;
								}
							}
						}
						else if (text2 == "kdop")
						{
							this._gameInstance.SceneRenderer.SetSunShadowsGlobalKDopEnabled(!this._gameInstance.SceneRenderer.UseSunShadowsGlobalKDop);
							goto IL_1135;
						}
					}
					else if (num != 2620177347U)
					{
						if (num == 2818407847U)
						{
							if (text2 == "cache_on")
							{
								this._gameInstance.SceneRenderer.SetSunShadowMapCachingEnabled(true);
								goto IL_1135;
							}
						}
					}
					else if (text2 == "noise_on")
					{
						this._gameInstance.SceneRenderer.SetDeferredShadowsNoiseEnabled(true);
						goto IL_1135;
					}
				}
				else if (num <= 2872740362U)
				{
					if (num != 2840061160U)
					{
						if (num == 2872740362U)
						{
							if (text2 == "off")
							{
								this._gameInstance.SceneRenderer.SetSunShadowsEnabled(false);
								goto IL_1135;
							}
						}
					}
					else if (text2 == "stable_off")
					{
						this._gameInstance.SceneRenderer.SetSunShadowMappingStableProjectionEnabled(false);
						goto IL_1135;
					}
				}
				else if (num != 2944350215U)
				{
					if (num == 2969317968U)
					{
						if (text2 == "blur_on")
						{
							this._gameInstance.SceneRenderer.SetDeferredShadowsBlurEnabled(true);
							goto IL_1135;
						}
					}
				}
				else if (text2 == "noise_off")
				{
					this._gameInstance.SceneRenderer.SetDeferredShadowsNoiseEnabled(false);
					goto IL_1135;
				}
			}
			else if (num <= 3229783613U)
			{
				if (num <= 3065056945U)
				{
					if (num <= 3048105950U)
					{
						if (num != 2974822476U)
						{
							if (num == 3048105950U)
							{
								if (text2 == "normal_bias_on")
								{
									this._gameInstance.SceneRenderer.SetDeferredShadowsNormalBiasEnabled(true);
									goto IL_1135;
								}
							}
						}
						else if (text2 == "lod_backface_dist")
						{
							int num3;
							bool flag8 = int.TryParse(args[1], out num3);
							if (flag8)
							{
								this._gameInstance.SetUseShadowBackfaceLODDistance(true, (float)num3);
							}
							goto IL_1135;
						}
					}
					else if (num != 3052458854U)
					{
						if (num == 3065056945U)
						{
							if (text2 == "lod_backface_off")
							{
								this._gameInstance.SetUseShadowBackfaceLODDistance(false, -1f);
								goto IL_1135;
							}
						}
					}
					else if (text2 == "debug_frustum_split")
					{
						this._gameInstance.SceneRenderer.ToggleCameraFrustumSplitsDebug();
						goto IL_1135;
					}
				}
				else if (num <= 3137483493U)
				{
					if (num != 3070998193U)
					{
						if (num == 3137483493U)
						{
							if (text2 == "dir_sun")
							{
								this._gameInstance.SceneRenderer.SetSunShadowsDirectionSun(false);
								goto IL_1135;
							}
						}
					}
					else if (text2 == "alphablended_off")
					{
						this._gameInstance.UseAlphaBlendedChunksSunShadows(false);
						goto IL_1135;
					}
				}
				else if (num != 3218714735U)
				{
					if (num == 3229783613U)
					{
						if (text2 == "model_vfx")
						{
							this._gameInstance.SceneRenderer.ToggleSunShadowsWithModelVFXs();
							goto IL_1135;
						}
					}
				}
				else if (text2 == "deferred_scale")
				{
					float num4;
					bool flag9 = float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num4) && num4 > 0f && num4 <= 1f;
					if (flag9)
					{
						this._gameInstance.SceneRenderer.SetDeferredShadowResolutionScale(num4);
					}
					else
					{
						flag2 = false;
						this._gameInstance.Chat.Error("Invalid scale value provided.");
					}
					goto IL_1135;
				}
			}
			else if (num <= 3409983853U)
			{
				if (num <= 3340382635U)
				{
					if (num != 3273030026U)
					{
						if (num == 3340382635U)
						{
							if (text2 == "cull_small_entities_on")
							{
								this._gameInstance.CullSmallEntityShadowCasters = true;
								goto IL_1135;
							}
						}
					}
					else if (text2 == "blur_off")
					{
						this._gameInstance.SceneRenderer.SetDeferredShadowsBlurEnabled(false);
						goto IL_1135;
					}
				}
				else if (num != 3386255838U)
				{
					if (num == 3409983853U)
					{
						if (text2 == "fade_on")
						{
							this._gameInstance.SceneRenderer.SetDeferredShadowsFadingEnabled(true);
							goto IL_1135;
						}
					}
				}
				else if (text2 == "cull_underground_chunks_on")
				{
					this._gameInstance.CullUndergroundChunkShadowCasters = true;
					goto IL_1135;
				}
			}
			else if (num <= 3782907061U)
			{
				if (num != 3412141744U)
				{
					if (num == 3782907061U)
					{
						if (text2 == "alphablended_on")
						{
							this._gameInstance.UseAlphaBlendedChunksSunShadows(true);
							goto IL_1135;
						}
					}
				}
				else if (text2 == "single_sample_on")
				{
					this._gameInstance.SceneRenderer.SetDeferredShadowsWithSingleSampleEnabled(true);
					goto IL_1135;
				}
			}
			else if (num != 4026765674U)
			{
				if (num == 4135930169U)
				{
					if (text2 == "chunks_off")
					{
						this._gameInstance.SceneRenderer.SetSunShadowsWithChunks(false);
						goto IL_1135;
					}
				}
			}
			else if (text2 == "single_sample_off")
			{
				this._gameInstance.SceneRenderer.SetDeferredShadowsWithSingleSampleEnabled(false);
				goto IL_1135;
			}
			IL_112F:
			throw new InvalidCommandUsage();
			IL_1135:
			bool flag10 = flag2;
			if (flag10)
			{
				string message = this._gameInstance.SceneRenderer.WriteShadowMappingStateToString();
				this._gameInstance.Chat.Log(message);
			}
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x000DE9B0 File Offset: 0x000DCBB0
		[Usage("volsunshaft", new string[]
		{
			"on|off|",
			"strength <x>"
		})]
		private void VolumetricSunshaftSetup(string[] args)
		{
			bool flag = args.Length < 1 || args.Length > 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "strength"))
					{
						throw new InvalidCommandUsage();
					}
					float volumetricSunshaftStrength;
					bool flag2 = float.TryParse(args[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out volumetricSunshaftStrength);
					if (flag2)
					{
						this._gameInstance.PostEffectRenderer.SetVolumetricSunshaftStrength(volumetricSunshaftStrength);
					}
					else
					{
						this._gameInstance.Chat.Error(args[1] + " is not a valid number!");
					}
				}
				else
				{
					this._gameInstance.UseVolumetricSunshaft(false);
				}
			}
			else
			{
				this._gameInstance.UseVolumetricSunshaft(true);
			}
			this._gameInstance.Chat.Log("Volumetric sunshafts " + args[0]);
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x000DEA9C File Offset: 0x000DCC9C
		[Usage("water", new string[]
		{
			"[0|1|2|3] - quality settings, for test!"
		})]
		private void WaterTestSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "0"))
			{
				if (!(a == "1"))
				{
					if (!(a == "2"))
					{
						if (!(a == "3"))
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SetWaterQuality(3);
					}
					else
					{
						this._gameInstance.SetWaterQuality(2);
					}
				}
				else
				{
					this._gameInstance.SetWaterQuality(1);
				}
			}
			else
			{
				this._gameInstance.SetWaterQuality(0);
			}
			this._gameInstance.Chat.Log("Water quality state : " + args[0]);
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x000DEB60 File Offset: 0x000DCD60
		[Usage("renderscale", new string[]
		{
			"<percentage>\n (default : 100)"
		})]
		private void ResolutionScaleSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num;
			bool flag2 = int.TryParse(args[0], out num);
			if (flag2)
			{
				float resolutionScale = (float)num * 0.01f;
				bool flag3 = this._gameInstance.SetResolutionScale(resolutionScale);
				bool flag4 = flag3;
				if (flag4)
				{
					this._gameInstance.Chat.Log(string.Format("Render scale changed to {0}.", num));
				}
				else
				{
					this._gameInstance.Chat.Log(string.Format("Invalid render scale factor {0}.\nMin-Max range is [{1} - {2}]", num, this._gameInstance.ResolutionScaleMin * 100f, this._gameInstance.ResolutionScaleMax * 100f));
				}
			}
			else
			{
				this._gameInstance.Chat.Log("Invalid input : " + args[0]);
			}
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x000DEC48 File Offset: 0x000DCE48
		[Usage("dof", new string[]
		{
			"[on | preset_1 | preset_2 | preset_3 | off | v1 | v2 | v2b | v3]"
		})]
		private void DepthOfFieldSetup(string[] args)
		{
			int num = 0;
			string text = args[num++].ToLower();
			string text2 = text;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num2 <= 1630810064U)
			{
				if (num2 <= 1036778419U)
				{
					if (num2 != 1020000800U)
					{
						if (num2 == 1036778419U)
						{
							if (text2 == "preset_2")
							{
								this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
								this._gameInstance.PostEffectRenderer.SetupDepthOfField(1f, 2f, 30f, 70f, 0.5f, 0.3f);
								this._gameInstance.Chat.Log("depth of field preset 2");
								return;
							}
						}
					}
					else if (text2 == "preset_3")
					{
						this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
						this._gameInstance.PostEffectRenderer.SetupDepthOfField(2f, 15f, 1000f, 1000f, 0.5f, 0f);
						this._gameInstance.Chat.Log("depth of field preset 3");
						return;
					}
				}
				else if (num2 != 1053556038U)
				{
					if (num2 == 1630810064U)
					{
						if (text2 == "on")
						{
							this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
							this._gameInstance.Chat.Log("depth of field is on.");
							return;
						}
					}
				}
				else if (text2 == "preset_1")
				{
					this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
					this._gameInstance.PostEffectRenderer.SetupDepthOfField(0f, 0f, 3f, 10f, 0f, 0.4f);
					this._gameInstance.Chat.Log("depth of field preset 1");
					return;
				}
			}
			else if (num2 <= 2487895656U)
			{
				if (num2 != 1827961465U)
				{
					if (num2 == 2487895656U)
					{
						if (text2 == "v1")
						{
							this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
							this._gameInstance.PostEffectRenderer.SetDepthOfFieldVersion(0);
							this._gameInstance.Chat.Log("depth of field version 1");
							return;
						}
					}
				}
				else if (text2 == "v2b")
				{
					this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
					this._gameInstance.PostEffectRenderer.SetDepthOfFieldVersion(2);
					this._gameInstance.Chat.Log("depth of field version 2bis");
					return;
				}
			}
			else if (num2 != 2521450894U)
			{
				if (num2 != 2538228513U)
				{
					if (num2 == 2872740362U)
					{
						if (text2 == "off")
						{
							this._gameInstance.PostEffectRenderer.UseDepthOfField(false);
							this._gameInstance.Chat.Log("depth of field is off.");
							return;
						}
					}
				}
				else if (text2 == "v2")
				{
					this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
					this._gameInstance.PostEffectRenderer.SetDepthOfFieldVersion(1);
					this._gameInstance.Chat.Log("depth of field version 2");
					return;
				}
			}
			else if (text2 == "v3")
			{
				this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
				this._gameInstance.PostEffectRenderer.SetDepthOfFieldVersion(3);
				this._gameInstance.Chat.Log("depth of field version 3");
				return;
			}
			bool flag = args.Length != 6;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.PostEffectRenderer.UseDepthOfField(true);
			float num3;
			bool flag2 = float.TryParse(args[num - 1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
			bool flag3 = !flag2;
			if (flag3)
			{
				this._gameInstance.Chat.Error(args[num - 1] + " is not a valid number!");
			}
			float num4;
			flag2 = float.TryParse(args[num], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num4);
			bool flag4 = !flag2;
			if (flag4)
			{
				this._gameInstance.Chat.Error(args[num] + " is not a valid number!");
			}
			float num5;
			flag2 = float.TryParse(args[num + 1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num5);
			bool flag5 = !flag2;
			if (flag5)
			{
				this._gameInstance.Chat.Error(args[num + 1] + " is not a valid number!");
			}
			float num6;
			flag2 = float.TryParse(args[num + 2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num6);
			bool flag6 = !flag2;
			if (flag6)
			{
				this._gameInstance.Chat.Error(args[num + 2] + " is not a valid number!");
			}
			float num7;
			flag2 = float.TryParse(args[num + 3], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num7);
			bool flag7 = !flag2;
			if (flag7)
			{
				this._gameInstance.Chat.Error(args[num + 3] + " is not a valid number!");
			}
			float num8;
			flag2 = float.TryParse(args[num + 4], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num8);
			bool flag8 = !flag2;
			if (flag8)
			{
				this._gameInstance.Chat.Error(args[num + 4] + " is not a valid number!");
			}
			this._gameInstance.PostEffectRenderer.SetupDepthOfField(num3, num4, num5, num6, num7, num8);
			this._gameInstance.Chat.Log(string.Format("depth of field parameters set to {0} {1} {2} {3} {4} {5}", new object[]
			{
				num3,
				num4,
				num5,
				num6,
				num7,
				num8
			}));
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x000DF250 File Offset: 0x000DD450
		[Usage("ssao", new string[]
		{
			"on|off|blur_on|blur_off",
			"max [0.0-1.0]",
			"strength [0.0-10.0]",
			"radius [0.1-2.0]",
			"reset_params"
		})]
		private void SSAOSetup(string[] args)
		{
			bool flag = args.Length < 1 || args.Length > 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 1630810064U)
			{
				if (num <= 263456517U)
				{
					if (num != 230313139U)
					{
						if (num != 241064269U)
						{
							if (num != 263456517U)
							{
								goto IL_410;
							}
							if (!(text2 == "info"))
							{
								goto IL_410;
							}
						}
						else
						{
							if (!(text2 == "temporal_on"))
							{
								goto IL_410;
							}
							this._gameInstance.SceneRenderer.SetUseSSAO(true, true, -1);
						}
					}
					else
					{
						if (!(text2 == "radius"))
						{
							goto IL_410;
						}
						float num2;
						bool flag2 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
						if (flag2)
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SceneRenderer.SSAOParamRadius = num2;
					}
				}
				else if (num <= 890022063U)
				{
					if (num != 873244444U)
					{
						if (num != 890022063U)
						{
							goto IL_410;
						}
						if (!(text2 == "0"))
						{
							goto IL_410;
						}
						this._gameInstance.SceneRenderer.SetUseSSAO(true, true, 0);
					}
					else
					{
						if (!(text2 == "1"))
						{
							goto IL_410;
						}
						this._gameInstance.SceneRenderer.SetUseSSAO(true, true, 1);
					}
				}
				else if (num != 923577301U)
				{
					if (num != 1630810064U)
					{
						goto IL_410;
					}
					if (!(text2 == "on"))
					{
						goto IL_410;
					}
					this._gameInstance.SceneRenderer.SetUseSSAO(true, true, -1);
				}
				else
				{
					if (!(text2 == "2"))
					{
						goto IL_410;
					}
					this._gameInstance.SceneRenderer.SetUseSSAO(true, true, 2);
				}
			}
			else if (num <= 2969317968U)
			{
				if (num <= 2016490230U)
				{
					if (num != 1862445131U)
					{
						if (num != 2016490230U)
						{
							goto IL_410;
						}
						if (!(text2 == "state"))
						{
							goto IL_410;
						}
					}
					else
					{
						if (!(text2 == "reset_params"))
						{
							goto IL_410;
						}
						this._gameInstance.SceneRenderer.ResetSSAOParameters();
					}
				}
				else if (num != 2872740362U)
				{
					if (num != 2969317968U)
					{
						goto IL_410;
					}
					if (!(text2 == "blur_on"))
					{
						goto IL_410;
					}
					this._gameInstance.SceneRenderer.UseSSAOBlur = true;
				}
				else
				{
					if (!(text2 == "off"))
					{
						goto IL_410;
					}
					this._gameInstance.SceneRenderer.SetUseSSAO(false, true, -1);
				}
			}
			else if (num <= 3617776409U)
			{
				if (num != 3273030026U)
				{
					if (num != 3617776409U)
					{
						goto IL_410;
					}
					if (!(text2 == "max"))
					{
						goto IL_410;
					}
					float num2;
					bool flag3 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
					if (flag3)
					{
						throw new InvalidCommandUsage();
					}
					this._gameInstance.SceneRenderer.SSAOParamOcclusionMax = num2;
				}
				else
				{
					if (!(text2 == "blur_off"))
					{
						goto IL_410;
					}
					this._gameInstance.SceneRenderer.UseSSAOBlur = false;
				}
			}
			else if (num != 3648188457U)
			{
				if (num != 3766098096U)
				{
					goto IL_410;
				}
				if (!(text2 == "strength"))
				{
					goto IL_410;
				}
				float num2;
				bool flag4 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
				if (flag4)
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.SceneRenderer.SSAOParamOcclusionStrength = num2;
			}
			else
			{
				if (!(text2 == "temporal_off"))
				{
					goto IL_410;
				}
				this._gameInstance.SceneRenderer.SetUseSSAO(true, false, -1);
			}
			goto IL_416;
			IL_410:
			throw new InvalidCommandUsage();
			IL_416:
			string text3 = "SSAO state : " + args[0] + "\n";
			text3 += string.Format(".quality : {0}\n", this._gameInstance.SceneRenderer.SSAOQuality);
			text3 += string.Format(".max : {0}\n", this._gameInstance.SceneRenderer.SSAOParamOcclusionMax);
			text3 += string.Format(".strength: {0}\n", this._gameInstance.SceneRenderer.SSAOParamOcclusionStrength);
			text3 += string.Format(".radius: {0}", this._gameInstance.SceneRenderer.SSAOParamRadius);
			this._gameInstance.Chat.Log(text3);
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x000DF730 File Offset: 0x000DD930
		[Usage("skyambient", new string[]
		{
			"[on|off|intensity X]\n (default : intensity = 0.12)"
		})]
		private void SkyAmbientSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "less_at_noon_on"))
					{
						if (!(a == "less_at_noon_off"))
						{
							if (!(a == "intensity"))
							{
								throw new InvalidCommandUsage();
							}
							float skyAmbientIntensity;
							bool flag2 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out skyAmbientIntensity);
							if (flag2)
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.SetSkyAmbientIntensity(skyAmbientIntensity);
						}
						else
						{
							this._gameInstance.UseLessSkyAmbientAtNoon = false;
						}
					}
					else
					{
						this._gameInstance.UseLessSkyAmbientAtNoon = true;
					}
				}
				else
				{
					this._gameInstance.SceneRenderer.SetUseSkyAmbient(false);
				}
			}
			else
			{
				this._gameInstance.SceneRenderer.SetUseSkyAmbient(true);
			}
			this._gameInstance.Chat.Log("Ambient from sky state : " + args[0]);
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x000DF834 File Offset: 0x000DDA34
		[Usage("bloom", new string[]
		{
			"[on {sun,fb,sunshaft,underwater} | off {sun,fb,sunshaft,underwater}] | intensity[0-1] | sun_intensity[0-1] | power[1-10] | sunshaft_scale[1-4] | sunshaft_intensity[0-0.5]"
		})]
		private void BloomSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			bool flag2 = true;
			int i = 0;
			string text = args[i++].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 2172753914U)
			{
				if (num <= 1035581717U)
				{
					if (num <= 327759535U)
					{
						if (num != 13026411U)
						{
							if (num == 327759535U)
							{
								if (text2 == "pow_power")
								{
									float num2;
									bool flag3 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num2);
									if (flag3)
									{
										this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
										return;
									}
									this._gameInstance.DefaultBloomPower = num2;
									this._gameInstance.PostEffectRenderer.SetBloomOnPowPower(num2);
									goto IL_AB4;
								}
							}
						}
						else if (text2 == "dither_off")
						{
							this._gameInstance.PostEffectRenderer.UseDitheringOnBloom(false);
							goto IL_AB4;
						}
					}
					else if (num != 616129863U)
					{
						if (num == 1035581717U)
						{
							if (text2 == "down")
							{
								int downsampleMethod;
								bool flag4 = !int.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out downsampleMethod);
								if (flag4)
								{
									this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
									return;
								}
								this._gameInstance.PostEffectRenderer.SetDownsampleMethod(downsampleMethod);
								goto IL_AB4;
							}
						}
					}
					else if (text2 == "dither_on")
					{
						this._gameInstance.PostEffectRenderer.UseDitheringOnBloom(true);
						goto IL_AB4;
					}
				}
				else if (num <= 1630810064U)
				{
					if (num != 1128467232U)
					{
						if (num == 1630810064U)
						{
							if (text2 == "on")
							{
								this._gameInstance.PostEffectRenderer.UseBloom(true);
								while (i < args.Length)
								{
									string text3 = args[i].ToLower();
									string a = text3;
									if (!(a == "sun"))
									{
										if (!(a == "moon"))
										{
											if (!(a == "fb"))
											{
												if (!(a == "pow"))
												{
													if (!(a == "sunshaft"))
													{
														if (!(a == "underwater"))
														{
															flag2 = false;
														}
														else
														{
															this._gameInstance.UseBloomUnderwater = true;
														}
													}
													else
													{
														this._gameInstance.PostEffectRenderer.UseBloomSunShaft(true);
													}
												}
												else
												{
													this._gameInstance.PostEffectRenderer.UseBloomOnFullscreen(true);
												}
											}
											else
											{
												this._gameInstance.PostEffectRenderer.UseBloomOnFullbright(true);
											}
										}
										else
										{
											this._gameInstance.PostEffectRenderer.UseBloomOnMoon(true);
										}
									}
									else
									{
										this._gameInstance.PostEffectRenderer.UseBloomOnSun(true);
									}
									i++;
								}
								goto IL_AB4;
							}
						}
					}
					else if (text2 == "up")
					{
						int upsampleMethod;
						bool flag5 = !int.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out upsampleMethod);
						if (flag5)
						{
							this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
							return;
						}
						this._gameInstance.PostEffectRenderer.SetUpsampleMethod(upsampleMethod);
						goto IL_AB4;
					}
				}
				else if (num != 2110549221U)
				{
					if (num == 2172753914U)
					{
						if (text2 == "underwater_intensity")
						{
							float num3;
							bool flag6 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num3);
							if (flag6)
							{
								this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
								return;
							}
							this._gameInstance.UnderwaterBloomIntensity = num3;
							this._gameInstance.PostEffectRenderer.SetBloomOnPowIntensity(num3);
							goto IL_AB4;
						}
					}
				}
				else if (text2 == "sunshaft_intensity")
				{
					float sunshaftIntensity;
					bool flag7 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out sunshaftIntensity);
					if (flag7)
					{
						this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
						return;
					}
					this._gameInstance.PostEffectRenderer.SetSunshaftIntensity(sunshaftIntensity);
					goto IL_AB4;
				}
			}
			else if (num <= 2504673275U)
			{
				if (num <= 2237916426U)
				{
					if (num != 2202882350U)
					{
						if (num == 2237916426U)
						{
							if (text2 == "intensity")
							{
								float bloomGlobalIntensity;
								bool flag8 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bloomGlobalIntensity);
								if (flag8)
								{
									this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
									return;
								}
								this._gameInstance.PostEffectRenderer.SetBloomGlobalIntensity(bloomGlobalIntensity);
								goto IL_AB4;
							}
						}
					}
					else if (text2 == "sunshaft_scale")
					{
						float sunshaftScaleFactor;
						bool flag9 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out sunshaftScaleFactor);
						if (flag9)
						{
							this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
							return;
						}
						this._gameInstance.PostEffectRenderer.SetSunshaftScaleFactor(sunshaftScaleFactor);
						goto IL_AB4;
					}
				}
				else if (num != 2487895656U)
				{
					if (num == 2504673275U)
					{
						if (text2 == "v0")
						{
							this._gameInstance.PostEffectRenderer.SetBloomVersion(0);
							goto IL_AB4;
						}
					}
				}
				else if (text2 == "v1")
				{
					this._gameInstance.PostEffectRenderer.SetBloomVersion(1);
					goto IL_AB4;
				}
			}
			else if (num <= 2872740362U)
			{
				if (num != 2806816203U)
				{
					if (num == 2872740362U)
					{
						if (text2 == "off")
						{
							bool flag10 = i == args.Length;
							if (flag10)
							{
								this._gameInstance.PostEffectRenderer.UseBloom(false);
								goto IL_AB4;
							}
							while (i < args.Length)
							{
								string text4 = args[i].ToLower();
								string a2 = text4;
								if (!(a2 == "sun"))
								{
									if (!(a2 == "moon"))
									{
										if (!(a2 == "fb"))
										{
											if (!(a2 == "pow"))
											{
												if (!(a2 == "sunshaft"))
												{
													if (!(a2 == "underwater"))
													{
														flag2 = false;
													}
													else
													{
														this._gameInstance.UseBloomUnderwater = false;
													}
												}
												else
												{
													this._gameInstance.PostEffectRenderer.UseBloomSunShaft(false);
												}
											}
											else
											{
												this._gameInstance.PostEffectRenderer.UseBloomOnFullscreen(false);
											}
										}
										else
										{
											this._gameInstance.PostEffectRenderer.UseBloomOnFullbright(false);
										}
									}
									else
									{
										this._gameInstance.PostEffectRenderer.UseBloomOnMoon(false);
									}
								}
								else
								{
									this._gameInstance.PostEffectRenderer.UseBloomOnSun(false);
								}
								i++;
							}
							goto IL_AB4;
						}
					}
				}
				else if (text2 == "sun_intensity")
				{
					float sunIntensity;
					bool flag11 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out sunIntensity);
					if (flag11)
					{
						this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
						return;
					}
					this._gameInstance.PostEffectRenderer.SetSunIntensity(sunIntensity);
					goto IL_AB4;
				}
			}
			else if (num != 3874857775U)
			{
				if (num != 4081571094U)
				{
					if (num == 4115604294U)
					{
						if (text2 == "power")
						{
							float bloomPower;
							bool flag12 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bloomPower);
							if (flag12)
							{
								this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
								return;
							}
							this._gameInstance.PostEffectRenderer.SetBloomPower(bloomPower);
							goto IL_AB4;
						}
					}
				}
				else if (text2 == "underwater_power")
				{
					float num4;
					bool flag13 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num4);
					if (flag13)
					{
						this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
						return;
					}
					this._gameInstance.UnderwaterBloomPower = num4;
					this._gameInstance.PostEffectRenderer.SetBloomOnPowPower(num4);
					goto IL_AB4;
				}
			}
			else if (text2 == "pow_intensity")
			{
				float num5;
				bool flag14 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num5);
				if (flag14)
				{
					this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
					return;
				}
				this._gameInstance.DefaultBloomIntensity = num5;
				this._gameInstance.PostEffectRenderer.SetBloomOnPowIntensity(num5);
				goto IL_AB4;
			}
			float i2;
			bool flag15 = !float.TryParse(args[i - 1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out i2);
			if (flag15)
			{
				this._gameInstance.Chat.Error(args[i - 1] + " is not a valid number!");
				return;
			}
			float i3;
			bool flag16 = !float.TryParse(args[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out i3);
			if (flag16)
			{
				this._gameInstance.Chat.Error(args[i] + " is not a valid number!");
				return;
			}
			float i4;
			bool flag17 = !float.TryParse(args[i + 1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out i4);
			if (flag17)
			{
				this._gameInstance.Chat.Error(args[i + 1] + " is not a valid number!");
				return;
			}
			float i5;
			bool flag18 = !float.TryParse(args[i + 2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out i5);
			if (flag18)
			{
				this._gameInstance.Chat.Error(args[i + 2] + " is not a valid number!");
				return;
			}
			float i6;
			bool flag19 = !float.TryParse(args[i + 3], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out i6);
			if (flag19)
			{
				this._gameInstance.Chat.Error(args[i + 3] + " is not a valid number!");
				return;
			}
			this._gameInstance.PostEffectRenderer.SetBloomIntensities(i2, i3, i4, i5, i6);
			IL_AB4:
			bool flag20 = flag2;
			if (!flag20)
			{
				throw new InvalidCommandUsage();
			}
			string message = this._gameInstance.PostEffectRenderer.PrintBloomState();
			this._gameInstance.Chat.Log(message);
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x000E032C File Offset: 0x000DE52C
		[Usage("blur", new string[]
		{
			"[strong|normal|light|off]"
		})]
		private void BlurSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "light"))
					{
						if (!(a == "normal"))
						{
							if (!(a == "strong"))
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.PostEffectRenderer.SetBlurStrength(3);
						}
						else
						{
							this._gameInstance.PostEffectRenderer.SetBlurStrength(2);
						}
					}
					else
					{
						this._gameInstance.PostEffectRenderer.SetBlurStrength(1);
					}
				}
				else
				{
					this._gameInstance.PostEffectRenderer.UseBlur(false);
				}
			}
			else
			{
				this._gameInstance.PostEffectRenderer.UseBlur(true);
			}
			this._gameInstance.Chat.Log("Blur : " + args[0]);
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x000E0420 File Offset: 0x000DE620
		[Usage("caustics", new string[]
		{
			"[on|off|intensity X|scale X|distortion X]\n (default : intensity = 1.0, scale = 0.095, distortion = 0.05)"
		})]
		private void UnderwaterCausticsSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			string a = text2;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "intensity"))
					{
						if (!(a == "scale"))
						{
							if (!(a == "distortion"))
							{
								throw new InvalidCommandUsage();
							}
							float num;
							bool flag2 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
							if (flag2)
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.SetUnderwaterCausticsDistortion(num);
						}
						else
						{
							float num;
							bool flag3 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
							if (flag3)
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.SetUnderwaterCausticsScale(num);
						}
					}
					else
					{
						float num;
						bool flag4 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
						if (flag4)
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SetUnderwaterCausticsIntensity(num);
					}
				}
				else
				{
					this._gameInstance.SetUseUnderwaterCaustics(false);
				}
			}
			else
			{
				this._gameInstance.SetUseUnderwaterCaustics(true);
			}
			this._gameInstance.PrintUnderwaterCausticsParams();
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x000E055C File Offset: 0x000DE75C
		[Usage("clouduvmotion", new string[]
		{
			"[scale X|strength X]\n (default : scale = 50, strength = 0.1)"
		})]
		private void CloudsUVMotionSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			string a = text2;
			if (!(a == "scale"))
			{
				if (!(a == "strength"))
				{
					throw new InvalidCommandUsage();
				}
				float num;
				bool flag2 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
				if (flag2)
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.SetCloudsUVMotionStrength(num);
			}
			else
			{
				float num;
				bool flag3 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
				if (flag3)
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.SetCloudsUVMotionScale(num);
			}
			this._gameInstance.PrintCloudsUVMotionParams();
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x000E0618 File Offset: 0x000DE818
		[Usage("cloudshadow", new string[]
		{
			"[on|off|intensity X|scale X|blur X|speed X]\n (default : intensity = 0.25, scale = 0.005, blur = 3.5, speed = 1.0)"
		})]
		private void CloudsShadowsSetup(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			string a = text2;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "intensity"))
					{
						if (!(a == "scale"))
						{
							if (!(a == "blur"))
							{
								if (!(a == "speed"))
								{
									throw new InvalidCommandUsage();
								}
								float num;
								bool flag2 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
								if (flag2)
								{
									throw new InvalidCommandUsage();
								}
								this._gameInstance.SetCloudsShadowsSpeed(num);
							}
							else
							{
								float num;
								bool flag3 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
								if (flag3)
								{
									throw new InvalidCommandUsage();
								}
								this._gameInstance.SetCloudsShadowsBlurriness(num);
							}
						}
						else
						{
							float num;
							bool flag4 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
							if (flag4)
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.SetCloudsShadowsScale(num);
						}
					}
					else
					{
						float num;
						bool flag5 = !float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
						if (flag5)
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SetCloudsShadowsIntensity(num);
					}
				}
				else
				{
					this._gameInstance.SetUseCloudsShadows(false);
				}
			}
			else
			{
				this._gameInstance.SetUseCloudsShadows(true);
			}
			this._gameInstance.PrintCloudsShadowsParams();
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x000E0794 File Offset: 0x000DE994
		[Usage("sky", new string[]
		{
			"[rotation 0-360 | sun_height (%)]"
		})]
		private void Sky(string[] args)
		{
			bool flag = args.Length < 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num = 0;
			string text = args[num++].ToLower();
			string a = text;
			if (!(a == "sun_height"))
			{
				if (!(a == "rotation"))
				{
					if (!(a == "dither_on"))
					{
						if (!(a == "dither_off"))
						{
							if (!(a == "test"))
							{
								throw new InvalidCommandUsage();
							}
							this._gameInstance.UseSkyboxTest = !this._gameInstance.UseSkyboxTest;
						}
						else
						{
							this._gameInstance.UseDitheringOnSky(false);
						}
					}
					else
					{
						this._gameInstance.UseDitheringOnSky(true);
					}
				}
				else
				{
					float degrees;
					bool flag2 = float.TryParse(args[num], out degrees);
					if (flag2)
					{
						this._gameInstance.WeatherModule.SkyRotation = Quaternion.CreateFromAxisAngle(Vector3.Down, MathHelper.ToRadians(degrees));
						this._gameInstance.Chat.Log("Sky rotated by " + args[num] + " from default angle");
					}
					else
					{
						this._gameInstance.Chat.Error(args[num] + " is not a valid number!");
					}
				}
			}
			else
			{
				float num2;
				bool flag3 = float.TryParse(args[num], out num2);
				if (flag3)
				{
					num2 = MathHelper.Clamp(num2, 0f, 150f);
					this._gameInstance.WeatherModule.SunHeight = 2f * (num2 / 100f);
					this._gameInstance.Chat.Log(string.Format("Sun moved to {0}% of default height", num2));
				}
				else
				{
					this._gameInstance.Chat.Error(args[num] + " is not a valid number!");
				}
			}
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x000E0960 File Offset: 0x000DEB60
		[Usage("forcefield", new string[]
		{
			"[sphere|wall|anim_on|anim_off|distort_on|distort_off|color_on|color_off|off]"
		})]
		private void ForceFieldSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			bool flag2 = true;
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 2338261066U)
			{
				if (num <= 2024388324U)
				{
					if (num != 1112031231U)
					{
						if (num != 1892056626U)
						{
							if (num == 2024388324U)
							{
								if (text2 == "distort_on")
								{
									this._gameInstance.ForceFieldOptionDistortion = true;
									goto IL_2CF;
								}
							}
						}
						else if (text2 == "box")
						{
							this._gameInstance.ForceFieldTest = 3;
							goto IL_2CF;
						}
					}
					else if (text2 == "outline_on")
					{
						this._gameInstance.ForceFieldOptionOutline = true;
						goto IL_2CF;
					}
				}
				else if (num != 2051696968U)
				{
					if (num != 2232868894U)
					{
						if (num == 2338261066U)
						{
							if (text2 == "color_off")
							{
								this._gameInstance.ForceFieldOptionColor = false;
								goto IL_2CF;
							}
						}
					}
					else if (text2 == "distort_off")
					{
						this._gameInstance.ForceFieldOptionDistortion = false;
						goto IL_2CF;
					}
				}
				else if (text2 == "anim_on")
				{
					this._gameInstance.ForceFieldOptionAnimation = true;
					goto IL_2CF;
				}
			}
			else if (num <= 2950268184U)
			{
				if (num != 2804296981U)
				{
					if (num != 2872740362U)
					{
						if (num == 2950268184U)
						{
							if (text2 == "sphere")
							{
								this._gameInstance.ForceFieldTest = 2;
								goto IL_2CF;
							}
						}
					}
					else if (text2 == "off")
					{
						this._gameInstance.ForceFieldTest = 0;
						goto IL_2CF;
					}
				}
				else if (text2 == "wall")
				{
					this._gameInstance.ForceFieldTest = 1;
					goto IL_2CF;
				}
			}
			else if (num != 3239060706U)
			{
				if (num != 3771801443U)
				{
					if (num == 4157972496U)
					{
						if (text2 == "color_on")
						{
							this._gameInstance.ForceFieldOptionColor = true;
							goto IL_2CF;
						}
					}
				}
				else if (text2 == "outline_off")
				{
					this._gameInstance.ForceFieldOptionOutline = false;
					goto IL_2CF;
				}
			}
			else if (text2 == "anim_off")
			{
				this._gameInstance.ForceFieldOptionAnimation = false;
				goto IL_2CF;
			}
			int forceFieldCount;
			bool flag3 = int.TryParse(args[0], out forceFieldCount);
			if (!flag3)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.ForceFieldCount = forceFieldCount;
			IL_2CF:
			bool flag4 = flag2;
			if (flag4)
			{
				this._gameInstance.Chat.Log("Post-FXAA state : " + args[0]);
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x000E0C64 File Offset: 0x000DEE64
		[Usage("sharpen", new string[]
		{
			"[on|off]"
		})]
		private void SharpenSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					float strength;
					bool flag2 = !float.TryParse(args[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out strength);
					if (flag2)
					{
						throw new InvalidCommandUsage();
					}
					this._gameInstance.PostEffectRenderer.UseFXAASharpened(true, strength);
				}
				else
				{
					this._gameInstance.PostEffectRenderer.UseFXAASharpened(false, -1f);
				}
			}
			else
			{
				this._gameInstance.PostEffectRenderer.UseFXAASharpened(true, -1f);
			}
			this._gameInstance.Chat.Log("Post-FXAA Sharpen state : " + args[0]);
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x000E0D34 File Offset: 0x000DEF34
		[Usage("fxaa", new string[]
		{
			"[on|off]"
		})]
		private void FXAASetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.PostEffectRenderer.UseFXAA(false);
			}
			else
			{
				this._gameInstance.PostEffectRenderer.UseFXAA(true);
			}
			this._gameInstance.Chat.Log("Post-FXAA state : " + args[0]);
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x000E0DC8 File Offset: 0x000DEFC8
		[Usage("taa", new string[]
		{
			"[on|off]"
		})]
		private void TAASetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.PostEffectRenderer.UseTemporalAA(false);
			}
			else
			{
				this._gameInstance.PostEffectRenderer.UseTemporalAA(true);
			}
			this._gameInstance.Chat.Log("Post-TAA state : " + args[0]);
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x000E0E5C File Offset: 0x000DF05C
		[Usage("distortion", new string[]
		{
			"[on|off]"
		})]
		private void DistortionSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.PostEffectRenderer.UseDistortion(false);
			}
			else
			{
				this._gameInstance.PostEffectRenderer.UseDistortion(true);
			}
			this._gameInstance.Chat.Log("Distortion state : " + args[0]);
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x000E0EF0 File Offset: 0x000DF0F0
		[Usage("postfx", new string[]
		{
			"[brightness x|contrast x] (default : brightness = 0; contrast = 1"
		})]
		private void PostFXSetup(string[] args)
		{
			bool flag = args.Length != 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "brightness"))
			{
				if (!(a == "contrast"))
				{
					throw new InvalidCommandUsage();
				}
				float num;
				bool flag2 = float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
				if (!flag2)
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.PostEffectRenderer.SetPostFXContrast(num);
			}
			else
			{
				float num;
				bool flag3 = float.TryParse(args[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out num);
				if (!flag3)
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.PostEffectRenderer.SetPostFXBrightness(num);
			}
			this._gameInstance.Chat.Log("Post-FX state : " + args[0] + " " + args[1]);
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x000E0FD0 File Offset: 0x000DF1D0
		[Usage("dmgindicatorangle", new string[]
		{
			"[angleValueDegree]"
		})]
		private void IndicatorAngle(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			int num;
			bool flag2 = int.TryParse(args[0], out num) && num >= 0;
			if (flag2)
			{
				this._gameInstance.DamageEffectModule.AngleHideDamage = num;
			}
			else
			{
				this._gameInstance.Chat.Log(args[0] + " is not a valid int or is not greater than or equal to 0!");
			}
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x000E1040 File Offset: 0x000DF240
		[Usage("speed", new string[]
		{
			"[speed]"
		})]
		private void SpeedCommand(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			float num;
			bool flag2 = float.TryParse(args[0], out num);
			if (flag2)
			{
				MovementController movementController = this._gameInstance.CharacterControllerModule.MovementController;
				MovementSettings movementSettings = movementController.MovementSettings;
				movementController.SpeedMultiplier = Math.Min(movementSettings.MaxSpeedMultiplier, Math.Max(movementSettings.MinSpeedMultiplier, num));
				this._gameInstance.Chat.Log(string.Format("Speed set to: {0}", num));
			}
			else
			{
				this._gameInstance.Chat.Log("Invalid speed value: " + args[0]);
			}
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x000E10EC File Offset: 0x000DF2EC
		[Usage("debugmove", new string[]
		{
			"[mode|off]"
		})]
		private void DebugMove(string[] args)
		{
			bool flag = args.Length > 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			MovementController.DebugMovement debugMovement = MovementController.DebugMovement.None;
			bool flag2 = args.Length == 1;
			if (flag2)
			{
				debugMovement = (MovementController.DebugMovement)Enum.Parse(typeof(MovementController.DebugMovement), args[0], true);
			}
			MovementController.DebugMovementMode = debugMovement;
			this._gameInstance.Chat.Log(string.Format("Debug movement mode set to: {0}", debugMovement));
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x000E1158 File Offset: 0x000DF358
		[Usage("movsettings", new string[]
		{
			"[mass|dragCoefficient|jumpForce|swimJumpForce|acceleration|airDragMin|airDragMax|airDragMinSpeed|airDragMaxSpeed|airFrictionMin|airFrictionMax|airFrictionMinSpeed|airFrictionMaxSpeed|airSpeedMul|airControlMinSpeed|airControlMaxSpeed|airControlMinMultiplier|airControlMaxMultiplier|comboAirSpeedMul|runSpeedMul|baseSpeed|climbSpeed|groundDrag|jumpBufferDuration|jumpBufferMaxYVelocity|forwardWalkSpeedMul|backwardWalkSpeedMul|strafeWalkSpeedMul|forwardRunSpeedMul|strafeRunSpeedMul|strafeRunSpeedMul|forwardCrouchSpeedMul|backwardCrouchSpeedMul|strafeCrouchSpeedMul|forwardSprintSpeedMul|mariojumpfallforce|fallThreshold|fallEffectDuration|fallJumpForce|fallMomentumLoss|autoJumpObstacleEffectDuration|autoJumpObstacleSpeedLoss|autoJumpObstacleSprintSpeedLoss|autoJumpObstacleMaxAngle|autoJumpObstacleSprintEffectDuration|autoJumpDisableJumping] [value]"
		})]
		private void UpdateMovementSettings(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				MovementSettings movementSettings = this._gameInstance.CharacterControllerModule.MovementController.MovementSettings;
				string message = string.Concat(new string[]
				{
					"Settings: \n",
					string.Format(" - mass: {0}\n", movementSettings.Mass),
					string.Format(" - dragCoefficient: {0}\n", movementSettings.DragCoefficient),
					string.Format(" - jumpForce: {0}\n", movementSettings.JumpForce),
					string.Format(" - swimJumpForce: {0}\n", movementSettings.SwimJumpForce),
					string.Format(" - jumpBufferDuration: {0}\n", movementSettings.JumpBufferDuration),
					string.Format(" - jumpBufferMaxYVelocity: {0}\n", movementSettings.JumpBufferMaxYVelocity),
					string.Format(" - acceleration: {0}\n", movementSettings.Acceleration),
					string.Format(" - airDragMin: {0}\n", movementSettings.AirDragMin),
					string.Format(" - airDragMax: {0}\n", movementSettings.AirDragMax),
					string.Format(" - airDragMinSpeed: {0}\n", movementSettings.AirDragMinSpeed),
					string.Format(" - airDragMaxSpeed: {0}\n", movementSettings.AirDragMaxSpeed),
					string.Format(" - airFrictionMin: {0}\n", movementSettings.AirFrictionMin),
					string.Format(" - airFrictionMax: {0}\n", movementSettings.AirFrictionMax),
					string.Format(" - airFrictionMinSpeed: {0}\n", movementSettings.AirFrictionMinSpeed),
					string.Format(" - airFrictionMaxSpeed: {0}\n", movementSettings.AirFrictionMaxSpeed),
					string.Format(" - airSpeedMul: {0}\n", movementSettings.AirSpeedMultiplier),
					string.Format(" - airControlMinSpeed: {0}\n", movementSettings.AirControlMinSpeed),
					string.Format(" - airControlMaxSpeed: {0}\n", movementSettings.AirControlMaxSpeed),
					string.Format(" - airControlMinMultiplier: {0}\n", movementSettings.AirControlMinMultiplier),
					string.Format(" - airControlMaxMultiplier: {0}\n", movementSettings.AirControlMaxMultiplier),
					string.Format(" - comboAirSpeedMul: {0}\n", movementSettings.ComboAirSpeedMultiplier),
					string.Format(" - baseSpeed: {0}\n", movementSettings.BaseSpeed),
					string.Format(" - climbSpeed: {0}\n", movementSettings.ClimbSpeed),
					string.Format(" - horizontalFlySpeed: {0}\n", movementSettings.HorizontalFlySpeed),
					string.Format(" - verticalFlySpeed: {0}\n", movementSettings.VerticalFlySpeed),
					string.Format(" - groundDrag: {0}\n", this._gameInstance.CharacterControllerModule.MovementController.DefaultBlockDrag),
					"\n\nTemporary settings: \n",
					string.Format(" - forwardWalkSpeedMul: {0}\n", movementSettings.ForwardWalkSpeedMultiplier),
					string.Format(" - backwardWalkSpeedMul: {0}\n", movementSettings.BackwardWalkSpeedMultiplier),
					string.Format(" - strafeWalkSpeedMul: {0}\n", movementSettings.StrafeWalkSpeedMultiplier),
					string.Format(" - forwardRunSpeedMul: {0}\n", movementSettings.ForwardRunSpeedMultiplier),
					string.Format(" - backwardRunSpeedMul: {0}\n", movementSettings.BackwardRunSpeedMultiplier),
					string.Format(" - strafeRunSpeedMul: {0}\n", movementSettings.StrafeRunSpeedMultiplier),
					string.Format(" - forwardCrouchSpeedMul: {0}\n", movementSettings.ForwardCrouchSpeedMultiplier),
					string.Format(" - backwardCrouchSpeedMul: {0}\n", movementSettings.BackwardCrouchSpeedMultiplier),
					string.Format(" - strafeCrouchSpeedMul: {0}\n", movementSettings.StrafeCrouchSpeedMultiplier),
					string.Format(" - forwardSprintSpeedMul: {0}\n", movementSettings.ForwardSprintSpeedMultiplier),
					"\n\n",
					string.Format(" - marioJumpFallForce: {0}\n", movementSettings.MarioJumpFallForce),
					string.Format(" - fallEffectDuration: {0}\n", movementSettings.FallEffectDuration),
					string.Format(" - fallJumpForce: {0}\n", movementSettings.FallJumpForce),
					string.Format(" - fallMomentumLoss: {0}\n", movementSettings.FallMomentumLoss),
					"\n\n",
					string.Format(" - autoJumpObstacleEffectDuration: {0}\n", movementSettings.AutoJumpObstacleEffectDuration),
					string.Format(" - autoJumpObstacleSpeedLoss: {0}\n", movementSettings.AutoJumpObstacleSpeedLoss),
					string.Format(" - autoJumpObstacleSprintSpeedLoss: {0}\n", movementSettings.AutoJumpObstacleSprintSpeedLoss),
					string.Format(" - autoJumpObstacleSprintEffectDuration: {0}\n", movementSettings.AutoJumpObstacleSprintEffectDuration),
					string.Format(" - autoJumpObstacleMaxAngle: {0}\n", movementSettings.AutoJumpObstacleMaxAngle),
					string.Format(" - autoJumpDisableJumping: {0}\n", movementSettings.AutoJumpDisableJumping)
				});
				this._gameInstance.Chat.Log(message);
			}
			else
			{
				string text = args[0].ToLower();
				bool flag2 = true;
				bool flag4;
				bool flag3 = bool.TryParse(args[1], out flag4);
				if (flag3)
				{
					string text2 = text;
					string a = text2;
					if (!(a == "autojumpdisablejumping"))
					{
						flag2 = false;
					}
					else
					{
						this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AutoJumpDisableJumping = flag4;
					}
					bool flag5 = flag2;
					if (flag5)
					{
						this._gameInstance.Chat.Log(string.Format("{0} changed to: {1}", text, flag4));
					}
				}
				else
				{
					float num;
					bool flag6 = float.TryParse(args[1], out num);
					if (flag6)
					{
						string text3 = text;
						string text4 = text3;
						uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text4);
						if (num2 <= 2071849184U)
						{
							if (num2 <= 1038759260U)
							{
								if (num2 <= 252028367U)
								{
									if (num2 <= 124372051U)
									{
										if (num2 != 93031711U)
										{
											if (num2 == 124372051U)
											{
												if (text4 == "basespeed")
												{
													this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.BaseSpeed = num;
													goto IL_1296;
												}
											}
										}
										else if (text4 == "falljumpforce")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.FallJumpForce = num;
											goto IL_1296;
										}
									}
									else if (num2 != 172825192U)
									{
										if (num2 != 184878001U)
										{
											if (num2 == 252028367U)
											{
												if (text4 == "strafecrouchspeedmul")
												{
													this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.StrafeCrouchSpeedMultiplier = num;
													goto IL_1296;
												}
											}
										}
										else if (text4 == "horizontalflyspeed")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.HorizontalFlySpeed = num;
											goto IL_1296;
										}
									}
									else if (text4 == "airfrictionmaxspeed")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirFrictionMaxSpeed = num;
										goto IL_1296;
									}
								}
								else if (num2 <= 799062685U)
								{
									if (num2 != 553538857U)
									{
										if (num2 != 795308170U)
										{
											if (num2 == 799062685U)
											{
												if (text4 == "acceleration")
												{
													this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.Acceleration = num;
													goto IL_1296;
												}
											}
										}
										else if (text4 == "backwardrunspeedmul")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.BackwardRunSpeedMultiplier = num;
											goto IL_1296;
										}
									}
									else if (text4 == "aircontrolmaxmultiplier")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirControlMaxMultiplier = num;
										goto IL_1296;
									}
								}
								else if (num2 != 858154740U)
								{
									if (num2 != 914207645U)
									{
										if (num2 == 1038759260U)
										{
											if (text4 == "strafewalkspeedmul")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.StrafeWalkSpeedMultiplier = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "airfrictionmax")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirFrictionMax = num;
										goto IL_1296;
									}
								}
								else if (text4 == "swimjumpforce")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.SwimJumpForce = num;
									goto IL_1296;
								}
							}
							else if (num2 <= 1373593993U)
							{
								if (num2 <= 1139862956U)
								{
									if (num2 != 1080703907U)
									{
										if (num2 == 1139862956U)
										{
											if (text4 == "dragCoefficient")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.DragCoefficient = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "airfrictionmin")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirFrictionMin = num;
										goto IL_1296;
									}
								}
								else if (num2 != 1252982220U)
								{
									if (num2 != 1362158265U)
									{
										if (num2 == 1373593993U)
										{
											if (text4 == "aircontrolmaxspeed")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirControlMaxSpeed = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "backwardcrouchspeedmul")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.BackwardCrouchSpeedMultiplier = num;
										goto IL_1296;
									}
								}
								else if (text4 == "jumpforce")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.JumpForce = num;
									goto IL_1296;
								}
							}
							else if (num2 <= 1759939836U)
							{
								if (num2 != 1438859525U)
								{
									if (num2 != 1621416780U)
									{
										if (num2 == 1759939836U)
										{
											if (text4 == "airspeedmul")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirSpeedMultiplier = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "straferunspeedmul")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.StrafeRunSpeedMultiplier = num;
										goto IL_1296;
									}
								}
								else if (text4 == "jumpbuffermaxyvelocity")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.JumpBufferMaxYVelocity = num;
									goto IL_1296;
								}
							}
							else if (num2 != 1890134017U)
							{
								if (num2 != 2021928055U)
								{
									if (num2 == 2071849184U)
									{
										if (text4 == "forwardrunspeedmul")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.ForwardRunSpeedMultiplier = num;
											goto IL_1296;
										}
									}
								}
								else if (text4 == "airdragmin")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirDragMin = num;
									goto IL_1296;
								}
							}
							else if (text4 == "autojumpobstaclesprintspeedloss")
							{
								this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AutoJumpObstacleSprintSpeedLoss = num;
								goto IL_1296;
							}
						}
						else if (num2 <= 2995664621U)
						{
							if (num2 <= 2602409995U)
							{
								if (num2 <= 2258094649U)
								{
									if (num2 != 2201702914U)
									{
										if (num2 == 2258094649U)
										{
											if (text4 == "airdragmax")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirDragMax = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "airdragminspeed")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirDragMinSpeed = num;
										goto IL_1296;
									}
								}
								else if (num2 != 2379894584U)
								{
									if (num2 != 2391044777U)
									{
										if (num2 == 2602409995U)
										{
											if (text4 == "forwardcrouchspeedmul")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.ForwardCrouchSpeedMultiplier = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "forwardsprintspeedmul")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.ForwardSprintSpeedMultiplier = num;
										goto IL_1296;
									}
								}
								else if (text4 == "forwardwalkspeedmul")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.ForwardWalkSpeedMultiplier = num;
									goto IL_1296;
								}
							}
							else if (num2 <= 2629025310U)
							{
								if (num2 != 2606214944U)
								{
									if (num2 != 2609425070U)
									{
										if (num2 == 2629025310U)
										{
											if (text4 == "autojumpobstaclemaxangle")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AutoJumpObstacleMaxAngle = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "backwardwalkspeedmul")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.BackwardWalkSpeedMultiplier = num;
										goto IL_1296;
									}
								}
								else if (text4 == "comboairspeedmul")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.ComboAirSpeedMultiplier = num;
									goto IL_1296;
								}
							}
							else if (num2 != 2644126903U)
							{
								if (num2 != 2906559098U)
								{
									if (num2 == 2995664621U)
									{
										if (text4 == "autojumpobstaclespeedloss")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AutoJumpObstacleSpeedLoss = num;
											goto IL_1296;
										}
									}
								}
								else if (text4 == "autojumpobstaclesprinteffectduration")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AutoJumpObstacleSprintEffectDuration = num;
									goto IL_1296;
								}
							}
							else if (text4 == "jumpbufferduration")
							{
								this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.JumpBufferDuration = num;
								goto IL_1296;
							}
						}
						else if (num2 <= 3566439565U)
						{
							if (num2 <= 3336600510U)
							{
								if (num2 != 3095479309U)
								{
									if (num2 != 3194214387U)
									{
										if (num2 == 3336600510U)
										{
											if (text4 == "airfrictionminspeed")
											{
												this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirFrictionMinSpeed = num;
												goto IL_1296;
											}
										}
									}
									else if (text4 == "verticalflyspeed")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.VerticalFlySpeed = num;
										goto IL_1296;
									}
								}
								else if (text4 == "climbspeed")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.ClimbSpeed = num;
									goto IL_1296;
								}
							}
							else if (num2 != 3438532875U)
							{
								if (num2 != 3532619590U)
								{
									if (num2 == 3566439565U)
									{
										if (text4 == "mariojumpfallforce")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.MarioJumpFallForce = num;
											goto IL_1296;
										}
									}
								}
								else if (text4 == "autojumpobstacleeffectduration")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AutoJumpObstacleEffectDuration = num;
									goto IL_1296;
								}
							}
							else if (text4 == "aircontrolminmultiplier")
							{
								this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirControlMinMultiplier = num;
								goto IL_1296;
							}
						}
						else if (num2 <= 3907098337U)
						{
							if (num2 != 3749132497U)
							{
								if (num2 != 3806123231U)
								{
									if (num2 == 3907098337U)
									{
										if (text4 == "falleffectduration")
										{
											this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.FallEffectDuration = num;
											goto IL_1296;
										}
									}
								}
								else if (text4 == "aircontrolminspeed")
								{
									this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirControlMinSpeed = num;
									goto IL_1296;
								}
							}
							else if (text4 == "mass")
							{
								this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.Mass = num;
								goto IL_1296;
							}
						}
						else if (num2 != 3913333208U)
						{
							if (num2 != 4109125636U)
							{
								if (num2 == 4220820517U)
								{
									if (text4 == "fallmomentumloss")
									{
										this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.FallMomentumLoss = num;
										goto IL_1296;
									}
								}
							}
							else if (text4 == "airdragmaxspeed")
							{
								this._gameInstance.CharacterControllerModule.MovementController.MovementSettings.AirDragMaxSpeed = num;
								goto IL_1296;
							}
						}
						else if (text4 == "grounddrag")
						{
							this._gameInstance.CharacterControllerModule.MovementController.DefaultBlockDrag = num;
							goto IL_1296;
						}
						flag2 = false;
						IL_1296:
						bool flag7 = flag2;
						if (flag7)
						{
							this._gameInstance.Chat.Log(string.Format("{0} changed to: {1}", text, num));
						}
					}
				}
				bool flag8 = !flag2;
				if (flag8)
				{
					throw new InvalidCommandUsage();
				}
			}
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x000E2438 File Offset: 0x000E0638
		[Usage("speedo", new string[]
		{
			"[on|off]>"
		})]
		private void UpdateSpeedometer(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.App.Interface.InGameView.SpeedometerComponent.Enabled = false;
			}
			else
			{
				this._gameInstance.App.Interface.InGameView.SpeedometerComponent.Enabled = true;
			}
			this._gameInstance.App.Interface.InGameView.UpdateSpeedometerVisibility(true);
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x000E24E8 File Offset: 0x000E06E8
		[Usage("render", new string[]
		{
			"[on|off|list] <name> - \ne.g. render off map_near"
		})]
		private void RenderSetup(string[] args)
		{
			bool flag = args.Length > 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text3 = null;
			string text = args[0].ToLower();
			bool enable = false;
			string text2 = text;
			string a = text2;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					if (!(a == "pause"))
					{
						if (!(a == "list"))
						{
							throw new InvalidCommandUsage();
						}
						text3 = string.Format("{0}", string.Join(",", this._gameInstance.RenderPassNames));
					}
					else
					{
						this._gameInstance.RenderTimePaused = !this._gameInstance.RenderTimePaused;
					}
				}
				else
				{
					enable = false;
				}
			}
			else
			{
				enable = true;
			}
			bool flag2 = text3 != null;
			if (flag2)
			{
				this._gameInstance.Chat.Log("Available render passes:\n" + text3);
			}
			else
			{
				text = args[1].ToLower();
				bool flag3 = text == "all";
				if (flag3)
				{
					for (int i = 0; i < this._gameInstance.RenderPassNames.Length; i++)
					{
						this._gameInstance.SetRenderPassEnabled((uint)i, enable);
					}
				}
				else
				{
					bool flag4 = Enumerable.Contains<string>(this._gameInstance.RenderPassNames, text);
					if (flag4)
					{
						int passId = Array.FindIndex<string>(this._gameInstance.RenderPassNames, (string item) => item == text);
						this._gameInstance.SetRenderPassEnabled((uint)passId, enable);
					}
					else
					{
						this._gameInstance.Chat.Log("Unknown render pass name");
					}
				}
			}
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x000E26A0 File Offset: 0x000E08A0
		[Usage("parallel", new string[]
		{
			"on|off",
			"light_on|light_off",
			"particle_on|particle_off",
			"anim_on|anim_off"
		})]
		private void ParallelSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 1630810064U)
			{
				if (num <= 1220951161U)
				{
					if (num != 902478281U)
					{
						if (num == 1220951161U)
						{
							if (text2 == "light_on")
							{
								this._gameInstance.SceneRenderer.ClusteredLighting.UseParallelExecution(true);
								goto IL_298;
							}
						}
					}
					else if (text2 == "particle_on")
					{
						this._gameInstance.Engine.FXSystem.Particles.UseParallelExecution(true);
						goto IL_298;
					}
				}
				else if (num != 1295367429U)
				{
					if (num == 1630810064U)
					{
						if (text2 == "on")
						{
							this._gameInstance.SceneRenderer.ClusteredLighting.UseParallelExecution(true);
							this._gameInstance.Engine.FXSystem.Particles.UseParallelExecution(true);
							this._gameInstance.Engine.AnimationSystem.UseParallelExecution(true);
							goto IL_298;
						}
					}
				}
				else if (text2 == "particle_off")
				{
					this._gameInstance.Engine.FXSystem.Particles.UseParallelExecution(false);
					goto IL_298;
				}
			}
			else if (num <= 2670044757U)
			{
				if (num != 2051696968U)
				{
					if (num == 2670044757U)
					{
						if (text2 == "light_off")
						{
							this._gameInstance.SceneRenderer.ClusteredLighting.UseParallelExecution(false);
							goto IL_298;
						}
					}
				}
				else if (text2 == "anim_on")
				{
					this._gameInstance.Engine.AnimationSystem.UseParallelExecution(true);
					goto IL_298;
				}
			}
			else if (num != 2872740362U)
			{
				if (num == 3239060706U)
				{
					if (text2 == "anim_off")
					{
						this._gameInstance.Engine.AnimationSystem.UseParallelExecution(false);
						goto IL_298;
					}
				}
			}
			else if (text2 == "off")
			{
				this._gameInstance.SceneRenderer.ClusteredLighting.UseParallelExecution(false);
				this._gameInstance.Engine.FXSystem.Particles.UseParallelExecution(false);
				this._gameInstance.Engine.AnimationSystem.UseParallelExecution(false);
				goto IL_298;
			}
			throw new InvalidCommandUsage();
			IL_298:
			this._gameInstance.Chat.Log("Parallel state : " + args[0]);
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x000E2964 File Offset: 0x000E0B64
		[Usage("test", new string[]
		{
			"[on|off]"
		})]
		private void TestSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "on"))
			{
				if (!(a == "off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.TestBranch = false;
			}
			else
			{
				this._gameInstance.TestBranch = true;
			}
			this._gameInstance.Chat.Log("Test state : " + args[0]);
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x000E29F0 File Offset: 0x000E0BF0
		[Usage("graphics", new string[]
		{
			"[mode_ingame|mode_cutscene|mode_trailer|mode_slowgpu]"
		})]
		private void GraphicsSetup(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "mode_slowgpu"))
			{
				if (!(a == "mode_ingame"))
				{
					if (!(a == "mode_cutscene"))
					{
						if (!(a == "mode_trailer"))
						{
							throw new InvalidCommandUsage();
						}
						this._gameInstance.SetRenderingOptions(ref this._gameInstance.TrailerMode);
					}
					else
					{
						this._gameInstance.SetRenderingOptions(ref this._gameInstance.CutscenesMode);
					}
				}
				else
				{
					this._gameInstance.SetRenderingOptions(ref this._gameInstance.IngameMode);
				}
			}
			else
			{
				this._gameInstance.SetRenderingOptions(ref this._gameInstance.LowEndGPUMode);
			}
			this._gameInstance.Chat.Log("graphics state : " + args[0]);
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x000E2ADC File Offset: 0x000E0CDC
		[Usage("packetstats", new string[]
		{
			"[reset]"
		})]
		private void PacketStats(string[] args)
		{
			ConnectionToServer.PacketStat[] packetStats = this._gameInstance.Connection.PacketStats;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Packets Sent:");
			for (int i = 0; i < packetStats.Length; i++)
			{
				ConnectionToServer.PacketStat packetStat = packetStats[i];
				long sentCount = packetStat.SentCount;
				bool flag = sentCount <= 0L;
				if (!flag)
				{
					long sentTotalSize = packetStat.SentTotalSize;
					stringBuilder.AppendLine(string.Format("\t{0} ({1})\n\t{2} packets, Size: {3} bytes, Avg: {4} bytes", new object[]
					{
						packetStat.Name,
						i,
						sentCount,
						sentTotalSize,
						sentTotalSize / sentCount
					}));
				}
			}
			stringBuilder.AppendLine("\nPackets Received:");
			for (int j = 0; j < packetStats.Length; j++)
			{
				ConnectionToServer.PacketStat packetStat2 = packetStats[j];
				long receivedCount = packetStat2.ReceivedCount;
				bool flag2 = receivedCount <= 0L;
				if (!flag2)
				{
					long receivedTotalElapsed = packetStat2.ReceivedTotalElapsed;
					stringBuilder.AppendLine(string.Format("\t{0} ({1})\n\t\t{2} packets, Time: {3}, Avg: {4}", new object[]
					{
						packetStat2.Name,
						j,
						receivedCount,
						TimeHelper.FormatTicks(receivedTotalElapsed),
						TimeHelper.FormatTicks(receivedTotalElapsed / receivedCount)
					}));
				}
			}
			string message = stringBuilder.ToString();
			DebugCommandsModule.Logger.Info(message);
			bool flag3 = args.Length == 1 && args[0] == "reset";
			if (flag3)
			{
				this._gameInstance.Connection.ResetPacketStats();
				this._gameInstance.Chat.Log("Reset packet Stats (Logged old stats to console)");
			}
			else
			{
				this._gameInstance.Chat.Log(message);
			}
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x000E2CB0 File Offset: 0x000E0EB0
		[Usage("heartbeatsettings", new string[]
		{
			"[healthAlertThreshold|minAlphaHealthBorder|maxAlphaHealthBorder|minVariance|maxVariance|lerpSpeed|resetSpeedHealthBorder] <value>"
		})]
		private void UpdateHeartbeatSettings(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				string message = string.Concat(new string[]
				{
					"Settings: \n",
					string.Format(" - healthAlertThreshold: {0}\n", this._gameInstance.DamageEffectModule.HealthAlertThreshold),
					string.Format(" - minAlphaHealthBorder: {0}\n", this._gameInstance.DamageEffectModule.MinAlphaHealthBorder),
					string.Format(" - maxAlphaHealthBorder: {0}\n", this._gameInstance.DamageEffectModule.MaxAlphaHealthBorder),
					string.Format(" - minVariance: {0}\n", this._gameInstance.DamageEffectModule.MinVarianceHealthBorder),
					string.Format(" - maxVariance: {0}\n", this._gameInstance.DamageEffectModule.MaxVarianceHealthBorder),
					string.Format(" - lerpSpeed: {0}\n", this._gameInstance.DamageEffectModule.LerpSpeedHealthBorder),
					string.Format(" - resetSpeedHealthBorder: {0}\n", this._gameInstance.DamageEffectModule.ResetSpeedHealthBorder)
				});
				this._gameInstance.Chat.Log(message);
			}
			else
			{
				string text = args[0];
				float num = 0f;
				bool flag2 = args.Length > 1 && args[0] != "easing" && !float.TryParse(args[1], out num);
				if (flag2)
				{
					this._gameInstance.Chat.Log(args[1] + " is not a valid number");
					throw new InvalidCommandUsage();
				}
				string text2 = text;
				string a = text2;
				if (!(a == "minAlphaHealthBorder"))
				{
					if (!(a == "maxAlphaHealthBorder"))
					{
						if (!(a == "minVariance"))
						{
							if (!(a == "maxVariance"))
							{
								if (!(a == "lerpSpeed"))
								{
									if (!(a == "resetSpeedHealthBorder"))
									{
										throw new InvalidCommandUsage();
									}
									this._gameInstance.DamageEffectModule.ResetSpeedHealthBorder = num;
								}
								else
								{
									this._gameInstance.DamageEffectModule.LerpSpeedHealthBorder = num;
								}
							}
							else
							{
								this._gameInstance.DamageEffectModule.MaxVarianceHealthBorder = num;
							}
						}
						else
						{
							this._gameInstance.DamageEffectModule.MinVarianceHealthBorder = num;
						}
					}
					else
					{
						this._gameInstance.DamageEffectModule.MaxAlphaHealthBorder = num;
					}
				}
				else
				{
					this._gameInstance.DamageEffectModule.MinAlphaHealthBorder = num;
				}
				this._gameInstance.Chat.Log(string.Format("{0} changed to: {1}", text, num));
			}
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x000E2F38 File Offset: 0x000E1138
		[Usage("hitdetection", new string[]
		{
			""
		})]
		private void HitDetection(string[] args)
		{
			this._gameInstance.InteractionModule.ShowSelectorDebug = !this._gameInstance.InteractionModule.ShowSelectorDebug;
			this._gameInstance.InteractionModule.SelectorDebugMeshes.Clear();
			this._gameInstance.Chat.Log(string.Format("Toggled hitdetection preview : {0}", this._gameInstance.InteractionModule.ShowSelectorDebug));
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x000E2FB0 File Offset: 0x000E11B0
		private void RenderPlayers(string[] args)
		{
			this._gameInstance.RenderPlayers = !this._gameInstance.RenderPlayers;
			this._gameInstance.Chat.Log(string.Format("Toggled player rendering : {0}", this._gameInstance.RenderPlayers));
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x000E3004 File Offset: 0x000E1204
		[Usage("blockpreview", new string[]
		{
			"[dither_on|dither_off]"
		})]
		private void BlockPreview(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "dither_on"))
			{
				if (!(a == "dither_off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.InteractionModule.BlockPreview.EnableDithering(false);
			}
			else
			{
				this._gameInstance.InteractionModule.BlockPreview.EnableDithering(true);
			}
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x000E3084 File Offset: 0x000E1284
		[Usage("buildertool", new string[]
		{
			"[highlight_on|highlight_off]"
		})]
		private void BuilderTool(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0].ToLower();
			string a = text;
			if (!(a == "highlight_on"))
			{
				if (!(a == "highlight_off"))
				{
					throw new InvalidCommandUsage();
				}
				this._gameInstance.BuilderToolsModule.DrawHighlightAndUndergroundColor = false;
			}
			else
			{
				this._gameInstance.BuilderToolsModule.DrawHighlightAndUndergroundColor = true;
			}
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x000E30F8 File Offset: 0x000E12F8
		private void ForceTint(string[] args)
		{
			bool flag = args.Length < 3;
			if (flag)
			{
				ChunkGeometryBuilder.ForceTint = ChunkGeometryBuilder.NoTint;
			}
			else
			{
				ChunkGeometryBuilder.ForceTint = new ShortVector3((short)int.Parse(args[0]), (short)int.Parse(args[1]), (short)int.Parse(args[2]));
			}
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x000E3145 File Offset: 0x000E1345
		private void StressTestBatcher(string[] args)
		{
			this._gameInstance.App.Interface.InGameView.UpdateDebugStressVisibility();
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x000E3163 File Offset: 0x000E1363
		private void LogDisposableSummaryCommand(string[] args)
		{
			Disposable.LogSummary(true);
			Disposable.LogSummary(false);
		}

		// Token: 0x04002177 RID: 8567
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002178 RID: 8568
		private const int RowBlockCount = 32;

		// Token: 0x04002179 RID: 8569
		private const int RowGap = 2;

		// Token: 0x0400217A RID: 8570
		private const int ColumnGap = 3;

		// Token: 0x0400217B RID: 8571
		private const int MaxRows = 32;

		// Token: 0x0400217C RID: 8572
		private const int EdgePadding = 3;
	}
}
