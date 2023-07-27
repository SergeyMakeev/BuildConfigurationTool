using BCT.BuildScript.Projects;
using BCT.Source.Model;

namespace BCT.BuildScript.Solutions
{
	public class GAPI : SolutionFile
	{
		public GAPI()
		{
			AddConfiguration( Configuration.DEBUG );
			AddConfiguration( Configuration.FINALRELEASE );
			AddConfiguration( Configuration.PROFILE );
			AddConfiguration( Configuration.RELEASE );

			AddPlatform( PlatformType.Win32 );
			AddPlatform( PlatformType.Win64 );

			//рутовые проекты солюшена - все зависимости будут добавлены автоматически			
			Project<SandboxGAPI>();
			Project<DBCore>();
			Project<ShaderExport>();

			Project<RenderD3D9>();
		}
	}
}