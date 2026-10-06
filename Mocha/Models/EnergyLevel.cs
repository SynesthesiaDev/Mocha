// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel.DataAnnotations;

namespace Mocha.Models;

public enum EnergyLevel
{
    [Display(Name = "Low")]
    Low,

    [Display(Name = "Medium")]
    Medium,

    [Display(Name = "High")]
    High,

    [Display(Name = "Sensory Heavy")]
    SensoryHeavy
}
