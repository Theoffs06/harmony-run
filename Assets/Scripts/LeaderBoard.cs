public static class LeaderBoard {
    public static string Entries = "";
    
    public static void NewEntry(string playerName1, string playerName2) {
        Entries = $"{Entries}\n{Chronometer.Minutes:00}:{Chronometer.Seconds:00}:{Chronometer.Milliseconds:00} {playerName1} | {playerName2}";
    }
}