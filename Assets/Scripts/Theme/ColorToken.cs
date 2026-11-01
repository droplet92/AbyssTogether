// Scenes and palettes store tokens as numbers: append new tokens, never renumber.
public enum ColorToken
{
    None = 0,

    SurfacePanel = 1,
    SurfaceRaised = 2,
    SurfaceLine = 3,
    SurfaceVeil = 4,
    SurfaceScrim = 5,

    TextPrimary = 10,
    TextSecondary = 11,
    TextDim = 12,
    TextOnLight = 13,
    TextShadow = 14,
    TextValueNeutral = 15,

    ControlFill = 20,
    ControlField = 21,
    ControlTrack = 22,
    ControlTrackFill = 23,
    ControlThumb = 24,
    ControlHover = 25,
    ControlPressed = 26,
    ControlPressedStrong = 27,
    ControlSelected = 28,
    ControlDisabled = 29,
    ControlDisabledField = 30,
    ControlDanger = 31,
    ControlArtHover = 32,

    Accent = 40,

    StatusHp = 50,
    StatusHpEmpty = 51,
    StatusDamage = 52,
    StatusHeal = 53,
    StatusPositive = 54,
    StatusWarning = 55,
    StatusBuffUp = 56,
    StatusBuffDown = 57,

    StateInactive = 60,
    StateBlocked = 61,

    SlotEmpty = 70,
    TintTitle = 71,
    TintGrave = 72,
}
