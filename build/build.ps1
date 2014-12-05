properties { 
  $majorVersion = "1.0"
  $majorWithReleaseVersion = "1.0.1"
  $version = GetVersion $majorWithReleaseVersion
  $signAssemblies = $false
  $signKeyPath = "C:\key.snk"
  $treatWarningsAsErrors = $false
  
  $baseDir  = resolve-path ..
  $buildDir = "$baseDir\build"
  $sourceDir = "$baseDir\src"
  $toolsDir = "$baseDir\tools"
  $workingDir = "$baseDir\release"
  $builds = @(
      @{Name = "InterfaceBooster.Database.Core"; TestsName = "InterfaceBooster.Database.Test.Core"; TestsFunction = "NUnitTests"; Constants=""; FinalDir="Net45"; NuGetDir = "net45"; Framework="net-4.5"; Sign=$false; Unsafe=$true},
      @{Name = "InterfaceBooster.Database.Interfaces"; TestsName = ""; TestsFunction = ""; Constants=""; FinalDir="Net45"; NuGetDir = "net45"; Framework="net-4.5"; Sign=$false; Unsafe=$false}
  )
}

$framework = '4.0x64'

task default -depends Test

# Ensure a clean working directory
task Clean {
  Set-Location $baseDir
  
  if (Test-Path -path $workingDir)
  {
    Write-Output "Deleting Working Directory"
    
    del $workingDir -Recurse -Force
  }
  
  New-Item -Path $workingDir -ItemType Directory
}

# Build each solution, optionally signed
task Build -depends Clean { 
  Write-Host -ForegroundColor Green "Updating assembly version"
  Write-Host
  Update-AssemblyInfoFiles $sourceDir ($majorVersion + '.0.0') $version

  foreach ($build in $builds)
  {
    $name = $build.Name
    if ($name -ne $null)
    {
      $finalDir = $build.FinalDir
      $sign = ($build.Sign -and $signAssemblies)
      $unsafe = $build.Unsafe

      Write-Host -ForegroundColor Green "Building " $name
      Write-Host -ForegroundColor Green "Signed " $sign

      Write-Host
      Write-Host "Building"
      exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:DocumentationFile=bin\Release\$finalDir\$name.xml "/p:Platform=x64" /p:OutputPath=bin\Release\$finalDir\ /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$sign" "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=12.0" "/p:AllowUnsafeBlocks=$unsafe" (GetConstants $build.Constants $sign) "$sourceDir\$name\$name.csproj" | Out-Default } "Error building $name"
    }
  }
}

# Builds and copies the assemblies to a release directory
task Package -depends Build {
  foreach ($build in $builds)
  {
    $name = $build.Name
    $finalDir = $build.FinalDir
    
    robocopy "$sourceDir\$name\bin\Release\$finalDir" $workingDir *.dll *.pdb *.xml /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
  }
}

task Test -depends Deploy {
  foreach ($build in $builds)
  {
    if ($build.TestsFunction -ne $null)
    {
      & $build.TestsFunction $build
    }
  }
}

function Update-AssemblyInfoFiles ([string] $sourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber)
{
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
    $fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
    
    Get-ChildItem -Path $sourceDir -r -filter AssemblyInfo.cs | ForEach-Object {
        
        $filename = $_.Directory.ToString() + '\' + $_.Name
        Write-Host $filename
        $filename + ' -> ' + $version
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}

function GetConstants($constants, $includeSigned)
{
  $signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

  return "/p:DefineConstants=`"CODE_ANALYSIS;TRACE;$constants$signed`""
}

function GetVersion($majorVersion)
{
    $now = [DateTime]::Now
    
    $year = $now.Year - 2000
    $month = $now.Month
    $totalMonthsSince2000 = ($year * 12) + $month
    $day = $now.Day
    $minor = "{0}{1:00}" -f $totalMonthsSince2000, $day
    
    $hour = $now.Hour
    $minute = $now.Minute
    $revision = "{0:00}{1:00}" -f $hour, $minute
    
    return $majorVersion + "." + $minor
}
