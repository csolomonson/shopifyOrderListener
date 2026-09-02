using System;
using System.ComponentModel;

namespace M1.Core;

public delegate bool ProcessCheckValidation(IServiceProvider provider, ErrorItemsList errors, CancelEventArgs arg);
