# Sprint 2 Code Quality Analysis

## Tool and Configuration

We used the .NET/Roslyn code analyzers during a Release build.
The project enables analyzers in `Directory.Build.props` with
`EnableNETAnalyzers` set to `true` and `AnalysisLevel` set to `latest`.

## Analysis Run

- Date: 2026.10.05
- Performed by: Ashley Zhang
- Environment: macOS
- Project: `GameProject/GameProject.csproj`
- Configuration: Release

Command run from the project root:

```sh
dotnet build GameProject/GameProject.csproj --configuration Release --no-incremental -p:RunAnalyzersDuringBuild=true -v:normal
```

## Results and Actions

The build succeeded with 0 warnings and 0 errors under the current
analyzer configuration.

No warning or error fixes were needed for this run, and no new
suppressions were added as part of this analysis.

The saved build output is available in
[the build log](code-analysis-build.txt).

This result covers the analyzed project build. It does not establish
that all gameplay behavior or visual output is correct.