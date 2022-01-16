using System;

namespace Conway_Reborn
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Conway())
                game.Run();
        }
    }
}
