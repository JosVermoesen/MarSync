# Fix for DAO COM Reference Warning

## Problem
The build warning `MSB3284: Cannot get the file path for type library "00025e01-0000-0000-c000-000000000046" version 4.0` occurs because MSBuild cannot access the OLE registry when `EmbedInteropTypes` is set to `True` for the DAO COM reference.

## Solution
To fix this warning, follow these steps:

### Option 1: Manual Edit (Recommended)
1. **Close the solution** in Visual Studio
2. Open `MarSync\MarSync.csproj` in a text editor
3. Find the DAO COM reference (around line 330-340):
   ```xml
   <COMReference Include="DAO">
	 <Guid>{00025E01-0000-0000-C000-000000000046}</Guid>
	 <VersionMajor>4</VersionMajor>
	 <VersionMinor>0</VersionMinor>
	 <Lcid>0</Lcid>
	 <WrapperTool>tlbimp</WrapperTool>
	 <Isolated>False</Isolated>
	 <EmbedInteropTypes>True</EmbedInteropTypes>
   </COMReference>
   ```
4. Change `<EmbedInteropTypes>True</EmbedInteropTypes>` to `<EmbedInteropTypes>False</EmbedInteropTypes>`
5. Save the file
6. Reopen the solution in Visual Studio
7. Rebuild the project

### Option 2: PowerShell Script (When Solution is Closed)
Run this PowerShell command when Visual Studio is closed:

```powershell
$projectFile = "C:\Users\Jos\source\repos\mar\vss\Peppol\MarSync\MarSync\MarSync.csproj"
(Get-Content $projectFile) -replace '<EmbedInteropTypes>True</EmbedInteropTypes>', '<EmbedInteropTypes>False</EmbedInteropTypes>' | Set-Content $projectFile
```

### Why This Works
When `EmbedInteropTypes` is set to `False`, MSBuild doesn't need to access the OLE registry to embed the type information, which eliminates the warning.

### Alternative (If DAO is Not Used)
If your project doesn't actively use DAO, you can remove the entire COM reference block for DAO from the .csproj file.

## Impact
- This change should not affect your application's functionality
- The warning will be eliminated
- Your project will still have access to DAO functionality
