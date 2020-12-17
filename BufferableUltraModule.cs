using System;
using System.Reflection;

namespace Celeste.Mod.BufferableUltra {
    public class BufferableUltraModule : EverestModule {
        public static BufferableUltraModule Instance { get; private set; }
        public static BufferableUltraSettings Settings => (BufferableUltraSettings) Instance._Settings;

        private static readonly FieldInfo OnGround = typeof(Player).GetField("onGround", BindingFlags.Instance | BindingFlags.NonPublic);
        public override Type SettingsType => typeof(BufferableUltraSettings);

        private bool climbJump = false;
        private readonly Version stableVersion = new Version(1, 3, 1, 2);

        public BufferableUltraModule() {
            Instance = this;
        }

        public override void Load() {
            On.Celeste.Player.ClimbJump += PlayerOnClimbJump;
            On.Celeste.Player.Jump += PlayerOnJump;
        }

        public override void Unload() {
            On.Celeste.Player.ClimbJump -= PlayerOnClimbJump;
            On.Celeste.Player.Jump -= PlayerOnJump;
        }

        private void PlayerOnClimbJump(On.Celeste.Player.orig_ClimbJump orig, Player self) {
            climbJump = true;
            orig(self);
            climbJump = false;
        }

        private void PlayerOnJump(On.Celeste.Player.orig_Jump orig, Player player, bool particles, bool playSfx) {
            if (Celeste.Instance.Version == stableVersion && Settings.Enabled && !climbJump && player.DashDir.X != 0f && player.DashDir.Y > 0f &&
                player.Speed.Y > 0f && (bool) OnGround.GetValue(player)) {
                player.DashDir.X = Math.Sign(player.DashDir.X);
                player.DashDir.Y = 0f;
                player.Ducking = true;
                player.Speed.X *= 1.2f;
            }

            orig(player, particles, playSfx);
        }
    }
}