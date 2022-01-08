using System;

using Dalamud.Game;
using Dalamud.Logging;

namespace XIVComboExpandedPlugin
{
    /// <summary>
    /// Plugin address resolver.
    /// </summary>
    internal class PluginAddressResolver : BaseAddressResolver
    {
        /// <summary>
        /// Gets the address of the member ComboTimer.
        /// </summary>
        public IntPtr ComboTimer { get; private set; }

        /// <summary>
        /// Gets the address of the member LastComboMove.
        /// </summary>
        public IntPtr LastComboMove => this.ComboTimer + 0x4;

        /// <summary>
        /// Gets the address of fpGetAdjustedActionId.
        /// </summary>
        public IntPtr GetAdjustedActionId { get; private set; }

        /// <summary>
        /// Gets the address of fpIsIconReplacable.
        /// </summary>
        public IntPtr IsActionIdReplaceable { get; private set; }

        /// <inheritdoc/>
        protected override void Setup64Bit(SigScanner scanner)
        {
            this.ComboTimer = scanner.GetStaticAddressFromSig("48 89 2D ?? ?? ?? ?? 85 C0 74 0F");

            this.GetAdjustedActionId = scanner.ScanText("E8 ?? ?? ?? ?? 8B F8 3B DF");  // Client::Game::ActionManager.GetAdjustedActionId

            this.IsActionIdReplaceable = scanner.ScanText("81 F9 ?? ?? ?? ?? 7F 35");

            PluginLog.Verbose("===== X I V C O M B O =====");
            PluginLog.Verbose($"{nameof(this.GetAdjustedActionId)}   0x{this.GetAdjustedActionId:X}");
            PluginLog.Verbose($"{nameof(this.IsActionIdReplaceable)} 0x{this.IsActionIdReplaceable:X}");
            PluginLog.Verbose($"{nameof(this.ComboTimer)}            0x{this.ComboTimer:X}");
            PluginLog.Verbose($"{nameof(this.LastComboMove)}         0x{this.LastComboMove:X}");
        }
    }
}
