using System;
using BCT.BuildScript.BaseProjects;
using BCT.Source;
using BCT.Source.Model;
using Microsoft.Win32;

namespace BCT.BuildScript.Projects
{
	public class EditorControls : BaseCSharpLibrary
	{
		public EditorControls( Workspace workSpace, PlatformType platform, Configuration configuration )
			: base( workSpace, platform, configuration )
		{
			rootNamespace = "EditorControls";

			DependsOn<EditorControlsLite>();
			DependsOn<ClientEditorManaged>();
			DependsOn<RenderCommonExport>();
			DependsOn<ClientEditorBridge>( true );
			DependsOn<EditedTerrain>( true );
			DependsOn<GameExport>( true );
			DependsOn<ClientEditorEngine>( true );

			AddProjectFiles();

			ReferenceAssembly( "ActiproSoftware.Docking.Wpf", @"%(VendorsDir)WPFControls\ActiproSoftware.Docking.Wpf.dll" );
			ReferenceAssembly( "ActiproSoftware.Shared.Wpf", @"%(VendorsDir)WPFControls\ActiproSoftware.Shared.Wpf.dll" );
			ReferenceAssembly( "ActiproSoftware.Themes.Office.Wpf", @"%(VendorsDir)WPFControls\ActiproSoftware.Themes.Office.Wpf.dll" );
			ReferenceAssembly( "GongSolutions.Wpf.DragDrop", @"%(VendorsDir)gong-wpf-dragdrop\GongSolutions.Wpf.DragDrop\bin\Release\NET4\GongSolutions.Wpf.DragDrop.dll" );
			ReferenceAssembly( "Hessiancsharp", @"%(VendorsDir)HessianCSharp\bin\Debug\Hessiancsharp.dll" );
			ReferenceAssembly( "MySql.Data", @"%(VendorsDir)MySQL\MySql.Data.dll" );
			ReferenceAssembly( "PropertyChangedNotificator", @"%(VendorsDir)PropertyChangedNotificator\Bin\PropertyChangedNotificator.dll" );
			ReferenceAssembly( "System.Windows.Interactivity", @"%(VendorsDir)Mvvm Light Toolkit\WPF4\System.Windows.Interactivity.dll" );
			ReferenceAssembly( "WPFToolkit.Extended", @"%(VendorsDir)WPFToolKit\WPFToolkit.Extended.dll" );
			ReferenceAssembly( "Microsoft.CSharp" );
			ReferenceAssembly( "System.Drawing" );
			ReferenceAssembly( "System.Data" );
			ReferenceAssembly( "System.Xaml" );
			ReferenceAssembly( "System.Xml" );
			ReferenceAssembly( "System.ServiceModel" );
			ReferenceAssembly( "System.Windows.Forms" );

			// EditorControls2 crap
			Exclude( "RemoteExporter.cs" );

			if ( workSpace.CheckFirstStart<EditorControls>() )
				RegisterActirpoLicenses();
		}

		private static void RegisterActirpoLicenses()
		{
			RegistryKey baseKey = RegistryKey.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Registry64 );

			const string path = @"Software\Wow6432Node\Actipro Software\WPF Controls\12.2";
			Tuple<string, string>[] vals =
			{
				Tuple.Create( "InstallDate", "28.02.2013" ),
				Tuple.Create( "Licensee", "Mail.ru Games LLC" ),
				Tuple.Create( "LicenseKey", "WPF122-CURA4-LPRN8-156NA-BQGG" ),
				Tuple.Create( "ResellerUrl", "https://www.actiprosoftware.com/purchase/cart" ),
				Tuple.Create( "UserName", "noname" ),
				Tuple.Create( "LicenseType", "Full Release" ),
			};

			var key = baseKey.OpenSubKey( path );
			if ( key != null )
			{
				bool f = true;
				foreach ( Tuple<string, string> q in vals )
					f &= string.Equals( key.GetValue( q.Item1 ), q.Item2 );

				if ( f )
					return;
			}

			try
			{
				if ( key != null )
					baseKey.DeleteSubKey( path );

				key = baseKey.CreateSubKey( path );
				if ( key == null )
				{
					Log.Error( "Project EditorControls: can't register Actipro Software licenses." );
					return;
				}

				foreach ( Tuple<string, string> q in vals )
					key.SetValue( q.Item1, q.Item2 );
			}
			catch ( UnauthorizedAccessException )
			{
				Log.Error( "Project EditorControls: can't register Actipro Software licenses. Need administrative privileges." );
			}
		}
	}
}