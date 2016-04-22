# IES Beacon Plugin for Autodesk Revit

## Using the Plugin

To use the plugin on your own machine with Revit installed, you are going to need to include an AddIn file in your Revit AddIns folder and also change a line in this Addin File to point to the right DLL files. All of these files can be found in the ExtractBeacons folder in this repository.

1. Copy the XYZFamily.addin file from the ExtractBeacons folder into the Revit Addins Folder on your machine (should be something like C:\ProgramData\Autodesk\Revit\Addins\2016)
2. Change the <Assembly> file path in XYZFamily.addin to point to the location of the XYZFamily.dll file in the ExtractBeacons file (should look like <Assembly>C:\Users\Danny\Desktop\TTT\ExtractBeacons\XYZFamily.dll</Assembly>)

## Project Description from Jane

"Graphical tools built on the geometric model can:

1. Help the developer select the right type of beacons for each place,
2. Experiment with the placements and orientations of the selected beacons,
3. Visualize and assess the coverage provided by them,
4. Generate the coordinates and technical specifications of the beacons."

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Added some feature'`
4. Push to branch: `git push origin my-new-feature`
5. Submit a pull request :D


## Troubleshooting

* If Revit got a External Tool Failure - System.IO.FileLoadException
[Please follow this blog's instruction](https://github.com/OpenISDM/IES_revit_plugin)
