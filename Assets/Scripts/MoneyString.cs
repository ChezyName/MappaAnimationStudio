public class MoneyString
{
        public static string MoneyToString(float Cash)
        {
                if (Cash >= 1000000000000) return "$" + (Cash / 1000000000000).ToString("N0") + "T";
                if (Cash >= 1000000000) return "$" + (Cash / 1000000000).ToString("N0") + "B";
                if (Cash >= 1000000) return "$" + (Cash / 1000000).ToString("N0") + "M";
                if (Cash >= 1000) return "$" + (Cash / 1000).ToString("N0") + "K";
                return "$" + Cash.ToString("0.00");
        }
}