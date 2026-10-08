// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel.DataAnnotations;

namespace Mocha.Models;

public enum MochaTaskStatus
{
    [Display(Name = "Todo")]
    Todo,
    [Display(Name = "In Progress")]
    InProgress,
    [Display(Name = "Waiting")]
    Waiting,
    [Display(Name = "Done")]
    Done,
    [Display(Name = "Dropped")]
    Dropped
}
