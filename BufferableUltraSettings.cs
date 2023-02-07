namespace Celeste.Mod.BufferableUltra {
    public class BufferableUltraSettings : EverestModuleSettings {
        public bool Enabled { get; set; } = true;
        public bool GroundOnly { get; set; } = false;
        public bool AvoidAffectingTAS { get; set; } = false;

        [SettingSubText("Enable \"Buffered climb jumps also keeping momentum\n" +
            "from the ultra momentum\" fix in v1.3.3.6 beta")]
        public bool EnableClimbJumpFix { get; set; } = false;
    }
}