# MarshmallowDamageHandler
### A plugin and independent API for SCP-SL.
Plugin:
- Allows you to force marshmallow damage to respect Friendly Fire.
- Utilizes the included API.
- Comes in both a Northwood Plugin API variant and an EXILED variant.

API:
- Adds a new damage handler for MarshmallowDamage
- Make sure to init the api first. (Call MarshmallowDamageHandler.API.InitDependencies())
  - No need to worry about calling this more than once. It will only initialize the first time.
- Dependencies are embeded in already.
