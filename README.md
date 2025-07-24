# XR Tools

A Unity project containing tools and utilities for XR (Extended Reality) development, featuring a scene validation editor script to help ensure your XR scenes are properly configured.

## Features

### XR Scene Validator
An editor window tool that validates your XR scenes for common configuration issues and best practices.

**Access:** Window → XR Tools → Scene Validator

#### What it checks:
- ✅ **XR Rig Detection** - Identifies XR-related components and rigs in the scene
- ✅ **Main Camera** - Ensures a camera is properly tagged as 'MainCamera'
- ✅ **Event System** - Verifies the presence of an EventSystem for UI interactions
- ✅ **Input System** - Checks if the new Input System package is properly configured
- ✅ **Recommended Layers** - Validates the presence of XR-specific layers:
  - UI
  - XR
  - Hands
  - Interactable

## Project Structure

```
Assets/
├── Scripts/
│   └── Editor/
│       └── XRSceneValidator/
│           └── XRSceneValidator.cs    # Main editor script
├── Scenes/
│   └── SampleScene.unity              # Example scene
├── Settings/                          # Project settings
├── TutorialInfo/                      # Tutorial assets
└── InputSystem_Actions.inputactions   # Input System configuration
```

## Getting Started

1. **Open the project** in Unity 2022.3 LTS or later
2. **Open the Scene Validator** via Window → XR Tools → Scene Validator
3. **Run validation** by clicking "Run Scene Validation" in the editor window
4. **Review the report** to identify any missing components or configuration issues

## Requirements

- Unity 2022.3 LTS or later
- Universal Render Pipeline (URP) template
- Input System package (recommended)

## Usage

### Scene Validation
1. Open any XR scene in your project
2. Navigate to **Window → XR Tools → Scene Validator**
3. Click **"Run Scene Validation"**
4. Review the generated report for any issues
5. Address any warnings or errors before building your XR application

### Recommended Setup
For optimal XR development, ensure your scene includes:
- An XR Rig (XR Origin or similar)
- A properly tagged Main Camera
- An EventSystem for UI interactions
- Appropriate layers for XR interactions
- Input System package configuration

## Development

This project is set up as a Unity template for XR development. The editor script provides immediate feedback on scene configuration, helping developers maintain consistent XR project standards.

## License

This project is licensed under the terms specified in the LICENSE file.

## Contributing

Feel free to extend the XR Scene Validator with additional checks or create new XR development tools within this project structure.

