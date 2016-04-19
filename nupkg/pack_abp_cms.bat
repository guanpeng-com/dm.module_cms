"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.CMS\Abp.CMS.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.CMS.EntityFramework\Abp.CMS.EntityFramework.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols
"..\src\.nuget\NuGet.exe" "pack" "..\src\Abp.CMS.NHibernate\Abp.CMS.NHibernate.csproj" -Properties Configuration=Release -IncludeReferencedProjects -Symbols
pause