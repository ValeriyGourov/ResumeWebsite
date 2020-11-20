#Coverlet: https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md
#ReportGenerator: https://danielpalme.github.io/ReportGenerator/usage.html

$CodeCoverageFolderPath = "TestInfrastructure\CodeCoverage"
$AnalyzeFolderPath = $CodeCoverageFolderPath + "\TestResults"
$ReportFolderPath = $CodeCoverageFolderPath + "\CoverageReport"

$ReportGeneratorVersion = "4.8.1"
$ReportGeneratorPath = "$env:USERPROFILE\.nuget\packages\reportgenerator\$ReportGeneratorVersion\tools\netcoreapp3.0\ReportGenerator.dll"

if (Test-Path $AnalyzeFolderPath)
{
	Remove-Item "$AnalyzeFolderPath\*" -Force -Recurse -Confirm:$false
}
if (Test-Path $ReportFolderPath)
{
	Remove-Item "$ReportFolderPath\*" -Force -Recurse -Confirm:$false
}

dotnet test --collect:"XPlat Code Coverage" --results-directory:$AnalyzeFolderPath

dotnet $ReportGeneratorPath -reports:$AnalyzeFolderPath\*\coverage.cobertura.xml -targetdir:$ReportFolderPath -reporttypes:HtmlInline_AzurePipelines_Dark
