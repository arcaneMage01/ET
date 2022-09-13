using System;
using System.Diagnostics;
using System.Threading;
using CommandLine;
using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;
using Sirenix.Serialization;

namespace ET
{
	public class Init: MonoBehaviour
	{
		public static Init Instance;
		
		public GlobalConfig GlobalConfig;
		[LabelText("资源服地址")] public string HotfixResUrl = "http://127.0.0.1:8088";

		[LabelText("版本号")] public int Version = 2;
		
		[LabelText("资源模式")]
		public YooAssets.EPlayMode PlayMode = YooAssets.EPlayMode.EditorSimulateMode;
		
		private void Awake()
		{
			Instance = this;
			
			DontDestroyOnLoad(gameObject);
			
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};
				
			Game.AddSingleton<MainThreadSynchronizationContext>();

			// 命令行参数
			string[] args = "".Split(" ");
			Parser.Default.ParseArguments<Options>(args)
				.WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
				.WithParsed(Game.AddSingleton);
			
			Game.AddSingleton<TimeInfo>();
			Game.AddSingleton<Logger>().ILog = new UnityLogger();
			Game.AddSingleton<ObjectPool>();
			Game.AddSingleton<IdGenerater>();
			Game.AddSingleton<EventSystem>();
			Game.AddSingleton<TimerComponent>();
			Game.AddSingleton<CoroutineLockComponent>();
			
			ETTask.ExceptionHandler += Log.Error;

			FUIEntry.Init();
			if (PlayMode == YooAssets.EPlayMode.HostPlayMode)
			{
				FUI_CheckForResUpdateComponent.Init();	
			}

			Game.AddSingleton<CodeLoader>().Start();
		}

		private void Update()
		{
			Game.Update();
		}

		private void LateUpdate()
		{
			Game.LateUpdate();
			Game.FrameFinishUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
		
		// private async ETTask LoadAssetsAndHotfix()
		// {
		// 	// 启动YooAsset引擎，并在初始化完毕后进行热更代码加载
		// 	YooAssetProxy.StartYooAssetEngine(YooAssets.EPlayMode.HostPlayMode, () =>
		// 	{
		// 		// Shader Warm Up
		// 		ShaderVariantCollection shaderVariantCollection =
		// 				(await YooAssetProxy.LoadAssetAsync<ShaderVariantCollection>(
		// 					YooAssetProxy.GetYooAssetFormatResPath("ProjectSShaderVariant",
		// 						YooAssetProxy.YooAssetResType.Shader)))
		// 				.GetAssetObject<ShaderVariantCollection>();
		//
		// 		Stopwatch stopwatch = Stopwatch.StartNew();
		//
		// 		Log.Info(
		// 			$"开始Shader Warm Up, shaderCount: {shaderVariantCollection.shaderCount} variantCount: {shaderVariantCollection.variantCount}");
		//
		// 		shaderVariantCollection.WarmUp();
		//
		// 		stopwatch.Stop();
		//
		// 		Log.Info($"Shader Warm Up完成, 耗时: {stopwatch.ElapsedMilliseconds}ms");
		//
		// 		await LoadCode();
		//
		// 		if (PlayMode == YooAssets.EPlayMode.HostPlayMode)
		// 		{
		// 			FUI_CheckForResUpdateComponent.Release();
		// 		}
		// 	});
		// }
	}
}