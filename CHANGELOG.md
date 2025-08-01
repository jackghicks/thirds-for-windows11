# Changelog

## [1.1.0](https://github.com/jackghicks/thirds-for-windows11/compare/v1.0.0...v1.1.0) (2025-07-29)


### Features

* reset version in file to 0.0.0.0 to indicate development builds ([11067bd](https://github.com/jackghicks/thirds-for-windows11/commit/11067bd4a55d3e5ba0d23088a54dcbe7e11670d0))

## 1.0.0 (2025-07-29)


### Features

* added better tray menu with start with windows option ([64ee1ad](https://github.com/jackghicks/thirds-for-windows11/commit/64ee1ad68564fcff29e31209aa25bfeb55e49076))
* added icon ([05baf34](https://github.com/jackghicks/thirds-for-windows11/commit/05baf345ee2fa9e0727fd9bde4fe1612f971dd03))
* added icon to application incl tray icon ([dfb0496](https://github.com/jackghicks/thirds-for-windows11/commit/dfb04965504043ec290c2355ace5d83bec904020))
* added simplistic mechanism for detecting processible windows, and counting them to determine which key the thirds layout should be on ([0273fab](https://github.com/jackghicks/thirds-for-windows11/commit/0273fab17442b899daeee7524079b02da3bfad11))
* added versioning ([e43d95c](https://github.com/jackghicks/thirds-for-windows11/commit/e43d95c63db04bd8cef8f4558ff5d5f09254673d))
* first pass implementation ([5e34876](https://github.com/jackghicks/thirds-for-windows11/commit/5e34876963a72014d96c007af002e9dc89c0f0d5))
* implemented CI as a release-please workflow for versioning and changelog handling ([9aeb38b](https://github.com/jackghicks/thirds-for-windows11/commit/9aeb38b30737aa3c6e6d2c8eaec76630814e6455))
* inspired by the PowerToys FancyZones code, filter out tool windows, non visibles, improperly styled, etc ([803baa5](https://github.com/jackghicks/thirds-for-windows11/commit/803baa55407ef8b9a3d8ac63d92e8d944283558f))
* more filtering on what makes a processable window ([ccfa28f](https://github.com/jackghicks/thirds-for-windows11/commit/ccfa28fac8c5b9712ab1846ddaf94fe87737f2ee))
* re-order checks to dismiss most common first ([4c17b2a](https://github.com/jackghicks/thirds-for-windows11/commit/4c17b2a20994255fab9e52f4b149a7fbe4ae2af0))
* use a composite action so we can move the "if" onto the invocation of that ([866d2a3](https://github.com/jackghicks/thirds-for-windows11/commit/866d2a3a5b640b49b3f37e04a1b027f3cf7a6cd3))


### Bug Fixes

* adds CI token ([74c6d34](https://github.com/jackghicks/thirds-for-windows11/commit/74c6d34a70c3b1b7fa3ad8ea17ce077fb6c28ae8))
* compiler warning for exhaustice switch case ([0254b4f](https://github.com/jackghicks/thirds-for-windows11/commit/0254b4fbe10417f9790ef134a021337f87873766))
* don't snap for MSTaskSwWClass, clarify loggers ([a29cea7](https://github.com/jackghicks/thirds-for-windows11/commit/a29cea72018fa0af3e1f68e323f9d71b169274d8))
* increase Sleeps slightly to be more lenient in case of performance issues ([39341c7](https://github.com/jackghicks/thirds-for-windows11/commit/39341c716523777126b0ac76e3978cdaba6449fc))
* possible ordering issue ([06c2580](https://github.com/jackghicks/thirds-for-windows11/commit/06c25806f6ef59a9df4c60c28fc2b406b5e42fd7))
