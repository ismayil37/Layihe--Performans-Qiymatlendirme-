using System;

namespace EmployeePerformanceSystem.Services
{
    public class PerformanceService
    {
        // 1. Ümumi balın hesablanması
        public double CalculateFinalScore(double taskScore, double qualityScore, double attendanceScore)
        {
            // Task(40%) + Quality(10 ballıq sistemdən 100-ə keçidlə 40%) + Attendance(20%)
            double final = (taskScore * 0.4) + (qualityScore * 10 * 0.4) + (attendanceScore * 0.2);
            return Math.Round(final, 1);
        }

        // 2. Bal əsasında rəy və statusun müəyyən edilməsi
        public (string Status, string Comment) GetPerformanceFeedback(double finalScore, double attendance)
        {
            if (attendance < 70)
                return ("Risk", "Davamiyyət çox aşağıdır. İntizam tədbirləri görülməlidir.");

            if (finalScore >= 90)
                return ("A+", "Mükəmməl nəticə! İşçi yüksəliş (promosiya) üçün namizəddir.");

            if (finalScore >= 75)
                return ("B", "Yaxşı performans. Mövcud temp qorunmalıdır.");

            if (finalScore >= 50)
                return ("C", "Kafi nəticə. Müəyyən sahələrdə inkişaf lazımdır.");

            return ("Zəif", "Performans kritik səviyyədədir. Təlimlərdə iştirak vacibdir.");
        }
    }
}