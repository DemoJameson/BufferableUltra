namespace Celeste.Mod.BufferableUltra {
    public class BufferableUltraSettings : EverestModuleSettings {
        public bool Enabled { get; set; } = true;
        public bool GroundOnly { get; set; } = true;
        public bool AvoidAffectingTAS { get; set; } = true;

        [SettingSubText("Enable \"Buffered climb jumps also keeping momentum\n" +
            "from the ultra momentum\" fix in v1.3.3.6 beta")]
        public bool EnableClimbJumpFix { get; set; } = true;
    }
}