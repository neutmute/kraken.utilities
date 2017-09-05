param(
    [string]$packageVersion = $null,
    [string]$configuration = "Release"
)

. ".\common.ps1"

$solutionName = "kraken.utilities"
$sourceUrl = "https://github.com/neutmute/kraken.utilities"

function init {
    # Initialization
    $global:rootFolder = Split-Path -parent $script:MyInvocation.MyCommand.Path
    $global:rootFolder = Join-Path $rootFolder .
    $global:packagesFolder = Join-Path $rootFolder packages
    $global:outputFolder = Join-Path $rootFolder _output
    $global:msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

    _WriteOut -ForegroundColor $ColorScheme.Banner "-= $solutionName Build =-"
    _WriteConfig "rootFolder" $rootFolder
}

function restorePackages{
    _WriteOut -ForegroundColor $ColorScheme.Banner "nuget, gitlink restore"
    
    #New-Item -Force -ItemType directory -Path $packagesFolder
    #_DownloadNuget $packagesFolder
    dotnet restore
    #nuget install gitlink -SolutionDir "$rootFolder" -ExcludeVersion
}

function nugetPack{
    _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget pack"
    
    New-Item -Force -ItemType directory -Path $outputFolder

    if(!(Test-Path Env:\nuget )){
        $env:nuget = nuget
    }
    if(!(Test-Path Env:\PackageVersion )){
        $env:PackageVersion = "1.0.0.0"
    }
    
    $packableProjects = @("Kraken.Core", "Kraken.Tests")

   $packableProjects | foreach {
       nuget pack "$rootFolder\Source\$_\$_.csproj" -o $outputFolder -IncludeReferencedProjects -p Configuration=$configuration -Version $env:PackageVersion
   }    
}

function nugetPublish{

    if(Test-Path Env:\nugetapikey ){
        _WriteOut -ForegroundColor $ColorScheme.Banner "Nuget publish..."
        &nuget push $outputFolder\* -ApiKey "$env:nugetapikey" -source https://www.nuget.org
    }
    else{
        _WriteOut -ForegroundColor Yellow "nugetapikey environment variable not detected. Skipping nuget publish"
    }
}

function buildSolution{

    _WriteOut -ForegroundColor $ColorScheme.Banner "Build Solution"
    & dotnet build "$rootFolder\$solutionName.sln" /p:Configuration=$configuration /verbosity:minimal

    #&"$rootFolder\packages\gitlink\lib\net45\GitLink.exe" $rootFolder -u $sourceUrl
}

function executeTests{

    Write-Host "Execute Tests"

    $testResultformat = ""
    $nunitConsole = "$rootFolder\packages\NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe"

    if(Test-Path Env:\APPVEYOR){
        $testResultformat = ";format=AppVeyor"
        $nunitConsole = "nunit3-console"
    }
	
   #$testProjects = @("Kraken.Core.Tests","Kraken.Core.Windows.Tests", "Kraken.Net.Tests", "Kraken.Tests.Tests")
   #
   #$testProjectsArgs = $testProjects | Foreach {".\Source\_Tests\$_\bin\Release\$_.dll"}
   #
	#& $nunitConsole $testProjectsArgs --result=$outputFolder\Kraken.Tests.xml$testResultformat
    
        & $nunitConsole .\Source\_Tests\Kraken.Core.Tests\bin\Release\Kraken.Core.Tests.dll `
                    .\Source\_Tests\Kraken.Core.Windows.Tests\bin\Release\Kraken.Core.Windows.Tests.dll `
                    .\Source\_Tests\Kraken.Net.Tests\bin\Release\Kraken.Net.Tests.dll `
                    .\Source\_Tests\Kraken.Tests.Tests\bin\Release\Kraken.Tests.Tests.dll `
                    --result=$outputFolder\Kraken.Tests.xml$testResultformat

	        
	checkExitCode
}

init

restorePackages

buildSolution

executeTests

nugetPack

nugetPublish

Write-Host "Build $env:PackageVersion complete"