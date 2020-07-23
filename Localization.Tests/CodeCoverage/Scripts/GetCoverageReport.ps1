#Coverlet: https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md
#ReportGenerator: https://danielpalme.github.io/ReportGenerator/usage.html

$CodeCoverageFolderPath = "Localization.Tests\CodeCoverage"
$AnalyzeFolderPath = $CodeCoverageFolderPath + "\TestResults"
$ReportFolderPath = $CodeCoverageFolderPath + "\CoverageReport"

$ReportGeneratorVersion = "4.6.1"
$ReportGeneratorPath = "$env:USERPROFILE\.nuget\packages\reportgenerator\$ReportGeneratorVersion\tools\netcoreapp3.0\ReportGenerator.dll"

Remove-Item "$AnalyzeFolderPath\*" -Force -Recurse -Confirm:$false
Remove-Item "$ReportFolderPath\*" -Force -Recurse -Confirm:$false

dotnet test --collect:"XPlat Code Coverage" --results-directory:$AnalyzeFolderPath

dotnet $ReportGeneratorPath -reports:$AnalyzeFolderPath\*\coverage.cobertura.xml -targetdir:$ReportFolderPath -reporttypes:HtmlInline_AzurePipelines_Dark

Start-Process -FilePath "$ReportFolderPath\index.html"