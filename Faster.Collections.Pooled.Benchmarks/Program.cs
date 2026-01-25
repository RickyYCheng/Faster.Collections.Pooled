// Copyright (c) 2026, RickyYC and Contributors. All rights reserved.
// Distributed under the MIT Software License, Version 1.0.

using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
