using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Coherent.UI.Binding;
using HytaleClient.Application;
using HytaleClient.Core;
using HytaleClient.Data.Items;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Commands;
using HytaleClient.InGame.Modules.BuilderTools;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Client;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Events;
using HytaleClient.InGame.Modules.Machinima.Settings;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.InGame.Modules.Machinima.TrackPath;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.InGame.Modules.Machinima
{
	// Token: 0x0200090C RID: 2316
	internal class MachinimaModule : Module
	{
		// Token: 0x060045F2 RID: 17906 RVA: 0x00100E80 File Offset: 0x000FF080
		private void RegisterCommands()
		{
			this._gameInstance.RegisterCommand("mach", new GameInstance.Command(this.MachinimaCommand));
			this._subCommands.Add("scene", new GameInstance.Command(this.SceneCommand));
			this._subCommands.Add("actor", new GameInstance.Command(this.ActorCommand));
			this._subCommands.Add("event", new GameInstance.Command(this.EventCommand));
			this._subCommands.Add("fps", new GameInstance.Command(this.FpsCommand));
			this._subCommands.Add("clear", new GameInstance.Command(this.ClearCommand));
			this._subCommands.Add("display", new GameInstance.Command(this.DisplayCommand));
			this._subCommands.Add("restart", new GameInstance.Command(this.RestartCommand));
			this._subCommands.Add("autorestart", new GameInstance.Command(this.AutorestartCommand));
			this._subCommands.Add("pause", new GameInstance.Command(this.PauseCommand));
			this._subCommands.Add("files", new GameInstance.Command(this.FilesCommand));
			this._subCommands.Add("tool", new GameInstance.Command(this.ToolCommand));
			this._subCommands.Add("key", new GameInstance.Command(this.KeyCommand));
			this._subCommands.Add("save", new GameInstance.Command(this.SaveCommand));
			this._subCommands.Add("load", new GameInstance.Command(this.LoadCommand));
			this._subCommands.Add("update", new GameInstance.Command(this.UpdateCommand));
			this._subCommands.Add("edit", new GameInstance.Command(this.EditCommand));
			this._subCommands.Add("speed", new GameInstance.Command(this.SpeedCommand));
			this._subCommands.Add("bezier", new GameInstance.Command(this.BezierCommand));
			this._subCommands.Add("spline", new GameInstance.Command(this.SplineCommand));
			this._subCommands.Add("modeldebug", new GameInstance.Command(this.ModelDebugCommand));
			this._subCommands.Add("demo", new GameInstance.Command(this.DemoCommand));
			this._subCommands.Add("align", new GameInstance.Command(this.AlignCommand));
			this._subCommands.Add("offset", new GameInstance.Command(this.OffsetCommand));
			this._subCommands.Add("fixlook", new GameInstance.Command(this.FixLookCommand));
			this._subCommands.Add("camera", new GameInstance.Command(this.CameraCommand));
			this._subCommands.Add("entitylight", new GameInstance.Command(this.EntityLightCommand));
			this._subCommands.Add("zip", new GameInstance.Command(this.ZipCommand));
			this._subCommands.Add("autosave", new GameInstance.Command(this.AutosaveCommand));
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x001011D8 File Offset: 0x000FF3D8
		private string GetCommandList()
		{
			string text = "Mach Sub Commands: [";
			foreach (KeyValuePair<string, GameInstance.Command> keyValuePair in this._subCommands)
			{
				text = text + keyValuePair.Key + ", ";
			}
			return text.Substring(0, text.Length - 2) + "]";
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x00101260 File Offset: 0x000FF460
		[Usage("mach", new string[]
		{

		})]
		[Description("Machinima commands")]
		public void MachinimaCommand(string[] args)
		{
			bool flag = args.Length == 0;
			if (flag)
			{
				this._gameInstance.Chat.Log("Please enter a mach sub command.");
				this._gameInstance.Chat.Log(this.GetCommandList());
			}
			else
			{
				GameInstance.Command command;
				this._subCommands.TryGetValue(args[0], out command);
				bool flag2 = command == null;
				if (flag2)
				{
					this._gameInstance.Chat.Log("Unknown mach sub command! '" + args[0] + "'");
					this._gameInstance.Chat.Log(this.GetCommandList());
				}
				else
				{
					command(args);
				}
			}
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00101308 File Offset: 0x000FF508
		[Usage("mach scene", new string[]
		{
			"list",
			"clear",
			"add [name]",
			"copy",
			"save [name]",
			"load [name]",
			"rotate",
			"modelupdate",
			"remove [name]",
			"set [name]",
			"rename [name]"
		})]
		private void SceneCommand(string[] args)
		{
			bool flag = args.Length < 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[1];
			string text2 = text;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 2180167635U)
			{
				if (num <= 783778355U)
				{
					if (num != 217798785U)
					{
						if (num == 783778355U)
						{
							if (text2 == "modelupdate")
							{
								bool flag2 = this.ActiveScene == null;
								if (flag2)
								{
									this._gameInstance.Chat.Log("No active scene found");
								}
								else
								{
									for (int i = 0; i < this.ActiveScene.Actors.Count; i++)
									{
										EntityActor entityActor = this.ActiveScene.Actors[i] as EntityActor;
										bool flag3 = entityActor != null;
										if (flag3)
										{
											entityActor.UpdateModel(this._gameInstance, null);
										}
									}
									this._gameInstance.Chat.Log("Scene models updated");
								}
								return;
							}
						}
					}
					else if (text2 == "list")
					{
						this.ListScenes();
						return;
					}
				}
				else if (num != 993596020U)
				{
					if (num != 1550717474U)
					{
						if (num == 2180167635U)
						{
							if (text2 == "rename")
							{
								bool flag4 = args.Length < 3;
								if (flag4)
								{
									throw new InvalidCommandUsage();
								}
								MachinimaScene activeScene = this.ActiveScene;
								this._scenes.Remove(activeScene.Name);
								activeScene.Name = args[2];
								this._scenes.Add(activeScene.Name, activeScene);
								this.SetActiveScene(args[2]);
								this._gameInstance.Chat.Log("Active scene set to '" + args[2] + "'");
								return;
							}
						}
					}
					else if (text2 == "clear")
					{
						this.ClearScenes();
						return;
					}
				}
				else if (text2 == "add")
				{
					string text3 = (args.Length < 3) ? "" : args[2];
					text3 = this.GetNextSceneName(string.IsNullOrWhiteSpace(text3) ? "scene" : text3);
					MachinimaScene scene = new MachinimaScene(this._gameInstance, text3);
					bool flag5 = !this.AddScene(scene, true);
					if (flag5)
					{
						this._gameInstance.Chat.Log("A scene already exists with the name '" + text3 + "'");
					}
					else
					{
						this._gameInstance.Chat.Log("Added new scene '" + text3 + "'");
					}
					return;
				}
			}
			else if (num <= 3439296072U)
			{
				if (num != 2784296202U)
				{
					if (num != 3324446467U)
					{
						if (num == 3439296072U)
						{
							if (text2 == "save")
							{
								string text4 = (args.Length > 2 && args[2] != "json") ? args[2] : this.ActiveScene.Name;
								bool flag6 = string.IsNullOrEmpty(text4) || !this._scenes.ContainsKey(text4);
								if (flag6)
								{
									bool flag7 = args.Length < 3;
									if (flag7)
									{
										this._gameInstance.Chat.Log("No active scene found, please specify a scene name, or set one to active");
									}
									else
									{
										this._gameInstance.Chat.Log("Unable to find scene '" + args[2] + "'");
									}
									return;
								}
								SceneDataType dataType = this._settings.CompressSaveFiles ? SceneDataType.CompressedFile : SceneDataType.JSONFile;
								bool flag8 = args.Length > 2;
								if (flag8)
								{
									bool flag9 = args[2] == "json";
									if (flag9)
									{
										dataType = SceneDataType.JSONFile;
									}
									else
									{
										bool flag10 = args[2] == "hms";
										if (flag10)
										{
											dataType = SceneDataType.CompressedFile;
										}
									}
								}
								MachinimaScene scene2 = this._scenes[text4];
								this.SaveSceneFile(scene2, dataType, null, "");
								this._gameInstance.Chat.Log("'" + text4 + "' scene successfully saved to file");
								return;
							}
						}
					}
					else if (text2 == "set")
					{
						bool flag11 = args.Length < 3;
						if (flag11)
						{
							throw new InvalidCommandUsage();
						}
						bool flag12 = !this._scenes.ContainsKey(args[2]);
						if (flag12)
						{
							this._gameInstance.Chat.Log("Unable to find scene '" + args[2] + "'");
						}
						else
						{
							this.SetActiveScene(args[2]);
							this._gameInstance.Chat.Log("Active scene set to '" + args[2] + "'");
						}
						return;
					}
				}
				else if (text2 == "rotate")
				{
					this._gameInstance.Chat.Log("Please left click an actor keyframe to rotate around, or right click to cancel");
					this._onUseToolItem = delegate(InteractionType actionType)
					{
						bool flag19 = actionType == 1;
						bool result;
						if (flag19)
						{
							this._gameInstance.Chat.Log("Rotate stopped");
							result = false;
						}
						else
						{
							bool flag20 = this.HoveredKeyframe != null;
							Vector3 rotatePosition;
							if (flag20)
							{
								this.ActiveActor = this.HoveredActor;
								rotatePosition = this.HoveredKeyframe.GetSetting<Vector3>("Position").Value;
							}
							else
							{
								rotatePosition = this.ActiveScene.Origin;
							}
							Vector3 currentRotation = Vector3.Zero;
							this._rotationGizmo.Show(rotatePosition, new Vector3?(currentRotation), delegate(Vector3 newRotation)
							{
								Vector3 rotation = newRotation - currentRotation;
								this.ActiveScene.Rotate(rotation, rotatePosition);
								currentRotation = newRotation;
								this.UpdateFrame(0L, true);
							}, null);
							this.EditMode = MachinimaModule.EditorMode.RotateBody;
							result = false;
						}
						return result;
					};
					return;
				}
			}
			else if (num != 3683784189U)
			{
				if (num != 3848464964U)
				{
					if (num == 3859241449U)
					{
						if (text2 == "load")
						{
							bool flag13 = args.Length < 3;
							MachinimaScene machinimaScene;
							if (flag13)
							{
								machinimaScene = this.LoadSceneFile("clipboard", true, SceneDataType.Clipboard);
							}
							else
							{
								machinimaScene = this.LoadSceneFile(Path.Combine(MachinimaModule.SceneDirectory, args[2]), true, SceneDataType.CompressedFile);
							}
							bool flag14 = machinimaScene != null;
							if (flag14)
							{
								this._gameInstance.Chat.Log("Loaded scene '" + machinimaScene.Name + "'");
								this.SetActiveScene(machinimaScene.Name);
							}
							return;
						}
					}
				}
				else if (text2 == "copy")
				{
					string text5 = (args.Length > 2 && args[2] != "json") ? args[2] : this.ActiveScene.Name;
					bool flag15 = string.IsNullOrEmpty(text5) || !this._scenes.ContainsKey(text5);
					if (flag15)
					{
						bool flag16 = args.Length < 3;
						if (flag16)
						{
							this._gameInstance.Chat.Log("No active scene found, please specify a scene name, or set one to active");
						}
						else
						{
							this._gameInstance.Chat.Log("Unable to find scene '" + args[2] + "'");
						}
						return;
					}
					MachinimaScene scene3 = this._scenes[text5];
					this.SaveSceneFile(scene3, SceneDataType.Clipboard, null, "");
					this._gameInstance.Chat.Log("'" + text5 + "' scene successfully saved to the clipboard!");
					return;
				}
			}
			else if (text2 == "remove")
			{
				bool flag17 = args.Length < 3;
				if (flag17)
				{
					throw new InvalidCommandUsage();
				}
				bool flag18 = !this.RemoveScene(args[2]);
				if (flag18)
				{
					this._gameInstance.Chat.Log("Unable to find scene '" + args[2] + "'");
				}
				else
				{
					this._gameInstance.Chat.Log("Removed scene '" + args[2] + "'");
				}
				return;
			}
			throw new InvalidCommandUsage();
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00101A40 File Offset: 0x000FFC40
		[Usage("mach actor", new string[]
		{
			"list",
			"clear",
			"model",
			"move",
			"clone",
			"rotate",
			"modelupdate",
			"add [name]",
			"remove [name]",
			"set [name]"
		})]
		private void ActorCommand(string[] args)
		{
			bool flag = args.Length < 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			bool flag2 = this.ActiveScene == null;
			if (!flag2)
			{
				string text = args[1];
				string text2 = text;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
				if (num <= 993596020U)
				{
					if (num <= 407568404U)
					{
						if (num != 217798785U)
						{
							if (num != 407568404U)
							{
								goto IL_781;
							}
							if (!(text2 == "move"))
							{
								goto IL_781;
							}
						}
						else
						{
							if (!(text2 == "list"))
							{
								goto IL_781;
							}
							this.ActiveScene.ListActors();
							return;
						}
					}
					else if (num != 730356610U)
					{
						if (num != 783778355U)
						{
							if (num != 993596020U)
							{
								goto IL_781;
							}
							if (!(text2 == "add"))
							{
								goto IL_781;
							}
							bool flag3 = args.Length < 3;
							if (flag3)
							{
								throw new InvalidCommandUsage();
							}
							string text3 = args[2];
							string text4 = (args.Length > 3) ? args[3] : "";
							text4 = this.ActiveScene.GetNextActorName(string.IsNullOrWhiteSpace(text4) ? text3 : text4);
							bool flag4 = this.ActiveScene.GetActor(text4) != null;
							if (flag4)
							{
								this._gameInstance.Chat.Log(string.Concat(new string[]
								{
									"An actor already exists with the name '",
									text4,
									"' in scene '",
									this.ActiveScene.Name,
									"'"
								}));
								return;
							}
							string text5 = text3;
							string a = text5;
							SceneActor sceneActor;
							if (!(a == "player"))
							{
								if (!(a == "entity"))
								{
									if (!(a == "camera"))
									{
										if (!(a == "ref"))
										{
											this._gameInstance.Chat.Log("Invalid actor type '" + text3 + "', acceptable types are player, entity and camera.");
											return;
										}
										sceneActor = new ReferenceActor(this._gameInstance, text4);
									}
									else
									{
										sceneActor = new CameraActor(this._gameInstance, text4);
									}
								}
								else
								{
									sceneActor = new EntityActor(this._gameInstance, text4, null);
									((EntityActor)sceneActor).SetBaseModel(this._gameInstance.LocalPlayer.ModelPacket);
								}
							}
							else
							{
								sceneActor = new PlayerActor(this._gameInstance, text4);
							}
							bool flag5 = !this.ActiveScene.AddActor(sceneActor, true);
							if (flag5)
							{
								this._gameInstance.Chat.Log(string.Concat(new string[]
								{
									"Error adding '",
									text4,
									"' to scene '",
									this.ActiveScene.Name,
									"'"
								}));
								sceneActor.Dispose();
								return;
							}
							bool flag6 = sceneActor is CameraActor;
							if (flag6)
							{
								sceneActor.Track.Keyframes[0].AddEvent(new CameraEvent(true));
							}
							this.ActiveActor = sceneActor;
							this._gameInstance.Chat.Log(string.Concat(new string[]
							{
								"New actor '",
								text4,
								"' added to scene '",
								this.ActiveScene.Name,
								"'"
							}));
							return;
						}
						else
						{
							if (!(text2 == "modelupdate"))
							{
								goto IL_781;
							}
							bool flag7 = this.ActiveActor == null;
							if (flag7)
							{
								this._gameInstance.Chat.Log("No active actor found");
							}
							else
							{
								bool flag8 = this.ActiveActor is EntityActor;
								if (flag8)
								{
									(this.ActiveActor as EntityActor).UpdateModel(this._gameInstance, null);
									this._gameInstance.Chat.Log("Entity model updated");
								}
								else
								{
									this._gameInstance.Chat.Log("Unable to update model, actor is not an Entity");
								}
							}
							return;
						}
					}
					else if (!(text2 == "clone"))
					{
						goto IL_781;
					}
					bool isMoving = args[1] == "move";
					this._gameInstance.Chat.Log("Please select an actor keyframe to " + (isMoving ? "move" : "copy from"));
					Func<SceneActor, Vector3, SceneActor> cloneActor = delegate(SceneActor actor, Vector3 position)
					{
						Vector3 value = actor.Track.GetStartingPosition() - position;
						SceneActor sceneActor2 = actor.Clone(this._gameInstance);
						sceneActor2.Name = this.ActiveScene.GetNextActorName("clone");
						sceneActor2.Track.OffsetPositions(-value);
						sceneActor2.Track.UpdateKeyframeData();
						return sceneActor2;
					};
					this._onUseToolItem = delegate(InteractionType actionType)
					{
						TrackKeyframe hoveredKeyframe = this.HoveredKeyframe;
						bool flag17 = hoveredKeyframe == null;
						if (flag17)
						{
							this._gameInstance.Chat.Log("Unable to find a keyframe in this location, aborting...");
						}
						else
						{
							SceneActor selectedActor = this.HoveredActor;
							this.ActiveActor = selectedActor;
							this._gameInstance.Chat.Log("Now select an offset position to " + (isMoving ? "move to" : "add"));
							Vector3 keyframeOffset = this.HoveredKeyframe.GetSetting<Vector3>("Position").Value - this.HoveredActor.Track.GetStartingPosition();
							this._onUseToolItem = delegate(InteractionType actionTypeSub)
							{
								bool flag18 = actionTypeSub == 1;
								bool result;
								if (flag18)
								{
									bool isMoving;
									this._gameInstance.Chat.Log(isMoving ? "Move cancelled" : "Clone stopped");
									result = false;
								}
								else
								{
									Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
									bool flag19 = !this._gameInstance.InteractionModule.HasFoundTargetBlock;
									if (flag19)
									{
										this._gameInstance.Chat.Log("Invalid position, aborting...");
										result = false;
									}
									else
									{
										Vector3 vector = this._gameInstance.InteractionModule.TargetBlockHit.HitPosition - keyframeOffset;
										bool isMoving = isMoving;
										if (isMoving)
										{
											Vector3 value = this.ActiveActor.Track.GetStartingPosition() - vector;
											this.ActiveActor.Track.OffsetPositions(-value);
											this._gameInstance.Chat.Log("Actor offset complete");
											result = false;
										}
										else
										{
											SceneActor sceneActor2 = cloneActor(selectedActor, vector);
											this.ActiveScene.AddActor(sceneActor2, false);
											this.ActiveActor = sceneActor2;
											selectedActor = sceneActor2;
											this._gameInstance.Chat.Log("Clone Finished!");
											this._gameInstance.Chat.Log("Continue Cloning with left click, or right to finish");
											result = true;
										}
									}
								}
								return result;
							};
						}
						return false;
					};
					return;
				}
				if (num <= 2190941297U)
				{
					if (num != 1550717474U)
					{
						if (num != 1890226832U)
						{
							if (num == 2190941297U)
							{
								if (text2 == "scale")
								{
									bool flag9 = args.Length < 3;
									if (flag9)
									{
										throw new InvalidCommandUsage();
									}
									bool flag10 = this.ActiveActor == null;
									if (flag10)
									{
										this._gameInstance.Chat.Log("No active actor found");
										return;
									}
									EntityActor entityActor = this.ActiveActor as EntityActor;
									bool flag11 = entityActor != null;
									if (flag11)
									{
										float num2;
										bool flag12 = float.TryParse(args[2], out num2);
										if (flag12)
										{
											entityActor.SetScale(num2);
											this._gameInstance.Chat.Log(string.Format("Actor scale set to {0}", num2));
										}
										else
										{
											this._gameInstance.Chat.Log("Unable to parse float value from: `" + args[2] + "`");
										}
									}
									else
									{
										this._gameInstance.Chat.Log("Active actor must be an entiy type to use this.");
									}
									return;
								}
							}
						}
						else if (text2 == "rotate:")
						{
							this._gameInstance.Chat.Log("Please left click an actor keyframe to rotate around, or right click to cancel");
							this._onUseToolItem = delegate(InteractionType actionType)
							{
								bool flag17 = actionType == 1;
								bool result;
								if (flag17)
								{
									this._gameInstance.Chat.Log("Rotate stopped");
									result = false;
								}
								else
								{
									bool flag18 = this.HoveredKeyframe != null;
									Vector3 rotateCenter;
									if (flag18)
									{
										this.ActiveActor = this.HoveredActor;
										rotateCenter = this.HoveredKeyframe.GetSetting<Vector3>("Position").Value;
									}
									else
									{
										bool flag19 = this.ActiveActor == null;
										if (flag19)
										{
											this._gameInstance.Chat.Log("No active actor found, select one and try again");
											return false;
										}
										rotateCenter = this.ActiveActor.Track.GetStartingPosition();
									}
									Vector3 currentRotation = Vector3.Zero;
									this._rotationGizmo.Show(rotateCenter, new Vector3?(currentRotation), delegate(Vector3 newRotation)
									{
										Vector3 rotation = newRotation - currentRotation;
										this.ActiveActor.Track.RotatePath(rotation, rotateCenter);
										currentRotation = newRotation;
										this.UpdateFrame(0L, true);
									}, null);
									this.EditMode = MachinimaModule.EditorMode.RotateBody;
									result = false;
								}
								return result;
							};
							return;
						}
					}
					else if (text2 == "clear")
					{
						this.ActiveScene.ClearActors();
						return;
					}
				}
				else if (num != 2961925722U)
				{
					if (num != 3324446467U)
					{
						if (num == 3683784189U)
						{
							if (text2 == "remove")
							{
								bool flag13 = args.Length < 3;
								if (flag13)
								{
									throw new InvalidCommandUsage();
								}
								bool flag14 = !this.ActiveScene.RemoveActor(args[2]);
								if (flag14)
								{
									this._gameInstance.Chat.Log("Unable to find actor: " + args[2]);
								}
								else
								{
									this._gameInstance.Chat.Log("Removed actor: " + args[2]);
								}
								return;
							}
						}
					}
					else if (text2 == "set")
					{
						bool flag15 = args.Length < 3;
						if (flag15)
						{
							throw new InvalidCommandUsage();
						}
						bool flag16 = !this.ActiveScene.HasActor(args[2]);
						if (flag16)
						{
							this._gameInstance.Chat.Log("Unable to find actor: " + args[2]);
						}
						else
						{
							this.ActiveActor = this.ActiveScene.GetActor(args[2]);
						}
						return;
					}
				}
				else if (text2 == "model")
				{
					this._gameInstance.Chat.Log("Please select an entity to copy the model from.");
					this._onUseToolItem = delegate(InteractionType actionType)
					{
						Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
						bool flag17;
						HitDetection.RaycastHit raycastHit;
						bool flag18;
						HitDetection.EntityHitData entityHitData;
						this._gameInstance.HitDetection.Raycast(lookRay.Position, lookRay.Direction, this._toolRaycastOptions, out flag17, out raycastHit, out flag18, out entityHitData);
						bool flag19 = !flag18;
						if (flag19)
						{
							this._gameInstance.Chat.Log("Unable to find an entity in this location, aborting...");
						}
						else
						{
							Entity entity = entityHitData.Entity;
							Model selectedEntityModel = entity.ModelPacket;
							this._gameInstance.Chat.Log("Entity selected!");
							this._gameInstance.Chat.Log("Now please select the the EntityActor to apply the model to.");
							this._onUseToolItem = delegate(InteractionType actionTypeSub)
							{
								Ray lookRay2 = this._gameInstance.CameraModule.GetLookRay();
								bool flag20;
								HitDetection.RaycastHit raycastHit2;
								bool flag21;
								HitDetection.EntityHitData entityHitData2;
								this._gameInstance.HitDetection.Raycast(lookRay2.Position, lookRay2.Direction, this._toolRaycastOptions, out flag20, out raycastHit2, out flag21, out entityHitData2);
								bool flag22 = !flag21;
								bool result;
								if (flag22)
								{
									this._gameInstance.Chat.Log("Unable to find an Entity in that location, aborting...");
									result = false;
								}
								else
								{
									Entity entity2 = entityHitData2.Entity;
									EntityActor actorFromEntity = this.GetActorFromEntity(entity2);
									bool flag23 = actorFromEntity == null;
									if (flag23)
									{
										this._gameInstance.Chat.Log("Unable to find an Actor for that Entity, aborting...");
									}
									else
									{
										bool flag24 = !(actorFromEntity is PlayerActor);
										if (flag24)
										{
											actorFromEntity.SetBaseModel(selectedEntityModel);
											this._gameInstance.Chat.Log("Model successfully applied to EntityActor '" + actorFromEntity.Name + "'");
										}
										else
										{
											this._gameInstance.Chat.Log("Invalid actor type found, aborting...");
										}
									}
									result = false;
								}
								return result;
							};
						}
						return false;
					};
					return;
				}
				IL_781:
				throw new InvalidCommandUsage();
			}
			this._gameInstance.Chat.Log("No active scene found");
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x001021D8 File Offset: 0x001003D8
		[Usage("mach event", new string[]
		{
			"list",
			"add target",
			"add animation [id]",
			"add command [text]",
			"add camera [on|off]",
			"add particle [id]",
			"remove [id]"
		})]
		private void EventCommand(string[] args)
		{
			bool flag = args.Length < 2;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			bool flag2 = this.ActiveScene == null;
			if (flag2)
			{
				this._gameInstance.Chat.Log("No active scene found");
			}
			bool flag3 = this.ActiveActor == null;
			if (flag3)
			{
				this._gameInstance.Chat.Log("No active actor found in the scene");
			}
			string text = args[1];
			string a = text;
			if (!(a == "list"))
			{
				if (!(a == "add"))
				{
					if (!(a == "remove"))
					{
						throw new InvalidCommandUsage();
					}
					bool flag4 = args.Length < 3;
					if (flag4)
					{
						this._gameInstance.Chat.Log("Plese enter the keyframe event id # to remove");
					}
					else
					{
						int num;
						bool flag5 = !int.TryParse(args[2], out num);
						if (flag5)
						{
							this._gameInstance.Chat.Log("Unable to parse a keyframe id from '" + args[2] + "'");
						}
						else
						{
							TrackKeyframe eventKeyframe = this.ActiveScene.GetEventKeyframe(num);
							bool flag6 = eventKeyframe == null;
							if (flag6)
							{
								this._gameInstance.Chat.Log(string.Format("Unable to find event with id '{0}'", num));
							}
							else
							{
								eventKeyframe.RemoveEvent(num);
								this._gameInstance.Chat.Log("Event succesfully removed from keyframe");
							}
						}
					}
				}
				else
				{
					bool flag7 = args[2] == "target";
					HytaleClient.InGame.Modules.Machinima.Events.KeyframeEvent newEvent;
					if (flag7)
					{
						newEvent = new TargetEvent(null);
					}
					else
					{
						bool flag8 = args.Length < 4;
						if (flag8)
						{
							throw new InvalidCommandUsage();
						}
						bool flag9 = args[2] == "animation";
						if (flag9)
						{
							string text2 = args[3].Trim();
							AnimationSlot slot = 0;
							bool flag10 = text2 == "off";
							if (flag10)
							{
								text2 = null;
							}
							bool flag11 = args.Length > 4;
							if (flag11)
							{
								slot = (AnimationSlot)Enum.Parse(typeof(AnimationSlot), args[4].Trim(), true);
							}
							newEvent = new AnimationEvent(text2, slot);
						}
						else
						{
							bool flag12 = args[2] == "command";
							if (flag12)
							{
								string text3 = "";
								for (int i = 3; i < args.Length; i++)
								{
									text3 = text3 + args[i] + ((i - 1 == args.Length) ? "" : " ");
								}
								newEvent = new CommandEvent(text3);
							}
							else
							{
								bool flag13 = args[2] == "camera";
								if (flag13)
								{
									newEvent = new CameraEvent(bool.Parse(args[3]));
								}
								else
								{
									bool flag14 = args[2] == "particle";
									if (!flag14)
									{
										this._gameInstance.Chat.Log("Invalid event type '" + args[2] + "'");
										return;
									}
									newEvent = new ParticleEvent(args[3]);
								}
							}
						}
					}
					Func<TrackKeyframe, HytaleClient.InGame.Modules.Machinima.Events.KeyframeEvent, bool> addEvent = delegate(TrackKeyframe kFrame, HytaleClient.InGame.Modules.Machinima.Events.KeyframeEvent kEvent)
					{
						bool result;
						try
						{
							kFrame.AddEvent(kEvent);
							this._gameInstance.Chat.Log(string.Format("New event added to keyframe at frame {0}", kFrame.Frame));
							result = true;
						}
						catch (TrackKeyframe.DuplicateKeyframeEvent)
						{
							this._gameInstance.Chat.Log("Only one instance of that event may be added to a keyframe, aborting...");
							result = false;
						}
						return result;
					};
					this._gameInstance.Chat.Log("Please select the keyframe the event should be added to.");
					this._onUseToolItem = delegate(InteractionType actionType)
					{
						TrackKeyframe hoveredFrame = this.HoveredKeyframe;
						bool flag15 = hoveredFrame == null;
						if (flag15)
						{
							this._gameInstance.Chat.Log("Unable to find a keyframe in this location, aborting...");
						}
						else
						{
							bool flag16 = newEvent is TargetEvent;
							if (flag16)
							{
								this._gameInstance.Chat.Log("Please select the keyframe of the actor to target");
								this._onUseToolItem = delegate(InteractionType actionTypeSub)
								{
									TrackKeyframe hoveredKeyframe = this.HoveredKeyframe;
									SceneActor hoveredActor = this.HoveredActor;
									newEvent = new TargetEvent((hoveredKeyframe == null) ? null : this.HoveredActor);
									addEvent(hoveredFrame, newEvent);
									return false;
								};
							}
							else
							{
								addEvent(hoveredFrame, newEvent);
							}
						}
						return false;
					};
				}
			}
			else
			{
				this.ActiveActor.Track.ListEvents();
			}
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x00102538 File Offset: 0x00100738
		[Usage("mach fps", new string[]
		{
			"[speed]"
		})]
		private void FpsCommand(string[] args)
		{
			bool flag = args.Length < 2;
			if (flag)
			{
				this._gameInstance.Chat.Log(string.Format("Playback FPS currently set to {0}", this.PlaybackFPS));
			}
			else
			{
				float playbackFPS;
				bool flag2 = !float.TryParse(args[1], out playbackFPS);
				if (flag2)
				{
					this._gameInstance.Chat.Log("Unable to parse number from " + args[1]);
				}
				else
				{
					this.PlaybackFPS = playbackFPS;
					this._gameInstance.Chat.Log(string.Format("Playback FPS set to {0}", this.PlaybackFPS));
				}
			}
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x001025D9 File Offset: 0x001007D9
		[Usage("mach clear", new string[]
		{

		})]
		private void ClearCommand(string[] args)
		{
			this.ClearScenes();
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x001025E4 File Offset: 0x001007E4
		[Usage("mach display", new string[]
		{

		})]
		private void DisplayCommand(string[] args)
		{
			this.ShowEditor = !this.ShowEditor;
			string str = this.ShowEditor ? "Enabled" : "Disabled";
			this._gameInstance.Chat.Log("Path Display " + str);
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x00102633 File Offset: 0x00100833
		[Usage("mach restart", new string[]
		{

		})]
		private void RestartCommand(string[] args)
		{
			this.ResetScene(true);
			this._gameInstance.Chat.Log("Playback restarted.");
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00102654 File Offset: 0x00100854
		[Usage("mach autorestart", new string[]
		{

		})]
		private void AutorestartCommand(string[] args)
		{
			this.AutoRestartScene = !this.AutoRestartScene;
			string str = this.AutoRestartScene ? "Enabled" : "Disabled";
			this._gameInstance.Chat.Log("AutoRestart " + str);
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x001026A4 File Offset: 0x001008A4
		[Usage("mach pause", new string[]
		{

		})]
		private void PauseCommand(string[] args)
		{
			this.Running = !this.Running;
			bool running = this.Running;
			if (running)
			{
				this._gameInstance.Chat.Log(string.Format("Playback started at frame {0}", this.CurrentFrame));
				this._lastFrameTick = this.GetCurrentTime();
			}
			else
			{
				this._gameInstance.Chat.Log(string.Format("Paused at frame {0}", this.CurrentFrame));
			}
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x00102728 File Offset: 0x00100928
		[Usage("mach files", new string[]
		{

		})]
		private void FilesCommand(string[] args)
		{
			Process.Start(MachinimaModule.SceneDirectory);
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00102736 File Offset: 0x00100936
		[Usage("mach tool", new string[]
		{

		})]
		private void ToolCommand(string[] args)
		{
			this.GiveMachinimaTool();
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x00102740 File Offset: 0x00100940
		[Usage("mach key", new string[]
		{

		})]
		private void KeyCommand(string[] args)
		{
			bool flag = this.ActiveActor == null;
			if (flag)
			{
				this._gameInstance.Chat.Log("No active actor found, please select a keyframe to set.");
			}
			else
			{
				SceneTrack track = this.ActiveActor.Track;
				bool flag2 = args.Length > 1 && args[1] == "copy";
				TrackKeyframe trackKeyframe;
				if (flag2)
				{
					bool flag3 = this.ActiveActor == null;
					if (flag3)
					{
						this._gameInstance.Chat.Log("No Active Actor found!");
						return;
					}
					trackKeyframe = this.ActiveKeyframe.Clone();
					trackKeyframe.Frame = this.CurrentFrame;
				}
				else
				{
					bool flag4 = this.CurrentFrame >= track.GetTrackLength() || args.Length > 1;
					if (flag4)
					{
						float num = this.CurrentFrame;
						bool flag5 = this.CurrentFrame == track.GetTrackLength();
						if (flag5)
						{
							num += (float)this._gameInstance.App.Settings.MachinimaEditorSettings.NewKeyframeFrameOffset;
						}
						bool flag6 = args.Length > 1 && !float.TryParse(args[1], out num);
						if (flag6)
						{
							this._gameInstance.Chat.Log(string.Format("Unable to parse number from {0}, using default value {1}", args[1], num));
						}
						PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
						Vector3 zero = Vector3.Zero;
						bool flag7 = track.Parent is CameraActor;
						if (flag7)
						{
							zero.Y = localPlayer.EyeOffset;
						}
						Vector3 position = localPlayer.Position + zero;
						Vector3 lookOrientation = localPlayer.LookOrientation;
						Vector3 bodyOrientation = localPlayer.BodyOrientation;
						bodyOrientation.Y = MathHelper.WrapAngle(bodyOrientation.Y);
						lookOrientation.Y -= bodyOrientation.Y;
						trackKeyframe = this.ActiveActor.CreateKeyframe(num, position, bodyOrientation, lookOrientation);
						this.CurrentFrame = num;
					}
					else
					{
						trackKeyframe = track.GetCurrentFrame(this.CurrentFrame);
					}
				}
				track.AddKeyframe(trackKeyframe, true);
				this.UpdateFrame(0L, true);
				this._gameInstance.Chat.Log(string.Format("Added key at frame {0}", this.CurrentFrame));
			}
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0010296C File Offset: 0x00100B6C
		[Usage("mach save", new string[]
		{

		})]
		private void SaveCommand(string[] args)
		{
			this.SaveAllScenesToFile();
			this._gameInstance.Chat.Log(string.Format("{0} scenes saved to file.", this._scenes.Count));
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x001029A1 File Offset: 0x00100BA1
		[Usage("mach load", new string[]
		{

		})]
		private void LoadCommand(string[] args)
		{
			this.LoadAllScenesFromFile();
			this.SetActiveScene(null);
			this._gameInstance.Chat.Log(string.Format("{0} scenes loaded from file.", this._scenes.Count));
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x001029E0 File Offset: 0x00100BE0
		[Usage("mach update", new string[]
		{

		})]
		private void UpdateCommand(string[] args)
		{
			string text = (args.Length > 1) ? args[1] : "*";
			byte[] array = this.ActiveScene.ToCompressedByteArray(this._serializerSettings);
			sbyte[] array2 = Array.ConvertAll<byte, sbyte>(array, (byte b) => (sbyte)b);
			this._gameInstance.Connection.SendPacket(new UpdateMachinimaScene(text, this.ActiveScene.Name, 0f, 0, array2));
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x00102A60 File Offset: 0x00100C60
		[Usage("mach edit", new string[]
		{

		})]
		private void EditCommand(string[] args)
		{
			bool editFrame = args.Length > 1 && args[1] == "frame";
			this._gameInstance.Chat.Log("Please select the keyframe to " + (editFrame ? "change the frame of" : "edit"));
			this._onUseToolItem = delegate(InteractionType actionType)
			{
				TrackKeyframe hoveredKeyframe = this.HoveredKeyframe;
				bool flag = hoveredKeyframe == null;
				if (flag)
				{
					this._gameInstance.Chat.Log("Unable to find a keyframe in this location, aborting...");
				}
				else
				{
					this.ActiveActor = this.HoveredActor;
					bool editFrame = editFrame;
					if (editFrame)
					{
						bool flag2 = this.HoveredActor.Track.GetKeyframeByFrame(this.CurrentFrame) == null;
						if (flag2)
						{
							hoveredKeyframe.Frame = this.CurrentFrame;
							this.HoveredActor.Track.UpdateKeyframeData();
							this._gameInstance.Chat.Log("Keyframe updated.");
						}
						else
						{
							this._gameInstance.Chat.Log(string.Format("Unable to change keyframe, one is already set at frame #{0}", this.CurrentFrame));
						}
						return false;
					}
					this.ActiveActor.Visible = false;
					KeyframeSetting<Vector3> postionSetting = hoveredKeyframe.GetSetting<Vector3>("Position");
					KeyframeSetting<Vector3> lookSetting = hoveredKeyframe.GetSetting<Vector3>("Look");
					KeyframeSetting<Vector3> rotationSetting = hoveredKeyframe.GetSetting<Vector3>("Rotation");
					bool flag3;
					if (postionSetting != null)
					{
						Vector3 value = postionSetting.Value;
						flag3 = true;
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						Vector3 vector = postionSetting.Value;
						bool flag5 = this.ActiveActor is CameraActor;
						if (flag5)
						{
							vector -= new Vector3(0f, this._gameInstance.LocalPlayer.EyeOffset, 0f);
						}
						this._gameInstance.LocalPlayer.SetPosition(vector);
					}
					bool flag6;
					if (rotationSetting != null)
					{
						Vector3 value2 = rotationSetting.Value;
						flag6 = true;
					}
					else
					{
						flag6 = false;
					}
					bool flag7 = flag6;
					if (flag7)
					{
						this._gameInstance.LocalPlayer.SetBodyOrientation(rotationSetting.Value);
					}
					bool flag8;
					if (lookSetting != null)
					{
						Vector3 value3 = lookSetting.Value;
						flag8 = true;
					}
					else
					{
						flag8 = false;
					}
					bool flag9 = flag8;
					if (flag9)
					{
						this._gameInstance.LocalPlayer.LookOrientation = lookSetting.Value;
						bool flag10;
						if (rotationSetting != null)
						{
							Vector3 value4 = rotationSetting.Value;
							flag10 = true;
						}
						else
						{
							flag10 = false;
						}
						bool flag11 = flag10;
						if (flag11)
						{
							PlayerEntity localPlayer = this._gameInstance.LocalPlayer;
							localPlayer.LookOrientation.Y = localPlayer.LookOrientation.Y + rotationSetting.Value.Y;
						}
					}
					this.CurrentFrame = hoveredKeyframe.Frame;
					this._gameInstance.Chat.Log("Adjust your position and rotation then left click to set, or right click to cancel");
					this._onUseToolItem = delegate(InteractionType actionTypeSub)
					{
						this.ActiveActor.Visible = true;
						Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
						Vector3 bodyOrientation = this._gameInstance.LocalPlayer.BodyOrientation;
						bodyOrientation.Y = MathHelper.WrapAngle(bodyOrientation.Y);
						lookOrientation.Y -= bodyOrientation.Y;
						bool flag12 = actionTypeSub == 1;
						bool result;
						if (flag12)
						{
							this._gameInstance.Chat.Log("Keyframe edit cancelled");
							result = false;
						}
						else
						{
							bool flag13;
							if (postionSetting != null)
							{
								Vector3 value5 = postionSetting.Value;
								flag13 = true;
							}
							else
							{
								flag13 = false;
							}
							bool flag14 = flag13;
							if (flag14)
							{
								Vector3 vector2 = this._gameInstance.LocalPlayer.Position;
								bool flag15 = this.ActiveActor is CameraActor;
								if (flag15)
								{
									vector2 += new Vector3(0f, this._gameInstance.LocalPlayer.EyeOffset, 0f);
								}
								postionSetting.Value = vector2;
							}
							bool flag16;
							if (rotationSetting != null)
							{
								Vector3 value6 = rotationSetting.Value;
								flag16 = true;
							}
							else
							{
								flag16 = false;
							}
							bool flag17 = flag16;
							if (flag17)
							{
								rotationSetting.Value = bodyOrientation;
							}
							bool flag18;
							if (lookSetting != null)
							{
								Vector3 value7 = lookSetting.Value;
								flag18 = true;
							}
							else
							{
								flag18 = false;
							}
							bool flag19 = flag18;
							if (flag19)
							{
								lookSetting.Value = lookOrientation;
							}
							this.ActiveActor.Track.UpdatePositions();
							MachinimaScene activeScene = this.ActiveScene;
							if (activeScene != null)
							{
								activeScene.Update(this.CurrentFrame);
							}
							this._gameInstance.Chat.Log("Keyframe updated.");
							result = false;
						}
						return result;
					};
				}
				return false;
			};
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x00102AD8 File Offset: 0x00100CD8
		[Usage("mach speed", new string[]
		{

		})]
		private void SpeedCommand(string[] args)
		{
			bool flag = this.ActiveActor == null;
			if (!flag)
			{
				this.ResetScene(true);
				bool flag2 = args.Length > 2 && args[2] == "scale";
				bool flag3 = args.Length > 1;
				if (flag3)
				{
					float num;
					bool flag4 = !float.TryParse(args[1], out num);
					if (flag4)
					{
						this._gameInstance.Chat.Log("Unable to parse number from " + args[1]);
						return;
					}
					bool flag5 = flag2;
					if (flag5)
					{
						this.ActiveActor.Track.ScalePathSpeed(num);
					}
					else
					{
						this.ActiveActor.Track.SetPathSegmentSpeed(num / this.PlaybackFPS, 0, this.ActiveActor.Track.Keyframes.Count - 1);
					}
				}
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				for (int i = 0; i < this.ActiveActor.Track.Keyframes.Count - 1; i++)
				{
					float num5 = this.ActiveActor.Track.GetPathSegmentSpeed(i, 1) * this.PlaybackFPS;
					bool flag6 = i == 0;
					if (flag6)
					{
						num2 = num5;
						num3 = num5;
					}
					else
					{
						bool flag7 = num5 < num2;
						if (flag7)
						{
							num2 = num5;
						}
						bool flag8 = num5 > num3;
						if (flag8)
						{
							num3 = num5;
						}
					}
					num4 += num5;
				}
				double num6 = Math.Round((double)(num4 / (float)(this.ActiveActor.Track.Keyframes.Count - 1)), 2);
				this._gameInstance.Chat.Log(string.Format("Speed Avg: {0}, Min: {1}, Max: {2}", num6, Math.Round((double)num2, 2), Math.Round((double)num3, 2)));
			}
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x00102CA4 File Offset: 0x00100EA4
		[Usage("mach bezier", new string[]
		{

		})]
		private void BezierCommand(string[] args)
		{
			bool flag = this.ActiveActor == null;
			if (flag)
			{
				this._gameInstance.Chat.Log("No active actor found!");
			}
			else
			{
				bool flag2 = args.Length > 1 && args[1] == "smooth";
				if (flag2)
				{
					bool flag3 = !(this.ActiveActor.Track.Path is BezierPath);
					if (flag3)
					{
						this.ActiveActor.Track.SetPathType(SceneTrack.TrackPathType.Bezier, false);
					}
					this.ActiveActor.Track.SmoothBezierPath();
				}
				else
				{
					bool flag4 = args.Length > 1 && args[1] == "reset";
					if (flag4)
					{
						this.ActiveActor.Track.SetPathType(SceneTrack.TrackPathType.Bezier, true);
					}
					else
					{
						this.ActiveActor.Track.SetPathType(SceneTrack.TrackPathType.Bezier, false);
					}
				}
				this._gameInstance.Chat.Log("Bezier path updated.");
			}
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x00102D98 File Offset: 0x00100F98
		[Usage("mach spline", new string[]
		{

		})]
		private void SplineCommand(string[] args)
		{
			bool flag = this.ActiveActor == null;
			if (flag)
			{
				this._gameInstance.Chat.Log("No active actor found!");
			}
			else
			{
				this.ActiveActor.Track.SetPathType(SceneTrack.TrackPathType.Spline, false);
			}
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00102DE0 File Offset: 0x00100FE0
		[Usage("mach modeldebug", new string[]
		{

		})]
		private void ModelDebugCommand(string[] args)
		{
			bool flag = this.ActiveScene == null;
			if (flag)
			{
				this._gameInstance.Chat.Log("No active scene found");
			}
			else
			{
				int num = 0;
				string text = "";
				for (int i = 0; i < this.ActiveScene.Actors.Count; i++)
				{
					SceneActor sceneActor = this.ActiveScene.Actors[i];
					bool flag2 = !(sceneActor is EntityActor);
					if (!flag2)
					{
						bool flag3 = string.IsNullOrWhiteSpace((sceneActor as EntityActor).ModelId);
						if (flag3)
						{
							text = text + sceneActor.Name + ", ";
							num++;
						}
					}
				}
				this._gameInstance.Chat.Log(string.Format("{0} actors found with missing models", num));
				bool flag4 = num > 0;
				if (flag4)
				{
					this._gameInstance.Chat.Log(text);
				}
			}
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x00102ED8 File Offset: 0x001010D8
		[Usage("mach demo", new string[]
		{
			"[1|2|3|4|5|6|7]"
		})]
		private void DemoCommand(string[] args)
		{
			this.ResetScene(true);
			int num = 1;
			bool flag = args.Length > 1;
			if (flag)
			{
				int num2;
				bool flag2 = int.TryParse(args[1], out num2);
				if (!flag2)
				{
					this._gameInstance.Chat.Log("Invalid demo scene int number: " + args[1]);
					return;
				}
				num = num2;
			}
			string text = string.Format("{0}/demo{1}.hms", MachinimaModule.DemoSceneDirectory, num);
			string hash;
			bool flag3 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(text, ref hash);
			if (flag3)
			{
				this._gameInstance.Chat.Log(string.Format("Unable to find demo scene #{0}", num));
			}
			else
			{
				byte[] assetUsingHash = AssetManager.GetAssetUsingHash(hash, false);
				MachinimaScene activeScene = MachinimaScene.FromCompressedByteArray(assetUsingHash, this._gameInstance, this._serializerSettings);
				this.ActiveScene = activeScene;
				this._gameInstance.LocalPlayer.LookOrientation = this.ActiveScene.OriginLook;
				this.ActiveScene.OffsetOrigin(this._gameInstance.LocalPlayer.Position);
				this.ActiveScene.Update(0f);
				this._gameInstance.Chat.Log(string.Format("Demo scene #{0} loaded.", num));
			}
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x00103020 File Offset: 0x00101220
		[Usage("mach align [true|false]", new string[]
		{

		})]
		private void AlignCommand(string[] args)
		{
			bool flag = this.ActiveActor == null;
			if (flag)
			{
				this._gameInstance.Chat.Log("No active actor found!");
			}
			else
			{
				this.ResetScene(true);
				bool alignAll = false;
				bool flag2 = args.Length > 1;
				if (flag2)
				{
					bool.TryParse(args[1], out alignAll);
				}
				this.ActiveActor.AlignToPath(alignAll);
			}
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x00103084 File Offset: 0x00101284
		[Usage("mach offset", new string[]
		{

		})]
		private void OffsetCommand(string[] args)
		{
			bool flag = this.ActiveActor == null;
			if (flag)
			{
				this._gameInstance.Chat.Log("No active actor found!");
			}
			else
			{
				int num = 0;
				int num2 = 0;
				bool flag2 = args.Length > 1;
				if (flag2)
				{
					bool flag3 = !int.TryParse(args[1], out num2);
					if (flag3)
					{
						this._gameInstance.Chat.Log("Invalid value found for offset amount " + args[1] + "!");
						return;
					}
				}
				bool flag4 = args.Length > 2;
				if (flag4)
				{
					bool flag5 = !int.TryParse(args[2], out num);
					if (flag5)
					{
						this._gameInstance.Chat.Log("Invalid value found for insert frame " + args[1] + "!");
						return;
					}
				}
				this.ResetScene(true);
				foreach (SceneActor sceneActor in this.ActiveScene.Actors)
				{
					sceneActor.Track.InsertKeyframeOffset((float)num, (float)num2);
				}
				this.ResetScene(false);
				this.UpdateFrame(0L, true);
				this._gameInstance.Chat.Log("Offset actor keyframes!");
			}
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x001031DC File Offset: 0x001013DC
		[Usage("mach fixlook", new string[]
		{

		})]
		private void FixLookCommand(string[] args)
		{
			bool flag = this.ActiveScene == null;
			if (!flag)
			{
				this.ResetScene(true);
				foreach (SceneActor sceneActor in this.ActiveScene.Actors)
				{
					foreach (TrackKeyframe trackKeyframe in sceneActor.Track.Keyframes)
					{
						Vector3 value = trackKeyframe.GetSetting<Vector3>("Look").Value;
						value.Y -= trackKeyframe.GetSetting<Vector3>("Rotation").Value.Y;
						trackKeyframe.GetSetting<Vector3>("Look").Value = value;
					}
					sceneActor.Track.UpdateKeyframeData();
				}
				this.ResetScene(true);
				this._gameInstance.Chat.Log("Keyframe look settings fixed!");
			}
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x0010330C File Offset: 0x0010150C
		[Usage("mach camera", new string[]
		{

		})]
		private void CameraCommand(string[] args)
		{
			CameraActor cameraActor = null;
			bool flag = this.ActiveActor is CameraActor;
			if (flag)
			{
				cameraActor = (this.ActiveActor as CameraActor);
			}
			else
			{
				List<SceneActor> actors = this.ActiveScene.GetActors();
				foreach (SceneActor sceneActor in actors)
				{
					bool flag2 = sceneActor is CameraActor;
					if (flag2)
					{
						cameraActor = (sceneActor as CameraActor);
						break;
					}
				}
			}
			bool flag3 = cameraActor == null;
			if (flag3)
			{
				this._gameInstance.Chat.Log("Unable to find any cameras in the current scene.");
			}
			else
			{
				cameraActor.SetState(!cameraActor.Active);
			}
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x001033D8 File Offset: 0x001015D8
		[Usage("mach entitylight", new string[]
		{

		})]
		private void EntityLightCommand(string[] args)
		{
			bool flag = args.Length < 2;
			if (flag)
			{
				this._gameInstance.Chat.Log("Please add the name of the actor to change the light value");
			}
			else
			{
				string text = args[1];
				SceneActor actor = this.ActiveScene.GetActor(text);
				bool flag2 = actor == null;
				if (flag2)
				{
					this._gameInstance.Chat.Log("Unable to find an actor with name " + text);
				}
				else
				{
					EntityActor entityActor = actor as EntityActor;
					bool flag3 = entityActor != null;
					if (flag3)
					{
						bool flag4 = args.Length < 5;
						if (flag4)
						{
							entityActor.GetEntity().SetDynamicLight(null);
						}
						else
						{
							byte b;
							byte.TryParse(args[2], out b);
							byte b2;
							byte.TryParse(args[3], out b2);
							byte b3;
							byte.TryParse(args[4], out b3);
							ColorLight dynamicLight = new ColorLight(10, (sbyte)b, (sbyte)b2, (sbyte)b3);
							entityActor.GetEntity().SetDynamicLight(dynamicLight);
						}
					}
					else
					{
						this._gameInstance.Chat.Log("Only entity actors may have there light value set");
					}
				}
			}
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x001034D4 File Offset: 0x001016D4
		[Usage("mach zip", new string[]
		{
			"[true|false]"
		})]
		private void ZipCommand(string[] args)
		{
			bool flag = !this._settings.CompressSaveFiles;
			bool flag2 = args.Length > 1;
			if (flag2)
			{
				bool flag4;
				bool flag3 = !bool.TryParse(args[1].ToLower(), out flag4);
				if (flag3)
				{
					this._gameInstance.Chat.Log("Unable to parse bool value: '" + args[1] + "'");
					return;
				}
				flag = flag4;
			}
			this._settings.CompressSaveFiles = flag;
			this._gameInstance.App.Settings.Save();
			this._gameInstance.Chat.Log("Scene file compression " + (flag ? "enabled" : "disabled") + ".");
			this.Autosave(true);
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x00103594 File Offset: 0x00101794
		[Usage("mach autosave", new string[]
		{
			"[seconds]"
		})]
		private void AutosaveCommand(string[] args)
		{
			bool flag = args.Length == 1;
			if (flag)
			{
				this._gameInstance.Chat.Log(string.Format("Autosave Delay: {0} secs", this._settings.AutosaveDelay));
			}
			else
			{
				int num;
				bool flag2 = !int.TryParse(args[1].ToLower(), out num);
				if (flag2)
				{
					this._gameInstance.Chat.Log("Unable to parse int value: '" + args[1] + "'");
				}
				else
				{
					bool flag3 = num < 10 || num > 600;
					if (flag3)
					{
						this._gameInstance.Chat.Log(string.Format("Delay must be between {0} and {1}", 10, 600));
					}
					else
					{
						this._settings.AutosaveDelay = num;
						this._gameInstance.App.Settings.Save();
						this._gameInstance.Chat.Log(string.Format("Autosave delay set to: {0} secs.", num));
					}
				}
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x06004611 RID: 17937 RVA: 0x001036A4 File Offset: 0x001018A4
		// (set) Token: 0x06004612 RID: 17938 RVA: 0x001036BC File Offset: 0x001018BC
		public bool Running
		{
			get
			{
				return this._running;
			}
			private set
			{
				this._running = value;
				this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.sceneRunningChanged", value, null, null, null, null);
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06004613 RID: 17939 RVA: 0x001036EC File Offset: 0x001018EC
		// (set) Token: 0x06004614 RID: 17940 RVA: 0x00103704 File Offset: 0x00101904
		public bool AutoRestartScene
		{
			get
			{
				return this._autoRestartScene;
			}
			set
			{
				this._autoRestartScene = value;
				this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.sceneAutoRestartChanged", value, null, null, null, null);
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x00103733 File Offset: 0x00101933
		// (set) Token: 0x06004616 RID: 17942 RVA: 0x0010373B File Offset: 0x0010193B
		public float CurrentFrame { get; private set; }

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x06004617 RID: 17943 RVA: 0x00103744 File Offset: 0x00101944
		// (set) Token: 0x06004618 RID: 17944 RVA: 0x00103775 File Offset: 0x00101975
		public float PlaybackFPS
		{
			get
			{
				return (float)Math.Round((double)(1000f / this._msTimePerFrame * 100f)) / 100f;
			}
			set
			{
				this._msTimePerFrame = MathHelper.Min(MathHelper.Max(1000f / value, 0.01f), 1000f);
			}
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x0010379C File Offset: 0x0010199C
		public MachinimaModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._serializerSettings = new JsonSerializerSettings
			{
				Converters = 
				{
					new MachinimaSceneJsonConverter(gameInstance, this)
				}
			};
			this._settings = this._gameInstance.App.Settings.MachinimaEditorSettings;
			this._gameInstance = gameInstance;
			GraphicsDevice graphics = gameInstance.Engine.Graphics;
			FontFamily defaultFontFamily = gameInstance.App.Fonts.DefaultFontFamily;
			this._rotationGizmo = new RotationGizmo(graphics, defaultFontFamily.RegularFont, new RotationGizmo.OnRotationChange(this.OnRotationChange), 0.2617994f);
			this._translationGizmo = new TranslationGizmo(graphics, new TranslationGizmo.OnPositionChange(this.OnPositionChange));
			this._boxRenderer = new BoxRenderer(graphics, graphics.GPUProgramStore.BasicProgram);
			this._textRenderer = new TextRenderer(graphics, defaultFontFamily.RegularFont, "Entity", uint.MaxValue, 4278190080U);
			this._curvePathRenderer = new LineRenderer(graphics, graphics.GPUProgramStore.BasicProgram);
			this._tooltip = new MachinimaModule.Tooltip(gameInstance.Engine.Graphics, gameInstance.App.Fonts.DefaultFontFamily.RegularFont);
			int width = this._gameInstance.Engine.Window.Viewport.Width;
			int height = this._gameInstance.Engine.Window.Viewport.Height;
			this._tooltip.UpdateOrthographicProjectionMatrix(width, height);
			this._tooltip.ScreenPosition = new Vector2(5f, (float)(height - 5));
			Directory.CreateDirectory(MachinimaModule.SceneDirectory);
			Directory.CreateDirectory(MachinimaModule.AutosaveDirectory);
			this.PlaybackFPS = 60f;
			this.RegisterCommands();
			this.RegisterEvents();
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x00103A74 File Offset: 0x00101C74
		protected override void DoDispose()
		{
			this.Autosave(true);
			this.UnregisterEvents();
			foreach (MachinimaScene machinimaScene in this._scenes.Values)
			{
				machinimaScene.Dispose();
			}
			this._rotationGizmo.Dispose();
			this._translationGizmo.Dispose();
			this._boxRenderer.Dispose();
			this._textRenderer.Dispose();
			this._curvePathRenderer.Dispose();
			this._tooltip.Dispose();
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x00103B28 File Offset: 0x00101D28
		[Obsolete]
		public override void Tick()
		{
			long currentTime = this.GetCurrentTime();
			long num = currentTime - this._lastFrameTick;
			bool flag = this.HasActiveTool();
			if (flag)
			{
				this.TickEditor((float)num);
				this.Autosave(false);
			}
			this._lastFrameTick = currentTime;
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00103B6C File Offset: 0x00101D6C
		[Obsolete]
		public override void OnNewFrame(float deltaTime)
		{
			bool running = this.Running;
			if (running)
			{
				this.UpdateFrame((long)(deltaTime * 1000f), false);
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x00103B94 File Offset: 0x00101D94
		private void UpdateFrame(long deltaTime = 0L, bool forceUpdate = false)
		{
			bool flag = this.ActiveScene == null || (!this.Running && !forceUpdate);
			if (!flag)
			{
				int num = (int)this.CurrentFrame;
				float sceneLength = this.ActiveScene.GetSceneLength();
				bool flag2 = this.CurrentFrame >= sceneLength && this.Running;
				if (flag2)
				{
					this.OnSceneEnd();
				}
				else
				{
					float num2 = (float)deltaTime / this._msTimePerFrame;
					this.CurrentFrame = MathHelper.Max(this.CurrentFrame + num2, 0f);
				}
				bool flag3 = this.ActiveScene != null;
				if (flag3)
				{
					this.ActiveScene.Update(this.CurrentFrame);
					float sceneLength2 = this.ActiveScene.GetSceneLength();
					this._tooltip.Progress = this.CurrentFrame / sceneLength2;
					this._tooltip.TooltipText = string.Format("Frame: {0}", Math.Round((double)this.CurrentFrame));
				}
				bool flag4 = this._isInterfaceOpen && (int)this.CurrentFrame != num;
				if (flag4)
				{
					this.UpdateCurrentFrameInInterface();
				}
			}
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x00103CB4 File Offset: 0x00101EB4
		public void Draw(ref Matrix viewProjectionMatrix)
		{
			bool flag = this.HasActiveTool();
			if (flag)
			{
				GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
				GLFunctions gl = graphics.GL;
				bool flag2 = this.EditMode == MachinimaModule.EditorMode.RotateHead || this.EditMode == MachinimaModule.EditorMode.RotateBody;
				if (flag2)
				{
					bool visible = this._rotationGizmo.Visible;
					if (visible)
					{
						this._rotationGizmo.Draw(ref viewProjectionMatrix, this._gameInstance.CameraModule.Controller, Vector3.Zero);
					}
				}
				bool flag3 = this.EditMode == MachinimaModule.EditorMode.Translate;
				if (flag3)
				{
					this._translationGizmo.Draw(ref viewProjectionMatrix, Vector3.Zero);
				}
				gl.DepthFunc(GL.ALWAYS);
				bool flag4 = this.ShowEditor && this.ActiveScene != null;
				if (flag4)
				{
					this.ActiveScene.Draw(ref viewProjectionMatrix);
					this._boxRenderer.Draw(this.ActiveScene.Origin, TrackKeyframe.PathBox, viewProjectionMatrix, graphics.WhiteColor, 0.7f, graphics.WhiteColor, 0.2f);
				}
				this._tooltip.DrawBackground(ref viewProjectionMatrix);
				bool flag5 = this.ActiveActor != null && this.ActiveKeyframe != null;
				if (flag5)
				{
					bool flag6 = this.ActiveActor.Track.PathType == SceneTrack.TrackPathType.Bezier && this.SelectionMode == MachinimaModule.EditorSelectionMode.Keyframe;
					if (flag6)
					{
						foreach (TrackKeyframe trackKeyframe in this.ActiveActor.Track.Keyframes)
						{
							bool flag7 = trackKeyframe.Frame >= this.ActiveActor.Track.GetTrackLength();
							if (!flag7)
							{
								Vector3 value = trackKeyframe.GetSetting<Vector3>("Position").Value;
								KeyframeSetting<Vector3[]> setting = trackKeyframe.GetSetting<Vector3[]>("Curve");
								Vector3 yellowColor = graphics.YellowColor;
								bool flag8 = setting != null;
								if (flag8)
								{
									int nextKeyframe = this.ActiveActor.Track.GetNextKeyframe(trackKeyframe.Frame);
									float num = 0.7f;
									bool flag9 = this._selectedGrip == null;
									if (flag9)
									{
										num = 0.3f;
									}
									Vector3[] value2 = setting.Value;
									Vector3[] array = new Vector3[value2.Length + 2];
									array[0] = value;
									for (int i = 0; i < value2.Length; i++)
									{
										bool flag10 = this._hoveredGrip != null && this._hoveredGrip.Matches(this.ActiveActor, trackKeyframe, i);
										bool flag11 = flag10 && this._selectedGrip != null;
										float num2 = num;
										bool flag12 = flag10 || flag11;
										if (flag12)
										{
											num2 = 0.8f;
										}
										Vector3 vector = flag11 ? graphics.CyanColor : (flag10 ? graphics.MagentaColor : graphics.YellowColor);
										this._boxRenderer.Draw(value2[i] + value, TrackKeyframe.PathBox, viewProjectionMatrix, vector, num2, vector, num2 * 0.25f);
										array[i + 1] = value2[i] + value;
									}
									bool flag13 = nextKeyframe != -1;
									if (flag13)
									{
										TrackKeyframe trackKeyframe2 = this.ActiveActor.Track.Keyframes[nextKeyframe];
										bool flag14 = trackKeyframe2.Frame != trackKeyframe.Frame;
										if (flag14)
										{
											array[array.Length - 1] = trackKeyframe2.GetSetting<Vector3>("Position").Value;
											this._curvePathRenderer.UpdateLineData(array);
											this._curvePathRenderer.Draw(ref viewProjectionMatrix, yellowColor, num);
										}
									}
								}
							}
						}
					}
				}
				gl.DepthFunc((!graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			}
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x001040A8 File Offset: 0x001022A8
		public void DrawText(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.TextNeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with TextNeedsDrawing() first before calling this.");
			}
			bool flag2 = this.HasActiveTool();
			if (flag2)
			{
				bool flag3 = this.EditMode == MachinimaModule.EditorMode.RotateHead || this.EditMode == MachinimaModule.EditorMode.RotateBody;
				if (flag3)
				{
					this._rotationGizmo.DrawText();
				}
				this._tooltip.DrawText(ref viewProjectionMatrix);
			}
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x0010410C File Offset: 0x0010230C
		public bool NeedsDrawing()
		{
			return this.HasActiveTool() && this.ShowEditor && this.ActiveScene != null;
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0010413C File Offset: 0x0010233C
		public bool TextNeedsDrawing()
		{
			return this.HasActiveTool() && this.ShowEditor;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00104160 File Offset: 0x00102360
		private void TogglePause()
		{
			this.Running = !this.Running;
			bool running = this.Running;
			if (running)
			{
				this._lastFrameTick = this.GetCurrentTime();
			}
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00104194 File Offset: 0x00102394
		private long GetCurrentTime()
		{
			return DateTime.Now.Ticks / 10000L;
		}

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x06004624 RID: 17956 RVA: 0x001041BA File Offset: 0x001023BA
		// (set) Token: 0x06004625 RID: 17957 RVA: 0x001041C2 File Offset: 0x001023C2
		public TrackKeyframe HoveredKeyframe { get; private set; }

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06004626 RID: 17958 RVA: 0x001041CB File Offset: 0x001023CB
		// (set) Token: 0x06004627 RID: 17959 RVA: 0x001041D3 File Offset: 0x001023D3
		public TrackKeyframe SelectedKeyframe { get; private set; }

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x06004628 RID: 17960 RVA: 0x001041DC File Offset: 0x001023DC
		// (set) Token: 0x06004629 RID: 17961 RVA: 0x001041E4 File Offset: 0x001023E4
		public TrackKeyframe ActiveKeyframe { get; private set; }

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x0600462A RID: 17962 RVA: 0x001041ED File Offset: 0x001023ED
		// (set) Token: 0x0600462B RID: 17963 RVA: 0x001041F5 File Offset: 0x001023F5
		public SceneActor HoveredActor { get; private set; }

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x001041FE File Offset: 0x001023FE
		// (set) Token: 0x0600462D RID: 17965 RVA: 0x00104206 File Offset: 0x00102406
		public SceneActor SelectedActor { get; private set; }

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x0010420F File Offset: 0x0010240F
		// (set) Token: 0x0600462F RID: 17967 RVA: 0x00104217 File Offset: 0x00102417
		public SceneActor ActiveActor { get; private set; }

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x00104220 File Offset: 0x00102420
		// (set) Token: 0x06004631 RID: 17969 RVA: 0x00104228 File Offset: 0x00102428
		public bool ShowEditor { get; private set; } = true;

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x00104231 File Offset: 0x00102431
		// (set) Token: 0x06004633 RID: 17971 RVA: 0x00104239 File Offset: 0x00102439
		public bool ShowPathNodes { get; private set; } = true;

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06004634 RID: 17972 RVA: 0x00104242 File Offset: 0x00102442
		// (set) Token: 0x06004635 RID: 17973 RVA: 0x0010424A File Offset: 0x0010244A
		public bool ShowCameraFrustum { get; private set; } = true;

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x00104253 File Offset: 0x00102453
		// (set) Token: 0x06004637 RID: 17975 RVA: 0x0010425B File Offset: 0x0010245B
		public bool ContinousPlaybackEnabled { get; private set; }

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06004638 RID: 17976 RVA: 0x00104264 File Offset: 0x00102464
		// (set) Token: 0x06004639 RID: 17977 RVA: 0x0010426C File Offset: 0x0010246C
		public bool BodyRotateHover { get; private set; }

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x0600463A RID: 17978 RVA: 0x00104275 File Offset: 0x00102475
		// (set) Token: 0x0600463B RID: 17979 RVA: 0x0010427D File Offset: 0x0010247D
		public MachinimaModule.EditorMode EditMode { get; private set; } = MachinimaModule.EditorMode.None;

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x00104286 File Offset: 0x00102486
		// (set) Token: 0x0600463D RID: 17981 RVA: 0x0010428E File Offset: 0x0010248E
		public MachinimaModule.EditorSelectionMode SelectionMode { get; private set; } = MachinimaModule.EditorSelectionMode.Keyframe;

		// Token: 0x0600463E RID: 17982 RVA: 0x00104298 File Offset: 0x00102498
		public void OnInteraction(InteractionType interactionType)
		{
			bool flag = this._onUseToolItem != null;
			if (flag)
			{
				MachinimaModule.OnUseToolItem onUseToolItem = this._onUseToolItem;
				this._onUseToolItem = null;
				bool flag2 = onUseToolItem(interactionType);
				if (flag2)
				{
					this._onUseToolItem = onUseToolItem;
				}
			}
			else
			{
				bool flag3 = this._rotationGizmo.Visible && (!this._gameInstance.Input.IsAnyModifierHeld() || interactionType == 1 || this._rotationGizmo.InUse());
				if (flag3)
				{
					this._rotationGizmo.OnInteract(interactionType);
					bool flag4 = !this._rotationGizmo.Visible && (this.EditMode == MachinimaModule.EditorMode.RotateHead || this.EditMode == MachinimaModule.EditorMode.RotateBody);
					if (flag4)
					{
						this.SelectedKeyframe = null;
						this.EditMode = MachinimaModule.EditorMode.None;
					}
				}
				else
				{
					bool flag5 = this._translationGizmo.Visible && (!this._gameInstance.Input.IsAnyModifierHeld() || interactionType == 1 || this._translationGizmo.InUse());
					if (flag5)
					{
						Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
						this._translationGizmo.OnInteract(lookRay, interactionType);
						bool flag6 = !this._translationGizmo.Visible && this.EditMode == MachinimaModule.EditorMode.Translate;
						if (flag6)
						{
							bool flag7 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe;
							if (flag7)
							{
								this.SelectedKeyframe = null;
							}
							else
							{
								this._selectedGrip = null;
							}
							this.EditMode = MachinimaModule.EditorMode.None;
						}
					}
					else
					{
						bool flag8 = interactionType == 0;
						if (flag8)
						{
							bool flag9 = this.EditMode == MachinimaModule.EditorMode.FreeMove;
							if (flag9)
							{
								bool flag10 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe;
								if (flag10)
								{
									this.SelectedKeyframe = null;
								}
								else
								{
									this._selectedGrip = null;
								}
								this.EditMode = MachinimaModule.EditorMode.None;
							}
							else
							{
								bool flag11 = this.EditMode == MachinimaModule.EditorMode.None && (this.HoveredKeyframe != null || this._hoveredGrip != null);
								if (flag11)
								{
									this.SelectKeyframe();
								}
							}
						}
						else
						{
							bool flag12 = this.EditMode == MachinimaModule.EditorMode.FreeMove;
							if (flag12)
							{
								bool flag13 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Keyframe;
								if (flag13)
								{
									this.OnPositionChange(this._lastKeyframePosition);
								}
								else
								{
									bool flag14 = this.SelectedKeyframe != null;
									if (flag14)
									{
										Vector3 value = this.SelectedKeyframe.GetSetting<Vector3>("Position").Value;
										Vector3 vector = this._lastKeyframePosition - value;
										bool flag15 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Actor;
										if (flag15)
										{
											this.ActiveActor.Track.OffsetPositions(vector);
										}
										else
										{
											this.ActiveScene.OffsetOrigin(this.ActiveScene.Origin + vector);
										}
									}
								}
								bool flag16 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe;
								if (flag16)
								{
									this.SelectedKeyframe = null;
								}
								else
								{
									this._selectedGrip = null;
								}
								this.EditMode = MachinimaModule.EditorMode.None;
							}
							else
							{
								bool flag17 = this.EditMode == MachinimaModule.EditorMode.None && this.HoveredKeyframe != null;
								if (flag17)
								{
									this.ActiveActor = this.HoveredActor;
									this.ActiveKeyframe = this.HoveredKeyframe;
									bool flag18 = this.ActiveActor.Track.Keyframes.Count > 1;
									if (flag18)
									{
										this.CurrentFrame = this.HoveredKeyframe.Frame;
										this.ActiveScene.Update(this.CurrentFrame);
										this.UpdateFrame(0L, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x001045E8 File Offset: 0x001027E8
		private void TickEditor(float dt)
		{
			Input input = this._gameInstance.Input;
			float num = 1f;
			bool flag = input.IsAltHeld();
			if (flag)
			{
				num = 0.5f;
			}
			else
			{
				bool flag2 = input.IsShiftHeld();
				if (flag2)
				{
					num = 2f;
				}
			}
			bool flag3 = input.IsKeyHeld(this._keybinds[MachinimaModule.Keybind.FrameDecrement], false);
			if (flag3)
			{
				this.UpdateFrame(-(long)(dt * num), true);
			}
			bool flag4 = input.IsKeyHeld(this._keybinds[MachinimaModule.Keybind.FrameIncrement], false);
			if (flag4)
			{
				this.UpdateFrame((long)(dt * num), true);
			}
			bool flag5 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.KeyframeDecrement], false);
			if (flag5)
			{
				bool flag6 = this.ActiveKeyframe != null && this.ActiveActor != null;
				if (flag6)
				{
					int previousKeyframe = this.ActiveActor.Track.GetPreviousKeyframe(input.IsShiftHeld() ? 0f : (this.ActiveKeyframe.Frame - 0.01f));
					this.ActiveKeyframe = this.ActiveActor.Track.Keyframes[previousKeyframe];
					this.CurrentFrame = this.ActiveKeyframe.Frame;
				}
			}
			bool flag7 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.KeyframeIncrement], false);
			if (flag7)
			{
				bool flag8 = this.ActiveKeyframe != null && this.ActiveActor != null;
				if (flag8)
				{
					int nextKeyframe = this.ActiveActor.Track.GetNextKeyframe(input.IsShiftHeld() ? this.ActiveScene.GetSceneLength() : this.ActiveKeyframe.Frame);
					this.ActiveKeyframe = this.ActiveActor.Track.Keyframes[nextKeyframe];
					this.CurrentFrame = this.ActiveKeyframe.Frame;
				}
			}
			bool flag9 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.CycleSelectionMode], false);
			if (flag9)
			{
				this.CycleSelectionMode();
			}
			bool flag10 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.TogglePause], false);
			if (flag10)
			{
				bool flag11 = input.IsShiftHeld();
				if (flag11)
				{
					this._continousPlayback = !this._continousPlayback;
					bool flag12 = this._continousPlayback && !this.AutoRestartScene;
					if (flag12)
					{
						this.AutoRestartScene = true;
					}
					this._gameInstance.Chat.Log("Continous playback " + (this._continousPlayback ? "enabled" : "disabled"));
				}
				else
				{
					bool flag13 = input.IsAltHeld();
					if (flag13)
					{
						this.AutoRestartScene = !this.AutoRestartScene;
						this._gameInstance.Chat.Log("Auto scene restart " + (this.AutoRestartScene ? "enabled" : "disabled"));
					}
					else
					{
						this.TogglePause();
					}
				}
			}
			bool flag14 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.ToggleDisplay], false);
			if (flag14)
			{
				bool flag15 = input.IsShiftHeld();
				if (flag15)
				{
					this.ShowPathNodes = !this.ShowPathNodes;
				}
				else
				{
					this.ShowEditor = !this.ShowEditor;
				}
			}
			bool flag16 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.ToggleCamera], false);
			if (flag16)
			{
				bool flag17 = input.IsShiftHeld();
				if (flag17)
				{
					this.ShowCameraFrustum = !this.ShowCameraFrustum;
				}
				else
				{
					this._gameInstance.ExecuteCommand(".mach camera");
				}
			}
			bool flag18 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.AddKeyframe], false);
			if (flag18)
			{
				string text = ".mach key";
				bool flag19 = input.IsAltHeld();
				if (flag19)
				{
					text += " copy";
				}
				else
				{
					bool flag20 = input.IsShiftHeld();
					if (flag20)
					{
						text += string.Format(" {0}", this.CurrentFrame);
					}
				}
				this._gameInstance.ExecuteCommand(text);
			}
			bool flag21 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.RestartScene], false);
			if (flag21)
			{
				bool flag22 = input.IsShiftHeld();
				if (flag22)
				{
					this.EndScene();
				}
				else
				{
					this._gameInstance.ExecuteCommand(".mach restart");
				}
			}
			bool flag23 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.EditKeyframe], false);
			if (flag23)
			{
				bool flag24 = input.IsAltHeld();
				if (flag24)
				{
					this._gameInstance.ExecuteCommand(".mach edit frame");
				}
				else
				{
					bool flag25 = input.IsShiftHeld();
					if (flag25)
					{
						this._gameInstance.ExecuteCommand(".mach actor move");
					}
					else
					{
						this._gameInstance.ExecuteCommand(".mach edit");
					}
				}
			}
			bool flag26 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.OriginAction], false) && this.ActiveScene != null;
			if (flag26)
			{
				bool flag27 = input.IsShiftHeld();
				if (flag27)
				{
					this.ActiveScene.Origin = this._gameInstance.LocalPlayer.Position;
					this.ActiveScene.OriginLook = this._gameInstance.LocalPlayer.LookOrientation;
					this._gameInstance.Chat.Log("Scene origin set.");
				}
				else
				{
					bool flag28 = input.IsAltHeld();
					if (flag28)
					{
						this.ActiveScene.OffsetOrigin(this._gameInstance.LocalPlayer.Position);
						this._gameInstance.Chat.Log("Scene offset to origin.");
					}
					else
					{
						this._gameInstance.LocalPlayer.SetPosition(this.ActiveScene.Origin);
						this._gameInstance.LocalPlayer.LookOrientation = this.ActiveScene.OriginLook;
					}
				}
			}
			bool flag29 = this.HoveredKeyframe != null || this.SelectedKeyframe != null;
			if (flag29)
			{
				TrackKeyframe trackKeyframe = (this.HoveredKeyframe == null) ? this.SelectedKeyframe : this.HoveredKeyframe;
				SceneTrack sceneTrack = (this.HoveredActor == null) ? this.SelectedActor.Track : this.HoveredActor.Track;
				bool flag30 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.RemoveKeyframe], false);
				if (flag30)
				{
					try
					{
						float frame = trackKeyframe.Frame;
						sceneTrack.RemoveKeyframe(frame);
						this._gameInstance.Chat.Log(string.Format("Removed keyframe for frame {0}", frame));
					}
					catch (Exception ex)
					{
						this._gameInstance.Chat.Error(ex.Message);
					}
				}
				bool flag31 = trackKeyframe.Frame > 0f;
				if (flag31)
				{
					bool flag32 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.FrameTimeDecrease], false);
					if (flag32)
					{
						sceneTrack.InsertKeyframeOffset(trackKeyframe.Frame, 1f);
					}
					bool flag33 = input.ConsumeKey(this._keybinds[MachinimaModule.Keybind.FrameTimeIncrease], false);
					if (flag33)
					{
						sceneTrack.InsertKeyframeOffset(trackKeyframe.Frame, -1f);
					}
				}
			}
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			float targetBlockHitDistance = this._gameInstance.InteractionModule.HasFoundTargetBlock ? this._gameInstance.InteractionModule.TargetBlockHit.Distance : 0f;
			bool flag34 = this.EditMode == MachinimaModule.EditorMode.RotateHead || this.EditMode == MachinimaModule.EditorMode.RotateBody;
			if (flag34)
			{
				this._rotationGizmo.Tick(lookRay, targetBlockHitDistance);
				this._rotationGizmo.UpdateRotation(this._gameInstance.Input.IsShiftHeld());
			}
			bool flag35 = this.EditMode == MachinimaModule.EditorMode.Translate;
			if (flag35)
			{
				this._translationGizmo.Tick(lookRay);
			}
			this.CheckKeyframeHover();
			bool flag36 = this.EditMode == MachinimaModule.EditorMode.FreeMove;
			if (flag36)
			{
				Vector3 vector = lookRay.Position + lookRay.Direction * this._targetDistance;
				bool hasFoundTargetBlock = this._gameInstance.InteractionModule.HasFoundTargetBlock;
				if (hasFoundTargetBlock)
				{
					bool flag37 = this._gameInstance.InteractionModule.TargetBlockHit.Distance < this._targetDistance || input.IsShiftHeld();
					if (flag37)
					{
						bool flag38 = !input.IsAltHeld();
						if (flag38)
						{
							vector = this._gameInstance.InteractionModule.TargetBlockHit.HitPosition;
						}
					}
				}
				bool flag39 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Keyframe;
				if (flag39)
				{
					bool flag40 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe;
					if (flag40)
					{
						this.SelectedKeyframe.GetSetting<Vector3>("Position").Value = vector;
						this.ActiveActor.Track.UpdatePositions();
						bool flag41 = this.ActiveActor is EntityActor;
						if (flag41)
						{
							this.ActiveActor.Position = vector;
							((EntityActor)this.ActiveActor).ForceUpdate(this._gameInstance);
						}
					}
					else
					{
						KeyframeSetting<Vector3[]> setting = this._selectedGrip.Keyframe.GetSetting<Vector3[]>("Curve");
						Vector3 value = this._selectedGrip.Keyframe.GetSetting<Vector3>("Position").Value;
						Vector3[] value2 = setting.Value;
						value2[this._selectedGrip.Index] = vector - value;
						setting.Value = value2;
						bool updateTangent = this._selectedGrip.UpdateTangent;
						if (updateTangent)
						{
							bool flag42 = this._selectedGrip.PrevKeyframe != null;
							if (flag42)
							{
								KeyframeSetting<Vector3[]> setting2 = this._selectedGrip.PrevKeyframe.GetSetting<Vector3[]>("Curve");
								Vector3 value3 = this._selectedGrip.PrevKeyframe.GetSetting<Vector3>("Position").Value;
								Vector3[] value4 = setting2.Value;
								value4[1] = value + (vector - value) * -1f - value3;
								setting2.Value = value4;
							}
							bool flag43 = this._selectedGrip.NextKeyframe != null;
							if (flag43)
							{
								KeyframeSetting<Vector3[]> setting3 = this._selectedGrip.NextKeyframe.GetSetting<Vector3[]>("Curve");
								Vector3 value5 = this._selectedGrip.NextKeyframe.GetSetting<Vector3>("Position").Value;
								Vector3[] value6 = setting3.Value;
								value6[0] = value5 - vector;
								setting3.Value = value6;
							}
						}
						this._selectedGrip.Actor.Track.UpdatePositions();
					}
				}
				else
				{
					bool flag44 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe;
					if (flag44)
					{
						Vector3 value7 = this.SelectedKeyframe.GetSetting<Vector3>("Position").Value;
						Vector3 vector2 = vector - value7;
						bool flag45 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Actor;
						if (flag45)
						{
							this.ActiveActor.Track.OffsetPositions(vector2);
						}
						else
						{
							this.ActiveScene.OffsetOrigin(this.ActiveScene.Origin + vector2);
						}
					}
				}
			}
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x0010509C File Offset: 0x0010329C
		private void OnRotationChange(Vector3 rotation)
		{
			bool flag = this._onRotationChange != null;
			if (flag)
			{
				this._onRotationChange(rotation);
			}
			else
			{
				bool flag2 = this.EditMode == MachinimaModule.EditorMode.RotateHead;
				if (flag2)
				{
					this.SelectedKeyframe.GetSetting<Vector3>("Look").Value = new Vector3(rotation.X, rotation.Y, rotation.Z);
					bool flag3 = this.SelectedActor is EntityActor;
					if (flag3)
					{
						EntityActor entityActor = (EntityActor)this.SelectedActor;
						entityActor.GetEntity().LookOrientation.Z = rotation.Z;
					}
				}
				else
				{
					bool flag4 = this.EditMode == MachinimaModule.EditorMode.RotateBody;
					if (!flag4)
					{
						return;
					}
					this.SelectedKeyframe.GetSetting<Vector3>("Rotation").Value = new Vector3(rotation.X, rotation.Y, rotation.Z);
					bool flag5 = this.SelectedActor is EntityActor;
					if (flag5)
					{
					}
				}
				SceneActor selectedActor = this.SelectedActor;
				if (selectedActor != null)
				{
					selectedActor.Track.UpdatePositions();
				}
				MachinimaScene activeScene = this.ActiveScene;
				if (activeScene != null)
				{
					activeScene.Update(this.CurrentFrame);
				}
			}
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x001051C8 File Offset: 0x001033C8
		private void OnPositionChange(Vector3 position)
		{
			bool flag = this.EditMode == MachinimaModule.EditorMode.Translate;
			if (flag)
			{
				bool flag2 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe;
				if (flag2)
				{
					this.SelectedKeyframe.GetSetting<Vector3>("Position").Value = position;
					this.SelectedActor.Track.UpdatePositions();
				}
				else
				{
					KeyframeSetting<Vector3[]> setting = this._selectedGrip.Keyframe.GetSetting<Vector3[]>("Curve");
					Vector3 value = this._selectedGrip.Keyframe.GetSetting<Vector3>("Position").Value;
					Vector3[] value2 = setting.Value;
					value2[this._selectedGrip.Index] = position - value;
					setting.Value = value2;
					this._selectedGrip.Actor.Track.UpdatePositions();
				}
			}
			else
			{
				bool flag3 = this.EditMode == MachinimaModule.EditorMode.FreeMove;
				if (flag3)
				{
					bool flag4 = this._selectedNodeType == MachinimaModule.NodeType.Keyframe && this.SelectedKeyframe != null;
					if (flag4)
					{
						this.SelectedKeyframe.GetSetting<Vector3>("Position").Value = position;
						this.SelectedActor.Track.UpdatePositions();
					}
					else
					{
						bool flag5 = this._selectedNodeType == MachinimaModule.NodeType.CurveHandle && this._selectedGrip != null;
						if (flag5)
						{
							Vector3 value3 = this._selectedGrip.Keyframe.GetSetting<Vector3>("Position").Value;
							this._selectedGrip.Keyframe.GetSetting<Vector3[]>("Curve").Value[this._selectedGrip.Index] = position - value3;
							this._selectedGrip.Actor.Track.UpdatePositions();
						}
					}
				}
			}
			MachinimaScene activeScene = this.ActiveScene;
			if (activeScene != null)
			{
				activeScene.Update(this.CurrentFrame);
			}
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x00105390 File Offset: 0x00103590
		private bool CheckKeyframeHover()
		{
			this.HoveredKeyframe = null;
			this._hoveredGrip = null;
			this.BodyRotateHover = false;
			bool flag = this.SelectedKeyframe != null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
				float num = float.NaN;
				Dictionary<string, MachinimaScene> scenes = this.GetScenes();
				bool flag2 = this.ActiveScene == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					List<SceneActor> actors = this.ActiveScene.GetActors();
					for (int i = 0; i < actors.Count; i++)
					{
						SceneTrack track = actors[i].Track;
						for (int j = 0; j < track.Keyframes.Count; j++)
						{
							TrackKeyframe trackKeyframe = track.Keyframes[j];
							KeyframeSetting<Vector3> setting = trackKeyframe.GetSetting<Vector3>("Position");
							bool flag3 = setting == null;
							if (!flag3)
							{
								Vector3 value = setting.Value;
								BoundingBox box = TrackKeyframe.KeyframeBox;
								box.Translate(value);
								HitDetection.RayBoxCollision rayBoxCollision;
								bool flag4 = this.ShowPathNodes && HitDetection.CheckRayBoxCollision(box, lookRay.Position, lookRay.Direction, out rayBoxCollision, false);
								if (flag4)
								{
									float num2 = Vector3.Distance(lookRay.Position, rayBoxCollision.Position);
									bool flag5 = float.IsNaN(num) || num2 < num;
									if (flag5)
									{
										this.HoveredKeyframe = trackKeyframe;
										this.HoveredActor = track.Parent;
										num = num2;
									}
								}
								bool flag6 = track.Parent is EntityActor && !(track.Parent is ItemActor);
								if (flag6)
								{
									box.Translate(new Vector3(0f, -TrackKeyframe.KeyframeBox.GetSize().Y, 0f));
									bool flag7 = HitDetection.CheckRayBoxCollision(box, lookRay.Position, lookRay.Direction, out rayBoxCollision, false);
									if (flag7)
									{
										float num3 = Vector3.Distance(lookRay.Position, rayBoxCollision.Position);
										bool flag8 = float.IsNaN(num) || num3 < num;
										if (flag8)
										{
											this.HoveredKeyframe = trackKeyframe;
											this.HoveredActor = track.Parent;
											num = num3;
											this.BodyRotateHover = true;
										}
									}
								}
								bool flag9 = track.PathType == SceneTrack.TrackPathType.Bezier && track.Parent == this.ActiveActor;
								if (flag9)
								{
									KeyframeSetting<Vector3[]> setting2 = trackKeyframe.GetSetting<Vector3[]>("Curve");
									bool flag10 = setting2 == null;
									if (!flag10)
									{
										Vector3[] value2 = setting2.Value;
										for (int k = 0; k < value2.Length; k++)
										{
											box = TrackKeyframe.PathBox;
											box.Translate(value2[k] + value);
											bool flag11 = HitDetection.CheckRayBoxCollision(box, lookRay.Position, lookRay.Direction, out rayBoxCollision, false);
											if (flag11)
											{
												float num4 = Vector3.Distance(lookRay.Position, rayBoxCollision.Position);
												bool flag12 = float.IsNaN(num) || num4 < num;
												if (flag12)
												{
													int previousKeyframe = this.ActiveActor.Track.GetPreviousKeyframe(trackKeyframe.Frame - 0.01f);
													int nextKeyframe = this.ActiveActor.Track.GetNextKeyframe(trackKeyframe.Frame + 0.01f);
													TrackKeyframe prevKeyframe = (k == 0 && trackKeyframe.Frame > 0f && previousKeyframe != -1) ? this.ActiveActor.Track.Keyframes[previousKeyframe] : null;
													TrackKeyframe nextKeyframe2 = (k == 1 && nextKeyframe != -1 && nextKeyframe < this.ActiveActor.Track.Keyframes.Count - 1) ? this.ActiveActor.Track.Keyframes[nextKeyframe] : null;
													this._hoveredGrip = new MachinimaModule.CurveHandle(this.ActiveActor, trackKeyframe, k, prevKeyframe, nextKeyframe2);
												}
											}
										}
									}
								}
							}
						}
						num = float.NaN;
					}
					result = (this.HoveredKeyframe != null);
				}
			}
			return result;
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x001057A8 File Offset: 0x001039A8
		private void SelectKeyframe()
		{
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			bool flag = this._hoveredGrip != null;
			if (flag)
			{
				this._selectedGrip = this._hoveredGrip;
				this._selectedNodeType = MachinimaModule.NodeType.CurveHandle;
				Vector3 value = this._selectedGrip.Keyframe.GetSetting<Vector3>("Position").Value;
				Vector3 value2 = this._selectedGrip.Keyframe.GetSetting<Vector3[]>("Curve").Value[this._selectedGrip.Index];
				Vector3 vector = value + value2;
				this._targetDistance = Vector3.Distance(lookRay.Position, vector);
				this._lastKeyframePosition = vector;
				bool flag2 = this._gameInstance.Input.IsShiftHeld();
				if (flag2)
				{
					this._translationGizmo.Show(vector, Vector3.Zero, null);
					this.EditMode = MachinimaModule.EditorMode.Translate;
				}
				else
				{
					bool flag3 = this._gameInstance.Input.IsAltHeld();
					if (flag3)
					{
						this._selectedGrip.UpdateTangent = true;
						this.EditMode = MachinimaModule.EditorMode.FreeMove;
					}
					else
					{
						this.EditMode = MachinimaModule.EditorMode.FreeMove;
					}
				}
			}
			else
			{
				bool flag4 = this.HoveredKeyframe != null;
				if (flag4)
				{
					this.ActiveKeyframe = (this.SelectedKeyframe = this.HoveredKeyframe);
					this.ActiveActor = (this.SelectedActor = this.HoveredActor);
					this._selectedNodeType = MachinimaModule.NodeType.Keyframe;
					Vector3 keyframePos = this.SelectedKeyframe.GetSetting<Vector3>("Position").Value;
					this._targetDistance = Vector3.Distance(lookRay.Position, keyframePos);
					this._lastKeyframePosition = keyframePos;
					Vector3 value3 = this.SelectedKeyframe.GetSetting<Vector3>("Look").Value;
					Vector3 value4 = this.SelectedKeyframe.GetSetting<Vector3>("Rotation").Value;
					bool flag5 = this._gameInstance.Input.IsAltHeld() || this._gameInstance.Input.IsShiftHeld();
					if (flag5)
					{
						bool flag6 = this._gameInstance.Input.IsAltHeld();
						if (flag6)
						{
							this.EditMode = MachinimaModule.EditorMode.RotateBody;
							bool flag7 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Scene;
							if (flag7)
							{
								Vector3 currentRotation = Vector3.Zero;
								this._rotationGizmo.Show(keyframePos, new Vector3?(currentRotation), delegate(Vector3 newRotation)
								{
									Vector3 rotation = newRotation - currentRotation;
									this.ActiveScene.Rotate(rotation, keyframePos);
									currentRotation = newRotation;
									this.UpdateFrame(0L, true);
									bool flag14 = !this._rotationGizmo.Visible;
									if (flag14)
									{
										this.EditMode = MachinimaModule.EditorMode.None;
									}
								}, null);
							}
							else
							{
								bool flag8 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Actor;
								if (flag8)
								{
									Vector3 currentRotation = Vector3.Zero;
									this._rotationGizmo.Show(keyframePos, new Vector3?(currentRotation), delegate(Vector3 newRotation)
									{
										Vector3 rotation = newRotation - currentRotation;
										this.ActiveActor.Track.RotatePath(rotation, keyframePos);
										currentRotation = newRotation;
										bool flag14 = !this._rotationGizmo.Visible;
										if (flag14)
										{
											this.EditMode = MachinimaModule.EditorMode.None;
										}
									}, null);
								}
								else
								{
									bool flag9 = this.ActiveActor is ItemActor || this.BodyRotateHover;
									if (flag9)
									{
										this.EditMode = MachinimaModule.EditorMode.RotateBody;
										this._rotationGizmo.Show(keyframePos, new Vector3?(new Vector3(value4.X, value4.Y, value4.Z)), new RotationGizmo.OnRotationChange(this.OnRotationChange), null);
									}
									else
									{
										this.EditMode = MachinimaModule.EditorMode.RotateHead;
										this._rotationGizmo.Show(keyframePos, new Vector3?(new Vector3(value3.X, value3.Y, 0f)), new RotationGizmo.OnRotationChange(this.OnRotationChange), new Vector3?(new Vector3(0f, value4.Y, 0f)));
									}
								}
							}
						}
						else
						{
							bool flag10 = this._gameInstance.Input.IsShiftHeld();
							if (flag10)
							{
								this.EditMode = MachinimaModule.EditorMode.RotateBody;
								bool flag11 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Scene;
								if (flag11)
								{
									Vector3 currentPosition = keyframePos;
									this._translationGizmo.Show(currentPosition, Vector3.Zero, delegate(Vector3 newPosition)
									{
										Vector3 value5 = newPosition - currentPosition;
										this.ActiveScene.OffsetOrigin(this.ActiveScene.Origin + value5);
										currentPosition = newPosition;
										bool flag14 = !this._translationGizmo.Visible;
										if (flag14)
										{
											this.EditMode = MachinimaModule.EditorMode.None;
										}
									});
								}
								else
								{
									bool flag12 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Actor;
									if (flag12)
									{
										Vector3 currentPosition = keyframePos;
										this._translationGizmo.Show(currentPosition, Vector3.Zero, delegate(Vector3 newPosition)
										{
											Vector3 offset = newPosition - currentPosition;
											this.ActiveActor.Track.OffsetPositions(offset);
											currentPosition = newPosition;
											bool flag14 = !this._translationGizmo.Visible;
											if (flag14)
											{
												this.EditMode = MachinimaModule.EditorMode.None;
											}
										});
									}
									else
									{
										this._translationGizmo.Show(keyframePos, new Vector3(value3.X, value3.Y, 0f), new TranslationGizmo.OnPositionChange(this.OnPositionChange));
									}
								}
								this.EditMode = MachinimaModule.EditorMode.Translate;
							}
						}
					}
					else
					{
						this.EditMode = MachinimaModule.EditorMode.FreeMove;
					}
					bool flag13 = !this.Running && this.SelectedActor.Track.Keyframes.Count > 1;
					if (flag13)
					{
						this.CurrentFrame = this.SelectedKeyframe.Frame;
					}
				}
			}
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x00105D00 File Offset: 0x00103F00
		private void CycleSelectionMode()
		{
			bool flag = this.SelectionMode == MachinimaModule.EditorSelectionMode.Keyframe;
			if (flag)
			{
				this.SelectionMode = MachinimaModule.EditorSelectionMode.Actor;
			}
			else
			{
				bool flag2 = this.SelectionMode == MachinimaModule.EditorSelectionMode.Actor;
				if (flag2)
				{
					this.SelectionMode = MachinimaModule.EditorSelectionMode.Scene;
				}
				else
				{
					this.SelectionMode = MachinimaModule.EditorSelectionMode.Keyframe;
				}
			}
			this.ResetEditing();
			this._gameInstance.Chat.Log(string.Format("SelectionMode mode set to {0}", this.SelectionMode));
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00105D71 File Offset: 0x00103F71
		private void ResetEditing()
		{
			this.EditMode = MachinimaModule.EditorMode.None;
			this._translationGizmo.Hide();
			this._rotationGizmo.Hide();
			this.SelectedKeyframe = null;
			this.SelectedActor = null;
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x00105DA4 File Offset: 0x00103FA4
		private bool HasActiveTool()
		{
			BuilderToolsModule builderToolsModule = this._gameInstance.BuilderToolsModule;
			object obj;
			if (builderToolsModule == null)
			{
				obj = null;
			}
			else
			{
				ToolInstance activeTool = builderToolsModule.ActiveTool;
				obj = ((activeTool != null) ? activeTool.ClientTool : null);
			}
			return obj is MachinimaTool;
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x00105DE4 File Offset: 0x00103FE4
		private void GiveMachinimaTool()
		{
			Item item = new ClientItemStack(new Item("EditorTool_Machinima", 1, 1.0, 1.0, false, new sbyte[0])).ToItemPacket(true);
			int hotbarActiveSlot = this._gameInstance.InventoryModule.HotbarActiveSlot;
			this._gameInstance.Connection.SendPacket(new SetCreativeItem(new InventoryPosition(-1, hotbarActiveSlot, item), false));
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x00105E54 File Offset: 0x00104054
		private EntityActor GetActorFromEntity(Entity entity)
		{
			bool flag = entity == null || !entity.IsLocalEntity;
			EntityActor result;
			if (flag)
			{
				result = null;
			}
			else
			{
				foreach (MachinimaScene machinimaScene in this._scenes.Values)
				{
					foreach (SceneActor sceneActor in machinimaScene.Actors)
					{
						bool flag2 = sceneActor is EntityActor && (sceneActor as EntityActor).GetEntity() == entity;
						if (flag2)
						{
							return sceneActor as EntityActor;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x00105F38 File Offset: 0x00104138
		private void RegisterEvents()
		{
			WebView webView = this._gameInstance.EditorWebViewModule.WebView;
			webView.RegisterForEvent<bool>("machinima.setRunning", this, new Action<bool>(this.OnSetRunning));
			webView.RegisterForEvent<bool>("machinima.setLoop", this, new Action<bool>(this.OnSetLoop));
			webView.RegisterForEvent<float>("machinima.setCurrentFrame", this, new Action<float>(this.OnSetCurrentFrame));
			webView.RegisterForEvent<string>("machinima.selectScene", this, new Action<string>(this.OnSelectScene));
			webView.RegisterForEvent<string>("machinima.deleteScene", this, new Action<string>(this.OnDeleteScene));
			webView.RegisterForEvent("machinima.saveScene", this, new Action(this.OnSaveScene));
			webView.RegisterForEvent<string>("machinima.addScene", this, new Action<string>(this.OnAddScene));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.moveKeyframe", this, new Action<MachinimaModule.KeyframeEvent>(this.OnMoveKeyframe));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.deleteKeyframe", this, new Action<MachinimaModule.KeyframeEvent>(this.OnDeleteKeyframe));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.addKeyframe", this, new Action<MachinimaModule.KeyframeEvent>(this.OnAddKeyframe));
			webView.RegisterForEvent<MachinimaModule.KeyframeSettingEvent>("machinima.setKeyframeSetting", this, new Action<MachinimaModule.KeyframeSettingEvent>(this.OnSetKeyframeSetting));
			webView.RegisterForEvent<MachinimaModule.KeyframeSettingEvent>("machinima.removeKeyframeSetting", this, new Action<MachinimaModule.KeyframeSettingEvent>(this.OnRemoveKeyframeSetting));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.setActorVisibility", this, new Action<MachinimaModule.KeyframeEvent>(this.OnSetActorVisibility));
			webView.RegisterForEvent<int>("machinima.deleteActor", this, new Action<int>(this.OnDeleteActor));
			webView.RegisterForEvent<int>("machinima.addActor", this, new Action<int>(this.OnAddActor));
			webView.RegisterForEvent<int>("machinima.duplicateActor", this, new Action<int>(this.OnDuplicateActor));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.selectKeyframe", this, new Action<MachinimaModule.KeyframeEvent>(this.OnSelectKeyframe));
			webView.RegisterForEvent("machinima.deselectKeyframe", this, new Action(this.OnDeselectKeyframe));
			webView.RegisterForEvent<int, string>("machinima.setActorModel", this, new Action<int, string>(this.OnSetActorModel));
			webView.RegisterForEvent<int, string>("machinima.setActorItem", this, new Action<int, string>(this.OnSetActorItem));
			webView.RegisterForEvent<int>("machinima.setActorModelToLocalPlayer", this, new Action<int>(this.OnSetActorModelToLocalPlayer));
			webView.RegisterForEvent<int, string>("machinima.updateActor", this, new Action<int, string>(this.OnUpdateActor));
			webView.RegisterForEvent<MachinimaModule.KeyframeEventEvent>("machinima.addKeyframeEvent", this, new Action<MachinimaModule.KeyframeEventEvent>(this.OnAddKeyframeEvent));
			webView.RegisterForEvent<MachinimaModule.KeyframeEventEvent>("machinima.updateKeyframeEvent", this, new Action<MachinimaModule.KeyframeEventEvent>(this.OnUpdateKeyframeEvent));
			webView.RegisterForEvent<MachinimaModule.KeyframeEventEvent>("machinima.deleteKeyframeEvent", this, new Action<MachinimaModule.KeyframeEventEvent>(this.OnDeleteKeyframeEvent));
			webView.RegisterForEvent<int>("machinima.selectCamera", this, new Action<int>(this.OnSelectCamera));
			webView.RegisterForEvent<bool>("machinima.setIsInterfaceOpen", this, new Action<bool>(this.OnSetIsInterfaceOpen));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.setKeyframeClipboard", this, new Action<MachinimaModule.KeyframeEvent>(this.OnSetKeyframeClipboard));
			webView.RegisterForEvent<MachinimaModule.KeyframeEvent>("machinima.pasteKeyframe", this, new Action<MachinimaModule.KeyframeEvent>(this.OnPasteKeyframe));
			webView.RegisterForEvent("machinima.openAssetEditor", this, new Action(this.OnOpenAssetEditor));
			webView.RegisterForEvent<string, string>("machinima.openAssetEditorWith", this, new Action<string, string>(this.OnOpenAssetEditorWith));
			webView.RegisterForEvent<int>("settings.machinimaEditorSettings.setNewKeyframeFrameOffset", this, new Action<int>(this.OnSetNewKeyframeFrameOffset));
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x00106278 File Offset: 0x00104478
		private void UnregisterEvents()
		{
			WebView webView = this._gameInstance.EditorWebViewModule.WebView;
			webView.UnregisterFromEvent("machinima.setRunning");
			webView.UnregisterFromEvent("machinima.setLoop");
			webView.UnregisterFromEvent("machinima.setCurrentFrame");
			webView.UnregisterFromEvent("machinima.selectScene");
			webView.UnregisterFromEvent("machinima.deleteScene");
			webView.UnregisterFromEvent("machinima.saveScene");
			webView.UnregisterFromEvent("machinima.addScene");
			webView.UnregisterFromEvent("machinima.moveKeyframe");
			webView.UnregisterFromEvent("machinima.deleteKeyframe");
			webView.UnregisterFromEvent("machinima.addKeyframe");
			webView.UnregisterFromEvent("machinima.setKeyframeSetting");
			webView.UnregisterFromEvent("machinima.removeKeyframeSetting");
			webView.UnregisterFromEvent("machinima.setActorVisibility");
			webView.UnregisterFromEvent("machinima.deleteActor");
			webView.UnregisterFromEvent("machinima.addActor");
			webView.UnregisterFromEvent("machinima.duplicateActor");
			webView.UnregisterFromEvent("machinima.selectKeyframe");
			webView.UnregisterFromEvent("machinima.deselectKeyframe");
			webView.UnregisterFromEvent("machinima.setActorModel");
			webView.UnregisterFromEvent("machinima.setActorItem");
			webView.UnregisterFromEvent("machinima.setActorModelToLocalPlayer");
			webView.UnregisterFromEvent("machinima.updateActor");
			webView.UnregisterFromEvent("machinima.addKeyframeEvent");
			webView.UnregisterFromEvent("machinima.updateKeyframeEvent");
			webView.UnregisterFromEvent("machinima.deleteKeyframeEvent");
			webView.UnregisterFromEvent("machinima.selectCamera");
			webView.UnregisterFromEvent("machinima.setIsInterfaceOpen");
			webView.UnregisterFromEvent("machinima.setKeyframeClipboard");
			webView.UnregisterFromEvent("machinima.pasteKeyframe");
			webView.UnregisterFromEvent("machinima.openAssetEditor");
			webView.UnregisterFromEvent("machinima.openAssetEditorWith");
			webView.UnregisterFromEvent("settings.machinimaEditorSettings.setNewKeyframeFrameOffset");
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x00106418 File Offset: 0x00104618
		private void OnSetNewKeyframeFrameOffset(int offset)
		{
			Settings settings = this._gameInstance.App.Settings.Clone();
			settings.MachinimaEditorSettings.NewKeyframeFrameOffset = offset;
			this._gameInstance.App.ApplyNewSettings(settings);
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x0010645C File Offset: 0x0010465C
		private void OnOpenAssetEditor()
		{
			bool flag = this._gameInstance.App.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._gameInstance.App.InGame.OpenAssetEditor();
			}
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0010649C File Offset: 0x0010469C
		private void OnOpenAssetEditorWith(string assetType, string assetId)
		{
			bool flag = this._gameInstance.App.Stage == App.AppStage.InGame;
			if (flag)
			{
				this._gameInstance.App.InGame.OpenAssetIdInAssetEditor(assetType, assetId);
			}
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x001064DC File Offset: 0x001046DC
		public void ShowInterface()
		{
			bool flag = this._gameInstance.App.Stage == App.AppStage.InGame;
			if (flag)
			{
				bool flag2 = !this._hasInterfaceLoaded;
				if (flag2)
				{
					bool flag3 = this.GetScenes().Count == 0;
					if (flag3)
					{
						this.LoadAllScenesFromFile();
						this.SetActiveScene(null);
					}
					this._hasInterfaceLoaded = true;
				}
				this.UpdateInterfaceData();
				this._gameInstance.App.InGame.SetCurrentOverlay(AppInGame.InGameOverlay.MachinimaEditor);
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0010655A File Offset: 0x0010475A
		private void UpdateCurrentFrameInInterface()
		{
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.currentFrameChanged", (int)this.CurrentFrame, null, null, null, null);
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x00106588 File Offset: 0x00104788
		private void UpdateInterfaceData()
		{
			Dictionary<string, MachinimaScene> dictionary = new Dictionary<string, MachinimaScene>();
			foreach (string text in this._scenes.Keys)
			{
				dictionary.Add(text, (text == this.ActiveScene.Name) ? this.ActiveScene : new MachinimaScene(this._gameInstance, text));
			}
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.scenesInitialized", dictionary, this.Running, this.AutoRestartScene, null, null);
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.keybindsInitialized", Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<MachinimaModule.Keybind, SDL.SDL_Scancode>, string>(this._keybinds, (KeyValuePair<MachinimaModule.Keybind, SDL.SDL_Scancode> k) => k.Key.ToString())), Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<MachinimaModule.Keybind, SDL.SDL_Scancode>, string>(this._keybinds, (KeyValuePair<MachinimaModule.Keybind, SDL.SDL_Scancode> k) => SDL.SDL_GetKeyName(SDL.SDL_GetKeyFromScancode(k.Value)))), null, null, null);
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x001066C4 File Offset: 0x001048C4
		private void OnSetIsInterfaceOpen(bool isOpen)
		{
			this._isInterfaceOpen = isOpen;
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x001066CE File Offset: 0x001048CE
		private void OnSetKeyframeClipboard(MachinimaModule.KeyframeEvent e)
		{
			this._keyframeClipboard = this.ActiveScene.GetActor(e.Actor).Track.GetKeyframe(e.Keyframe).Clone();
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x00106700 File Offset: 0x00104900
		private void OnPasteKeyframe(MachinimaModule.KeyframeEvent e)
		{
			int num = e.Frame;
			foreach (TrackKeyframe trackKeyframe in this.ActiveScene.GetActor(e.Actor).Track.Keyframes)
			{
				bool flag = trackKeyframe.Frame < (float)num;
				if (!flag)
				{
					bool flag2 = (int)trackKeyframe.Frame != num;
					if (flag2)
					{
						break;
					}
					num++;
				}
			}
			TrackKeyframe trackKeyframe2 = this._keyframeClipboard.Clone();
			trackKeyframe2.Frame = (float)num;
			this.ActiveScene.GetActor(e.Actor).Track.AddKeyframe(trackKeyframe2, true);
			this.CurrentFrame = (float)num;
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.keyframeAdded", e.Actor, trackKeyframe2, null, null, null);
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x00106800 File Offset: 0x00104A00
		private void OnSaveScene()
		{
			this.SaveSceneFile(this.ActiveScene, this._settings.CompressSaveFiles ? SceneDataType.CompressedFile : SceneDataType.JSONFile, null, "");
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x00106827 File Offset: 0x00104A27
		private void OnSetActorModel(int id, string modelId)
		{
			(this._activeScene.GetActor(id) as EntityActor).UpdateModel(this._gameInstance, modelId);
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00106848 File Offset: 0x00104A48
		private void OnSetActorItem(int id, string itemId)
		{
			(this._activeScene.GetActor(id) as ItemActor).SetItemId(itemId, this._gameInstance);
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00106869 File Offset: 0x00104A69
		private void OnSetActorModelToLocalPlayer(int id)
		{
			((EntityActor)this.ActiveScene.GetActor(id)).SetBaseModel(this._gameInstance.LocalPlayer.ModelPacket);
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x00106894 File Offset: 0x00104A94
		private void OnDuplicateActor(int actorId)
		{
			SceneActor sceneActor = this.ActiveScene.GetActor(actorId).Clone(this._gameInstance);
			sceneActor.Name = this.ActiveScene.GetNextActorName("Copy");
			this.ActiveScene.AddActor(sceneActor, false);
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.actorAdded", new Dictionary<string, SceneActor>
			{
				{
					sceneActor.Name,
					sceneActor
				}
			}, null, null, null, null);
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00106911 File Offset: 0x00104B11
		private void OnUpdateActor(int id, string name)
		{
			this.ActiveScene.GetActor(id).Name = name;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00106928 File Offset: 0x00104B28
		private void OnAddScene(string name)
		{
			MachinimaScene scene = new MachinimaScene(this._gameInstance, name);
			this.AddScene(scene, true);
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0010694C File Offset: 0x00104B4C
		private void OnDeleteScene(string name)
		{
			this.RemoveScene(name);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00106957 File Offset: 0x00104B57
		private void OnSelectScene(string name)
		{
			this.SetActiveScene(name);
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00106964 File Offset: 0x00104B64
		private void OnMoveKeyframe(MachinimaModule.KeyframeEvent e)
		{
			SceneActor actor = this.ActiveScene.GetActor(e.Actor);
			foreach (TrackKeyframe trackKeyframe in actor.Track.Keyframes)
			{
				bool flag = trackKeyframe.Frame == (float)e.Frame;
				if (flag)
				{
					return;
				}
			}
			actor.Track.GetKeyframe(e.Keyframe).Frame = (float)e.Frame;
			actor.Track.UpdateKeyframeData();
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00106A0C File Offset: 0x00104C0C
		private void OnDeleteKeyframe(MachinimaModule.KeyframeEvent e)
		{
			bool flag = this.ActiveScene.GetActor(e.Actor).Track.Keyframes.Count > 1;
			if (flag)
			{
				this.ActiveScene.GetActor(e.Actor).Track.RemoveKeyframe(this.ActiveScene.GetActor(e.Actor).Track.GetKeyframe(e.Keyframe).Frame);
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00106A88 File Offset: 0x00104C88
		private void OnSelectKeyframe(MachinimaModule.KeyframeEvent e)
		{
			this.ActiveActor = this.ActiveScene.GetActor(e.Actor);
			this.ActiveKeyframe = this.ActiveActor.Track.GetKeyframe(e.Keyframe);
			this.CurrentFrame = this.ActiveKeyframe.Frame;
			this.UpdateFrame(0L, true);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00106AE7 File Offset: 0x00104CE7
		private void OnDeselectKeyframe()
		{
			this.ActiveActor = null;
			this.ActiveKeyframe = null;
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00106AFC File Offset: 0x00104CFC
		private void OnAddKeyframe(MachinimaModule.KeyframeEvent e)
		{
			SceneTrack track = this.ActiveScene.GetActor(e.Actor).Track;
			int frame = e.Frame;
			foreach (TrackKeyframe trackKeyframe in track.Keyframes)
			{
				bool flag = trackKeyframe.Frame == (float)frame;
				if (flag)
				{
					return;
				}
			}
			bool flag2 = track.Keyframes.Count > 0 && (float)frame <= track.Keyframes[0].Frame;
			TrackKeyframe currentFrame;
			if (flag2)
			{
				currentFrame = track.GetCurrentFrame(track.Keyframes[0].Frame);
			}
			else
			{
				bool flag3 = (float)frame >= track.GetTrackLength();
				if (flag3)
				{
					currentFrame = track.GetCurrentFrame(track.GetTrackLength());
				}
				else
				{
					currentFrame = track.GetCurrentFrame((float)frame);
				}
			}
			currentFrame.Frame = (float)frame;
			track.AddKeyframe(currentFrame, true);
			this.CurrentFrame = (float)e.Frame;
			this.UpdateFrame(0L, true);
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.keyframeAdded", e.Actor, currentFrame, null, null, null);
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00106C50 File Offset: 0x00104E50
		private void OnSetKeyframeSetting(MachinimaModule.KeyframeSettingEvent e)
		{
			JObject jsonData = JObject.Parse(e.SettingValue);
			IKeyframeSetting setting = KeyframeSetting<object>.ConvertJsonObject(e.SettingName, jsonData);
			this.ActiveScene.GetActor(e.Actor).Track.GetKeyframe(e.Keyframe).AddSetting(setting);
			this.ActiveScene.GetActor(e.Actor).Track.UpdatePositions();
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x00106CBC File Offset: 0x00104EBC
		private void OnRemoveKeyframeSetting(MachinimaModule.KeyframeSettingEvent e)
		{
			this.ActiveScene.GetActor(e.Actor).Track.GetKeyframe(e.Keyframe).RemoveSetting(e.SettingName);
			this.ActiveScene.GetActor(e.Actor).Track.UpdatePositions();
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x00106D13 File Offset: 0x00104F13
		private void OnSetActorVisibility(MachinimaModule.KeyframeEvent e)
		{
			this.ActiveScene.GetActor(e.Actor).Visible = e.Visible;
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x00106D32 File Offset: 0x00104F32
		private void OnDeleteActor(int actor)
		{
			this.ActiveScene.RemoveActor(this.ActiveScene.GetActor(actor).Name);
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00106D54 File Offset: 0x00104F54
		private void OnAddActor(int objectTypeId)
		{
			string availableObjectName = this.GetAvailableObjectName((ActorType)objectTypeId);
			SceneActor sceneActor;
			switch (objectTypeId)
			{
			case 0:
				sceneActor = new ReferenceActor(this._gameInstance, availableObjectName);
				break;
			case 1:
				sceneActor = new CameraActor(this._gameInstance, availableObjectName);
				break;
			case 2:
				sceneActor = new PlayerActor(this._gameInstance, availableObjectName);
				break;
			case 3:
				sceneActor = new EntityActor(this._gameInstance, availableObjectName, null);
				((EntityActor)sceneActor).SetBaseModel(this._gameInstance.LocalPlayer.ModelPacket);
				break;
			case 4:
				sceneActor = new ItemActor(this._gameInstance, availableObjectName, null, "");
				break;
			default:
				return;
			}
			this.ActiveScene.AddActor(sceneActor, true);
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.actorAdded", new Dictionary<string, SceneActor>
			{
				{
					sceneActor.Name,
					sceneActor
				}
			}, null, null, null, null);
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x00106E40 File Offset: 0x00105040
		private void OnSetRunning(bool running)
		{
			this.Running = running;
			bool running2 = this.Running;
			if (running2)
			{
				this._lastFrameTick = this.GetCurrentTime();
			}
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x00106E6C File Offset: 0x0010506C
		private void OnSetCurrentFrame(float frame)
		{
			this.CurrentFrame = frame;
			this.ActiveScene.Update(this.CurrentFrame);
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x00106E89 File Offset: 0x00105089
		private void OnSetLoop(bool loop)
		{
			this.AutoRestartScene = loop;
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x00106E94 File Offset: 0x00105094
		private string GetAvailableObjectName(ActorType objectType)
		{
			string text = "Object";
			switch (objectType)
			{
			case ActorType.Reference:
				text = "Reference";
				break;
			case ActorType.Camera:
				text = "Camera";
				break;
			case ActorType.Player:
				text = "Player";
				break;
			case ActorType.Entity:
				text = "Entity";
				break;
			case ActorType.Item:
				text = "Item";
				break;
			}
			Regex regex = new Regex("^" + text + " ([0-9]+)+$", 1);
			int num = 0;
			foreach (SceneActor sceneActor in this.ActiveScene.Actors)
			{
				bool flag = sceneActor.Type == objectType;
				if (flag)
				{
					Match match = regex.Match(sceneActor.Name);
					bool success = match.Success;
					if (success)
					{
						int num2 = int.Parse(match.Groups[1].Value);
						bool flag2 = num2 > num;
						if (flag2)
						{
							num = num2;
						}
					}
				}
			}
			return text + " " + (num + 1).ToString();
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x00106FCC File Offset: 0x001051CC
		private void OnAddKeyframeEvent(MachinimaModule.KeyframeEventEvent evt)
		{
			this.UpdateKeyframeEvent(evt, true);
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x00106FD8 File Offset: 0x001051D8
		private void OnUpdateKeyframeEvent(MachinimaModule.KeyframeEventEvent evt)
		{
			this.UpdateKeyframeEvent(evt, false);
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x00106FE4 File Offset: 0x001051E4
		private void UpdateKeyframeEvent(MachinimaModule.KeyframeEventEvent evt, bool insert)
		{
			JObject jsonData = JObject.Parse(evt.Options);
			HytaleClient.InGame.Modules.Machinima.Events.KeyframeEvent keyframeEvent = HytaleClient.InGame.Modules.Machinima.Events.KeyframeEvent.ConvertJsonObject(jsonData);
			bool flag = keyframeEvent == null;
			if (flag)
			{
				throw new Exception("unknown keyframe event type");
			}
			keyframeEvent.Initialize(this.ActiveScene);
			TrackKeyframe keyframe = this.ActiveScene.GetActor(evt.Actor).Track.GetKeyframe(evt.Keyframe);
			if (insert)
			{
				keyframe.AddEvent(keyframeEvent);
			}
			else
			{
				keyframe.Events[keyframe.Events.IndexOf(keyframe.GetEvent(evt.Event))] = keyframeEvent;
			}
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.keyframeEventAdded", evt.Actor, evt.Keyframe, evt.Event, keyframeEvent.ToCoherentJson(), null);
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x001070C4 File Offset: 0x001052C4
		private void OnDeleteKeyframeEvent(MachinimaModule.KeyframeEventEvent evt)
		{
			TrackKeyframe keyframe = this.ActiveScene.GetActor(evt.Actor).Track.GetKeyframe(evt.Keyframe);
			keyframe.Events.Remove(keyframe.GetEvent(evt.Event));
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.keyframeEventDeleted", evt.Actor, evt.Keyframe, evt.Event, null, null);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x0010714C File Offset: 0x0010534C
		private void OnSelectCamera(int actorId)
		{
			CameraActor cameraActor = (CameraActor)this.ActiveScene.GetActor(actorId);
			cameraActor.SetState(!cameraActor.Active);
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06004670 RID: 18032 RVA: 0x0010717C File Offset: 0x0010537C
		// (set) Token: 0x06004671 RID: 18033 RVA: 0x00107194 File Offset: 0x00105394
		public MachinimaScene ActiveScene
		{
			get
			{
				return this._activeScene;
			}
			set
			{
				bool flag = this._activeScene == value;
				if (!flag)
				{
					bool flag2 = this._activeScene != null;
					if (flag2)
					{
						this.Autosave(true);
						this._activeScene.IsActive = false;
					}
					bool flag3 = value != null && !this._scenes.ContainsKey(value.Name);
					if (flag3)
					{
						this.AddScene(value, false);
					}
					this._activeScene = value;
					bool flag4 = this._activeScene != null;
					if (flag4)
					{
						this._activeScene.IsActive = true;
					}
					this._nextAutosaveTick = 0L;
					this.ResetScene(true);
				}
			}
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x00107230 File Offset: 0x00105430
		private bool AddScene(MachinimaScene scene, bool makeActive = false)
		{
			bool flag = this._scenes.ContainsKey(scene.Name);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._scenes.Add(scene.Name, scene);
				if (makeActive)
				{
					this.SetActiveScene(scene.Name);
				}
				this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.sceneAdded", scene, null, null, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x001072A4 File Offset: 0x001054A4
		private bool RemoveScene(string sceneName)
		{
			bool flag = !this._scenes.ContainsKey(sceneName);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.ActiveScene != null && this.ActiveScene.Name == sceneName;
				if (flag2)
				{
					this.ResetScene(false);
					this.ActiveScene = null;
				}
				this._scenes[sceneName].Dispose();
				this._scenes.Remove(sceneName);
				this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.sceneDeleted", sceneName, null, null, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x00107340 File Offset: 0x00105540
		private void ClearScenes()
		{
			this.ResetScene(false);
			this.ActiveScene = null;
			foreach (MachinimaScene machinimaScene in this._scenes.Values)
			{
				machinimaScene.Dispose();
			}
			this._scenes.Clear();
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.scenesCleared", null, null, null, null, null);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x001073DC File Offset: 0x001055DC
		private void ListScenes()
		{
			MachinimaScene activeScene = this.ActiveScene;
			string b = ((activeScene != null) ? activeScene.Name : null) ?? "";
			bool flag = this._scenes.Count == 0;
			if (flag)
			{
				this._gameInstance.Chat.Log("No scenes currently exist");
			}
			else
			{
				this._gameInstance.Chat.Log(string.Format("{0} Loaded Scenes:", this._scenes.Count));
				foreach (KeyValuePair<string, MachinimaScene> keyValuePair in this._scenes)
				{
					float num = 0f;
					foreach (SceneActor sceneActor in keyValuePair.Value.Actors)
					{
						bool flag2 = sceneActor.Track.GetTrackLength() > num;
						if (flag2)
						{
							num = sceneActor.Track.GetTrackLength();
						}
					}
					string text = string.Format("{0} Actors ({1} sec)", keyValuePair.Value.Actors.Count, Math.Round((double)(num / this.PlaybackFPS * 10f)) / 10.0);
					string text2 = (keyValuePair.Key == b) ? " - Active" : "";
					this._gameInstance.Chat.Log(string.Concat(new string[]
					{
						"'",
						keyValuePair.Key,
						"' - ",
						text,
						text2
					}));
				}
			}
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x001075D8 File Offset: 0x001057D8
		private void OnSceneEnd()
		{
			bool autoRestartScene = this.AutoRestartScene;
			if (autoRestartScene)
			{
				this.ResetScene(true);
			}
			bool continousPlayback = this._continousPlayback;
			if (continousPlayback)
			{
				this.Running = true;
			}
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x0010760C File Offset: 0x0010580C
		public MachinimaScene GetScene(string sceneName)
		{
			bool flag = this._scenes.ContainsKey(sceneName);
			MachinimaScene result;
			if (flag)
			{
				result = this._scenes[sceneName];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x00107640 File Offset: 0x00105840
		public Dictionary<string, MachinimaScene> GetScenes()
		{
			return this._scenes;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x00107658 File Offset: 0x00105858
		private void ResetScene(bool doUpdate = false)
		{
			this.Running = false;
			this.CurrentFrame = 0f;
			if (doUpdate)
			{
				this.UpdateFrame(0L, true);
			}
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x00107689 File Offset: 0x00105889
		private void EndScene()
		{
			this.CurrentFrame = ((this.ActiveScene == null) ? 0f : this.ActiveScene.GetSceneLength());
			this.UpdateFrame(0L, true);
			this.Running = false;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x001076C0 File Offset: 0x001058C0
		private void SetActiveScene(string sceneName = null)
		{
			bool flag = sceneName == null && this._scenes.Count > 0;
			if (flag)
			{
				Dictionary<string, MachinimaScene>.Enumerator enumerator = this._scenes.GetEnumerator();
				enumerator.MoveNext();
				KeyValuePair<string, MachinimaScene> keyValuePair = enumerator.Current;
				this.ActiveScene = keyValuePair.Value;
			}
			else
			{
				bool flag2 = sceneName != null && this._scenes.ContainsKey(sceneName);
				if (flag2)
				{
					this.ActiveScene = this._scenes[sceneName];
				}
			}
			string data = (this.ActiveScene == null) ? null : this.ActiveScene.Name;
			bool isInterfaceOpen = this._isInterfaceOpen;
			if (isInterfaceOpen)
			{
				this.UpdateInterfaceData();
			}
			this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.activeSceneChanged", data, null, null, null, null);
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x0010778C File Offset: 0x0010598C
		private string GetNextSceneName(string sceneName)
		{
			string text = sceneName;
			bool flag = string.IsNullOrWhiteSpace(text);
			if (flag)
			{
				text = "scene";
			}
			bool flag2 = this._scenes.ContainsKey(text);
			if (flag2)
			{
				for (int i = 1; i < 999999; i++)
				{
					string text2 = string.Format("{0}{1}", text, i);
					bool flag3 = !this._scenes.ContainsKey(text2);
					if (flag3)
					{
						return text2;
					}
				}
			}
			return text;
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00107810 File Offset: 0x00105A10
		private void Autosave(bool force = false)
		{
			bool flag = this.Running || this.ActiveScene == null || (this._nextAutosaveTick > this._lastFrameTick && !force);
			if (!flag)
			{
				int num = this._settings.AutosaveDelay * 1000;
				bool flag2 = this._nextAutosaveTick == 0L;
				if (flag2)
				{
					this._nextAutosaveTick = this._lastFrameTick + (long)num;
				}
				else
				{
					this.SaveSceneFile(this.ActiveScene, this._settings.CompressSaveFiles ? SceneDataType.CompressedFile : SceneDataType.JSONFile, null, MachinimaModule.AutosaveDirectory);
					this._nextAutosaveTick = this._lastFrameTick + (long)num;
					this._gameInstance.Notifications.AddNotification("Scene '" + this.ActiveScene.Name + "' autosaved to file.", null);
				}
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x001078E0 File Offset: 0x00105AE0
		private void SaveAllScenesToFile()
		{
			foreach (MachinimaScene scene in this._scenes.Values)
			{
				this.SaveSceneFile(scene, this._settings.CompressSaveFiles ? SceneDataType.CompressedFile : SceneDataType.JSONFile, null, "");
			}
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x00107958 File Offset: 0x00105B58
		private void LoadAllScenesFromFile()
		{
			bool flag = !Directory.Exists(MachinimaModule.SceneDirectory);
			if (!flag)
			{
				string[] files = Directory.GetFiles(MachinimaModule.SceneDirectory);
				int i = 0;
				while (i < files.Length)
				{
					string text = files[i];
					try
					{
						bool flag2 = !text.EndsWith(".json") && !text.EndsWith(".hms");
						if (!flag2)
						{
							this.LoadSceneFile(text, false, SceneDataType.CompressedFile);
						}
					}
					catch (Exception ex)
					{
						Trace.WriteLine("Failed to load machinima scene: " + text);
						Trace.WriteLine(ex);
					}
					IL_7D:
					i++;
					continue;
					goto IL_7D;
				}
			}
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x001079FC File Offset: 0x00105BFC
		private void SaveSceneFile(MachinimaScene scene, SceneDataType dataType, string filename = null, string path = "")
		{
			bool flag = string.IsNullOrWhiteSpace(filename);
			if (flag)
			{
				filename = scene.Name;
			}
			bool flag2 = string.IsNullOrWhiteSpace(path);
			if (flag2)
			{
				path = MachinimaModule.SceneDirectory;
			}
			switch (dataType)
			{
			case SceneDataType.JSONFile:
			{
				string text = Path.Combine(MachinimaModule.SceneDirectory, filename + ".json");
				string text2 = scene.Serialize(this._serializerSettings);
				try
				{
					File.WriteAllText(text, text2);
				}
				catch (Exception ex)
				{
					this._gameInstance.Chat.Log("Error saving data to file " + text + "! - " + ex.Message);
					Trace.WriteLine(ex);
				}
				break;
			}
			case SceneDataType.CompressedFile:
			{
				string text3 = Path.Combine(path, filename + ".hms");
				byte[] array = scene.ToCompressedByteArray(this._serializerSettings);
				try
				{
					File.WriteAllBytes(text3, array);
				}
				catch (Exception ex2)
				{
					this._gameInstance.Chat.Log("Error saving data to file " + text3 + "! - " + ex2.Message);
					Trace.WriteLine(ex2);
				}
				break;
			}
			case SceneDataType.Clipboard:
			{
				string text4 = scene.Serialize(this._serializerSettings);
				SDL.SDL_SetClipboardText(text4);
				break;
			}
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00107B5C File Offset: 0x00105D5C
		private MachinimaScene LoadSceneFile(string filename, bool updateInterface = true, SceneDataType dataType = SceneDataType.CompressedFile)
		{
			bool flag = filename.EndsWith(".json");
			if (flag)
			{
				dataType = SceneDataType.JSONFile;
			}
			else
			{
				bool flag2 = filename.EndsWith(".hms");
				if (flag2)
				{
					dataType = SceneDataType.CompressedFile;
				}
				else
				{
					filename += (this._settings.CompressSaveFiles ? ".hms" : ".json");
				}
			}
			bool flag3 = dataType != SceneDataType.Clipboard && !File.Exists(filename);
			MachinimaScene result;
			if (flag3)
			{
				this._gameInstance.Chat.Log("Unable to find file '" + filename + "'");
				result = null;
			}
			else
			{
				MachinimaScene machinimaScene = null;
				switch (dataType)
				{
				case SceneDataType.JSONFile:
					machinimaScene = MachinimaScene.Deserialize(File.ReadAllText(filename), this._gameInstance, this._serializerSettings);
					break;
				case SceneDataType.CompressedFile:
				{
					byte[] compressedByteArray;
					try
					{
						compressedByteArray = File.ReadAllBytes(filename);
					}
					catch (Exception ex)
					{
						this._gameInstance.Chat.Log("Error reading data from file " + filename + "! - " + ex.Message);
						Trace.WriteLine(ex);
						return null;
					}
					machinimaScene = MachinimaScene.FromCompressedByteArray(compressedByteArray, this._gameInstance, this._serializerSettings);
					bool flag4 = machinimaScene == null;
					if (flag4)
					{
						return null;
					}
					break;
				}
				case SceneDataType.Clipboard:
				{
					string jsonString = SDL.SDL_GetClipboardText();
					machinimaScene = MachinimaScene.Deserialize(jsonString, this._gameInstance, this._serializerSettings);
					break;
				}
				}
				bool flag5 = machinimaScene != null;
				if (flag5)
				{
					bool flag6 = this._scenes.ContainsKey(machinimaScene.Name);
					if (flag6)
					{
						this.RemoveScene(machinimaScene.Name);
					}
					this.AddScene(machinimaScene, false);
					if (updateInterface)
					{
						this._gameInstance.EditorWebViewModule.WebView.TriggerEvent("machinima.scenesInitialized", this._scenes, null, null, null, null);
					}
				}
				result = machinimaScene;
			}
			return result;
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00107D38 File Offset: 0x00105F38
		public void HandleSceneUpdatePacket(UpdateMachinimaScene packet)
		{
			byte[] compressedByteArray = Array.ConvertAll<sbyte, byte>(packet.Scene, (sbyte b) => (byte)b);
			MachinimaScene machinimaScene = MachinimaScene.FromCompressedByteArray(compressedByteArray, this._gameInstance, this._serializerSettings);
			this.ActiveScene = machinimaScene;
			this._gameInstance.Chat.Log("Recieved '" + machinimaScene.Name + "' scene update from " + packet.Player);
		}

		// Token: 0x04002350 RID: 9040
		private readonly Dictionary<string, GameInstance.Command> _subCommands = new Dictionary<string, GameInstance.Command>();

		// Token: 0x04002351 RID: 9041
		private readonly JsonSerializerSettings _serializerSettings;

		// Token: 0x04002352 RID: 9042
		private readonly MachinimaEditorSettings _settings;

		// Token: 0x04002353 RID: 9043
		private bool _running;

		// Token: 0x04002354 RID: 9044
		private bool _continousPlayback;

		// Token: 0x04002355 RID: 9045
		private bool _autoRestartScene = true;

		// Token: 0x04002357 RID: 9047
		private long _lastFrameTick;

		// Token: 0x04002358 RID: 9048
		private long _nextAutosaveTick;

		// Token: 0x04002359 RID: 9049
		private float _msTimePerFrame;

		// Token: 0x0400235A RID: 9050
		private const string MachinimaToolName = "EditorTool_Machinima";

		// Token: 0x0400235B RID: 9051
		private readonly HitDetection.RaycastOptions _toolRaycastOptions = new HitDetection.RaycastOptions
		{
			IgnoreFluids = true,
			CheckOversizedBoxes = true,
			CheckOnlyTangibleEntities = false
		};

		// Token: 0x0400235C RID: 9052
		private readonly RotationGizmo _rotationGizmo;

		// Token: 0x0400235D RID: 9053
		private readonly TranslationGizmo _translationGizmo;

		// Token: 0x0400235E RID: 9054
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x0400235F RID: 9055
		private readonly TextRenderer _textRenderer;

		// Token: 0x04002360 RID: 9056
		private readonly LineRenderer _curvePathRenderer;

		// Token: 0x04002361 RID: 9057
		private readonly MachinimaModule.Tooltip _tooltip;

		// Token: 0x0400236F RID: 9071
		private float _targetDistance;

		// Token: 0x04002370 RID: 9072
		private Vector3 _lastKeyframePosition = Vector3.Zero;

		// Token: 0x04002371 RID: 9073
		private MachinimaModule.CurveHandle _hoveredGrip;

		// Token: 0x04002372 RID: 9074
		private MachinimaModule.CurveHandle _selectedGrip;

		// Token: 0x04002373 RID: 9075
		private MachinimaModule.NodeType _selectedNodeType = MachinimaModule.NodeType.Keyframe;

		// Token: 0x04002374 RID: 9076
		private Dictionary<MachinimaModule.Keybind, SDL.SDL_Scancode> _keybinds = new Dictionary<MachinimaModule.Keybind, SDL.SDL_Scancode>
		{
			{
				MachinimaModule.Keybind.TogglePause,
				SDL.SDL_Scancode.SDL_SCANCODE_P
			},
			{
				MachinimaModule.Keybind.ToggleDisplay,
				SDL.SDL_Scancode.SDL_SCANCODE_O
			},
			{
				MachinimaModule.Keybind.ToggleCamera,
				SDL.SDL_Scancode.SDL_SCANCODE_C
			},
			{
				MachinimaModule.Keybind.RestartScene,
				SDL.SDL_Scancode.SDL_SCANCODE_R
			},
			{
				MachinimaModule.Keybind.FrameDecrement,
				SDL.SDL_Scancode.SDL_SCANCODE_LEFT
			},
			{
				MachinimaModule.Keybind.FrameIncrement,
				SDL.SDL_Scancode.SDL_SCANCODE_RIGHT
			},
			{
				MachinimaModule.Keybind.KeyframeDecrement,
				SDL.SDL_Scancode.SDL_SCANCODE_DOWN
			},
			{
				MachinimaModule.Keybind.KeyframeIncrement,
				SDL.SDL_Scancode.SDL_SCANCODE_UP
			},
			{
				MachinimaModule.Keybind.FrameTimeDecrease,
				SDL.SDL_Scancode.SDL_SCANCODE_MINUS
			},
			{
				MachinimaModule.Keybind.FrameTimeIncrease,
				SDL.SDL_Scancode.SDL_SCANCODE_EQUALS
			},
			{
				MachinimaModule.Keybind.AddKeyframe,
				SDL.SDL_Scancode.SDL_SCANCODE_K
			},
			{
				MachinimaModule.Keybind.RemoveKeyframe,
				SDL.SDL_Scancode.SDL_SCANCODE_DELETE
			},
			{
				MachinimaModule.Keybind.OriginAction,
				SDL.SDL_Scancode.SDL_SCANCODE_HOME
			},
			{
				MachinimaModule.Keybind.EditKeyframe,
				SDL.SDL_Scancode.SDL_SCANCODE_T
			},
			{
				MachinimaModule.Keybind.CycleSelectionMode,
				SDL.SDL_Scancode.SDL_SCANCODE_G
			}
		};

		// Token: 0x04002375 RID: 9077
		private RotationGizmo.OnRotationChange _onRotationChange = null;

		// Token: 0x04002376 RID: 9078
		private MachinimaModule.OnUseToolItem _onUseToolItem = null;

		// Token: 0x04002377 RID: 9079
		private bool _hasInterfaceLoaded;

		// Token: 0x04002378 RID: 9080
		private bool _isInterfaceOpen;

		// Token: 0x04002379 RID: 9081
		private TrackKeyframe _keyframeClipboard;

		// Token: 0x0400237A RID: 9082
		private static readonly string SceneDirectory = Path.Combine(Paths.UserData, "Scenes");

		// Token: 0x0400237B RID: 9083
		private static readonly string AutosaveDirectory = Path.Combine(MachinimaModule.SceneDirectory, "Autosave");

		// Token: 0x0400237C RID: 9084
		private static readonly string DemoSceneDirectory = "Tools/Machinima/DemoScenes";

		// Token: 0x0400237D RID: 9085
		private MachinimaScene _activeScene;

		// Token: 0x0400237E RID: 9086
		private Dictionary<string, MachinimaScene> _scenes = new Dictionary<string, MachinimaScene>();

		// Token: 0x02000DE7 RID: 3559
		// (Invoke) Token: 0x06006683 RID: 26243
		public delegate bool OnUseToolItem(InteractionType action);

		// Token: 0x02000DE8 RID: 3560
		private class CurveHandle
		{
			// Token: 0x06006686 RID: 26246 RVA: 0x002148A6 File Offset: 0x00212AA6
			public CurveHandle(SceneActor actor, TrackKeyframe keyframe, int index, TrackKeyframe prevKeyframe = null, TrackKeyframe nextKeyframe = null)
			{
				this.Actor = actor;
				this.Keyframe = keyframe;
				this.PrevKeyframe = prevKeyframe;
				this.NextKeyframe = nextKeyframe;
				this.Index = index;
			}

			// Token: 0x06006687 RID: 26247 RVA: 0x002148D8 File Offset: 0x00212AD8
			public bool Matches(SceneActor actor, TrackKeyframe keyframe, int index)
			{
				return this.Actor == actor && this.Keyframe == keyframe && this.Index == index;
			}

			// Token: 0x04004466 RID: 17510
			public readonly SceneActor Actor;

			// Token: 0x04004467 RID: 17511
			public readonly TrackKeyframe Keyframe;

			// Token: 0x04004468 RID: 17512
			public readonly TrackKeyframe PrevKeyframe;

			// Token: 0x04004469 RID: 17513
			public readonly TrackKeyframe NextKeyframe;

			// Token: 0x0400446A RID: 17514
			public readonly int Index;

			// Token: 0x0400446B RID: 17515
			public bool UpdateTangent;
		}

		// Token: 0x02000DE9 RID: 3561
		private class Tooltip : Disposable
		{
			// Token: 0x06006688 RID: 26248 RVA: 0x00214908 File Offset: 0x00212B08
			public Tooltip(GraphicsDevice graphics, Font font)
			{
				this._graphics = graphics;
				this._font = font;
				this._backgroundRenderer = new QuadRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram.AttribPosition, this._graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
				this._textRenderer = new TextRenderer(this._graphics, this._font, this.TooltipText, uint.MaxValue, 4278190080U);
			}

			// Token: 0x06006689 RID: 26249 RVA: 0x002149C7 File Offset: 0x00212BC7
			protected override void DoDispose()
			{
				this._textRenderer.Dispose();
				this._backgroundRenderer.Dispose();
			}

			// Token: 0x0600668A RID: 26250 RVA: 0x002149E4 File Offset: 0x00212BE4
			public void DrawBackground(ref Matrix viewProjectionMatrix)
			{
				BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
				basicProgram.AssertInUse();
				this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
				this.UpdateTooltip(ref viewProjectionMatrix);
				basicProgram.Color.SetValue(this._graphics.BlackColor);
				basicProgram.Opacity.SetValue(0.35f);
				basicProgram.MVPMatrix.SetValue(ref this._backgroundMatrix);
				this._backgroundRenderer.Draw();
				bool flag = this.Progress > 0f;
				if (flag)
				{
					basicProgram.Color.SetValue(this._graphics.WhiteColor);
					basicProgram.Opacity.SetValue(0.75f);
					basicProgram.MVPMatrix.SetValue(ref this._progressMatrix);
					this._backgroundRenderer.Draw();
				}
			}

			// Token: 0x0600668B RID: 26251 RVA: 0x00214AD8 File Offset: 0x00212CD8
			public void DrawText(ref Matrix viewProjectionMatrix)
			{
				TextProgram textProgram = this._graphics.GPUProgramStore.TextProgram;
				GLFunctions gl = this._graphics.GL;
				textProgram.AssertInUse();
				textProgram.FillThreshold.SetValue(0f);
				textProgram.FillBlurThreshold.SetValue(0.1f);
				textProgram.OutlineThreshold.SetValue(0f);
				textProgram.OutlineBlurThreshold.SetValue(0f);
				textProgram.OutlineOffset.SetValue(Vector2.Zero);
				textProgram.FogParams.SetValue(Vector4.Zero);
				textProgram.MVPMatrix.SetValue(ref this._textMatrix);
				gl.DepthFunc(GL.ALWAYS);
				this._textRenderer.Draw();
				gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
			}

			// Token: 0x0600668C RID: 26252 RVA: 0x00214BBC File Offset: 0x00212DBC
			public void UpdateOrthographicProjectionMatrix(int width, int height)
			{
				Matrix.CreateOrthographicOffCenter(0f, (float)width, 0f, (float)height, 0.1f, 1000f, out this._orthographicProjectionMatrix);
			}

			// Token: 0x0600668D RID: 26253 RVA: 0x00214BE4 File Offset: 0x00212DE4
			private void UpdateTooltip(ref Matrix viewProjectionMatrix)
			{
				float x = this.WindowSize.X;
				float y = this.WindowSize.Y;
				float num = 16f / (float)this._font.BaseSize;
				Vector3 position = this.Position;
				float num2 = 3f;
				this._textRenderer.Text = this.TooltipText;
				Vector2 screenPosition = this.ScreenPosition;
				Vector3 vector = new Vector3(-this._textRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Left), -this._textRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Top) - num2, -1f);
				Matrix.CreateTranslation(ref vector, out this._tempMatrix);
				Matrix.CreateScale(num, out this._matrix);
				Matrix.Multiply(ref this._tempMatrix, ref this._matrix, out this._matrix);
				Matrix.AddTranslation(ref this._matrix, screenPosition.X + num2, screenPosition.Y - num2, 0f);
				Matrix.Multiply(ref this._matrix, ref this._orthographicProjectionMatrix, out this._textMatrix);
				Vector3 vector2 = new Vector3(this._textRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Right) + num2 * 6f, this._textRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Top), 1f);
				Matrix.CreateScale(ref vector2, out this._matrix);
				Matrix.CreateScale(num, out this._tempMatrix);
				Matrix.Multiply(ref this._tempMatrix, ref this._matrix, out this._matrix);
				this._progressMatrix = this._matrix;
				Matrix.AddTranslation(ref this._matrix, screenPosition.X, screenPosition.Y - vector2.Y * num, 0f);
				Matrix.Multiply(ref this._matrix, ref this._orthographicProjectionMatrix, out this._backgroundMatrix);
				Matrix.CreateScale(this.Progress, -0.15f, 1f, out this._matrix);
				Matrix.Multiply(ref this._progressMatrix, ref this._matrix, out this._progressMatrix);
				Matrix.AddTranslation(ref this._progressMatrix, screenPosition.X, screenPosition.Y - vector2.Y * num, 0f);
				Matrix.Multiply(ref this._progressMatrix, ref this._orthographicProjectionMatrix, out this._progressMatrix);
			}

			// Token: 0x0600668E RID: 26254 RVA: 0x00214E00 File Offset: 0x00213000
			private static Vector2 WorldToScreenPos(ref Matrix viewProjectionMatrix, float viewWidth, float viewHeight, Vector3 worldPosition)
			{
				Matrix matrix = Matrix.CreateTranslation(worldPosition);
				Matrix.Multiply(ref matrix, ref viewProjectionMatrix, out matrix);
				Vector3 vector = matrix.Translation / matrix.M44;
				return new Vector2((vector.X / 2f + 0.5f) * viewWidth, (vector.Y / 2f + 0.5f) * viewHeight);
			}

			// Token: 0x0400446C RID: 17516
			private readonly GraphicsDevice _graphics;

			// Token: 0x0400446D RID: 17517
			private readonly Font _font;

			// Token: 0x0400446E RID: 17518
			private readonly TextRenderer _textRenderer;

			// Token: 0x0400446F RID: 17519
			private readonly QuadRenderer _backgroundRenderer;

			// Token: 0x04004470 RID: 17520
			private Matrix _tempMatrix;

			// Token: 0x04004471 RID: 17521
			private Matrix _matrix;

			// Token: 0x04004472 RID: 17522
			private Matrix _backgroundMatrix;

			// Token: 0x04004473 RID: 17523
			private Matrix _progressMatrix;

			// Token: 0x04004474 RID: 17524
			private Matrix _textMatrix;

			// Token: 0x04004475 RID: 17525
			private Matrix _orthographicProjectionMatrix;

			// Token: 0x04004476 RID: 17526
			public Vector2 WindowSize = Vector2.One;

			// Token: 0x04004477 RID: 17527
			public Vector3 Position = Vector3.Zero;

			// Token: 0x04004478 RID: 17528
			public Vector2 ScreenPosition = Vector2.NaN;

			// Token: 0x04004479 RID: 17529
			public float Progress = 0f;

			// Token: 0x0400447A RID: 17530
			public string TooltipText = "";

			// Token: 0x0400447B RID: 17531
			public bool IsVisible = true;
		}

		// Token: 0x02000DEA RID: 3562
		public enum EditorMode
		{
			// Token: 0x0400447D RID: 17533
			None,
			// Token: 0x0400447E RID: 17534
			FreeMove,
			// Token: 0x0400447F RID: 17535
			RotateHead,
			// Token: 0x04004480 RID: 17536
			RotateBody,
			// Token: 0x04004481 RID: 17537
			Translate
		}

		// Token: 0x02000DEB RID: 3563
		public enum EditorSelectionMode
		{
			// Token: 0x04004483 RID: 17539
			Keyframe,
			// Token: 0x04004484 RID: 17540
			Actor,
			// Token: 0x04004485 RID: 17541
			Scene
		}

		// Token: 0x02000DEC RID: 3564
		private enum NodeType
		{
			// Token: 0x04004487 RID: 17543
			Keyframe,
			// Token: 0x04004488 RID: 17544
			CurveHandle
		}

		// Token: 0x02000DED RID: 3565
		private enum Keybind
		{
			// Token: 0x0400448A RID: 17546
			TogglePause,
			// Token: 0x0400448B RID: 17547
			ToggleDisplay,
			// Token: 0x0400448C RID: 17548
			ToggleCamera,
			// Token: 0x0400448D RID: 17549
			RestartScene,
			// Token: 0x0400448E RID: 17550
			FrameIncrement,
			// Token: 0x0400448F RID: 17551
			FrameDecrement,
			// Token: 0x04004490 RID: 17552
			KeyframeIncrement,
			// Token: 0x04004491 RID: 17553
			KeyframeDecrement,
			// Token: 0x04004492 RID: 17554
			FrameTimeDecrease,
			// Token: 0x04004493 RID: 17555
			FrameTimeIncrease,
			// Token: 0x04004494 RID: 17556
			AddKeyframe,
			// Token: 0x04004495 RID: 17557
			RemoveKeyframe,
			// Token: 0x04004496 RID: 17558
			OriginAction,
			// Token: 0x04004497 RID: 17559
			EditKeyframe,
			// Token: 0x04004498 RID: 17560
			CycleSelectionMode
		}

		// Token: 0x02000DEE RID: 3566
		[CoherentType]
		public class KeyframeEvent
		{
			// Token: 0x04004499 RID: 17561
			[CoherentProperty("actor")]
			public int Actor;

			// Token: 0x0400449A RID: 17562
			[CoherentProperty("keyframe")]
			public int Keyframe;

			// Token: 0x0400449B RID: 17563
			[CoherentProperty("frame")]
			public int Frame;

			// Token: 0x0400449C RID: 17564
			[CoherentProperty("objectType")]
			public ActorType ObjectType;

			// Token: 0x0400449D RID: 17565
			[CoherentProperty("visible")]
			public bool Visible;
		}

		// Token: 0x02000DEF RID: 3567
		[CoherentType]
		public class KeyframeSettingEvent : MachinimaModule.KeyframeEvent
		{
			// Token: 0x0400449E RID: 17566
			[CoherentProperty("settingName")]
			public string SettingName;

			// Token: 0x0400449F RID: 17567
			[CoherentProperty("settingValue")]
			public string SettingValue;

			// Token: 0x040044A0 RID: 17568
			[CoherentProperty("settingType")]
			public string SettingType;
		}

		// Token: 0x02000DF0 RID: 3568
		[CoherentType]
		public class KeyframeEventEvent
		{
			// Token: 0x040044A1 RID: 17569
			[CoherentProperty("actor")]
			public int Actor;

			// Token: 0x040044A2 RID: 17570
			[CoherentProperty("keyframe")]
			public int Keyframe;

			// Token: 0x040044A3 RID: 17571
			[CoherentProperty("event")]
			public int Event = -1;

			// Token: 0x040044A4 RID: 17572
			[CoherentProperty("type")]
			public string Type;

			// Token: 0x040044A5 RID: 17573
			[CoherentProperty("options")]
			public string Options;
		}
	}
}
