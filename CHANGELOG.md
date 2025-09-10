# Changelog

## [1.3.0](https://github.com/jackghicks/thirds-for-windows11/compare/v1.2.0...v1.3.0) (2025-08-29)


### Features

* added snap overlay visual feedback ([#9](https://github.com/jackghicks/thirds-for-windows11/issues/9)) ([8613abb](https://github.com/jackghicks/thirds-for-windows11/commit/8613abb2e544d7dd4a2928f90bd8a6ef772b9178))

## [1.2.0](https://github.com/jackghicks/thirds-for-windows11/compare/v1.1.4...v1.2.0) (2025-08-27)


### Features

* clean up file names for self-contained vs nonsc builds ([f202b60](https://github.com/jackghicks/thirds-for-windows11/commit/f202b60a7a012f542be1a629afdcaf3c5f44f46f))

## [1.1.4](https://github.com/jackghicks/thirds-for-windows11/compare/v1.1.3...v1.1.4) (2025-08-27)


### Bug Fixes

* each file in a github release must have a different file name, so include the matrix definition ([4f36d8e](https://github.com/jackghicks/thirds-for-windows11/commit/4f36d8e8f1f0cb618963cc226ae0328521ca33e3))

## [1.1.3](https://github.com/jackghicks/thirds-for-windows11/compare/v1.1.2...v1.1.3) (2025-08-27)


### Bug Fixes

* executable file name does not have the version number in it ([cd816b9](https://github.com/jackghicks/thirds-for-windows11/commit/cd816b944756241295da38b0e3eed7078b2e5307))

## [1.1.2](https://github.com/jackghicks/thirds-for-windows11/compare/v1.1.1...v1.1.2) (2025-08-27)


### Bug Fixes

* download artifact must happen in tandem with build, not in opposition to it ([0123072](https://github.com/jackghicks/thirds-for-windows11/commit/0123072abbd11f08fc8bfcbed50709033e5a5000))

## [1.1.1](https://github.com/jackghicks/thirds-for-windows11/compare/v1.1.0...v1.1.1) (2025-08-27)


### Bug Fixes

* artifact usage for releases ([5f29d2e](https://github.com/jackghicks/thirds-for-windows11/commit/5f29d2eeb35410183dd52c6b4b63c17d319a5e99))
* remove slash character causing doubling in the output ([d9448b9](https://github.com/jackghicks/thirds-for-windows11/commit/d9448b9e2e71dcbb083f45419aed6cae0e4b13c4))

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
