using System;

namespace M1.Core.Mail;

public delegate string ResendEmailDelegate(M1ExceptionAction ex, Tuple<string, string> message);
