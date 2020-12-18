namespace Celeste.Mod.BufferableUltra {
    public class BufferableUltraSettings : EverestModuleSettings {
        public bool Enabled { get; set; } = true;
        public bool GroundOnly { get; set; } = true;
        public bool AvoidAffectingTAS { get; set; } = true;
    }
}