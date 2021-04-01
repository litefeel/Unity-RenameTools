# Rename Tools


[![](https://img.shields.io/github/release/litefeel/Unity-RenameTools.svg?label=latest%20version)](https://github.com/litefeel/Unity-RenameTools/releases)
[![](https://img.shields.io/github/license/litefeel/Unity-RenameTools.svg)](https://github.com/litefeel/Unity-RenameTools/blob/master/LICENSE.md)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/litefeel)

[Rename Tools][RenameTools] is just perfect Unity asset plugin to rename game object.  

## Feature list

- Free
- Rename game object
- No runtime resources required
- No scripting required

## Install

#### Using npm (Ease upgrade in Package Manager UI)**Recommend**

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
``` js
{
  "scopedRegistries": [
    {
      "name": "My Registry",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.litefeel"
      ]
    }
  ],
  "dependencies": {
    "com.litefeel.renametools": "2.0.0",
    ...
  }
}
```

#### Using git

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
``` js
{
  "dependencies": {
    "com.litefeel.renametools": "https://github.com/litefeel/Unity-RenameTools.git#2.0.0",
    ...
  }
}
```


## How to use?

1. Select `Edit > Preferences… > Rename Tools` from the menu
2. Input the start number, like 1
3. Select multiple game objects in the hierarchy
4. Press `Shift + F2`
5. Click Rename button


## Support

- Create issues by [issues][issues] page
- Send email to me: <litefeel@gmail.com>


[RenameTools]: https://github.com/litefeel/Unity-RenameTools (RenameTools)
[issues]: https://github.com/litefeel/Unity-RenameTools/issues (RenameTools issues)
