set props=/property:Configuration=Release

\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %props% "%~dp0desktop-app.sln"
