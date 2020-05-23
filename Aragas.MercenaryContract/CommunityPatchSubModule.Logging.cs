using System;
using System.Diagnostics;

using TaleWorlds.Engine;

namespace CommunityPatch
{
    // I don't see any reason to use the whole thing right now
    // https://github.com/Tyler-IN/MnB2-Bannerlord-CommunityPatch/blob/master/src/CommunityPatch/CommunityPatchSubModule.Logging.cs
    public partial class CommunityPatchSubModule
    {
        [Conditional("TRACE")]
        public static void Error(Exception ex, string msg = null)
        {
            if (msg != null)
                Error(msg);

            var st = new StackTrace(ex, true);
            var f = st.GetFrame(0);
            var exMsg = $"{f.GetFileName()}:{f.GetFileLineNumber()}:{f.GetFileColumnNumber()}: {ex.GetType().Name}: {ex.Message}";

            MBDebug.ConsolePrint(exMsg);
            MBDebug.ConsolePrint(ex.StackTrace);
            Debugger.Log(3, "CommunityPatch", exMsg + '\n');
            Debugger.Log(3, "CommunityPatch", ex.StackTrace + '\n');
        }

        [Conditional("TRACE")]
        public static void Error(Exception ex, FormattableString msg)
            => Error(ex, FormattableString.Invariant(msg));

        [Conditional("TRACE")]
        public static void Error(FormattableString msg)
            => Error(FormattableString.Invariant(msg));

        [Conditional("TRACE")]
        public static void Error(string msg = null)
        {
            if (msg == null)
                return;

            Debugger.Log(3, "CommunityPatch", msg);
        }

        [Conditional("DEBUG")]
        public static void Print(FormattableString msg)
            => Print(FormattableString.Invariant(msg));

        [Conditional("DEBUG")]
        public static void Print(string msg)
            => Debugger.Log(0, "CommunityPatch", msg + '\n');

    }
}
