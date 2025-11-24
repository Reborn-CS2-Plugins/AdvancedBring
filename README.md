# AdvancedBring

A CounterStrikeSharp plugin for CS2 that allows administrators to teleport players to their aim position using ray tracing.

## Installation

1. Install [CS2TraceRay](https://github.com/schwarper/CS2TraceRay) on your server
2. Download the latest release from the [Releases](https://github.com/btnrv/AdvancedBring/releases) page
3. Extract and drop files into `addons/counterstrikesharp/plugins/`
4. Restart your CS2 server
5. Configure permissions and commands from `addons/counterstrikesharp/configs/plugins/AdvancedBring`

## Features

- **Precise teleportation** using ray tracing to detect aim position
- **Team-specific commands** for teleporting Terrorists or Counter-Terrorists
- **Permission-based access control** using configurable permissions

### Commands

- `css_agel` / `css_aimegel` - Teleport targeted players to your aim position
- `css_agelt` / `css_aimegelt` - Teleport all Terrorists to your aim position
- `css_agelct` / `css_aimegelct` - Teleport all Counter-Terrorists to your aim position

### Permissions

Requires `@css/slay` permission by default (configurable)

*Supported languages: TR*

## License

This project is licensed under the **Creative Commons Attribution-NonCommercial 4.0 International License (CC BY-NC 4.0)**.

You are free to:
- Share, copy and redistribute the material in any medium or format for **non-commercial purposes**.

Under the following terms:
- **Attribution**: You must give appropriate credit, provide a link to the license, and indicate if changes were made. You may do so in any reasonable manner, but not in any way that suggests the licensor endorses you or your use.
- **NonCommercial**: You may not use the material for commercial purposes (e.g., selling or monetizing the software).

For full details, please see the [CC BY-NC 4.0 License](https://creativecommons.org/licenses/by-nc/4.0/legalcode).
