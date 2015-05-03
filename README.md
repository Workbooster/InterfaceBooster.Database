Interface Booster: Synery Database
===================================

This repository contains the source code of the small but efficient Synery Database written in C# .NET

## What is the Synery Database?

The Synery Database is used in Interface Booster for fast and efficient working with medium-size data (perfect for 1MB ~ 2GB) in a single-user environment. It was developed to reach an outstanding performance in data serialization. But for that reason the database doesn't contain any expensive data checks. This means that data inconsistency is possible if the database isn't used correctly or if something goes wrong while writing the data to the disk.

## What do I need to use the Database?

* Microsoft .NET Framework 4.5
* A development environment (e.g. MSBuild, Visual Studio, MonoDevelop)

## Content

Directory | Description
----------| -------------
/src | the source code (Visual Studio Solution and Projects)

## License

This project is licensed under the terms of the GNU Lesser General Public License (LGPL). See LICENSE for more information.

The code in src/InterfaceBooster.Database.Core/Storage/PrimitiveSerializer.cs is an adaption of the the original "NetSerializer.Primitives"-class by Tomi Valkeinen (https://github.com/tomba/netserializer). For that reason this code is licensed under the terms of Mozilla Public License 2.0.

## Contact

Workbooster GmbH<br/>
Obermuelistrasse 85<br/>
8320 Fehraltorf (Switzerland)<br/>

Web: www.workbooster.ch<br/>
E-Mail: info@workbooster.ch<br/>
Phone: +41 (0)44 515 48 80<br/>