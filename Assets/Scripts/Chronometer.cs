public static class Chronometer {
    private const int SecondsPerHour = 3600;
    private const int SecondsPerMinute = 60;
    private const int MillisecondsPerSecond = 100;
    
    private static float _elapsedTime;
    
    public static int Minutes => (int) (_elapsedTime % SecondsPerHour / SecondsPerMinute);
    public static int Seconds => (int) (_elapsedTime % SecondsPerMinute);
    public static int Milliseconds => (int) (_elapsedTime * MillisecondsPerSecond % MillisecondsPerSecond);
    
    public static void Update(float deltaTime) {
        _elapsedTime += deltaTime;
    }
    
    public static void Reset() {
        _elapsedTime = 0;
    }
}