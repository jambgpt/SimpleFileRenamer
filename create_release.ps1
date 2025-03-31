# Packaging script for SimpleFileRenamer
# This script creates a release build and packages it for distribution

$ErrorActionPreference = "Stop"
$releaseDir = "release"
$version = "1.0.0"
$appName = "SimpleFileRenamer"

# Get version from project file
try {
    $projectFile = Join-Path -Path $appName -ChildPath "$appName.csproj"
    $projectContent = Get-Content $projectFile -Raw
    $versionMatch = [regex]::Match($projectContent, '<Version>(.*?)</Version>')
    if ($versionMatch.Success) {
        $version = $versionMatch.Groups[1].Value
        Write-Host "Detected version: $version"
    }
}
catch {
    Write-Warning "Could not detect version from project file. Using default version $version"
}

# Create output directories
$outputDir = Join-Path -Path $releaseDir -ChildPath "SimpleFileRenamer-v$version"
$portableDir = Join-Path -Path $outputDir -ChildPath "Portable"
$setupDir = Join-Path -Path $outputDir -ChildPath "Setup"

# Clean up
Write-Host "Cleaning up previous build..."
Remove-Item -Path $outputDir -Recurse -Force -ErrorAction SilentlyContinue
New-Item -Path $outputDir -ItemType Directory -Force | Out-Null
New-Item -Path $portableDir -ItemType Directory -Force | Out-Null
New-Item -Path $setupDir -ItemType Directory -Force | Out-Null

# Build the project
Write-Host "Building SimpleFileRenamer..."
dotnet publish SimpleFileRenamer/SimpleFileRenamer.csproj -c Release -o "$portableDir/app" --self-contained true -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

# Copy the shell extension to the portable output
Write-Host "Building Shell Extension..."
dotnet build SimpleFileRenamer.ShellExtension/SimpleFileRenamer.ShellExtension.csproj -c Release -o "$portableDir/shell"

# Create portable package readme
$readmeContent = @"
# Simple File Renamer v$version - Portable Edition

## Installation
1. Extract this zip file to a location of your choice
2. Run the SimpleFileRenamer.exe application from the app folder
3. (Optional) To install the Windows Explorer integration:
   - Right-click on install_shell_extension.bat and select "Run as administrator"

## Uninstallation
1. (If shell extension was installed) Right-click on uninstall_shell_extension.bat and select "Run as administrator"
2. Delete the extracted folder

## License
This software is provided under the MIT License.
"@

Set-Content -Path "$portableDir/README.txt" -Value $readmeContent

# Create install bat file for shell extension
$installBatContent = @"
@echo off
echo Installing Windows Explorer integration...
cd shell
regsvr32 SimpleFileRenamer.ShellExtension.dll
echo Done!
pause
"@

Set-Content -Path "$portableDir/install_shell_extension.bat" -Value $installBatContent -Encoding ASCII

# Create uninstall bat file for shell extension
$uninstallBatContent = @"
@echo off
echo Uninstalling Windows Explorer integration...
cd shell
regsvr32 /u SimpleFileRenamer.ShellExtension.dll
echo Done!
pause
"@

Set-Content -Path "$portableDir/uninstall_shell_extension.bat" -Value $uninstallBatContent -Encoding ASCII

# Create zip of portable edition
Write-Host "Creating portable zip package..."
Compress-Archive -Path "$portableDir/*" -DestinationPath "$outputDir/SimpleFileRenamer-v$version-Portable.zip" -Force

# Set up for installer creation (in future)
Write-Host "Creating installer directory structure..."
Copy-Item -Path "$portableDir/*" -Destination $setupDir -Recurse

# Output success message
Write-Host "Packaging complete!" -ForegroundColor Green
Write-Host "Portable edition: $outputDir/SimpleFileRenamer-v$version-Portable.zip"
Write-Host "Setup source files: $setupDir"