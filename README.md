# UnityOpener
CLI tool to automatically open a Unity project with the appropriate version.

## Usage
Run `UnityOpener.exe` from the command line, specifying all desired options, for example:
```
UnityOpener.exe --root "C:\Program Files\Unity" --depth 1
```

The tool was modelled after Unity Hub and assumes all Unity versions are installed in a single folder and follow a common naming strategy, i.e.
```
C:
└─ Program Files
   └─ Unity
      └─ 2017.3.1p1
      └─ 2017.4.15f
      └─ 2018.2.10f1
```
The root folder and name format can be specified although the contained version always has to follow Unity's default versioning for now.

### Options
```
-r, --root               Required. The root directory of all Unity installations.
-f, --format             (Default: {0}) The format Unity installation folder names follow. {0} will be replaced with the actual version number.
-p, --path               (Default: ) The path at which to search for a project.
-d, --depth              (Default: 1) The depth until which to search for Unity projects. A depth of 0 is unlimited.
-o, --versionoverride    (Default: ) Overrides the project's set Unity version and uses the specified one instead.
-v, --verbose            (Default: false) Enables log output.
--help                   Display this help screen.
--version                Display version information.
```