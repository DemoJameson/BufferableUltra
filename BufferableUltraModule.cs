using System;
using System.Reflection;

namespace Celeste.Mod.BufferableUltra {
    public class BufferableUltraModule : EverestModule {
        public static BufferableUltraModule Instance { get; private set; }
        public static BufferableUltraSettings Settings => (BufferableUltraSettings) Instance._Settings;

        private static readonly FieldInfo OnGround = typeof(Player).GetField("onGround", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly Lazy<FieldInfo> tasManagerRunning =
            new Lazy<FieldInfo>(() => Type.GetType("TAS.Manager, CelesteTAS-EverestInterop")?.GetField("Running", BindingFlags.Static | BindingFlags.Public));

        public override Type SettingsType => typeof(BufferableUltraSettings);

        private bool climbJump = false;

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
            if (!Settings.Enabled ||
                Settings.GroundOnly && (bool) OnGround.GetValue(player) == false ||
                Settings.AvoidAffectingTAS && (bool?) tasManagerRunning.Value?.GetValue(null) == true
            ) {
                orig(player, particles, playSfx);
                return;
            }

            if ((!Settings.EnableClimbJumpFix || !climbJump) && player.DashDir.X != 0f && player.DashDir.Y > 0f && player.Speed.Y > 0f) {
                player.DashDir.X = Math.Sign(player.DashDir.X);
                player.DashDir.Y = 0f;
                player.Ducking = true;
                player.Speed.X *= 1.2f;
            }

            orig(player, particles, playSfx);
        }
    }
}