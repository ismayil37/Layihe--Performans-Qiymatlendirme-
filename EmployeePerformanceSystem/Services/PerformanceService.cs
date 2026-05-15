using System;

namespace EmployeePerformanceSystem.Services
{
    public class PerformanceService
    {
        
        public double CalculateFinalScore(double taskScore, double qualityScore, double attendanceScore)
        {
            
            double final = (taskScore * 0.4) + (qualityScore * 10 * 0.4) + (attendanceScore * 0.2);
            return Math.Round(final, 1);
        }

        
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
